using System;

namespace Frontend
{
    public class Player : IEntity
    {
        public Player(IEntityId id, int dimensionId, string username)
        {
            Id = id;
            DimensionId = dimensionId;
            Username = username;
        }

        public IEntityId Id { get; }
        
        public int DimensionId { get; }
        
        public string Username { get; set; }

        public PlayerSettings Settings { get; set; }

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
    }
}