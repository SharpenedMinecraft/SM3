using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Frontend.Packets.Play
{
    public readonly struct KeepAliveClientbound : IWriteablePacket
    {
        public int Id => 0x21;
        
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
            var broadcastQueue = serviceProvider.GetRequiredService<IBroadcastQueue>();
            var ping = DateTime.UtcNow - DateTime.FromBinary(KeepAliveId);
            broadcastQueue.Broadcast(new PlayerInfo(PlayerInfo.InfoType.UpdateLatency, new[] {state.PlayerEntity}));
            state.PlayerEntity.Ping = ping;
            logger.LogInformation($"New Ping {ping.TotalMilliseconds}ms");
            
            
            Task.Delay(15 * 1000).ContinueWith(t =>
            {
                state.PacketQueue.WriteImmediate(new KeepAliveClientbound(DateTime.UtcNow.ToBinary()));
            });
        }
    }
}