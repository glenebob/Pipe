using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pipe.Test
{
    [TestClass]
    public class StressTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Pipe pipe = new Pipe();

            using (PipeReadStream pipeReadStream = new PipeReadStream(pipe))
            using (PipeWriteStream pipeWriteStream = new PipeWriteStream(pipe))
            {
                byte[] readBuffer = new byte[8192];
                byte[] writeBuffer = Enumerable.Range(0, 256).Select((i) => (byte)i).ToArray();
                Random random = new Random();

                Task.Run(
                    () =>
                    {
                        byte sequence = 0;

                        while (true)
                        {
                            int readCount = random.Next(8192) + 1;
                            int read = pipeReadStream.Read(readBuffer, 0, readCount);

                            Debug.WriteLine($"Tried: {readCount}, Read: {read}");

                            if (read == 0)
                            {
                                Debug.WriteLine($"<End of stream>");

                                return;
                            }

                            for (int i = 0; i < read; i++)
                            {
                                Assert.AreEqual(sequence++, readBuffer[i]);
                            }
                        }
                    }
                );

                byte position = 0;

                for (int i = 0; i < 500; ++i)
                {
                    int writeCount = (byte)random.Next(writeBuffer.Length - position) + 1;

                    pipeWriteStream.Write(writeBuffer, position, writeCount);

                    position += (byte)writeCount;
                    Debug.WriteLine($"Wrote: {writeCount}");
                }
            }
        }
    }
}
