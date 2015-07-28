using Circuit_Breaker.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circuit_Breaker.CircuitBreaker
{
    internal class HalfOpenCircuitBreaker : StatedCircuitBreaker
    {
        public HalfOpenCircuitBreaker(CircuitBreakerContext context)
            : base(context)
        {
        }

        internal override State State
        {
            get { return Circuit_Breaker.State.HalfOpen; }
        }

        internal override void Handle(Action action)
        {
            //throw new BrokenException();
        }
    }
}
