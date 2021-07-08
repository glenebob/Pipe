using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Pipe.Test
{
    [TestClass]
    public class PipeExceptionTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeCtorThrowsOnNonPositiveBufferSize()
        {
            new Pipe(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeCtorThrowsOnNegativeMaximiumByteCount()
        {
            new Pipe(1, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipeReadThrowsOnNullBuffer()
        {
            new Pipe().Read(null, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeReadThrowsOnNegativeOffset()
        {
            new Pipe().Read(Utilities.EmptyByteArray, -1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeReadThrowsOnOffsetTooLarge()
        {
            new Pipe().Read(Utilities.EmptyByteArray, 1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeReadThrowsOnNegativeCount()
        {
            new Pipe().Read(Utilities.EmptyByteArray, 0, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeReadThrowsOnCountTooLarge()
        {
            new Pipe().Read(Utilities.EmptyByteArray, 0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PipeReadThrowsOnReadInProgress()
        {
            var pipe = new Pipe();
            var buffer = new byte[1];

            pipe.ReadAsync(buffer, 0, buffer.Length);
            pipe.Read(buffer, 0, buffer.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipeReadAsyncThrowsOnNullBuffer()
        {
            new Pipe().ReadAsync(null, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeReadAsyncThrowsOnNegativeOffset()
        {
            new Pipe().ReadAsync(Utilities.EmptyByteArray, -1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeReadAsyncThrowsOnOffsetTooLarge()
        {
            new Pipe().ReadAsync(Utilities.EmptyByteArray, 1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeReadAsyncThrowsOnNegativeCount()
        {
            new Pipe().ReadAsync(Utilities.EmptyByteArray, 0, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeReadAsyncThrowsOnCountTooLarge()
        {
            new Pipe().ReadAsync(Utilities.EmptyByteArray, 0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task PipeReadAsyncThrowsOnReadInProgress()
        {
            var pipe = new Pipe();
            var buffer = new byte[1];

            var t = pipe.ReadAsync(buffer, 0, buffer.Length);
            await pipe.ReadAsync(buffer, 0, buffer.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipeWriteThrowsOnNullBuffer()
        {
            new Pipe().Write(null, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeWriteThrowsOnNegativeOffset()
        {
            new Pipe().Write(Utilities.EmptyByteArray, -1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeWriteThrowsOnOffsetTooLarge()
        {
            new Pipe().Write(Utilities.EmptyByteArray, 1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeWriteThrowsOnNegativeCount()
        {
            new Pipe().Write(Utilities.EmptyByteArray, 0, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeWriteThrowsOnCountTooLarge()
        {
            new Pipe().Write(Utilities.EmptyByteArray, 0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void PipeWriteThrowsOnWriteAfterClose()
        {
            var pipe = new Pipe();

            pipe.Close();
            pipe.Write(Utilities.EmptyByteArray, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipeWriteAsyncThrowsOnNullBuffer()
        {
            new Pipe().WriteAsync(null, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeWriteAsyncThrowsOnNegativeOffset()
        {
            new Pipe().WriteAsync(Utilities.EmptyByteArray, -1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeWriteAsyncThrowsOnOffsetTooLarge()
        {
            new Pipe().WriteAsync(Utilities.EmptyByteArray, 1, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeWriteAsyncThrowsOnNegativeCount()
        {
            new Pipe().WriteAsync(Utilities.EmptyByteArray, 0, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeWriteAsyncThrowsOnCountTooLarge()
        {
            new Pipe().WriteAsync(Utilities.EmptyByteArray, 0, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public async Task PipeWriteAsyncThrowsOnWriteAfterClose()
        {
            var pipe = new Pipe();

            pipe.Close();
            await pipe.WriteAsync(Utilities.EmptyByteArray, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task PipeWriteAsyncThrowsOnWriteInProgress()
        {
            var pipe = new Pipe();
            var buffer = new byte[1];

            var t = pipe.WriteAsync(buffer, 0, buffer.Length);
            await pipe.WriteAsync(buffer, 0, buffer.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PipeWriteThrowsOnByteCountExceedsMaximum()
        {
            var pipe = new Pipe(1, 1);
            var buffer = new byte[2];

            pipe.Write(buffer, 0, buffer.Length);
        }
    }
}
