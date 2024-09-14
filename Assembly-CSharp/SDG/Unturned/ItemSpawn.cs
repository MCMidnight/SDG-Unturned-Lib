using System;

namespace SDG.Unturned
{
	// Token: 0x020004CC RID: 1228
	public class ItemSpawn
	{
		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x0600258B RID: 9611 RVA: 0x0009556D File Offset: 0x0009376D
		public ushort item
		{
			get
			{
				return this._item;
			}
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x00095575 File Offset: 0x00093775
		public ItemSpawn(ushort newItem)
		{
			this._item = newItem;
		}

		// Token: 0x0400134B RID: 4939
		private ushort _item;
	}
}
