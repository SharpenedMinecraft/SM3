using System;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace Frontend
{
    public sealed class MCPipeWriter : PipeWriter
    {
        private PipeWriter _underlyingPipeWriter;

        public MCPipeWriter(PipeWriter underlyingPipeWriter)
        {
            _underlyingPipeWriter = underlyingPipeWriter;
        }

        public override void Advance(int bytes) 
            => _underlyingPipeWriter.Advance(bytes);

        public override Memory<byte> GetMemory(int sizeHint = 0) 
            => _underlyingPipeWriter.GetMemory(sizeHint);

        public override Span<byte> GetSpan(int sizeHint = 0) 
            => _underlyingPipeWriter.GetSpan(sizeHint);

        public override void CancelPendingFlush() 
            => _underlyingPipeWriter.CancelPendingFlush();

        public override void Complete(Exception exception = null) 
            => _underlyingPipeWriter.Complete(exception);

        public override ValueTask<FlushResult> FlushAsync(CancellationToken cancellationToken = new CancellationToken()) 
            => _underlyingPipeWriter.FlushAsync(cancellationToken);
    }
}