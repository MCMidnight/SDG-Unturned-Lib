using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200071F RID: 1823
	public class SleekItems : SleekWrapper
	{
		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x06003C28 RID: 15400 RVA: 0x0011B53D File Offset: 0x0011973D
		public byte page
		{
			get
			{
				return this._page;
			}
		}

		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x06003C29 RID: 15401 RVA: 0x0011B545 File Offset: 0x00119745
		public byte width
		{
			get
			{
				return this._width;
			}
		}

		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x06003C2A RID: 15402 RVA: 0x0011B54D File Offset: 0x0011974D
		public byte height
		{
			get
			{
				return this._height;
			}
		}

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x06003C2B RID: 15403 RVA: 0x0011B555 File Offset: 0x00119755
		public List<SleekItem> items
		{
			get
			{
				return this._items;
			}
		}

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x06003C2C RID: 15404 RVA: 0x0011B55D File Offset: 0x0011975D
		// (set) Token: 0x06003C2D RID: 15405 RVA: 0x0011B568 File Offset: 0x00119768
		public bool areItemsEnabled
		{
			get
			{
				return this._areItemsEnabled;
			}
			set
			{
				this._areItemsEnabled = value;
				foreach (SleekItem sleekItem in this._items)
				{
					sleekItem.setEnabled(this._areItemsEnabled);
				}
			}
		}

		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x06003C2E RID: 15406 RVA: 0x0011B5C8 File Offset: 0x001197C8
		// (set) Token: 0x06003C2F RID: 15407 RVA: 0x0011B5D5 File Offset: 0x001197D5
		public bool isGridRaycastTarget
		{
			get
			{
				return this.grid.IsRaycastTarget;
			}
			set
			{
				this.grid.IsRaycastTarget = value;
			}
		}

		/// <summary>
		/// Reset all items hotkey label.
		/// </summary>
		// Token: 0x06003C30 RID: 15408 RVA: 0x0011B5E4 File Offset: 0x001197E4
		public void resetHotkeyDisplay()
		{
			foreach (SleekItem sleekItem in this._items)
			{
				if (sleekItem.hotkey != 255)
				{
					sleekItem.updateHotkey(byte.MaxValue);
				}
			}
		}

		// Token: 0x06003C31 RID: 15409 RVA: 0x0011B648 File Offset: 0x00119848
		public void updateHotkey(ItemJar jar, byte button)
		{
			int num = this.indexOfItemElement(jar);
			if (num >= 0)
			{
				this.items[num].updateHotkey(button);
				return;
			}
			int num2 = this.pendingItems.IndexOf(jar);
			if (num2 >= 0)
			{
				this.pendingItems.RemoveAtFast(num2);
				this.createElementForItem(jar).updateHotkey(button);
			}
		}

		// Token: 0x06003C32 RID: 15410 RVA: 0x0011B6A0 File Offset: 0x001198A0
		public void resize(byte newWidth, byte newHeight)
		{
			this._width = newWidth;
			this._height = newHeight;
			this.horizontalScrollView.ContentSizeOffset = new Vector2((float)this.width * 50f, (float)this.height * 50f);
			base.SizeOffset_Y = (float)(this.height * 50 + 30);
			this.grid.TileRepeatHintForUITK = new Vector2Int((int)newWidth, (int)newHeight);
		}

		// Token: 0x06003C33 RID: 15411 RVA: 0x0011B70A File Offset: 0x0011990A
		public void clear()
		{
			this.items.Clear();
			this.itemsPanel.RemoveAllChildren();
			this.pendingItems.Clear();
		}

		// Token: 0x06003C34 RID: 15412 RVA: 0x0011B730 File Offset: 0x00119930
		public void updateItem(ItemJar jar)
		{
			int num = this.indexOfItemElement(jar);
			if (num >= 0)
			{
				this.items[num].updateItem(jar);
			}
		}

		// Token: 0x06003C35 RID: 15413 RVA: 0x0011B75B File Offset: 0x0011995B
		public void addItem(ItemJar jar)
		{
			this.pendingItems.Add(jar);
		}

		// Token: 0x06003C36 RID: 15414 RVA: 0x0011B76C File Offset: 0x0011996C
		public void removeItem(ItemJar jar)
		{
			int num = this.indexOfItemElement(jar);
			if (num >= 0)
			{
				this.itemsPanel.RemoveChild(this.items[num]);
				this.items.RemoveAtFast(num);
				return;
			}
			this.pendingItems.RemoveFast(jar);
		}

		// Token: 0x06003C37 RID: 15415 RVA: 0x0011B7B8 File Offset: 0x001199B8
		public override void OnUpdate()
		{
			int num = Mathf.Max(0, this.pendingItems.Count - 5);
			for (int i = this.pendingItems.Count - 1; i >= num; i--)
			{
				ItemJar jar = this.pendingItems[i];
				this.pendingItems.RemoveAt(i);
				this.createElementForItem(jar);
			}
		}

		// Token: 0x06003C38 RID: 15416 RVA: 0x0011B814 File Offset: 0x00119A14
		private int indexOfItemElement(ItemJar jar)
		{
			int num = (int)(jar.x * 50);
			int num2 = (int)(jar.y * 50);
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i].PositionOffset_X == (float)num && this.items[i].PositionOffset_Y == (float)num2)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06003C39 RID: 15417 RVA: 0x0011B878 File Offset: 0x00119A78
		private SleekItem createElementForItem(ItemJar jar)
		{
			SleekItem sleekItem = new SleekItem(jar);
			sleekItem.PositionOffset_X = (float)(jar.x * 50);
			sleekItem.PositionOffset_Y = (float)(jar.y * 50);
			sleekItem.onClickedItem = new ClickedItem(this.onClickedItem);
			sleekItem.onDraggedItem = new DraggedItem(this.onDraggedItem);
			this.itemsPanel.AddChild(sleekItem);
			sleekItem.setEnabled(this._areItemsEnabled);
			this.items.Add(sleekItem);
			return sleekItem;
		}

		// Token: 0x06003C3A RID: 15418 RVA: 0x0011B8F5 File Offset: 0x00119AF5
		private void onClickedItem(SleekItem item)
		{
			SelectedItem selectedItem = this.onSelectedItem;
			if (selectedItem == null)
			{
				return;
			}
			selectedItem(this.page, (byte)(item.PositionOffset_X / 50f), (byte)(item.PositionOffset_Y / 50f));
		}

		// Token: 0x06003C3B RID: 15419 RVA: 0x0011B927 File Offset: 0x00119B27
		private void onDraggedItem(SleekItem item)
		{
			GrabbedItem grabbedItem = this.onGrabbedItem;
			if (grabbedItem == null)
			{
				return;
			}
			grabbedItem(this.page, (byte)(item.PositionOffset_X / 50f), (byte)(item.PositionOffset_Y / 50f), item);
		}

		// Token: 0x06003C3C RID: 15420 RVA: 0x0011B95C File Offset: 0x00119B5C
		private void onClickedGrid()
		{
			Vector2 normalizedCursorPosition = this.grid.GetNormalizedCursorPosition();
			byte x = (byte)(normalizedCursorPosition.x * (float)this.width);
			byte y = (byte)(normalizedCursorPosition.y * (float)this.height);
			PlacedItem placedItem = this.onPlacedItem;
			if (placedItem == null)
			{
				return;
			}
			placedItem(this.page, x, y);
		}

		// Token: 0x06003C3D RID: 15421 RVA: 0x0011B9AC File Offset: 0x00119BAC
		public SleekItems(byte newPage)
		{
			this._page = newPage;
			this._items = new List<SleekItem>();
			this.pendingItems = new List<ItemJar>();
			base.SizeScale_X = 1f;
			this.horizontalScrollView = Glazier.Get().CreateScrollView();
			this.horizontalScrollView.SizeScale_X = 1f;
			this.horizontalScrollView.SizeScale_Y = 1f;
			this.horizontalScrollView.HandleScrollWheel = false;
			base.AddChild(this.horizontalScrollView);
			this.grid = Glazier.Get().CreateSprite();
			this.grid.SizeScale_X = 1f;
			this.grid.SizeScale_Y = 1f;
			this.grid.Sprite = PlayerDashboardInventoryUI.icons.load<Sprite>("Grid_Sprite");
			this.grid.OnClicked += new Action(this.onClickedGrid);
			this.grid.TintColor = 2;
			this.horizontalScrollView.AddChild(this.grid);
			this.itemsPanel = Glazier.Get().CreateFrame();
			this.itemsPanel.SizeScale_X = 1f;
			this.itemsPanel.SizeScale_Y = 1f;
			this.grid.AddChild(this.itemsPanel);
		}

		// Token: 0x0400259D RID: 9629
		public SelectedItem onSelectedItem;

		// Token: 0x0400259E RID: 9630
		public GrabbedItem onGrabbedItem;

		// Token: 0x0400259F RID: 9631
		public PlacedItem onPlacedItem;

		// Token: 0x040025A0 RID: 9632
		private ISleekElement itemsPanel;

		// Token: 0x040025A1 RID: 9633
		private ISleekSprite grid;

		// Token: 0x040025A2 RID: 9634
		private ISleekScrollView horizontalScrollView;

		// Token: 0x040025A3 RID: 9635
		private byte _page;

		// Token: 0x040025A4 RID: 9636
		private byte _width;

		// Token: 0x040025A5 RID: 9637
		private byte _height;

		// Token: 0x040025A6 RID: 9638
		private List<SleekItem> _items;

		/// <summary>
		/// Rather than creating all SleekItems as once we create a few per frame.
		/// </summary>
		// Token: 0x040025A7 RID: 9639
		private List<ItemJar> pendingItems;

		// Token: 0x040025A8 RID: 9640
		private bool _areItemsEnabled = true;
	}
}
