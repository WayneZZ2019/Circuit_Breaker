using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circuit_Breaker.CircuitBreaker
{
    internal class ClosedCircuitBreaker : StatedCircuitBreaker
    {
        public ClosedCircuitBreaker(CircuitBreakerContext context)
            : base(context)
        {
        }

        internal override State State
        {
            get { return Circuit_Breaker.State.Closed; }
        }

        internal override void Handle(Action action)
        {
            try
            {
                action();
            }
            catch (System.Exception ex)
            {
                context.Metrics.LastException = ex;
                context.Metrics.FailureCount++;
                throw;
            }
        }
    }
}
