using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Frontend
{
    public class CommandProvider : ICommandProvider
    {
        private List<ICommand> _commands = new List<ICommand>();
        private readonly ILogger<CommandProvider> _logger;

        public IReadOnlyCollection<ICommand> Commands => _commands;
        public ICommandProvider.CommandInfo[] SortedCommandInfos { get; private set; }

        public void Register(ICommand command)
        {
            _commands.Add(command);
            SortedCommandInfos = BuildSortedCommandInfos();
        }

        public bool Deregister(ICommand command)
        {
            if (_commands.Remove(command))
            {
                SortedCommandInfos = BuildSortedCommandInfos();
                return true;
            }

            return false;
        }

        public CommandProvider(ILogger<CommandProvider> logger)
        {
            _logger = logger;
            SortedCommandInfos = BuildSortedCommandInfos();
        }

        private ICommandProvider.CommandInfo[] BuildSortedCommandInfos()
        {
            var root = new RootCommandNode(_commands.Select(x => x.CommandNode).ToList());
            int count = 0;
            Count(root, ref count);
            
            var array = new ICommandProvider.CommandInfo[count];

            int i = 0;
            Build(root, ref i, ref array);

            return array;
        }
        
        private int Build(ICommandNode node, ref int i, ref ICommandProvider.CommandInfo[] array)
        {
            var children = new int[node.Children.Count];
            for (var index = 0; index < node.Children.Count; index++)
            {
                children[index] = Build(node.Children[index], ref i, ref array);
            }

            if (node.Redirect != null)
                _logger.LogCritical($"Node {node.Name} has redirect. This is unsupported. ");
            
            array[i] = new ICommandProvider.CommandInfo(node.Name, null, node.Type, children, node.IsExecutable, node.Parser);
            i++;
            return i - 1;
        }

        private void Count(ICommandNode node, ref int count)
        {
            foreach (var child in node.Children)
            {
                Count(child, ref count);
            }

            count += 1;
        }
    }
}