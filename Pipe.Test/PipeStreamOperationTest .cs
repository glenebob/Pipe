using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pipe.Test
{
    [TestClass]
    public class PipeStreamOperationTest
    {
        [TestMethod]
        public void PipeReadStreamReadReturnsZeroAfterWritePipeDisposed()
        {
            var pipe = new Pipe();
            var pipeReadStream = new PipeReadStream(pipe);
            var pipeWriteStream = new PipeWriteStream(pipe);

            pipeWriteStream.Dispose();
            Assert.AreEqual(0, pipeReadStream.Read(Utilities.OneByteArray, 0, 1));
        }

        [TestMethod]
        public void PipeReadStreamReadReturnsZeroTwiceAfterWritePipeDisposed()
        {
            var pipe = new Pipe();
            var pipeReadStream = new PipeReadStream(pipe);
            var pipeWriteStream = new PipeWriteStream(pipe);

            pipeWriteStream.Dispose();
            Assert.AreEqual(0, pipeReadStream.Read(Utilities.OneByteArray, 0, 1));
            Assert.AreEqual(0, pipeReadStream.Read(Utilities.OneByteArray, 0, 1));
        }
    }
}
