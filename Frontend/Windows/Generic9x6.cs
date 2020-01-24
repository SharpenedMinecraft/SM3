namespace Frontend.Windows
{
    public class Generic9x6 : BaseWindow
    {
        public Generic9x6(IWindowRegistry windowRegistry) : base(windowRegistry)
        { }
        public override string Type => "minecraft:generic_9x6";
        public override int SlotCount => 9 * 6;
    }
}