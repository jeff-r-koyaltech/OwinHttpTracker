using System;
using System.Diagnostics.Tracing;
using Newtonsoft.Json;

namespace OwinHttpTracker
{
    [EventSource(Name="OwinHttpTrackerEventSource")]
    public class HttpEventSource : EventSource
    {
        #region Shared Instance
        private static readonly Lazy<HttpEventSource> Instance =
            new Lazy<HttpEventSource>(() => new HttpEventSource());
        public static HttpEventSource Log => Instance.Value;
        #endregion

        #region Events
        [Event(1, Message = "Owin Http Event {0} {1}", Level = EventLevel.Informational, Version = 1)]
        public void EmitEvent(HttpEvent httpEvent)
        {
            string trackingId = httpEvent.TrackingId.ToString(("d"));
            string serializedEvent = JsonConvert.SerializeObject(httpEvent);
            WriteEvent(101, trackingId, serializedEvent);
        }

        [Event(2, Message = "Owin Http Request {0} {1}", Level = EventLevel.Verbose, Version = 1)]
        public void EmitRequest(HttpEvent httpEvent)
        {
            string trackingId = httpEvent.TrackingId.ToString(("d"));
            string request = httpEvent.Request;
            WriteEvent(101, trackingId, request);
        }

        [Event(3, Message = "Owin Http Response {0} {1}", Level = EventLevel.Verbose, Version = 1)]
        public void EmitResponse(HttpEvent httpEvent)
        {
            string trackingId = httpEvent.TrackingId.ToString(("d"));
            string response = httpEvent.Response;
            WriteEvent(101, trackingId, response);
        }
        #endregion
    }
}
