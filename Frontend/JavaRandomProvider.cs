using System;
using System.Threading;

namespace Frontend
{
    public sealed class JavaRandomProvider : IRandomProvider, IDisposable
    {
        public long Seed { get; private set; }
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        public int Next(int bits)
        {
            _semaphore.Wait();
            try
            {
                unchecked
                {
                    Seed = ((Seed * 0x5DEECE66DL) + 0xBL) & ((1L << 48) - 1);
                    return (int) (Seed >> (48 - bits));
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public void Dispose()
        {
            _semaphore.Dispose();
        }
    }
}