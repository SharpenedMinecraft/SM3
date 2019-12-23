using Microsoft.Extensions.Logging;

namespace Frontend.Packets.Status
{
    public struct StatusRequest : IPacket
    {
        public readonly int Id => 0x00;
        public readonly MCConnectionStage Stage => MCConnectionStage.Status;
        public readonly bool IsServerbound => true;
        public readonly int Size => 0;
        public readonly void Write(IPacketWriter writer)
        {
            // no fields
        }

        public readonly void Read(IPacketReader reader)
        {
            // no fields
        }

        public readonly void Process(ILogger logger, IConnectionState connectionState, IPacketQueue packetQueue)
        {
            // ReSharper disable once HeapView.BoxingAllocation
            packetQueue.Write(new StatusResponse()
            {
                Data = new StatusResponse.Payload(
                    new StatusResponse.Payload.VersionPayload(MCPacketHandler.VERSION_NAME,
                                                              MCPacketHandler.PROTOCOL_VERSION),
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
                         .Build(), null)
            });
        }
    }
}