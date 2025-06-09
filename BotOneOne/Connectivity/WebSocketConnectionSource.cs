namespace BotOneOne.Connectivity;

public class WebSocketConnectionSource : IConnectionSource
{
    private readonly string _serverAddr;
    private readonly ushort _port;

    public WebSocketConnectionSource(string serverAddr, ushort port)
    {
        _serverAddr = serverAddr;
        _port = port;
    }

    public Task<Memory<byte>> ReadPacket()
    {
        throw new NotImplementedException();
    }

    public Task SendPacket(Memory<byte> packet)
    {
        throw new NotImplementedException();
    }
}