using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC
{
    public class RetryPolicy
    {
        public int RetryCount { get; set; }
        public TimeSpan Interval { get; set; }
    }
}
