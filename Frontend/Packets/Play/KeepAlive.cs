using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Frontend.Packets.Play
{
    public readonly struct KeepAliveClientbound : IWriteablePacket
    {
        public int Id => 0x20;
        
        public readonly long KeepAliveId;

        public KeepAliveClientbound(long keepAliveId)
        {
            KeepAliveId = keepAliveId;
        }

        public void Write(IPacketWriter writer)
        {
            writer.WriteInt64(KeepAliveId);
        }
    }
    
    public struct KeepAliveServerbound : IReadablePacket
    {
        public int Id => 0x0F;

        public MCConnectionStage Stage => MCConnectionStage.Playing;
        
        public long KeepAliveId;
        
        public void Read(IPacketReader reader)
        {
            KeepAliveId = reader.ReadInt64();
        }

        public readonly void Process(ILogger logger, IConnectionState state, IServiceProvider serviceProvider)
        {
            Task.Delay(15).ContinueWith(t => state.PacketQueue.WriteImmediate(new KeepAliveClientbound(1)));
        }
    }
}