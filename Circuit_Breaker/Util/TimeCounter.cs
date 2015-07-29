using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Circuit_Breaker.Util
{
    public class TimeCounter : IDisposable
    {
        private readonly TimeSpan timeSpan;
        private Timer timer;
        private Action timeOut;

        public TimeCounter(TimeSpan ts, Action onTimeout)
            : this(ts, onTimeout, true)
        {
        }

        public TimeCounter(TimeSpan ts, Action onTimeout, bool autoReset)
        {
            this.timeSpan = ts;
            this.timeOut = onTimeout;
            this.AutoReset = autoReset;

            this.timer = new Timer(this.callBack, null, Timeout.Infinite, Timeout.Infinite);
        }

        private void callBack(object state)
        {
            timeOut();
            if (AutoReset)
            {
                this.Reset();
            }
        }

        public void Start()
        {
            timer.Change(timeSpan, Timeout.InfiniteTimeSpan);
        }


        public bool AutoReset { get; set; }

        public void Reset()
        {
            timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        }

        public void Restart()
        {
            this.Reset();
            this.Start();
        }

        public void Close()
        {
            timer.Dispose();
        }

        public void Dispose()
        {
            Close();
        }
    }
}
