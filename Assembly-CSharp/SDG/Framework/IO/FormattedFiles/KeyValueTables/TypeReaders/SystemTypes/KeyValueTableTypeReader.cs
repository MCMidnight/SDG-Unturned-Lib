using System;
using SDG.Unturned;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeReaders.SystemTypes
{
	// Token: 0x020000D8 RID: 216
	public class KeyValueTableTypeReader : IFormattedTypeReader
	{
		// Token: 0x06000587 RID: 1415 RVA: 0x000152FC File Offset: 0x000134FC
		public object read(IFormattedFileReader reader)
		{
			string text = reader.readValue();
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			text = KeyValueTableTypeRedirectorRegistry.chase(text);
			if (text.IndexOfAny(DatValue.INVALID_TYPE_CHARS) >= 0)
			{
				return null;
			}
			return Type.GetType(text);
		}
	}
}
