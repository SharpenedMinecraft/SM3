using System;
using System.Buffers;
using System.Text.Json;
using EnumsNET;
using Microsoft.Extensions.Logging;

namespace Frontend
{
    public sealed class MCPacketHandler : IPacketHandler
    {
        private readonly ILogger _logger;
        public const int PROTOCOL_VERSION = 498;
        public const string VERSION_NAME = "SM3-1.14.4";
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IPacketResolver _resolver;
        private readonly IServiceProvider _serviceProvider;

        public MCPacketHandler(ILogger<MCPacketHandler> logger, IPacketResolver resolver, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _resolver = resolver;
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions()
            {
                IgnoreNullValues = true
            };
        }
        
        public void HandlePacket(MCConnectionContext ctx, IPacketReader reader, IPacketQueue packetQueue, int id)
        {
            var packet = _resolver.GetReadablePacket(id, ctx);
            if (packet == null)
            {
                _logger.LogInformation($"Unknown {ctx.ConnectionStage.AsString()} Packet {id:x2}");
                return;
            }

            packet.Read(reader);
            packet.Process(_logger, ctx, _serviceProvider);
        }
    }
}