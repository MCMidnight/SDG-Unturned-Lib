using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000303 RID: 771
	public class ItemTacticalAsset : ItemCaliberAsset
	{
		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x0600174F RID: 5967 RVA: 0x00055499 File Offset: 0x00053699
		public GameObject tactical
		{
			get
			{
				return this._tactical;
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06001750 RID: 5968 RVA: 0x000554A1 File Offset: 0x000536A1
		public bool isLaser
		{
			get
			{
				return this._isLaser;
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06001751 RID: 5969 RVA: 0x000554A9 File Offset: 0x000536A9
		public bool isLight
		{
			get
			{
				return this._isLight;
			}
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06001752 RID: 5970 RVA: 0x000554B1 File Offset: 0x000536B1
		// (set) Token: 0x06001753 RID: 5971 RVA: 0x000554B9 File Offset: 0x000536B9
		public PlayerSpotLightConfig lightConfig { get; protected set; }

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06001754 RID: 5972 RVA: 0x000554C2 File Offset: 0x000536C2
		public bool isRangefinder
		{
			get
			{
				return this._isRangefinder;
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06001755 RID: 5973 RVA: 0x000554CA File Offset: 0x000536CA
		public bool isMelee
		{
			get
			{
				return this._isMelee;
			}
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06001756 RID: 5974 RVA: 0x000554D2 File Offset: 0x000536D2
		// (set) Token: 0x06001757 RID: 5975 RVA: 0x000554DA File Offset: 0x000536DA
		public Color laserColor { get; protected set; }

		// Token: 0x06001758 RID: 5976 RVA: 0x000554E4 File Offset: 0x000536E4
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._tactical = base.loadRequiredAsset<GameObject>(bundle, "Tactical");
			this._isLaser = data.ContainsKey("Laser");
			this._isLight = data.ContainsKey("Light");
			if (this.isLight)
			{
				this.lightConfig = new PlayerSpotLightConfig(data);
			}
			this._isRangefinder = data.ContainsKey("Rangefinder");
			this._isMelee = data.ContainsKey("Melee");
			Color color = data.LegacyParseColor("Laser_Color", Color.red);
			color = MathfEx.Clamp01(color);
			color.a = 1f;
			this.laserColor = color;
		}

		// Token: 0x04000A4D RID: 2637
		protected GameObject _tactical;

		// Token: 0x04000A4E RID: 2638
		private bool _isLaser;

		// Token: 0x04000A4F RID: 2639
		private bool _isLight;

		// Token: 0x04000A51 RID: 2641
		private bool _isRangefinder;

		// Token: 0x04000A52 RID: 2642
		private bool _isMelee;
	}
}
