using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                target.LogContentLength.ShouldBeEquivalentTo(input.Length);
                target.LogContentLength.ShouldBeEquivalentTo(observedStream.Length);
            }
        }
    }
}
