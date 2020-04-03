namespace WFHelper.Executions
{
    public interface IWorkflowRunner
    {
        bool IsRunning { get; }

        void Abort();

        void Run();
    }
}