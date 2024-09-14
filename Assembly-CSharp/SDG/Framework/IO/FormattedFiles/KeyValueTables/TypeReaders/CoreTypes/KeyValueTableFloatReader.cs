using System;
using System.Globalization;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeReaders.CoreTypes
{
	// Token: 0x020000DB RID: 219
	public class KeyValueTableFloatReader : IFormattedTypeReader
	{
		// Token: 0x0600058D RID: 1421 RVA: 0x00015448 File Offset: 0x00013648
		public object read(IFormattedFileReader reader)
		{
			float num;
			float.TryParse(reader.readValue(), 511, CultureInfo.InvariantCulture, ref num);
			return num;
		}
	}
}
