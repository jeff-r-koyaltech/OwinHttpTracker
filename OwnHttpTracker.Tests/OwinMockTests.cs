using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Owin.Testing;
using Moq;
using Owin;
using OwinHttpTracker;
using Xunit;

namespace OwnHttpTracker.Tests
{
    public class OwinMockTests
    {
        [Fact]
        public async void ShouldEmitLog_WhenRequestIsMade()
        {
            string trackingHeader = OwinHttpTracker.OwinHttpTracker.TrackingHeader;
            string staticResponse = @"{ ""Hello"": ""world"" ""from"": ""OWIN"" }";

            Mock<IHttpEventTracker> mockTracker = new Mock<IHttpEventTracker>();

            using (var server = TestServer.Create(app =>
            {
                app.UseOwinHttpTracker(mockTracker.Object);
                app.Run(context => context.Response.WriteAsync(staticResponse));
            }))
            {
                HttpResponseMessage response = await server.HttpClient.GetAsync("/");

                mockTracker.Verify(c => c.EmitEvent(It.Is<HttpEvent>((e) =>
                    e.Verb == "GET" && e.ResponseLength == staticResponse.Length)));

                response.Headers.Contains(trackingHeader).ShouldBeEquivalentTo(true);
                IEnumerable<string> headerValues = response.Headers.GetValues(trackingHeader);
                var value = headerValues.First();
                string.IsNullOrEmpty(value).ShouldBeEquivalentTo(false);
            }
        }
    }
}
