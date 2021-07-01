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
            var readBuffer = new byte[256 * 52];
            var writeBuffer = Enumerable.Range(0, 256 * 56).Select((i) => (byte)i).ToArray();
            var random = new Random(5);

            var readTask = Task.Run(
                () =>
                {
                    var random = new Random(10);
                    byte sequence = 0;

                    while (true)
                    {
                        int readCount = random.Next(1, 8192);
                        int read = pipe.Read(readBuffer, 0, readCount);

                        Debug.WriteLine($"Tried: {readCount}, Read: {read}");

                        if (read == 0)
                        {
                            // This should mean the write loop ended and closed the pipe.
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

            int position = 0;

            for (int i = 0; i < 10000; ++i)
            {
                int writeCount = random.Next(1, writeBuffer.Length - position);

                pipe.Write(writeBuffer, position, writeCount);

                if ((position += writeCount) == writeBuffer.Length)
                {
                    position = 0;
                }

                Debug.WriteLine($"Wrote: {writeCount}");

                if (readTask.IsCompleted)
                {
                    // Task should not complete until after pipe.Close is called.
                    // Something is wrong. We'll get the exception below.
                    break;
                }
            }

            pipe.Close();

            await readTask;
        }
    }
}
