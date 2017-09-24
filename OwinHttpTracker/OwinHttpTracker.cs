using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using System.IO;

namespace OwinHttpTracker
{
    public class OwinHttpTracker : OwinMiddleware
    {
        /// <summary>
        /// A custom HTTP header to detect when the middleware is active, and to correlate a request with its emitted event.
        /// </summary>
        public const string TrackingHeader = "x-http-tracker-id";

        /// <summary>
        /// Maximum size, in characters, of the request to record
        /// </summary>
        private const long MaxRequestSize = 1024;

        /// <summary>
        /// Maximum size, in characters, of the response to record
        /// </summary>
        private const long MaxResponseSize = 1024;

        private readonly IHttpEventTracker _tracker;

        public OwinHttpTracker(OwinMiddleware next, IHttpEventTracker tracker = null) : base(next)
        {
            _tracker = tracker ?? new HttpEventSourceEventTracker();
        }

        public override async Task Invoke(IOwinContext context)
        {
            var request = context.Request;
            var response = context.Response;

            var requestBuffer = new MemoryStream((int)MaxRequestSize);
            var requestStream = new LoggingStream(requestBuffer, request.Body, MaxRequestSize);
            request.Body = requestStream;

            var responseBuffer = new MemoryStream((int)MaxResponseSize);
            var responseStream = new LoggingStream(responseBuffer, response.Body, MaxResponseSize);
            response.Body = responseStream;

            var httpEvent = new HttpEvent();

            context.Response.OnSendingHeaders(state =>
            {
                var resp = ((IOwinContext)state).Response;
                resp.Headers.Add(TrackingHeader, new[] {httpEvent.TrackingId.ToString("d"),});
            }, context);

            await Next.Invoke(context);

            SetEventRequestHeaders(request, httpEvent);
            httpEvent.RequestLength = requestStream.LogContentLength;
            var reqBytes = await requestStream.ReadLogAsync();
            httpEvent.Request = Convert.ToBase64String(reqBytes);

            SetEventResponseHeaders(response, httpEvent);
            httpEvent.ResponseLength = responseStream.LogContentLength;
            var respBytes = await responseStream.ReadLogAsync();
            httpEvent.Response = Convert.ToBase64String(respBytes);

            _tracker.EmitEvent(httpEvent);
        }

        private static void SetEventRequestHeaders(IOwinRequest request, HttpEvent record)
        {
            record.Verb = request.Method;
            record.Uri = request.Uri.ToString();
            record.RequestHeaders = request.Headers;
        }

        private static void SetEventResponseHeaders(IOwinResponse response, HttpEvent record)
        {
            record.Status = response.StatusCode;
            record.ResponseHeaders = response.Headers;
        }
    }
}
