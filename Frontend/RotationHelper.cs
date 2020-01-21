using System;
using System.Numerics;

namespace Frontend
{
    public static class RotationHelper
    {
        public static Vector3 ToLookAt(Vector2 rotation)
            => new Vector3(-MathF.Sin(-rotation.X), MathF.Sin(rotation.Y), MathF.Cos(rotation.Y));

        public static Vector2 FromLookAt(Vector3 dir)
            => new Vector2(-MathF.Asin(-dir.Y), MathF.Atan2(dir.X, dir.Z));

        public static byte RadiansTo256Angle(float radians)
            => (byte)MathF.Round(radians / (2 * MathF.PI)  * 256);
    }
}
