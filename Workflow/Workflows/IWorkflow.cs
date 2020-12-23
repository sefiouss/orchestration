using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POC
{
    public interface IWorkflow
    {
        string Name { get; set; }
        string Version { get; set; }
        WorkflowStatus Status { get; }
        LinkedList<ITwistTask> Tasks { get; set;  }
        Task Start(dynamic conf, CancellationToken cancellationToken);
        Task Pause();
        Task Resume();
        Task Cancel();
    }
}