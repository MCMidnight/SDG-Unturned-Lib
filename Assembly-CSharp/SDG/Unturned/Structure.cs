using System;

namespace SDG.Unturned
{
	// Token: 0x020004A3 RID: 1187
	public class Structure
	{
		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x060024E1 RID: 9441 RVA: 0x000930A2 File Offset: 0x000912A2
		public bool isDead
		{
			get
			{
				return this.health == 0;
			}
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x060024E2 RID: 9442 RVA: 0x000930AD File Offset: 0x000912AD
		public bool isRepaired
		{
			get
			{
				return this.health == this.asset.health;
			}
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x060024E3 RID: 9443 RVA: 0x000930C2 File Offset: 0x000912C2
		[Obsolete]
		public ushort id
		{
			get
			{
				return this.asset.id;
			}
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x060024E4 RID: 9444 RVA: 0x000930CF File Offset: 0x000912CF
		// (set) Token: 0x060024E5 RID: 9445 RVA: 0x000930D7 File Offset: 0x000912D7
		public ItemStructureAsset asset { get; private set; }

		// Token: 0x060024E6 RID: 9446 RVA: 0x000930E0 File Offset: 0x000912E0
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

		// Token: 0x060024E7 RID: 9447 RVA: 0x00093110 File Offset: 0x00091310
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

		// Token: 0x060024E8 RID: 9448 RVA: 0x0009315F File Offset: 0x0009135F
		[Obsolete]
		public Structure(ushort newID)
		{
			this.asset = (Assets.find(EAssetType.ITEM, newID) as ItemStructureAsset);
			this.health = this.asset.health;
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x0009318A File Offset: 0x0009138A
		[Obsolete]
		public Structure(ushort newID, ushort newHealth, ItemStructureAsset newAsset)
		{
			this.health = newHealth;
			this.asset = newAsset;
		}

		// Token: 0x060024EA RID: 9450 RVA: 0x000931A0 File Offset: 0x000913A0
		public Structure(ItemStructureAsset newAsset, ushort newHealth)
		{
			this.asset = newAsset;
			this.health = newHealth;
		}

		// Token: 0x060024EB RID: 9451 RVA: 0x000931B6 File Offset: 0x000913B6
		public override string ToString()
		{
			ItemStructureAsset asset = this.asset;
			return ((asset != null) ? asset.ToString() : null) + " " + this.health.ToString();
		}

		// Token: 0x040012C1 RID: 4801
		public ushort health;
	}
}
