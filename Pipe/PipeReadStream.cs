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

        public override bool CanRead => true;
        public override bool CanWrite => false;

        public override int Read(byte[] buffer, int offset, int count)
        {
            return pipe.Read(buffer, offset, count);
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return pipe.ReadAsync(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}
