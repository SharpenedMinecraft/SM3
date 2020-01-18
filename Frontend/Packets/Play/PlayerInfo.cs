using System;

namespace Frontend.Packets.Play
{
    public readonly struct PlayerInfo : IWriteablePacket
    {
        public enum InfoType
        {
            AddPlayer = 0,
            UpdateGamemode = 1,
            UpdateLatency = 2,
            UpdateDisplayName = 3,
            RemovePlayer = 4
        }

        public int Id => 0x34;

        public readonly InfoType Type;
        public readonly Player[] Players;

        public PlayerInfo(InfoType type, Player[] players)
        {
            Type = type;
            Players = players;
        }

        public void Write(IPacketWriter writer)
        {
            writer.WriteVarInt((int)Type);
            writer.WriteVarInt(Players.Length);

            foreach (var player in Players)
            {
                writer.WriteGuid(player.Guid);
                switch (Type)
                {
                    case InfoType.AddPlayer:
                        writer.WriteString(player.Username);
                        writer.WriteVarInt(0); // no skin support
                        writer.WriteVarInt(1); // Gamemode 1
                        writer.WriteVarInt((int)(player.Ping?.TotalMilliseconds ?? -1));
                        writer.WriteBoolean(false); // No display name
                        break;
                    case InfoType.UpdateGamemode:
                        writer.WriteVarInt(1); // Gamemode 1
                        break;
                    case InfoType.UpdateLatency:
                        writer.WriteVarInt((int)(player.Ping?.TotalMilliseconds ?? -1)); // No Connection ping
                        break;
                    case InfoType.UpdateDisplayName:
                        writer.WriteBoolean(false); // No display name
                        break;
                    case InfoType.RemovePlayer:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Type));
                }
            }
        }
    }
}