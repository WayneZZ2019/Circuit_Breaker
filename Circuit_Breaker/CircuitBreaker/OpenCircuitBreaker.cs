using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circuit_Breaker.CircuitBreaker
{
    internal class OpenCircuitBreaker : StatedCircuitBreaker
    {
        public OpenCircuitBreaker(CircuitBreakerContext context)
            : base(context)
        {
        }

        internal override State State
        {
            get { return Circuit_Breaker.State.Open; }
        }

        internal override void Handle(Action action)
        {
            //throw new BrokenException();
        }
    }
}
