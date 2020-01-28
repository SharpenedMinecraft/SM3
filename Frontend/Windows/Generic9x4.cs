namespace Frontend.Windows
{
    public class Generic9x4 : BaseWindow
    {
        public Generic9x4(IWindowRegistry windowRegistry) : base(windowRegistry)
        { }
        public override string Type => "minecraft:generic_9x4";
        public override int SlotCount => 9 * 4;
    }
}
