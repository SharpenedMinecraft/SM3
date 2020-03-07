using System.IO.Pipelines;
using SM3.Network;

namespace SM3.Frontend
{
    public interface IPacketQueueFactory
    {
        IPacketQueue CreateQueue(PipeWriter writer);
    }
}