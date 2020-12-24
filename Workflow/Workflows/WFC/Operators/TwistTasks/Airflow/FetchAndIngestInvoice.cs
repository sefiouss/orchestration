using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkflowCore.Interface;

namespace POC.Workflows.WFC.Operators.TwistTasks.Airflow
{
    public class FetchAndIngestInvoice : AirflowTaskBase
    {
        public FetchAndIngestInvoice(IWorkflowHost workflowHost) : base(Guid.NewGuid(), workflowHost)
        {
        }

        public override string Name => "Fetch & Ingest Invoices";

    }
}
