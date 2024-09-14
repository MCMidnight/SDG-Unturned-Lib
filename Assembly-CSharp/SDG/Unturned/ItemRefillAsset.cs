using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002FB RID: 763
	public class ItemRefillAsset : ItemAsset
	{
		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x060016D7 RID: 5847 RVA: 0x000540DA File Offset: 0x000522DA
		public AudioClip use
		{
			get
			{
				return this._use;
			}
		}

		/// <summary>
		/// Kept for backwards compatibility with plugins.
		/// </summary>
		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x060016D8 RID: 5848 RVA: 0x000540E2 File Offset: 0x000522E2
		[Obsolete("Replaced by separate stats for each water type")]
		public byte water
		{
			get
			{
				return MathfEx.RoundAndClampToByte(this.cleanWater);
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x060016D9 RID: 5849 RVA: 0x000540EF File Offset: 0x000522EF
		// (set) Token: 0x060016DA RID: 5850 RVA: 0x000540F7 File Offset: 0x000522F7
		public float cleanHealth { get; protected set; }

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x060016DB RID: 5851 RVA: 0x00054100 File Offset: 0x00052300
		// (set) Token: 0x060016DC RID: 5852 RVA: 0x00054108 File Offset: 0x00052308
		public float saltyHealth { get; protected set; }

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x060016DD RID: 5853 RVA: 0x00054111 File Offset: 0x00052311
		// (set) Token: 0x060016DE RID: 5854 RVA: 0x00054119 File Offset: 0x00052319
		public float dirtyHealth { get; protected set; }

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x060016DF RID: 5855 RVA: 0x00054122 File Offset: 0x00052322
		// (set) Token: 0x060016E0 RID: 5856 RVA: 0x0005412A File Offset: 0x0005232A
		public float cleanFood { get; protected set; }

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x060016E1 RID: 5857 RVA: 0x00054133 File Offset: 0x00052333
		// (set) Token: 0x060016E2 RID: 5858 RVA: 0x0005413B File Offset: 0x0005233B
		public float saltyFood { get; protected set; }

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x060016E3 RID: 5859 RVA: 0x00054144 File Offset: 0x00052344
		// (set) Token: 0x060016E4 RID: 5860 RVA: 0x0005414C File Offset: 0x0005234C
		public float dirtyFood { get; protected set; }

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x060016E5 RID: 5861 RVA: 0x00054155 File Offset: 0x00052355
		// (set) Token: 0x060016E6 RID: 5862 RVA: 0x0005415D File Offset: 0x0005235D
		public float cleanWater { get; protected set; }

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x060016E7 RID: 5863 RVA: 0x00054166 File Offset: 0x00052366
		// (set) Token: 0x060016E8 RID: 5864 RVA: 0x0005416E File Offset: 0x0005236E
		public float saltyWater { get; protected set; }

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x060016E9 RID: 5865 RVA: 0x00054177 File Offset: 0x00052377
		// (set) Token: 0x060016EA RID: 5866 RVA: 0x0005417F File Offset: 0x0005237F
		public float dirtyWater { get; protected set; }

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x060016EB RID: 5867 RVA: 0x00054188 File Offset: 0x00052388
		// (set) Token: 0x060016EC RID: 5868 RVA: 0x00054190 File Offset: 0x00052390
		public float cleanVirus { get; protected set; }

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x060016ED RID: 5869 RVA: 0x00054199 File Offset: 0x00052399
		// (set) Token: 0x060016EE RID: 5870 RVA: 0x000541A1 File Offset: 0x000523A1
		public float saltyVirus { get; protected set; }

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x060016EF RID: 5871 RVA: 0x000541AA File Offset: 0x000523AA
		// (set) Token: 0x060016F0 RID: 5872 RVA: 0x000541B2 File Offset: 0x000523B2
		public float dirtyVirus { get; protected set; }

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x060016F1 RID: 5873 RVA: 0x000541BB File Offset: 0x000523BB
		// (set) Token: 0x060016F2 RID: 5874 RVA: 0x000541C3 File Offset: 0x000523C3
		public float cleanStamina { get; protected set; }

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x060016F3 RID: 5875 RVA: 0x000541CC File Offset: 0x000523CC
		// (set) Token: 0x060016F4 RID: 5876 RVA: 0x000541D4 File Offset: 0x000523D4
		public float saltyStamina { get; protected set; }

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x060016F5 RID: 5877 RVA: 0x000541DD File Offset: 0x000523DD
		// (set) Token: 0x060016F6 RID: 5878 RVA: 0x000541E5 File Offset: 0x000523E5
		public float dirtyStamina { get; protected set; }

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x060016F7 RID: 5879 RVA: 0x000541EE File Offset: 0x000523EE
		// (set) Token: 0x060016F8 RID: 5880 RVA: 0x000541F6 File Offset: 0x000523F6
		public float cleanOxygen { get; protected set; }

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x060016F9 RID: 5881 RVA: 0x000541FF File Offset: 0x000523FF
		// (set) Token: 0x060016FA RID: 5882 RVA: 0x00054207 File Offset: 0x00052407
		public float saltyOxygen { get; protected set; }

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x060016FB RID: 5883 RVA: 0x00054210 File Offset: 0x00052410
		// (set) Token: 0x060016FC RID: 5884 RVA: 0x00054218 File Offset: 0x00052418
		public float dirtyOxygen { get; protected set; }

		// Token: 0x060016FD RID: 5885 RVA: 0x00054221 File Offset: 0x00052421
		public float GetRefillHealth(ERefillWaterType refillWaterType)
		{
			switch (refillWaterType)
			{
			case ERefillWaterType.CLEAN:
				return this.cleanHealth;
			case ERefillWaterType.SALTY:
				return this.saltyHealth;
			case ERefillWaterType.DIRTY:
				return this.dirtyHealth;
			default:
				return 0f;
			}
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x00054253 File Offset: 0x00052453
		public float GetRefillFood(ERefillWaterType refillWaterType)
		{
			switch (refillWaterType)
			{
			case ERefillWaterType.CLEAN:
				return this.cleanFood;
			case ERefillWaterType.SALTY:
				return this.saltyFood;
			case ERefillWaterType.DIRTY:
				return this.dirtyFood;
			default:
				return 0f;
			}
		}

		// Token: 0x060016FF RID: 5887 RVA: 0x00054285 File Offset: 0x00052485
		public float GetRefillWater(ERefillWaterType refillWaterType)
		{
			switch (refillWaterType)
			{
			case ERefillWaterType.CLEAN:
				return this.cleanWater;
			case ERefillWaterType.SALTY:
				return this.saltyWater;
			case ERefillWaterType.DIRTY:
				return this.dirtyWater;
			default:
				return 0f;
			}
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x000542B7 File Offset: 0x000524B7
		public float GetRefillVirus(ERefillWaterType refillWaterType)
		{
			switch (refillWaterType)
			{
			case ERefillWaterType.CLEAN:
				return this.cleanVirus;
			case ERefillWaterType.SALTY:
				return this.saltyVirus;
			case ERefillWaterType.DIRTY:
				return this.dirtyVirus;
			default:
				return 0f;
			}
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x000542E9 File Offset: 0x000524E9
		public float GetRefillStamina(ERefillWaterType refillWaterType)
		{
			switch (refillWaterType)
			{
			case ERefillWaterType.CLEAN:
				return this.cleanStamina;
			case ERefillWaterType.SALTY:
				return this.saltyStamina;
			case ERefillWaterType.DIRTY:
				return this.dirtyStamina;
			default:
				return 0f;
			}
		}

		// Token: 0x06001702 RID: 5890 RVA: 0x0005431B File Offset: 0x0005251B
		public float GetRefillOxygen(ERefillWaterType refillWaterType)
		{
			switch (refillWaterType)
			{
			case ERefillWaterType.CLEAN:
				return this.cleanOxygen;
			case ERefillWaterType.SALTY:
				return this.saltyOxygen;
			case ERefillWaterType.DIRTY:
				return this.dirtyOxygen;
			default:
				return 0f;
			}
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x00054350 File Offset: 0x00052550
		public override byte[] getState(EItemOrigin origin)
		{
			byte[] array = new byte[1];
			if (origin == EItemOrigin.ADMIN)
			{
				array[0] = 1;
			}
			else
			{
				array[0] = 0;
			}
			return array;
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x00054374 File Offset: 0x00052574
		public override void BuildDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			base.BuildDescription(builder, itemInstance);
			ERefillWaterType refillWaterType = (ERefillWaterType)itemInstance.state[0];
			string key;
			switch (refillWaterType)
			{
			case ERefillWaterType.EMPTY:
				key = "Empty";
				break;
			case ERefillWaterType.CLEAN:
				key = "Clean";
				break;
			case ERefillWaterType.SALTY:
				key = "Salty";
				break;
			case ERefillWaterType.DIRTY:
				key = "Dirty";
				break;
			default:
				key = "Full";
				break;
			}
			builder.Append(PlayerDashboardInventoryUI.localization.format("Refill", PlayerDashboardInventoryUI.localization.format(key)), 2000);
			if (builder.shouldRestrictToLegacyContent)
			{
				return;
			}
			int num = Mathf.RoundToInt(this.GetRefillHealth(refillWaterType));
			if (num > 0)
			{
				string arg = PlayerDashboardInventoryUI.FormatStatColor(num.ToString(), true);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Consumeable_HealthPositive", arg), 9999);
			}
			else if (num < 0)
			{
				string arg2 = PlayerDashboardInventoryUI.FormatStatColor((-num).ToString(), false);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Consumeable_HealthNegative", arg2), 10001);
			}
			int num2 = Mathf.RoundToInt(this.GetRefillFood(refillWaterType));
			if (num2 > 0)
			{
				string arg3 = PlayerDashboardInventoryUI.FormatStatColor(num2.ToString(), true);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Consumeable_FoodPositive", arg3), 9999);
			}
			else if (num2 < 0)
			{
				string arg4 = PlayerDashboardInventoryUI.FormatStatColor((-num2).ToString(), false);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Consumeable_FoodNegative", arg4), 10001);
			}
			int num3 = Mathf.RoundToInt(this.GetRefillWater(refillWaterType));
			if (num3 > 0)
			{
				string arg5 = PlayerDashboardInventoryUI.FormatStatColor(num3.ToString(), true);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Consumeable_WaterPositive", arg5), 9999);
			}
			else if (num3 < 0)
			{
				string arg6 = PlayerDashboardInventoryUI.FormatStatColor((-num3).ToString(), false);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Consumeable_WaterNegative", arg6), 10001);
			}
			int num4 = Mathf.RoundToInt(this.GetRefillVirus(refillWaterType));
			if (num4 > 0)
			{
				string arg7 = PlayerDashboardInventoryUI.FormatStatColor(num4.ToString(), true);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Consumeable_VirusPositive", arg7), 9999);
			}
			else if (num4 < 0)
			{
				string arg8 = PlayerDashboardInventoryUI.FormatStatColor((-num4).ToString(), false);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Consumeable_VirusNegative", arg8), 10001);
			}
			int num5 = Mathf.RoundToInt(this.GetRefillStamina(refillWaterType));
			if (num5 > 0)
			{
				string arg9 = PlayerDashboardInventoryUI.FormatStatColor(num5.ToString(), true);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Consumeable_StaminaPositive", arg9), 9999);
			}
			else if (num5 < 0)
			{
				string arg10 = PlayerDashboardInventoryUI.FormatStatColor((-num5).ToString(), false);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Consumeable_StaminaNegative", arg10), 10001);
			}
			int num6 = Mathf.RoundToInt(this.GetRefillOxygen(refillWaterType));
			if (num6 > 0)
			{
				string arg11 = PlayerDashboardInventoryUI.FormatStatColor(num6.ToString(), true);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Consumeable_OxygenPositive", arg11), 9999);
				return;
			}
			if (num6 < 0)
			{
				string arg12 = PlayerDashboardInventoryUI.FormatStatColor((-num6).ToString(), false);
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Consumeable_OxygenNegative", arg12), 10001);
			}
		}

		// Token: 0x06001705 RID: 5893 RVA: 0x000546C0 File Offset: 0x000528C0
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._use = base.LoadRedirectableAsset<AudioClip>(bundle, "Use", data, "ConsumeAudioClip");
			float defaultValue = data.ParseFloat("Water", 0f);
			this.cleanHealth = data.ParseFloat("Clean_Health", 0f);
			this.saltyHealth = data.ParseFloat("Salty_Health", this.cleanHealth * 0.25f);
			this.dirtyHealth = data.ParseFloat("Dirty_Health", this.cleanHealth * 0.6f);
			this.cleanFood = data.ParseFloat("Clean_Food", 0f);
			this.saltyFood = data.ParseFloat("Salty_Food", this.cleanFood * 0.25f);
			this.dirtyFood = data.ParseFloat("Dirty_Food", this.cleanFood * 0.6f);
			this.cleanWater = data.ParseFloat("Clean_Water", defaultValue);
			this.saltyWater = data.ParseFloat("Salty_Water", this.cleanWater * 0.25f);
			this.dirtyWater = data.ParseFloat("Dirty_Water", this.cleanWater * 0.6f);
			this.cleanVirus = data.ParseFloat("Clean_Virus", 0f);
			this.saltyVirus = data.ParseFloat("Salty_Virus", this.cleanWater * -0.75f);
			this.dirtyVirus = data.ParseFloat("Dirty_Virus", this.cleanWater * -0.39999998f);
			this.cleanStamina = data.ParseFloat("Clean_Stamina", 0f);
			this.saltyStamina = data.ParseFloat("Salty_Stamina", this.cleanStamina * 0.25f);
			this.dirtyStamina = data.ParseFloat("Dirty_Stamina", this.cleanStamina * 0.6f);
			this.cleanOxygen = data.ParseFloat("Clean_Oxygen", 0f);
			this.saltyOxygen = data.ParseFloat("Salty_Oxygen", this.cleanOxygen * 0.25f);
			this.dirtyOxygen = data.ParseFloat("Dirty_Oxygen", this.cleanOxygen * 0.6f);
		}

		// Token: 0x04000A01 RID: 2561
		protected AudioClip _use;
	}
}
