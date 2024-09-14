using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000366 RID: 870
	public class SkinAsset : Asset
	{
		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06001A41 RID: 6721 RVA: 0x0005EA2F File Offset: 0x0005CC2F
		public bool isPattern
		{
			get
			{
				return this._isPattern;
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001A42 RID: 6722 RVA: 0x0005EA37 File Offset: 0x0005CC37
		public bool hasSight
		{
			get
			{
				return this._hasSight;
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06001A43 RID: 6723 RVA: 0x0005EA3F File Offset: 0x0005CC3F
		public bool hasTactical
		{
			get
			{
				return this._hasTactical;
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06001A44 RID: 6724 RVA: 0x0005EA47 File Offset: 0x0005CC47
		public bool hasGrip
		{
			get
			{
				return this._hasGrip;
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06001A45 RID: 6725 RVA: 0x0005EA4F File Offset: 0x0005CC4F
		public bool hasBarrel
		{
			get
			{
				return this._hasBarrel;
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06001A46 RID: 6726 RVA: 0x0005EA57 File Offset: 0x0005CC57
		public bool hasMagazine
		{
			get
			{
				return this._hasMagazine;
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06001A47 RID: 6727 RVA: 0x0005EA5F File Offset: 0x0005CC5F
		public Material primarySkin
		{
			get
			{
				return this._primarySkin;
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06001A48 RID: 6728 RVA: 0x0005EA67 File Offset: 0x0005CC67
		public Dictionary<ushort, Material> secondarySkins
		{
			get
			{
				return this._secondarySkins;
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06001A49 RID: 6729 RVA: 0x0005EA6F File Offset: 0x0005CC6F
		public Material attachmentSkin
		{
			get
			{
				return this._attachmentSkin;
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06001A4A RID: 6730 RVA: 0x0005EA77 File Offset: 0x0005CC77
		public Material tertiarySkin
		{
			get
			{
				return this._tertiarySkin;
			}
		}

		/// <summary>
		/// Used by dawn and dusk skins which pull per-level lighting colors.
		/// </summary>
		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06001A4B RID: 6731 RVA: 0x0005EA7F File Offset: 0x0005CC7F
		// (set) Token: 0x06001A4C RID: 6732 RVA: 0x0005EA87 File Offset: 0x0005CC87
		public ELightingTime? lightingTime { get; private set; }

		/// <summary>
		/// Note: unfortunately it appears the stupid skin system always instantiated materials, but never destroys
		/// them... will need to clean this up, but it will be tricky because the game does not hold a reference to them.
		/// </summary>
		// Token: 0x06001A4D RID: 6733 RVA: 0x0005EA90 File Offset: 0x0005CC90
		public void SetMaterialProperties(Material instance)
		{
			if (this.lightingTime != null && LevelLighting.times != null)
			{
				LightingInfo lightingInfo = LevelLighting.times[(int)this.lightingTime.Value];
				instance.SetVector("_SunColor", lightingInfo.colors[0] * 1.5f);
				instance.SetVector("_RaysColor", lightingInfo.colors[10] * 1.5f);
				instance.SetVector("_SkyColor", lightingInfo.colors[3]);
				instance.SetVector("_EquatorColor", lightingInfo.colors[4]);
				instance.SetVector("_GroundColor", lightingInfo.colors[5]);
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06001A4E RID: 6734 RVA: 0x0005EB70 File Offset: 0x0005CD70
		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.SKIN;
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06001A4F RID: 6735 RVA: 0x0005EB73 File Offset: 0x0005CD73
		// (set) Token: 0x06001A50 RID: 6736 RVA: 0x0005EB7B File Offset: 0x0005CD7B
		public ERagdollEffect ragdollEffect { get; protected set; }

		// Token: 0x06001A51 RID: 6737 RVA: 0x0005EB84 File Offset: 0x0005CD84
		public SkinAsset()
		{
		}

		// Token: 0x06001A52 RID: 6738 RVA: 0x0005EB8C File Offset: 0x0005CD8C
		public SkinAsset(bool isPattern, Material primarySkin, Dictionary<ushort, Material> secondarySkins, Material attachmentSkin, Material tertiarySkin)
		{
			this._isPattern = isPattern;
			this._hasSight = true;
			this._hasTactical = true;
			this._hasGrip = true;
			this._hasBarrel = true;
			this._hasMagazine = true;
			this._primarySkin = primarySkin;
			this._secondarySkins = secondarySkins;
			this._attachmentSkin = attachmentSkin;
			this._tertiarySkin = tertiarySkin;
			this.overrideMeshes = new List<Mesh>(0);
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x0005EBF4 File Offset: 0x0005CDF4
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			if (this.id < 2000 && !base.OriginAllowsVanillaLegacyId && !data.ContainsKey("Bypass_ID_Limit"))
			{
				throw new NotSupportedException("ID < 2000");
			}
			this._isPattern = data.ContainsKey("Pattern");
			if (data.ContainsKey("LightingTime"))
			{
				this.lightingTime = new ELightingTime?(data.ParseEnum<ELightingTime>("LightingTime", ELightingTime.DAWN));
			}
			else
			{
				this.lightingTime = default(ELightingTime?);
			}
			this._hasSight = data.ContainsKey("Sight");
			this._hasTactical = data.ContainsKey("Tactical");
			this._hasGrip = data.ContainsKey("Grip");
			this._hasBarrel = data.ContainsKey("Barrel");
			this._hasMagazine = data.ContainsKey("Magazine");
			this.ragdollEffect = data.ParseEnum<ERagdollEffect>("Ragdoll_Effect", ERagdollEffect.NONE);
		}

		// Token: 0x04000C15 RID: 3093
		protected bool _isPattern;

		// Token: 0x04000C16 RID: 3094
		protected bool _hasSight;

		// Token: 0x04000C17 RID: 3095
		protected bool _hasTactical;

		// Token: 0x04000C18 RID: 3096
		protected bool _hasGrip;

		// Token: 0x04000C19 RID: 3097
		protected bool _hasBarrel;

		// Token: 0x04000C1A RID: 3098
		protected bool _hasMagazine;

		// Token: 0x04000C1B RID: 3099
		protected Material _primarySkin;

		// Token: 0x04000C1C RID: 3100
		protected Dictionary<ushort, Material> _secondarySkins;

		// Token: 0x04000C1D RID: 3101
		protected Material _attachmentSkin;

		// Token: 0x04000C1E RID: 3102
		protected Material _tertiarySkin;

		// Token: 0x04000C20 RID: 3104
		public List<Mesh> overrideMeshes;

		// Token: 0x04000C21 RID: 3105
		public bool hasStatTrackerTransformOverride;

		// Token: 0x04000C22 RID: 3106
		public Vector3 statTrackerPosition;

		// Token: 0x04000C23 RID: 3107
		public Quaternion statTrackerRotation;
	}
}
