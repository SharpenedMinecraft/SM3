namespace Frontend.Windows
{
    public class Generic9x1 : BaseWindow
    {
        public Generic9x1(IWindowRegistry windowRegistry) : base(windowRegistry)
        { }
        public override string Type => "minecraft:generic_9x1";
        public override int SlotCount => 9 * 1;
    }
}
