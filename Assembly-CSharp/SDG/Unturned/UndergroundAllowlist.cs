using System;
using SDG.Framework.Devkit;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000511 RID: 1297
	public static class UndergroundAllowlist
	{
		/// <summary>
		/// If level is using underground allowlist then conditionally clamp world-space position.
		/// </summary>
		// Token: 0x0600289C RID: 10396 RVA: 0x000ACEA0 File Offset: 0x000AB0A0
		public static bool AdjustPosition(ref Vector3 worldspacePosition, float offset, float threshold = 0.1f)
		{
			if (Level.info == null || !Level.info.configData.Use_Underground_Whitelist)
			{
				return false;
			}
			if (VolumeManager<UndergroundWhitelistVolume, UndergroundWhitelistVolumeManager>.Get().IsPositionInsideAnyVolume(worldspacePosition))
			{
				return false;
			}
			float height = LevelGround.getHeight(worldspacePosition);
			if (worldspacePosition.y < height - threshold)
			{
				worldspacePosition.y = height + offset;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Used by animals and zombies to teleport to a spawnpoint if outside the map.
		/// </summary>
		// Token: 0x0600289D RID: 10397 RVA: 0x000ACF00 File Offset: 0x000AB100
		public static bool IsPositionWithinValidHeight(Vector3 position, float threshold = 0.1f)
		{
			if (position.y < -1024f || position.y > 1024f)
			{
				return false;
			}
			if (Level.info == null || !Level.info.configData.Use_Underground_Whitelist)
			{
				return true;
			}
			float height = LevelGround.getHeight(position);
			return position.y > height - threshold || VolumeManager<UndergroundWhitelistVolume, UndergroundWhitelistVolumeManager>.Get().IsPositionInsideAnyVolume(position);
		}

		/// <summary>
		/// Used by housing validation to check item isn't placed underground.
		/// </summary>
		// Token: 0x0600289E RID: 10398 RVA: 0x000ACF64 File Offset: 0x000AB164
		public static bool IsPositionBuildable(Vector3 position)
		{
			if (Level.info == null || !Level.info.configData.Use_Underground_Whitelist)
			{
				return true;
			}
			float height = LevelGround.getHeight(position);
			return position.y > height || VolumeManager<UndergroundWhitelistVolume, UndergroundWhitelistVolumeManager>.Get().IsPositionInsideAnyVolume(position);
		}
	}
}
