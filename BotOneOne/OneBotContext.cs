using BotOneOne.Connectivity;
using BotOneOne.Connectivity.Models;
using Newtonsoft.Json;

namespace BotOneOne;

public class OneBotContext
{
    private readonly IConnectionSource _connectionSource;
    private readonly OneBotContextOptions _options;
    private readonly Dictionary<string, TaskCompletionSource<ActionResponseDto>> _pendingRequests = [];

    private CancellationTokenSource _cancellationTokenSource = new();
    private Task? _workerTask;

    public OneBotContext(IConnectionSource connectionSource, OneBotContextOptions? options = null)
    {
        _connectionSource = connectionSource;
        _options = options ?? OneBotContextOptions.Default;
    }

    public void Open()
    {
        if (_workerTask != null)
        {
            return;
        }

        _workerTask = RxWorker(_cancellationTokenSource.Token);
    }

    public void Close()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        _workerTask = null;
    }

    private void HandleActionResponse(string packet)
    {
        var actionResponse = JsonConvert.DeserializeObject<ActionResponseDto>(packet);
        if (actionResponse?.Echo == null)
        {
            return;
        }

        // handle packet by echo
        if (_pendingRequests.TryGetValue(actionResponse.Echo, out var source))
        {
            source.TrySetResult(actionResponse!);
        }
    }

    private async Task RxWorker(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var packet = await _connectionSource.ReceiveTextAsync(cancellationToken);
                var baseResponse = JsonConvert.DeserializeObject<BaseResponseDto>(packet);

                if (baseResponse == null)
                {
                    continue;
                }

                if (baseResponse.IsEventPacket)
                {
                    // TODO
                }
                else
                {
                    HandleActionResponse(packet);
                }
            }
            catch
            {
                // TODO
            }
        }
    }

    private async ValueTask<ActionResponseDto> InvokeActionInternal(ActionRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        var packetEcho = Guid.NewGuid().ToString();
        requestDto.Echo = packetEcho;

        // prepare for receive
        var taskCompletionSource = new TaskCompletionSource<ActionResponseDto>();
        _pendingRequests.Add(packetEcho, taskCompletionSource);
        cancellationToken.Register(() => taskCompletionSource.TrySetCanceled(), false);

        // send action packet
        await _connectionSource.SendTextAsync(
            JsonConvert.SerializeObject(requestDto), cancellationToken);

        try
        {
            // receive response
            var response = await taskCompletionSource.Task;

            return response;
        }
        catch (TaskCanceledException)
        {
            throw new Exception("Action invocation timeout");
        }
        finally
        {
            _pendingRequests.Remove(packetEcho);
        }
    }

    public async Task InvokeAction<T>(string actionName, T? parameters)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(_options.Timeout);
        await InvokeActionInternal(new ActionRequestDto<T>
        {
            Action = actionName,
            Params = parameters
        }, cancellationTokenSource.Token);
    }

    public async Task<TReturn?> InvokeAction<TReturn, TParam>(string actionName, TParam? parameters)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(_options.Timeout);

        var response = await InvokeActionInternal(new ActionRequestDto<TParam>
        {
            Action = actionName,
            Params = parameters
        }, cancellationTokenSource.Token);

        return response.ExtensionData.TryGetValue("data", out var value)
            ? value.ToObject<TReturn?>()
            : default;
    }
}
