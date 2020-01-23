using System.Collections.Generic;

namespace Frontend.Menus
{
    public class Generic9x4 : BaseMenu
    {
        public Generic9x4(IMenuRegistry menuRegistry) : base(menuRegistry)
        { }
        public override string Type => "minecraft:generic_9x4";
    }
}