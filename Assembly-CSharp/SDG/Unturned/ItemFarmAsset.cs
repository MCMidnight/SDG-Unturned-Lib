using System;

namespace SDG.Unturned
{
	// Token: 0x020002DF RID: 735
	public class ItemFarmAsset : ItemBarricadeAsset
	{
		// Token: 0x17000385 RID: 901
		// (get) Token: 0x060015E2 RID: 5602 RVA: 0x0005127E File Offset: 0x0004F47E
		public uint growth
		{
			get
			{
				return this._growth;
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x060015E3 RID: 5603 RVA: 0x00051286 File Offset: 0x0004F486
		public ushort grow
		{
			get
			{
				return this._grow;
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x060015E4 RID: 5604 RVA: 0x0005128E File Offset: 0x0004F48E
		// (set) Token: 0x060015E5 RID: 5605 RVA: 0x00051296 File Offset: 0x0004F496
		public bool ignoreSoilRestrictions { get; protected set; }

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x060015E6 RID: 5606 RVA: 0x0005129F File Offset: 0x0004F49F
		// (set) Token: 0x060015E7 RID: 5607 RVA: 0x000512A7 File Offset: 0x0004F4A7
		public bool canFertilize { get; protected set; }

		/// <summary>
		/// If true, harvesting has a chance to provide a second item.
		/// </summary>
		// Token: 0x17000389 RID: 905
		// (get) Token: 0x060015E8 RID: 5608 RVA: 0x000512B0 File Offset: 0x0004F4B0
		// (set) Token: 0x060015E9 RID: 5609 RVA: 0x000512B8 File Offset: 0x0004F4B8
		public bool isAffectedByAgricultureSkill { get; protected set; }

		/// <summary>
		/// If true, rain will finish growing the plant.
		/// </summary>
		// Token: 0x1700038A RID: 906
		// (get) Token: 0x060015EA RID: 5610 RVA: 0x000512C1 File Offset: 0x0004F4C1
		// (set) Token: 0x060015EB RID: 5611 RVA: 0x000512C9 File Offset: 0x0004F4C9
		public bool shouldRainAffectGrowth { get; protected set; }

		// Token: 0x060015EC RID: 5612 RVA: 0x000512D4 File Offset: 0x0004F4D4
		public override void BuildDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			base.BuildDescription(builder, itemInstance);
			if (builder.shouldRestrictToLegacyContent)
			{
				return;
			}
			if (this.grow != 0)
			{
				ItemAsset itemAsset = Assets.find(EAssetType.ITEM, this.grow) as ItemAsset;
				if (itemAsset != null)
				{
					builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Farmable_GrowSpecificItem", string.Concat(new string[]
					{
						"<color=",
						Palette.hex(ItemTool.getRarityColorUI(itemAsset.rarity)),
						">",
						itemAsset.itemName,
						"</color>"
					})), 2000);
				}
			}
			builder.stringBuilder.Clear();
			if (!this.ignoreSoilRestrictions)
			{
				builder.stringBuilder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Farmable_RequiresSoil"));
			}
			if (this.canFertilize)
			{
				if (builder.stringBuilder.Length > 0)
				{
					builder.stringBuilder.Append(' ');
				}
				builder.stringBuilder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Farmable_CanFertilize"));
			}
			if (this.isAffectedByAgricultureSkill)
			{
				if (builder.stringBuilder.Length > 0)
				{
					builder.stringBuilder.Append(' ');
				}
				builder.stringBuilder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Farmable_AffectedByAgricultureSkill"));
			}
			if (this.shouldRainAffectGrowth)
			{
				if (builder.stringBuilder.Length > 0)
				{
					builder.stringBuilder.Append(' ');
				}
				builder.stringBuilder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Farmable_AffectedByRain"));
			}
			if (builder.stringBuilder.Length > 0)
			{
				builder.Append(builder.stringBuilder.ToString(), 15000);
			}
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x00051480 File Offset: 0x0004F680
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._growth = data.ParseUInt32("Growth", 0U);
			this._grow = data.ParseUInt16("Grow", 0);
			this.growSpawnTableGuid = data.ParseGuid("Grow_SpawnTable", default(Guid));
			this.ignoreSoilRestrictions = data.ContainsKey("Ignore_Soil_Restrictions");
			this.canFertilize = data.ParseBool("Allow_Fertilizer", true);
			this.harvestRewardExperience = data.ParseUInt32("Harvest_Reward_Experience", 1U);
			this.isAffectedByAgricultureSkill = data.ParseBool("Affected_By_Agriculture_Skill", true);
			this.shouldRainAffectGrowth = data.ParseBool("Rain_Affects_Growth", true);
			this.harvestRewardsList.Parse(data, localization, this, "Harvest_Rewards", "Harvest_Reward_");
		}

		// Token: 0x0400092C RID: 2348
		protected uint _growth;

		// Token: 0x0400092D RID: 2349
		protected ushort _grow;

		// Token: 0x0400092E RID: 2350
		public Guid growSpawnTableGuid;

		/// <summary>
		/// Amount of experience to reward harvesting player.
		/// </summary>
		// Token: 0x04000931 RID: 2353
		public uint harvestRewardExperience;

		/// <summary>
		/// NPC rewards to grant upon harvesting the crop.
		/// </summary>
		// Token: 0x04000934 RID: 2356
		internal NPCRewardsList harvestRewardsList;
	}
}
