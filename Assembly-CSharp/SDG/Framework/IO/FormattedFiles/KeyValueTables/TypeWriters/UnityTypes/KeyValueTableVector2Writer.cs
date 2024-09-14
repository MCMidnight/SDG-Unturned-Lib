using System;
using UnityEngine;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeWriters.UnityTypes
{
	// Token: 0x020000C9 RID: 201
	public class KeyValueTableVector2Writer : IFormattedTypeWriter
	{
		// Token: 0x06000569 RID: 1385 RVA: 0x00014ED0 File Offset: 0x000130D0
		public void write(IFormattedFileWriter writer, object value)
		{
			writer.beginObject();
			Vector2 vector = (Vector2)value;
			writer.writeValue<float>("X", vector.x);
			writer.writeValue<float>("Y", vector.y);
			writer.endObject();
		}
	}
}
