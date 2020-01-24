namespace Frontend.Windows
{
    public class Generic9x5 : BaseWindow
    {
        public Generic9x5(IWindowRegistry windowRegistry) : base(windowRegistry)
        { }
        public override string Type => "minecraft:generic_9x5";
        public override int SlotCount => 9 * 5;
    }
}