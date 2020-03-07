using System.Collections.Generic;

namespace SM3
{
    public sealed class RootCommandNode : ICommandNode
    {
        public CommandNodeType Type => CommandNodeType.Root;
        public IReadOnlyList<ICommandNode> Children { get; }
        public string? Name => null;
        public IArgumentParser? Parser => null;
        public ICommandNode? Redirect => null;
        public bool IsExecutable => false;

        public RootCommandNode(IReadOnlyList<ICommandNode> children)
        {
            Children = children;
        }
    }
}