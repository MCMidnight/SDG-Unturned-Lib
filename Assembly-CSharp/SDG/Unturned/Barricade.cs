using System;

namespace SDG.Unturned
{
	// Token: 0x0200048B RID: 1163
	public class Barricade
	{
		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x0600245A RID: 9306 RVA: 0x0009164D File Offset: 0x0008F84D
		public bool isDead
		{
			get
			{
				return this.health == 0;
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x0600245B RID: 9307 RVA: 0x00091658 File Offset: 0x0008F858
		public bool isRepaired
		{
			get
			{
				return this.health == this.asset.health;
			}
		}

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x0600245C RID: 9308 RVA: 0x0009166D File Offset: 0x0008F86D
		[Obsolete]
		public ushort id
		{
			get
			{
				return this.asset.id;
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x0600245D RID: 9309 RVA: 0x0009167A File Offset: 0x0008F87A
		// (set) Token: 0x0600245E RID: 9310 RVA: 0x00091682 File Offset: 0x0008F882
		public ItemBarricadeAsset asset { get; private set; }

		// Token: 0x0600245F RID: 9311 RVA: 0x0009168B File Offset: 0x0008F88B
		public void askDamage(ushort amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (amount >= this.health)
			{
				this.health = 0;
				return;
			}
			this.health -= amount;
		}

		// Token: 0x06002460 RID: 9312 RVA: 0x000916BC File Offset: 0x0008F8BC
		public void askRepair(ushort amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (amount >= this.asset.health - this.health)
			{
				this.health = this.asset.health;
				return;
			}
			this.health += amount;
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x0009170C File Offset: 0x0008F90C
		[Obsolete]
		public Barricade(ushort newID)
		{
			this.asset = (Assets.find(EAssetType.ITEM, newID) as ItemBarricadeAsset);
			if (this.asset == null)
			{
				this.health = 0;
				this.state = new byte[0];
				return;
			}
			this.health = this.asset.health;
			this.state = this.asset.getState();
		}

		// Token: 0x06002462 RID: 9314 RVA: 0x0009176F File Offset: 0x0008F96F
		[Obsolete]
		public Barricade(ushort newID, ushort newHealth, byte[] newState, ItemBarricadeAsset newAsset)
		{
			this.health = newHealth;
			this.state = newState;
			this.asset = newAsset;
		}

		// Token: 0x06002463 RID: 9315 RVA: 0x00091790 File Offset: 0x0008F990
		public Barricade(ItemBarricadeAsset newAsset)
		{
			this.asset = newAsset;
			if (this.asset != null)
			{
				this.health = this.asset.health;
				this.state = this.asset.getState();
				return;
			}
			this.health = 0;
			this.state = new byte[0];
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x000917E8 File Offset: 0x0008F9E8
		public Barricade(ItemBarricadeAsset newAsset, ushort newHealth, byte[] newState)
		{
			this.asset = newAsset;
			this.health = newHealth;
			this.state = newState;
		}

		// Token: 0x06002465 RID: 9317 RVA: 0x00091808 File Offset: 0x0008FA08
		public override string ToString()
		{
			string[] array = new string[5];
			int num = 0;
			ItemBarricadeAsset asset = this.asset;
			array[num] = ((asset != null) ? asset.ToString() : null);
			array[1] = " ";
			array[2] = this.health.ToString();
			array[3] = " ";
			array[4] = this.state.Length.ToString();
			return string.Concat(array);
		}

		// Token: 0x04001255 RID: 4693
		public ushort health;

		// Token: 0x04001256 RID: 4694
		public byte[] state;
	}
}
