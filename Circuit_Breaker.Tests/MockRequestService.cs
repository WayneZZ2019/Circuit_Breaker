using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Circuit_Breaker.Tests
{
    public class MockRequestService
    {
        public void ExcellentService()
        {
            Trace.WriteLine("Good Word");
        }

        public void DangerousService()
        {
            throw new NotImplementedException();
        }
    }
}
