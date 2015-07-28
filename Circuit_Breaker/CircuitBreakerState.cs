using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circuit_Breaker
{
    public enum State
    {
        Closed,
        Open,
        HalfOpen
    }
}
