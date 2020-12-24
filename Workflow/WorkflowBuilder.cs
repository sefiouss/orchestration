using System;
using System.Collections.Generic;
using System.Text;
using POC.WB;

namespace POC
{
    public class WorkflowBuilder : IWorkflowBuilder
    {
        private LinkedList<ITwistTask> twistTasks = new LinkedList<ITwistTask>();
        public IWorkflow Build<T>(string name, string version) where T : IWorkflow, new()
        {
            return new T()
            {
                Name = name,
                Version = version,
                Tasks = twistTasks
            };
        }

        public IWorkflowBuilder RetryOnError(int retryCount, TimeSpan interval)
        {
            this.twistTasks.Last.Value.RetryPolicy = new RetryPolicy
            {
                RetryCount = retryCount,
                Interval = interval
            };

            return this;
        }

        public IWorkflowBuilder StartWith<T>() where T : ITwistTask, new()
        {
            twistTasks.AddFirst(new T());

            return this;
        }

        public IWorkflowBuilder Then<T>() where T : ITwistTask, new()
        {
            twistTasks.AddLast(new T());
            return this;
        }

        public IWorkflowBuilder WaitForEvents(string[] eventName, TimeSpan timeout)
        {
            twistTasks.Last.Value.EndEventsNames = eventName;
            twistTasks.Last.Value.Timeout = timeout;
            return this;
        }

        public IWorkflowBuilder WithEscalation(TimeSpan approvalTimeout)
        {
            return this;
        }
    }
}
