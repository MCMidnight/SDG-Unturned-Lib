using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002E7 RID: 743
	public class ItemGripAsset : ItemCaliberAsset
	{
		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06001618 RID: 5656 RVA: 0x00051AF1 File Offset: 0x0004FCF1
		public GameObject grip
		{
			get
			{
				return this._grip;
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06001619 RID: 5657 RVA: 0x00051AF9 File Offset: 0x0004FCF9
		[Obsolete]
		public bool isBipod
		{
			get
			{
				return this._isBipod;
			}
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x00051B01 File Offset: 0x0004FD01
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._grip = base.loadRequiredAsset<GameObject>(bundle, "Grip");
		}

		// Token: 0x0400094D RID: 2381
		protected GameObject _grip;
	}
}
