using System;
using System.Globalization;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeWriters.CoreTypes
{
	// Token: 0x020000CF RID: 207
	public class KeyValueTableDoubleWriter : IFormattedTypeWriter
	{
		// Token: 0x06000575 RID: 1397 RVA: 0x00015070 File Offset: 0x00013270
		public void write(IFormattedFileWriter writer, object value)
		{
			string value2 = ((double)value).ToString(CultureInfo.InvariantCulture);
			writer.writeValue(value2);
		}
	}
}
