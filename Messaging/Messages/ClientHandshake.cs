namespace Messaging.Messages
{
    public readonly struct ClientHandshake
    {
        public readonly int ProtocolVersion;
        public readonly string UsedServerAddress;
        public readonly short UsedServerPort;
        public readonly bool RequestsLogin;
        
        public ClientHandshake(int protocolVersion, string usedServerAddress, short usedServerPort, bool requestsLogin)
        {
            ProtocolVersion = protocolVersion;
            UsedServerAddress = usedServerAddress;
            UsedServerPort = usedServerPort;
            RequestsLogin = requestsLogin;
        }
    }
}