using System;
using System.IO.Pipelines;

namespace Frontend
{
    public sealed class MCPacketQueueFactory : IPacketQueueFactory
    {
        private readonly IPacketWriterFactory _writerFactory;
        private readonly IServiceProvider _serviceProvider;

        public MCPacketQueueFactory(IPacketWriterFactory writerFactory, IServiceProvider serviceProvider)
        {
            _writerFactory = writerFactory;
            _serviceProvider = serviceProvider;
        }

        public IPacketQueue CreateQueue(PipeWriter writer) 
            => new MCPacketQueue(writer, _writerFactory, _serviceProvider);
    }
}