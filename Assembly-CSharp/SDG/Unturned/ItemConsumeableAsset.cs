using System;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x020002DB RID: 731
	public class ItemConsumeableAsset : ItemWeaponAsset
	{
		// Token: 0x1700036A RID: 874
		// (get) Token: 0x060015AB RID: 5547 RVA: 0x000507A6 File Offset: 0x0004E9A6
		public AudioClip use
		{
			get
			{
				return this._use;
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x060015AC RID: 5548 RVA: 0x000507AE File Offset: 0x0004E9AE
		public byte health
		{
			get
			{
				return this._health;
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x060015AD RID: 5549 RVA: 0x000507B6 File Offset: 0x0004E9B6
		public byte food
		{
			get
			{
				return this._food;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x060015AE RID: 5550 RVA: 0x000507BE File Offset: 0x0004E9BE
		public byte water
		{
			get
			{
				return this._water;
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x060015AF RID: 5551 RVA: 0x000507C6 File Offset: 0x0004E9C6
		public byte virus
		{
			get
			{
				return this._virus;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x060015B0 RID: 5552 RVA: 0x000507CE File Offset: 0x0004E9CE
		public byte disinfectant
		{
			get
			{
				return this._disinfectant;
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x060015B1 RID: 5553 RVA: 0x000507D6 File Offset: 0x0004E9D6
		public byte energy
		{
			get
			{
				return this._energy;
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x060015B2 RID: 5554 RVA: 0x000507DE File Offset: 0x0004E9DE
		public byte vision
		{
			get
			{
				return this._vision;
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x060015B3 RID: 5555 RVA: 0x000507E6 File Offset: 0x0004E9E6
		// (set) Token: 0x060015B4 RID: 5556 RVA: 0x000507EE File Offset: 0x0004E9EE
		public sbyte oxygen { get; protected set; }

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x060015B5 RID: 5557 RVA: 0x000507F7 File Offset: 0x0004E9F7
		public uint warmth
		{
			get
			{
				return this._warmth;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x060015B6 RID: 5558 RVA: 0x000507FF File Offset: 0x0004E9FF
		// (set) Token: 0x060015B7 RID: 5559 RVA: 0x00050807 File Offset: 0x0004EA07
		public ItemConsumeableAsset.Bleeding bleedingModifier { get; protected set; }

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x060015B8 RID: 5560 RVA: 0x00050810 File Offset: 0x0004EA10
		// (set) Token: 0x060015B9 RID: 5561 RVA: 0x00050818 File Offset: 0x0004EA18
		public ItemConsumeableAsset.Bones bonesModifier { get; protected set; }

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x060015BA RID: 5562 RVA: 0x00050821 File Offset: 0x0004EA21
		public bool hasAid
		{
			get
			{
				return this._hasAid;
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x060015BB RID: 5563 RVA: 0x00050829 File Offset: 0x0004EA29
		// (set) Token: 0x060015BC RID: 5564 RVA: 0x00050831 File Offset: 0x0004EA31
		public bool foodConstrainsWater { get; protected set; }

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x060015BD RID: 5565 RVA: 0x0005083A File Offset: 0x0004EA3A
		// (set) Token: 0x060015BE RID: 5566 RVA: 0x00050842 File Offset: 0x0004EA42
		public bool shouldDeleteAfterUse { get; protected set; }

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x060015BF RID: 5567 RVA: 0x0005084B File Offset: 0x0004EA4B
		public override bool showQuality
		{
			get
			{
				return this.type == EItemType.FOOD || this.type == EItemType.WATER;
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x060015C0 RID: 5568 RVA: 0x00050863 File Offset: 0x0004EA63
		public Guid ExplosionEffectGuid
		{
			get
			{
				return this._explosionEffectGuid;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x060015C1 RID: 5569 RVA: 0x0005086B File Offset: 0x0004EA6B
		public ushort explosion
		{
			[Obsolete]
			get
			{
				return this._explosion;
			}
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x00050873 File Offset: 0x0004EA73
		public bool IsExplosionEffectRefNull()
		{
			return this._explosion == 0 && GuidExtension.IsEmpty(this._explosionEffectGuid);
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x0005088A File Offset: 0x0004EA8A
		public EffectAsset FindExplosionEffectAsset()
		{
			return Assets.FindEffectAssetByGuidOrLegacyId(this._explosionEffectGuid, this._explosion);
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x060015C4 RID: 5572 RVA: 0x0005089D File Offset: 0x0004EA9D
		// (set) Token: 0x060015C5 RID: 5573 RVA: 0x000508A5 File Offset: 0x0004EAA5
		public bool IsExplosive { get; private set; }

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x060015C6 RID: 5574 RVA: 0x000508AE File Offset: 0x0004EAAE
		public INPCReward[] questRewards
		{
			get
			{
				return this.questRewardsList.rewards;
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x060015C7 RID: 5575 RVA: 0x000508BB File Offset: 0x0004EABB
		// (set) Token: 0x060015C8 RID: 5576 RVA: 0x000508C3 File Offset: 0x0004EAC3
		public SpawnTableReward itemRewards { get; protected set; }

		/// <summary>
		/// Canned beans have skins from April Fools.
		/// </summary>
		// Token: 0x1700037F RID: 895
		// (get) Token: 0x060015C9 RID: 5577 RVA: 0x000508CC File Offset: 0x0004EACC
		protected override bool doesItemTypeHaveSkins
		{
			get
			{
				return this.id == 13;
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x060015CA RID: 5578 RVA: 0x000508D8 File Offset: 0x0004EAD8
		public override bool shouldFriendlySentryTargetUser
		{
			get
			{
				return this.IsExplosive || base.shouldFriendlySentryTargetUser;
			}
		}

		// Token: 0x060015CB RID: 5579 RVA: 0x000508EA File Offset: 0x0004EAEA
		public void GrantQuestRewards(Player player)
		{
			this.questRewardsList.Grant(player);
		}

		// Token: 0x060015CC RID: 5580 RVA: 0x000508F8 File Offset: 0x0004EAF8
		public override void BuildDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			base.BuildDescription(builder, itemInstance);
			if (builder.shouldRestrictToLegacyContent)
			{
				return;
			}
			if (this._health > 0)
			{
				string arg = PlayerDashboardInventoryUI.FormatStatColor(this._health.ToString(), true);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Consumeable_HealthPositive", arg), 9999);
			}
			if (this._food > 0)
			{
				string arg2 = PlayerDashboardInventoryUI.FormatStatColor(this._food.ToString(), true);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Consumeable_FoodPositive", arg2), 9999);
			}
			if (this._water > 0)
			{
				string arg3 = PlayerDashboardInventoryUI.FormatStatColor(this.water.ToString(), true);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Consumeable_WaterPositive", arg3), 9999);
			}
			if (this._virus > 0)
			{
				string arg4 = PlayerDashboardInventoryUI.FormatStatColor(this.virus.ToString(), false);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Consumeable_VirusNegative", arg4), 10001);
			}
			if (this._disinfectant > 0)
			{
				string arg5 = PlayerDashboardInventoryUI.FormatStatColor(this._disinfectant.ToString(), true);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Consumeable_VirusPositive", arg5), 9999);
			}
			if (this._energy > 0)
			{
				string arg6 = PlayerDashboardInventoryUI.FormatStatColor(this._energy.ToString(), true);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Consumeable_StaminaPositive", arg6), 9999);
			}
			if (this.oxygen > 0)
			{
				string arg7 = PlayerDashboardInventoryUI.FormatStatColor(this.oxygen.ToString(), true);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Consumeable_OxygenPositive", arg7), 9999);
			}
			else if (this.oxygen < 0)
			{
				string arg8 = PlayerDashboardInventoryUI.FormatStatColor(((int)(-(int)this.oxygen)).ToString(), false);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Consumeable_OxygenNegative", arg8), 10001);
			}
			int num = Mathf.RoundToInt(this._warmth / 12.5f);
			if (num > 0)
			{
				string arg9 = PlayerDashboardInventoryUI.FormatStatColor(string.Format("{0} s", num), true);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Consumeable_WarmthPositive", arg9), 9999);
			}
			if (itemInstance.quality < 50 && this._food + this._water > 0)
			{
				builder.Append(PlayerDashboardInventoryUI.FormatStatColor(PlayerDashboardInventoryUI.localization.format("ItemDescription_ConsumeableMoldy"), false), 10001);
			}
			ItemConsumeableAsset.Bleeding bleedingModifier = this.bleedingModifier;
			if (bleedingModifier != ItemConsumeableAsset.Bleeding.Heal)
			{
				if (bleedingModifier == ItemConsumeableAsset.Bleeding.Cut)
				{
					builder.Append(PlayerDashboardInventoryUI.FormatStatColor(PlayerDashboardInventoryUI.localization.format("ItemDescription_ConsumeableBleeding_Cut"), false), 10001);
				}
			}
			else
			{
				builder.Append(PlayerDashboardInventoryUI.FormatStatColor(PlayerDashboardInventoryUI.localization.format("ItemDescription_ConsumeableBleeding_Heal"), true), 9999);
			}
			ItemConsumeableAsset.Bones bonesModifier = this.bonesModifier;
			if (bonesModifier != ItemConsumeableAsset.Bones.Heal)
			{
				if (bonesModifier == ItemConsumeableAsset.Bones.Break)
				{
					builder.Append(PlayerDashboardInventoryUI.FormatStatColor(PlayerDashboardInventoryUI.localization.format("ItemDescription_ConsumeableBones_Break"), false), 10001);
				}
			}
			else
			{
				builder.Append(PlayerDashboardInventoryUI.FormatStatColor(PlayerDashboardInventoryUI.localization.format("ItemDescription_ConsumeableBones_Heal"), true), 9999);
			}
			if (this.IsExplosive)
			{
				builder.Append(PlayerDashboardInventoryUI.FormatStatColor(PlayerDashboardInventoryUI.localization.format("ItemDescription_Consumeable_Explosive"), false), 2000);
				base.BuildExplosiveDescription(builder, itemInstance);
			}
		}

		// Token: 0x060015CD RID: 5581 RVA: 0x00050C54 File Offset: 0x0004EE54
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._use = base.LoadRedirectableAsset<AudioClip>(bundle, "Use", data, "ConsumeAudioClip");
			this._health = data.ParseUInt8("Health", 0);
			this._food = data.ParseUInt8("Food", 0);
			this._water = data.ParseUInt8("Water", 0);
			this._virus = data.ParseUInt8("Virus", 0);
			this._disinfectant = data.ParseUInt8("Disinfectant", 0);
			this._energy = data.ParseUInt8("Energy", 0);
			this._vision = data.ParseUInt8("Vision", 0);
			this.oxygen = data.ParseInt8("Oxygen", 0);
			this._warmth = data.ParseUInt32("Warmth", 0U);
			this.experience = data.ParseInt32("Experience", 0);
			if (data.ContainsKey("Bleeding"))
			{
				this.bleedingModifier = ItemConsumeableAsset.Bleeding.Heal;
			}
			else
			{
				this.bleedingModifier = data.ParseEnum<ItemConsumeableAsset.Bleeding>("Bleeding_Modifier", ItemConsumeableAsset.Bleeding.None);
			}
			if (data.ContainsKey("Broken"))
			{
				this.bonesModifier = ItemConsumeableAsset.Bones.Heal;
			}
			else
			{
				this.bonesModifier = data.ParseEnum<ItemConsumeableAsset.Bones>("Bones_Modifier", ItemConsumeableAsset.Bones.None);
			}
			this._hasAid = data.ContainsKey("Aid");
			this.foodConstrainsWater = (this.food >= this.water);
			this.shouldDeleteAfterUse = data.ParseBool("Should_Delete_After_Use", true);
			this.questRewardsList.Parse(data, localization, this, "Quest_Rewards", "Quest_Reward_");
			ushort tableID = data.ParseUInt16("Item_Reward_Spawn_ID", 0);
			int min = data.ParseInt32("Min_Item_Rewards", 0);
			int max = data.ParseInt32("Max_Item_Rewards", 0);
			this.itemRewards = new SpawnTableReward(tableID, min, max);
			this._explosion = data.ParseGuidOrLegacyId("Explosion", out this._explosionEffectGuid);
			this.IsExplosive = !this.IsExplosionEffectRefNull();
		}

		// Token: 0x060015CE RID: 5582 RVA: 0x00050E33 File Offset: 0x0004F033
		[Obsolete("Removed shouldSend parameter")]
		public void grantQuestRewards(Player player, bool shouldSend)
		{
			this.GrantQuestRewards(player);
		}

		// Token: 0x04000911 RID: 2321
		protected AudioClip _use;

		// Token: 0x04000912 RID: 2322
		private byte _health;

		// Token: 0x04000913 RID: 2323
		private byte _food;

		// Token: 0x04000914 RID: 2324
		private byte _water;

		// Token: 0x04000915 RID: 2325
		private byte _virus;

		// Token: 0x04000916 RID: 2326
		private byte _disinfectant;

		// Token: 0x04000917 RID: 2327
		private byte _energy;

		// Token: 0x04000918 RID: 2328
		private byte _vision;

		// Token: 0x0400091A RID: 2330
		private uint _warmth;

		/// <summary>
		/// Experience to add or subtract when used. Defaults to zero.
		/// </summary>
		// Token: 0x0400091B RID: 2331
		public int experience;

		// Token: 0x0400091E RID: 2334
		private bool _hasAid;

		// Token: 0x04000921 RID: 2337
		private Guid _explosionEffectGuid;

		// Token: 0x04000922 RID: 2338
		protected ushort _explosion;

		// Token: 0x04000924 RID: 2340
		protected NPCRewardsList questRewardsList;

		// Token: 0x0200091D RID: 2333
		public enum Bleeding
		{
			// Token: 0x04003256 RID: 12886
			None,
			// Token: 0x04003257 RID: 12887
			Heal,
			// Token: 0x04003258 RID: 12888
			Cut
		}

		// Token: 0x0200091E RID: 2334
		public enum Bones
		{
			// Token: 0x0400325A RID: 12890
			None,
			// Token: 0x0400325B RID: 12891
			Heal,
			// Token: 0x0400325C RID: 12892
			Break
		}
	}
}
