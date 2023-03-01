using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Pipe
{
    public class Pipe
    {
        private enum CallerType
        {
            None,
            Reader,
            Writer
        }

        private static readonly Task<int> zeroByteCompletedTask = Task.FromResult(0);

        private readonly object readWriteExclusionLock;

        private int bufferSize;
        private int maximumByteCount;
        private LinkedList<byte[]> buffers;
        private int readBufferPosition;
        private int writeBufferPosition;
        private int byteCount;
        private byte[] callersBuffer;
        private int callersBufferOffset;
        private int callersBufferCount;
        private TaskCompletionSource<int> callersCompletionSource;
        private CallerType callerType;
        private Action ensureBuffersFunction;
        private bool isEofSet;

        public Pipe(int bufferSize = 8192, int maximumByteCount = 0)
        {
            if (bufferSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            }

            if (maximumByteCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maximumByteCount));
            }

            this.bufferSize = bufferSize;
            this.maximumByteCount = maximumByteCount;

            readWriteExclusionLock = new object();
            ensureBuffersFunction = EnsureBuffers;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            CheckReadWriteParameters(buffer, offset, count, CallerType.Reader);

            if (callerType == CallerType.Reader && callersCompletionSource != null)
            {
                throw new InvalidOperationException("Read operation already in progress");
            }

            Task<int> readTask = SetupBlockedReadIfNeeded(buffer, offset, count);

            if (readTask != null)
            {
                return readTask.Result;
            }

            return this.CopyToReadBuffer(buffer, offset, count);
        }

        public Task<int> ReadAsync(byte[] buffer, int offset, int count)
        {
            CheckReadWriteParameters(buffer, offset, count, CallerType.Reader);

            if (callerType == CallerType.Reader && callersCompletionSource != null)
            {
                return Task.FromException<int>(new InvalidOperationException("Read operation already in progress"));
            }

            Task<int> readTask = SetupBlockedReadIfNeeded(buffer, offset, count);
            
            if (readTask != null)
            {
                return readTask;
            }

            return Task.FromResult(CopyToReadBuffer(buffer, offset, count));
        }

        private Task<int> SetupBlockedReadIfNeeded(byte[] buffer, int offset, int count)
        {
            if (count == 0)
            {
                return zeroByteCompletedTask;
            }

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
                    if (isEofSet)
                    {
                        return zeroByteCompletedTask;
                    }
                    else
                    {
                        callersBuffer = buffer;
                        callersBufferOffset = offset;
                        callersBufferCount = count;
                        callersCompletionSource = new TaskCompletionSource<int>();
                        callerType = CallerType.Reader;

                        return callersCompletionSource.Task;
                    }
                }
            }

            return null;
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

                Array.Copy(readBuffer, readBufferPosition, buffer, offset, bytesToCopy);
                readBufferPosition += bytesToCopy;
                byteCount -= bytesToCopy;

                if (readBufferPosition == bufferSize)
                {
                    readBufferPosition = 0;
                    buffers.RemoveFirst();
                }

                if (callersCompletionSource != null)
                {
                    if (byteCount + callersBufferCount <= maximumByteCount)
                    {
                        CopyFromWriteBuffer(callersBuffer, callersBufferOffset, callersBufferCount);

                        callersBuffer = null;
                        callersBufferOffset = 0;
                        callersBufferCount = 0;
                        callersCompletionSource = null;
                    }
                }

                return bytesToCopy;
            }
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            CheckReadWriteParameters(buffer, offset, count, CallerType.Writer);

            if (callerType == CallerType.Writer && callersCompletionSource != null)
            {
                throw new InvalidOperationException("Write operation already in progress");
            }

            if (isEofSet)
            {
                throw new EndOfStreamException("Pipe is closed");
            }

            this.CopyFromWriteBuffer(buffer, offset, count);
        }

        public Task WriteAsync(byte[] buffer, int offset, int count)
        {
            CheckReadWriteParameters(buffer, offset, count, CallerType.Writer);

            if (callerType == CallerType.Writer && callersCompletionSource != null)
            {
                return Task.FromException(new InvalidOperationException("Write operation already in progress"));
            }

            if (isEofSet)
            {
                return Task.FromException(new EndOfStreamException("Pipe is closed"));
            }

            Task<int> writeTask = SetupBlockedWriteIfNeeded(buffer, offset, count);

            if (writeTask != null)
            {
                return writeTask;
            }

            this.CopyFromWriteBuffer(buffer, offset, count);

            return Task.CompletedTask;
        }

        private Task<int> SetupBlockedWriteIfNeeded(byte[] buffer, int offset, int count)
        {
            if (count == 0)
            {
                return zeroByteCompletedTask;
            }

            lock (readWriteExclusionLock)
            {
                if (
                    byteCount + count > maximumByteCount
                )
                {
                    callersBuffer = buffer;
                    callersBufferOffset = offset;
                    callersBufferCount = count;
                    callersCompletionSource = new TaskCompletionSource<int>();
                    callerType = CallerType.Writer;

                    return callersCompletionSource.Task;
                }
            }

            return null;
        }

        private void CopyFromWriteBuffer(byte[] buffer, int offset, int count)
        {
            int bytesToCopy;

            while (count > 0)
            {
                lock (readWriteExclusionLock)
                {
                    if (callersCompletionSource != null)
                    {
                        bytesToCopy = Math.Min(count, callersBufferCount);
                        Array.Copy(buffer, offset, callersBuffer, callersBufferOffset, bytesToCopy);
                        count -= bytesToCopy;
                        offset += bytesToCopy;
                        callersBuffer = null;
                        callersBufferOffset = 0;
                        callersBufferCount = 0;

                        TaskCompletionSource<int> completionSource = callersCompletionSource;

                        callersCompletionSource = null;
                        completionSource.SetResult(bytesToCopy);
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
                        byteCount += bytesToCopy;
                    }
                }
            }
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
                    callersCompletionSource.SetResult(0);
                }
            }
        }

        private void CheckReadWriteParameters(byte[] buffer, int offset, int count, CallerType callerType)
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

            if (callerType == CallerType.Writer)
            {
                if (
                    maximumByteCount > 0 &&
                    count > maximumByteCount
                )
                {
                    throw new ArgumentOutOfRangeException(nameof(count), "Byte count exceeds the pipe's maximum byte count");
                }
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
            if (buffers.Count == 0 || writeBufferPosition == bufferSize)
            {
                buffers.AddLast(new byte[bufferSize]);
                writeBufferPosition = 0;
            }
        }
    }
}
