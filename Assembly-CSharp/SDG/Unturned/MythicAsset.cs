using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000319 RID: 793
	public class MythicAsset : Asset
	{
		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x060017F0 RID: 6128 RVA: 0x0005839C File Offset: 0x0005659C
		// (set) Token: 0x060017F1 RID: 6129 RVA: 0x000583A4 File Offset: 0x000565A4
		public string particleTagName { get; protected set; }

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x060017F2 RID: 6130 RVA: 0x000583AD File Offset: 0x000565AD
		public GameObject systemArea
		{
			get
			{
				return this._systemArea;
			}
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x060017F3 RID: 6131 RVA: 0x000583B5 File Offset: 0x000565B5
		public GameObject systemHook
		{
			get
			{
				return this._systemHook;
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x060017F4 RID: 6132 RVA: 0x000583BD File Offset: 0x000565BD
		public GameObject systemFirst
		{
			get
			{
				return this._systemFirst;
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x060017F5 RID: 6133 RVA: 0x000583C5 File Offset: 0x000565C5
		public GameObject systemThird
		{
			get
			{
				return this._systemThird;
			}
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x060017F6 RID: 6134 RVA: 0x000583CD File Offset: 0x000565CD
		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.MYTHIC;
			}
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x000583D0 File Offset: 0x000565D0
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			if (this.id < 500 && !base.OriginAllowsVanillaLegacyId && !data.ContainsKey("Bypass_ID_Limit"))
			{
				throw new NotSupportedException("ID < 500");
			}
		}

		// Token: 0x04000AD6 RID: 2774
		protected GameObject _systemArea;

		// Token: 0x04000AD7 RID: 2775
		protected GameObject _systemHook;

		// Token: 0x04000AD8 RID: 2776
		protected GameObject _systemFirst;

		// Token: 0x04000AD9 RID: 2777
		protected GameObject _systemThird;
	}
}
