using System;
using System.Collections.Generic;

namespace SDG.Framework.IO.FormattedFiles
{
	// Token: 0x020000BB RID: 187
	public interface IFormattedFileReader
	{
		// Token: 0x060004F4 RID: 1268
		IEnumerable<string> getKeys();

		// Token: 0x060004F5 RID: 1269
		bool containsKey(string key);

		// Token: 0x060004F6 RID: 1270
		void readKey(string key);

		// Token: 0x060004F7 RID: 1271
		int readArrayLength(string key);

		// Token: 0x060004F8 RID: 1272
		int readArrayLength();

		// Token: 0x060004F9 RID: 1273
		void readArrayIndex(string key, int index);

		// Token: 0x060004FA RID: 1274
		void readArrayIndex(int index);

		// Token: 0x060004FB RID: 1275
		string readValue(string key);

		// Token: 0x060004FC RID: 1276
		string readValue(int index);

		// Token: 0x060004FD RID: 1277
		string readValue(string key, int index);

		// Token: 0x060004FE RID: 1278
		string readValue();

		// Token: 0x060004FF RID: 1279
		object readValue(Type type, string key);

		// Token: 0x06000500 RID: 1280
		object readValue(Type type, int index);

		// Token: 0x06000501 RID: 1281
		object readValue(Type type, string key, int index);

		// Token: 0x06000502 RID: 1282
		object readValue(Type type);

		// Token: 0x06000503 RID: 1283
		T readValue<T>(string key);

		// Token: 0x06000504 RID: 1284
		T readValue<T>(int index);

		// Token: 0x06000505 RID: 1285
		T readValue<T>(string key, int index);

		// Token: 0x06000506 RID: 1286
		T readValue<T>();

		// Token: 0x06000507 RID: 1287
		IFormattedFileReader readObject(string key);

		// Token: 0x06000508 RID: 1288
		IFormattedFileReader readObject(int index);

		// Token: 0x06000509 RID: 1289
		IFormattedFileReader readObject(string key, int index);

		// Token: 0x0600050A RID: 1290
		IFormattedFileReader readObject();
	}
}
