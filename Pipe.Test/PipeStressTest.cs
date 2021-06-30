using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pipe.Test
{
    [TestClass]
    public class PipeStressTest
    {
        // This test tries to expose errors in thread synchronization.
        // This will never be a 100% reliable test, so it should be run
        // multiple times after changes that might affect synchronization.
        [TestMethod]
        public async Task StressTest()
        {
            var pipe = new Pipe();
            var readBuffer = new byte[8192];
            var writeBuffer = Enumerable.Range(0, 256).Select((i) => (byte)i).ToArray();
            var random = new Random();

            var readTask = Task.Run(
                () =>
                {
                    byte sequence = 0;

                    while (true)
                    {
                        int readCount = random.Next(8192) + 1;
                        int read = pipe.Read(readBuffer, 0, readCount);

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

                pipe.Write(writeBuffer, position, writeCount);

                position += (byte)writeCount;
                Debug.WriteLine($"Wrote: {writeCount}");
            }

            pipe.Close();

            await readTask;
        }
    }
}
