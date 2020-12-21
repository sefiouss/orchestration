using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POC
{
    public class FetchAndIngestUsage : TwistTaskBase
    {
       public FetchAndIngestUsage() : base(Guid.NewGuid())
       {
       }

        public override string Name => "Fetch & Ingest Usage";

        public override Task Cancel()
        {
            throw new NotImplementedException();
        }

        public override Task Retry(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override async Task Start(CancellationToken cancellationToken)
        {
            this.Status = TaskStatus.Running;

            await Task.Delay(1000);

            //this.Status = TaskStatus.Succeeded;

            //await TheBestEventHub.Instance.Publish($"TaskEvents/{this.TaskId}/End", "Succeeded");
        }
    }
}
