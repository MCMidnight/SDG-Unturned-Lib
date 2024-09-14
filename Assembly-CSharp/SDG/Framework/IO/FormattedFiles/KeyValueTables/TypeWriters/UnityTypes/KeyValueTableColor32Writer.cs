using System;
using UnityEngine;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeWriters.UnityTypes
{
	// Token: 0x020000C6 RID: 198
	public class KeyValueTableColor32Writer : IFormattedTypeWriter
	{
		// Token: 0x06000563 RID: 1379 RVA: 0x00014D84 File Offset: 0x00012F84
		public void write(IFormattedFileWriter writer, object value)
		{
			writer.beginObject();
			Color32 color = (Color32)value;
			writer.writeValue<byte>("R", color.r);
			writer.writeValue<byte>("G", color.g);
			writer.writeValue<byte>("B", color.b);
			writer.writeValue<byte>("A", color.a);
			writer.endObject();
		}
	}
}
