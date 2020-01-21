using System.Collections.Generic;

namespace Frontend
{
    public interface IEntity
    {
        int Id { get; set; }
        IEnumerable<IWriteablePacket> SpawnPackets { get; }
        void ProcessTick(IEntityManager preTick, IEntityManager postTick);
    }
}
