using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POC
{
    public class SendApprovalEmail : TwistTaskBase
    {
       public SendApprovalEmail() : base(Guid.NewGuid())
       {
       }

        public override string Name => "Send Approval Email";

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
            if (cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Cancelled !");
                this.Status = TaskStatus.Cancelled;

                await TheBestEventHub.Instance.Publish($"TaskEvents/{this.TaskId}/End", "Cancelled");

                return;
            }

            this.Status = TaskStatus.Running;

            await Task.Delay(2000);

            this.Status = TaskStatus.Succeeded;

            await TheBestEventHub.Instance.Publish($"TaskEvents/{this.TaskId}/End", "Succeeded");
        }
    }
}
