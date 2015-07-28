using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circuit_Breaker.CircuitBreaker
{
    public abstract class StatedCircuitBreaker
    {
        protected readonly CircuitBreakerContext context;

        public StatedCircuitBreaker(CircuitBreakerContext context)
        {
            this.context = context;
            this.context.State = State;
        }
        abstract internal State State { get; }
        abstract internal void Handle(Action action);
    }
}
