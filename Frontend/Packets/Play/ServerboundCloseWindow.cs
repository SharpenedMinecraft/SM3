﻿using System;
using Microsoft.Extensions.Logging;

namespace Frontend.Packets.Play
{
    public struct ServerboundCloseWindow : IReadablePacket
    {
        public int Id => 0x0A;
        public MCConnectionStage Stage => MCConnectionStage.Playing;

        public byte WindowId;
        
        public void Read(IPacketReader reader)
        {
            WindowId = reader.ReadUInt8();
        }

        public void Process(ILogger logger, IConnectionState state, IServiceProvider serviceProvider)
        {
            if (WindowId == 0)
            {
                // Inventory was closed
            }
            
            var openMenu = state.PlayerEntity.MenuManager.OpenMenu;
            if (openMenu?.Id == WindowId)
                state.PlayerEntity.MenuManager.Close(openMenu, true);
        }
    }
}