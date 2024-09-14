using System;
using UnityEngine;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeReaders.UnityTypes
{
	// Token: 0x020000D6 RID: 214
	public class KeyValueTableVector4Reader : IFormattedTypeReader
	{
		// Token: 0x06000583 RID: 1411 RVA: 0x0001525C File Offset: 0x0001345C
		public object read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			if (reader == null)
			{
				return null;
			}
			return new Vector4(reader.readValue<float>("X"), reader.readValue<float>("Y"), reader.readValue<float>("Z"), reader.readValue<float>("W"));
		}
	}
}
