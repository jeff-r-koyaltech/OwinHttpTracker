namespace OwinHttpTracker
{
    public class HttpEventSourceEventTracker : IHttpEventTracker
    {
        private readonly HttpEventSource _log = HttpEventSource.Log;
        public void EmitEvent(HttpEvent httpEvent)
        {
            _log.EmitEvent(httpEvent);
            _log.EmitRequest(httpEvent);
            _log.EmitResponse(httpEvent);
        }
    }
}
