using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200056B RID: 1387
	public class ItemRegion
	{
		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06002C53 RID: 11347 RVA: 0x000C0082 File Offset: 0x000BE282
		public List<ItemDrop> drops
		{
			get
			{
				return this._drops;
			}
		}

		// Token: 0x06002C54 RID: 11348 RVA: 0x000C008A File Offset: 0x000BE28A
		[Obsolete("Renamed to DestroyAll")]
		public void destroy()
		{
			this.DestroyAll();
		}

		// Token: 0x06002C55 RID: 11349 RVA: 0x000C0092 File Offset: 0x000BE292
		internal void DestroyTail()
		{
			Object.Destroy(this.drops.GetAndRemoveTail<ItemDrop>().model.gameObject);
		}

		// Token: 0x06002C56 RID: 11350 RVA: 0x000C00B0 File Offset: 0x000BE2B0
		public void DestroyAll()
		{
			ushort num = 0;
			while ((int)num < this.drops.Count)
			{
				Object.Destroy(this.drops[(int)num].model.gameObject);
				num += 1;
			}
			this.drops.Clear();
		}

		// Token: 0x06002C57 RID: 11351 RVA: 0x000C00FC File Offset: 0x000BE2FC
		public ItemRegion()
		{
			this._drops = new List<ItemDrop>();
			this.items = new List<ItemData>();
			this.isNetworked = false;
			this.isPendingDestroy = false;
			this.lastRespawn = Time.realtimeSinceStartup;
			this.despawnItemIndex = 0;
			this.respawnItemIndex = 0;
		}

		// Token: 0x040017D1 RID: 6097
		private List<ItemDrop> _drops;

		// Token: 0x040017D2 RID: 6098
		public List<ItemData> items;

		// Token: 0x040017D3 RID: 6099
		public bool isNetworked;

		// Token: 0x040017D4 RID: 6100
		internal bool isPendingDestroy;

		// Token: 0x040017D5 RID: 6101
		public ushort despawnItemIndex;

		// Token: 0x040017D6 RID: 6102
		public ushort respawnItemIndex;

		// Token: 0x040017D7 RID: 6103
		public float lastRespawn;
	}
}
