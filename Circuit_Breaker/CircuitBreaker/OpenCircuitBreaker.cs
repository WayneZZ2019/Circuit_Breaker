using Circuit_Breaker.Exception;
using Circuit_Breaker.Util;
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
            retryTimeCounter = new TimeCounter(this.RetryTimeout, () => context.TransferHalfOpenState());
        }

        internal override State State
        {
            get { return Circuit_Breaker.State.Open; }
        }

        internal override void Handle(Action action)
        {
            throw new BrokenException();
        }

        public override void Close()
        {
            retryTimeCounter.Close();
        }

        /// <summary>
        /// 尝试重试计时器
        /// </summary>
        private TimeCounter retryTimeCounter;
        /// <summary>
        /// 尝试重试等待超时间隔
        /// </summary>
        public TimeSpan RetryTimeout { get; set; }
    }
}
