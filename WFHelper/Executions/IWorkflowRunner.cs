using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFHelper.Executions
{
    public interface IWorkflowRunner
    {
        bool IsRunning { get; }

        void Abort();

        void Run();
    }
}
