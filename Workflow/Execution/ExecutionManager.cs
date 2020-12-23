using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POC
{
    public class ExecutionManager
    {
        public async Task InstanciateAndStart(string name, dynamic config, CancellationToken cancellationToken)
        {
            var wf = WorkflowDefinitions.Instance.GetWorkflow(name);

            Console.WriteLine($"[ExecutionManager] starting '{wf.Name}' WORKFLOW version {wf.Version}.");

            
            await wf.Start(config, cancellationToken);

            Console.WriteLine("[ExecutionManager] WORKFLOW finished with status " + wf.Status);

            foreach (var task in wf.Tasks)
            {
                Console.WriteLine($"\t- {task.Name}\t\t{task.Status}");
            }
        }

        public void SaveState()
        {
            // scheduler //
            // resume after crash //
        }
    }
}
