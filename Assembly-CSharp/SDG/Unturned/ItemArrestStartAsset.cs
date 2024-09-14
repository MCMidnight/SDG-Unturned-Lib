using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002CC RID: 716
	public class ItemArrestStartAsset : ItemAsset
	{
		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x060014E0 RID: 5344 RVA: 0x0004D724 File Offset: 0x0004B924
		public AudioClip use
		{
			get
			{
				return this._use;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x060014E1 RID: 5345 RVA: 0x0004D72C File Offset: 0x0004B92C
		public ushort strength
		{
			get
			{
				return this._strength;
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x060014E2 RID: 5346 RVA: 0x0004D734 File Offset: 0x0004B934
		public override bool shouldFriendlySentryTargetUser
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x0004D737 File Offset: 0x0004B937
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._use = bundle.load<AudioClip>("Use");
			this._strength = data.ParseUInt16("Strength", 0);
		}

		// Token: 0x04000860 RID: 2144
		protected AudioClip _use;

		// Token: 0x04000861 RID: 2145
		protected ushort _strength;
	}
}
