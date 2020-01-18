using System;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace Frontend
{
    public sealed class MCPipeReader : PipeReader
    {
        private PipeReader _underlyingPipeReader;

        public MCPipeReader(PipeReader underlyingPipeReader)
        {
            _underlyingPipeReader = underlyingPipeReader;
        }

        public override void AdvanceTo(SequencePosition consumed) 
            => _underlyingPipeReader.AdvanceTo(consumed);

        public override void AdvanceTo(SequencePosition consumed, SequencePosition examined) 
            => _underlyingPipeReader.AdvanceTo(consumed, examined);

        public override void CancelPendingRead() 
            => _underlyingPipeReader.CancelPendingRead();

        public override void Complete(Exception? exception = null) 
            => _underlyingPipeReader.Complete(exception);

        public override ValueTask<ReadResult> ReadAsync(CancellationToken cancellationToken = new CancellationToken())
            => _underlyingPipeReader.ReadAsync(cancellationToken);

        public override bool TryRead(out ReadResult result) 
            => _underlyingPipeReader.TryRead(out result);
    }
}