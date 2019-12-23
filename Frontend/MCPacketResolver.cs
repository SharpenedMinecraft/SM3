using System;
using System.Collections.Generic;
using Frontend.Packets.Handshaking;
using Frontend.Packets.Status;
using Microsoft.Extensions.Logging;

namespace Frontend
{
    public sealed class MCPacketResolver : IPacketResolver
    {
        HashSet<IPacket> _packets = new HashSet<IPacket>(new IPacket[]
        {
            new Handshake(),
            new Ping(), new Pong(),
            new StatusRequest(), new Packets.Status.StatusResponse(),
        }, new NetworkPacketComparer());

        private class NetworkPacketComparer : EqualityComparer<IPacket>
        {
            public override bool Equals(IPacket x, IPacket y)
            {
                if (x is null)
                    return y is null;
                if (y is null)
                    return false;
                
                return x.Id == y.Id && x.IsServerbound == y.IsServerbound && x.Stage == y.Stage;
            }

            public override int GetHashCode(IPacket obj) 
                => HashCode.Combine(obj.Id, obj.Stage, obj.IsServerbound);
        }

        private readonly struct FindPacket : IPacket
        {
            public int Id { get; }
            public bool IsServerbound { get; }
            public MCConnectionStage Stage { get; }
            public int Size => NoOperationsG<int>();
            public void Write(IPacketWriter writer)
                => NoOperations();

            public FindPacket(int id, bool isServerbound, MCConnectionStage stage)
            {
                Id = id;
                IsServerbound = isServerbound;
                Stage = stage;
            }

            public void Read(IPacketReader reader)
                => NoOperations();

            public void Process(ILogger logger, IConnectionState connectionState, IPacketQueue packetQueue)
                => NoOperations();

            private T NoOperationsG<T>() => throw new NotSupportedException("This Type is only intended to be used to find matching other types!");
            private void NoOperations() => throw new NotSupportedException("This Type is only intended to be used to find matching other types!");
        }
        
        public IPacket? GetPacket(int id, IConnectionState connectionState, bool serverBound)
        {
            _packets.TryGetValue(new FindPacket(id, serverBound, connectionState.ConnectionStage), out var packet);
            return packet;
        }
    }
}