using System;
using System.Collections.Generic;
using System.Globalization;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020001B3 RID: 435
	internal class SteamItemStore : ItemStore
	{
		// Token: 0x1400006F RID: 111
		// (add) Token: 0x06000DBD RID: 3517 RVA: 0x000300C8 File Offset: 0x0002E2C8
		// (remove) Token: 0x06000DBE RID: 3518 RVA: 0x00030100 File Offset: 0x0002E300
		public override event Action OnPricesReceived;

		// Token: 0x14000070 RID: 112
		// (add) Token: 0x06000DBF RID: 3519 RVA: 0x00030138 File Offset: 0x0002E338
		// (remove) Token: 0x06000DC0 RID: 3520 RVA: 0x00030170 File Offset: 0x0002E370
		public override event Action<ItemStore.EPurchaseResult> OnPurchaseResult;

		// Token: 0x06000DC1 RID: 3521 RVA: 0x000301A8 File Offset: 0x0002E3A8
		public override void ViewItem(int itemdefid)
		{
			if (this.listings != null && this.listings.Length != 0)
			{
				ItemStore.Listing listing;
				if (base.FindListing(itemdefid, out listing))
				{
					if (this.IsOverlayEnabledForCheckout)
					{
						UnturnedLog.info("Item store has listing for itemdefid {0}, using in-game menu", new object[]
						{
							itemdefid
						});
						MenuUI.closeAll();
						ItemStoreDetailsMenu.instance.Open(listing);
						return;
					}
					UnturnedLog.warn("Would not be able to checkout because Steam overlay is disabled, using browser");
				}
				else
				{
					UnturnedLog.warn("Item store does not have a listing for itemdefid {0}, using browser", new object[]
					{
						itemdefid
					});
				}
			}
			else
			{
				UnturnedLog.warn("Item store unavailable for itemdefid {0}, using browser", new object[]
				{
					itemdefid
				});
			}
			Provider.openURL("http://store.steampowered.com/itemstore/" + Provider.APP_ID.ToString() + "/detail/" + itemdefid.ToString());
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x00030274 File Offset: 0x0002E474
		public override void ViewNewItems()
		{
			if (base.HasNewListings)
			{
				if (this.IsOverlayEnabledForCheckout)
				{
					MenuUI.closeAll();
					ItemStoreMenu.instance.OpenNewItems();
					return;
				}
				UnturnedLog.warn("Would not be able to checkout because Steam overlay is disabled, using browser");
			}
			else
			{
				UnturnedLog.warn("Item store does not have listings for new items, using browser");
			}
			Provider.openURL("http://store.steampowered.com/itemstore/" + Provider.APP_ID.ToString() + "/browse/?filter=New");
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x000302E0 File Offset: 0x0002E4E0
		public override void ViewStore()
		{
			if (this.listings != null && this.listings.Length != 0)
			{
				if (this.IsOverlayEnabledForCheckout)
				{
					MenuUI.closeAll();
					ItemStoreMenu.instance.Open();
					return;
				}
				UnturnedLog.warn("Would not be able to checkout because Steam overlay is disabled, using browser");
			}
			else
			{
				UnturnedLog.warn("Item store unavailable, using browser");
			}
			Provider.openURL("http://store.steampowered.com/itemstore/" + Provider.APP_ID.ToString());
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x00030350 File Offset: 0x0002E550
		public override void RequestPrices()
		{
			UnturnedLog.info("Requesting Steam item store prices");
			SteamAPICall_t steamAPICall_t = SteamInventory.RequestPrices();
			if (steamAPICall_t != SteamAPICall_t.Invalid)
			{
				this.requestPricesCallResult.Set(steamAPICall_t, null);
				return;
			}
			UnturnedLog.info("Steam internal problem requesting item store prices");
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x00030394 File Offset: 0x0002E594
		public override void StartPurchase()
		{
			if (base.IsCartEmpty)
			{
				throw new Exception("should not have been called with an empty cart");
			}
			uint count = (uint)this.itemsInCart.Count;
			UnturnedLog.info("Requesting purchase of {0} item(s)", new object[]
			{
				count
			});
			SteamItemDef_t[] array = new SteamItemDef_t[count];
			uint[] array2 = new uint[count];
			int num = 0;
			while ((long)num < (long)((ulong)count))
			{
				ItemStore.CartEntry cartEntry = this.itemsInCart[num];
				array[num] = new SteamItemDef_t(cartEntry.itemdefid);
				array2[num] = (uint)cartEntry.quantity;
				UnturnedLog.info("[{0}]: {1} x {2}", new object[]
				{
					num,
					cartEntry.itemdefid,
					cartEntry.quantity
				});
				num++;
			}
			this.itemsInCart.Clear();
			SteamAPICall_t steamAPICall_t = SteamInventory.StartPurchase(array, array2, count);
			if (steamAPICall_t != SteamAPICall_t.Invalid)
			{
				this.startPurchaseCallResult.Set(steamAPICall_t, null);
				return;
			}
			UnturnedLog.info("Start purchase invalid input");
			Action<ItemStore.EPurchaseResult> onPurchaseResult = this.OnPurchaseResult;
			if (onPurchaseResult == null)
			{
				return;
			}
			onPurchaseResult.Invoke(ItemStore.EPurchaseResult.UnableToInitialize);
		}

		/// <summary>
		/// Steam currency codes seem to be ISO 4217, however the documentation (as of 2021-01-29) does not say so.
		/// </summary>
		// Token: 0x06000DC6 RID: 3526 RVA: 0x000304AC File Offset: 0x0002E6AC
		private NumberFormatInfo GetCurrencyFormatInfo(string threeLetterCode)
		{
			try
			{
				CultureInfo currentCulture = CultureInfo.CurrentCulture;
				if (currentCulture != null && string.Equals(new RegionInfo(currentCulture.LCID).ISOCurrencySymbol, threeLetterCode, 5))
				{
					UnturnedLog.info("Item store using current culture {0} for Steam currency code {1}", new object[]
					{
						currentCulture.DisplayName,
						threeLetterCode
					});
					return currentCulture.NumberFormat;
				}
				foreach (CultureInfo cultureInfo in CultureInfo.GetCultures(2))
				{
					if (string.Equals(new RegionInfo(cultureInfo.LCID).ISOCurrencySymbol, threeLetterCode, 5))
					{
						UnturnedLog.info("Item store using fallback culture {0} for Steam currency code {1}", new object[]
						{
							cultureInfo.DisplayName,
							threeLetterCode
						});
						return cultureInfo.NumberFormat;
					}
				}
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Exception trying to find region for Steam currency code {0}:", new object[]
				{
					threeLetterCode
				});
			}
			return null;
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x00030588 File Offset: 0x0002E788
		private void OnRequestPricesResultReady(SteamInventoryRequestPricesResult_t result, bool ioFailure)
		{
			if (ioFailure || result.m_result != EResult.k_EResultOK)
			{
				UnturnedLog.error("Request prices result: {0} I/O Failure: {1}", new object[]
				{
					result.m_result,
					ioFailure
				});
				return;
			}
			this.numberFormatInfo = this.GetCurrencyFormatInfo(result.m_rgchCurrency);
			if (this.numberFormatInfo == null)
			{
				UnturnedLog.error("Unable to find currency format info for Steam currency code: {0}", new object[]
				{
					result.m_rgchCurrency
				});
				return;
			}
			uint numItemsWithPrices = SteamInventory.GetNumItemsWithPrices();
			if (numItemsWithPrices < 1U)
			{
				UnturnedLog.error("Steam returned zero items with prices");
				return;
			}
			SteamItemDef_t[] array = new SteamItemDef_t[numItemsWithPrices];
			ulong[] array2 = new ulong[numItemsWithPrices];
			ulong[] array3 = new ulong[numItemsWithPrices];
			if (!SteamInventory.GetItemsWithPrices(array, array2, array3, numItemsWithPrices))
			{
				UnturnedLog.error("Unable to get items with prices");
				return;
			}
			List<ItemStore.Listing> list = new List<ItemStore.Listing>((int)numItemsWithPrices);
			List<int> list2 = new List<int>();
			for (uint num = 0U; num < numItemsWithPrices; num += 1U)
			{
				int steamItemDef = array[(int)num].m_SteamItemDef;
				if (!Provider.provider.economyService.IsItemKnown(steamItemDef))
				{
					UnturnedLog.warn("Item store missing details for itemdefid {0}", new object[]
					{
						steamItemDef
					});
				}
				else if (Provider.provider.economyService.isItemHiddenByCountryRestrictions(steamItemDef))
				{
					UnturnedLog.info("Item store hiding \"{0}\" due to country restrictions", new object[]
					{
						Provider.provider.economyService.getInventoryName(steamItemDef)
					});
				}
				else
				{
					ItemStore.Listing listing = default(ItemStore.Listing);
					listing.itemdefid = steamItemDef;
					listing.currentPrice = array2[(int)num];
					listing.basePrice = array3[(int)num];
					int count = list.Count;
					list.Add(listing);
					if (listing.currentPrice < listing.basePrice)
					{
						list2.Add(count);
					}
				}
			}
			if (list.Count < 1)
			{
				UnturnedLog.error("Item store has no valid listings");
				return;
			}
			this.listings = list.ToArray();
			this.discountedListingIndices = list2.ToArray();
			int num2 = base.HasDiscountedListings ? base.GetDiscountedListingIndices().Length : 0;
			int num3 = base.GetListings().Length;
			UnturnedLog.info(string.Format("Received Steam item store prices - Discounted: {0} All: {1}", num2, num3));
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x0003079C File Offset: 0x0002E99C
		private void OnStartPurchaseResultReady(SteamInventoryStartPurchaseResult_t result, bool ioFailure)
		{
			if (!ioFailure && result.m_result == EResult.k_EResultOK)
			{
				UnturnedLog.info("Start purchase Order ID: {0} Transaction ID: {1}", new object[]
				{
					result.m_ulOrderID,
					result.m_ulTransID
				});
				Provider.provider.economyService.isExpectingPurchaseResult = true;
				return;
			}
			UnturnedLog.error("Start purchase result: {0} I/O Failure: {1}", new object[]
			{
				result.m_result,
				ioFailure
			});
			Action<ItemStore.EPurchaseResult> onPurchaseResult = this.OnPurchaseResult;
			if (onPurchaseResult == null)
			{
				return;
			}
			onPurchaseResult.Invoke(ItemStore.EPurchaseResult.UnableToInitialize);
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x0003082C File Offset: 0x0002EA2C
		private void OnMicroTxnAuthorizationResponse(MicroTxnAuthorizationResponse_t responseData)
		{
			if (responseData.m_unAppID != Provider.APP_ID.m_AppId)
			{
				return;
			}
			if (responseData.m_bAuthorized > 0)
			{
				UnturnedLog.info("Purchase authorized Order ID: {0}", new object[]
				{
					responseData.m_ulOrderID
				});
				return;
			}
			Provider.provider.economyService.isExpectingPurchaseResult = false;
			UnturnedLog.info("Purchase denied Order ID: {0}", new object[]
			{
				responseData.m_ulOrderID
			});
			Action<ItemStore.EPurchaseResult> onPurchaseResult = this.OnPurchaseResult;
			if (onPurchaseResult == null)
			{
				return;
			}
			onPurchaseResult.Invoke(ItemStore.EPurchaseResult.Denied);
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x000308B4 File Offset: 0x0002EAB4
		public SteamItemStore()
		{
			this.requestPricesCallResult = CallResult<SteamInventoryRequestPricesResult_t>.Create(new CallResult<SteamInventoryRequestPricesResult_t>.APIDispatchDelegate(this.OnRequestPricesResultReady));
			this.startPurchaseCallResult = CallResult<SteamInventoryStartPurchaseResult_t>.Create(new CallResult<SteamInventoryStartPurchaseResult_t>.APIDispatchDelegate(this.OnStartPurchaseResultReady));
			this.microTxnAuthCallback = Callback<MicroTxnAuthorizationResponse_t>.Create(new Callback<MicroTxnAuthorizationResponse_t>.DispatchDelegate(this.OnMicroTxnAuthorizationResponse));
		}

		/// <summary>
		/// If overlay is disabled there is no point showing the in-game item store because the player will not be able
		/// to checkout. We request listings regardless in order to show the "sale" label automatically.
		/// </summary>
		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000DCB RID: 3531 RVA: 0x0003090C File Offset: 0x0002EB0C
		private bool IsOverlayEnabledForCheckout
		{
			get
			{
				return SteamUtils.IsOverlayEnabled();
			}
		}

		// Token: 0x04000553 RID: 1363
		public const string NEW_ITEM_PROMOTION_KEY = "NewItemSeenPromotionId";

		// Token: 0x04000556 RID: 1366
		private CallResult<SteamInventoryRequestPricesResult_t> requestPricesCallResult;

		// Token: 0x04000557 RID: 1367
		private CallResult<SteamInventoryStartPurchaseResult_t> startPurchaseCallResult;

		// Token: 0x04000558 RID: 1368
		private Callback<MicroTxnAuthorizationResponse_t> microTxnAuthCallback;
	}
}
