using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circuit_Breaker.CircuitBreaker
{
    public abstract class StatedCircuitBreaker : IDisposable
    {
        protected readonly CircuitBreakerContext context;

        public StatedCircuitBreaker(CircuitBreakerContext context)
        {
            this.context = context;
            this.context.State = State;

            this.context.ResetMetrics();
        }
        abstract internal State State { get; }
        abstract internal void Handle(Action action);

        public virtual void Close() { }

        public void Dispose()
        {
            Close();
        }
    }
}
