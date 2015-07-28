using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circuit_Breaker.Exception
{
    public class BrokenException : System.Exception
    {
        public BrokenException() : base() { }

        public BrokenException(string message)
            : base(message) { }

        public BrokenException(string message, System.Exception innerException)
            : base(message, innerException) { }
    }
}
