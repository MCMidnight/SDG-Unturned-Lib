using System;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Provider
{
	// Token: 0x02000034 RID: 52
	public struct DynamicEconDetails
	{
		// Token: 0x06000127 RID: 295 RVA: 0x00004A0C File Offset: 0x00002C0C
		public bool getStatTrackerType(out EStatTrackerType type)
		{
			type = EStatTrackerType.NONE;
			if (this.tags.Contains("stat_tracker:total_kills"))
			{
				type = EStatTrackerType.TOTAL;
				return true;
			}
			if (this.tags.Contains("stat_tracker:player_kills"))
			{
				type = EStatTrackerType.PLAYER;
				return true;
			}
			return false;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00004A40 File Offset: 0x00002C40
		public bool getRagdollEffect(out ERagdollEffect effect)
		{
			int num = this.tags.IndexOf("ragdoll_effect:");
			if (num >= 0)
			{
				num += "ragdoll_effect:".Length;
				if (num < this.tags.Length - 1)
				{
					ReadOnlySpan<char> readOnlySpan = MemoryExtensions.AsSpan(this.tags, num, this.tags.Length - num);
					if (MemoryExtensions.StartsWith<char>(readOnlySpan, "zero_kelvin"))
					{
						effect = ERagdollEffect.ZERO_KELVIN;
						return true;
					}
					if (MemoryExtensions.StartsWith<char>(readOnlySpan, "jaded"))
					{
						effect = ERagdollEffect.JADED;
						return true;
					}
					if (MemoryExtensions.StartsWith<char>(readOnlySpan, "soulcrystal_green"))
					{
						effect = ERagdollEffect.SOUL_CRYSTAL_GREEN;
						return true;
					}
					if (MemoryExtensions.StartsWith<char>(readOnlySpan, "soulcrystal_magenta"))
					{
						effect = ERagdollEffect.SOUL_CRYSTAL_MAGENTA;
						return true;
					}
					if (MemoryExtensions.StartsWith<char>(readOnlySpan, "soulcrystal_red"))
					{
						effect = ERagdollEffect.SOUL_CRYSTAL_RED;
						return true;
					}
					if (MemoryExtensions.StartsWith<char>(readOnlySpan, "soulcrystal_yellow"))
					{
						effect = ERagdollEffect.SOUL_CRYSTAL_YELLOW;
						return true;
					}
				}
			}
			effect = ERagdollEffect.NONE;
			return false;
		}

		/// <summary>
		/// Parse dynamic tag mythic effect.
		/// </summary>
		/// <returns>ID of mythical asset, or zero if not in tags.</returns>
		// Token: 0x06000129 RID: 297 RVA: 0x00004B30 File Offset: 0x00002D30
		public ushort getParticleEffect()
		{
			int num = this.tags.IndexOf("particle_effect:");
			if (num < 0)
			{
				return 0;
			}
			int num2 = num + "particle_effect:".Length;
			if (num2 >= this.tags.Length)
			{
				return 0;
			}
			int num3 = this.tags.IndexOf(';', num2);
			if (num3 < 0)
			{
				num3 = this.tags.Length;
			}
			int num4 = num3 - num2;
			ushort result;
			if (ushort.TryParse(this.tags.Substring(num2, num4), ref result))
			{
				return result;
			}
			return 0;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00004BB0 File Offset: 0x00002DB0
		public bool getStatTrackerValue(out EStatTrackerType type, out int kills)
		{
			kills = -1;
			if (!this.getStatTrackerType(out type))
			{
				return false;
			}
			EStatTrackerType estatTrackerType = type;
			if (estatTrackerType == EStatTrackerType.TOTAL)
			{
				if (string.IsNullOrEmpty(this.dynamic_props))
				{
					kills = 0;
				}
				else
				{
					StatTrackerTotalKillsJson statTrackerTotalKillsJson = JsonUtility.FromJson<StatTrackerTotalKillsJson>(this.dynamic_props);
					kills = statTrackerTotalKillsJson.total_kills;
				}
				return true;
			}
			if (estatTrackerType != EStatTrackerType.PLAYER)
			{
				return false;
			}
			if (string.IsNullOrEmpty(this.dynamic_props))
			{
				kills = 0;
			}
			else
			{
				StatTrackerPlayerKillsJson statTrackerPlayerKillsJson = JsonUtility.FromJson<StatTrackerPlayerKillsJson>(this.dynamic_props);
				kills = statTrackerPlayerKillsJson.player_kills;
			}
			return true;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00004C2C File Offset: 0x00002E2C
		public string getPredictedDynamicPropsJsonForStatTracker(EStatTrackerType type, int kills)
		{
			if (type == EStatTrackerType.TOTAL)
			{
				return JsonUtility.ToJson(new StatTrackerTotalKillsJson
				{
					total_kills = kills
				});
			}
			if (type != EStatTrackerType.PLAYER)
			{
				return string.Empty;
			}
			return JsonUtility.ToJson(new StatTrackerPlayerKillsJson
			{
				player_kills = kills
			});
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00004C80 File Offset: 0x00002E80
		public DynamicEconDetails(string tags, string dynamic_props)
		{
			this.tags = (string.IsNullOrEmpty(tags) ? string.Empty : tags);
			this.dynamic_props = (string.IsNullOrEmpty(dynamic_props) ? string.Empty : dynamic_props);
		}

		// Token: 0x04000079 RID: 121
		public string tags;

		// Token: 0x0400007A RID: 122
		public string dynamic_props;
	}
}
