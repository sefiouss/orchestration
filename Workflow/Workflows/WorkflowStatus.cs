using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC
{
    public enum WorkflowStatus
    {
        Pending,

        Running,

        Paused,

        Cancelled,

        Rollbacked,

        Succeeded,

        Failed
    }
}
