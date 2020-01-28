using System;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Frontend.Packets.Login
{
    // ReSharper disable once CA1815
    public readonly struct Disconnect : IWriteablePacket
    {
        public int Id => 0x00;

        public Disconnect(Chat message)
        {
            Message = message;
        }

        public readonly Chat Message;

        public readonly void Write(IPacketWriter writer)
        {
            writer.WriteSpecialType(Message);
        }
    }
}
