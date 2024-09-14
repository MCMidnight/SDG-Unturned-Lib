using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002B3 RID: 691
	public class EffectAsset : Asset
	{
		// Token: 0x170002DF RID: 735
		// (get) Token: 0x060014AF RID: 5295 RVA: 0x0004CEDB File Offset: 0x0004B0DB
		public GameObject effect
		{
			get
			{
				return this._effect;
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x060014B0 RID: 5296 RVA: 0x0004CEE3 File Offset: 0x0004B0E3
		public GameObject[] splatters
		{
			get
			{
				return this._splatters;
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x060014B1 RID: 5297 RVA: 0x0004CEEB File Offset: 0x0004B0EB
		public bool gore
		{
			get
			{
				return this._gore;
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x060014B2 RID: 5298 RVA: 0x0004CEF3 File Offset: 0x0004B0F3
		public byte splatter
		{
			get
			{
				return this._splatter;
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x060014B3 RID: 5299 RVA: 0x0004CEFB File Offset: 0x0004B0FB
		public float splatterLifetime
		{
			get
			{
				return this._splatterLifetime;
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x060014B4 RID: 5300 RVA: 0x0004CF03 File Offset: 0x0004B103
		public float splatterLifetimeSpread
		{
			get
			{
				return this._splatterLifetimeSpread;
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x060014B5 RID: 5301 RVA: 0x0004CF0B File Offset: 0x0004B10B
		public bool splatterLiquid
		{
			get
			{
				return this._splatterLiquid;
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x060014B6 RID: 5302 RVA: 0x0004CF13 File Offset: 0x0004B113
		public EPlayerTemperature splatterTemperature
		{
			get
			{
				return this._splatterTemperature;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x060014B7 RID: 5303 RVA: 0x0004CF1B File Offset: 0x0004B11B
		public byte splatterPreload
		{
			get
			{
				return this._splatterPreload;
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x060014B8 RID: 5304 RVA: 0x0004CF23 File Offset: 0x0004B123
		public float lifetime
		{
			get
			{
				return this._lifetime;
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x060014B9 RID: 5305 RVA: 0x0004CF2B File Offset: 0x0004B12B
		public float lifetimeSpread
		{
			get
			{
				return this._lifetimeSpread;
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x060014BA RID: 5306 RVA: 0x0004CF33 File Offset: 0x0004B133
		public bool isStatic
		{
			get
			{
				return this._isStatic;
			}
		}

		/// <summary>
		/// If true the music option is respected when this effect is used by ambiance volume.
		/// </summary>
		// Token: 0x170002EB RID: 747
		// (get) Token: 0x060014BB RID: 5307 RVA: 0x0004CF3B File Offset: 0x0004B13B
		// (set) Token: 0x060014BC RID: 5308 RVA: 0x0004CF43 File Offset: 0x0004B143
		public bool isMusic { get; private set; }

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x060014BD RID: 5309 RVA: 0x0004CF4C File Offset: 0x0004B14C
		public byte preload
		{
			get
			{
				return this._preload;
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x060014BE RID: 5310 RVA: 0x0004CF54 File Offset: 0x0004B154
		public ushort blast
		{
			[Obsolete]
			get
			{
				return this._blast;
			}
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x0004CF5C File Offset: 0x0004B15C
		public EffectAsset FindBlastmarkEffectAsset()
		{
			return Assets.FindEffectAssetByGuidOrLegacyId(this.blastmarkEffectGuid, this.blast);
		}

		/// <summary>
		/// In multiplayer the effect will be spawned for players within this radius.
		/// </summary>
		// Token: 0x170002EE RID: 750
		// (get) Token: 0x060014C0 RID: 5312 RVA: 0x0004CF6F File Offset: 0x0004B16F
		// (set) Token: 0x060014C1 RID: 5313 RVA: 0x0004CF77 File Offset: 0x0004B177
		public float relevantDistance { get; protected set; }

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x060014C2 RID: 5314 RVA: 0x0004CF80 File Offset: 0x0004B180
		// (set) Token: 0x060014C3 RID: 5315 RVA: 0x0004CF88 File Offset: 0x0004B188
		public bool spawnOnDedicatedServer { get; protected set; }

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x060014C4 RID: 5316 RVA: 0x0004CF91 File Offset: 0x0004B191
		// (set) Token: 0x060014C5 RID: 5317 RVA: 0x0004CF99 File Offset: 0x0004B199
		public bool randomizeRotation { get; protected set; }

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x060014C6 RID: 5318 RVA: 0x0004CFA2 File Offset: 0x0004B1A2
		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.EFFECT;
			}
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x0004CFA8 File Offset: 0x0004B1A8
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			if (this.id < 200 && !base.OriginAllowsVanillaLegacyId && !data.ContainsKey("Bypass_ID_Limit"))
			{
				throw new NotSupportedException("ID < 200");
			}
			this._effect = bundle.load<GameObject>("Effect");
			if (this.effect == null)
			{
				throw new NotSupportedException("Missing effect gameobject");
			}
			this._gore = data.ContainsKey("Gore");
			this._splatters = new GameObject[(int)data.ParseUInt8("Splatter", 0)];
			for (int i = 0; i < this.splatters.Length; i++)
			{
				this.splatters[i] = bundle.load<GameObject>("Splatter_" + i.ToString());
				if (this.splatters[i] == null)
				{
					Assets.reportError(this, string.Format("missing 'Splatter_{0}' gameobject", i));
				}
			}
			this._splatter = data.ParseUInt8("Splatters", 0);
			this._splatterLiquid = data.ContainsKey("Splatter_Liquid");
			if (data.ContainsKey("Splatter_Temperature"))
			{
				this._splatterTemperature = (EPlayerTemperature)Enum.Parse(typeof(EPlayerTemperature), data.GetString("Splatter_Temperature", null), true);
			}
			else
			{
				this._splatterTemperature = EPlayerTemperature.NONE;
			}
			this._splatterLifetime = data.ParseFloat("Splatter_Lifetime", 0f);
			if (data.ContainsKey("Splatter_Lifetime_Spread"))
			{
				this._splatterLifetimeSpread = data.ParseFloat("Splatter_Lifetime_Spread", 0f);
			}
			else
			{
				this._splatterLifetimeSpread = 1f;
			}
			this._lifetime = data.ParseFloat("Lifetime", 0f);
			if (data.ContainsKey("Lifetime_Spread"))
			{
				this._lifetimeSpread = data.ParseFloat("Lifetime_Spread", 0f);
			}
			else
			{
				this._lifetimeSpread = 4f;
			}
			this._isStatic = data.ContainsKey("Static");
			this.isMusic = data.ParseBool("Is_Music", false);
			if (data.ContainsKey("Preload"))
			{
				this._preload = data.ParseUInt8("Preload", 0);
			}
			else
			{
				this._preload = 1;
			}
			if (data.ContainsKey("Splatter_Preload"))
			{
				this._splatterPreload = data.ParseUInt8("Splatter_Preload", 0);
			}
			else
			{
				this._splatterPreload = (byte)(Mathf.CeilToInt((float)this.splatter / (float)this.splatters.Length) * (int)this.preload);
			}
			this._blast = data.ParseGuidOrLegacyId("Blast", out this.blastmarkEffectGuid);
			this.relevantDistance = data.ParseFloat("Relevant_Distance", -1f);
			this.spawnOnDedicatedServer = data.ContainsKey("Spawn_On_Dedicated_Server");
			if (data.ContainsKey("Randomize_Rotation"))
			{
				this.randomizeRotation = data.ParseBool("Randomize_Rotation", false);
			}
			else
			{
				this.randomizeRotation = true;
			}
			this.cameraShakeRadius = data.ParseFloat("CameraShake_Radius", 0f);
			this.cameraShakeMagnitudeDegrees = data.ParseFloat("CameraShake_MagnitudeDegrees", 0f);
		}

		// Token: 0x04000774 RID: 1908
		protected GameObject _effect;

		// Token: 0x04000775 RID: 1909
		protected GameObject[] _splatters;

		// Token: 0x04000776 RID: 1910
		private bool _gore;

		// Token: 0x04000777 RID: 1911
		private byte _splatter;

		// Token: 0x04000778 RID: 1912
		private float _splatterLifetime;

		// Token: 0x04000779 RID: 1913
		private float _splatterLifetimeSpread;

		// Token: 0x0400077A RID: 1914
		private bool _splatterLiquid;

		// Token: 0x0400077B RID: 1915
		private EPlayerTemperature _splatterTemperature;

		// Token: 0x0400077C RID: 1916
		private byte _splatterPreload;

		// Token: 0x0400077D RID: 1917
		private float _lifetime;

		// Token: 0x0400077E RID: 1918
		private float _lifetimeSpread;

		// Token: 0x0400077F RID: 1919
		private bool _isStatic;

		// Token: 0x04000781 RID: 1921
		private byte _preload;

		// Token: 0x04000782 RID: 1922
		public Guid blastmarkEffectGuid;

		// Token: 0x04000783 RID: 1923
		private ushort _blast;

		// Token: 0x04000787 RID: 1927
		public float cameraShakeRadius;

		// Token: 0x04000788 RID: 1928
		public float cameraShakeMagnitudeDegrees;
	}
}
