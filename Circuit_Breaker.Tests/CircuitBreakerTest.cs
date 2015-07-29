using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Circuit_Breaker.Tests
{
    [TestClass]
    public class CircuitBreakerTest
    {
        private readonly MockRequestService service;
        private CircuitBreakerContext context;

        public CircuitBreakerTest()
        {
            service = new MockRequestService();
        }

        [TestMethod]
        public void TestCircuitBreaker()
        {
            CircuitBreakerContext context = CircuitBreakerContext.Create(threshold =>
            {
                threshold.ConsecutiveSuccessThreshold = 5;
                threshold.FailureThreshold = 10;
                threshold.FailureTimeout = TimeSpan.FromMinutes(1);
                threshold.RetryTimeout = TimeSpan.FromSeconds(10);
            });

            
        }
    }
}
