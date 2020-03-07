﻿using System;
using Microsoft.Extensions.Logging;

namespace SM3.Network.Play
{
    public struct ChatMessage : IReadablePacket
    {
        public readonly int Id => 0x03;
        public readonly ConnectionStage Stage => ConnectionStage.Playing;

        public string Message;

        public void Read(IPacketReader reader)
        {
            Message = reader.ReadString().ToString();
        }

        public readonly void Process(ILogger logger, IConnectionState state, IServiceProvider serviceProvider)
        {
            logger.LogInformation($"{state.PlayerEntity.Username} sent a Chat Message: {Message}");
        }

    }
}
