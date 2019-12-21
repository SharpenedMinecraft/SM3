using System;

namespace Frontend
{
    public interface IConnectionState
    {
        MCConnectionStage ConnectionStage { get; set; }

        bool IsLocal { get; set; }

        Guid Guid { get; set; }
    }
}