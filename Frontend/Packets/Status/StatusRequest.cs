using System;
using App.Metrics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SM3.Frontend.Packets.Status
{
    public struct StatusRequest : IReadablePacket
    {
        public readonly int Id => 0x00;
        public readonly MCConnectionStage Stage => MCConnectionStage.Status;

        public readonly void Read(IPacketReader reader)
        {
            // no fields
        }

        public readonly void Process(ILogger logger, IConnectionState connectionState, IServiceProvider serviceProvider)
        {
            // ReSharper disable once HeapView.BoxingAllocation
            connectionState.PacketQueue.Write(new StatusResponse(
                                  new StatusResponse.Payload(
                                      new StatusResponse.Payload.VersionPayload(MCPacketHandler.VersionName,
                                                                                MCPacketHandler.ProtocolVersion),
                                      new StatusResponse.Payload.PlayersPayload(100, 0, null),
                                      new ChatBuilder(
                                          ).AppendText("This ")
                                           .WithColor("blue")
                                           .Bold()
                                           .WithExtra(builder => builder
                                                                 .AppendText("is ")
                                                                 .WithColor("red")
                                                                 .Bold())
                                           .WithExtra(builder => builder
                                                                 .AppendText("the ")
                                                                 .WithColor("green")
                                                                 .Bold())
                                           .WithExtra(builder => builder
                                                                 .AppendText("MODT")
                                                                 .WithColor("purple")
                                                                 .Bold())
                                           .Build(), null)));
            serviceProvider.GetRequiredService<IMetrics>().Measure.Meter.Mark(MetricsRegistry.StatusRequests);
        }
    }
}