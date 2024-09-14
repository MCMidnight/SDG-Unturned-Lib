using System;

namespace SDG.Unturned
{
	// Token: 0x020005FB RID: 1531
	public class InventorySearch
	{
		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x0600304D RID: 12365 RVA: 0x000D4CE5 File Offset: 0x000D2EE5
		public byte page
		{
			get
			{
				return this._page;
			}
		}

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x0600304E RID: 12366 RVA: 0x000D4CED File Offset: 0x000D2EED
		public ItemJar jar
		{
			get
			{
				return this._jar;
			}
		}

		// Token: 0x0600304F RID: 12367 RVA: 0x000D4CF5 File Offset: 0x000D2EF5
		public ItemAsset GetAsset()
		{
			if (this._jar == null)
			{
				return null;
			}
			return this._jar.GetAsset();
		}

		// Token: 0x06003050 RID: 12368 RVA: 0x000D4D0C File Offset: 0x000D2F0C
		public T GetAsset<T>() where T : ItemAsset
		{
			if (this._jar == null)
			{
				return default(T);
			}
			return this._jar.GetAsset<T>();
		}

		// Token: 0x06003051 RID: 12369 RVA: 0x000D4D36 File Offset: 0x000D2F36
		private void dequipIfEquipped(Player player)
		{
			if (player.equipment.checkSelection(this.page, this.jar.x, this.jar.y))
			{
				player.equipment.dequip();
			}
		}

		/// <summary>
		/// Serverside delete an amount of this item.
		/// </summary>
		/// <returns>Total amount deleted.</returns>
		// Token: 0x06003052 RID: 12370 RVA: 0x000D4D6C File Offset: 0x000D2F6C
		public uint deleteAmount(Player player, uint desiredAmount)
		{
			this.dequipIfEquipped(player);
			uint amount = (uint)this.jar.item.amount;
			if (amount > desiredAmount)
			{
				player.inventory.sendUpdateAmount(this.page, this.jar.x, this.jar.y, (byte)((uint)this.jar.item.amount - desiredAmount));
				return desiredAmount;
			}
			player.inventory.sendUpdateAmount(this.page, this.jar.x, this.jar.y, 0);
			player.crafting.removeItem(this.page, this.jar);
			if (this.page < PlayerInventory.SLOTS)
			{
				player.equipment.sendSlot(this.page);
			}
			return amount;
		}

		// Token: 0x06003053 RID: 12371 RVA: 0x000D4E2F File Offset: 0x000D302F
		public InventorySearch(byte newPage, ItemJar newJar)
		{
			this._page = newPage;
			this._jar = newJar;
		}

		// Token: 0x04001B67 RID: 7015
		private byte _page;

		// Token: 0x04001B68 RID: 7016
		private ItemJar _jar;
	}
}
