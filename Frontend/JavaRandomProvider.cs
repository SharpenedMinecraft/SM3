using System;
using System.Threading;

namespace Frontend
{
    public sealed class JavaRandomProvider : IRandomProvider
    {
        public long Seed { get; private set; }
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        public int Next(int bits)
        {
            _semaphore.Wait();
            try
            {
                Seed = ((Seed * 0x5DEECE66DL) + 0xBL) & ((1L << 48) - 1);
                return (int) (Seed >> (48 - bits));
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}