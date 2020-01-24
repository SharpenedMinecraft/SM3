namespace Frontend.Windows
{
    public class Generic9x3 : BaseWindow
    {
        public Generic9x3(IWindowRegistry windowRegistry) : base(windowRegistry)
        { }
        public override string Type => "minecraft:generic_9x3";
        public override int SlotCount => 9 * 3;
    }
}