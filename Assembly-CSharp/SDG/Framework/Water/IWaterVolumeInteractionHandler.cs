using System;

namespace SDG.Framework.Water
{
	// Token: 0x02000077 RID: 119
	public interface IWaterVolumeInteractionHandler
	{
		// Token: 0x060002DA RID: 730
		void waterBeginCollision(WaterVolume volume);

		// Token: 0x060002DB RID: 731
		void waterEndCollision(WaterVolume volume);
	}
}
