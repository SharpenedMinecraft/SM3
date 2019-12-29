using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using Microsoft.Extensions.DependencyInjection;

namespace Frontend
{
    public sealed class MCPacketQueue : IPacketQueue
    {
        private readonly Queue<IWriteablePacket> _toWrite = new Queue<IWriteablePacket>();
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

        public void Write(IWriteablePacket packet)
        {
            _toWrite.Enqueue(packet);
        }

        public void WriteImmediate(IWriteablePacket packet)
        {
            WritePacketToPipe(packet);
            _writer.FlushAsync().GetAwaiter().GetResult();
        }

        private void WritePacketToPipe(IWriteablePacket packet)
        {
            var dataSize = packet.CalculateSize() + MCPacketWriter.GetVarIntSize(packet.Id);
            var packetSize = dataSize + MCPacketWriter.GetVarIntSize(dataSize);

            var writer = _writerFactory.CreateWriter(_writer.GetMemory(packetSize));
            
            writer.WriteVarInt(dataSize);
            writer.WriteVarInt(packet.Id);
            packet.Write(writer);
            
            _writer.Advance(packetSize);
        }
    }
}