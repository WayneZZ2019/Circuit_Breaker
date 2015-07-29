using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circuit_Breaker.Exception
{
    public class BrokenException : System.Exception
    {
        public BrokenException() : base() 
        {
            this.OccurTime = DateTime.Now;
        }

        public BrokenException(string message)
            : base(message) 
        {
            this.OccurTime = DateTime.Now;
        }

        public BrokenException(string message, System.Exception innerException)
            : base(message, innerException) 
        {
            this.OccurTime = DateTime.Now;
        }

        public BrokenException(string message, System.Exception innerException, DateTime time)
            : base(message, innerException)
        {
            this.OccurTime = time;
        }

        /// <summary>
        /// 异常发生时间
        /// </summary>
        public DateTime OccurTime { get; private set; }
    }
}
