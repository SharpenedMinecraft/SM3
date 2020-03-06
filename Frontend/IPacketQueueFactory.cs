using System.IO.Pipelines;

namespace SM3.Frontend
{
    public interface IPacketQueueFactory
    {
        IPacketQueue CreateQueue(PipeWriter writer);
    }
}