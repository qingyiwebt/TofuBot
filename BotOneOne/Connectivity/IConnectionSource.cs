﻿namespace BotOneOne.Connectivity;

public interface IConnectionSource
{
    public Task<Memory<byte>> ReadPacket(CancellationToken cancellationToken);
    public Task SendPacket(Memory<byte> packet, CancellationToken cancellationToken);
}