using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Nelson 2023-08-11: this probably should be rewritten a bit if used in the future
	/// because the error context currently assumes this is an item reward for consumables.
	/// </summary>
	// Token: 0x0200036A RID: 874
	public struct SpawnTableReward
	{
		// Token: 0x06001A77 RID: 6775 RVA: 0x0005F9E4 File Offset: 0x0005DBE4
		public SpawnTableReward(ushort tableID, int min, int max)
		{
			this.tableID = tableID;
			this.min = min;
			this.max = max;
		}

		// Token: 0x06001A78 RID: 6776 RVA: 0x0005F9FB File Offset: 0x0005DBFB
		public int count()
		{
			return Random.Range(this.min, this.max + 1);
		}

		// Token: 0x06001A79 RID: 6777 RVA: 0x0005FA10 File Offset: 0x0005DC10
		public int count(float multiplier)
		{
			return Mathf.CeilToInt((float)this.count() * multiplier);
		}

		/// <summary>
		/// Resolve table as items and grant random number to player.
		/// </summary>
		// Token: 0x06001A7A RID: 6778 RVA: 0x0005FA20 File Offset: 0x0005DC20
		public void grantItems(Player player, EItemOrigin itemOrigin, bool shouldAutoEquip)
		{
			foreach (ushort newID in this.spawn())
			{
				player.inventory.forceAddItem(new Item(newID, itemOrigin), shouldAutoEquip, false);
			}
		}

		/// <summary>
		/// Resolve table as items and grant random number to player.
		/// </summary>
		// Token: 0x06001A7B RID: 6779 RVA: 0x0005FA80 File Offset: 0x0005DC80
		public void grantItems(Player player, EItemOrigin itemOrigin, bool shouldAutoEquip, float countMultiplier)
		{
			foreach (ushort newID in this.spawn(countMultiplier))
			{
				player.inventory.forceAddItem(new Item(newID, itemOrigin), shouldAutoEquip, false);
			}
		}

		/// <summary>
		/// Enumerate random number of valid assetIDs.
		/// </summary>
		// Token: 0x06001A7C RID: 6780 RVA: 0x0005FAE0 File Offset: 0x0005DCE0
		public SpawnTableRewardEnumerator spawn()
		{
			return new SpawnTableRewardEnumerator(this.tableID, this.count());
		}

		// Token: 0x06001A7D RID: 6781 RVA: 0x0005FAF3 File Offset: 0x0005DCF3
		public SpawnTableRewardEnumerator spawn(float multiplier)
		{
			return new SpawnTableRewardEnumerator(this.tableID, this.count(multiplier));
		}

		// Token: 0x04000C37 RID: 3127
		public ushort tableID;

		// Token: 0x04000C38 RID: 3128
		public int min;

		// Token: 0x04000C39 RID: 3129
		public int max;
	}
}
