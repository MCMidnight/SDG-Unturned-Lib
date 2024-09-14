using System;
using UnityEngine;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeWriters.UnityTypes
{
	// Token: 0x020000CA RID: 202
	public class KeyValueTableVector3Writer : IFormattedTypeWriter
	{
		// Token: 0x0600056B RID: 1387 RVA: 0x00014F1C File Offset: 0x0001311C
		public void write(IFormattedFileWriter writer, object value)
		{
			writer.beginObject();
			Vector3 vector = (Vector3)value;
			writer.writeValue<float>("X", vector.x);
			writer.writeValue<float>("Y", vector.y);
			writer.writeValue<float>("Z", vector.z);
			writer.endObject();
		}
	}
}
