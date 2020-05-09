using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Frontend
{
    public sealed class FullAsyncMediator : Mediator
    {
        public FullAsyncMediator(ServiceFactory serviceFactory) : base(serviceFactory)
        { }

        protected override async Task PublishCore(IEnumerable<Func<INotification, CancellationToken, Task>> handlers,
            INotification notification, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();
            var exceptions = new List<Exception>();

            foreach (var handler in handlers)
            {
                try
                {
                    tasks.Add(handler(notification, cancellationToken));
                }
                catch (Exception ex) when (!(ex is OutOfMemoryException || ex is StackOverflowException))
                {
                    exceptions.Add(ex);
                }
            }

            try
            {
                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            catch (AggregateException ex)
            {
                exceptions.AddRange(ex.Flatten().InnerExceptions);
            }
            catch (Exception ex) when (!(ex is OutOfMemoryException || ex is StackOverflowException))
            {
                exceptions.Add(ex);
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }
    }
}