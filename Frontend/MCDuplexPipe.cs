using System.IO.Pipelines;

namespace SM3.Frontend
{
    public sealed class MCDuplexPipe : IDuplexPipe
    {
        private IDuplexPipe _underlyingDuplexPipe;
        
        public PipeReader Input { get; }

        public PipeWriter Output { get; }

        public MCDuplexPipe(IDuplexPipe pipe)
        {
            _underlyingDuplexPipe = pipe;
            Input = new MCPipeReader(_underlyingDuplexPipe.Input);
            Output = new MCPipeWriter(_underlyingDuplexPipe.Output);
        }
    }
}