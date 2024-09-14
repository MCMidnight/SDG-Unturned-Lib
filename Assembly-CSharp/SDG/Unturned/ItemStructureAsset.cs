using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000301 RID: 769
	public class ItemStructureAsset : ItemPlaceableAsset
	{
		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x0600172E RID: 5934 RVA: 0x00054E19 File Offset: 0x00053019
		public GameObject structure
		{
			get
			{
				return this._structure;
			}
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x0600172F RID: 5935 RVA: 0x00054E21 File Offset: 0x00053021
		[Obsolete("Only one of Structure.prefab or Clip.prefab are loaded now as _structure")]
		public GameObject clip
		{
			get
			{
				return this._structure;
			}
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06001730 RID: 5936 RVA: 0x00054E29 File Offset: 0x00053029
		public GameObject nav
		{
			get
			{
				return this._nav;
			}
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06001731 RID: 5937 RVA: 0x00054E31 File Offset: 0x00053031
		public AudioClip use
		{
			get
			{
				return this._use;
			}
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06001732 RID: 5938 RVA: 0x00054E39 File Offset: 0x00053039
		public EConstruct construct
		{
			get
			{
				return this._construct;
			}
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06001733 RID: 5939 RVA: 0x00054E41 File Offset: 0x00053041
		public ushort health
		{
			get
			{
				return this._health;
			}
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06001734 RID: 5940 RVA: 0x00054E49 File Offset: 0x00053049
		public float range
		{
			get
			{
				return this._range;
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06001735 RID: 5941 RVA: 0x00054E51 File Offset: 0x00053051
		public Guid explosionGuid
		{
			get
			{
				return this._explosionGuid;
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06001736 RID: 5942 RVA: 0x00054E59 File Offset: 0x00053059
		public ushort explosion
		{
			[Obsolete]
			get
			{
				return this._explosion;
			}
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x00054E61 File Offset: 0x00053061
		public EffectAsset FindExplosionEffectAsset()
		{
			return Assets.FindEffectAssetByGuidOrLegacyId(this._explosionGuid, this._explosion);
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06001738 RID: 5944 RVA: 0x00054E74 File Offset: 0x00053074
		public bool isVulnerable
		{
			get
			{
				return this._isVulnerable;
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06001739 RID: 5945 RVA: 0x00054E7C File Offset: 0x0005307C
		public bool isRepairable
		{
			get
			{
				return this._isRepairable;
			}
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x0600173A RID: 5946 RVA: 0x00054E84 File Offset: 0x00053084
		public bool proofExplosion
		{
			get
			{
				return this._proofExplosion;
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x0600173B RID: 5947 RVA: 0x00054E8C File Offset: 0x0005308C
		public bool isUnpickupable
		{
			get
			{
				return this._isUnpickupable;
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x0600173C RID: 5948 RVA: 0x00054E94 File Offset: 0x00053094
		public bool isSalvageable
		{
			get
			{
				return this._isSalvageable;
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x0600173D RID: 5949 RVA: 0x00054E9C File Offset: 0x0005309C
		// (set) Token: 0x0600173E RID: 5950 RVA: 0x00054EA4 File Offset: 0x000530A4
		public float salvageDurationMultiplier { get; protected set; }

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x0600173F RID: 5951 RVA: 0x00054EAD File Offset: 0x000530AD
		public bool isSaveable
		{
			get
			{
				return this._isSaveable;
			}
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06001740 RID: 5952 RVA: 0x00054EB5 File Offset: 0x000530B5
		// (set) Token: 0x06001741 RID: 5953 RVA: 0x00054EBD File Offset: 0x000530BD
		public EArmorTier armorTier { get; protected set; }

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06001742 RID: 5954 RVA: 0x00054EC6 File Offset: 0x000530C6
		// (set) Token: 0x06001743 RID: 5955 RVA: 0x00054ECE File Offset: 0x000530CE
		public float foliageCutRadius { get; protected set; }

		/// <summary>
		/// Length of raycast downward from pivot to check floor is above terrain.
		/// Vanilla floors can be placed a maximum of 10 meters above terrain.
		/// </summary>
		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06001744 RID: 5956 RVA: 0x00054ED7 File Offset: 0x000530D7
		// (set) Token: 0x06001745 RID: 5957 RVA: 0x00054EDF File Offset: 0x000530DF
		public float terrainTestHeight { get; protected set; }

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06001746 RID: 5958 RVA: 0x00054EE8 File Offset: 0x000530E8
		public override bool shouldFriendlySentryTargetUser
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001747 RID: 5959 RVA: 0x00054EEB File Offset: 0x000530EB
		public override bool canBeUsedInSafezone(SafezoneNode safezone, bool byAdmin)
		{
			return !safezone.noBuildables;
		}

		// Token: 0x06001748 RID: 5960 RVA: 0x00054EF8 File Offset: 0x000530F8
		public override void BuildDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			base.BuildDescription(builder, itemInstance);
			if (builder.shouldRestrictToLegacyContent)
			{
				return;
			}
			builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Buildable_Health", this._health), 20000);
			EArmorTier armorTier = this.armorTier;
			if (armorTier != EArmorTier.LOW)
			{
				if (armorTier == EArmorTier.HIGH)
				{
					builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Buildable_ArmorTier_High"), 20000);
				}
			}
			else
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Buildable_ArmorTier_Low"), 20000);
			}
			if (this._isUnpickupable)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Buildable_CannotPickup"), 20000);
			}
			else if (!this._isSalvageable)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Buildable_CannotSalvage"), 20000);
			}
			if (!this.isRepairable)
			{
				builder.Append(PlayerDashboardInventoryUI.FormatStatColor(PlayerDashboardInventoryUI.localization.format("ItemDescription_Buildable_CannotRepair"), false), 20001);
			}
			if (this.proofExplosion)
			{
				builder.Append(PlayerDashboardInventoryUI.FormatStatColor(PlayerDashboardInventoryUI.localization.format("ItemDescription_Buildable_ExplosionProof"), true), 19999);
			}
			if (!this._isVulnerable)
			{
				builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_Buildable_Invulnerable"), 20000);
			}
		}

		// Token: 0x06001749 RID: 5961 RVA: 0x00055044 File Offset: 0x00053244
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			bool flag;
			if (data.ParseBool("Has_Clip_Prefab", true))
			{
				this._structure = bundle.load<GameObject>("Clip");
				if (this.structure == null)
				{
					flag = true;
					Assets.reportError(this, "missing \"Clip\" GameObject, loading \"Structure\" GameObject instead");
				}
				else
				{
					flag = false;
					AssetValidation.searchGameObjectForErrors(this, this.structure);
				}
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				this._structure = bundle.load<GameObject>("Structure");
				if (this.structure == null)
				{
					Assets.reportError(this, "missing \"Structure\" GameObject");
				}
				else
				{
					AssetValidation.searchGameObjectForErrors(this, this.structure);
					ServerPrefabUtil.RemoveClientComponents(this._structure);
					this.RemoveClientComponents(this._structure);
				}
			}
			this._nav = bundle.load<GameObject>("Nav");
			this._use = base.LoadRedirectableAsset<AudioClip>(bundle, "Use", data, "PlacementAudioClip");
			this._construct = (EConstruct)Enum.Parse(typeof(EConstruct), data.GetString("Construct", null), true);
			this._health = data.ParseUInt16("Health", 0);
			this._range = data.ParseFloat("Range", 0f);
			this._explosion = data.ParseGuidOrLegacyId("Explosion", out this._explosionGuid);
			this.canBeDamaged = data.ParseBool("Can_Be_Damaged", true);
			this.eligibleForPooling = data.ParseBool("Eligible_For_Pooling", true);
			this.requiresPillars = data.ParseBool("Requires_Pillars", true);
			this._isVulnerable = data.ContainsKey("Vulnerable");
			this._isRepairable = !data.ContainsKey("Unrepairable");
			this._proofExplosion = data.ContainsKey("Proof_Explosion");
			this._isUnpickupable = data.ContainsKey("Unpickupable");
			this._isSalvageable = !data.ContainsKey("Unsalvageable");
			this.salvageDurationMultiplier = data.ParseFloat("Salvage_Duration_Multiplier", 1f);
			this._isSaveable = !data.ContainsKey("Unsaveable");
			if (data.ContainsKey("Armor_Tier"))
			{
				this.armorTier = (EArmorTier)Enum.Parse(typeof(EArmorTier), data.GetString("Armor_Tier", null), true);
			}
			else if (this.name.Contains("Metal") || this.name.Contains("Brick"))
			{
				this.armorTier = EArmorTier.HIGH;
			}
			else
			{
				this.armorTier = EArmorTier.LOW;
			}
			this.foliageCutRadius = data.ParseFloat("Foliage_Cut_Radius", 6f);
			this.terrainTestHeight = data.ParseFloat("Terrain_Test_Height", 10f);
		}

		// Token: 0x0600174A RID: 5962 RVA: 0x000552DC File Offset: 0x000534DC
		protected override AudioReference GetDefaultInventoryAudio()
		{
			if (this.name.Contains("Metal", 3))
			{
				return new AudioReference("core.masterbundle", "Sounds/Inventory/SmallMetal.asset");
			}
			if (this.size_x <= 1 || this.size_y <= 1)
			{
				return new AudioReference("core.masterbundle", "Sounds/Inventory/LightMetalEquipment.asset");
			}
			if (this.size_x <= 2 || this.size_y <= 2)
			{
				return new AudioReference("core.masterbundle", "Sounds/Inventory/MediumMetalEquipment.asset");
			}
			return new AudioReference("core.masterbundle", "Sounds/Inventory/HeavyMetalEquipment.asset");
		}

		/// <summary>
		/// Called on the dedicated server to optimize client prefab for server usage.
		/// </summary>
		// Token: 0x0600174B RID: 5963 RVA: 0x00055360 File Offset: 0x00053560
		private void RemoveClientComponents(GameObject gameObject)
		{
			foreach (object obj in gameObject.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.name == "Climb" || transform.name == "Hatch" || transform.name == "Slot" || transform.name == "Door" || transform.name == "Gate")
				{
					ItemStructureAsset.transformsToDestroy.Add(transform);
				}
			}
			foreach (Transform transform2 in ItemStructureAsset.transformsToDestroy)
			{
				Object.DestroyImmediate(transform2.gameObject, true);
			}
			ItemStructureAsset.transformsToDestroy.Clear();
		}

		// Token: 0x04000A37 RID: 2615
		protected GameObject _structure;

		// Token: 0x04000A38 RID: 2616
		protected GameObject _nav;

		// Token: 0x04000A39 RID: 2617
		protected AudioClip _use;

		// Token: 0x04000A3A RID: 2618
		protected EConstruct _construct;

		// Token: 0x04000A3B RID: 2619
		protected ushort _health;

		// Token: 0x04000A3C RID: 2620
		protected float _range;

		// Token: 0x04000A3D RID: 2621
		private Guid _explosionGuid;

		// Token: 0x04000A3E RID: 2622
		protected ushort _explosion;

		/// <summary>
		/// If false this structure cannot take damage.
		/// </summary>
		// Token: 0x04000A3F RID: 2623
		public bool canBeDamaged = true;

		/// <summary>
		/// Modded structures can disable pooling if they have custom incompatible logic.
		/// </summary>
		// Token: 0x04000A40 RID: 2624
		public bool eligibleForPooling = true;

		// Token: 0x04000A41 RID: 2625
		public bool requiresPillars = true;

		// Token: 0x04000A42 RID: 2626
		protected bool _isVulnerable;

		// Token: 0x04000A43 RID: 2627
		protected bool _isRepairable;

		// Token: 0x04000A44 RID: 2628
		protected bool _proofExplosion;

		// Token: 0x04000A45 RID: 2629
		protected bool _isUnpickupable;

		// Token: 0x04000A46 RID: 2630
		protected bool _isSalvageable;

		// Token: 0x04000A48 RID: 2632
		protected bool _isSaveable;

		// Token: 0x04000A4C RID: 2636
		private static List<Transform> transformsToDestroy = new List<Transform>();
	}
}
