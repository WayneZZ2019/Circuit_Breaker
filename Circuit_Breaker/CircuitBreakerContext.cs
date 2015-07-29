using Circuit_Breaker.CircuitBreaker;
using Circuit_Breaker.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Circuit_Breaker
{
    public class CircuitBreakerContext
    {
        private CircuitBreakerContext() { }

        private CircuitBreakerContext(StatedCircuitBreaker circuitBreaker)
        {
            this.StatedBreaker = circuitBreaker;
        }

        public static CircuitBreakerContext Create(Action<CircuitBreakerThreshold> setter)
        {
            CircuitBreakerContext instance = new CircuitBreakerContext();
            instance.SetThreshold(setter);
            ClosedCircuitBreaker circuitBreaker = new ClosedCircuitBreaker(instance);
            instance.StatedBreaker = circuitBreaker;

            return instance;
        }

        internal StatedCircuitBreaker StatedBreaker { get; set; }

        public State State { get; internal set; }

        internal void TransferOpenState()
        {
            if (StatedBreaker != null)
                StatedBreaker.Dispose();
            StatedBreaker = new OpenCircuitBreaker(this);
        }

        internal void TransferCloseState()
        {
            if (StatedBreaker != null)
                StatedBreaker.Dispose();
            StatedBreaker = new ClosedCircuitBreaker(this);
        }

        internal void TransferHalfOpenState()
        {
            if (StatedBreaker != null)
                StatedBreaker.Dispose();
            StatedBreaker = new HalfOpenCircuitBreaker(this);
        }

        internal void IncrementConsecutiveSucessCount()
        {
            Interlocked.Increment(ref this.Metrics.ConsecutiveSucessCount);
        }

        internal void IncrementFailure(System.Exception ex)
        {
            this.Metrics.LastException = ex;
            Interlocked.Increment(ref this.Metrics.FailureCount);
        }

        internal void ResetFailure()
        {
            Interlocked.Exchange(ref this.Metrics.FailureCount, 0);
        }

        internal void ResetMetrics()
        {
            Interlocked.Exchange(ref this.Metrics.FailureCount, 0);
            Interlocked.Exchange(ref this.Metrics.ConsecutiveSucessCount, 0);
        }

        internal void SetThreshold(Action<CircuitBreakerThreshold> setter)
        {
            Threshold = new CircuitBreakerThreshold();
            setter(Threshold);
        }

        private void EnsureThresholdInit()
        {
            if (Threshold == null)
            {
                throw new ArgumentNullException("Threshold");
            }
        }

        public void Execute(Action action)
        {
            EnsureThresholdInit();
            StatedBreaker.Handle(action);
        }

        internal CircuitBreakerThreshold Threshold { get; private set; }

        private CircuitBreakerMetrics metrics = new CircuitBreakerMetrics();
 
        internal CircuitBreakerMetrics Metrics { get { return metrics; } }

    }

    public class CircuitBreakerThreshold
    {
        public TimeSpan FailureTimeout { get; set; }

        public int FailureThreshold { get; set; }

        public int ConsecutiveSuccessThreshold { get; set; }

        public TimeSpan RetryTimeout { get; set; }
    }

    internal class CircuitBreakerMetrics
    {
        public int FailureCount = 0;

        public int ConsecutiveSucessCount = 0;

        public System.Exception LastException = null;
    }

    
}
