using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002CD RID: 717
	public interface ISkinableAsset
	{
		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x060014E5 RID: 5349
		Texture albedoBase { get; }

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x060014E6 RID: 5350
		Texture metallicBase { get; }

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x060014E7 RID: 5351
		Texture emissionBase { get; }
	}
}
