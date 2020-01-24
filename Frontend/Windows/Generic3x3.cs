namespace Frontend.Windows
{
    public class Generic3x3 : BaseWindow
    {
        public Generic3x3(IWindowRegistry windowRegistry) : base(windowRegistry)
        { }
        public override string Type => "minecraft:generic_3x3";
        public override int SlotCount => 3*3;
    }
}