using System;
using System.Globalization;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeReaders.CoreTypes
{
	// Token: 0x020000DF RID: 223
	public class KeyValueTableShortReader : IFormattedTypeReader
	{
		// Token: 0x06000595 RID: 1429 RVA: 0x00015518 File Offset: 0x00013718
		public object read(IFormattedFileReader reader)
		{
			short num;
			short.TryParse(reader.readValue(), 511, CultureInfo.InvariantCulture, ref num);
			return num;
		}
	}
}
