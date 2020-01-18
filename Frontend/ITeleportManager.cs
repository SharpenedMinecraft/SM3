using System.Numerics;

namespace Frontend
{
    public interface ITeleportManager
    {
        int BeginTeleport(int entityId, Vector3 target);
        Vector3? EndTeleport(int entityId, int teleportId);
    }
}