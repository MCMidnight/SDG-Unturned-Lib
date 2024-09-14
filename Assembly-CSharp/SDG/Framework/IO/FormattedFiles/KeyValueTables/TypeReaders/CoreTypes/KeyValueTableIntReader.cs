using System;
using System.Globalization;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeReaders.CoreTypes
{
	// Token: 0x020000DC RID: 220
	public class KeyValueTableIntReader : IFormattedTypeReader
	{
		// Token: 0x0600058F RID: 1423 RVA: 0x0001547C File Offset: 0x0001367C
		public object read(IFormattedFileReader reader)
		{
			int num;
			int.TryParse(reader.readValue(), 511, CultureInfo.InvariantCulture, ref num);
			return num;
		}
	}
}
