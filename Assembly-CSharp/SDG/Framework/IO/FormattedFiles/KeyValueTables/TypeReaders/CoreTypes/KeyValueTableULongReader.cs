using System;
using System.Globalization;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeReaders.CoreTypes
{
	// Token: 0x020000E2 RID: 226
	public class KeyValueTableULongReader : IFormattedTypeReader
	{
		// Token: 0x0600059B RID: 1435 RVA: 0x00015590 File Offset: 0x00013790
		public object read(IFormattedFileReader reader)
		{
			ulong num;
			ulong.TryParse(reader.readValue(), 511, CultureInfo.InvariantCulture, ref num);
			return num;
		}
	}
}
