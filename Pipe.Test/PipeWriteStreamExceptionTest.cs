using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Pipe.Test
{
    [TestClass]
    public class PipeWriteStreamExceptionTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipeWriteStreamCtorThrowsOnNullArgument()
        {
            new PipeWriteStream(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void PipeWriteStreamLengthGetThrows()
        {
            _ = new PipeWriteStream(new Pipe()).Length;
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void PipeWriteStreamSetLengthThrows()
        {
            new PipeWriteStream(new Pipe()).SetLength(0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void PipeWriteStreamPositionGetThrows()
        {
            _ = new PipeWriteStream(new Pipe()).Position;
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void PipeWriteStreamPositionSetThrows()
        {
            new PipeWriteStream(new Pipe()).Position = 0;
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void PipeWriteStreamSeekThrows()
        {
            _ = new PipeWriteStream(new Pipe()).Seek(0, SeekOrigin.Begin);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void PipeWriteStreamReadThrows()
        {
            new PipeWriteStream(new Pipe()).Read(Utilities.EmptyByteArray, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void PipeWriteStreamReadAsyncThrows()
        {
            new PipeWriteStream(new Pipe()).ReadAsync(Utilities.EmptyByteArray, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipeWriteStreamWriteThrowsOnNullBuffer()
        {
            new PipeWriteStream(new Pipe()).Write(null, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipeWriteStreamWriteAsyncThrowsOnNullBuffer()
        {
            new PipeWriteStream(new Pipe()).WriteAsync(null, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeWriteStreamWriteThrowsOnNegativeOffset()
        {
            new PipeWriteStream(new Pipe()).Write(Utilities.EmptyByteArray, -1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeWriteStreamWriteAsyncThrowsOnNegativeOffset()
        {
            new PipeWriteStream(new Pipe()).WriteAsync(Utilities.EmptyByteArray, -1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeWriteStreamWriteThrowsOnOffsetTooLarge()
        {
            new PipeWriteStream(new Pipe()).Write(Utilities.EmptyByteArray, 1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeWriteStreamWriteAsyncThrowsOnOffsetTooLarge()
        {
            new PipeWriteStream(new Pipe()).WriteAsync(Utilities.EmptyByteArray, 1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeWriteStreamWriteThrowsOnNegativeCount()
        {
            new PipeWriteStream(new Pipe()).Write(Utilities.EmptyByteArray, 0, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeWriteStreamWriteAsyncThrowsOnNegativeCount()
        {
            new PipeWriteStream(new Pipe()).WriteAsync(Utilities.EmptyByteArray, 0, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeWriteStreamWriteThrowsOnCountTooLarge()
        {
            new PipeWriteStream(new Pipe()).Write(Utilities.EmptyByteArray, 0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeWriteStreamWriteAsyncThrowsOnCountTooLarge()
        {
            new PipeWriteStream(new Pipe()).WriteAsync(Utilities.EmptyByteArray, 0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void PipeWriteStreamCanReadGetThrowsAfterDispose()
        {
            _ = GetDisposedPipeWriteStream().CanRead;
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void PipeWriteStreamCanWriteGetThrowsAfterDispose()
        {
            _ = GetDisposedPipeWriteStream().CanWrite;
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void PipeWriteStreamCanCanSeekGetThrowsAfterDispose()
        {
            _ = GetDisposedPipeWriteStream().CanSeek;
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void PipeWriteStreamCanTimeoutGetThrowsAfterDispose()
        {
            _ = GetDisposedPipeWriteStream().CanTimeout;
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void PipeWriteStreamWriteThrowsAfterDispose()
        {
            GetDisposedPipeWriteStream().Write(Utilities.EmptyByteArray, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void PipeWriteStreamWriteAsyncThrowsAfterDispose()
        {
            GetDisposedPipeWriteStream().WriteAsync(Utilities.EmptyByteArray, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void PipeWriteStreamFlushThrowsAfterDispose()
        {
            GetDisposedPipeWriteStream().Flush();
        }

        private static PipeWriteStream GetDisposedPipeWriteStream()
        {
            var stream = new PipeWriteStream(new Pipe());

            stream.Dispose();

            return stream;
        }
    }
}