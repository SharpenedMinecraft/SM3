using System;

namespace Frontend
{
    public readonly struct ChunkPosition : IEquatable<ChunkPosition>, IComparable<ChunkPosition>, IComparable
    {
        public readonly int X;
        public readonly int Z;

        public ChunkPosition(int x, int z)
        {
            X = x;
            Z = z;
        }

        public void Deconstruct(out int x, out int z)
        {
            x = X;
            z = Z;
        }

        public static implicit operator ChunkPosition((int x, int z) tuple)
            => From(tuple);
        
        public static ChunkPosition From((int x, int z) tuple)
            => new ChunkPosition(tuple.x, tuple.z);

        public static ChunkPosition operator +(ChunkPosition left, ChunkPosition right)
            => Add(left, right);
        
        public static ChunkPosition Add(ChunkPosition left, ChunkPosition right)
            => new ChunkPosition(left.X + right.X, left.Z + right.Z);
        
        public static ChunkPosition operator -(ChunkPosition left, ChunkPosition right)
            => Subtract(left, right);
        
        public static ChunkPosition Subtract(ChunkPosition left, ChunkPosition right)
            => new ChunkPosition(left.X - right.X, left.Z - right.Z);

        public bool Equals(ChunkPosition other)
            => X == other.X && Z == other.Z;

        public override bool Equals(object? obj) 
            => obj is ChunkPosition other && Equals(other);

        public override int GetHashCode() 
            => HashCode.Combine(X, Z);

        public static bool operator ==(ChunkPosition left, ChunkPosition right) 
            => left.Equals(right);

        public static bool operator !=(ChunkPosition left, ChunkPosition right) 
            => !left.Equals(right);

        public int CompareTo(ChunkPosition other)
        {
            var xComparison = X.CompareTo(other.X);
            if (xComparison != 0) return xComparison;
            return Z.CompareTo(other.Z);
        }

        public int CompareTo(object? obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            return obj is ChunkPosition other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(ChunkPosition)}");
        }

        public static bool operator <(ChunkPosition left, ChunkPosition right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(ChunkPosition left, ChunkPosition right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(ChunkPosition left, ChunkPosition right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(ChunkPosition left, ChunkPosition right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}