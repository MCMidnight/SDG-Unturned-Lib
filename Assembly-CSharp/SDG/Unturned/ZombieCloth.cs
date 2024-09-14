using System;

namespace SDG.Unturned
{
	// Token: 0x02000525 RID: 1317
	public class ZombieCloth
	{
		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06002934 RID: 10548 RVA: 0x000AF7B5 File Offset: 0x000AD9B5
		public ushort item
		{
			get
			{
				return this._item;
			}
		}

		// Token: 0x06002935 RID: 10549 RVA: 0x000AF7BD File Offset: 0x000AD9BD
		public ZombieCloth(ushort newItem)
		{
			this._item = newItem;
		}

		// Token: 0x040015F2 RID: 5618
		private ushort _item;
	}
}
