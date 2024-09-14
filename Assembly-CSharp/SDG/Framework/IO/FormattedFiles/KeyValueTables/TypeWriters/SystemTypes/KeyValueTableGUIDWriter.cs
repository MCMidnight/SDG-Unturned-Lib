using System;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeWriters.SystemTypes
{
	// Token: 0x020000CC RID: 204
	public class KeyValueTableGUIDWriter : IFormattedTypeWriter
	{
		// Token: 0x0600056F RID: 1391 RVA: 0x00014FE4 File Offset: 0x000131E4
		public void write(IFormattedFileWriter writer, object value)
		{
			writer.writeValue(((Guid)value).ToString("N"));
		}
	}
}
