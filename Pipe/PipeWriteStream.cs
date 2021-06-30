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

        public override bool CanRead => false;
        public override bool CanWrite => true;

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            pipe.Write(buffer, offset, count);
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return pipe.WriteAsync(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (true)
            {
                pipe.Close();
            }
        }
    }
}
