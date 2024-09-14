using System;
using System.Globalization;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeReaders.CoreTypes
{
	// Token: 0x020000E3 RID: 227
	public class KeyValueTableUShortReader : IFormattedTypeReader
	{
		// Token: 0x0600059D RID: 1437 RVA: 0x000155C4 File Offset: 0x000137C4
		public object read(IFormattedFileReader reader)
		{
			ushort num;
			ushort.TryParse(reader.readValue(), 511, CultureInfo.InvariantCulture, ref num);
			return num;
		}
	}
}
