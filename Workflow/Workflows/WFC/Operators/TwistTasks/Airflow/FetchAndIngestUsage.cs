using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace POC.Workflows.WFC.Operators.TwistTasks.Airflow
{
    public class FetchAndIngestUsage : AirflowTaskBase
    {
        public FetchAndIngestUsage(IWorkflowHost workflowHost) : base(Guid.NewGuid(), workflowHost)
        {
        }

        public override string Name => "Fetch & Ingest Usage";

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            await base.RunAsync(context);
            context.ExecutionPointer.Status = PointerStatus.Failed;
            return ExecutionResult.Next();
        }
    }
}
