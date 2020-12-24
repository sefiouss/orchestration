using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace POC.Workflows.WFC.Operators.TwistTasks
{
    public abstract class TwistTaskBase : StepBodyAsync
    {
        private string[] endEventsNames { get; set; }
        
        protected readonly IWorkflowHost _workflowHost;
        
        public Guid TaskId { get; private set; }

        public TaskStatus Status { get; set; } = TaskStatus.Pending;

        public TwistTaskBase(Guid taskId, IWorkflowHost workflowHost) : base()
        {
            this.TaskId = taskId;
            this._workflowHost = workflowHost;
        }

        public string[] EndEventsNames
        {
            get
            {
                return this.endEventsNames;
            }
            set
            {
                var newList = new List<string>();
                foreach (var val in value)
                {
                    newList.Add(val.Replace("{{TaskId}}", this.TaskId.ToString()));
                }

                this.endEventsNames = newList.ToArray();
            }
        }
        public TimeSpan Timeout { get; set; }

        public abstract string Name { get; }
        public RetryPolicy RetryPolicy { get; set; }
    }
}
