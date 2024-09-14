using System;
using SDG.Framework.Devkit;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000510 RID: 1296
	[Obsolete("Renamed to UndergroundAllowlist")]
	public static class UndergroundWhitelist
	{
		// Token: 0x0600289A RID: 10394 RVA: 0x000ACE89 File Offset: 0x000AB089
		public static bool isPointInsideVolume(Vector3 worldspacePosition)
		{
			return VolumeManager<UndergroundWhitelistVolume, UndergroundWhitelistVolumeManager>.Get().IsPositionInsideAnyVolume(worldspacePosition);
		}

		/// <summary>
		/// If level is using underground whitelist then conditionally clamp world-space position.
		/// </summary>
		// Token: 0x0600289B RID: 10395 RVA: 0x000ACE96 File Offset: 0x000AB096
		[Obsolete("Renamed to UndergroundAllowlist.AdjustPosition")]
		public static bool adjustPosition(ref Vector3 worldspacePosition, float offset, float threshold = 0.1f)
		{
			return UndergroundAllowlist.AdjustPosition(ref worldspacePosition, offset, threshold);
		}
	}
}
