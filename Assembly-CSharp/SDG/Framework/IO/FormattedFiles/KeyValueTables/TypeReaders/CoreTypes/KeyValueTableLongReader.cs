using System;
using System.Globalization;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeReaders.CoreTypes
{
	// Token: 0x020000DD RID: 221
	public class KeyValueTableLongReader : IFormattedTypeReader
	{
		// Token: 0x06000591 RID: 1425 RVA: 0x000154B0 File Offset: 0x000136B0
		public object read(IFormattedFileReader reader)
		{
			long num;
			long.TryParse(reader.readValue(), 511, CultureInfo.InvariantCulture, ref num);
			return num;
		}
	}
}
