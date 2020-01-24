using System;
using System.Collections.Generic;
using Frontend.Packets.Play;

namespace Frontend.Entities
{
    public sealed class Player : Living
    {
        private int _selectedSlot = 0;
        public override string Type => "sm3:player";
        public override int TypeId => throw new InvalidOperationException("Player does not have a Type ID");
        public override IEnumerable<IWriteablePacket> SpawnPackets
        {
            get {
                yield return new SpawnPlayer(this);
                yield return new Packets.Play.EntityMetadata(this);
                yield return new PlayerInfo(PlayerInfo.InfoType.AddPlayer, new[] { this });
                yield return new PlayerInfo(PlayerInfo.InfoType.UpdateLatency, new[] { this });
                yield return new Packets.Play.EntityStatus(Id, (byte) EntityStatus.SetOpLevel4);
                yield return new Packets.Play.EntityStatus(Id, (byte) EntityStatus.DisableReducedDebugInfo);
            }
        }

        public string Username { get; set; }
        public PlayerSettings Settings { get; set; }
        public TimeSpan? Ping { get; set; }
        public IWindowManager WindowManager { get; set; }
        
        public Memory<ItemStack<IItem>> Inventory { get; }

        public int SelectedSlot
        
        {
            get => _selectedSlot;
            set
            {
                if (value < 0 || value > 8)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _selectedSlot = value;
            }
        }

        public ref ItemStack<IItem> MainHand => ref Inventory.Span[SelectedSlot + 36];
        public ref ItemStack<IItem> OffHand => ref Inventory.Span[45];
        public Memory<ItemStack<IItem>> Gear => Inventory.Slice(4, 4);
        public ref ItemStack<IItem> Head => ref Gear.Span[0];
        public ref ItemStack<IItem> Torso => ref Gear.Span[1];
        public ref ItemStack<IItem> Legs => ref Gear.Span[2];
        public ref ItemStack<IItem> Feet => ref Gear.Span[3];
        public Memory<ItemStack<IItem>> CraftingArea => Inventory.Slice(1, 4);
        public ref ItemStack<IItem> CraftingResult => ref Inventory.Span[0];
        public Memory<ItemStack<IItem>> Hotbar => Inventory.Slice(35, 9);

        public Player(IEntityRegistry entityRegistry) : base(entityRegistry)
        {
            Username = "";
            WindowManager = null!;
            Inventory = new ItemStack<IItem>[46];
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
}
