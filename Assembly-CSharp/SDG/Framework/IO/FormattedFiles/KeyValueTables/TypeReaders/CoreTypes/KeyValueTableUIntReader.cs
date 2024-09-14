using System;
using System.Globalization;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeReaders.CoreTypes
{
	// Token: 0x020000E1 RID: 225
	public class KeyValueTableUIntReader : IFormattedTypeReader
	{
		// Token: 0x06000599 RID: 1433 RVA: 0x0001555C File Offset: 0x0001375C
		public object read(IFormattedFileReader reader)
		{
			uint num;
			uint.TryParse(reader.readValue(), 511, CultureInfo.InvariantCulture, ref num);
			return num;
		}
	}
}
