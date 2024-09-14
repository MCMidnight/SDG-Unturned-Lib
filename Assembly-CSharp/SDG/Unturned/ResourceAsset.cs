using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000361 RID: 865
	public class ResourceAsset : Asset
	{
		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06001A1F RID: 6687 RVA: 0x0005DD88 File Offset: 0x0005BF88
		public string resourceName
		{
			get
			{
				ENPCHoliday holidayRestriction = this.holidayRestriction;
				if (holidayRestriction == ENPCHoliday.HALLOWEEN)
				{
					return this._resourceName + " [HW]";
				}
				if (holidayRestriction != ENPCHoliday.CHRISTMAS)
				{
					return this._resourceName;
				}
				return this._resourceName + " [XMAS]";
			}
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06001A20 RID: 6688 RVA: 0x0005DDCE File Offset: 0x0005BFCE
		public override string FriendlyName
		{
			get
			{
				return this.resourceName;
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06001A21 RID: 6689 RVA: 0x0005DDD6 File Offset: 0x0005BFD6
		public GameObject modelGameObject
		{
			get
			{
				return this._modelGameObject;
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001A22 RID: 6690 RVA: 0x0005DDDE File Offset: 0x0005BFDE
		public GameObject stumpGameObject
		{
			get
			{
				return this._stumpGameObject;
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001A23 RID: 6691 RVA: 0x0005DDE6 File Offset: 0x0005BFE6
		public GameObject skyboxGameObject
		{
			get
			{
				return this._skyboxGameObject;
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06001A24 RID: 6692 RVA: 0x0005DDEE File Offset: 0x0005BFEE
		public GameObject debrisGameObject
		{
			get
			{
				return this._debrisGameObject;
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06001A25 RID: 6693 RVA: 0x0005DDF6 File Offset: 0x0005BFF6
		// (set) Token: 0x06001A26 RID: 6694 RVA: 0x0005DDFE File Offset: 0x0005BFFE
		public Material skyboxMaterial { get; private set; }

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06001A27 RID: 6695 RVA: 0x0005DE07 File Offset: 0x0005C007
		public Guid explosionGuid
		{
			get
			{
				return this._explosionGuid;
			}
		}

		// Token: 0x06001A28 RID: 6696 RVA: 0x0005DE0F File Offset: 0x0005C00F
		public EffectAsset FindExplosionEffectAsset()
		{
			return Assets.FindEffectAssetByGuidOrLegacyId(this._explosionGuid, this.explosion);
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06001A29 RID: 6697 RVA: 0x0005DE22 File Offset: 0x0005C022
		// (set) Token: 0x06001A2A RID: 6698 RVA: 0x0005DE2A File Offset: 0x0005C02A
		public bool vulnerableToFists { get; protected set; }

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001A2B RID: 6699 RVA: 0x0005DE33 File Offset: 0x0005C033
		// (set) Token: 0x06001A2C RID: 6700 RVA: 0x0005DE3B File Offset: 0x0005C03B
		public bool vulnerableToAllMeleeWeapons { get; protected set; }

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06001A2D RID: 6701 RVA: 0x0005DE44 File Offset: 0x0005C044
		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.RESOURCE;
			}
		}

		/// <summary>
		/// Only activated during this holiday.
		/// </summary>
		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06001A2E RID: 6702 RVA: 0x0005DE47 File Offset: 0x0005C047
		// (set) Token: 0x06001A2F RID: 6703 RVA: 0x0005DE4F File Offset: 0x0005C04F
		public ENPCHoliday holidayRestriction { get; protected set; }

		/// <summary>
		/// Get asset ref to replace this one for holiday, or null if it should not be redirected.
		/// </summary>
		// Token: 0x06001A30 RID: 6704 RVA: 0x0005DE58 File Offset: 0x0005C058
		public AssetReference<ResourceAsset> getHolidayRedirect()
		{
			ENPCHoliday activeHoliday = HolidayUtil.getActiveHoliday();
			if (activeHoliday == ENPCHoliday.HALLOWEEN)
			{
				return this.halloweenRedirect;
			}
			if (activeHoliday == ENPCHoliday.CHRISTMAS)
			{
				return this.christmasRedirect;
			}
			return AssetReference<ResourceAsset>.invalid;
		}

		// Token: 0x06001A31 RID: 6705 RVA: 0x0005DE88 File Offset: 0x0005C088
		protected void applyDefaultLODs(LODGroup lod, bool fade)
		{
			LOD[] lods = lod.GetLODs();
			lods[0].screenRelativeTransitionHeight = (fade ? 0.7f : 0.6f);
			lods[1].screenRelativeTransitionHeight = (fade ? 0.5f : 0.4f);
			lods[2].screenRelativeTransitionHeight = 0.15f;
			lods[3].screenRelativeTransitionHeight = 0.03f;
			lod.SetLODs(lods);
		}

		// Token: 0x06001A32 RID: 6706 RVA: 0x0005DEFC File Offset: 0x0005C0FC
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			if (this.id < 50 && !base.OriginAllowsVanillaLegacyId && !data.ContainsKey("Bypass_ID_Limit"))
			{
				throw new NotSupportedException("ID < 50");
			}
			this.isSpeedTree = false;
			this.defaultLODWeights = data.ContainsKey("SpeedTree_Default_LOD_Weights");
			this._resourceName = localization.format("Name");
			if (data.ParseBool("Has_Clip_Prefab", true))
			{
				this._modelGameObject = bundle.load<GameObject>("Resource_Clip");
				if (this._modelGameObject == null)
				{
					Assets.reportError(this, "missing \"Resource_Clip\" GameObject, loading \"Resource\" GameObject instead");
				}
				this._stumpGameObject = bundle.load<GameObject>("Stump_Clip");
				if (this._stumpGameObject == null)
				{
					Assets.reportError(this, "missing \"Stump_Clip\" GameObject, loading \"Stump\" GameObject instead");
				}
			}
			if (this._modelGameObject == null)
			{
				this._modelGameObject = bundle.load<GameObject>("Resource");
				if (this._modelGameObject == null)
				{
					Assets.reportError(this, "missing \"Resource\" GameObject");
				}
				else
				{
					ServerPrefabUtil.RemoveClientComponents(this._modelGameObject);
				}
			}
			if (this._stumpGameObject == null)
			{
				this._stumpGameObject = bundle.load<GameObject>("Stump");
				if (this._stumpGameObject == null)
				{
					Assets.reportError(this, "missing \"Stump\" GameObject");
				}
				else
				{
					ServerPrefabUtil.RemoveClientComponents(this._stumpGameObject);
				}
			}
			if (this._modelGameObject != null)
			{
				this._modelGameObject.SetTagIfUntaggedRecursively("Resource");
			}
			if (this._stumpGameObject != null)
			{
				this._stumpGameObject.SetTagIfUntaggedRecursively("Resource");
			}
			if (this._skyboxGameObject != null)
			{
				this._skyboxGameObject.SetTagIfUntaggedRecursively("Resource");
			}
			this.health = data.ParseUInt16("Health", 0);
			this.scale = Mathf.Abs(data.ParseFloat("Scale", 0f));
			this.verticalOffset = data.ParseFloat("Vertical_Offset", -0.75f);
			this.explosion = data.ParseGuidOrLegacyId("Explosion", out this._explosionGuid);
			this.log = data.ParseUInt16("Log", 0);
			this.stick = data.ParseUInt16("Stick", 0);
			this.rewardID = data.ParseUInt16("Reward_ID", 0);
			this.rewardXP = data.ParseUInt32("Reward_XP", 0U);
			if (data.ContainsKey("Reward_Min"))
			{
				this.rewardMin = data.ParseUInt8("Reward_Min", 0);
			}
			else
			{
				this.rewardMin = 6;
			}
			if (data.ContainsKey("Reward_Max"))
			{
				this.rewardMax = data.ParseUInt8("Reward_Max", 0);
			}
			else
			{
				this.rewardMax = 9;
			}
			this.bladeID = data.ParseUInt8("BladeID", 0);
			this.vulnerableToFists = data.ParseBool("Vulnerable_To_Fists", false);
			this.vulnerableToAllMeleeWeapons = data.ParseBool("Vulnerable_To_All_Melee_Weapons", false);
			this.reset = data.ParseFloat("Reset", 0f);
			this.isForage = data.ContainsKey("Forage");
			if (this.isForage && this._modelGameObject != null)
			{
				Transform transform = this._modelGameObject.transform.Find("Forage");
				if (transform != null)
				{
					transform.gameObject.layer = 14;
				}
				else
				{
					Assets.reportError(this, "foragable resource missing \"Forage\" GameObject");
				}
			}
			this.forageRewardExperience = data.ParseUInt32("Forage_Reward_Experience", 1U);
			if (this.isForage)
			{
				this.interactabilityText = localization.read("Interact");
				this.interactabilityText = ItemTool.filterRarityRichText(this.interactabilityText);
			}
			this.hasDebris = !data.ContainsKey("No_Debris");
			if (data.ContainsKey("Holiday_Restriction"))
			{
				this.holidayRestriction = (ENPCHoliday)Enum.Parse(typeof(ENPCHoliday), data.GetString("Holiday_Restriction", null), true);
				if (this.holidayRestriction == ENPCHoliday.NONE)
				{
					Assets.reportError(this, "has no holiday restriction, so value is ignored");
				}
			}
			else
			{
				this.holidayRestriction = ENPCHoliday.NONE;
			}
			this.christmasRedirect = data.readAssetReference("Christmas_Redirect");
			this.halloweenRedirect = data.readAssetReference("Halloween_Redirect");
			this.chart = data.ParseEnum<EObjectChart>("Chart", EObjectChart.NONE);
			this.shouldExcludeFromLevelBatching = data.ParseBool("Exclude_From_Level_Batching", false);
			this.shouldExcludeFromLevelBatching |= this.isSpeedTree;
		}

		// Token: 0x06001A33 RID: 6707 RVA: 0x0005E346 File Offset: 0x0005C546
		internal string OnGetRewardSpawnTableErrorContext()
		{
			return this.FriendlyName + " reward";
		}

		// Token: 0x04000BEF RID: 3055
		private static List<MeshFilter> meshes = new List<MeshFilter>();

		// Token: 0x04000BF0 RID: 3056
		private static Shader shader;

		// Token: 0x04000BF1 RID: 3057
		protected string _resourceName;

		// Token: 0x04000BF2 RID: 3058
		protected GameObject _modelGameObject;

		// Token: 0x04000BF3 RID: 3059
		protected GameObject _stumpGameObject;

		// Token: 0x04000BF4 RID: 3060
		protected GameObject _skyboxGameObject;

		// Token: 0x04000BF5 RID: 3061
		protected GameObject _debrisGameObject;

		// Token: 0x04000BF7 RID: 3063
		public ushort health;

		// Token: 0x04000BF8 RID: 3064
		public uint rewardXP;

		// Token: 0x04000BF9 RID: 3065
		public float scale;

		// Token: 0x04000BFA RID: 3066
		public float verticalOffset;

		// Token: 0x04000BFB RID: 3067
		private Guid _explosionGuid;

		// Token: 0x04000BFC RID: 3068
		public ushort explosion;

		// Token: 0x04000BFD RID: 3069
		public ushort log;

		// Token: 0x04000BFE RID: 3070
		public ushort stick;

		// Token: 0x04000BFF RID: 3071
		public byte rewardMin;

		// Token: 0x04000C00 RID: 3072
		public byte rewardMax;

		// Token: 0x04000C01 RID: 3073
		public ushort rewardID;

		// Token: 0x04000C02 RID: 3074
		public bool isForage;

		/// <summary>
		/// Amount of experience to reward foraging player.
		/// </summary>
		// Token: 0x04000C03 RID: 3075
		public uint forageRewardExperience;

		/// <summary>
		/// Forageable resource message.
		/// </summary>
		// Token: 0x04000C04 RID: 3076
		public string interactabilityText;

		// Token: 0x04000C05 RID: 3077
		public bool hasDebris;

		/// <summary>
		/// Weapon must have matching blade ID to damage tree.
		/// Both weapons and trees default to zero so they can be damaged by default.
		/// </summary>
		// Token: 0x04000C06 RID: 3078
		public byte bladeID;

		// Token: 0x04000C07 RID: 3079
		public float reset;

		/// <summary>
		/// Whether this asset is a SpeedTree model, can be false if an option to use the old models is enabled.
		/// </summary>
		// Token: 0x04000C0A RID: 3082
		public bool isSpeedTree;

		/// <summary>
		/// Whether to reset SpeedTree LOD weights to default.
		/// </summary>
		// Token: 0x04000C0B RID: 3083
		public bool defaultLODWeights;

		/// <summary>
		/// Tree to use during the Christmas event instead.
		/// </summary>
		// Token: 0x04000C0D RID: 3085
		public AssetReference<ResourceAsset> christmasRedirect;

		/// <summary>
		/// Tree to use during the Halloween event instead.
		/// </summary>
		// Token: 0x04000C0E RID: 3086
		public AssetReference<ResourceAsset> halloweenRedirect;

		// Token: 0x04000C0F RID: 3087
		public EObjectChart chart;

		// Token: 0x04000C10 RID: 3088
		public bool shouldExcludeFromLevelBatching;
	}
}
