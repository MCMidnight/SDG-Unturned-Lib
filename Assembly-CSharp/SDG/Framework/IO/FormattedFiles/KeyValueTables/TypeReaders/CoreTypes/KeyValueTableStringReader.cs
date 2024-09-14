using System;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeReaders.CoreTypes
{
	// Token: 0x020000E0 RID: 224
	public class KeyValueTableStringReader : IFormattedTypeReader
	{
		// Token: 0x06000597 RID: 1431 RVA: 0x0001554B File Offset: 0x0001374B
		public object read(IFormattedFileReader reader)
		{
			return reader.readValue();
		}
	}
}
