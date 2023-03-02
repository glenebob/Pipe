using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Pipe.Test
{
    [TestClass]
    public class PipeReadStreamExceptionTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipeReadStreamCtorThrowsOnNullArgument()
        {
            new PipeReadStream(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void PipeReadStreamLengthGetThrows()
        {
            _ = new PipeReadStream(new Pipe()).Length;
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void PipeReadStreamSetLengthThrows()
        {
            new PipeReadStream(new Pipe()).SetLength(0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void PipeReadStreamPositionGetThrows()
        {
            _ = new PipeReadStream(new Pipe()).Position;
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void PipeReadStreamPositionSetThrows()
        {
            new PipeReadStream(new Pipe()).Position = 0;
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void PipeReadStreamSeekThrows()
        {
            _ = new PipeReadStream(new Pipe()).Seek(0, SeekOrigin.Begin);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void PipeReadStreamWriteThrows()
        {
            new PipeReadStream(new Pipe()).Write(Utilities.EmptyByteArray, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void PipeReadStreamWriteAsyncThrows()
        {
            new PipeReadStream(new Pipe()).WriteAsync(Utilities.EmptyByteArray, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void PipeReadStreamFlushThrows()
        {
            new PipeReadStream(new Pipe()).Flush();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipeReadStreamReadThrowsOnNullBuffer()
        {
            new PipeReadStream(new Pipe()).Read(null, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipeReadStreamReadAsyncThrowsOnNullBuffer()
        {
            new PipeReadStream(new Pipe()).ReadAsync(null, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeReadStreamReadThrowsOnNegativeOffset()
        {
            new PipeReadStream(new Pipe()).Read(Utilities.EmptyByteArray, -1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeReadStreamReadAsyncThrowsOnNegativeOffset()
        {
            new PipeReadStream(new Pipe()).ReadAsync(Utilities.EmptyByteArray, -1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeReadStreamReadThrowsOnOffsetTooLarge()
        {
            new PipeReadStream(new Pipe()).Read(Utilities.EmptyByteArray, 1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeReadStreamReadAsyncThrowsOnOffsetTooLarge()
        {
            new PipeReadStream(new Pipe()).ReadAsync(Utilities.EmptyByteArray, 1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeReadStreamReadThrowsOnNegativeCount()
        {
            new PipeReadStream(new Pipe()).Read(Utilities.EmptyByteArray, 0, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeReadStreamReadAsyncThrowsOnNegativeCount()
        {
            new PipeReadStream(new Pipe()).ReadAsync(Utilities.EmptyByteArray, 0, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeReadStreamReadThrowsOnCountTooLarge()
        {
            new PipeReadStream(new Pipe()).Read(Utilities.EmptyByteArray, 0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeReadStreamReadAsyncThrowsOnCountTooLarge()
        {
            new PipeReadStream(new Pipe()).ReadAsync(Utilities.EmptyByteArray, 0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void PipeReadStreamCanReadGetThrowsAfterDispose()
        {
            _ = GetDisposedPipeReadStream().CanRead;
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void PipeReadStreamCanWriteGetThrowsAfterDispose()
        {
            _ = GetDisposedPipeReadStream().CanWrite;
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void PipeReadStreamCanSeekGetThrowsAfterDispose()
        {
            _ =  GetDisposedPipeReadStream().CanSeek;
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void PipeReadStreamCanTimeoutGetThrowsAfterDispose()
        {
            _ = GetDisposedPipeReadStream().CanTimeout;
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void PipeReadStreamReadThrowsAfterDispose()
        {
            GetDisposedPipeReadStream().Read(Utilities.EmptyByteArray, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void PipeReadStreamReadAsyncThrowsAfterDispose()
        {
            GetDisposedPipeReadStream().ReadAsync(Utilities.EmptyByteArray, 0, 0);
        }

        private static PipeReadStream GetDisposedPipeReadStream()
        {
            var stream = new PipeReadStream(new Pipe());

            stream.Dispose();

            return stream;
        }
    }
}