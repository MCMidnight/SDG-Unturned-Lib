using System;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x0200010A RID: 266
	public class DeadzoneVolumeManager : VolumeManager<DeadzoneVolume, DeadzoneVolumeManager>
	{
		// Token: 0x060006D4 RID: 1748 RVA: 0x00019F04 File Offset: 0x00018104
		public DeadzoneVolume GetMostDangerousOverlappingVolume(Vector3 position)
		{
			DeadzoneVolume deadzoneVolume = null;
			foreach (DeadzoneVolume deadzoneVolume2 in this.allVolumes)
			{
				if ((deadzoneVolume == null || deadzoneVolume2.DeadzoneType > deadzoneVolume.DeadzoneType) && deadzoneVolume2.IsPositionInsideVolume(position))
				{
					deadzoneVolume = deadzoneVolume2;
					if (deadzoneVolume.DeadzoneType == EDeadzoneType.FullSuitRadiation)
					{
						break;
					}
				}
			}
			return deadzoneVolume;
		}

		/// <summary>
		/// Hacked to check horizontal distance.
		/// </summary>
		// Token: 0x060006D5 RID: 1749 RVA: 0x00019F80 File Offset: 0x00018180
		public bool IsNavmeshCenterInsideAnyVolume(Vector3 position)
		{
			foreach (DeadzoneVolume deadzoneVolume in this.allVolumes)
			{
				Vector3 position2 = new Vector3(position.x, deadzoneVolume.transform.position.y, position.z);
				if (deadzoneVolume.IsPositionInsideVolume(position2))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0001A000 File Offset: 0x00018200
		public DeadzoneVolumeManager()
		{
			base.FriendlyName = "Deadzone";
			base.SetDebugColor(new Color32(byte.MaxValue, 0, 0, byte.MaxValue));
		}
	}
}
