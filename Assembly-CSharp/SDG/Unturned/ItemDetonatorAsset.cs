using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002DE RID: 734
	public class ItemDetonatorAsset : ItemAsset
	{
		// Token: 0x17000384 RID: 900
		// (get) Token: 0x060015DF RID: 5599 RVA: 0x00051252 File Offset: 0x0004F452
		public AudioClip use
		{
			get
			{
				return this._use;
			}
		}

		// Token: 0x060015E0 RID: 5600 RVA: 0x0005125A File Offset: 0x0004F45A
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._use = bundle.load<AudioClip>("Use");
		}

		// Token: 0x0400092B RID: 2347
		protected AudioClip _use;
	}
}
