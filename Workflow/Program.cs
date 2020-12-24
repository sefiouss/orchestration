using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using POC.Workflows.WFC;
using POC.Workflows.WFC.Operators;
using POC.Workflows.WFC.Operators.TwistTasks;
using POC.Workflows.WFC.Operators.TwistTasks.Airflow;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Services;

namespace POC
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            var host = serviceProvider.GetRequiredService<IWorkflowHost>();

            host.RegisterWorkflow<OrangeWorkflow, OrangeWorkflowConfig>();
            host.Start();

            var em = new ExecutionManager(host);

            var ctsource = new CancellationTokenSource();

            dynamic config = new OrangeWorkflowConfig { BillingIntegrationRetryWindow = 3 };

            var task = em.InstanciateAndStart(nameof(OrangeWorkflow), config, ctsource.Token);

            ctsource.CancelAfter(3000);

            await task;

            Console.ReadLine();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddWorkflow(cfg =>
            {
                //cfg.UseMongoDB(@"mongodb://mongo:27017", "workflow");
                //cfg.UseElasticsearch(new ConnectionSettings(new Uri("http://elastic:9200")), "workflows");
            });

            services.AddLogging();

            //Steps
            services.AddTransient<FetchAndIngestInvoice>();
            services.AddTransient<FetchAndIngestUsage>();
            services.AddTransient<SendApprovalEmail>();
        }
    }
}
