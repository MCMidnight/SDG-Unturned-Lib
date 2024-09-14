using System;
using System.Globalization;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeReaders.CoreTypes
{
	// Token: 0x020000DE RID: 222
	public class KeyValueTableSByteReader : IFormattedTypeReader
	{
		// Token: 0x06000593 RID: 1427 RVA: 0x000154E4 File Offset: 0x000136E4
		public object read(IFormattedFileReader reader)
		{
			sbyte b;
			sbyte.TryParse(reader.readValue(), 511, CultureInfo.InvariantCulture, ref b);
			return b;
		}
	}
}
