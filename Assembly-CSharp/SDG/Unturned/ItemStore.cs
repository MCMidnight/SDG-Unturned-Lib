using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// All main menu MTX shop code should be routed through here so that it could theoretically be ported to other
	/// platforms or stores. Obviously this is all very Steam specific at the moment, but at least the UI does not
	/// depend on Steam API here as much as older parts of the game.
	/// </summary>
	// Token: 0x020001AB RID: 427
	internal abstract class ItemStore
	{
		// Token: 0x06000D51 RID: 3409 RVA: 0x0002CABF File Offset: 0x0002ACBF
		public static ItemStore Get()
		{
			return ItemStore.instance;
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x0002CAC6 File Offset: 0x0002ACC6
		public IEnumerable<ItemStore.CartEntry> GetCart()
		{
			return this.itemsInCart;
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000D53 RID: 3411 RVA: 0x0002CACE File Offset: 0x0002ACCE
		public bool IsCartEmpty
		{
			get
			{
				return this.itemsInCart.IsEmpty<ItemStore.CartEntry>();
			}
		}

		// Token: 0x06000D54 RID: 3412
		public abstract void ViewItem(int itemdefid);

		// Token: 0x06000D55 RID: 3413
		public abstract void ViewNewItems();

		// Token: 0x06000D56 RID: 3414
		public abstract void ViewStore();

		/// <summary>
		/// Do we have pricing details for a given item?
		/// Price results may not have been returned yet, or item might not be public.
		/// </summary>
		// Token: 0x06000D57 RID: 3415 RVA: 0x0002CADC File Offset: 0x0002ACDC
		public bool FindListing(int itemdefid, out ItemStore.Listing listing)
		{
			foreach (ItemStore.Listing listing2 in this.listings)
			{
				if (listing2.itemdefid == itemdefid)
				{
					listing = listing2;
					return true;
				}
			}
			listing = default(ItemStore.Listing);
			return false;
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x0002CB20 File Offset: 0x0002AD20
		public int GetQuantityInCart(int itemdefid)
		{
			foreach (ItemStore.CartEntry cartEntry in this.itemsInCart)
			{
				if (cartEntry.itemdefid == itemdefid)
				{
					return cartEntry.quantity;
				}
			}
			return 0;
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x0002CB84 File Offset: 0x0002AD84
		public void SetQuantityInCart(int itemdefid, int quantity)
		{
			int i = 0;
			while (i < this.itemsInCart.Count)
			{
				ItemStore.CartEntry cartEntry = this.itemsInCart[i];
				if (cartEntry.itemdefid == itemdefid)
				{
					if (quantity > 0)
					{
						cartEntry.quantity = quantity;
						this.itemsInCart[i] = cartEntry;
					}
					else
					{
						this.itemsInCart.RemoveAt(i);
					}
					Action onCartChanged = this.OnCartChanged;
					if (onCartChanged == null)
					{
						return;
					}
					onCartChanged.Invoke();
					return;
				}
				else
				{
					i++;
				}
			}
			if (quantity > 0)
			{
				ItemStore.CartEntry cartEntry2 = default(ItemStore.CartEntry);
				cartEntry2.itemdefid = itemdefid;
				cartEntry2.quantity = quantity;
				this.itemsInCart.Add(cartEntry2);
				Action onCartChanged2 = this.OnCartChanged;
				if (onCartChanged2 == null)
				{
					return;
				}
				onCartChanged2.Invoke();
			}
		}

		// Token: 0x1400006C RID: 108
		// (add) Token: 0x06000D5A RID: 3418
		// (remove) Token: 0x06000D5B RID: 3419
		public abstract event Action OnPricesReceived;

		// Token: 0x1400006D RID: 109
		// (add) Token: 0x06000D5C RID: 3420
		// (remove) Token: 0x06000D5D RID: 3421
		public abstract event Action<ItemStore.EPurchaseResult> OnPurchaseResult;

		// Token: 0x1400006E RID: 110
		// (add) Token: 0x06000D5E RID: 3422 RVA: 0x0002CC30 File Offset: 0x0002AE30
		// (remove) Token: 0x06000D5F RID: 3423 RVA: 0x0002CC68 File Offset: 0x0002AE68
		public event Action OnCartChanged;

		// Token: 0x06000D60 RID: 3424
		public abstract void RequestPrices();

		// Token: 0x06000D61 RID: 3425
		public abstract void StartPurchase();

		/// <summary>
		/// Already filtered to only return locally known items which pass country restrictions.
		/// </summary>
		// Token: 0x06000D62 RID: 3426 RVA: 0x0002CC9D File Offset: 0x0002AE9D
		public ItemStore.Listing[] GetListings()
		{
			return this.listings;
		}

		/// <summary>
		/// Empty if outside new time window.
		/// </summary>
		// Token: 0x06000D63 RID: 3427 RVA: 0x0002CCA5 File Offset: 0x0002AEA5
		public int[] GetNewListingIndices()
		{
			return this.newListingIndices;
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000D64 RID: 3428 RVA: 0x0002CCAD File Offset: 0x0002AEAD
		public bool HasNewListings
		{
			get
			{
				return this.newListingIndices != null && this.newListingIndices.Length != 0;
			}
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x0002CCC3 File Offset: 0x0002AEC3
		public int[] GetFeaturedListingIndices()
		{
			return this.featuredListingIndices;
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000D66 RID: 3430 RVA: 0x0002CCCB File Offset: 0x0002AECB
		public bool HasFeaturedListings
		{
			get
			{
				return this.featuredListingIndices != null && this.featuredListingIndices.Length != 0;
			}
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x0002CCE1 File Offset: 0x0002AEE1
		public int[] GetDiscountedListingIndices()
		{
			return this.discountedListingIndices;
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000D68 RID: 3432 RVA: 0x0002CCE9 File Offset: 0x0002AEE9
		public bool HasDiscountedListings
		{
			get
			{
				return this.discountedListingIndices != null && this.discountedListingIndices.Length != 0;
			}
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x0002CCFF File Offset: 0x0002AEFF
		public int[] GetExcludedListingIndices()
		{
			return this.exludedListingIndices;
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x0002CD08 File Offset: 0x0002AF08
		public string FormatPrice(ulong price)
		{
			return (price / 100.0).ToString("C", this.numberFormatInfo);
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x0002CD38 File Offset: 0x0002AF38
		public string FormatDiscount(ulong currentPrice, ulong basePrice)
		{
			return (currentPrice / basePrice - 1.0).ToString("P0", this.numberFormatInfo);
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x0002CD6C File Offset: 0x0002AF6C
		protected int FindListingIndex(int itemdefid)
		{
			for (int i = 0; i < this.listings.Length; i++)
			{
				if (this.listings[i].itemdefid == itemdefid)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x0002CDA3 File Offset: 0x0002AFA3
		protected void RefreshNewItems()
		{
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x0002CDA5 File Offset: 0x0002AFA5
		protected void RefreshFeaturedItems()
		{
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x0002CDA7 File Offset: 0x0002AFA7
		protected void RefreshExcludedItems()
		{
		}

		// Token: 0x0400050C RID: 1292
		public static readonly Color32 PremiumColor = new Color32(100, 200, 25, byte.MaxValue);

		// Token: 0x0400050E RID: 1294
		private static ItemStore instance = new SteamItemStore();

		// Token: 0x0400050F RID: 1295
		protected NumberFormatInfo numberFormatInfo;

		// Token: 0x04000510 RID: 1296
		protected ItemStore.Listing[] listings;

		/// <summary>
		/// Subset of listings.
		/// </summary>
		// Token: 0x04000511 RID: 1297
		protected int[] newListingIndices;

		/// <summary>
		/// Subset of listings.
		/// </summary>
		// Token: 0x04000512 RID: 1298
		protected int[] featuredListingIndices;

		/// <summary>
		/// Subset of listings.
		/// </summary>
		// Token: 0x04000513 RID: 1299
		protected int[] discountedListingIndices;

		/// <summary>
		/// Subset of listings.
		/// </summary>
		// Token: 0x04000514 RID: 1300
		protected int[] exludedListingIndices;

		// Token: 0x04000515 RID: 1301
		protected List<ItemStore.CartEntry> itemsInCart = new List<ItemStore.CartEntry>();

		// Token: 0x0200087E RID: 2174
		public struct Listing
		{
			// Token: 0x0400319C RID: 12700
			public int itemdefid;

			/// <summary>
			/// Was this item marked as new in the config?
			/// If new, and not marked as seen, then a "NEW" label is shown on the listing.
			/// </summary>
			// Token: 0x0400319D RID: 12701
			public bool isNew;

			// Token: 0x0400319E RID: 12702
			public ulong currentPrice;

			// Token: 0x0400319F RID: 12703
			public ulong basePrice;
		}

		// Token: 0x0200087F RID: 2175
		public struct CartEntry
		{
			// Token: 0x040031A0 RID: 12704
			public int itemdefid;

			// Token: 0x040031A1 RID: 12705
			public int quantity;
		}

		/// <summary>
		/// Messy, but we only show a menu alert if there was a problem.
		/// </summary>
		// Token: 0x02000880 RID: 2176
		public enum EPurchaseResult
		{
			// Token: 0x040031A3 RID: 12707
			UnableToInitialize,
			// Token: 0x040031A4 RID: 12708
			Denied
		}
	}
}
