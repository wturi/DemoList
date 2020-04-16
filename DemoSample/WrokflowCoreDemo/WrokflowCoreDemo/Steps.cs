using System;

using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WrokflowCoreDemo
{
    public class Steps
    {
    }

    public class HelloWorld : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("Hello World");
            return ExecutionResult.Next();
        }
    }

    public class ActiveWorld : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("I am activing in the World!");
            return ExecutionResult.Next();
        }
    }

    public class GoodbyeWorld : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("Goodbye World!");
            return ExecutionResult.Next();
        }
    }
}