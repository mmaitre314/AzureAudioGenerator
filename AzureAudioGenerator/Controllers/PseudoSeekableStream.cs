using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AzureAudioGenerator.Controllers
{
    // Allow the stream given by PushStreamContent to be wrapped in an IRandomAccessStream
    class PseudoSeekableStream : Stream
    {
        Stream m_stream;
        long m_position;

        public PseudoSeekableStream(Stream stream)
        {
            m_stream = stream;
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            try
            {
                m_stream.Flush();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Flush exception");
            }
        }

        public override long Length
        {
            get { return m_position; }
        }

        public override long Position
        {
            get
            {
                return m_position;
            }
            set
            {
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return 0;
        }

        public override void SetLength(long value)
        {
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            try
            {
                m_stream.Write(buffer, offset, count);
                m_position += count;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Write exception");
            }
        }
    }
}
