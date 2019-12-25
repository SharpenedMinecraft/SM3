using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using Microsoft.Extensions.DependencyInjection;

namespace Frontend
{
    public sealed class MCPacketQueue : IPacketQueue
    {
        private readonly Queue<IPacket> _toWrite = new Queue<IPacket>();
        private readonly PipeWriter _writer;
        private readonly IPacketWriterFactory _writerFactory;
        private readonly IServiceProvider _serviceProvider;

        public bool NeedsWriting => _toWrite.Count != 0;

        public MCPacketQueue(PipeWriter writer, IPacketWriterFactory writerFactory, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _writer = writer;
            _writerFactory = writerFactory;
        }

        public void WriteQueued()
        {
            while (_toWrite.TryDequeue(out var packet))
            {
                WritePacketToPipe(packet);
            }
        }

        private IPacket Instantiate<T>(params object[] parameters) where T : IPacket 
            => ActivatorUtilities.CreateInstance<T>(_serviceProvider, parameters);

        public void Write<T>(params object[] parameters) where T : IPacket
        {
            _toWrite.Enqueue(Instantiate<T>(parameters));
        }

        public void WriteImmediate<T>(params object[] parameters) where T : IPacket
        {
            WritePacketToPipe(Instantiate<T>(parameters));
            _writer.FlushAsync().GetAwaiter().GetResult();
        }

        private void WritePacketToPipe(IPacket packet)
        {
            var dataSize = packet.Size + MCPacketWriter.GetVarIntSize(packet.Id);
            var packetSize = dataSize + MCPacketWriter.GetVarIntSize(dataSize);

            var writer = _writerFactory.CreateWriter(_writer.GetMemory(packetSize));
            
            writer.WriteVarInt(dataSize);
            writer.WriteVarInt(packet.Id);
            packet.Write(writer);
            
            _writer.Advance(packetSize);
        }
    }
}