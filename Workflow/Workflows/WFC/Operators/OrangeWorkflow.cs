

using System;
using POC.Workflows.WFC;
using POC.Workflows.WFC.Operators;
using POC.Workflows.WFC.Operators.TwistTasks;
using POC.Workflows.WFC.Operators.TwistTasks.Airflow;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace POC.Workflows.WFC
{
    public class OrangeWorkflow : IWorkflow<OrangeWorkflowConfig>
    {
        public string Id => nameof(OrangeWorkflow);

        public int Version => 0;

        public void Build(IWorkflowBuilder<OrangeWorkflowConfig> builder)
        {
            builder.StartWith<FetchAndIngestInvoice>()
                     .WaitFor("TaskUpdated", (config, context) => "Succeeded_0") // I can use the event data here
                .Then<FetchAndIngestUsage>()
                   .WaitFor("TaskUpdated", (config, context) => "Succeeded_2")
                   .OnError(WorkflowErrorHandling.Retry, TimeSpan.FromSeconds(5))
                   .Then<SendApprovalEmail>()
                   .WaitFor("TaskUpdated", (config, context) => "Succeeded_2");

            //builder.StartWith<FetchAndIngestInvoice>()
            //                            .WaitForEvents(this.events, TimeSpan.FromSeconds(20))
            //                            .Then<FetchAndIngestUsage>()
            //                            .WaitForEvents(this.events, TimeSpan.FromSeconds(1))
            //                            .RetryOnError(3, TimeSpan.FromSeconds(5))
            //                            .Then<SendApprovalEmail>()
            //                            .WaitForEvents(this.events, TimeSpan.FromDays(5))
            //                            .WithEscalation(TimeSpan.FromDays(2))
            //                            .Build<WorkflowBase>("Orange Connector", "0.0.1-alpha-1");
        }
    }
}
