using System;
using Microsoft.Extensions.Logging;

namespace SM3.Frontend.Packets.Status
{
    public struct Ping : IReadablePacket
    {
        public readonly int Id => 0x01;
        public readonly  MCConnectionStage Stage => MCConnectionStage.Status;

        public long Seed;

        public void Read(IPacketReader reader)
        {
            Seed = reader.ReadInt64();
        }

        public readonly void Process(ILogger logger, IConnectionState connectionState, IServiceProvider serviceProvider)
        {
            connectionState.PacketQueue.Write(new Pong(Seed));
        }
    }
}