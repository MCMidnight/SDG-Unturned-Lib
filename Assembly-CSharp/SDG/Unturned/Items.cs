using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x020004A1 RID: 1185
	public class Items
	{
		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x060024BE RID: 9406 RVA: 0x00092268 File Offset: 0x00090468
		public byte page
		{
			get
			{
				return this._page;
			}
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x060024BF RID: 9407 RVA: 0x00092270 File Offset: 0x00090470
		public byte width
		{
			get
			{
				return this._width;
			}
		}

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x060024C0 RID: 9408 RVA: 0x00092278 File Offset: 0x00090478
		public byte height
		{
			get
			{
				return this._height;
			}
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x060024C1 RID: 9409 RVA: 0x00092280 File Offset: 0x00090480
		// (set) Token: 0x060024C2 RID: 9410 RVA: 0x00092288 File Offset: 0x00090488
		public List<ItemJar> items { get; protected set; }

		// Token: 0x060024C3 RID: 9411 RVA: 0x00092294 File Offset: 0x00090494
		public void updateAmount(byte index, byte newAmount)
		{
			if (index < 0 || (int)index >= this.items.Count)
			{
				return;
			}
			this.items[(int)index].item.amount = newAmount;
			ItemUpdated itemUpdated = this.onItemUpdated;
			if (itemUpdated != null)
			{
				itemUpdated(this.page, index, this.items[(int)index]);
			}
			StateUpdated stateUpdated = this.onStateUpdated;
			if (stateUpdated == null)
			{
				return;
			}
			stateUpdated();
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x00092300 File Offset: 0x00090500
		public void updateQuality(byte index, byte newQuality)
		{
			if (index < 0 || (int)index >= this.items.Count)
			{
				return;
			}
			this.items[(int)index].item.quality = newQuality;
			ItemUpdated itemUpdated = this.onItemUpdated;
			if (itemUpdated != null)
			{
				itemUpdated(this.page, index, this.items[(int)index]);
			}
			StateUpdated stateUpdated = this.onStateUpdated;
			if (stateUpdated == null)
			{
				return;
			}
			stateUpdated();
		}

		// Token: 0x060024C5 RID: 9413 RVA: 0x0009236C File Offset: 0x0009056C
		public void updateState(byte index, byte[] newState)
		{
			if (index < 0 || (int)index >= this.items.Count)
			{
				return;
			}
			this.items[(int)index].item.state = newState;
			ItemUpdated itemUpdated = this.onItemUpdated;
			if (itemUpdated != null)
			{
				itemUpdated(this.page, index, this.items[(int)index]);
			}
			StateUpdated stateUpdated = this.onStateUpdated;
			if (stateUpdated == null)
			{
				return;
			}
			stateUpdated();
		}

		// Token: 0x060024C6 RID: 9414 RVA: 0x000923D7 File Offset: 0x000905D7
		public byte getItemCount()
		{
			return (byte)this.items.Count;
		}

		// Token: 0x060024C7 RID: 9415 RVA: 0x000923E5 File Offset: 0x000905E5
		public bool containsItem(ItemJar jar)
		{
			return this.items.Contains(jar);
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x000923F3 File Offset: 0x000905F3
		public ItemJar getItem(byte index)
		{
			if (index < 0 || (int)index >= this.items.Count)
			{
				return null;
			}
			return this.items[(int)index];
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x00092418 File Offset: 0x00090618
		public byte getIndex(byte x, byte y)
		{
			if (this.page < PlayerInventory.SLOTS)
			{
				return 0;
			}
			if (x < 0 || y < 0 || x >= this.width || y >= this.height)
			{
				return byte.MaxValue;
			}
			byte b = 0;
			while ((int)b < this.items.Count)
			{
				if (this.items[(int)b].x == x && this.items[(int)b].y == y)
				{
					return b;
				}
				b += 1;
			}
			return byte.MaxValue;
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x0009249C File Offset: 0x0009069C
		internal int FindIndexOfJar(ItemJar jar)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i] == jar)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060024CB RID: 9419 RVA: 0x000924D4 File Offset: 0x000906D4
		public byte findIndex(byte x, byte y, out byte find_x, out byte find_y)
		{
			find_x = byte.MaxValue;
			find_y = byte.MaxValue;
			if (x < 0 || y < 0 || x >= this.width || y >= this.height)
			{
				return byte.MaxValue;
			}
			byte b = 0;
			while ((int)b < this.items.Count)
			{
				if (this.items[(int)b].x <= x && this.items[(int)b].y <= y)
				{
					byte b2 = this.items[(int)b].size_x;
					byte b3 = this.items[(int)b].size_y;
					if (this.items[(int)b].rot % 2 == 1)
					{
						b2 = this.items[(int)b].size_y;
						b3 = this.items[(int)b].size_x;
					}
					if (this.items[(int)b].x + b2 > x && this.items[(int)b].y + b3 > y)
					{
						find_x = this.items[(int)b].x;
						find_y = this.items[(int)b].y;
						return b;
					}
				}
				b += 1;
			}
			return byte.MaxValue;
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x00092614 File Offset: 0x00090814
		public List<InventorySearch> search(List<InventorySearch> search, EItemType type)
		{
			byte b = 0;
			while ((int)b < this.items.Count)
			{
				ItemJar itemJar = this.items[(int)b];
				if (itemJar.item.amount > 0)
				{
					ItemAsset asset = itemJar.GetAsset();
					if (asset != null && asset.type == type)
					{
						search.Add(new InventorySearch(this.page, itemJar));
					}
				}
				b += 1;
			}
			return search;
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x00092679 File Offset: 0x00090879
		[Obsolete]
		public List<InventorySearch> search(List<InventorySearch> search, EItemType type, ushort caliber)
		{
			return this.search(search, type, caliber, true);
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x00092688 File Offset: 0x00090888
		public List<InventorySearch> search(List<InventorySearch> search, EItemType type, ushort caliber, bool allowZeroCaliber)
		{
			byte b = 0;
			while ((int)b < this.items.Count)
			{
				ItemJar itemJar = this.items[(int)b];
				if (itemJar.item.amount > 0)
				{
					bool flag = false;
					for (int i = 0; i < search.Count; i++)
					{
						if (search[i].page == this.page && search[i].jar.x == itemJar.x && search[i].jar.y == itemJar.y)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						ItemAsset asset = itemJar.GetAsset();
						if (asset != null && asset.type == type)
						{
							if (((ItemCaliberAsset)asset).calibers.Length == 0)
							{
								if (allowZeroCaliber)
								{
									search.Add(new InventorySearch(this.page, itemJar));
								}
							}
							else
							{
								byte b2 = 0;
								while ((int)b2 < ((ItemCaliberAsset)asset).calibers.Length)
								{
									if (((ItemCaliberAsset)asset).calibers[(int)b2] == caliber)
									{
										search.Add(new InventorySearch(this.page, itemJar));
										break;
									}
									b2 += 1;
								}
							}
						}
					}
				}
				b += 1;
			}
			return search;
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x000927B8 File Offset: 0x000909B8
		public List<InventorySearch> search(List<InventorySearch> search, ushort id, bool findEmpty, bool findHealthy)
		{
			byte b = 0;
			while ((int)b < this.items.Count)
			{
				ItemJar itemJar = this.items[(int)b];
				if ((findEmpty || itemJar.item.amount > 0) && (findHealthy || itemJar.item.quality < 100) && itemJar.item.id == id)
				{
					search.Add(new InventorySearch(this.page, itemJar));
				}
				b += 1;
			}
			return search;
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x00092830 File Offset: 0x00090A30
		public InventorySearch has(ushort id)
		{
			byte b = 0;
			while ((int)b < this.items.Count)
			{
				ItemJar itemJar = this.items[(int)b];
				if (itemJar.item.amount > 0 && itemJar.item.id == id)
				{
					return new InventorySearch(this.page, itemJar);
				}
				b += 1;
			}
			return null;
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x0009288C File Offset: 0x00090A8C
		public void loadItem(byte x, byte y, byte rot, Item item)
		{
			ItemJar itemJar = new ItemJar(x, y, rot, item);
			this.fillSlot(itemJar, true);
			this.items.Add(itemJar);
		}

		// Token: 0x060024D2 RID: 9426 RVA: 0x000928B8 File Offset: 0x00090AB8
		public void addItem(byte x, byte y, byte rot, Item item)
		{
			ItemJar itemJar = new ItemJar(x, y, rot, item);
			this.fillSlot(itemJar, true);
			this.items.Add(itemJar);
			try
			{
				ItemAdded itemAdded = this.onItemAdded;
				if (itemAdded != null)
				{
					itemAdded(this.page, (byte)(this.items.Count - 1), itemJar);
				}
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, string.Format("Caught exception during addItem (x: {0} y: {1} rot: {2} item: {3}):", new object[]
				{
					x,
					y,
					rot,
					(item != null) ? new ushort?(item.id) : default(ushort?)
				}));
			}
			StateUpdated stateUpdated = this.onStateUpdated;
			if (stateUpdated == null)
			{
				return;
			}
			stateUpdated();
		}

		// Token: 0x060024D3 RID: 9427 RVA: 0x00092984 File Offset: 0x00090B84
		public bool tryAddItem(Item item)
		{
			return this.tryAddItem(item, true);
		}

		// Token: 0x060024D4 RID: 9428 RVA: 0x00092990 File Offset: 0x00090B90
		public bool tryAddItem(Item item, bool isStateUpdatable)
		{
			if (this.getItemCount() >= 200)
			{
				return false;
			}
			ItemJar itemJar = new ItemJar(item);
			byte x;
			byte y;
			byte rot;
			if (!this.tryFindSpace(itemJar.size_x, itemJar.size_y, out x, out y, out rot))
			{
				return false;
			}
			itemJar.x = x;
			itemJar.y = y;
			itemJar.rot = rot;
			this.fillSlot(itemJar, true);
			this.items.Add(itemJar);
			try
			{
				ItemAdded itemAdded = this.onItemAdded;
				if (itemAdded != null)
				{
					itemAdded(this.page, (byte)(this.items.Count - 1), itemJar);
				}
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, string.Format("Caught exception during tryAddItem ({0}):", (item != null) ? new ushort?(item.id) : default(ushort?)));
			}
			if (isStateUpdatable)
			{
				StateUpdated stateUpdated = this.onStateUpdated;
				if (stateUpdated != null)
				{
					stateUpdated();
				}
			}
			return true;
		}

		// Token: 0x060024D5 RID: 9429 RVA: 0x00092A78 File Offset: 0x00090C78
		public void removeItem(byte index)
		{
			if (index < 0 || (int)index >= this.items.Count)
			{
				return;
			}
			this.fillSlot(this.items[(int)index], false);
			ItemRemoved itemRemoved = this.onItemRemoved;
			if (itemRemoved != null)
			{
				itemRemoved(this.page, index, this.items[(int)index]);
			}
			this.items.RemoveAt((int)index);
			StateUpdated stateUpdated = this.onStateUpdated;
			if (stateUpdated == null)
			{
				return;
			}
			stateUpdated();
		}

		// Token: 0x060024D6 RID: 9430 RVA: 0x00092AEB File Offset: 0x00090CEB
		public void clear()
		{
			this.items.Clear();
		}

		// Token: 0x060024D7 RID: 9431 RVA: 0x00092AF8 File Offset: 0x00090CF8
		public void loadSize(byte newWidth, byte newHeight)
		{
			this._width = newWidth;
			this._height = newHeight;
			this.slots = new bool[(int)this.width, (int)this.height];
			for (byte b = 0; b < this.width; b += 1)
			{
				for (byte b2 = 0; b2 < this.height; b2 += 1)
				{
					this.slots[(int)b, (int)b2] = false;
				}
			}
			List<ItemJar> list = new List<ItemJar>();
			if (this.items != null)
			{
				byte b3 = 0;
				while ((int)b3 < this.items.Count)
				{
					ItemJar itemJar = this.items[(int)b3];
					byte b4 = itemJar.size_x;
					byte b5 = itemJar.size_y;
					if (itemJar.rot % 2 == 1)
					{
						b4 = itemJar.size_y;
						b5 = itemJar.size_x;
					}
					if (this.width == 0 || this.height == 0 || (this.page >= PlayerInventory.SLOTS && (itemJar.x + b4 > this.width || itemJar.y + b5 > this.height)))
					{
						ItemDiscarded itemDiscarded = this.onItemDiscarded;
						if (itemDiscarded != null)
						{
							itemDiscarded(this.page, b3, itemJar);
						}
						StateUpdated stateUpdated = this.onStateUpdated;
						if (stateUpdated != null)
						{
							stateUpdated();
						}
					}
					else
					{
						this.fillSlot(itemJar, true);
						list.Add(itemJar);
					}
					b3 += 1;
				}
			}
			this.items = list;
		}

		// Token: 0x060024D8 RID: 9432 RVA: 0x00092C4E File Offset: 0x00090E4E
		public void resize(byte newWidth, byte newHeight)
		{
			this.loadSize(newWidth, newHeight);
			ItemsResized itemsResized = this.onItemsResized;
			if (itemsResized != null)
			{
				itemsResized(this.page, newWidth, newHeight);
			}
			StateUpdated stateUpdated = this.onStateUpdated;
			if (stateUpdated == null)
			{
				return;
			}
			stateUpdated();
		}

		/// checks whether a space contains any filled slots
		// Token: 0x060024D9 RID: 9433 RVA: 0x00092C84 File Offset: 0x00090E84
		public bool checkSpaceEmpty(byte pos_x, byte pos_y, byte size_x, byte size_y, byte rot)
		{
			if (this.page < PlayerInventory.SLOTS)
			{
				return this.items.Count == 0;
			}
			if (rot % 2 == 1)
			{
				byte b = size_x;
				size_x = size_y;
				size_y = b;
			}
			for (byte b2 = pos_x; b2 < pos_x + size_x; b2 += 1)
			{
				for (byte b3 = pos_y; b3 < pos_y + size_y; b3 += 1)
				{
					if (b2 >= this.width || b3 >= this.height)
					{
						return false;
					}
					if (this.slots[(int)b2, (int)b3])
					{
						return false;
					}
				}
			}
			return true;
		}

		/// checks whether an item can be dragged and takes into account if the item overlaps its old self
		// Token: 0x060024DA RID: 9434 RVA: 0x00092D00 File Offset: 0x00090F00
		public bool checkSpaceDrag(byte old_x, byte old_y, byte oldRot, byte new_x, byte new_y, byte newRot, byte size_x, byte size_y, bool checkSame)
		{
			if (this.page < PlayerInventory.SLOTS)
			{
				return this.items.Count == 0 || checkSame;
			}
			byte b = size_x;
			byte b2 = size_y;
			if (oldRot % 2 == 1)
			{
				b = size_y;
				b2 = size_x;
			}
			byte b3 = size_x;
			byte b4 = size_y;
			if (newRot % 2 == 1)
			{
				b3 = size_y;
				b4 = size_x;
			}
			for (byte b5 = new_x; b5 < new_x + b3; b5 += 1)
			{
				for (byte b6 = new_y; b6 < new_y + b4; b6 += 1)
				{
					if (b5 >= this.width || b6 >= this.height)
					{
						return false;
					}
					if (this.slots[(int)b5, (int)b6])
					{
						int num = (int)(b5 - old_x);
						int num2 = (int)(b6 - old_y);
						if (!checkSame || num < 0 || num2 < 0 || num >= (int)b || num2 >= (int)b2)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		/// <summary>
		/// checks whether the spot currently used by the old item is big enough to fit the new item
		/// </summary>
		// Token: 0x060024DB RID: 9435 RVA: 0x00092DCC File Offset: 0x00090FCC
		public bool checkSpaceSwap(byte x, byte y, byte oldSize_X, byte oldSize_Y, byte oldRot, byte newSize_X, byte newSize_Y, byte newRot)
		{
			if (this.page < PlayerInventory.SLOTS)
			{
				return true;
			}
			if (oldRot % 2 == 1)
			{
				byte b = oldSize_X;
				oldSize_X = oldSize_Y;
				oldSize_Y = b;
			}
			if (newRot % 2 == 1)
			{
				byte b2 = newSize_X;
				newSize_X = newSize_Y;
				newSize_Y = b2;
			}
			for (byte b3 = x; b3 < x + newSize_X; b3 += 1)
			{
				for (byte b4 = y; b4 < y + newSize_Y; b4 += 1)
				{
					if (b3 >= this.width || b4 >= this.height)
					{
						return false;
					}
					if (this.slots[(int)b3, (int)b4])
					{
						int num = (int)(b3 - x);
						int num2 = (int)(b4 - y);
						if (num < 0 || num2 < 0 || num >= (int)oldSize_X || num2 >= (int)oldSize_Y)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x060024DC RID: 9436 RVA: 0x00092E64 File Offset: 0x00091064
		public bool tryFindSpace(byte size_x, byte size_y, out byte x, out byte y, out byte rot)
		{
			x = byte.MaxValue;
			y = byte.MaxValue;
			rot = 0;
			if (this.page < PlayerInventory.SLOTS)
			{
				x = 0;
				y = 0;
				rot = 0;
				return this.items.Count == 0;
			}
			for (byte b = 0; b < this.height - size_y + 1; b += 1)
			{
				for (byte b2 = 0; b2 < this.width - size_x + 1; b2 += 1)
				{
					bool flag = false;
					byte b3 = 0;
					while (b3 < size_y && !flag)
					{
						for (byte b4 = 0; b4 < size_x; b4 += 1)
						{
							if (this.slots[(int)(b2 + b4), (int)(b + b3)])
							{
								flag = true;
								break;
							}
							if (b4 == size_x - 1 && b3 == size_y - 1)
							{
								x = b2;
								y = b;
								rot = 0;
								return true;
							}
						}
						b3 += 1;
					}
				}
			}
			for (byte b5 = 0; b5 < this.height - size_x + 1; b5 += 1)
			{
				for (byte b6 = 0; b6 < this.width - size_y + 1; b6 += 1)
				{
					bool flag2 = false;
					byte b7 = 0;
					while (b7 < size_x && !flag2)
					{
						for (byte b8 = 0; b8 < size_y; b8 += 1)
						{
							if (this.slots[(int)(b6 + b8), (int)(b5 + b7)])
							{
								flag2 = true;
								break;
							}
							if (b8 == size_y - 1 && b7 == size_x - 1)
							{
								x = b6;
								y = b5;
								rot = 1;
								return true;
							}
						}
						b7 += 1;
					}
				}
			}
			return false;
		}

		// Token: 0x060024DD RID: 9437 RVA: 0x00092FCC File Offset: 0x000911CC
		private void fillSlot(ItemJar jar, bool isOccupied)
		{
			byte b = jar.size_x;
			byte b2 = jar.size_y;
			if (jar.rot % 2 == 1)
			{
				b = jar.size_y;
				b2 = jar.size_x;
			}
			for (byte b3 = 0; b3 < b; b3 += 1)
			{
				for (byte b4 = 0; b4 < b2; b4 += 1)
				{
					if (jar.x + b3 < this.width && jar.y + b4 < this.height)
					{
						this.slots[(int)(jar.x + b3), (int)(jar.y + b4)] = isOccupied;
					}
				}
			}
		}

		// Token: 0x060024DE RID: 9438 RVA: 0x00093056 File Offset: 0x00091256
		public Items(byte newPage)
		{
			this._page = newPage;
			this.items = new List<ItemJar>();
		}

		// Token: 0x040012B3 RID: 4787
		public ItemsResized onItemsResized;

		// Token: 0x040012B4 RID: 4788
		public ItemUpdated onItemUpdated;

		// Token: 0x040012B5 RID: 4789
		public ItemAdded onItemAdded;

		// Token: 0x040012B6 RID: 4790
		public ItemRemoved onItemRemoved;

		// Token: 0x040012B7 RID: 4791
		public ItemDiscarded onItemDiscarded;

		// Token: 0x040012B8 RID: 4792
		public StateUpdated onStateUpdated;

		// Token: 0x040012B9 RID: 4793
		private byte _page;

		// Token: 0x040012BA RID: 4794
		private byte _width;

		// Token: 0x040012BB RID: 4795
		private byte _height;

		// Token: 0x040012BC RID: 4796
		private bool[,] slots;
	}
}
