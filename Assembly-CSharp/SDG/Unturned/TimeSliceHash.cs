using System;
using System.IO;
using System.Security.Cryptography;

namespace SDG.Unturned
{
	/// <summary>
	/// Utility to hash a stream of bytes over several frames.
	/// </summary>
	// Token: 0x020007FA RID: 2042
	public class TimeSliceHash<T> : IDisposable where T : HashAlgorithm, new()
	{
		// Token: 0x06004623 RID: 17955 RVA: 0x001A2FC8 File Offset: 0x001A11C8
		public TimeSliceHash(Stream stream)
		{
			this.algo = Activator.CreateInstance<T>();
			this.algo.Initialize();
			this.stream = stream;
			this.buffer = new byte[8192];
		}

		/// <summary>
		/// [0, 1] percentage progress through the stream.
		/// </summary>
		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x06004624 RID: 17956 RVA: 0x001A3002 File Offset: 0x001A1202
		public float progress
		{
			get
			{
				return (float)((double)this.stream.Position / (double)this.stream.Length);
			}
		}

		/// <summary>
		/// Advance 1MB further into the stream.
		/// </summary>
		/// <returns>True if there is more data, false if complete.</returns>
		// Token: 0x06004625 RID: 17957 RVA: 0x001A3020 File Offset: 0x001A1220
		public bool advance()
		{
			for (int i = 0; i < 122; i++)
			{
				int num = this.stream.Read(this.buffer, 0, this.buffer.Length);
				if (num <= 0)
				{
					this.algo.TransformFinalBlock(this.buffer, 0, 0);
					return false;
				}
				this.algo.TransformBlock(this.buffer, 0, num, this.buffer, 0);
			}
			return true;
		}

		// Token: 0x06004626 RID: 17958 RVA: 0x001A3097 File Offset: 0x001A1297
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06004627 RID: 17959 RVA: 0x001A30A6 File Offset: 0x001A12A6
		protected virtual void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			if (disposing)
			{
				this.algo.Dispose();
			}
			this.disposed = true;
		}

		/// <summary>
		/// Get the computed hash after processing stream.
		/// </summary>
		// Token: 0x06004628 RID: 17960 RVA: 0x001A30CB File Offset: 0x001A12CB
		public byte[] computeHash()
		{
			return this.algo.Hash;
		}

		// Token: 0x04002F34 RID: 12084
		private T algo;

		// Token: 0x04002F35 RID: 12085
		private Stream stream;

		// Token: 0x04002F36 RID: 12086
		private byte[] buffer;

		// Token: 0x04002F37 RID: 12087
		private bool disposed;
	}
}
