using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using POC.Workflows.WFC.Operators;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCore.Models.LifeCycleEvents;

namespace POC
{
    public class ExecutionManager
    {
        private readonly IWorkflowHost _workflowHost;

        public ExecutionManager(IWorkflowHost workflowHost)
        {
            _workflowHost = workflowHost;
        }

        public async Task InstanciateAndStart(string name, OrangeWorkflowConfig config, CancellationToken cancellationToken)
        {
            var wfId = await _workflowHost.StartWorkflow(name, config);
            var wf = await _workflowHost.PersistenceStore.GetWorkflowInstance(wfId);
            //var wf = WorkflowDefinitions.Instance.GetWorkflow(name);
            Console.WriteLine($"[ExecutionManager] started '{wf.Id}' WORKFLOW version {wf.Version}.");

            //await wf.Start(config, cancellationToken);
            Console.WriteLine("[ExecutionManager] WORKFLOW finished with status " + wf.Status);

            Console.WriteLine(wf.ExecutionPointers);

            _workflowHost.OnLifeCycleEvent += Handle;
            _workflowHost.OnStepError += HandleError;
            

            var def = _workflowHost.Registry.GetDefinition(name);

            foreach (WorkflowStep task in def.Steps)
            {
                Console.WriteLine($"\t- {task.Name}");
            }

            while (wf.CompleteTime == null)
            {
                await Task.Delay(5000);
            }
            _workflowHost.OnLifeCycleEvent -= Handle;
            _workflowHost.OnStepError -= HandleError;
        }

        private void HandleError(WorkflowInstance workflow, WorkflowStep step, Exception exception)
        {
            if (exception is /*FileScraperNotFound*/ Exception)
            {
                //Figure out a way to retry this step
            }
        }

        private void Handle(LifeCycleEvent evt)
        {
            Console.WriteLine("Event fired: {0}", evt.ToString());
        }

        public void SaveState()
        {
            // scheduler //
            // resume after crash //
        }
    }
}
