namespace Frontend.Windows
{
    public class Generic9x2 : BaseWindow
    {
        public Generic9x2(IWindowRegistry windowRegistry) : base(windowRegistry)
        { }
        public override string Type => "minecraft:generic_9x2";
        public override int SlotCount => 9 * 2;
    }
}
