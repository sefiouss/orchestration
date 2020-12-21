using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POC
{
    public class FetchAndIngestInvoice : TwistTaskBase
    {
       public FetchAndIngestInvoice() : base(Guid.NewGuid())
       {
       }

        public override string Name => "Fetch & Ingest Invoices";

        public override Task Cancel()
        {
            throw new NotImplementedException();
        }

        public override Task Retry(CancellationToken cancellationToken)
        {
            return this.Start(cancellationToken);
        }

        public override async Task Start(CancellationToken cancellationToken)
        {
            this.Status = TaskStatus.Running;

            await Task.Delay(2000);

            this.Status = TaskStatus.Succeeded;

            await TheBestEventHub.Instance.Publish($"TaskEvents/{this.TaskId}/End", "Succeeded");
        }
    }
}
