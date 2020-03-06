using System;
using System.Numerics;

namespace SM3.Frontend
{
    public readonly struct BlockPosition : IEquatable<BlockPosition>, IComparable<BlockPosition>, IComparable
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Z;

        public BlockPosition(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static implicit operator ChunkPosition(BlockPosition position)
            => position.ToChunkPosition();

        public ChunkPosition ToChunkPosition()
        {
            var cx = X >> 4;
            var cz = Z >> 4;
            
            return new ChunkPosition(cx, cz);
        }

        public void Deconstruct(out int x, out int y, out int z)
        {
            x = X;
            y = Y;
            z = Z;
        }

        public static implicit operator BlockPosition((int x, int y, int z) tuple)
            => From(tuple);

        public static BlockPosition From((int x, int y, int z) tuple)
            => new BlockPosition(tuple.x, tuple.y, tuple.z);

        public static BlockPosition From(Vector3 vector)
            => new BlockPosition((int) vector.X, (int) vector.Y, (int) vector.Z);
        
        public static BlockPosition operator +(BlockPosition left, BlockPosition right)
            => Add(left, right);
        
        public static BlockPosition Add(BlockPosition left, BlockPosition right)
            => new BlockPosition(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        
        public static BlockPosition operator -(BlockPosition left, BlockPosition right)
            => Subtract(left, right);
        
        public static BlockPosition Subtract(BlockPosition left, BlockPosition right)
            => new BlockPosition(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

        public bool Equals(BlockPosition other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override bool Equals(object? obj)
        {
            return obj is BlockPosition other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        public static bool operator ==(BlockPosition left, BlockPosition right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BlockPosition left, BlockPosition right)
        {
            return !left.Equals(right);
        }

        public int CompareTo(BlockPosition other)
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
            return obj is BlockPosition other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(BlockPosition)}");
        }

        public static bool operator <(BlockPosition left, BlockPosition right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(BlockPosition left, BlockPosition right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(BlockPosition left, BlockPosition right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(BlockPosition left, BlockPosition right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}