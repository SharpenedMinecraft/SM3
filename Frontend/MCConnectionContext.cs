using System.Collections.Generic;
using System.IO.Pipelines;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Features;

namespace Frontend
{
    public sealed class MCConnectionContext : ConnectionContext
    {
        private ConnectionContext _underlyingCtx;
        
        public MCConnectionContext(ConnectionContext ctx)
        {
            _underlyingCtx = ctx;
        }

        public override string ConnectionId
        {
            get => _underlyingCtx.ConnectionId;
            set => _underlyingCtx.ConnectionId = value;
        }

        public override IFeatureCollection Features => _underlyingCtx.Features;

        public override IDictionary<object, object> Items
        {
            get => _underlyingCtx.Items;
            set => _underlyingCtx.Items = value;
        }

        public override IDuplexPipe Transport
        {
            get => _underlyingCtx.Transport;
            set => _underlyingCtx.Transport = value;
        }

        public override void Abort() => _underlyingCtx.Abort();

        public override void Abort(ConnectionAbortedException abortReason) => _underlyingCtx.Abort(abortReason);

        public override ValueTask DisposeAsync() => _underlyingCtx.DisposeAsync();

        public override CancellationToken ConnectionClosed        
        {
            get => _underlyingCtx.ConnectionClosed;
            set => _underlyingCtx.ConnectionClosed = value;
        }
        public override EndPoint LocalEndPoint
        {
            get => _underlyingCtx.LocalEndPoint;
            set => _underlyingCtx.LocalEndPoint = value;
        }
        public override EndPoint RemoteEndPoint 
        {
            get => _underlyingCtx.RemoteEndPoint;
            set => _underlyingCtx.RemoteEndPoint = value;
        }

        public override bool Equals(object? obj) => _underlyingCtx.Equals(obj);

        public override int GetHashCode() => _underlyingCtx.GetHashCode();

        public override string ToString() => _underlyingCtx.ToString();
    }
}