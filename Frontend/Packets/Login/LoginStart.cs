using System;
using Microsoft.Extensions.Logging;

namespace Frontend.Packets.Login
{
    public struct LoginStart : IReadablePacket
    {
        public readonly int Id => 0x00;
        public readonly MCConnectionStage Stage => MCConnectionStage.Login;

        public string Username;

        public void Read(IPacketReader reader)
        {
            Username = reader.ReadString().ToString();
        }

        public readonly void Process(ILogger logger, IConnectionState connectionState, IServiceProvider serviceProvider)
        {
            if (!connectionState.IsLocal)
            {
                logger.LogInformation($"{Username} is trying to log in from Remote. Refusing.");
                
                connectionState.PacketQueue.Write(new Disconnect(new ChatBuilder()
                                                     .AppendText("Connection from Remote is not supported \n\n")
                                                     .Bold()
                                                     .WithColor("red")
                                                     .Build()));
            }
            else
            {
                logger.LogInformation($"Logging {Username} in");
                connectionState.Guid = Guid.NewGuid();
                connectionState.PacketQueue.Write(new LoginSuccess(connectionState.Guid, Username));
            }
        }
    }
}