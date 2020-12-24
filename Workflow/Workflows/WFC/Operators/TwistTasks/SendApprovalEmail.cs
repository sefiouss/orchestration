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
    public class SendApprovalEmail : TwistTaskBase
    {
        public SendApprovalEmail(IWorkflowHost workflowHost) : base(Guid.NewGuid(), workflowHost)
        {
        }

        public override string Name => "Send Approval Email";

        //TODO: Lookup ExecutionResultProcessor
        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            if (context.CancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Cancelled !");

                await RedisPubSub.Instance.Publish(new PubSubChannel($"Tasks/{context.Step.Id}/status"), new PubSubMessage("Cancelled"));
                await _workflowHost.PublishEvent($"TaskUpdated", $"Cancelled_{context.Step.Id}", null);

                return ExecutionResult.Next();
            }

            this.Status = TaskStatus.Running;
            
            Console.WriteLine($"Executing {Name}");

            await Task.Delay(2000);

            this.Status = TaskStatus.Succeeded;

            await RedisPubSub.Instance.Publish(new PubSubChannel($"Tasks/{context.Step.Id}/status"), new PubSubMessage("Succeeded"));
            await _workflowHost.PublishEvent("TaskUpdated", "Succeeded_" + context.Step.Id.ToString(), null);
            return ExecutionResult.Next();
        }
    }
}
