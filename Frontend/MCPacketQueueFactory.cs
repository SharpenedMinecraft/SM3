using System.IO.Pipelines;

namespace Frontend
{
    public sealed class MCPacketQueueFactory : IPacketQueueFactory
    {
        private readonly IPacketWriterFactory _writerFactory;

        public MCPacketQueueFactory(IPacketWriterFactory writerFactory)
        {
            _writerFactory = writerFactory;
        }

        public IPacketQueue CreateQueue(PipeWriter writer) 
            => new MCPacketQueue(writer, _writerFactory);
    }
}