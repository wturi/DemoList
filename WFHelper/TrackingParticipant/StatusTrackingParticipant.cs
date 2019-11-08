using System;
using System.Activities.Tracking;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WFHelper.ViewModels;

namespace WFHelper.TrackingParticipant
{
   public  class StatusTrackingParticipant: System.Activities.Tracking.TrackingParticipant
    {
        protected override void Track(TrackingRecord record, TimeSpan timeout)
        {
            if (record.Level != TraceLevel.Error) return;
            var obj = JsonConvert.DeserializeObject<ErrorRecordDataStructure>(JsonConvert.SerializeObject(record));
            if (obj.FaultSource == null) return;
            var str = obj.EventTime.ToString(CultureInfo.CurrentCulture) + "--" + obj.ActivityDefinitionId + "--" + obj.FaultSource.Name + "--" +
                      obj.UnhandledException.Message;

        }
    }
}
