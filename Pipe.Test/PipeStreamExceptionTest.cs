using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Pipe.Test
{
    [TestClass]
    public class PipeStreamExceptionTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipeReadStreamCtorThrowsOnNullPipe()
        {
            new PipeReadStream(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipeWriteStreamCtorThrowsOnNullPipe()
        {
            new PipeWriteStream(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PipeReadStreamThrowsOnLengthGet()
        {
            var l = new PipeReadStream(new Pipe()).Length;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PipeReadStreamThrowsOnSetLength()
        {
            new PipeReadStream(new Pipe()).SetLength(0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PipeReadStreamThrowsOnPositionGet()
        {
            var l = new PipeReadStream(new Pipe()).Position;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PipeReadStreamThrowsOnPositionSet()
        {
            new PipeReadStream(new Pipe()).Position = 0;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PipeReadStreamThrowsOnSeek()
        {
            var l = new PipeReadStream(new Pipe()).Seek(0, SeekOrigin.Begin);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void PipeReadStreamThrowsOnWrite()
        {
            new PipeReadStream(new Pipe()).Write(Utilities.EmptyByteArray, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void PipeReadStreamThrowsOnWriteAsync()
        {
            new PipeReadStream(new Pipe()).WriteAsync(Utilities.EmptyByteArray, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PipeWriteStreamThrowsOnLengthGet()
        {
            var l = new PipeWriteStream(new Pipe()).Length;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PipeWriteStreamThrowsOnSetLength()
        {
            new PipeWriteStream(new Pipe()).SetLength(0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PipeWriteStreamThrowsOnPositionGet()
        {
            var l = new PipeWriteStream(new Pipe()).Position;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PipeWriteStreamThrowsOnPositionSet()
        {
            new PipeWriteStream(new Pipe()).Position = 0;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PipeWriteStreamThrowsOnSeek()
        {
            var l = new PipeWriteStream(new Pipe()).Seek(0, SeekOrigin.Begin);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void PipeWriteStreamThrowsOnRead()
        {
            new PipeWriteStream(new Pipe()).Read(Utilities.EmptyByteArray, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void PipeWriteStreamThrowsOnReadAsync()
        {
            new PipeWriteStream(new Pipe()).ReadAsync(Utilities.EmptyByteArray, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipeReadStreamReadThrowsOnNullBuffer()
        {
            new PipeReadStream(new Pipe()).Read(null, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeReadStreamReadThrowsOnNegativeOffset()
        {
            new PipeReadStream(new Pipe()).Read(Utilities.EmptyByteArray, -1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeReadStreamReadThrowsOnOffsetTooLarge()
        {
            new PipeReadStream(new Pipe()).Read(Utilities.EmptyByteArray, 1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeReadStreamReadThrowsOnNegativeCount()
        {
            new PipeReadStream(new Pipe()).Read(Utilities.EmptyByteArray, 0, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeReadStreamReadThrowsOnCountTooLarge()
        {
            new PipeReadStream(new Pipe()).Read(Utilities.EmptyByteArray, 0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PipeReadStreamReadThrowsOnReadInProgress()
        {
            Pipe pipe = new Pipe();
            PipeReadStream pipeReadStream = new PipeReadStream(pipe);

            pipe.ReadAsync(Utilities.OneByteArray, 0, 1);
            pipe.Read(Utilities.EmptyByteArray, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipeReadStreamReadAsyncThrowsOnNullBuffer()
        {
            new PipeReadStream(new Pipe()).ReadAsync(null, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeReadStreamReadAsyncThrowsOnNegativeOffset()
        {
            new PipeReadStream(new Pipe()).ReadAsync(Utilities.EmptyByteArray, -1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeReadStreamReadAsyncThrowsOnOffsetTooLarge()
        {
            new PipeReadStream(new Pipe()).ReadAsync(Utilities.EmptyByteArray, 1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeReadStreamReadAsyncThrowsOnNegativeCount()
        {
            new PipeReadStream(new Pipe()).ReadAsync(Utilities.EmptyByteArray, 0, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeReadStreamReadAsyncThrowsOnCountTooLarge()
        {
            new PipeReadStream(new Pipe()).ReadAsync(Utilities.EmptyByteArray, 0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PipeReadStreamReadAsyncThrowsOnReadInProgress()
        {
            Pipe pipe = new Pipe();
            PipeReadStream pipeReadStream = new PipeReadStream(pipe);

            pipeReadStream.ReadAsync(Utilities.OneByteArray, 0, 1);
            pipeReadStream.ReadAsync(Utilities.EmptyByteArray, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipeWriteStreamWriteThrowsOnNullBuffer()
        {
            new PipeWriteStream(new Pipe()).Write(null, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeWriteStreamWriteThrowsOnNegativeOffset()
        {
            new PipeWriteStream(new Pipe()).Write(Utilities.EmptyByteArray, -1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeWriteStreamWriteThrowsOnOffsetTooLarge()
        {
            new PipeWriteStream(new Pipe()).Write(Utilities.EmptyByteArray, 1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeWriteStreamWriteThrowsOnNegativeCount()
        {
            new PipeWriteStream(new Pipe()).Write(Utilities.EmptyByteArray, 0, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeWriteStreamWriteThrowsOnCountTooLarge()
        {
            new PipeWriteStream(new Pipe()).Write(Utilities.EmptyByteArray, 0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void PipeWriteStreamWriteThrowsOnWriteAfterDispose()
        {
            Pipe pipe = new Pipe();
            PipeWriteStream pipeWriteStream = new PipeWriteStream(pipe);

            pipeWriteStream.Dispose();
            pipeWriteStream.Write(Utilities.EmptyByteArray, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipeWriteStreamWriteAsyncThrowsOnNullBuffer()
        {
            new PipeWriteStream(new Pipe()).WriteAsync(null, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeWriteStreamWriteAsyncThrowsOnNegativeOffset()
        {
            new PipeWriteStream(new Pipe()).WriteAsync(Utilities.EmptyByteArray, -1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeWriteStreamWriteAsyncThrowsOnOffsetTooLarge()
        {
            new PipeWriteStream(new Pipe()).WriteAsync(Utilities.EmptyByteArray, 1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeWriteStreamWriteAsyncThrowsOnNegativeCount()
        {
            new PipeWriteStream(new Pipe()).WriteAsync(Utilities.EmptyByteArray, 0, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeWriteStreamWriteAsyncThrowsOnCountTooLarge()
        {
            new PipeWriteStream(new Pipe()).WriteAsync(Utilities.EmptyByteArray, 0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void PipeWriteStreamWriteAsyncThrowsOnWriteAfterDispose()
        {
            Pipe pipe = new Pipe();
            PipeWriteStream pipeWriteStream = new PipeWriteStream(pipe);

            pipeWriteStream.Dispose();
            pipeWriteStream.WriteAsync(Utilities.EmptyByteArray, 0, 0);
        }
    }
}