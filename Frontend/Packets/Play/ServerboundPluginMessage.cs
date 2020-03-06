using System;
using System.Text;
using Microsoft.Extensions.Logging;

namespace SM3.Frontend.Packets.Play
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
            switch (Identifier)
            {
                // temporary solution, but this works
                
                case "minecraft:brand":
                    logger.LogInformation($"{state.PlayerEntity.Username}'s client: {Encoding.UTF8.GetString(Data.Span)}");
                    break;
            }
        }
    }
}