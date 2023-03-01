using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Pipe.Test
{
    [TestClass]
    public class PipeStreamOperationTest
    {
        [TestMethod]
        public void PipeReadStreamReadReturnsZeroAfterWritePipeDisposed()
        {
            var pipe = new Pipe();
            var buffer = new byte[1];

            using (var readStream = new PipeReadStream(pipe))
            {
                using (var writeStream = new PipeWriteStream(pipe)) { }

                Assert.AreEqual(0, readStream.Read(buffer, 0, buffer.Length));
            }
        }

        [TestMethod]
        public void PipeReadStreamReadReturnsZeroTwiceAfterWritePipeDisposed()
        {
            var pipe = new Pipe();
            var buffer = new byte[1];

            using (var readStream = new PipeReadStream(pipe))
            {
                using (var writeStream = new PipeWriteStream(pipe)) { }

                Assert.AreEqual(0, readStream.Read(buffer, 0, buffer.Length));
                Assert.AreEqual(0, readStream.Read(buffer, 0, buffer.Length));
            }
        }

        [TestMethod]
        public void PipeReadStreamReadReturnsAllRemainingDataThenZeroAfterWritePipeDisposed()
        {
            var pipe = new Pipe();
            var buffer = new byte[1];

            using (var readStream = new PipeReadStream(pipe))
            {
                using (var writeStream = new PipeWriteStream(pipe))
                {
                    writeStream.Write(buffer, 0, buffer.Length);
                }

                Assert.AreEqual(1, readStream.Read(buffer, 0, buffer.Length));
                Assert.AreEqual(0, readStream.Read(buffer, 0, buffer.Length));
            }
        }

        [TestMethod]
        public async Task BlockedReadStreamReadCompletesWithByteCountExceedingPipeBufferSizeAsync()
        {
            var pipe = new Pipe();
            var buffer = new byte[8192 * 2];

            using (var readStream = new PipeReadStream(pipe))
            {
                var readCountTask = readStream.ReadAsync(buffer, 0, buffer.Length);

                Assert.IsFalse(readCountTask.IsCompleted);

                using (var writeStream = new PipeWriteStream(pipe))
                {
                    writeStream.Write(buffer, 0, buffer.Length);
                }

                Assert.AreEqual(buffer.Length, await readCountTask);
            }
        }
    }
}
