using System;
using Microsoft.Extensions.Logging;

namespace SM3.Network.Play
{
    public struct PlayerSettings : IReadablePacket
    {
        public readonly int Id => 0x05;
        public readonly ConnectionStage Stage => ConnectionStage.Playing;
        
        public string Locale;
        public byte RenderDistance;
        public int ChatMode;
        public bool ChatColors;
        public byte SkinParts;
        public int MainHand;
        
        public void Read(IPacketReader reader)
        {
            Locale = reader.ReadString().ToString();
            RenderDistance = reader.ReadUInt8();
            ChatMode = reader.ReadVarInt();
            ChatColors = reader.ReadBoolean();
            SkinParts = reader.ReadUInt8();
            MainHand = reader.ReadVarInt();
        }

        public readonly void Process(ILogger logger, IConnectionState state, IServiceProvider serviceProvider)
        {
            state.PlayerEntity.Settings = new Player.PlayerSettings(Locale, RenderDistance, ChatMode, ChatColors,
                                                                    (Player.PlayerSettings.DisplayedSkinParts)
                                                                    SkinParts, MainHand);
            logger.LogInformation($"{state.PlayerEntity.Username} update their Settings");
        }
    }
}