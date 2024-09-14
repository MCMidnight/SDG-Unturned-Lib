using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Overrides breathability for example in a deep cave with no oxygen, or near a deep sea plant that provides oxygen.
	/// </summary>
	// Token: 0x02000500 RID: 1280
	public class OxygenVolumeManager : VolumeManager<OxygenVolume, OxygenVolumeManager>
	{
		/// <summary>
		/// Find highest alpha breathable volume overlapping position.
		/// </summary>
		// Token: 0x06002824 RID: 10276 RVA: 0x000A9904 File Offset: 0x000A7B04
		public bool IsPositionInsideBreathableVolume(Vector3 position, out float maxAlpha)
		{
			bool result = false;
			maxAlpha = 0f;
			using (List<OxygenVolume>.Enumerator enumerator = this.breathableVolumes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					float b;
					if (enumerator.Current.IsPositionInsideVolumeWithAlpha(position, out b))
					{
						result = true;
						maxAlpha = Mathf.Max(maxAlpha, b);
						if (maxAlpha > 0.9999f)
						{
							maxAlpha = 1f;
							break;
						}
					}
				}
			}
			return result;
		}

		/// <summary>
		/// Find highest alpha non-breathable volume overlapping position.
		/// </summary>
		// Token: 0x06002825 RID: 10277 RVA: 0x000A9984 File Offset: 0x000A7B84
		public bool IsPositionInsideNonBreathableVolume(Vector3 position, out float maxAlpha)
		{
			bool result = false;
			maxAlpha = 0f;
			using (List<OxygenVolume>.Enumerator enumerator = this.nonBreathableVolumes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					float b;
					if (enumerator.Current.IsPositionInsideVolumeWithAlpha(position, out b))
					{
						result = true;
						maxAlpha = Mathf.Max(maxAlpha, b);
						if (maxAlpha > 0.9999f)
						{
							maxAlpha = 1f;
							break;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06002826 RID: 10278 RVA: 0x000A9A04 File Offset: 0x000A7C04
		public override void AddVolume(OxygenVolume volume)
		{
			base.AddVolume(volume);
			if (volume.isBreathable)
			{
				this.breathableVolumes.Add(volume);
				return;
			}
			this.nonBreathableVolumes.Add(volume);
		}

		// Token: 0x06002827 RID: 10279 RVA: 0x000A9A2E File Offset: 0x000A7C2E
		public override void RemoveVolume(OxygenVolume volume)
		{
			base.RemoveVolume(volume);
			if (volume.isBreathable)
			{
				this.breathableVolumes.RemoveFast(volume);
				return;
			}
			this.nonBreathableVolumes.RemoveFast(volume);
		}

		// Token: 0x06002828 RID: 10280 RVA: 0x000A9A5C File Offset: 0x000A7C5C
		public OxygenVolumeManager()
		{
			base.FriendlyName = "Oxygen";
			base.SetDebugColor(new Color32(110, 100, 110, byte.MaxValue));
			this.supportsFalloff = true;
			this.breathableVolumes = new List<OxygenVolume>();
			this.nonBreathableVolumes = new List<OxygenVolume>();
		}

		// Token: 0x04001543 RID: 5443
		internal List<OxygenVolume> breathableVolumes;

		// Token: 0x04001544 RID: 5444
		internal List<OxygenVolume> nonBreathableVolumes;
	}
}
