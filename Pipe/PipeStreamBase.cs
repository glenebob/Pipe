using System;
using System.IO;

namespace Pipe
{
    public abstract class PipeStreamBase : Stream
    {
        protected Pipe pipe;

        protected PipeStreamBase(Pipe pipe)
        {
            this.pipe = pipe;
        }

        public override bool CanSeek => false;

        public override bool CanTimeout => false;

        public override long Length => throw new InvalidOperationException();

        public override long Position
        {
            get => throw new InvalidOperationException();
            set => throw new InvalidOperationException();
        }

        public override void Flush()
        { }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new InvalidOperationException();
        }

        public override void SetLength(long value)
        {
            throw new InvalidOperationException();
        }
    }
}
