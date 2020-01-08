using System.Collections.Generic;

namespace Frontend
{
    public interface ICommandNode
    {
        CommandNodeType Type { get; }
        IReadOnlyList<ICommandNode> Children { get; }
        string? Name { get; } // literal & argument
        IArgumentParser? Parser { get; } // parser of argument types
        ICommandNode? Redirect { get; }
        bool IsExecutable { get; }
        // ISuggestionType? SuggestionType { get; } // only valid for Argument Nodes
    }

    public enum CommandNodeType
    {
        Root = 0,
        Literal = 1,
        Argument = 2,
        // unused 3
    }
}