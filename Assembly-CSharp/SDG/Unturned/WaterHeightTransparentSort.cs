using System;
using SDG.Framework.Water;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200051E RID: 1310
	public class WaterHeightTransparentSort : MonoBehaviour
	{
		// Token: 0x06002902 RID: 10498 RVA: 0x000AEFE8 File Offset: 0x000AD1E8
		internal void updateRenderQueue()
		{
			if (this.material == null)
			{
				return;
			}
			if (WaterUtility.isPointUnderwater(base.transform.position))
			{
				if (LevelLighting.isSea)
				{
					this.material.renderQueue = 3100;
					return;
				}
				this.material.renderQueue = 2900;
				return;
			}
			else
			{
				if (LevelLighting.isSea)
				{
					this.material.renderQueue = 2900;
					return;
				}
				this.material.renderQueue = 3100;
				return;
			}
		}

		// Token: 0x06002903 RID: 10499 RVA: 0x000AF067 File Offset: 0x000AD267
		protected void handleIsSeaChanged(bool isSea)
		{
			this.updateRenderQueue();
		}

		// Token: 0x06002904 RID: 10500 RVA: 0x000AF06F File Offset: 0x000AD26F
		protected void Start()
		{
			this.material = HighlighterTool.getMaterialInstance(base.transform);
			if (this.material != null)
			{
				LevelLighting.isSeaChanged += this.handleIsSeaChanged;
				this.updateRenderQueue();
			}
		}

		// Token: 0x06002905 RID: 10501 RVA: 0x000AF0A7 File Offset: 0x000AD2A7
		protected void OnDestroy()
		{
			if (this.material != null)
			{
				LevelLighting.isSeaChanged -= this.handleIsSeaChanged;
				Object.DestroyImmediate(this.material);
				this.material = null;
			}
		}

		// Token: 0x040015CE RID: 5582
		protected bool isUnderwater;

		// Token: 0x040015CF RID: 5583
		protected Material material;
	}
}
