using System.Net.WebSockets;
using System.Threading.Tasks.Dataflow;

namespace BotOneOne.Connectivity;

public class ReversedWebSocketConnectionSource : IConnectionSource
{
    private readonly BufferBlock<Memory<byte>> _receiveQueue = new();
    private readonly BufferBlock<Memory<byte>> _sendQueue = new();
    
    private WebSocket? _socket = null;
    private CancellationTokenSource _cancellationTokenSource = new();
    public bool IsOpen => _socket != null;
    
    public Task<Memory<byte>> ReadPacket()
    {
        return _receiveQueue.ReceiveAsync();
    }

    public Task SendPacket(Memory<byte> packet)
    {
        return _sendQueue.SendAsync(packet);
    }

    public void UpgradeWebSocket(WebSocket socket)
    {
        if (_socket != null)
        {
            return;
        }
        
        _socket = socket;
        Worker();
    }

    public void Close()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    private async void Worker()
    {
        var cancellationToken = _cancellationTokenSource.Token;
        
        while (!cancellationToken.IsCancellationRequested
               && _socket!.State == WebSocketState.Open)
        {
            
        }

        _socket = null;
    } 
}