using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace OwinHttpTracker
{
    public class LoggingStream : Stream
    {
        protected readonly Stream Log;
        protected readonly Stream Stream;

        public long LogContentLength { get; private set; }

        public long MaxBytes { get; }

        public bool MaxExceeded => LogContentLength > MaxBytes;

        /// <summary>
        /// Initializes a new logging stream. Both Log and Stream should be disposed by the caller who provided them
        /// </summary>
        /// <param name="log">Typically a MemoryStream for easy interrogation of the data</param>
        /// <param name="stream">Actual request/response stream</param>
        /// <param name="maxBytes">Maximum number of bytes to log</param>
        public LoggingStream(Stream log, Stream stream, long maxBytes)
        {
            MaxBytes = maxBytes;
            Log = log;
            Stream = stream;
        }
        
        public async Task<byte[]> ReadLogAsync()
        {
            var length = Math.Min(LogContentLength, MaxBytes);
            Log.Seek(0, SeekOrigin.Begin);

            var buffer = new byte[length];
            await Log.ReadAsync(buffer, 0, buffer.Length);

            return buffer;
        }

        private void WriteToLog(byte[] buffer, int offset, int count)
        {
            if (!MaxExceeded)
            {
                Log.Write(buffer, offset, count);
                LogContentLength += count;
            }
        }
        
        public override bool CanSeek => Stream.CanSeek;

        public override bool CanWrite => Stream.CanWrite;

        public override bool CanRead => Stream.CanRead;

        public override void Flush() => Stream.Flush();

        public override long Length => Stream.Length;

        public override long Position
        {
            get { return Stream.Position; }
            set { Stream.Position = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = Stream.Read(buffer, offset, count);

            if (read != 0)
            {
                WriteToLog(buffer, offset, count);
            }

            return count;
        }
        
        public override long Seek(long offset, SeekOrigin origin)
        {
            return Stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            Stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            WriteToLog(buffer, 0, count);
            Stream.Write(buffer, offset, count);
        }
    }
}