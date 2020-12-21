using System;
using System.Collections.Generic;
using System.Text;

namespace POC
{
    public interface IWorkflowBuilder
    {
        IWorkflowBuilder StartWith<T>() where T : ITwistTask, new();

        IWorkflowBuilder Then<T>() where T : ITwistTask, new();

        IWorkflowBuilder WaitForEvents(string[] eventName, TimeSpan timeout);

        IWorkflowBuilder WithEscalation(TimeSpan approvalTimeout);
        
        IWorkflowBuilder RetryOnError(int retryCount, TimeSpan timeSpan);

        IWorkflow Build<T>(string name, string version) where T : IWorkflow, new();
    }
}
