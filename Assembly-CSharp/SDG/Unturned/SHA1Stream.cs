using System;
using System.IO;
using System.Security.Cryptography;

namespace SDG.Unturned
{
	// Token: 0x020007FC RID: 2044
	public class SHA1Stream : HashStream
	{
		// Token: 0x06004636 RID: 17974 RVA: 0x001A31FD File Offset: 0x001A13FD
		public SHA1Stream(Stream underlyingStream) : base(underlyingStream, new SHA1Managed())
		{
		}
	}
}
