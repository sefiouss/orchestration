using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace POC.Workflows.WFC.Operators.TwistTasks.Airflow
{
    public abstract class AirflowTaskBase : TwistTaskBase
    {
        public AirflowTaskBase(Guid id, IWorkflowHost workflowHost) : base(id, workflowHost)
        {

        }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            this.Status = TaskStatus.Running;

            Console.WriteLine($"Executing {Name}");

            await CallAirflowDag();

            this.Status = TaskStatus.Succeeded;

            await RedisPubSub.Instance.Publish(new PubSubChannel($"Tasks/{context.Step.Id}/status"), new PubSubMessage("Succeeded"));
            // !! Event Data can be used in the workflow builder
            Console.WriteLine($"{context.Step.Id}, {context.Step.Name}, {context.Step.ExternalId}, {GetType().Name}");
            await _workflowHost.PublishEvent("TaskUpdated", $"Succeeded_{context.Step.Name}", null); // Or use WaitFor ?

            return ExecutionResult.Next();
        }

        protected virtual async Task CallAirflowDag()
        {
            await Task.Delay((new Random().Next(4) + 1) * 1000);
        }
    }
}
