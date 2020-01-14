using System;
using System.Numerics;

namespace Frontend
{
    public sealed class Player : IEntity
    {
        public Player(IEntityId id, int dimensionId, string username, Guid guid, Vector3 position, Vector2 rotation)
        {
            Id = id;
            DimensionId = dimensionId;
            Username = username;
            Guid = guid;
            Position = position;
            Rotation = rotation;
        }

        public IEntityId Id { get; }

        public Guid Guid { get; }

        public int DimensionId { get; }
        
        public Vector3 Position { get; }
        public Vector2 Rotation { get; }

        public string Username { get; }

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
}