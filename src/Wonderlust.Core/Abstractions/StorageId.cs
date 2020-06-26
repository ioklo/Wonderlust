using System;

namespace Wonderlust.Core.Abstractions
{
    public struct StorageId : IEquatable<StorageId>
    {
        int Value;

        public StorageId(int value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            return obj is StorageId id && Equals(id);
        }

        public bool Equals(StorageId other)
        {
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }

        public static bool operator ==(StorageId left, StorageId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(StorageId left, StorageId right)
        {
            return !(left == right);
        }
    }
}