using System;
using UnityEngine;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeReaders.UnityTypes
{
	// Token: 0x020000D3 RID: 211
	public class KeyValueTableQuaternionReader : IFormattedTypeReader
	{
		// Token: 0x0600057D RID: 1405 RVA: 0x00015188 File Offset: 0x00013388
		public object read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			if (reader == null)
			{
				return null;
			}
			return new Quaternion(reader.readValue<float>("X"), reader.readValue<float>("Y"), reader.readValue<float>("Z"), reader.readValue<float>("W"));
		}
	}
}
