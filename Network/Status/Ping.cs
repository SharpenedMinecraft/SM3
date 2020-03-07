using System;
using Microsoft.Extensions.Logging;

namespace SM3.Network.Status
{
    public struct Ping : IReadablePacket
    {
        public readonly int Id => 0x01;
        public readonly  ConnectionStage Stage => ConnectionStage.Status;

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