using System.Collections.Generic;

namespace Frontend.Menus
{
    public class Generic9x5 : BaseMenu
    {
        public Generic9x5(IMenuRegistry menuRegistry) : base(menuRegistry)
        { }
        public override string Type => "minecraft:generic_9x5";
    }
}