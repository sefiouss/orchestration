using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POC
{
    public class FetchAndIngestInvoice : AirFlowTaskBase
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
            //return this.Start(cancellationToken);
            throw new NotImplementedException();
        }

        public override async Task Start(dynamic conf, CancellationToken cancellationToken)
        {

            this.Status = TaskStatus.Running;

            Console.WriteLine($">> Fetching and ingesting invoices using username = '{conf.Username}' and password = '{conf.Password}'");

            await Task.Delay(2000);

            this.Status = TaskStatus.Succeeded;

            await RedisPubSub.Instance.Publish(new PubSubChannel($"Tasks/{this.TaskId}/status"), new PubSubMessage("Succeeded"));
        }

        //public void ValidateConf()
        //{
        //    // 1 existance
        //    if (!this.conf.UserName)
        //        throw new ArgumentException("UserName is reuqired");

        //    // 2 type AutoCreateAssets 
        //    //bool AutoCreateAssets

        //    // 3 - content  (email val; range ...)
        //}
    }
}
