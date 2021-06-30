using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pipe
{
    public class PipeReadStream : PipeStreamBase
    {
        public PipeReadStream(Pipe pipe)
        : base(pipe)
        { }

        public override bool CanRead
        {
            get
            {
                AssertNotDisposed();

                return true;
            }
        }

        public override bool CanWrite
        {
            get
            {
                AssertNotDisposed();

                return false;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            AssertNotDisposed();

            return pipe.Read(buffer, offset, count);
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            AssertNotDisposed();

            return pipe.ReadAsync(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing)
            {
                return;
            }

            pipe = null;
        }
    }
}
