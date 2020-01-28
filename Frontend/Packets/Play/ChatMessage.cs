using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Packets.Play
{
    public struct ChatMessage : IReadablePacket
    {
        public readonly int Id => 0x03;
        public readonly MCConnectionStage Stage => MCConnectionStage.Playing;

        public string Message;

        public void Read(IPacketReader reader)
        {
            Message = reader.ReadString().ToString();
        }

        public readonly void Process(ILogger logger, IConnectionState state, IServiceProvider serviceProvider)
        {
            logger.LogInformation($"{state.PlayerEntity.Username} sent a Chat Message");
        }

    }
}
