using BotOneOne.Connectivity;
using BotOneOne.MessageFormat;
using BotOneOne.Protocol.OneBot.Dto;
using BotOneOne.Protocol.OneBot.Model;
using Newtonsoft.Json;

namespace BotOneOne.Protocol.OneBot;

public abstract class BaseOneBotContext
{
    private readonly IConnectionSource _connectionSource;
    private readonly BotContextOptions _options;
    private readonly Dictionary<string, TaskCompletionSource<ActionResponseDto>> _pendingRequests = [];

    private CancellationTokenSource _cancellationTokenSource = new();
    private Task? _workerTask;

    public event Action<IncomingMessageEventArgs>? MessageReceive;
    
    protected BaseOneBotContext(IConnectionSource connectionSource, BotContextOptions? options = null)
    {
        _connectionSource = connectionSource;
        _options = options ?? BotContextOptions.Default;
    }

    public bool IsOpened => _workerTask != null;

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
            source.TrySetResult(actionResponse);
        }
    }

    private void HandleEvent(string eventType, string packet)
    {
        switch (eventType)
        {
            case "message":
            {
                var dto = JsonConvert.DeserializeObject<MessageEventDto>(packet)
                    ?? throw new Exception("Null packet deserialized");
                
                // WORKAROUND: provide groupId when the message came from a group 
                dto.Sender.GroupId = dto.GroupId;
                
                MessageReceive?.Invoke(new IncomingMessageEventArgs
                {
                    Message = dto.Message == null ? Message.Empty : Message.Parse(dto.Message),
                    MessageId = dto.MessageId,
                    Sender = dto.Sender,
                    Time = DateTimeOffset.FromUnixTimeSeconds(dto.Time),
                    Type = dto.MessageType == "group" ? TargetType.Group : TargetType.User
                });
                break;
            }
            default: throw new Exception($"Unknown eventType \"{eventType}\"");
        }
    }

    private async Task RxWorker(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var packet = await _connectionSource.ReceiveTextAsync(cancellationToken);
                var baseResponse = JsonConvert.DeserializeObject<BaseImcomingDto>(packet);

                if (baseResponse == null)
                {
                    continue;
                }

                if (baseResponse.IsEventPacket)
                {
                    HandleEvent(baseResponse.PostType!, packet);
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
        cancellationTokenSource.CancelAfter(_options.InvocationTimeout);
        await InvokeActionInternal(new ActionRequestDto<T>
        {
            Action = actionName,
            Params = parameters
        }, cancellationTokenSource.Token);
    }

    public async Task<TReturn?> InvokeAction<TReturn, TParam>(string actionName, TParam? parameters)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(_options.InvocationTimeout);

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