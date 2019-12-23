using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;

namespace Frontend
{
    public sealed class MCConnectionHandler : ConnectionHandler
    {
        private ILogger _logger;
        private readonly IPacketReaderFactory _packetReaderFactory;
        private readonly IPacketQueueFactory _packetQueueFactory;
        private readonly IPacketHandler _packetHandler;

        public MCConnectionHandler(ILogger<MCConnectionHandler> logger, IPacketReaderFactory packetReaderFactory,
                                   IPacketHandler packetHandler,
                                   IPacketQueueFactory packetQueueFactory)
        {
            _packetQueueFactory = packetQueueFactory;
            _packetHandler = packetHandler;
            _packetReaderFactory = packetReaderFactory;
            _logger = logger;
        }

        public override Task OnConnectedAsync(ConnectionContext connection) 
            => HandleConnection(new MCConnectionContext(connection));

        private async Task HandleConnection(MCConnectionContext ctx)
        {
            var packetQueue = _packetQueueFactory.CreateQueue(ctx.Transport.Output);
            while (!ctx.ConnectionClosed.IsCancellationRequested)
            {
                var readResult = await ctx.Transport.Input.ReadAsync(ctx.ConnectionClosed);
                if (readResult.IsCanceled || readResult.IsCompleted)
                {
                    _logger.LogInformation("Connection Closed");
                    return;
                }
                
                var buffer = readResult.Buffer;
                HandlePacket(buffer, ctx, packetQueue);

                if (packetQueue.NeedsWriting)
                {
                    packetQueue.WriteQueued();
                    await ctx.Transport.Output.FlushAsync();
                }

                if (ctx.ShouldClose /* we don't specifically close, we just hand it back to kestrel to deal with */)
                    return;
            }
        }

        private void HandlePacket(ReadOnlySequence<byte> buffer, MCConnectionContext ctx, IPacketQueue packetQueue)
        {
            var reader = _packetReaderFactory.CreateReader(buffer);
            var length = reader.ReadVarInt();

            if (length > reader.Buffer.Length || length < 1 /* 1 = small ID but no fields*/)
            {
                _logger.LogCritical($"Read Invalid length {length:X}. Aborting");
                ctx.Abort();
                return;
            }
            
            reader = new MCPacketReader(reader.Buffer.Slice(0, length));
            var id = reader.ReadVarInt();
            using var packetIdScope = _logger.BeginScope($"Packet ID: {id:x2}");

            _packetHandler.HandlePacket(ctx, reader, packetQueue, id);
            
            // NOT IDEAL, but easiest
            ctx.Transport.Input.AdvanceTo(buffer.GetPosition(length + MCPacketWriter.GetVarIntSize(length)));
        }
    }
}