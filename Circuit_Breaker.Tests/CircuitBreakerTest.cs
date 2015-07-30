using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.Threading;
using Circuit_Breaker.Exception;
using System.Diagnostics;

namespace Circuit_Breaker.Tests
{
    [TestClass]
    public class CircuitBreakerTest
    {
        private readonly MockRequestService service;

        public CircuitBreakerTest()
        {
            service = new MockRequestService();
        }

        [TestMethod]
        public void TestCircuitBreakerInitState()
        {
            CircuitBreakerContext context = CircuitBreakerContext.Create(threshold =>
            {
                threshold.ConsecutiveSuccessThreshold = 5;
                threshold.FailureThreshold = 10;
                threshold.FailureTimeout = TimeSpan.FromMinutes(1);
                threshold.RetryTimeout = TimeSpan.FromSeconds(10);
            });

            context.State.Should().Be(State.Closed);

            for (int i = 0; i < 10; i++)
            {
                context.Execute(() => service.ExcellentService());
            }

            context.State.Should().Be(State.Closed);
            context.LastException.Should().BeNull();
        }

        [TestMethod]
        public void TestCircuitBreakerClosedToOpen()
        {
            CircuitBreakerContext context = CircuitBreakerContext.Create(threshold =>
            {
                threshold.ConsecutiveSuccessThreshold = 5;
                threshold.FailureThreshold = 10;
                threshold.FailureTimeout = TimeSpan.FromMinutes(1);
                threshold.RetryTimeout = TimeSpan.FromSeconds(10);
            });

            for (int i = 0; i < 10; i++)
            {
                context.Execute(() => service.DangerousService());
            }

            context.State.Should().Be(State.Open);
            context.LastException.Should().NotBeNull().And.BeOfType<NotImplementedException>();
        }

        [TestMethod]
        public void TestCircuitBreakerClosedTimeout()
        {
            CircuitBreakerContext context = CircuitBreakerContext.Create(threshold =>
            {
                threshold.ConsecutiveSuccessThreshold = 5;
                threshold.FailureThreshold = 10;
                threshold.FailureTimeout = TimeSpan.FromMilliseconds(100);
                threshold.RetryTimeout = TimeSpan.FromSeconds(10);
            });

            context.Execute(() => service.DangerousService());

            for (int i = 0; i < 10; i++)
            {
                context.Execute(() => service.DangerousService());
                Thread.Sleep(300);
            }

            context.State.Should().Be(State.Closed);
        }

        [TestMethod]
        public void TestCircuitBreakerOpenToHalfOpen()
        {
            CircuitBreakerContext context = CircuitBreakerContext.Create(threshold =>
            {
                threshold.ConsecutiveSuccessThreshold = 5;
                threshold.FailureThreshold = 10;
                threshold.FailureTimeout = TimeSpan.FromMinutes(1);
                threshold.RetryTimeout = TimeSpan.FromSeconds(5);
            });

            for (int i = 0; i < 10; i++)
            {
                context.Execute(() => service.DangerousService());
            }

            Thread.Sleep(6000);

            context.State.Should().Be(State.HalfOpen);
        }

        [TestMethod]
        public void TestCircuitBreakerHalfOpenToOpen()
        {
            CircuitBreakerContext context = CircuitBreakerContext.Create(threshold =>
            {
                threshold.ConsecutiveSuccessThreshold = 5;
                threshold.FailureThreshold = 10;
                threshold.FailureTimeout = TimeSpan.FromMinutes(1);
                threshold.RetryTimeout = TimeSpan.FromSeconds(5);
            });

            context.Execute(() => service.DangerousService());

            for (int i = 0; i < 10; i++)
            {
                context.Execute(() => service.DangerousService());
            }

            Thread.Sleep(6000);

            context.Execute(() => service.DangerousService());

            context.State.Should().Be(State.Open);
            context.LastException.Should().NotBeNull().And.BeOfType<NotImplementedException>();
        }

        [TestMethod]
        public void TestCircuitBreakerHalfOpenToClosed()
        {
            CircuitBreakerContext context = CircuitBreakerContext.Create(threshold =>
            {
                threshold.ConsecutiveSuccessThreshold = 5;
                threshold.FailureThreshold = 10;
                threshold.FailureTimeout = TimeSpan.FromMinutes(1);
                threshold.RetryTimeout = TimeSpan.FromSeconds(5);
            });

            context.Execute(() => service.DangerousService());

            for (int i = 0; i < 5; i++)
            {
                context.Execute(() => service.DangerousService());
            }

            Thread.Sleep(6000);

            for (int i = 0; i < 5; i++)
            {
                context.Execute(() => service.ExcellentService());
            }

            context.State.Should().Be(State.Closed);
        }

        [TestMethod]
        [ExpectedException(typeof(BrokenException))]
        public void TestCircuitBreakerExeption()
        {
            CircuitBreakerContext context = CircuitBreakerContext.Create(threshold =>
            {
                threshold.ConsecutiveSuccessThreshold = 5;
                threshold.FailureThreshold = 10;
                threshold.FailureTimeout = TimeSpan.FromMinutes(1);
                threshold.RetryTimeout = TimeSpan.FromSeconds(10);
                threshold.ExceptionHandler = ex =>
                {
                    if (ex is NotImplementedException)
                    {
                        Trace.WriteLine(ex.Message);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                };
            });

            for (int i = 0; i < 10; i++)
            {
                context.Execute(() => service.DangerousService());
            }

            context.State.Should().Be(State.Open);
            context.LastException.Should().NotBeNull().And.BeOfType<NotImplementedException>();

            context.Execute(() => service.DangerousService());
        }

    }
}
