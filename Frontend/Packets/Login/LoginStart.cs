using System;
using Microsoft.Extensions.Logging;

namespace Frontend.Packets.Login
{
    public struct LoginStart : IPacket
    {
        public readonly int Id => 0x00;
        public readonly bool IsServerbound => true;
        public readonly MCConnectionStage Stage => MCConnectionStage.Login;
        public readonly int Size => Username.Length + MCPacketWriter.GetVarIntSize(Username.Length);

        public string Username;
        
        public readonly void Write(IPacketWriter writer)
        {
            writer.WriteString(Username);
        }

        public void Read(IPacketReader reader)
        {
            Username = reader.ReadString().ToString();
        }

        public readonly void Process(ILogger logger, IConnectionState connectionState, IPacketQueue packetQueue)
        {
            if (!connectionState.IsLocal)
            {
                logger.LogInformation($"{Username} is trying to log in from Remote. Refusing.");
                
                packetQueue.Write(new Disconnect(new ChatBuilder()
                                                     .AppendText("Connection from Remote is not supported \n\n")
                                                     .Bold()
                                                     .WithColor("red")
                                                     .Build()));
            }
            else
            {
                logger.LogInformation($"Logging {Username} in");
                connectionState.Guid = Guid.NewGuid();
                packetQueue.Write(new LoginSuccess(connectionState.Guid, Username));
            }
        }
    }
}