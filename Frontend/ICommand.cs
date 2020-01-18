using System.Collections.Generic;

namespace Frontend
{
    public interface ICommand
    {
        ICommandNode CommandNode { get; }
    }
}