using System;
using UnityEngine;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeReaders.UnityTypes
{
	// Token: 0x020000D4 RID: 212
	public class KeyValueTableVector2Reader : IFormattedTypeReader
	{
		// Token: 0x0600057F RID: 1407 RVA: 0x000151E0 File Offset: 0x000133E0
		public object read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			if (reader == null)
			{
				return null;
			}
			return new Vector2(reader.readValue<float>("X"), reader.readValue<float>("Y"));
		}
	}
}
