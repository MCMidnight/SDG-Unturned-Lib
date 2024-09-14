using System;
using System.IO;
using System.Security.Cryptography;

namespace SDG.Unturned
{
	/// <summary>
	/// Run hash algorithm for all data passing through a stream.
	/// </summary>
	// Token: 0x020007FB RID: 2043
	public class HashStream : Stream
	{
		// Token: 0x06004629 RID: 17961 RVA: 0x001A30DD File Offset: 0x001A12DD
		public HashStream(Stream underlyingStream, HashAlgorithm hashAlgo)
		{
			this.underlyingStream = underlyingStream;
			this.hashAlgo = hashAlgo;
		}

		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x0600462A RID: 17962 RVA: 0x001A30F3 File Offset: 0x001A12F3
		public byte[] Hash
		{
			get
			{
				this.hashAlgo.TransformFinalBlock(new byte[0], 0, 0);
				return this.hashAlgo.Hash;
			}
		}

		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x0600462B RID: 17963 RVA: 0x001A3114 File Offset: 0x001A1314
		public override bool CanRead
		{
			get
			{
				return this.underlyingStream.CanRead;
			}
		}

		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x0600462C RID: 17964 RVA: 0x001A3121 File Offset: 0x001A1321
		public override bool CanSeek
		{
			get
			{
				return this.underlyingStream.CanSeek;
			}
		}

		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x0600462D RID: 17965 RVA: 0x001A312E File Offset: 0x001A132E
		public override bool CanWrite
		{
			get
			{
				return this.underlyingStream.CanWrite;
			}
		}

		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x0600462E RID: 17966 RVA: 0x001A313B File Offset: 0x001A133B
		public override long Length
		{
			get
			{
				return this.underlyingStream.Length;
			}
		}

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x0600462F RID: 17967 RVA: 0x001A3148 File Offset: 0x001A1348
		// (set) Token: 0x06004630 RID: 17968 RVA: 0x001A3155 File Offset: 0x001A1355
		public override long Position
		{
			get
			{
				return this.underlyingStream.Position;
			}
			set
			{
				if (value == 0L)
				{
					this.hashAlgo.Initialize();
				}
				this.underlyingStream.Position = value;
			}
		}

		// Token: 0x06004631 RID: 17969 RVA: 0x001A3171 File Offset: 0x001A1371
		public override void Flush()
		{
			this.underlyingStream.Flush();
		}

		// Token: 0x06004632 RID: 17970 RVA: 0x001A3180 File Offset: 0x001A1380
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = this.underlyingStream.Read(buffer, offset, count);
			this.hashAlgo.TransformBlock(buffer, offset, num, buffer, offset);
			return num;
		}

		// Token: 0x06004633 RID: 17971 RVA: 0x001A31AE File Offset: 0x001A13AE
		public override long Seek(long offset, SeekOrigin origin)
		{
			if (origin == null && offset == 0L)
			{
				this.hashAlgo.Initialize();
			}
			return this.underlyingStream.Seek(offset, origin);
		}

		// Token: 0x06004634 RID: 17972 RVA: 0x001A31CE File Offset: 0x001A13CE
		public override void SetLength(long value)
		{
			this.underlyingStream.SetLength(value);
		}

		// Token: 0x06004635 RID: 17973 RVA: 0x001A31DC File Offset: 0x001A13DC
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.underlyingStream.Write(buffer, offset, count);
			this.hashAlgo.TransformBlock(buffer, offset, count, buffer, offset);
		}

		// Token: 0x04002F38 RID: 12088
		private Stream underlyingStream;

		// Token: 0x04002F39 RID: 12089
		private HashAlgorithm hashAlgo;
	}
}
