using System;
using System.IO;

namespace SDG.Framework.IO.Deserialization
{
	// Token: 0x020000E4 RID: 228
	public interface IDeserializer
	{
		// Token: 0x0600059F RID: 1439
		T deserialize<T>(byte[] data, int offset);

		// Token: 0x060005A0 RID: 1440
		T deserialize<T>(MemoryStream memoryStream);

		// Token: 0x060005A1 RID: 1441
		T deserialize<T>(string path);
	}
}
