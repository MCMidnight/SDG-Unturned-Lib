using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002E8 RID: 744
	public class ItemGrowerAsset : ItemAsset
	{
		// Token: 0x1700039E RID: 926
		// (get) Token: 0x0600161C RID: 5660 RVA: 0x00051B26 File Offset: 0x0004FD26
		public AudioClip use
		{
			get
			{
				return this._use;
			}
		}

		// Token: 0x0600161D RID: 5661 RVA: 0x00051B2E File Offset: 0x0004FD2E
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._use = bundle.load<AudioClip>("Use");
		}

		// Token: 0x0400094E RID: 2382
		protected AudioClip _use;
	}
}
