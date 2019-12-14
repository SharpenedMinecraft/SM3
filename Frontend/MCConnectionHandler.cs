using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;

namespace Frontend
{
    public sealed class MCConnectionHandler : ConnectionHandler
    {
        public override Task OnConnectedAsync(ConnectionContext connection) 
            => HandleConnection(new MCConnectionContext(connection));

        private async Task HandleConnection(MCConnectionContext ctx)
        {
            ctx.Abort();
        }
    }
}