namespace Frontend.Items
{
    public abstract class BaseItem : IItem
    {
        public abstract string Type { get; }
        public int TypeId { get; }

        public BaseItem(IItemRegistry itemRegistry)
        {
            TypeId = itemRegistry[Type];
        }
    }
}
