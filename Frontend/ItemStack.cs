using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Frontend
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct ItemStack<T>
        where T : class, IItem
    {
        public readonly T? Item { get; }
        public readonly sbyte Count { get; }

        public ItemStack(T? item, sbyte count)
        {
            Item = item;
            Count = count;
        }

        public void Deconstruct(out T? item, out sbyte count)
        {
            item = Item;
            count = Count;
        }

        public NetworkSlot NetworkSlot => new NetworkSlot(Count != 0, Item.TypeId, Count, null);
    }
}
