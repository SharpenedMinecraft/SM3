using System.Collections.Generic;

namespace Frontend
{
    public interface ICommandProvider
    {
        IReadOnlyCollection<ICommand> Commands { get; }

        void Register(ICommand command);
        bool Deregister(ICommand command);
    }
}