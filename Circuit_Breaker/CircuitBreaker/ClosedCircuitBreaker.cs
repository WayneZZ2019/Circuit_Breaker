using Circuit_Breaker.Util;
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
            failedTimeCounter = new TimeCounter(context.Threshold.FailureTimeout, () => context.ResetFailure());
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
                context.IncrementFailure(ex);
                throw;
            }
            finally
            {
                failedTimeCounter.Restart();
            }

            if (context.Metrics.FailureCount >= context.Threshold.FailureThreshold)
            {
                context.TransferOpenState();
            }
        }

        public override void Close()
        {
            failedTimeCounter.Close();
        }

        /// <summary>
        /// 失败次数计时器
        /// </summary>
        private TimeCounter failedTimeCounter;
    }
}
