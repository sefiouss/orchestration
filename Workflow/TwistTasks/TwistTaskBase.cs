using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POC
{
    public abstract class TwistTaskBase : ITwistTask
    {
        private string[] endEventsNames { get; set; }
        public TwistTaskBase(Guid taskId)
        {
            this.TaskId = taskId;
        }

        public Guid TaskId { get; private set; }

        public TaskStatus Status { get; set; } = TaskStatus.Pending;
        public string[] EndEventsNames { get {
                return this.endEventsNames;
            }
            set {
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

        public abstract Task Cancel();

        public abstract Task Retry(CancellationToken cancellationToken);

        public abstract Task Start(CancellationToken cancellationToken);
    }
}
