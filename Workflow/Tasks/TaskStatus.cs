using System;
using System.Collections.Generic;
using System.Text;

namespace POC
{
    public enum TaskStatus
    {
        Pending,

        Running,

        Pausing,

        Paused,

        Cancelling,

        Cancelled,

        Rollbacking,

        Rollbacked,

        Succeeded,

        Failed,

        Timedout,
    }
}
