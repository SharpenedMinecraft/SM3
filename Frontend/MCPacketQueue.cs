using System.Collections.Generic;
using System.IO.Pipelines;

namespace Frontend
{
    public sealed class MCPacketQueue : IPacketQueue
    {
        private readonly Queue<IPacket> _toWrite = new Queue<IPacket>();
        private readonly PipeWriter _writer;
        private readonly IPacketWriterFactory _writerFactory;

        public bool NeedsWriting => _toWrite.Count != 0;

        public MCPacketQueue(PipeWriter writer, IPacketWriterFactory writerFactory)
        {
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
        
        public void Write(IPacket packet)
        {
            _toWrite.Enqueue(packet);
        }

        public void WriteImmediate(IPacket packet)
        {
            WritePacketToPipe(packet);
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