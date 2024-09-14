using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000553 RID: 1363
	public class ClaimManager : MonoBehaviour
	{
		// Token: 0x06002B24 RID: 11044 RVA: 0x000B81E0 File Offset: 0x000B63E0
		public static bool checkCanBuild(Vector3 point, CSteamID owner, CSteamID group, bool isClaim)
		{
			for (int i = 0; i < ClaimManager.bubbles.Count; i++)
			{
				ClaimBubble claimBubble = ClaimManager.bubbles[i];
				if ((isClaim ? ((claimBubble.origin - point).sqrMagnitude < 4f * claimBubble.sqrRadius) : ((claimBubble.origin - point).sqrMagnitude < claimBubble.sqrRadius)) && !OwnershipTool.checkToggle(owner, claimBubble.owner, group, claimBubble.group))
				{
					return false;
				}
			}
			return true;
		}

		/// <param name="isClaim">True if it's a new claim flag.</param>
		// Token: 0x06002B25 RID: 11045 RVA: 0x000B826C File Offset: 0x000B646C
		public static bool canBuildOnVehicle(Transform vehicle, CSteamID owner, CSteamID group)
		{
			foreach (ClaimPlant claimPlant in ClaimManager.plants)
			{
				if (!(claimPlant.parent != vehicle) && !OwnershipTool.checkToggle(owner, claimPlant.owner, group, claimPlant.group))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002B26 RID: 11046 RVA: 0x000B82E4 File Offset: 0x000B64E4
		public static ClaimBubble registerBubble(Vector3 origin, float radius, ulong owner, ulong group)
		{
			ClaimBubble claimBubble = new ClaimBubble(origin, radius * radius, owner, group);
			ClaimManager.bubbles.Add(claimBubble);
			return claimBubble;
		}

		// Token: 0x06002B27 RID: 11047 RVA: 0x000B8309 File Offset: 0x000B6509
		public static void deregisterBubble(ClaimBubble bubble)
		{
			ClaimManager.bubbles.Remove(bubble);
		}

		// Token: 0x06002B28 RID: 11048 RVA: 0x000B8318 File Offset: 0x000B6518
		public static ClaimPlant registerPlant(Transform parent, ulong owner, ulong group)
		{
			ClaimPlant claimPlant = new ClaimPlant(parent, owner, group);
			ClaimManager.plants.Add(claimPlant);
			return claimPlant;
		}

		// Token: 0x06002B29 RID: 11049 RVA: 0x000B833A File Offset: 0x000B653A
		public static void deregisterPlant(ClaimPlant plant)
		{
			ClaimManager.plants.Remove(plant);
		}

		// Token: 0x06002B2A RID: 11050 RVA: 0x000B8348 File Offset: 0x000B6548
		private void onLevelLoaded(int level)
		{
			ClaimManager.bubbles = new List<ClaimBubble>();
			ClaimManager.plants = new List<ClaimPlant>();
		}

		// Token: 0x06002B2B RID: 11051 RVA: 0x000B835E File Offset: 0x000B655E
		private void Start()
		{
			Level.onPrePreLevelLoaded = (PrePreLevelLoaded)Delegate.Combine(Level.onPrePreLevelLoaded, new PrePreLevelLoaded(this.onLevelLoaded));
		}

		// Token: 0x04001703 RID: 5891
		private static List<ClaimBubble> bubbles;

		// Token: 0x04001704 RID: 5892
		private static List<ClaimPlant> plants;
	}
}
