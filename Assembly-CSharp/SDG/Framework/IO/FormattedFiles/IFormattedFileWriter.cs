using System;

namespace SDG.Framework.IO.FormattedFiles
{
	// Token: 0x020000BD RID: 189
	public interface IFormattedFileWriter
	{
		// Token: 0x0600050C RID: 1292
		void writeKey(string key);

		// Token: 0x0600050D RID: 1293
		void writeValue(string key, string value);

		// Token: 0x0600050E RID: 1294
		void writeValue(string value);

		// Token: 0x0600050F RID: 1295
		void writeValue(string key, object value);

		// Token: 0x06000510 RID: 1296
		void writeValue(object value);

		// Token: 0x06000511 RID: 1297
		void writeValue<T>(string key, T value);

		// Token: 0x06000512 RID: 1298
		void writeValue<T>(T value);

		// Token: 0x06000513 RID: 1299
		void beginObject();

		// Token: 0x06000514 RID: 1300
		void beginObject(string key);

		// Token: 0x06000515 RID: 1301
		void endObject();

		// Token: 0x06000516 RID: 1302
		void beginArray(string key);

		// Token: 0x06000517 RID: 1303
		void beginArray();

		// Token: 0x06000518 RID: 1304
		void endArray();
	}
}
