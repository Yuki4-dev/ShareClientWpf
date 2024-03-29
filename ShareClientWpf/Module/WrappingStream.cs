﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace ShareClientWpf
{
    public class WrappingStream : Stream
    {
        private Stream m_streamBase;

        public WrappingStream(Stream streamBase)
        {
            m_streamBase = streamBase ?? throw new ArgumentNullException(nameof(streamBase));
        }

        public override bool CanRead => m_streamBase.CanRead;

        public override bool CanSeek => m_streamBase.CanSeek;

        public override bool CanWrite => m_streamBase.CanWrite;

        public override long Length => m_streamBase.Length;

        public override long Position { get => m_streamBase.Position; set => m_streamBase.Position = value; }

        public override void Flush()
        {
            ThrowIfDisposed();
            m_streamBase.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return m_streamBase.Read(buffer, offset, count);
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, System.Threading.CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            return m_streamBase.ReadAsync(buffer, offset, count, cancellationToken);
        }
        public new Task<int> ReadAsync(byte[] buffer, int offset, int count)
        {
            ThrowIfDisposed();
            return m_streamBase.ReadAsync(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            ThrowIfDisposed();
            return m_streamBase.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            ThrowIfDisposed();
            m_streamBase.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            ThrowIfDisposed();
            m_streamBase.Write(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_streamBase.Dispose();
                m_streamBase = null;  //disposeしたら内部ストリームをnullにして参照を外す
            }
            base.Dispose(disposing);
        }
        private void ThrowIfDisposed()
        {
            if (m_streamBase == null)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}
