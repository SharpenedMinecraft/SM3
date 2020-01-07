using System.Collections.Generic;

namespace Frontend
{
    public class CommandProvider : ICommandProvider
    {
        private List<ICommand> _commands = new List<ICommand>();

        public IReadOnlyCollection<ICommand> Commands => _commands;
        
        public void Register(ICommand command) => _commands.Add(command);
        public bool Deregister(ICommand command) => _commands.Remove(command);
    }
}