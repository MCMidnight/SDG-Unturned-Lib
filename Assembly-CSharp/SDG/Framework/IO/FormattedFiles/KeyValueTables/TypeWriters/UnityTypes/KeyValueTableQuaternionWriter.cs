using System;
using UnityEngine;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeWriters.UnityTypes
{
	// Token: 0x020000C8 RID: 200
	public class KeyValueTableQuaternionWriter : IFormattedTypeWriter
	{
		// Token: 0x06000567 RID: 1383 RVA: 0x00014E64 File Offset: 0x00013064
		public void write(IFormattedFileWriter writer, object value)
		{
			writer.beginObject();
			Quaternion quaternion = (Quaternion)value;
			writer.writeValue<float>("X", quaternion.x);
			writer.writeValue<float>("Y", quaternion.y);
			writer.writeValue<float>("Z", quaternion.z);
			writer.writeValue<float>("W", quaternion.w);
			writer.endObject();
		}
	}
}
