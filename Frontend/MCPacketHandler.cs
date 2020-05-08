using System;
using System.Buffers;
using System.Text.Json;
using EnumsNET;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Frontend
{
    public sealed class MCPacketHandler : IPacketHandler
    {
        private readonly ILogger _logger;
        public const int ProtocolVersion = 578;
        public const string VersionName = "SM3-1.15.2";
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IPacketResolver _resolver;
        private readonly ISubscriber _subscriber;
        private readonly IServiceProvider _serviceProvider;

        public MCPacketHandler(ILogger<MCPacketHandler> logger, IPacketResolver resolver, IServiceProvider serviceProvider, ISubscriber subscriber)
        {
            _serviceProvider = serviceProvider;
            _subscriber = subscriber;
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
            bool valid;
            using (_logger.BeginScope("Validation"))
                valid = packet.Validate(_logger);
            
            if (valid)
            {
                packet.Message(_subscriber);
            }
        }
    }
}
