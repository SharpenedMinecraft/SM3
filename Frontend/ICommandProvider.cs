using System.Collections.Generic;

namespace Frontend
{
    public interface ICommandProvider
    {
        IReadOnlyCollection<ICommand> Commands { get; }
        CommandInfo[] SortedCommandInfos { get; }

        void Register(ICommand command);
        bool Deregister(ICommand command);

        public readonly struct CommandInfo
        {
            public readonly string? Name;
            public readonly int? RedirectId;
            public readonly CommandNodeType NodeType;
            public readonly int[] Children;
            public readonly bool IsExecutable;
            public readonly IArgumentParser? Parser;

            public CommandInfo(string? name, int? redirectId, CommandNodeType nodeType, int[] children, bool isExecutable, IArgumentParser? parser)
            {
                Name = name;
                RedirectId = redirectId;
                NodeType = nodeType;
                Children = children;
                IsExecutable = isExecutable;
                Parser = parser;
            }
        }
    }
}