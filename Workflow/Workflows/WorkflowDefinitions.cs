using System;
using System.Collections.Generic;
using System.Linq;
using POC.Steps;
using POC.WB;

namespace POC
{
    public class WorkflowDefinitions
    {
        private static readonly Lazy<WorkflowDefinitions> lazy = new Lazy<WorkflowDefinitions>(() => new WorkflowDefinitions());

        private readonly string[] events = new[] { "TaskEvents/{{TaskId}}/Ended" };

        private WorkflowDefinitions()
        {
            this.Workflows = new List<IWorkflow>();

            this.Workflows.Add(this.BuildOrangeConnector());
        }

        public static WorkflowDefinitions Instance { get { return lazy.Value; } }


        public List<IWorkflow> Workflows { get; private set; }

        public IWorkflow GetWorkflow(string name)
        {
            return this.Workflows.SingleOrDefault(wf => wf.Name == name);
        }

        public IWorkflow BuildOrangeConnector()
        {
            return new WorkflowBuilder().StartWith<FetchAndIngestInvoice>()
                                        .WaitForEvents(this.events, TimeSpan.FromSeconds(20))
                                        .Then<FetchAndIngestUsage>()
                                        .WaitForEvents(this.events, TimeSpan.FromSeconds(1))
                                        .RetryOnError(3, TimeSpan.FromSeconds(5))
                                        .Then<SendApprovalEmail>()
                                        .WaitForEvents(this.events, TimeSpan.FromDays(5))
                                        .WithEscalation(TimeSpan.FromDays(2))
                                        .Build<WorkflowBase>("Orange Connector", "0.0.1-alpha-1");
        }


    }
}
