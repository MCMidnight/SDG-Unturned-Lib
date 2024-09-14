using System;
using UnityEngine;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeWriters.UnityTypes
{
	// Token: 0x020000CB RID: 203
	public class KeyValueTableVector4Writer : IFormattedTypeWriter
	{
		// Token: 0x0600056D RID: 1389 RVA: 0x00014F78 File Offset: 0x00013178
		public void write(IFormattedFileWriter writer, object value)
		{
			writer.beginObject();
			Vector4 vector = (Vector4)value;
			writer.writeValue<float>("X", vector.x);
			writer.writeValue<float>("Y", vector.y);
			writer.writeValue<float>("Z", vector.z);
			writer.writeValue<float>("W", vector.w);
			writer.endObject();
		}
	}
}
