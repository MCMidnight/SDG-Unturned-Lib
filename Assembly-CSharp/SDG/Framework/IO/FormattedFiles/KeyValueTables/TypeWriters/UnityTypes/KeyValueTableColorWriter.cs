using System;
using UnityEngine;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeWriters.UnityTypes
{
	// Token: 0x020000C7 RID: 199
	public class KeyValueTableColorWriter : IFormattedTypeWriter
	{
		// Token: 0x06000565 RID: 1381 RVA: 0x00014DF0 File Offset: 0x00012FF0
		public void write(IFormattedFileWriter writer, object value)
		{
			writer.beginObject();
			Color32 color = (Color)value;
			writer.writeValue<byte>("R", color.r);
			writer.writeValue<byte>("G", color.g);
			writer.writeValue<byte>("B", color.b);
			writer.writeValue<byte>("A", color.a);
			writer.endObject();
		}
	}
}
