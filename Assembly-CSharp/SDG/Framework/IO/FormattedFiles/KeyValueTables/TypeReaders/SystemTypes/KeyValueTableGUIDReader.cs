using System;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeReaders.SystemTypes
{
	// Token: 0x020000D7 RID: 215
	public class KeyValueTableGUIDReader : IFormattedTypeReader
	{
		// Token: 0x06000585 RID: 1413 RVA: 0x000152B4 File Offset: 0x000134B4
		public object read(IFormattedFileReader reader)
		{
			string text = reader.readValue();
			if (string.IsNullOrEmpty(text) || text.Equals("0"))
			{
				return Guid.Empty;
			}
			return new Guid(text);
		}
	}
}
