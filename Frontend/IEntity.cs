using System.Numerics;

namespace Frontend
{
    public interface IEntity
    {
        IEntityId Id { get; }
        int DimensionId { get; }
        Vector3 Position { get; }
        Vector2 Rotation { get; }
    }
}