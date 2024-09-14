using System;

namespace SDG.Unturned
{
	// Token: 0x02000376 RID: 886
	public struct PaintableVehicleSection : IDatParseable
	{
		// Token: 0x06001AB3 RID: 6835 RVA: 0x000603A4 File Offset: 0x0005E5A4
		public bool TryParse(IDatNode node)
		{
			DatDictionary datDictionary = node as DatDictionary;
			if (datDictionary != null)
			{
				this.path = datDictionary.GetString("Path", null);
				this.materialIndex = datDictionary.ParseInt32("MaterialIndex", 0);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Scene hierarchy path relative to vehicle root.
		/// </summary>
		// Token: 0x04000C59 RID: 3161
		public string path;

		/// <summary>
		/// Index in renderer's materials array.
		/// </summary>
		// Token: 0x04000C5A RID: 3162
		public int materialIndex;
	}
}
