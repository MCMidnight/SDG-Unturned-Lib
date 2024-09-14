using System;
using System.Globalization;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeReaders.CoreTypes
{
	// Token: 0x020000DA RID: 218
	public class KeyValueTableByteReader : IFormattedTypeReader
	{
		// Token: 0x0600058B RID: 1419 RVA: 0x00015414 File Offset: 0x00013614
		public object read(IFormattedFileReader reader)
		{
			byte b;
			byte.TryParse(reader.readValue(), 511, CultureInfo.InvariantCulture, ref b);
			return b;
		}
	}
}
