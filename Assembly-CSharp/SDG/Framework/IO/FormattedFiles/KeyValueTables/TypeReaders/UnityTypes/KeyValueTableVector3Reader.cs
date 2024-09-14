using System;
using UnityEngine;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeReaders.UnityTypes
{
	// Token: 0x020000D5 RID: 213
	public class KeyValueTableVector3Reader : IFormattedTypeReader
	{
		// Token: 0x06000581 RID: 1409 RVA: 0x00015217 File Offset: 0x00013417
		public object read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			if (reader == null)
			{
				return null;
			}
			return new Vector3(reader.readValue<float>("X"), reader.readValue<float>("Y"), reader.readValue<float>("Z"));
		}
	}
}
