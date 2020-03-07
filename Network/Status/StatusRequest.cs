using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SM3.Frontend;

namespace SM3.Network.Status
{
    public struct StatusRequest : IReadablePacket
    {
        public readonly int Id => 0x00;
        public readonly ConnectionStage Stage => ConnectionStage.Status;

        public readonly void Read(IPacketReader reader)
        {
            // no fields
        }

        public readonly void Process(ILogger logger, IConnectionState connectionState, IServiceProvider serviceProvider)
        {
            connectionState.PacketQueue.Write(new StatusResponse(
                                  new StatusResponse.Payload(
                                      new StatusResponse.Payload.VersionPayload(Constants.VersionName,
                                          Constants.ProtocolVersion),
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