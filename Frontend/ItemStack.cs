using System;
using System.Runtime.InteropServices;

namespace Frontend
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct ItemStack<T>
        where T : notnull, IItem
    {
        public readonly T Item { get; }
        public readonly sbyte Count { get; }

        public ItemStack(T item, sbyte count)
        {
            Item = item;
            Count = count;
        }
    }
}
