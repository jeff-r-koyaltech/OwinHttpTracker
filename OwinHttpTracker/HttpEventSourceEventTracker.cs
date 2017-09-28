using Newtonsoft.Json;

namespace OwinHttpTracker
{
    public class HttpEventSourceEventTracker : IHttpEventTracker
    {
        private readonly HttpEventSource _log = HttpEventSource.Log;
        public void EmitEvent(HttpEvent httpEvent)
        {
            string trackingId = httpEvent.TrackingId.ToString(("d"));
            string serializedEvent = JsonConvert.SerializeObject(httpEvent);

            _log.EmitEvent(trackingId, serializedEvent);
            _log.EmitRequest(trackingId, httpEvent.Request);
            _log.EmitResponse(trackingId, httpEvent.Response);
        }
    }
}
