using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POC
{
    public class WorkflowBase : IWorkflow
    {
        protected TaskCompletionSource<TaskStatus> taskCompletionSource;
        protected ITwistTask currentTask;

        public string Name { get; set; }
        public string Version { get; set; }

        public WorkflowStatus Status { get; set; }

        public LinkedList<ITwistTask> Tasks { get; set; }

        public virtual Task Cancel()
        {
            Console.WriteLine("Cancelling workflow...");
            throw new NotImplementedException();
        }

        public virtual Task Pause()
        {
            Console.WriteLine("Pausing workflow...");
            throw new NotImplementedException();
        }

        public virtual Task Resume()
        {
            Console.WriteLine("Resuming workflow...");
            throw new NotImplementedException();
        }

        public virtual Task Rollback()
        {
            Console.WriteLine("Rolling back workflow...");
            throw new NotImplementedException();
        }

        public virtual async Task Start(object conf, CancellationToken cancellationToken)
        {
            this.Status = WorkflowStatus.Running;

            foreach (var twistTask in this.Tasks)
            {
                this.currentTask = twistTask;

                await RedisPubSub.Instance.Subscribe(new PubSubChannel($"Tasks/{this.currentTask.TaskId}/status"), OnMessageReceived);

                var retryCount = 0;
                int timeout = (int)twistTask.Timeout.TotalSeconds;

                // Retry logic
                RetryBlock:
                retryCount++;
                this.currentTask.Status = TaskStatus.Running;

                this.taskCompletionSource = new TaskCompletionSource<TaskStatus>();
                var task = taskCompletionSource.Task;

                Console.WriteLine($"[WorkflowBase] Starting {twistTask.Name}:");
                await twistTask.Start(conf, cancellationToken);

                if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
                {
                    this.currentTask.Status = task.Result;
                    
                    Console.WriteLine($"[WorkflowBase] task {twistTask.Name} finished with success? : " + task.Result);
                }
                else
                {
                    Console.WriteLine($"[WorkflowBase] task {twistTask.Name} timeout  = '" + timeout + "s' elapsed !");
                    

                    // timeout logic
                    twistTask.Status = TaskStatus.Timedout;
                }

                if (this.currentTask.Status != TaskStatus.Succeeded && twistTask.RetryPolicy != null && retryCount <= twistTask.RetryPolicy.RetryCount)
                {
                    Console.WriteLine($"[WorkflowBase] task {twistTask.Name} Retry N° {retryCount} in {twistTask.RetryPolicy.Interval}");
                    await Task.Delay((int)twistTask.RetryPolicy.Interval.TotalMilliseconds);

                    goto RetryBlock;
                }

                await RedisPubSub.Instance.Unsubscribe(new PubSubChannel($"Tasks/{this.currentTask.TaskId}/status"), OnMessageReceived);

                if (this.currentTask.Status != TaskStatus.Succeeded)
                {
                    this.Status = WorkflowStatus.Failed;
                    break;
                }
            }

            if (this.currentTask == this.Tasks.Last.Value && this.currentTask.Status == TaskStatus.Succeeded)
            {
                this.Status = WorkflowStatus.Succeeded;
            }
            else
            {
                if (this.currentTask.Status == TaskStatus.Cancelled)
                {
                    this.Status = WorkflowStatus.Cancelled;
                }
                else
                {
                    this.Status = WorkflowStatus.Failed;
                }
            }

        }

        private void OnMessageReceived(PubSubChannel channel, PubSubMessage message)
        {
            if (this.Status != WorkflowStatus.Running)
            {
                return;
            }

            Console.WriteLine($"[WorkflowBase] {this.currentTask.Name} received an event with message " + message.Content);

            switch (message.Content)
            {
                case "Succeeded":
                    {
                        taskCompletionSource.SetResult(TaskStatus.Succeeded);

                        break;
                    }
                case "Failed":
                    {
                        taskCompletionSource.SetResult(TaskStatus.Failed);
                        break;
                    }

                case "Cancelled":
                    {
                        taskCompletionSource.SetResult(TaskStatus.Cancelled);
                        break;
                    }
            }
        }
    }
}
