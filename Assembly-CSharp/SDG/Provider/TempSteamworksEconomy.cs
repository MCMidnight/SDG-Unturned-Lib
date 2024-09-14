using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using SDG.Framework.IO.Deserialization;
using SDG.SteamworksProvider;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using Unturned.SystemEx;
using Unturned.UnityEx;

namespace SDG.Provider
{
	// Token: 0x02000035 RID: 53
	public class TempSteamworksEconomy
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00004CAE File Offset: 0x00002EAE
		public bool canOpenInventory
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00004CB4 File Offset: 0x00002EB4
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

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00004D1F File Offset: 0x00002F1F
		// (set) Token: 0x06000130 RID: 304 RVA: 0x00004D26 File Offset: 0x00002F26
		public static List<UnturnedEconInfo> econInfo { get; private set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00004D2E File Offset: 0x00002F2E
		// (set) Token: 0x06000132 RID: 306 RVA: 0x00004D35 File Offset: 0x00002F35
		public static byte[] econInfoHash { get; private set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00004D3D File Offset: 0x00002F3D
		public List<SteamItemDetails_t> inventory
		{
			get
			{
				return this.inventoryDetails;
			}
		}

		/// <summary>
		/// Find the first instanceId of a given itemDefId.
		/// </summary>
		// Token: 0x06000134 RID: 308 RVA: 0x00004D48 File Offset: 0x00002F48
		public ulong getInventoryPackage(int item)
		{
			if (this.inventoryDetails != null)
			{
				for (int i = 0; i < this.inventoryDetails.Count; i++)
				{
					if (this.inventoryDetails[i].m_iDefinition.m_SteamItemDef == item)
					{
						return this.inventoryDetails[i].m_itemId.m_SteamItemInstanceID;
					}
				}
			}
			return 0UL;
		}

		/// <summary>
		/// Count quantity of a given itemDefId.
		/// </summary>
		// Token: 0x06000135 RID: 309 RVA: 0x00004DA8 File Offset: 0x00002FA8
		public int countInventoryPackages(int item)
		{
			int num = 0;
			if (this.inventoryDetails != null)
			{
				for (int i = 0; i < this.inventoryDetails.Count; i++)
				{
					if (this.inventoryDetails[i].m_iDefinition.m_SteamItemDef == item)
					{
						num += (int)this.inventoryDetails[i].m_unQuantity;
					}
				}
			}
			return num;
		}

		/// <summary>
		/// Find certain quantity of given itemDefId.
		/// </summary>
		// Token: 0x06000136 RID: 310 RVA: 0x00004E04 File Offset: 0x00003004
		public bool getInventoryPackages(int item, ushort requiredQuantity, out List<EconExchangePair> pairs)
		{
			ushort num = 0;
			pairs = new List<EconExchangePair>();
			if (this.inventoryDetails != null)
			{
				for (int i = 0; i < this.inventoryDetails.Count; i++)
				{
					if (this.inventoryDetails[i].m_iDefinition.m_SteamItemDef == item)
					{
						ushort num2 = this.inventoryDetails[i].m_unQuantity;
						if (num2 >= 1)
						{
							ushort num3 = requiredQuantity - num;
							if (num2 >= num3)
							{
								num2 = num3;
							}
							EconExchangePair econExchangePair = new EconExchangePair(this.inventoryDetails[i].m_itemId.m_SteamItemInstanceID, num2);
							pairs.Add(econExchangePair);
							num += num2;
							if (num == requiredQuantity)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00004EA8 File Offset: 0x000030A8
		public int getInventoryItem(ulong package)
		{
			if (this.inventoryDetails != null)
			{
				for (int i = 0; i < this.inventoryDetails.Count; i++)
				{
					if (this.inventoryDetails[i].m_itemId.m_SteamItemInstanceID == package)
					{
						return this.inventoryDetails[i].m_iDefinition.m_SteamItemDef;
					}
				}
			}
			return 0;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00004F04 File Offset: 0x00003104
		public string getInventoryTags(ulong instance)
		{
			DynamicEconDetails dynamicEconDetails;
			if (!this.dynamicInventoryDetails.TryGetValue(instance, ref dynamicEconDetails))
			{
				return string.Empty;
			}
			return dynamicEconDetails.tags;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00004F30 File Offset: 0x00003130
		public string getInventoryDynamicProps(ulong instance)
		{
			DynamicEconDetails dynamicEconDetails;
			if (!this.dynamicInventoryDetails.TryGetValue(instance, ref dynamicEconDetails))
			{
				return string.Empty;
			}
			return dynamicEconDetails.dynamic_props;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00004F5C File Offset: 0x0000315C
		public bool getInventoryStatTrackerValue(ulong instance, out EStatTrackerType type, out int kills)
		{
			DynamicEconDetails dynamicEconDetails;
			if (!this.dynamicInventoryDetails.TryGetValue(instance, ref dynamicEconDetails))
			{
				type = EStatTrackerType.NONE;
				kills = -1;
				return false;
			}
			return dynamicEconDetails.getStatTrackerValue(out type, out kills);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00004F8C File Offset: 0x0000318C
		public bool getInventoryRagdollEffect(ulong instance, out ERagdollEffect effect)
		{
			DynamicEconDetails dynamicEconDetails;
			if (!this.dynamicInventoryDetails.TryGetValue(instance, ref dynamicEconDetails))
			{
				effect = ERagdollEffect.NONE;
				return false;
			}
			return dynamicEconDetails.getRagdollEffect(out effect);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00004FB8 File Offset: 0x000031B8
		public ushort getInventoryParticleEffect(ulong instance)
		{
			DynamicEconDetails dynamicEconDetails;
			if (this.dynamicInventoryDetails.TryGetValue(instance, ref dynamicEconDetails))
			{
				return dynamicEconDetails.getParticleEffect();
			}
			return 0;
		}

		/// <summary>
		/// Does itemdefid exist in the EconInfo.json file?
		/// </summary>
		// Token: 0x0600013D RID: 317 RVA: 0x00004FE0 File Offset: 0x000031E0
		public bool IsItemKnown(int item)
		{
			return TempSteamworksEconomy.econInfo.Find((UnturnedEconInfo x) => x.itemdefid == item) != null;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00005014 File Offset: 0x00003214
		public string getInventoryName(int item)
		{
			UnturnedEconInfo unturnedEconInfo = TempSteamworksEconomy.econInfo.Find((UnturnedEconInfo x) => x.itemdefid == item);
			if (unturnedEconInfo == null)
			{
				return "";
			}
			return unturnedEconInfo.name;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00005054 File Offset: 0x00003254
		public string getInventoryType(int item)
		{
			UnturnedEconInfo unturnedEconInfo = TempSteamworksEconomy.econInfo.Find((UnturnedEconInfo x) => x.itemdefid == item);
			if (unturnedEconInfo == null)
			{
				return "";
			}
			return unturnedEconInfo.type;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00005094 File Offset: 0x00003294
		public string getInventoryDescription(int item)
		{
			UnturnedEconInfo unturnedEconInfo = TempSteamworksEconomy.econInfo.Find((UnturnedEconInfo x) => x.itemdefid == item);
			if (unturnedEconInfo == null)
			{
				return "";
			}
			return unturnedEconInfo.description;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000050D4 File Offset: 0x000032D4
		public bool getInventoryMarketable(int item)
		{
			UnturnedEconInfo unturnedEconInfo = TempSteamworksEconomy.econInfo.Find((UnturnedEconInfo x) => x.itemdefid == item);
			return unturnedEconInfo != null && unturnedEconInfo.marketable;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00005110 File Offset: 0x00003310
		public int getInventoryScraps(int item)
		{
			UnturnedEconInfo unturnedEconInfo = TempSteamworksEconomy.econInfo.Find((UnturnedEconInfo x) => x.itemdefid == item);
			if (unturnedEconInfo == null)
			{
				return 0;
			}
			return unturnedEconInfo.scraps;
		}

		/// <summary>
		/// Get item with an exchange recipe for the appropriate number of scraps.
		/// </summary>
		// Token: 0x06000143 RID: 323 RVA: 0x0000514C File Offset: 0x0000334C
		public int getScrapExchangeItem(int item)
		{
			switch (this.getInventoryScraps(item))
			{
			default:
				return 0;
			case 1:
				return 19006;
			case 2:
				return 19007;
			case 3:
				return 19008;
			case 4:
				return 19009;
			case 5:
				return 19010;
			case 10:
				return 19011;
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000051B8 File Offset: 0x000033B8
		public Color getInventoryColor(int item)
		{
			UnturnedEconInfo unturnedEconInfo = TempSteamworksEconomy.econInfo.Find((UnturnedEconInfo x) => x.itemdefid == item);
			if (unturnedEconInfo == null)
			{
				return Color.white;
			}
			uint num;
			if (unturnedEconInfo.name_color != null && unturnedEconInfo.name_color.Length > 0 && uint.TryParse(unturnedEconInfo.name_color, 515, CultureInfo.CurrentCulture, ref num))
			{
				uint num2 = num >> 16 & 255U;
				uint num3 = num >> 8 & 255U;
				uint num4 = num & 255U;
				return new Color(num2 / 255f, num3 / 255f, num4 / 255f);
			}
			return Color.white;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00005264 File Offset: 0x00003464
		public UnturnedEconInfo.EQuality getInventoryQuality(int item)
		{
			UnturnedEconInfo unturnedEconInfo = TempSteamworksEconomy.econInfo.Find((UnturnedEconInfo x) => x.itemdefid == item);
			if (unturnedEconInfo == null)
			{
				return UnturnedEconInfo.EQuality.None;
			}
			return unturnedEconInfo.quality;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x000052A0 File Offset: 0x000034A0
		public UnturnedEconInfo.ERarity getInventoryRarity(int item)
		{
			switch (this.getInventoryQuality(item))
			{
			default:
				return UnturnedEconInfo.ERarity.Unknown;
			case UnturnedEconInfo.EQuality.Common:
				return UnturnedEconInfo.ERarity.Common;
			case UnturnedEconInfo.EQuality.Uncommon:
				return UnturnedEconInfo.ERarity.Uncommon;
			case UnturnedEconInfo.EQuality.Gold:
				return UnturnedEconInfo.ERarity.Gold;
			case UnturnedEconInfo.EQuality.Rare:
				return UnturnedEconInfo.ERarity.Rare;
			case UnturnedEconInfo.EQuality.Epic:
				return UnturnedEconInfo.ERarity.Epic;
			case UnturnedEconInfo.EQuality.Legendary:
				return UnturnedEconInfo.ERarity.Legendary;
			case UnturnedEconInfo.EQuality.Mythical:
				return UnturnedEconInfo.ERarity.Mythical;
			case UnturnedEconInfo.EQuality.Premium:
				return UnturnedEconInfo.ERarity.Premium;
			case UnturnedEconInfo.EQuality.Achievement:
				return UnturnedEconInfo.ERarity.Achievement;
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x000052F8 File Offset: 0x000034F8
		public EItemRarity getGameRarity(int item)
		{
			switch (this.getInventoryQuality(item))
			{
			default:
				return EItemRarity.COMMON;
			case UnturnedEconInfo.EQuality.Uncommon:
				return EItemRarity.UNCOMMON;
			case UnturnedEconInfo.EQuality.Rare:
				return EItemRarity.RARE;
			case UnturnedEconInfo.EQuality.Epic:
				return EItemRarity.EPIC;
			case UnturnedEconInfo.EQuality.Legendary:
				return EItemRarity.LEGENDARY;
			case UnturnedEconInfo.EQuality.Mythical:
				return EItemRarity.MYTHICAL;
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00005348 File Offset: 0x00003548
		public Color getStatTrackerColor(EStatTrackerType type)
		{
			switch (type)
			{
			case EStatTrackerType.NONE:
				return Color.white;
			case EStatTrackerType.TOTAL:
				return new Color(1f, 0.5f, 0f);
			case EStatTrackerType.PLAYER:
				return new Color(1f, 0f, 0f);
			default:
				return Color.black;
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0000539E File Offset: 0x0000359E
		public string getStatTrackerPropertyName(EStatTrackerType type)
		{
			if (type == EStatTrackerType.TOTAL)
			{
				return "total_kills";
			}
			if (type != EStatTrackerType.PLAYER)
			{
				return null;
			}
			return "player_kills";
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000053B8 File Offset: 0x000035B8
		public ushort getInventoryMythicID(int item)
		{
			UnturnedEconInfo unturnedEconInfo = TempSteamworksEconomy.econInfo.Find((UnturnedEconInfo x) => x.itemdefid == item);
			if (unturnedEconInfo == null)
			{
				return 0;
			}
			return (ushort)unturnedEconInfo.item_effect;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x000053F8 File Offset: 0x000035F8
		public void getInventoryTargetID(int item, out Guid item_guid, out Guid vehicle_guid)
		{
			UnturnedEconInfo unturnedEconInfo = TempSteamworksEconomy.econInfo.Find((UnturnedEconInfo x) => x.itemdefid == item);
			if (unturnedEconInfo == null)
			{
				item_guid = default(Guid);
				vehicle_guid = default(Guid);
				return;
			}
			item_guid = unturnedEconInfo.item_guid;
			vehicle_guid = unturnedEconInfo.vehicle_guid;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00005454 File Offset: 0x00003654
		public Guid getInventoryItemGuid(int item)
		{
			Guid result;
			Guid guid;
			this.getInventoryTargetID(item, out result, out guid);
			return result;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00005470 File Offset: 0x00003670
		public ushort getInventorySkinID(int item)
		{
			UnturnedEconInfo unturnedEconInfo = TempSteamworksEconomy.econInfo.Find((UnturnedEconInfo x) => x.itemdefid == item);
			if (unturnedEconInfo == null)
			{
				return 0;
			}
			return (ushort)unturnedEconInfo.item_skin;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x000054B0 File Offset: 0x000036B0
		public Texture2D LoadItemIcon(int itemdefid)
		{
			UnturnedEconInfo unturnedEconInfo = TempSteamworksEconomy.econInfo.Find((UnturnedEconInfo x) => x.itemdefid == itemdefid);
			if (unturnedEconInfo == null)
			{
				return null;
			}
			if (unturnedEconInfo.item_guid != default(Guid))
			{
				ItemAsset itemAsset = Assets.find<ItemAsset>(unturnedEconInfo.item_guid);
				if (itemAsset == null)
				{
					return null;
				}
				if (itemAsset.econIconUseId)
				{
					return Resources.Load<Texture2D>("Economy/Item/" + itemdefid.ToString() + "/Icon_Large");
				}
				if (itemAsset.type == EItemType.SHIRT || itemAsset.type == EItemType.PANTS || itemAsset.type == EItemType.HAT || itemAsset.type == EItemType.BACKPACK || itemAsset.type == EItemType.VEST || itemAsset.type == EItemType.GLASSES || itemAsset.type == EItemType.MASK)
				{
					return Resources.Load<Texture2D>("Economy/CosmeticPreviews/" + itemAsset.GUID.ToString("N"));
				}
				if (itemAsset.proPath == null || itemAsset.proPath.Length == 0)
				{
					return null;
				}
				return Resources.Load<Texture2D>("Economy" + itemAsset.proPath + "/Icon_Large");
			}
			else
			{
				if (unturnedEconInfo.vehicle_guid != default(Guid))
				{
					return null;
				}
				return Resources.Load<Texture2D>("Economy/Item/" + itemdefid.ToString() + "/Icon_Large");
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00005600 File Offset: 0x00003800
		public void consumeItem(ulong instance, uint quantity)
		{
			UnturnedLog.info("Consume item: {0} x{1}", new object[]
			{
				instance,
				quantity
			});
			SteamInventoryResult_t steamInventoryResult_t;
			SteamInventory.ConsumeItem(out steamInventoryResult_t, (SteamItemInstanceID_t)instance, quantity);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00005640 File Offset: 0x00003840
		public void exchangeInventory(int generate, List<EconExchangePair> destroy)
		{
			UnturnedLog.info("Exchange these item instances for " + generate.ToString());
			for (int i = 0; i < destroy.Count; i++)
			{
				ulong instanceId = destroy[i].instanceId;
				int num = -1;
				for (int j = 0; j < this.inventoryDetails.Count; j++)
				{
					if (this.inventoryDetails[j].m_itemId.m_SteamItemInstanceID == instanceId)
					{
						num = j;
						break;
					}
				}
				if (num == -1)
				{
					UnturnedLog.error("Unable to find item for exchange: {0}", new object[]
					{
						instanceId
					});
					return;
				}
				SteamItemDetails_t steamItemDetails_t = this.inventoryDetails[num];
				UnturnedLog.info(string.Concat(new string[]
				{
					steamItemDetails_t.m_iDefinition.m_SteamItemDef.ToString(),
					" [",
					instanceId.ToString(),
					"] x",
					destroy[i].quantity.ToString()
				}));
				if (destroy[i].quantity >= steamItemDetails_t.m_unQuantity)
				{
					UnturnedLog.info("Locally removed item - Instance: {0} Definition: {1}", new object[]
					{
						instanceId,
						steamItemDetails_t.m_iDefinition.m_SteamItemDef
					});
					this.inventoryDetails.RemoveAtFast(num);
					this.dynamicInventoryDetails.Remove(instanceId);
				}
			}
			SteamItemDef_t[] array = new SteamItemDef_t[1];
			uint[] array2 = new uint[1];
			array[0] = (SteamItemDef_t)generate;
			array2[0] = 1U;
			SteamItemInstanceID_t[] array3 = new SteamItemInstanceID_t[destroy.Count];
			uint[] array4 = new uint[destroy.Count];
			for (int k = 0; k < destroy.Count; k++)
			{
				array3[k] = (SteamItemInstanceID_t)destroy[k].instanceId;
				array4[k] = (uint)destroy[k].quantity;
			}
			SteamInventory.ExchangeItems(out this.exchangeResult, array, array2, (uint)array.Length, array3, array4, (uint)array3.Length);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000583D File Offset: 0x00003A3D
		public void updateInventory()
		{
			if (Time.realtimeSinceStartup - this.lastHeartbeat < 30f)
			{
				return;
			}
			this.lastHeartbeat = Time.realtimeSinceStartup;
			SteamInventory.SendItemDropHeartbeat();
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00005863 File Offset: 0x00003A63
		public void dropInventory()
		{
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00005865 File Offset: 0x00003A65
		public void GrantPromoItems()
		{
			if (this.promoResult == SteamInventoryResult_t.Invalid)
			{
				UnturnedLog.info("Requesting promo item grant");
				SteamInventory.GrantPromoItems(out this.promoResult);
			}
		}

		/// <summary>
		/// One player's inventory became so large that the Steam client's built-in GetInventory fails,
		/// so as temporary fix we can send them a json file with their inventory.
		/// </summary>
		// Token: 0x06000154 RID: 340 RVA: 0x00005890 File Offset: 0x00003A90
		private void loadInventoryFromResponseFile(string filePath)
		{
			UnturnedLog.info("Loading Steam inventory from GetInventory response file: {0}", new object[]
			{
				filePath
			});
			List<SteamGetInventoryResponse.Item> list = SteamGetInventoryResponse.parse(filePath);
			this.dynamicInventoryDetails.Clear();
			this.inventoryDetails = new List<SteamItemDetails_t>(list.Count);
			foreach (SteamGetInventoryResponse.Item item in list)
			{
				SteamItemDetails_t steamItemDetails_t = default(SteamItemDetails_t);
				steamItemDetails_t.m_iDefinition = new SteamItemDef_t(item.itemdefid);
				steamItemDetails_t.m_itemId = new SteamItemInstanceID_t(item.itemid);
				steamItemDetails_t.m_unFlags = 0;
				steamItemDetails_t.m_unQuantity = item.quantity;
				this.inventoryDetails.Add(steamItemDetails_t);
			}
			TempSteamworksEconomy.InventoryRefreshed inventoryRefreshed = this.onInventoryRefreshed;
			if (inventoryRefreshed != null)
			{
				inventoryRefreshed();
			}
			this.isInventoryAvailable = true;
			Provider.isLoadingInventory = false;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000597C File Offset: 0x00003B7C
		public void refreshInventory()
		{
			UnturnedLog.info("Refreshing Steam inventory");
			string text = Path.Combine(ReadWrite.PATH, "SteamInventory.json");
			if (File.Exists(text))
			{
				try
				{
					this.loadInventoryFromResponseFile(text);
					return;
				}
				catch (Exception e)
				{
					UnturnedLog.exception(e);
					return;
				}
			}
			if (!SteamInventory.GetAllItems(out this.inventoryResult))
			{
				Provider.isLoadingInventory = false;
			}
		}

		/// <summary>
		/// Add an item locally that we know exists in the online inventory, but is just a matter of waiting for it.
		/// </summary>
		// Token: 0x06000156 RID: 342 RVA: 0x000059E0 File Offset: 0x00003BE0
		private void addLocalItem(SteamItemDetails_t item, string tags, string dynamic_props)
		{
			this.inventoryDetails.Add(item);
			this.dynamicInventoryDetails.Remove(item.m_itemId.m_SteamItemInstanceID);
			DynamicEconDetails dynamicEconDetails = default(DynamicEconDetails);
			dynamicEconDetails.tags = (string.IsNullOrEmpty(tags) ? string.Empty : tags);
			dynamicEconDetails.dynamic_props = (string.IsNullOrEmpty(dynamic_props) ? string.Empty : dynamic_props);
			this.dynamicInventoryDetails.Add(item.m_itemId.m_SteamItemInstanceID, dynamicEconDetails);
		}

		/// <summary>
		/// Remove an item locally that we know no longer exists in the online inventory.
		/// </summary>
		// Token: 0x06000157 RID: 343 RVA: 0x00005A60 File Offset: 0x00003C60
		private void removeLocalItem(SteamItemDetails_t item)
		{
			for (int i = 0; i < this.inventoryDetails.Count; i++)
			{
				if (this.inventoryDetails[i].m_itemId == item.m_itemId)
				{
					this.inventoryDetails.RemoveAtFast(i);
					break;
				}
			}
			this.dynamicInventoryDetails.Remove(item.m_itemId.m_SteamItemInstanceID);
		}

		/// <summary>
		/// Update our local version of an item that we know has changed, but we are waiting for a full refresh.
		/// </summary>
		// Token: 0x06000158 RID: 344 RVA: 0x00005AC8 File Offset: 0x00003CC8
		private bool updateLocalItem(SteamItemDetails_t item, SteamInventoryResult_t resultHandle, uint resultIndex)
		{
			this.removeLocalItem(item);
			bool flag = (item.m_unFlags & 256) > 0;
			bool flag2 = item.m_unQuantity < 1;
			if (flag || flag2)
			{
				UnturnedLog.info("Locally removed item - Instance: {0} Definition: {1}", new object[]
				{
					item.m_itemId,
					item.m_iDefinition
				});
				return false;
			}
			uint num = 1024U;
			string text;
			SteamInventory.GetResultItemProperty(resultHandle, resultIndex, "tags", out text, ref num);
			uint num2 = 1024U;
			string text2;
			SteamInventory.GetResultItemProperty(resultHandle, resultIndex, "dynamic_props", out text2, ref num2);
			this.addLocalItem(item, text, text2);
			UnturnedLog.info("Locally added or updated item - Instance: {0} Definition: {1} Tags: {2} Props: {3}", new object[]
			{
				item.m_itemId,
				item.m_iDefinition,
				text,
				text2
			});
			return true;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00005B94 File Offset: 0x00003D94
		private void handleServerResultReady(SteamInventoryResultReady_t callback)
		{
			SteamPending steamPending = null;
			for (int i = 0; i < Provider.pending.Count; i++)
			{
				if (Provider.pending[i].inventoryResult == callback.m_handle)
				{
					steamPending = Provider.pending[i];
					break;
				}
			}
			if (steamPending == null)
			{
				return;
			}
			if (callback.m_result != EResult.k_EResultOK || !SteamGameServerInventory.CheckResultSteamID(callback.m_handle, steamPending.playerID.steamID))
			{
				UnturnedLog.info("inventory auth: " + callback.m_result.ToString() + " " + SteamGameServerInventory.CheckResultSteamID(callback.m_handle, steamPending.playerID.steamID).ToString());
				Provider.reject(steamPending.playerID.steamID, ESteamRejection.AUTH_ECON_VERIFY);
				return;
			}
			uint num = 0U;
			if (SteamGameServerInventory.GetResultItems(callback.m_handle, null, ref num) && num > 0U)
			{
				steamPending.inventoryDetails = new SteamItemDetails_t[num];
				SteamGameServerInventory.GetResultItems(callback.m_handle, steamPending.inventoryDetails, ref num);
				for (uint num2 = 0U; num2 < num; num2 += 1U)
				{
					uint num3 = 1024U;
					string text;
					SteamGameServerInventory.GetResultItemProperty(callback.m_handle, num2, "tags", out text, ref num3);
					uint num4 = 1024U;
					string text2;
					SteamGameServerInventory.GetResultItemProperty(callback.m_handle, num2, "dynamic_props", out text2, ref num4);
					DynamicEconDetails dynamicEconDetails = default(DynamicEconDetails);
					dynamicEconDetails.tags = (string.IsNullOrEmpty(text) ? string.Empty : text);
					dynamicEconDetails.dynamic_props = (string.IsNullOrEmpty(text2) ? string.Empty : text2);
					steamPending.dynamicInventoryDetails.Add(steamPending.inventoryDetails[(int)num2].m_itemId.m_SteamItemInstanceID, dynamicEconDetails);
				}
			}
			steamPending.inventoryDetailsReady();
		}

		/// <summary>
		/// Callback when client knows which items were crafted or exchanged.
		/// </summary>
		// Token: 0x0600015A RID: 346 RVA: 0x00005D54 File Offset: 0x00003F54
		private void handleClientExchangeResultReady(SteamInventoryResultReady_t callback)
		{
			SteamInventoryResult_t handle = callback.m_handle;
			List<SteamItemDetails_t> list = new List<SteamItemDetails_t>();
			uint num = 0U;
			if (SteamInventory.GetResultItems(handle, null, ref num) && num > 0U)
			{
				UnturnedLog.info("Exchange result items: {0}", new object[]
				{
					num
				});
				SteamItemDetails_t[] array = new SteamItemDetails_t[num];
				if (SteamInventory.GetResultItems(handle, array, ref num))
				{
					for (uint num2 = 0U; num2 < num; num2 += 1U)
					{
						SteamItemDetails_t steamItemDetails_t = array[(int)num2];
						if (this.updateLocalItem(steamItemDetails_t, handle, num2))
						{
							list.Add(steamItemDetails_t);
						}
					}
				}
			}
			if (list.Count > 0)
			{
				TempSteamworksEconomy.InventoryExchanged inventoryExchanged = this.onInventoryExchanged;
				if (inventoryExchanged != null)
				{
					inventoryExchanged(list);
				}
				TempSteamworksEconomy.InventoryRefreshed inventoryRefreshed = this.onInventoryRefreshed;
				if (inventoryRefreshed == null)
				{
					return;
				}
				inventoryRefreshed();
				return;
			}
			else
			{
				TempSteamworksEconomy.InventoryExchangeFailed inventoryExchangeFailed = this.onInventoryExchangeFailed;
				if (inventoryExchangeFailed == null)
				{
					return;
				}
				inventoryExchangeFailed();
				return;
			}
		}

		/// <summary>
		/// Callback when client thinks result was from purchase.
		/// </summary>
		// Token: 0x0600015B RID: 347 RVA: 0x00005E18 File Offset: 0x00004018
		private void handleClientPurchaseResultReady(SteamInventoryResultReady_t callback)
		{
			SteamInventoryResult_t handle = callback.m_handle;
			List<SteamItemDetails_t> list = new List<SteamItemDetails_t>();
			uint num = 0U;
			if (SteamInventory.GetResultItems(handle, null, ref num) && num > 0U)
			{
				UnturnedLog.info("Purchase result items: {0}", new object[]
				{
					num
				});
				SteamItemDetails_t[] array = new SteamItemDetails_t[num];
				if (SteamInventory.GetResultItems(handle, array, ref num))
				{
					for (uint num2 = 0U; num2 < num; num2 += 1U)
					{
						SteamItemDetails_t steamItemDetails_t = array[(int)num2];
						if (this.updateLocalItem(steamItemDetails_t, handle, num2))
						{
							list.Add(steamItemDetails_t);
						}
					}
				}
			}
			if (list.Count > 0)
			{
				this.onInventoryPurchased(list);
			}
			TempSteamworksEconomy.InventoryRefreshed inventoryRefreshed = this.onInventoryRefreshed;
			if (inventoryRefreshed == null)
			{
				return;
			}
			inventoryRefreshed();
		}

		/// <summary>
		/// 2022-01-01 it does not seem to be documented by Steam, but we get SteamInventoryResultReady callbacks
		/// for external events like AddItem calls, so we may as well handle them.
		/// </summary>
		// Token: 0x0600015C RID: 348 RVA: 0x00005EC4 File Offset: 0x000040C4
		private void UpdateLocalItemsFromUnknownResult(SteamInventoryResult_t resultHandle)
		{
			uint num = 0U;
			if (SteamInventory.GetResultItems(resultHandle, null, ref num) && num > 0U)
			{
				SteamItemDetails_t[] array = new SteamItemDetails_t[num];
				if (SteamInventory.GetResultItems(resultHandle, array, ref num))
				{
					for (uint num2 = 0U; num2 < num; num2 += 1U)
					{
						this.updateLocalItem(array[(int)num2], resultHandle, num2);
					}
				}
			}
			TempSteamworksEconomy.InventoryRefreshed inventoryRefreshed = this.onInventoryRefreshed;
			if (inventoryRefreshed == null)
			{
				return;
			}
			inventoryRefreshed();
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00005F20 File Offset: 0x00004120
		private void DumpInventoryResult(SteamInventoryResult_t handle)
		{
			uint num = 0U;
			if (!SteamInventory.GetResultItems(handle, null, ref num))
			{
				UnturnedLog.warn("Unable to get result items length from handle {0}", new object[]
				{
					handle
				});
				return;
			}
			if (num < 1U)
			{
				UnturnedLog.info("Handle {0} result items empty", new object[]
				{
					handle
				});
				return;
			}
			UnturnedLog.info("Handle {0} contains {1} result item(s)", new object[]
			{
				handle,
				num
			});
			SteamItemDetails_t[] array = new SteamItemDetails_t[num];
			if (!SteamInventory.GetResultItems(handle, array, ref num))
			{
				UnturnedLog.warn("Unable to get result items from handle {0}", new object[]
				{
					handle
				});
				return;
			}
			for (uint num2 = 0U; num2 < num; num2 += 1U)
			{
				SteamItemDetails_t steamItemDetails_t = array[(int)num2];
				UnturnedLog.info("[{0}] Instance: {1} Def: {2} Quantity: {3} Flags: {4}", new object[]
				{
					num2,
					steamItemDetails_t.m_itemId,
					steamItemDetails_t.m_iDefinition,
					steamItemDetails_t.m_unQuantity,
					(ESteamItemFlags)steamItemDetails_t.m_unFlags
				});
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00006028 File Offset: 0x00004228
		private void handleClientResultReady(SteamInventoryResultReady_t callback)
		{
			if (this.promoResult != SteamInventoryResult_t.Invalid && callback.m_handle == this.promoResult)
			{
				SteamInventory.DestroyResult(this.promoResult);
				this.promoResult = SteamInventoryResult_t.Invalid;
				return;
			}
			if (this.exchangeResult != SteamInventoryResult_t.Invalid && callback.m_handle == this.exchangeResult)
			{
				this.handleClientExchangeResultReady(callback);
				SteamInventory.DestroyResult(this.exchangeResult);
				this.exchangeResult = SteamInventoryResult_t.Invalid;
				return;
			}
			if (this.dropResult != SteamInventoryResult_t.Invalid && callback.m_handle == this.dropResult)
			{
				SteamItemDetails_t[] array = null;
				string tags = null;
				string dynamic_props = null;
				uint num = 0U;
				if (SteamInventory.GetResultItems(this.dropResult, null, ref num) && num > 0U)
				{
					array = new SteamItemDetails_t[num];
					SteamInventory.GetResultItems(this.dropResult, array, ref num);
					uint num2 = 1024U;
					SteamInventory.GetResultItemProperty(this.dropResult, 0U, "tags", out tags, ref num2);
					uint num3 = 1024U;
					SteamInventory.GetResultItemProperty(this.dropResult, 0U, "dynamic_props", out dynamic_props, ref num3);
				}
				UnturnedLog.info("onInventoryResultReady: Drop {0}", new object[]
				{
					num
				});
				if (array != null && num > 0U)
				{
					SteamItemDetails_t steamItemDetails_t = array[0];
					this.addLocalItem(steamItemDetails_t, tags, dynamic_props);
					TempSteamworksEconomy.InventoryDropped inventoryDropped = this.onInventoryDropped;
					if (inventoryDropped != null)
					{
						inventoryDropped(steamItemDetails_t.m_iDefinition.m_SteamItemDef, steamItemDetails_t.m_unQuantity, steamItemDetails_t.m_itemId.m_SteamItemInstanceID);
					}
					TempSteamworksEconomy.InventoryRefreshed inventoryRefreshed = this.onInventoryRefreshed;
					if (inventoryRefreshed != null)
					{
						inventoryRefreshed();
					}
				}
				SteamInventory.DestroyResult(this.dropResult);
				this.dropResult = SteamInventoryResult_t.Invalid;
				return;
			}
			if (!(this.wearingResult != SteamInventoryResult_t.Invalid) || !(callback.m_handle == this.wearingResult))
			{
				if (this.inventoryResult != SteamInventoryResult_t.Invalid && callback.m_handle == this.inventoryResult)
				{
					this.dynamicInventoryDetails.Clear();
					uint num4 = 0U;
					if (SteamInventory.GetResultItems(this.inventoryResult, null, ref num4) && num4 > 0U)
					{
						SteamItemDetails_t[] array2 = new SteamItemDetails_t[num4];
						SteamInventory.GetResultItems(this.inventoryResult, array2, ref num4);
						for (uint num5 = 0U; num5 < num4; num5 += 1U)
						{
							uint num6 = 1024U;
							string text;
							SteamInventory.GetResultItemProperty(this.inventoryResult, num5, "tags", out text, ref num6);
							uint num7 = 1024U;
							string text2;
							SteamInventory.GetResultItemProperty(this.inventoryResult, num5, "dynamic_props", out text2, ref num7);
							DynamicEconDetails dynamicEconDetails = default(DynamicEconDetails);
							dynamicEconDetails.tags = (string.IsNullOrEmpty(text) ? string.Empty : text);
							dynamicEconDetails.dynamic_props = (string.IsNullOrEmpty(text2) ? string.Empty : text2);
							this.dynamicInventoryDetails.Add(array2[(int)num5].m_itemId.m_SteamItemInstanceID, dynamicEconDetails);
							if (array2[(int)num5].m_unQuantity < 1)
							{
								uint num8 = 64U;
								string text3;
								if (SteamInventory.GetResultItemProperty(this.inventoryResult, num5, "quantity", out text3, ref num8))
								{
									ulong num9;
									if (ulong.TryParse(text3, ref num9))
									{
										ushort num10;
										if (num9 > 65535UL)
										{
											num10 = ushort.MaxValue;
										}
										else
										{
											num10 = (ushort)num9;
										}
										UnturnedLog.info(string.Format("Used quantity string fallback for itemdefid {0} (actual: {1} clamped: {2})", array2[(int)num5].m_iDefinition, num9, num10));
									}
									else
									{
										UnturnedLog.warn(string.Format("Tried using quantity string fallback for itemdefid {0} but unable to parse \"{1}\"", array2[(int)num5].m_iDefinition, text3));
									}
								}
								else
								{
									UnturnedLog.warn(string.Format("Tried using quantity string fallback for itemdefid {0} but GetResultItemProperty returned false", array2[(int)num5].m_iDefinition));
								}
							}
						}
						this.inventoryDetails = new List<SteamItemDetails_t>(array2);
					}
					TempSteamworksEconomy.InventoryRefreshed inventoryRefreshed2 = this.onInventoryRefreshed;
					if (inventoryRefreshed2 != null)
					{
						inventoryRefreshed2();
					}
					this.isInventoryAvailable = true;
					Provider.isLoadingInventory = false;
					SteamInventory.DestroyResult(this.inventoryResult);
					this.inventoryResult = SteamInventoryResult_t.Invalid;
					return;
				}
				if (this.commitResult != SteamInventoryResult_t.Invalid && callback.m_handle == this.commitResult)
				{
					UnturnedLog.info("Commit dynamic properties result: " + callback.m_result.ToString());
					SteamInventory.DestroyResult(this.commitResult);
					this.commitResult = SteamInventoryResult_t.Invalid;
					return;
				}
				if (this.isExpectingPurchaseResult)
				{
					this.isExpectingPurchaseResult = false;
					this.handleClientPurchaseResultReady(callback);
					SteamInventory.DestroyResult(callback.m_handle);
					return;
				}
				UnturnedLog.info("Unexpected client inventory result ready callback  - Handle: {0} Result: {1}", new object[]
				{
					callback.m_handle,
					callback.m_result
				});
				this.UpdateLocalItemsFromUnknownResult(callback.m_handle);
				this.DumpInventoryResult(callback.m_handle);
				SteamInventory.DestroyResult(callback.m_handle);
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x000064F5 File Offset: 0x000046F5
		private void onInventoryResultReady(SteamInventoryResultReady_t callback)
		{
			if (this.appInfo.isDedicated)
			{
				this.handleServerResultReady(callback);
				return;
			}
			this.handleClientResultReady(callback);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00006514 File Offset: 0x00004714
		public void loadTranslationEconInfo()
		{
			if (Provider.language == "English")
			{
				return;
			}
			string text = Provider.localizationRoot + "/EconInfo.json";
			if (!ReadWrite.fileExists(text, false, false))
			{
				UnturnedLog.warn("Looked for economy translation at: {0}", new object[]
				{
					text
				});
				return;
			}
			IDeserializer deserializer = new JSONDeserializer();
			new List<UnturnedEconInfo>();
			using (List<UnturnedEconInfo>.Enumerator enumerator = deserializer.deserialize<List<UnturnedEconInfo>>(text).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UnturnedEconInfo translatedItem = enumerator.Current;
					UnturnedEconInfo unturnedEconInfo = TempSteamworksEconomy.econInfo.Find((UnturnedEconInfo x) => x.itemdefid == translatedItem.itemdefid);
					if (unturnedEconInfo != null)
					{
						unturnedEconInfo.name = translatedItem.name;
						unturnedEconInfo.type = translatedItem.type;
						unturnedEconInfo.description = translatedItem.description;
					}
				}
			}
		}

		/// <summary>
		/// Do we know the player's region?
		/// If not, default to not allowing random items.
		/// </summary>
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00006604 File Offset: 0x00004804
		// (set) Token: 0x06000162 RID: 354 RVA: 0x0000660C File Offset: 0x0000480C
		public bool hasCountryDetails { get; protected set; }

		/// <summary>
		/// Does the player's region allow crates and keys to be used?
		/// Similar to TF2 and other Valve games we disable unboxing in certain regions.
		/// </summary>
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00006615 File Offset: 0x00004815
		// (set) Token: 0x06000164 RID: 356 RVA: 0x0000661D File Offset: 0x0000481D
		public bool doesCountryAllowRandomItems { get; protected set; }

		/// <summary>
		/// If player's region does not allow crates and keys to be used, return the country code.
		/// </summary>
		// Token: 0x06000165 RID: 357 RVA: 0x00006626 File Offset: 0x00004826
		public string getCountryWarningId()
		{
			return SteamUtils.GetIPCountry();
		}

		/// <summary>
		/// Similar to TF2 and other Valve games we disable unboxing in certain regions, so hide those items.
		/// </summary>
		// Token: 0x06000166 RID: 358 RVA: 0x00006630 File Offset: 0x00004830
		public bool isItemHiddenByCountryRestrictions(int itemdefid)
		{
			if (this.doesCountryAllowRandomItems)
			{
				return false;
			}
			ItemAsset itemAsset = Assets.find<ItemAsset>(this.getInventoryItemGuid(itemdefid));
			return itemAsset != null && (itemAsset.type == EItemType.KEY || itemAsset.type == EItemType.BOX);
		}

		/// <summary>
		/// Similar to TF2 and other Valve games we disable unboxing in certain regions.
		/// </summary>
		// Token: 0x06000167 RID: 359 RVA: 0x00006670 File Offset: 0x00004870
		private void initCountryRestrictions()
		{
			string ipcountry = SteamUtils.GetIPCountry();
			if (string.IsNullOrWhiteSpace(ipcountry))
			{
				this.hasCountryDetails = false;
				this.doesCountryAllowRandomItems = false;
				UnturnedLog.warn("Unable to determine country/region");
				return;
			}
			this.hasCountryDetails = true;
			this.doesCountryAllowRandomItems = true;
			this.doesCountryAllowRandomItems &= !string.Equals(ipcountry, "BE");
			this.doesCountryAllowRandomItems &= !string.Equals(ipcountry, "NL");
		}

		/// <summary>
		/// Not called on dedicated server.
		/// </summary>
		// Token: 0x06000168 RID: 360 RVA: 0x000066E8 File Offset: 0x000048E8
		public void initializeClient()
		{
			this.initCountryRestrictions();
		}

		// Token: 0x06000169 RID: 361 RVA: 0x000066F0 File Offset: 0x000048F0
		public TempSteamworksEconomy(SteamworksAppInfo newAppInfo)
		{
			this.appInfo = newAppInfo;
			string text;
			if (UnityPaths.ProjectDirectory != null)
			{
				text = PathEx.Join(UnityPaths.ProjectDirectory, "Builds", "Shared_Release", "EconInfo.json");
			}
			else
			{
				text = PathEx.Join(UnturnedPaths.RootDirectory, "EconInfo.json");
			}
			using (FileStream fileStream = new FileStream(text, 3, 1, 1))
			{
				using (SHA1Stream sha1Stream = new SHA1Stream(fileStream))
				{
					using (StreamReader streamReader = new StreamReader(sha1Stream))
					{
						using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
						{
							TempSteamworksEconomy.econInfo = new JsonSerializer().Deserialize<List<UnturnedEconInfo>>(jsonTextReader);
							TempSteamworksEconomy.econInfoHash = sha1Stream.Hash;
						}
					}
				}
			}
			if (this.appInfo.isDedicated)
			{
				this.inventoryResultReady = Callback<SteamInventoryResultReady_t>.CreateGameServer(new Callback<SteamInventoryResultReady_t>.DispatchDelegate(this.onInventoryResultReady));
				return;
			}
			this.inventoryResultReady = Callback<SteamInventoryResultReady_t>.Create(new Callback<SteamInventoryResultReady_t>.DispatchDelegate(this.onInventoryResultReady));
		}

		// Token: 0x0400007D RID: 125
		public TempSteamworksEconomy.InventoryRefreshed onInventoryRefreshed;

		// Token: 0x0400007E RID: 126
		public TempSteamworksEconomy.InventoryDropped onInventoryDropped;

		/// <summary>
		/// Invoked after a successful exchange with the newly granted items.
		/// </summary>
		// Token: 0x0400007F RID: 127
		public TempSteamworksEconomy.InventoryExchanged onInventoryExchanged;

		/// <summary>
		/// Invoke after a succesful purchase from the item store.
		/// </summary>
		// Token: 0x04000080 RID: 128
		public TempSteamworksEconomy.InventoryExchanged onInventoryPurchased;

		// Token: 0x04000081 RID: 129
		public TempSteamworksEconomy.InventoryExchangeFailed onInventoryExchangeFailed;

		// Token: 0x04000082 RID: 130
		private SteamInventoryResult_t promoResult = SteamInventoryResult_t.Invalid;

		// Token: 0x04000083 RID: 131
		public SteamInventoryResult_t exchangeResult = SteamInventoryResult_t.Invalid;

		// Token: 0x04000084 RID: 132
		public SteamInventoryResult_t dropResult = SteamInventoryResult_t.Invalid;

		// Token: 0x04000085 RID: 133
		public SteamInventoryResult_t wearingResult = SteamInventoryResult_t.Invalid;

		// Token: 0x04000086 RID: 134
		public SteamInventoryResult_t inventoryResult = SteamInventoryResult_t.Invalid;

		// Token: 0x04000087 RID: 135
		public SteamInventoryResult_t commitResult = SteamInventoryResult_t.Invalid;

		// Token: 0x04000088 RID: 136
		public List<SteamItemDetails_t> inventoryDetails = new List<SteamItemDetails_t>(0);

		// Token: 0x04000089 RID: 137
		public Dictionary<ulong, DynamicEconDetails> dynamicInventoryDetails = new Dictionary<ulong, DynamicEconDetails>();

		// Token: 0x0400008A RID: 138
		public bool isInventoryAvailable;

		/// <summary>
		/// Purchase result does not have a handle, so we guess based on when it arrives.
		/// </summary>
		// Token: 0x0400008B RID: 139
		public bool isExpectingPurchaseResult;

		// Token: 0x0400008C RID: 140
		private SteamworksAppInfo appInfo;

		// Token: 0x0400008D RID: 141
		private float lastHeartbeat;

		// Token: 0x0400008E RID: 142
		private Callback<SteamInventoryResultReady_t> inventoryResultReady;

		// Token: 0x02000832 RID: 2098
		// (Invoke) Token: 0x06004767 RID: 18279
		public delegate void InventoryRefreshed();

		// Token: 0x02000833 RID: 2099
		// (Invoke) Token: 0x0600476B RID: 18283
		public delegate void InventoryDropped(int item, ushort quantity, ulong instance);

		// Token: 0x02000834 RID: 2100
		// (Invoke) Token: 0x0600476F RID: 18287
		public delegate void InventoryExchanged(List<SteamItemDetails_t> grantedItems);

		// Token: 0x02000835 RID: 2101
		// (Invoke) Token: 0x06004773 RID: 18291
		public delegate void InventoryExchangeFailed();
	}
}
