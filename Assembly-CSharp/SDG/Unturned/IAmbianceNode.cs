using System;

namespace SDG.Unturned
{
	// Token: 0x020004B9 RID: 1209
	public interface IAmbianceNode
	{
		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06002544 RID: 9540
		[Obsolete]
		ushort id { get; }

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06002545 RID: 9541
		bool noWater { get; }

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06002546 RID: 9542
		bool noLighting { get; }

		// Token: 0x06002547 RID: 9543
		EffectAsset GetEffectAsset();
	}
}
