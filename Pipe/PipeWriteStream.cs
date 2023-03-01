using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pipe
{
    public class PipeWriteStream : PipeStreamBase
    {
        public PipeWriteStream(Pipe pipe)
        : base(pipe)
        { }

        public override bool CanRead
        {
            get
            {
                AssertNotDisposed();

                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                AssertNotDisposed();

                return true;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            AssertNotDisposed();

            pipe.Write(buffer, offset, count);
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            AssertNotDisposed();

            return pipe.WriteAsync(buffer, offset, count);
        }

        public override void Flush()
        {
            AssertNotDisposed();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing)
            {
                return;
            }

            Pipe pipe = this.pipe;

            this.pipe = null;

            if (pipe != null)
            {
                pipe.Close();
            }
        }
    }
}
