using System;
using Microsoft.Extensions.Logging;

namespace Frontend.Packets.Login
{
    public struct LoginSuccess : IPacket
    {
        public readonly int Id => 0x02;
        public readonly bool IsServerbound => false;
        public readonly MCConnectionStage Stage => MCConnectionStage.Login;
        public readonly int Size
        {
            get
            {
                var guidLength = Guid.ToString("D").Length;
                return MCPacketWriter.GetVarIntSize(guidLength) + guidLength + MCPacketWriter.GetVarIntSize(Username.Length) + Username.Length;
            }
        }

        public LoginSuccess(Guid guid, string username)
        {
            Guid = guid;
            Username = username;
        }

        public Guid Guid;
        public string Username;
        
        public readonly void Write(IPacketWriter writer)
        {
            var str = Guid.ToString("D");
            writer.WriteString(str);
            writer.WriteString(Username);
        }

        public void Read(IPacketReader reader)
        {
            Guid = Guid.Parse(reader.ReadString());
            Username = reader.ReadString().ToString();
        }

        public readonly void Process(ILogger logger, IConnectionState connectionState, IPacketQueue packetQueue)
        {
            connectionState.ConnectionStage = MCConnectionStage.Playing;
            // TODO: Tell client about dimension state
        }
    }
}