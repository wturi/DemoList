using System;
using System.Diagnostics;
using WFHelper.Annotations;

namespace WFHelper.TrackingParticipant
{
    public class RecordDataStructure
    {
    }

    public class ErrorRecordDataStructure
    {
        public UnhandledException UnhandledException { get; set; }
        public FaultSource FaultSource { get; set; }
        public object WorkflowDefinitionIdentity { get; set; }
        public string State { get; set; }
        public string ActivityDefinitionId { get; set; }
        public string InstanceId { get; set; }
        public int RecordNumber { get; set; }
        public object annotations { get; set; }
        public DateTime EventTime { get; set; }
        public TraceLevel Level { get; set; }
    }

    public  class UnhandledException
    {
        [CanBeNull]
        public string ClassName { get; set; }
        [CanBeNull]
        public string Message { get; set; }
        [CanBeNull] 
        public object Data { get; set; }
        [CanBeNull]
        public object InnerException { get; set; }
        [CanBeNull]
        public object HelpURL { get; set; }
        [CanBeNull]
        public string StackTraceString { get; set; }
        [CanBeNull] 
        public object RemoteStackTraceString { get; set; }
        public int RemoteStackIndex { get; set; }
        [CanBeNull] 
        public string ExceptionMethod { get; set; }
        public int HResult { get; set; }
        [CanBeNull] 
        public string Source { get; set; }
        [CanBeNull]
        public object WatsonBuckets { get; set; }
    }

    public  class FaultSource
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string InstanceId { get; set; }
        public string TypeName { get; set; }
    }
}