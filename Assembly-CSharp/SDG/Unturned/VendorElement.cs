using System;

namespace SDG.Unturned
{
	// Token: 0x02000382 RID: 898
	public abstract class VendorElement
	{
		/// <summary>
		/// Vendor asset that owns this buy/sell record.
		/// </summary>
		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001BDB RID: 7131 RVA: 0x00063D7A File Offset: 0x00061F7A
		// (set) Token: 0x06001BDC RID: 7132 RVA: 0x00063D82 File Offset: 0x00061F82
		public VendorAsset outerAsset { get; protected set; }

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001BDD RID: 7133 RVA: 0x00063D8B File Offset: 0x00061F8B
		// (set) Token: 0x06001BDE RID: 7134 RVA: 0x00063D93 File Offset: 0x00061F93
		public byte index { get; protected set; }

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001BDF RID: 7135 RVA: 0x00063D9C File Offset: 0x00061F9C
		// (set) Token: 0x06001BE0 RID: 7136 RVA: 0x00063DA4 File Offset: 0x00061FA4
		public Guid TargetAssetGuid { get; protected set; }

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06001BE1 RID: 7137 RVA: 0x00063DAD File Offset: 0x00061FAD
		// (set) Token: 0x06001BE2 RID: 7138 RVA: 0x00063DB5 File Offset: 0x00061FB5
		[Obsolete]
		public ushort id { get; protected set; }

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06001BE3 RID: 7139 RVA: 0x00063DBE File Offset: 0x00061FBE
		// (set) Token: 0x06001BE4 RID: 7140 RVA: 0x00063DC6 File Offset: 0x00061FC6
		public uint cost { get; protected set; }

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06001BE5 RID: 7141 RVA: 0x00063DCF File Offset: 0x00061FCF
		// (set) Token: 0x06001BE6 RID: 7142 RVA: 0x00063DD7 File Offset: 0x00061FD7
		public INPCCondition[] conditions { get; protected set; }

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06001BE7 RID: 7143 RVA: 0x00063DE0 File Offset: 0x00061FE0
		public INPCReward[] rewards
		{
			get
			{
				return this.rewardsList.rewards;
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06001BE8 RID: 7144
		public abstract string displayName { get; }

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06001BE9 RID: 7145 RVA: 0x00063DED File Offset: 0x00061FED
		public virtual string displayDesc
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06001BEA RID: 7146 RVA: 0x00063DF0 File Offset: 0x00061FF0
		public virtual bool hasIcon
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06001BEB RID: 7147
		public abstract EItemRarity rarity { get; }

		// Token: 0x06001BEC RID: 7148 RVA: 0x00063DF4 File Offset: 0x00061FF4
		public bool areConditionsMet(Player player)
		{
			if (this.conditions != null)
			{
				for (int i = 0; i < this.conditions.Length; i++)
				{
					if (!this.conditions[i].isConditionMet(player))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x00063E30 File Offset: 0x00062030
		public void ApplyConditions(Player player)
		{
			if (this.conditions != null)
			{
				for (int i = 0; i < this.conditions.Length; i++)
				{
					this.conditions[i].ApplyCondition(player);
				}
			}
		}

		// Token: 0x06001BEE RID: 7150 RVA: 0x00063E66 File Offset: 0x00062066
		public void GrantRewards(Player player)
		{
			this.rewardsList.Grant(player);
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x00063E74 File Offset: 0x00062074
		public VendorElement(VendorAsset newOuterAsset, byte newIndex, Guid newGuid, ushort newLegacyId, uint newCost, INPCCondition[] newConditions, NPCRewardsList newRewardsList)
		{
			this.outerAsset = newOuterAsset;
			this.index = newIndex;
			this.id = newLegacyId;
			this.cost = newCost;
			this.conditions = newConditions;
			this.rewardsList = newRewardsList;
		}

		// Token: 0x06001BF0 RID: 7152 RVA: 0x00063EAA File Offset: 0x000620AA
		[Obsolete("Removed shouldSend parameter")]
		public void applyConditions(Player player, bool shouldSend)
		{
			this.ApplyConditions(player);
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x00063EB3 File Offset: 0x000620B3
		[Obsolete("Removed shouldSend parameter")]
		public void grantRewards(Player player, bool shouldSend)
		{
			this.GrantRewards(player);
		}

		// Token: 0x04000D24 RID: 3364
		protected NPCRewardsList rewardsList;
	}
}
