using System.Collections.Generic;

namespace Frontend.Menus
{
    public class Generic9x6 : BaseMenu
    {
        public Generic9x6(IMenuRegistry menuRegistry) : base(menuRegistry)
        { }
        public override string Type => "minecraft:generic_9x6";
    }
}