using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace Pipe.Test
{
    [TestClass]
    public class PipeOperationTest
    {
        [TestMethod]
        public void PipeReadReturnsZeroAfterClose()
        {
            var pipe = new Pipe();

            pipe.Close();
            Assert.AreEqual(0, pipe.Read(Utilities.OneByteArray, 0, 1));
        }

        [TestMethod]
        public void PipeReadReturnsZeroTwiceAfterClose()
        {
            var pipe = new Pipe();
            var buffer = new byte[1];

            pipe.Close();
            Assert.AreEqual(0, pipe.Read(buffer, 0, buffer.Length));
            Assert.AreEqual(0, pipe.Read(buffer, 0, buffer.Length));
        }

        [TestMethod]
        public void PipeReadReturnsAllRemainingDataThenZeroAfterClose()
        {
            var pipe = new Pipe();
            var buffer = new byte[1];

            pipe.Write(buffer, 0, buffer.Length);
            pipe.Close();
            Assert.AreEqual(1, pipe.Read(buffer, 0, buffer.Length));
            Assert.AreEqual(0, pipe.Read(buffer, 0, buffer.Length));
        }

        [TestMethod]
        public async Task BlockedPipeReadCompletesWithByteCountExceedingPipeBufferSizeAsync()
        {
            var pipe = new Pipe();
            var buffer = new byte[8192 * 2];
            var readCountTask = pipe.ReadAsync(buffer, 0, buffer.Length);

            Assert.IsFalse(readCountTask.IsCompleted);

            pipe.Write(buffer, 0, buffer.Length);

            Assert.AreEqual(buffer.Length, await readCountTask);
        }

        [TestMethod]
        public void PipeReadReturnsNoMoreDataThanRequested()
        {
            var pipe = new Pipe();
            var readBuffer = new byte[10];
            var writeBuffer = Enumerable.Repeat(1, 10).Select((i) => (byte)i).ToArray();

            pipe.Write(writeBuffer, 0, writeBuffer.Length);

            Assert.AreEqual(5, pipe.Read(readBuffer, 0, 5));

            foreach (byte b in readBuffer.Take(5))
            {
                Assert.AreEqual(1, b);
            }

            foreach (byte b in readBuffer.Skip(5))
            {
                Assert.AreEqual(0, b);
            }
        }

        [TestMethod]
        public void PipeReadReturnsOnlyAvailableData()
        {
            var pipe = new Pipe();
            var readBuffer = new byte[10];
            var writeBuffer = Enumerable.Repeat(1, 5).Select((i) => (byte)i).ToArray();

            pipe.Write(writeBuffer, 0, writeBuffer.Length);

            Assert.AreEqual(5, pipe.Read(readBuffer, 0, readBuffer.Length));

            foreach (byte b in readBuffer.Take(5))
            {
                Assert.AreEqual(1, b);
            }

            foreach (byte b in readBuffer.Skip(5))
            {
                Assert.AreEqual(0, b);
            }
        }
    }
}
