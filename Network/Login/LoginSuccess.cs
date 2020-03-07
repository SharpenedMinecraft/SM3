using System;
using System.Globalization;

namespace SM3.Network.Login
{
    public readonly struct LoginSuccess : IWriteablePacket
    {
        public int Id => 0x02;
        public ConnectionStage Stage => ConnectionStage.Login;

        public LoginSuccess(Guid guid, string username)
        {
            Guid = guid;
            Username = username;
        }

        public readonly Guid Guid;
        public readonly string Username;
        
        public void Write(IPacketWriter writer)
        {
            var str = Guid.ToString("D", CultureInfo.InvariantCulture);
            writer.WriteString(str);
            writer.WriteString(Username);
        }
    }
}