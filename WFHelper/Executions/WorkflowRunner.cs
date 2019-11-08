using System;
using System.Activities;
using System.Activities.Presentation;
using System.Activities.Tracking;
using System.Activities.XamlIntegration;
using System.IO;
using System.Linq;
using System.Text;
using WFHelper.TrackingParticipant;

namespace WFHelper.Executions
{
    public class WorkflowRunner : IWorkflowRunner
    {
        private readonly WorkflowDesigner _workflowDesigner;

        public WorkflowRunner(WorkflowDesigner workflowDesigner)
        {
            _workflowDesigner = workflowDesigner;
        }

        public bool IsRunning { get; }

        public void Abort()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            var stream = new MemoryStream(Encoding.ASCII.GetBytes(_workflowDesigner.Text));
            var workflow = ActivityXamlServices.Load(stream);
            var wa = new WorkflowApplication(workflow);

            var txtFileTrackingParticipant = new TxtFileTrackingParticipant { TrackingProfile = Etw() };
            wa.Extensions.Add(txtFileTrackingParticipant);

            wa.Completed = Completed;
            wa.OnUnhandledException = UnhandledException;
            wa.Run();
        }

        /// <summary>
        /// 配置追踪器
        /// </summary>
        /// <returns></returns>
        private static TrackingProfile Etw()
        {
            var trackingProfile = new TrackingProfile();
            trackingProfile.Queries.Add(new WorkflowInstanceQuery
            {
                States = { "*" }
            });
            trackingProfile.Queries.Add(new ActivityStateQuery
            {
                States = { "*" }
            });
            trackingProfile.Queries.Add(new CustomTrackingQuery
            {
                ActivityName = "*",
                Name = "*"
            });

            return trackingProfile;
        }

        /// <summary>
        /// 完成
        /// </summary>
        /// <param name="eventArgs"></param>
        private static void Completed(WorkflowApplicationCompletedEventArgs eventArgs)
        {
            try
            {
                var stringWriter = eventArgs.GetInstanceExtensions<StringWriter>().First();
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="eventArgs"></param>
        private static UnhandledExceptionAction UnhandledException(WorkflowApplicationUnhandledExceptionEventArgs eventArgs)
        {
            return UnhandledExceptionAction.Terminate;
        }
    }
}