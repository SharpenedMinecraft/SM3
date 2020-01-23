using System.Collections.Generic;

namespace Frontend.Menus
{
    public class Generic3x3 : BaseMenu
    {
        public Generic3x3(IMenuRegistry menuRegistry) : base(menuRegistry)
        { }
        public override string Type => "minecraft:generic_3x3";
    }
}