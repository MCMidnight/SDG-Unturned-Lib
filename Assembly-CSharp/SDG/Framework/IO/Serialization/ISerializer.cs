using System;
using System.IO;

namespace SDG.Framework.IO.Serialization
{
	// Token: 0x020000B6 RID: 182
	public interface ISerializer
	{
		// Token: 0x060004E5 RID: 1253
		void serialize<T>(T instance, byte[] data, int offset, out int size, bool isFormatted);

		// Token: 0x060004E6 RID: 1254
		void serialize<T>(T instance, MemoryStream memoryStream, bool isFormatted);

		// Token: 0x060004E7 RID: 1255
		void serialize<T>(T instance, string path, bool isFormatted);
	}
}
