using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pipe.Test
{
    [TestClass]
    public class PipeOperationTest
    {
        [TestMethod]
        public void PipeReadReturnsZeroAfterClose()
        {
            Pipe pipe = new Pipe();

            pipe.Close();
            Assert.AreEqual(0, pipe.Read(Utilities.OneByteArray, 0, 1));
        }

        [TestMethod]
        public void PipeReadReturnsZeroTwiceAfterClose()
        {
            Pipe pipe = new Pipe();

            pipe.Close();
            Assert.AreEqual(0, pipe.Read(Utilities.OneByteArray, 0, 1));
            Assert.AreEqual(0, pipe.Read(Utilities.OneByteArray, 0, 1));
        }
    }
}
