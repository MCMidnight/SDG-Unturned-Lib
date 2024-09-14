using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000600 RID: 1536
	public struct PlayerSpotLightConfig
	{
		// Token: 0x0600306A RID: 12394 RVA: 0x000D4FD6 File Offset: 0x000D31D6
		public void applyToLight(Light light)
		{
			if (light == null)
			{
				return;
			}
			light.range = this.range;
			light.spotAngle = this.angle;
			light.intensity = this.intensity;
			light.color = this.color;
		}

		// Token: 0x0600306B RID: 12395 RVA: 0x000D5014 File Offset: 0x000D3214
		public PlayerSpotLightConfig(DatDictionary data)
		{
			this.isEnabled = data.ParseBool("SpotLight_Enabled", true);
			this.range = data.ParseFloat("SpotLight_Range", 64f);
			this.angle = data.ParseFloat("SpotLight_Angle", 90f);
			this.intensity = data.ParseFloat("SpotLight_Intensity", 1.3f);
			this.color = data.LegacyParseColor("SpotLight_Color", new Color32(245, 223, 147, byte.MaxValue));
		}

		/// <summary>
		/// If true, light contributes to player spotlight. Defaults to true.
		///
		/// Can be set to false for modders with a custom light setup. For example, this was added
		/// for a modder who is using melee lights to toggle a lightsaber-style glow.
		/// </summary>
		// Token: 0x04001B6C RID: 7020
		public bool isEnabled;

		// Token: 0x04001B6D RID: 7021
		public float range;

		// Token: 0x04001B6E RID: 7022
		public float angle;

		// Token: 0x04001B6F RID: 7023
		public float intensity;

		// Token: 0x04001B70 RID: 7024
		public Color color;
	}
}
