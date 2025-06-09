namespace BotOneOne.Connectivity;

public interface IConnectionSource
{
    public Task<Memory<byte>> ReadPacket();
    public Task SendPacket(Memory<byte> packet);
}