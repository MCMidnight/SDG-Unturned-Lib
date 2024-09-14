using System;

namespace SDG.Unturned
{
	// Token: 0x02000378 RID: 888
	internal class VehicleRandomPaintColorConfiguration : IDatParseable
	{
		// Token: 0x06001AB4 RID: 6836 RVA: 0x000603E4 File Offset: 0x0005E5E4
		public bool TryParse(IDatNode node)
		{
			DatDictionary datDictionary = node as DatDictionary;
			return datDictionary != null && (datDictionary.TryParseFloat("MinSaturation", out this.minSaturation) & datDictionary.TryParseFloat("MaxSaturation", out this.maxSaturation) & datDictionary.TryParseFloat("MinValue", out this.minValue) & datDictionary.TryParseFloat("MaxValue", out this.maxValue) & datDictionary.TryParseFloat("GrayscaleChance", out this.grayscaleChance));
		}

		// Token: 0x04000C5F RID: 3167
		public float minSaturation;

		// Token: 0x04000C60 RID: 3168
		public float maxSaturation;

		// Token: 0x04000C61 RID: 3169
		public float minValue;

		// Token: 0x04000C62 RID: 3170
		public float maxValue;

		/// <summary>
		/// [0, 1] color will have zero saturation if random value is less than this. For example, 0.2 means 20% of
		/// vehicles will be grayscale.
		/// </summary>
		// Token: 0x04000C63 RID: 3171
		public float grayscaleChance;
	}
}
