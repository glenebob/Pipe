using System;

namespace Pipe
{
    public class PipeWriteStream : PipeStreamBase
    {
        public PipeWriteStream(Pipe pipe)
        : base(pipe)
        { }

        public override bool CanRead => true;
        public override bool CanWrite => false;

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            pipe.Write(buffer, offset, count);
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
