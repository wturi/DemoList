using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;

namespace WrokflowCoreDemo
{
    public class HelloWorldWorkflow : IWorkflow
    {
        public string Id => "Hello World";

        public int Version => 1;

        public void Build(IWorkflowBuilder<object> builder)
        {
            builder
                .StartWith<HelloWorld>()
                .Then<ActiveWorld>()
                .Then<GoodbyeWorld>();
        }
    }
}
