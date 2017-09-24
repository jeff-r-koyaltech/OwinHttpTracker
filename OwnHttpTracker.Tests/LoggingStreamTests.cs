using System.IO;
using FluentAssertions;
using OwinHttpTracker;
using Xunit;

namespace OwnHttpTracker.Tests
{
    public class LoggingStreamTests
    {
        public const long MaxBytes = 1024;

        [Fact]
        public void ShouldDuplicateWrites_GivenInput()
        {
            byte[] input = { 0x65, 0x66, 0x67, 0x68 }; //ABCD

            using (MemoryStream observedStream = new MemoryStream())
            using (MemoryStream buffStream = new MemoryStream())
            using (LoggingStream target = new LoggingStream(buffStream, observedStream, MaxBytes))
            {
                target.Write(input, 0, input.Length);

                target.LogContentLength.ShouldBeEquivalentTo(4);
                target.LogContentLength.ShouldBeEquivalentTo(input.Length);
                target.LogContentLength.ShouldBeEquivalentTo(observedStream.Length);
            }
        }

        [Fact]
        public async void ShouldDuplicateWrites_GivenAsyncInput()
        {
            byte[] input = { 0x65, 0x66, 0x67, 0x68 }; //ABCD

            using (MemoryStream observedStream = new MemoryStream())
            using (MemoryStream buffStream = new MemoryStream())
            using (LoggingStream target = new LoggingStream(buffStream, observedStream, MaxBytes))
            {
                await target.WriteAsync(input, 0, input.Length);

                target.LogContentLength.ShouldBeEquivalentTo(4);
                target.LogContentLength.ShouldBeEquivalentTo(input.Length);
                target.LogContentLength.ShouldBeEquivalentTo(observedStream.Length);
            }
        }

        [Fact]
        public async void ShouldDuplicateReads_GivenAsyncInput()
        {
            byte[] input = { 0x65, 0x66, 0x67, 0x68 }; //ABCD

            using (MemoryStream observedStream = new MemoryStream(input, false))
            using (MemoryStream buffStream = new MemoryStream())
            using (LoggingStream target = new LoggingStream(buffStream, observedStream, MaxBytes))
            {
                int bytesToRead = 2;
                byte[] readBytes = new byte[bytesToRead];
                int bytesRead = await target.ReadAsync(readBytes, 0, bytesToRead);

                bytesRead.ShouldBeEquivalentTo(bytesToRead);
                buffStream.Length.ShouldBeEquivalentTo(bytesToRead);
                target.LogContentLength.ShouldBeEquivalentTo(bytesToRead);
                observedStream.Length.ShouldBeEquivalentTo(input.Length);
            }
        }
    }
}
