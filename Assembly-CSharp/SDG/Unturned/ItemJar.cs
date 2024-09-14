using System;

namespace SDG.Unturned
{
	// Token: 0x0200049A RID: 1178
	public class ItemJar
	{
		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x060024A1 RID: 9377 RVA: 0x0009217F File Offset: 0x0009037F
		public Item item
		{
			get
			{
				return this._item;
			}
		}

		// Token: 0x060024A2 RID: 9378 RVA: 0x00092187 File Offset: 0x00090387
		public ItemAsset GetAsset()
		{
			if (this._item == null)
			{
				return null;
			}
			return this._item.GetAsset();
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x000921A0 File Offset: 0x000903A0
		public T GetAsset<T>() where T : ItemAsset
		{
			if (this._item == null)
			{
				return default(T);
			}
			return this._item.GetAsset<T>();
		}

		// Token: 0x060024A4 RID: 9380 RVA: 0x000921CC File Offset: 0x000903CC
		public ItemJar(Item newItem)
		{
			this._item = newItem;
			ItemAsset asset = this.item.GetAsset();
			if (asset == null)
			{
				return;
			}
			this.size_x = asset.size_x;
			this.size_y = asset.size_y;
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x00092210 File Offset: 0x00090410
		public ItemJar(byte new_x, byte new_y, byte newRot, Item newItem)
		{
			this.x = new_x;
			this.y = new_y;
			this.rot = newRot;
			this._item = newItem;
			ItemAsset asset = this.item.GetAsset();
			if (asset == null)
			{
				return;
			}
			this.size_x = asset.size_x;
			this.size_y = asset.size_y;
		}

		// Token: 0x040012AC RID: 4780
		public byte x;

		// Token: 0x040012AD RID: 4781
		public byte y;

		// Token: 0x040012AE RID: 4782
		public byte rot;

		// Token: 0x040012AF RID: 4783
		public byte size_x;

		// Token: 0x040012B0 RID: 4784
		public byte size_y;

		// Token: 0x040012B1 RID: 4785
		private Item _item;

		// Token: 0x040012B2 RID: 4786
		public InteractableItem interactableItem;
	}
}
