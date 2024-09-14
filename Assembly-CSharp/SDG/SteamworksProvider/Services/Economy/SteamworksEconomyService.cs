using System;
using System.Collections.Generic;
using SDG.Provider.Services;
using SDG.Provider.Services.Economy;
using SDG.Unturned;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Economy
{
	// Token: 0x02000028 RID: 40
	public class SteamworksEconomyService : Service, IEconomyService, IService
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x0000433A File Offset: 0x0000253A
		public bool canOpenInventory
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00004340 File Offset: 0x00002540
		public IEconomyRequestHandle requestInventory(EconomyRequestReadyCallback inventoryRequestReadyCallback)
		{
			SteamInventoryResult_t steamInventoryResult;
			SteamInventory.GetAllItems(out steamInventoryResult);
			return this.addInventoryRequestHandle(steamInventoryResult, inventoryRequestReadyCallback);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00004360 File Offset: 0x00002560
		public IEconomyRequestHandle requestPromo(EconomyRequestReadyCallback inventoryRequestReadyCallback)
		{
			SteamInventoryResult_t steamInventoryResult;
			SteamInventory.GrantPromoItems(out steamInventoryResult);
			return this.addInventoryRequestHandle(steamInventoryResult, inventoryRequestReadyCallback);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00004380 File Offset: 0x00002580
		public IEconomyRequestHandle exchangeItems(IEconomyItemInstance[] inputItemInstanceIDs, uint[] inputItemQuantities, IEconomyItemDefinition[] outputItemDefinitionIDs, uint[] outputItemQuantities, EconomyRequestReadyCallback inventoryRequestReadyCallback)
		{
			if (inputItemInstanceIDs.Length != inputItemQuantities.Length)
			{
				throw new ArgumentException("Input item arrays need to be the same length.", "inputItemQuantities");
			}
			if (outputItemDefinitionIDs.Length != outputItemQuantities.Length)
			{
				throw new ArgumentException("Output item arrays need to be the same length.", "outputItemQuantities");
			}
			SteamItemInstanceID_t[] array = new SteamItemInstanceID_t[inputItemInstanceIDs.Length];
			for (int i = 0; i < inputItemInstanceIDs.Length; i++)
			{
				SteamworksEconomyItemInstance steamworksEconomyItemInstance = (SteamworksEconomyItemInstance)inputItemInstanceIDs[i];
				array[i] = steamworksEconomyItemInstance.steamItemInstanceID;
			}
			SteamItemDef_t[] array2 = new SteamItemDef_t[outputItemDefinitionIDs.Length];
			for (int j = 0; j < outputItemDefinitionIDs.Length; j++)
			{
				SteamworksEconomyItemDefinition steamworksEconomyItemDefinition = (SteamworksEconomyItemDefinition)outputItemDefinitionIDs[j];
				array2[j] = steamworksEconomyItemDefinition.steamItemDef;
			}
			SteamInventoryResult_t steamInventoryResult;
			SteamInventory.ExchangeItems(out steamInventoryResult, array2, outputItemQuantities, (uint)array2.Length, array, inputItemQuantities, (uint)array.Length);
			return this.addInventoryRequestHandle(steamInventoryResult, inventoryRequestReadyCallback);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00004440 File Offset: 0x00002640
		public void open(ulong id)
		{
			Provider.openURL(string.Concat(new string[]
			{
				"http://steamcommunity.com/profiles/",
				SteamUser.GetSteamID().ToString(),
				"/inventory/?sellOnLoad=1#",
				SteamUtils.GetAppID().ToString(),
				"_2_",
				id.ToString()
			}));
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000044AC File Offset: 0x000026AC
		private SteamworksEconomyRequestHandle findSteamworksEconomyRequestHandles(SteamInventoryResult_t steamInventoryResult)
		{
			return this.steamworksEconomyRequestHandles.Find((SteamworksEconomyRequestHandle handle) => handle.steamInventoryResult == steamInventoryResult);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000044E0 File Offset: 0x000026E0
		private IEconomyRequestHandle addInventoryRequestHandle(SteamInventoryResult_t steamInventoryResult, EconomyRequestReadyCallback inventoryRequestReadyCallback)
		{
			SteamworksEconomyRequestHandle steamworksEconomyRequestHandle = new SteamworksEconomyRequestHandle(steamInventoryResult, inventoryRequestReadyCallback);
			this.steamworksEconomyRequestHandles.Add(steamworksEconomyRequestHandle);
			return steamworksEconomyRequestHandle;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00004504 File Offset: 0x00002704
		private IEconomyRequestResult createInventoryRequestResult(SteamInventoryResult_t steamInventoryResult)
		{
			uint num = 0U;
			SteamworksEconomyItem[] array2;
			if (SteamGameServerInventory.GetResultItems(steamInventoryResult, null, ref num) && num > 0U)
			{
				SteamItemDetails_t[] array = new SteamItemDetails_t[num];
				SteamGameServerInventory.GetResultItems(steamInventoryResult, array, ref num);
				array2 = new SteamworksEconomyItem[num];
				for (uint num2 = 0U; num2 < num; num2 += 1U)
				{
					SteamworksEconomyItem steamworksEconomyItem = new SteamworksEconomyItem(array[(int)num2]);
					array2[(int)num2] = steamworksEconomyItem;
				}
			}
			else
			{
				array2 = new SteamworksEconomyItem[0];
			}
			EEconomyRequestState newEconomyRequestState = EEconomyRequestState.SUCCESS;
			IEconomyItem[] newItems = array2;
			return new EconomyRequestResult(newEconomyRequestState, newItems);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00004570 File Offset: 0x00002770
		private void onSteamInventoryResultReady(SteamInventoryResultReady_t callback)
		{
			SteamworksEconomyRequestHandle steamworksEconomyRequestHandle = this.findSteamworksEconomyRequestHandles(callback.m_handle);
			if (steamworksEconomyRequestHandle == null)
			{
				return;
			}
			IEconomyRequestResult inventoryRequestResult = this.createInventoryRequestResult(steamworksEconomyRequestHandle.steamInventoryResult);
			steamworksEconomyRequestHandle.triggerInventoryRequestReadyCallback(inventoryRequestResult);
			SteamInventory.DestroyResult(steamworksEconomyRequestHandle.steamInventoryResult);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x000045AD File Offset: 0x000027AD
		public SteamworksEconomyService()
		{
			this.steamworksEconomyRequestHandles = new List<SteamworksEconomyRequestHandle>();
			this.steamInventoryResultReady = Callback<SteamInventoryResultReady_t>.Create(new Callback<SteamInventoryResultReady_t>.DispatchDelegate(this.onSteamInventoryResultReady));
		}

		// Token: 0x0400006A RID: 106
		private List<SteamworksEconomyRequestHandle> steamworksEconomyRequestHandles;

		// Token: 0x0400006B RID: 107
		private Callback<SteamInventoryResultReady_t> steamInventoryResultReady;
	}
}
