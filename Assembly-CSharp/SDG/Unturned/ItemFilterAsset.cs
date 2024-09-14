using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002E0 RID: 736
	public class ItemFilterAsset : ItemAsset
	{
		// Token: 0x1700038B RID: 907
		// (get) Token: 0x060015EF RID: 5615 RVA: 0x0005154D File Offset: 0x0004F74D
		public AudioClip use
		{
			get
			{
				return this._use;
			}
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x00051555 File Offset: 0x0004F755
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._use = bundle.load<AudioClip>("Use");
		}

		// Token: 0x04000935 RID: 2357
		protected AudioClip _use;
	}
}
