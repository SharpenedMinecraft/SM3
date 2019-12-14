using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;

namespace Frontend
{
    public class MCConnectionHandler : ConnectionHandler
    {
        public override async Task OnConnectedAsync(ConnectionContext connection)
        {
            connection.Abort();
        }
    }
}