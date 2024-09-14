using System;
using UnityEngine;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeReaders.UnityTypes
{
	// Token: 0x020000D1 RID: 209
	public class KeyValueTableColor32Reader : IFormattedTypeReader
	{
		// Token: 0x06000579 RID: 1401 RVA: 0x000150D0 File Offset: 0x000132D0
		public object read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			if (reader == null)
			{
				return null;
			}
			return new Color32(reader.readValue<byte>("R"), reader.readValue<byte>("G"), reader.readValue<byte>("B"), reader.readValue<byte>("A"));
		}
	}
}
