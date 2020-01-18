using System.IO.Pipelines;

namespace Frontend
{
    public interface IPacketQueueFactory
    {
        IPacketQueue CreateQueue(PipeWriter writer);
    }
}