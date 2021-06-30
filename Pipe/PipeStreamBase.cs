using System;
using System.IO;

namespace Pipe
{
    public abstract class PipeStreamBase : Stream
    {
        protected Pipe pipe;

        protected PipeStreamBase(Pipe pipe)
        {
            if (pipe == null)
            {
                throw new ArgumentNullException(nameof(pipe));
            }

            this.pipe = pipe;
        }

        public override bool CanSeek
        {
            get
            {
                AssertNotDisposed();

                return false;
            }
        }

        public override bool CanTimeout
        {
            get
            {
                AssertNotDisposed();

                return false;
            }
        }

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        protected void AssertNotDisposed()
        {
            if (pipe == null)
            {
                throw new ObjectDisposedException(nameof(Pipe));
            }
        }
    }
}
