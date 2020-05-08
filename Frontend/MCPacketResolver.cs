using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Frontend.Packets.Handshaking;
using Frontend.Packets.Login;
using Frontend.Packets.Play;
using Frontend.Packets.Status;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Frontend
{
    public sealed class MCPacketResolver : IPacketResolver
    {
        private readonly Dictionary<int, Type> _handshakePackets;
        private readonly Dictionary<int, Type> _statusPackets;
        private readonly Dictionary<int, Type> _loginPackets;
        private readonly Dictionary<int, Type> _playingPackets;

        private readonly IServiceProvider _provider;

        public MCPacketResolver(IServiceProvider provider)
        {
            _provider = provider;
            _handshakePackets = new Dictionary<int, Type>
            {
                [0x00] = typeof(Handshake)
            };
            _statusPackets = new Dictionary<int, Type>
            {
                [0x01] = typeof(Ping),
                [0x00] = typeof(StatusRequest)
            };
            _loginPackets = new Dictionary<int, Type>
            {
                [0x00] = typeof(LoginStart)
            };
            _playingPackets = new Dictionary<int, Type>
            {
                [0x0F] = typeof(KeepAliveServerbound),
                [0x05] = typeof(PlayerSettings),
                [0x03] = typeof(ServerboundChatMessage),
                [0x0B] = typeof(ServerboundPluginMessage)
            };
        }

        public IReadablePacket? GetReadablePacket(int id, IConnectionState connectionState)
            => (IReadablePacket)ActivatorUtilities.CreateInstance(_provider, (connectionState.ConnectionStage switch
            {
                MCConnectionStage.Handshaking => _handshakePackets,
                MCConnectionStage.Status => _statusPackets,
                MCConnectionStage.Login => _loginPackets,
                MCConnectionStage.Playing => _playingPackets,
                _ => ThrowInvalidArgumentOutOfRangeExceptionConnectionState()
            })[id]);

        [DoesNotReturn]
        private static Dictionary<int, Type> ThrowInvalidArgumentOutOfRangeExceptionConnectionState() => throw new ArgumentOutOfRangeException("connectionState");
    }
}
