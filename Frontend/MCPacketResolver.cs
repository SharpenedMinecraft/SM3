using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SM3.Network;
using SM3.Network.Handshaking;
using SM3.Network.Login;
using SM3.Network.Play;
using SM3.Network.Status;

namespace SM3.Frontend
{
    public sealed class MCPacketResolver : IPacketResolver
    {
        HashSet<IReadablePacket> _packets = new HashSet<IReadablePacket>(new IReadablePacket[]
        {
            new ChatMessage(),
            new Handshake(),
            new Ping(),
            new StatusRequest(),
            new LoginStart(),
            new PlayerSettings(),
            new ServerboundPluginMessage(),
            new KeepAliveServerbound(),
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
            public ConnectionStage Stage { get; }

            public FindPacket(int id, ConnectionStage stage)
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
