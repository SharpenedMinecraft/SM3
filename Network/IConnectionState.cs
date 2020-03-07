using System;

namespace SM3.Network
{
    public interface IConnectionState
    {
        ConnectionStage ConnectionStage { get; set; }

        IPacketQueue PacketQueue { get; }

        bool IsLocal { get; set; }
        
        Entity? PlayerEntity { get; set; }
        
        public PlayerSettings Settings { get; set; }
        public TimeSpan? Ping { get; set; }
    }
    
    public readonly struct PlayerSettings
    {
        [Flags]
        public enum DisplayedSkinParts
        {
            None = 0x00,
            Cape = 0x01,
            Jacket = 0x02,
            LeftSleeve = 0x04,
            RightSleeve = 0x08,
            LeftPantsLeg = 0x10,
            RightPantsLeg = 0x20,
            Hat = 0x40
        }

        public readonly string Locale;
        public readonly byte RenderDistance;
        public readonly int ChatMode;
        public readonly bool ChatColors;
        public readonly DisplayedSkinParts SkinParts;
        public readonly int MainHand;

        public PlayerSettings(string locale, byte renderDistance, int chatMode, bool chatColors, DisplayedSkinParts skinParts, int mainHand)
        {
            Locale = locale;
            RenderDistance = renderDistance;
            ChatMode = chatMode;
            ChatColors = chatColors;
            SkinParts = skinParts;
            MainHand = mainHand;
        }
    }

    public enum EntityStatus : byte
    {
        ItemUseFinished = 9,
        EnableReducedDebugInfo = 22,
        DisableReducedDebugInfo = 23,
        SetOpLevel0 = 24,
        SetOpLevel1 = 25,
        SetOpLevel2 = 26,
        SetOpLevel3 = 27,
        SetOpLevel4 = 28,
    }
}