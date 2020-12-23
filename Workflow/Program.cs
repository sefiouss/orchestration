using System;
using System.Threading;
using System.Threading.Tasks;

namespace POC
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var em = new ExecutionManager();

            var ctsource = new CancellationTokenSource();

            dynamic config = new { Username = "admin", Password = "Lmn7op54#" };

            var task = em.InstanciateAndStart("Orange Connector", config, ctsource.Token);
            
            ctsource.CancelAfter(3000);

            await task;

            Console.ReadLine();
        }
    }
}
