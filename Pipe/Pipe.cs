using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace Pipe
{
    public class Pipe
    {
        private static int bufferSize = 8192;

        private LinkedList<byte[]> buffers;
        private int readBufferPosition;
        private int writeBufferPosition;
        private object readWriteExclusionLock;
        private byte[] callersBuffer;
        private int callersBufferOffset;
        private int callersBufferCount;
        private TaskCompletionSource<int> readCompletionSource;
        private Action ensureBuffersFunction;
        private bool isEofSet;

        public Pipe()
        {
            readWriteExclusionLock = new object();
            ensureBuffersFunction = EnsureBuffers;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            CheckReadWriteParameters(buffer, offset, count);

            if (readCompletionSource != null)
            {
                throw new InvalidOperationException("Read operation already in progress");
            }

            if (count == 0)
            {
                return 0;
            }

            if (SetupDirectWriteIfNeeded(buffer, offset, count))
            {
                return readCompletionSource.Task.Result;
            }

            return this.CopyToReadBuffer(buffer, offset, count);
        }

        public Task<int> ReadAsync(byte[] buffer, int offset, int count)
        {
            CheckReadWriteParameters(buffer, offset, count);

            if (readCompletionSource != null)
            {
                throw new InvalidOperationException("Read operation already in progress");
            }

            if (count == 0)
            {
                return Task.FromResult(0);
            }

            if (SetupDirectWriteIfNeeded(buffer, offset, count))
            {
                return readCompletionSource.Task;
            }

            return Task.FromResult(CopyToReadBuffer(buffer, offset, count));
        }

        private bool SetupDirectWriteIfNeeded(byte[] buffer, int offset, int count)
        {
            lock (readWriteExclusionLock)
            {
                if (
                    buffers == null ||
                    buffers.Count == 0 || (
                       buffers.Count == 1 &&
                       readBufferPosition == writeBufferPosition
                    )
                )
                {
                    readCompletionSource = new TaskCompletionSource<int>();

                    if (isEofSet)
                    {
                        readCompletionSource.SetResult(0);
                    }
                    else
                    {
                        callersBuffer = buffer;
                        callersBufferOffset = offset;
                        callersBufferCount = count;
                    }

                    return true;
                }
            }

            return false;
        }

        private int CopyToReadBuffer(byte[] buffer, int offset, int count)
        {
            int bytesToCopy;
            byte[] readBuffer = buffers.First.Value;

            lock (readWriteExclusionLock)
            {
                if (buffers.Count == 1)
                {
                    bytesToCopy = Math.Min(count, writeBufferPosition - readBufferPosition);
                }
                else
                {
                    bytesToCopy = Math.Min(count, readBuffer.Length - readBufferPosition);
                }
            }

            Array.Copy(readBuffer, readBufferPosition, buffer, offset, bytesToCopy);

            readBufferPosition += bytesToCopy;

            if (readBufferPosition == bufferSize)
            {
                readBufferPosition = 0;

                lock (readWriteExclusionLock)
                {
                    buffers.RemoveFirst();
                }
            }

            return bytesToCopy;
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            CheckReadWriteParameters(buffer, offset, count);

            if (isEofSet)
            {
                throw new EndOfStreamException("Pipe is closed");
            }

            int bytesToCopy;

            while (count > 0)
            {
                lock (readWriteExclusionLock)
                {
                    if (callersBuffer != null)
                    {
                        bytesToCopy = Math.Min(count, callersBufferCount);
                        Array.Copy(buffer, offset, callersBuffer, callersBufferOffset, bytesToCopy);
                        count -= bytesToCopy;
                        callersBuffer = null;
                        callersBufferOffset = 0;
                        callersBufferCount = 0;

                        TaskCompletionSource<int> completionSource = readCompletionSource;

                        readCompletionSource = null;
                        completionSource.SetResult(bytesToCopy);

                        if (count == 0)
                        {
                            return;
                        }

                        offset += bytesToCopy;
                    }
                    else
                    {
                        ensureBuffersFunction();
                        EnsureWriteBuffer();

                        byte[] writeBuffer = buffers.Last.Value;

                        bytesToCopy = Math.Min(count, writeBuffer.Length - writeBufferPosition);

                        Array.Copy(buffer, offset, writeBuffer, writeBufferPosition, bytesToCopy);
                        writeBufferPosition += bytesToCopy;
                        offset += bytesToCopy;
                        count -= bytesToCopy;
                    }
                }
            }
        }

        public Task WriteAsync(byte[] buffer, int offset, int count)
        {
            Write(buffer, offset, count);

            return Task.CompletedTask;
        }

        public void Close()
        {
            if (isEofSet)
            {
                return;
            }

            lock (readWriteExclusionLock)
            {
                isEofSet = true;

                if (callersBuffer != null)
                {
                    readCompletionSource.SetResult(0);
                }
            }
        }

        private void CheckReadWriteParameters(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (
                offset < 0 || (
                    offset > 0 &&
                    offset >= buffer.Length
                )
            )
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if (count < 0 || offset + count > buffer.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
        }

        private void EnsureBuffers()
        {
            if (buffers == null)
            {
                buffers = new LinkedList<byte[]>();
            }

            ensureBuffersFunction = EnsureBuffersStub;
        }

        private void EnsureBuffersStub()
        { }

        private void EnsureWriteBuffer()
        {
            lock (readWriteExclusionLock)
            {
                if (buffers.Count == 0 || writeBufferPosition == bufferSize)
                {
                    buffers.AddLast(new byte[bufferSize]);
                    writeBufferPosition = 0;
                }
            }
        }
    }
}
