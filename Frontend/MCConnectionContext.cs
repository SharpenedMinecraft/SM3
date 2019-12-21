using System.Collections.Generic;
using System.IO.Pipelines;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;

namespace Frontend
{
    public sealed class MCConnectionContext : ConnectionContext
    {
        private readonly ConnectionContext _underlyingCtx;
        private MCDuplexPipe _mcDuplexPipe;
        
        
        public MCConnectionContext(ConnectionContext ctx)
        {
            _underlyingCtx = ctx;
            _mcDuplexPipe = new MCDuplexPipe(_underlyingCtx.Transport);
            Items["state"] = MCConnectionState.Handshaking;
            Items["isLocal"] = false;
        }

        public MCConnectionState ConnectionState
        {
            get => (MCConnectionState)Items["state"];
            set => Items["state"] = value;
        }
        
        public bool IsLocal
        {
            get => (bool)Items["isLocal"];
            set => Items["isLocal"] = value;
        }

        public bool ShouldFlush { get; private set; }
        public void FlushNext() => ShouldFlush = true;
        
        public bool ShouldClose { get; private set; }
        public void CloseNext() => ShouldClose = true;

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
            get => _mcDuplexPipe;
            set => _mcDuplexPipe = new MCDuplexPipe(value);
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

        public override string? ToString() => _underlyingCtx.ToString();
    }
}