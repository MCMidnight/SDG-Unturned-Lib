using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020006F8 RID: 1784
	public class BlueprintItemIconsInfo
	{
		// Token: 0x06003B2A RID: 15146 RVA: 0x001147E0 File Offset: 0x001129E0
		public void onItemIconReady(Texture2D texture)
		{
			if (this.index >= this.textures.Length)
			{
				return;
			}
			this.textures[this.index] = texture;
			this.index++;
			if (this.index == this.textures.Length)
			{
				BlueprintItemIconsReady blueprintItemIconsReady = this.callback;
				if (blueprintItemIconsReady == null)
				{
					return;
				}
				blueprintItemIconsReady();
			}
		}

		// Token: 0x04002509 RID: 9481
		public Texture2D[] textures;

		// Token: 0x0400250A RID: 9482
		public BlueprintItemIconsReady callback;

		// Token: 0x0400250B RID: 9483
		private int index;
	}
}
