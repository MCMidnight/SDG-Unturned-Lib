using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200051F RID: 1311
	public class WeatherComponentBase : MonoBehaviour
	{
		/// <summary>
		/// Lesser of global or volume blend alphas. 
		/// </summary>
		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x06002907 RID: 10503 RVA: 0x000AF0E2 File Offset: 0x000AD2E2
		public float EffectBlendAlpha
		{
			get
			{
				return Mathf.Min(this.globalBlendAlpha, this.localVolumeBlendAlpha);
			}
		}

		// Token: 0x06002908 RID: 10504 RVA: 0x000AF0F5 File Offset: 0x000AD2F5
		public NetId GetNetId()
		{
			return this.netId;
		}

		// Token: 0x06002909 RID: 10505 RVA: 0x000AF0FD File Offset: 0x000AD2FD
		public virtual void InitializeWeather()
		{
		}

		// Token: 0x0600290A RID: 10506 RVA: 0x000AF0FF File Offset: 0x000AD2FF
		public virtual void UpdateWeather()
		{
		}

		// Token: 0x0600290B RID: 10507 RVA: 0x000AF101 File Offset: 0x000AD301
		public virtual void UpdateLightingTime(int blendKey, int currentKey, float timeAlpha)
		{
		}

		// Token: 0x0600290C RID: 10508 RVA: 0x000AF103 File Offset: 0x000AD303
		public virtual void PreDestroyWeather()
		{
		}

		// Token: 0x0600290D RID: 10509 RVA: 0x000AF105 File Offset: 0x000AD305
		public virtual void OnBeginTransitionIn()
		{
		}

		// Token: 0x0600290E RID: 10510 RVA: 0x000AF107 File Offset: 0x000AD307
		public virtual void OnEndTransitionIn()
		{
		}

		// Token: 0x0600290F RID: 10511 RVA: 0x000AF109 File Offset: 0x000AD309
		public virtual void OnBeginTransitionOut()
		{
		}

		// Token: 0x06002910 RID: 10512 RVA: 0x000AF10B File Offset: 0x000AD30B
		public virtual void OnEndTransitionOut()
		{
		}

		// Token: 0x06002911 RID: 10513 RVA: 0x000AF10D File Offset: 0x000AD30D
		public IEnumerable<Player> EnumerateMaskedPlayers()
		{
			foreach (Player player in PlayerTool.EnumeratePlayers())
			{
				if ((player.movement.WeatherMask & this.asset.volumeMask) != 0U)
				{
					yield return player;
				}
			}
			IEnumerator<Player> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040015D0 RID: 5584
		public WeatherAssetBase asset;

		/// <summary>
		/// [0, 1] blends towards one while active regardless of local volume.
		/// </summary>
		// Token: 0x040015D1 RID: 5585
		public float globalBlendAlpha;

		/// <summary>
		/// [0, 1] blends towards one if current volume bitwise AND with asset is non-zero.
		/// </summary>
		// Token: 0x040015D2 RID: 5586
		public float localVolumeBlendAlpha;

		// Token: 0x040015D3 RID: 5587
		public bool isWeatherActive;

		/// <summary>
		/// If blending was not ticket yet then local blend can use global value, e.g. loading into rain storm.
		/// </summary>
		// Token: 0x040015D4 RID: 5588
		public bool hasTickedBlending;

		/// <summary>
		/// Is blendAlpha at 100%?
		/// </summary>
		// Token: 0x040015D5 RID: 5589
		public bool isFullyTransitionedIn;

		// Token: 0x040015D6 RID: 5590
		public Color fogColor;

		// Token: 0x040015D7 RID: 5591
		public float fogDensity;

		// Token: 0x040015D8 RID: 5592
		public bool overrideFog;

		// Token: 0x040015D9 RID: 5593
		public bool overrideAtmosphericFog;

		// Token: 0x040015DA RID: 5594
		public bool overrideCloudColors;

		// Token: 0x040015DB RID: 5595
		public Color cloudColor;

		// Token: 0x040015DC RID: 5596
		public Color cloudRimColor;

		/// <summary>
		/// [0, 1] Rain puddle alpha cutoff.
		/// </summary>
		// Token: 0x040015DD RID: 5597
		public float puddleWaterLevel;

		/// <summary>
		/// [0, 1] Rain puddle ripples alpha.
		/// </summary>
		// Token: 0x040015DE RID: 5598
		public float puddleIntensity;

		// Token: 0x040015DF RID: 5599
		public float brightnessMultiplier = 1f;

		// Token: 0x040015E0 RID: 5600
		public float shadowStrengthMultiplier = 1f;

		// Token: 0x040015E1 RID: 5601
		public float fogBlendExponent = 1f;

		// Token: 0x040015E2 RID: 5602
		public float cloudBlendExponent = 1f;

		// Token: 0x040015E3 RID: 5603
		public float windMain;

		// Token: 0x040015E4 RID: 5604
		internal NetId netId;
	}
}
