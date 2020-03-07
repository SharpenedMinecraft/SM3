using System;
using System.Collections.Generic;
using SM3.Network;
using SM3.Network.Play;
using EntityStatus = SM3.Network.EntityStatus;

namespace SM3.Entities
{
    public sealed class Player : Living
    {
        public override string Type => "sm3:player";
        public override int TypeId => throw new InvalidOperationException("Player does not have a Type ID");
        public override IEnumerable<IWriteablePacket> SpawnPackets
        {
            get {
                yield return new SpawnPlayer(this);
                yield return new Network.Play.EntityMetadata(Entity);
                yield return new PlayerInfo(PlayerInfo.InfoType.AddPlayer, new[] { this });
                yield return new PlayerInfo(PlayerInfo.InfoType.UpdateLatency, new[] { this });
                yield return new Network.Play.EntityStatus(Entity, (byte) EntityStatus.SetOpLevel4);
                yield return new Network.Play.EntityStatus(Entity, (byte) EntityStatus.DisableReducedDebugInfo);
            }
        }

        public string Username { get; set; }

        public Player(Entity entity, IEntityRegistry entityRegistry) : base(entity, entityRegistry)
        {
            Username = "";
        }
    }
}
