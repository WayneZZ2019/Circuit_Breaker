using Circuit_Breaker.CircuitBreaker;
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

        public static CircuitBreakerContext Create()
        {
            CircuitBreakerContext instance = new CircuitBreakerContext();
            ClosedCircuitBreaker circuitBreaker = new ClosedCircuitBreaker(instance);
            instance.StatedBreaker = circuitBreaker;

            return instance;
        }

        internal StatedCircuitBreaker StatedBreaker { get; set; }

        public State State { get; internal set; }

        internal void TransferOpenState()
        {
            StatedBreaker = new OpenCircuitBreaker(this);
        }

        internal void TransferCloseState()
        {
            StatedBreaker = new ClosedCircuitBreaker(this);
        }

        internal void TransferHalfOpenState()
        {
            StatedBreaker = new HalfOpenCircuitBreaker(this);
        }

        public void Execute(Action action)
        {
            StatedBreaker.Handle(action);
            CheckCircuitBreakerState();
        }

        internal void CheckCircuitBreakerState()
        {

        }

        private CircuitBreakerThreshold threshold = new CircuitBreakerThreshold();
        /// <summary>
        /// 熔断器阀值配置
        /// </summary>
        internal CircuitBreakerThreshold Threshold { get { return threshold; } }

        private CircuitBreakerMetrics metrics = new CircuitBreakerMetrics();
        /// <summary>
        /// 熔断器运行参数
        /// </summary>
        internal CircuitBreakerMetrics Metrics { get { return metrics; } }

    }

    /// <summary>
    /// 熔断器阀值配置
    /// </summary>
    internal class CircuitBreakerThreshold
    {
        /// <summary>
        /// 失败次数（断开阀值），从正常（Closed）切换到断开（Open）
        /// </summary>
        public int FailureThreshold { get; set; }
        /// <summary>
        /// 连续成功次数，从半断开（HalfOpen）切换到正常（Closed）
        /// </summary>
        public int ConsecutiveSuccessThreshold { get; set; }
    }

    /// <summary>
    /// 熔断器运行参数
    /// </summary>
    internal class CircuitBreakerMetrics
    {
        /// <summary>
        /// 失败次数
        /// </summary>
        public int FailureCount { get; set; }
        /// <summary>
        /// 成功次数
        /// </summary>
        public int ConsecutiveSucessCount { get; set; }

        /// <summary>
        /// 最后一次运行异常信息
        /// </summary>
        public System.Exception LastException { get; set; }
    }

    
}
