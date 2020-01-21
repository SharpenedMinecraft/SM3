using System;
using System.Numerics;

namespace Frontend
{
    public struct Vector3Int : IEquatable<Vector3Int>, IComparable<Vector3Int>, IComparable
    {
        public int X;
        public int Y;
        public int Z;

        public Vector3Int(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void Deconstruct(out int x, out int y, out int z)
        {
            x = X;
            y = Y;
            z = Z;
        }

        public static implicit operator Vector3Int((int x, int y, int z) value)
            => ToVector3Int(value);

        public static Vector3Int ToVector3Int((int x, int y, int z) value)
            => new Vector3Int(value.x, value.y, value.z);

        public static implicit operator (int x, int y, int z)(Vector3Int value)
            => value.ToTuple();

        public (int x, int y, int z)ToTuple()
            => (X, Y, Z);

        public bool Equals(Vector3Int other) => X == other.X && Y == other.Y && Z == other.Z;

        public override bool Equals(object? obj) => obj is Vector3Int other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        public static bool operator ==(Vector3Int left, Vector3Int right) => left.Equals(right);

        public static bool operator !=(Vector3Int left, Vector3Int right) => !left.Equals(right);

        public int CompareTo(Vector3Int other)
        {
            var xComparison = X.CompareTo(other.X);
            if (xComparison != 0) return xComparison;
            var yComparison = Y.CompareTo(other.Y);
            if (yComparison != 0) return yComparison;
            return Z.CompareTo(other.Z);
        }

        public int CompareTo(object? obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            return obj is Vector3Int other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(Vector3Int)}");
        }

        public static bool operator <(Vector3Int left, Vector3Int right) => left.CompareTo(right) < 0;

        public static bool operator >(Vector3Int left, Vector3Int right) => left.CompareTo(right) > 0;

        public static bool operator <=(Vector3Int left, Vector3Int right) => left.CompareTo(right) <= 0;

        public static bool operator >=(Vector3Int left, Vector3Int right) => left.CompareTo(right) >= 0;
    }
}
