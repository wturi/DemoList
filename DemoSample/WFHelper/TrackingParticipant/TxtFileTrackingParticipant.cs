using System;
using System.Activities.Tracking;
using System.IO;

using Newtonsoft.Json;

namespace WFHelper.TrackingParticipant
{
    /// <summary>
    /// ETW跟踪参考者
    /// </summary>
    public class TxtFileTrackingParticipant : System.Activities.Tracking.TrackingParticipant
    {
        private string _fileName = "";

        protected override void Track(TrackingRecord record, TimeSpan timeout)
        {
            //if (record.Level != TraceLevel.Error) return;
            _fileName = @"d:\" + record.InstanceId + ".txt";
            using (var sw = File.AppendText(_fileName))
            {
                var json = JsonConvert.SerializeObject(record);
                var obj = JsonConvert.DeserializeObject<ErrorRecordDataStructure>(json);
                //if (obj.FaultSource == null) return;
                //var str = obj.EventTime.ToString(CultureInfo.CurrentCulture) + "--" + obj.ActivityDefinitionId + "--" + obj.FaultSource.Name.ToString() + "--" +
                //          obj.UnhandledException.Message;
                sw.WriteLine(record.ToString());
            }
        }
    }
}