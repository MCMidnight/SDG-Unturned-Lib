using System;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeWriters.CoreTypes
{
	// Token: 0x020000CE RID: 206
	public class KeyValueTableBoolWriter : IFormattedTypeWriter
	{
		// Token: 0x06000573 RID: 1395 RVA: 0x0001503C File Offset: 0x0001323C
		public void write(IFormattedFileWriter writer, object value)
		{
			writer.writeValue(((bool)value) ? "true" : "false");
		}
	}
}
