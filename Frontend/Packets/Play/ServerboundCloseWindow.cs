﻿using System;
using Microsoft.Extensions.Logging;

namespace Frontend.Packets.Play
{
    public struct ServerboundCloseWindow : IReadablePacket
    {
        public int Id => 0x13;
        public MCConnectionStage Stage => MCConnectionStage.Playing;

        public byte WindowId;
        
        public void Read(IPacketReader reader)
        {
            WindowId = reader.ReadUInt8();
        }

        public void Process(ILogger logger, IConnectionState state, IServiceProvider serviceProvider)
        {
            var openWindow = state.PlayerEntity.WindowManager.OpenWindow;
            if (openWindow.Id == WindowId)
                state.PlayerEntity.WindowManager.Close(openWindow, true);
        }
    }
}