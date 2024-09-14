using System;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeWriters.SystemTypes
{
	// Token: 0x020000CD RID: 205
	public class KeyValueTableTypeWriter : IFormattedTypeWriter
	{
		// Token: 0x06000571 RID: 1393 RVA: 0x00015014 File Offset: 0x00013214
		public void write(IFormattedFileWriter writer, object value)
		{
			Type type = value as Type;
			writer.writeValue(type.AssemblyQualifiedName);
		}
	}
}
