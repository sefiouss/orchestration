using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POC
{
    public interface ITwistTask
    {
        Task Start(CancellationToken cancellationToken);

        Task Cancel();

        Task Retry(CancellationToken cancellationToken);

        Guid TaskId { get; }

        string Name { get; }

        TaskStatus Status { get; set; }

        string[] EndEventsNames { get; set; }

        TimeSpan Timeout { get; set; }

        public RetryPolicy RetryPolicy { get; set; }
    }
}
