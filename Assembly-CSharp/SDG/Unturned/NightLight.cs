using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004FD RID: 1277
	public class NightLight : MonoBehaviour
	{
		// Token: 0x06002811 RID: 10257 RVA: 0x000A95BC File Offset: 0x000A77BC
		private void onLevelLoaded(int index)
		{
			if (this.isListeningLoad)
			{
				this.isListeningLoad = false;
				Level.onLevelLoaded = (LevelLoaded)Delegate.Remove(Level.onLevelLoaded, new LevelLoaded(this.onLevelLoaded));
			}
			if (!this.isListeningTime)
			{
				this.isListeningTime = true;
				LightingManager.onDayNightUpdated = (DayNightUpdated)Delegate.Combine(LightingManager.onDayNightUpdated, new DayNightUpdated(this.onDayNightUpdated));
			}
			this.onDayNightUpdated(LightingManager.isDaytime);
		}

		// Token: 0x06002812 RID: 10258 RVA: 0x000A9634 File Offset: 0x000A7834
		private void onDayNightUpdated(bool isDaytime)
		{
			if (this.target != null)
			{
				this.target.gameObject.SetActive(!isDaytime);
			}
			if (this.material != null)
			{
				this.material.SetColor("_EmissionColor", isDaytime ? Color.black : this.emissionColor);
			}
		}

		// Token: 0x06002813 RID: 10259 RVA: 0x000A9694 File Offset: 0x000A7894
		private void Awake()
		{
			this.material = HighlighterTool.getMaterialInstance(base.transform);
			if (this.material != null)
			{
				this.emissionColor = this.material.GetColor("_EmissionColor");
				if (this.emissionColor.IsNearlyBlack(0.001f))
				{
					this.emissionColor = new Color(1.5f, 1.5f, 1.5f);
				}
			}
			if (Level.isEditor)
			{
				this.onDayNightUpdated(false);
				return;
			}
			if (!this.isListeningLoad)
			{
				this.isListeningLoad = true;
				Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(Level.onLevelLoaded, new LevelLoaded(this.onLevelLoaded));
			}
		}

		// Token: 0x06002814 RID: 10260 RVA: 0x000A9740 File Offset: 0x000A7940
		private void OnDestroy()
		{
			if (this.material != null)
			{
				Object.DestroyImmediate(this.material);
			}
			if (this.isListeningLoad)
			{
				this.isListeningLoad = false;
				Level.onLevelLoaded = (LevelLoaded)Delegate.Remove(Level.onLevelLoaded, new LevelLoaded(this.onLevelLoaded));
			}
			if (this.isListeningTime)
			{
				this.isListeningTime = false;
				LightingManager.onDayNightUpdated = (DayNightUpdated)Delegate.Remove(LightingManager.onDayNightUpdated, new DayNightUpdated(this.onDayNightUpdated));
			}
		}

		// Token: 0x0400153A RID: 5434
		public Light target;

		// Token: 0x0400153B RID: 5435
		private Material material;

		// Token: 0x0400153C RID: 5436
		private Color emissionColor;

		// Token: 0x0400153D RID: 5437
		private bool isListeningLoad;

		// Token: 0x0400153E RID: 5438
		private bool isListeningTime;
	}
}
