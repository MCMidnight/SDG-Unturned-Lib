using System;
using System.Globalization;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeWriters.CoreTypes
{
	// Token: 0x020000D0 RID: 208
	public class KeyValueTableFloatWriter : IFormattedTypeWriter
	{
		// Token: 0x06000577 RID: 1399 RVA: 0x000150A0 File Offset: 0x000132A0
		public void write(IFormattedFileWriter writer, object value)
		{
			string value2 = ((float)value).ToString(CultureInfo.InvariantCulture);
			writer.writeValue(value2);
		}
	}
}
