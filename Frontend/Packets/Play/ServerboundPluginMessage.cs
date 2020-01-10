using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Frontend.Packets.Play
{
    public struct ServerboundPluginMessage : IReadablePacket
    {
        public readonly int Id => 0x0B;
        public readonly MCConnectionStage Stage => MCConnectionStage.Playing;

        public string Identifier;
        public Memory<byte> Data;
        
        public void Read(IPacketReader reader)
        {
            Identifier = reader.ReadString().ToString();
            var data = reader.ReadBytes((int)reader.Buffer.Length);
            Data = new Memory<byte>(new byte[data.Length]);
            data.CopyTo(Data.Span);
        }

        public void Process(ILogger logger, IConnectionState state, IServiceProvider serviceProvider)
        {
            var span = Data.Span;
            switch (Identifier)
            {
                // temporary solution, but this works
                
                case "minecraft:brand":
                    logger.LogInformation($"{state.PlayerEntity.Username}'s client: {Encoding.UTF8.GetString(Data.Span)}");
                    break;
                
                case "minecraft:register":
                    var channels = new List<string>();
                    while (true)
                    {
                        var end = span.IndexOf((byte)0x00);

                        if (end == -1)
                            break;
                        
                        channels.Add(Encoding.UTF8.GetString(span.Slice(0, end)));
                        
                        if (end >= span.Length)
                            break;
                        
                        span = span.Slice(end + 1);
                    }

                    if (channels.Any(x => x == "sm3:test"))
                    {
                        var stringData = Encoding.UTF8.GetBytes("sm3:test");
                        var memory = new Memory<byte>(new byte[stringData.Length + 1]);
                        stringData.CopyTo(memory);
                        memory.Span[stringData.Length] = 0x00;
                        state.PacketQueue.WriteImmediate(new ClientboundPluginMessage("minecraft:register", memory));
                        state.PacketQueue.Write(new ClientboundPluginMessage("sm3:test", new Memory<byte>(new byte[] { Byte.MaxValue, })));
                        logger.LogInformation("Send SM3:Test");
                    }
                    break;
                
                case "sm3:test":
                    logger.LogInformation("Received SM3:Test");
                    break;
            }
        }
    }
}