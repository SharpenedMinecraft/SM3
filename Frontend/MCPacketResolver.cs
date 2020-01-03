using System;
using System.Collections.Generic;
using Frontend.Packets.Handshaking;
using Frontend.Packets.Login;
using Frontend.Packets.Play;
using Frontend.Packets.Status;
using Microsoft.Extensions.Logging;

namespace Frontend
{
    public sealed class MCPacketResolver : IPacketResolver
    {
        HashSet<IReadablePacket> _packets = new HashSet<IReadablePacket>(new IReadablePacket[]
        {
            new Handshake(),
            new Ping(),
            new StatusRequest(),
            new LoginStart(), 
            new PlayerSettings(), 
            new ServerboundPluginMessage(), 
        }, new NetworkPacketComparer());

        private class NetworkPacketComparer : EqualityComparer<IReadablePacket>
        {
            public override bool Equals(IReadablePacket x, IReadablePacket y)
            {
                if (x is null)
                    return y is null;
                if (y is null)
                    return false;
                
                return x.Id == y.Id && x.Stage == y.Stage;
            }

            public override int GetHashCode(IReadablePacket obj) 
                => HashCode.Combine(obj.Id, obj.Stage);
        }

        private readonly struct FindPacket : IReadablePacket
        {
            public int Id { get; }
            public MCConnectionStage Stage { get; }

            public FindPacket(int id, MCConnectionStage stage)
            {
                Id = id;
                Stage = stage;
            }

            public void Read(IPacketReader reader)
                => throw new NotSupportedException("This Type is only intended to be used to find matching other types!");

            public void Process(ILogger logger, IConnectionState state, IServiceProvider serviceProvider)
                => throw new NotSupportedException("This Type is only intended to be used to find matching other types!");
        }
        
        public IReadablePacket? GetReadablePacket(int id, IConnectionState connectionState)
        {
            _packets.TryGetValue(new FindPacket(id, connectionState.ConnectionStage), out var packet);
            return packet;
        }
    }
}