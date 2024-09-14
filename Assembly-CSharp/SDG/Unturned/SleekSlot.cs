using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200072F RID: 1839
	public class SleekSlot : SleekWrapper
	{
		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x06003C93 RID: 15507 RVA: 0x00120506 File Offset: 0x0011E706
		public SleekItem item
		{
			get
			{
				return this._item;
			}
		}

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x06003C94 RID: 15508 RVA: 0x0012050E File Offset: 0x0011E70E
		public byte page
		{
			get
			{
				return this._page;
			}
		}

		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x06003C95 RID: 15509 RVA: 0x00120516 File Offset: 0x0011E716
		// (set) Token: 0x06003C96 RID: 15510 RVA: 0x0012051E File Offset: 0x0011E71E
		public bool isItemEnabled
		{
			get
			{
				return this._isItemEnabled;
			}
			set
			{
				this._isItemEnabled = value;
				if (this.item != null)
				{
					this.item.setEnabled(value);
				}
			}
		}

		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x06003C97 RID: 15511 RVA: 0x0012053B File Offset: 0x0011E73B
		// (set) Token: 0x06003C98 RID: 15512 RVA: 0x00120548 File Offset: 0x0011E748
		public bool isImageRaycastTarget
		{
			get
			{
				return this.image.IsRaycastTarget;
			}
			set
			{
				this.image.IsRaycastTarget = value;
			}
		}

		// Token: 0x06003C99 RID: 15513 RVA: 0x00120556 File Offset: 0x0011E756
		public void select()
		{
			PlacedItem placedItem = this.onPlacedItem;
			if (placedItem == null)
			{
				return;
			}
			placedItem(this.page, 0, 0);
		}

		// Token: 0x06003C9A RID: 15514 RVA: 0x00120570 File Offset: 0x0011E770
		public void updateItem(ItemJar jar)
		{
			if (this.item == null)
			{
				return;
			}
			this.item.updateItem(jar);
		}

		// Token: 0x06003C9B RID: 15515 RVA: 0x00120588 File Offset: 0x0011E788
		public void applyItem(ItemJar jar)
		{
			if (this.item != null)
			{
				this.image.RemoveChild(this.item);
				this._item = null;
			}
			if (jar != null)
			{
				this._item = new SleekItem(jar);
				this.item.PositionOffset_X = (float)(-jar.size_x * 25);
				this.item.PositionOffset_Y = (float)(-jar.size_y * 25);
				this.item.PositionScale_X = 0.5f;
				this.item.PositionScale_Y = 0.5f;
				this.item.updateHotkey(this.page);
				this.item.onClickedItem = new ClickedItem(this.onClickedItem);
				this.item.onDraggedItem = new DraggedItem(this.onDraggedItem);
				this.item.setEnabled(this._isItemEnabled);
				this.image.AddChild(this.item);
			}
		}

		// Token: 0x06003C9C RID: 15516 RVA: 0x00120674 File Offset: 0x0011E874
		private void onClickedItem(SleekItem item)
		{
			SelectedItem selectedItem = this.onSelectedItem;
			if (selectedItem == null)
			{
				return;
			}
			selectedItem(this.page, 0, 0);
		}

		// Token: 0x06003C9D RID: 15517 RVA: 0x0012068E File Offset: 0x0011E88E
		private void onDraggedItem(SleekItem item)
		{
			GrabbedItem grabbedItem = this.onGrabbedItem;
			if (grabbedItem == null)
			{
				return;
			}
			grabbedItem(this.page, 0, 0, item);
		}

		// Token: 0x06003C9E RID: 15518 RVA: 0x001206AC File Offset: 0x0011E8AC
		public SleekSlot(byte newPage)
		{
			this._page = newPage;
			base.SizeOffset_X = 250f;
			base.SizeOffset_Y = 150f;
			this.image = Glazier.Get().CreateSprite();
			this.image.SizeScale_X = 1f;
			this.image.SizeScale_Y = 1f;
			this.image.DrawMethod = 1;
			this.image.Sprite = PlayerDashboardInventoryUI.icons.load<Sprite>("Slot_Sprite");
			this.image.TintColor = 2;
			this.image.OnClicked += new Action(this.select);
			base.AddChild(this.image);
		}

		// Token: 0x040025F0 RID: 9712
		public SelectedItem onSelectedItem;

		// Token: 0x040025F1 RID: 9713
		public GrabbedItem onGrabbedItem;

		// Token: 0x040025F2 RID: 9714
		public PlacedItem onPlacedItem;

		// Token: 0x040025F3 RID: 9715
		private ISleekSprite image;

		// Token: 0x040025F4 RID: 9716
		private SleekItem _item;

		// Token: 0x040025F5 RID: 9717
		private byte _page;

		// Token: 0x040025F6 RID: 9718
		private bool _isItemEnabled = true;
	}
}
