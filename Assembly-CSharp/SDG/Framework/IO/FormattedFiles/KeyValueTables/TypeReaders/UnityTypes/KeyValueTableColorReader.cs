using System;
using UnityEngine;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeReaders.UnityTypes
{
	// Token: 0x020000D2 RID: 210
	public class KeyValueTableColorReader : IFormattedTypeReader
	{
		// Token: 0x0600057B RID: 1403 RVA: 0x00015128 File Offset: 0x00013328
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
