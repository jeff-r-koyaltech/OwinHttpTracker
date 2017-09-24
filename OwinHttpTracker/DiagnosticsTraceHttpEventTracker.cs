using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

namespace OwinHttpTracker
{
    public class DiagnosticsTraceHttpEventTracker : IHttpEventTracker
    {
        public void EmitEvent(HttpEvent httpEvent)
        {
            var eventAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(httpEvent);
            System.Diagnostics.Trace.WriteLine(eventAsJson);

            System.Diagnostics.Trace.WriteLine(httpEvent.Request);
            System.Diagnostics.Trace.WriteLine(httpEvent.Response);
        }
    }
}
