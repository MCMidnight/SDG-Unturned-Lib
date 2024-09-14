using System;
using System.Collections.Generic;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x0200076A RID: 1898
	public class SpawnTableTool
	{
		/// <summary>
		/// Returning an Asset rather than the older IDs allows GUIDs to be used.
		/// legacyTargetAssetType is required for compatibility with spawn tables using legacy 16-bit IDs. If set to
		/// None and the spawn asset uses legacy IDs a warning is logged explaining GUIDs are necessary.
		/// </summary>
		/// <returns></returns>
		// Token: 0x06003E03 RID: 15875 RVA: 0x0012CCE8 File Offset: 0x0012AEE8
		public static Asset Resolve(SpawnAsset spawnAsset, EAssetType legacyTargetAssetType, Func<string> errorContextCallback)
		{
			if (spawnAsset == null)
			{
				if (Assets.shouldLoadAnyAssets)
				{
					UnturnedLog.error((((errorContextCallback != null) ? errorContextCallback.Invoke() : null) ?? "Unknown") + " attempted to resolve null spawn table");
				}
				return null;
			}
			for (int i = 0; i < 32; i++)
			{
				SpawnTable spawnTable = spawnAsset.PickRandomEntry(errorContextCallback);
				if (spawnTable == null)
				{
					UnturnedLog.warn(string.Concat(new string[]
					{
						"Spawn table \"",
						spawnAsset.name,
						"\" from ",
						spawnAsset.GetOriginName(),
						" resolved by ",
						((errorContextCallback != null) ? errorContextCallback.Invoke() : null) ?? "Unknown",
						" returned null entry"
					}));
					return null;
				}
				if (spawnTable.legacySpawnId != 0)
				{
					SpawnAsset spawnAsset2 = Assets.find(EAssetType.SPAWN, spawnTable.legacySpawnId) as SpawnAsset;
					if (spawnAsset2 == null)
					{
						UnturnedLog.warn(string.Format("Spawn table \"{0}\" from {1} resolved by {2} unable to find table matching legacy spawn ID {3}", new object[]
						{
							spawnAsset.name,
							spawnAsset.GetOriginName(),
							((errorContextCallback != null) ? errorContextCallback.Invoke() : null) ?? "Unknown",
							spawnTable.legacySpawnId
						}));
						return null;
					}
					spawnAsset = spawnAsset2;
				}
				else if (spawnTable.legacyAssetId != 0)
				{
					if (legacyTargetAssetType == EAssetType.NONE)
					{
						UnturnedLog.warn(string.Format("Spawn table \"{0}\" from {1} resolved by {2} unable to use legacy ID {3} because context does not support legacy IDs", new object[]
						{
							spawnAsset.name,
							spawnAsset.GetOriginName(),
							((errorContextCallback != null) ? errorContextCallback.Invoke() : null) ?? "Unknown",
							spawnTable.legacyAssetId
						}));
						return null;
					}
					Asset asset = Assets.find(legacyTargetAssetType, spawnTable.legacyAssetId);
					if (asset == null)
					{
						UnturnedLog.warn(string.Format("Spawn table \"{0}\" from {1} resolved by {2} unable to find asset matching legacy ID {3}", new object[]
						{
							spawnAsset.name,
							spawnAsset.GetOriginName(),
							((errorContextCallback != null) ? errorContextCallback.Invoke() : null) ?? "Unknown",
							spawnTable.legacyAssetId
						}));
						return null;
					}
					return asset;
				}
				else
				{
					if (GuidExtension.IsEmpty(spawnTable.targetGuid))
					{
						return null;
					}
					Asset asset2 = Assets.find(spawnTable.targetGuid);
					if (asset2 == null)
					{
						UnturnedLog.warn(string.Format("Spawn table \"{0}\" from {1} resolved by {2} unable to find asset matching GUID {3}", new object[]
						{
							spawnAsset.name,
							spawnAsset.GetOriginName(),
							((errorContextCallback != null) ? errorContextCallback.Invoke() : null) ?? "Unknown",
							spawnTable.targetGuid
						}));
						return null;
					}
					SpawnAsset spawnAsset3 = asset2 as SpawnAsset;
					if (spawnAsset3 == null)
					{
						return asset2;
					}
					spawnAsset = spawnAsset3;
				}
			}
			UnturnedLog.warn(string.Concat(new string[]
			{
				"Spawn table \"",
				spawnAsset.name,
				"\" from ",
				spawnAsset.GetOriginName(),
				" resolved by ",
				((errorContextCallback != null) ? errorContextCallback.Invoke() : null) ?? "Unknown",
				" may have encountered a recursive loop and has given up"
			}));
			return null;
		}

		/// <summary>
		/// Doesn't support spawn assets with legacy 16-bit IDs.
		/// </summary>
		// Token: 0x06003E04 RID: 15876 RVA: 0x0012CFB7 File Offset: 0x0012B1B7
		public static Asset Resolve(SpawnAsset spawnAsset, Func<string> errorContextCallback)
		{
			return SpawnTableTool.Resolve(spawnAsset, EAssetType.NONE, errorContextCallback);
		}

		// Token: 0x06003E05 RID: 15877 RVA: 0x0012CFC4 File Offset: 0x0012B1C4
		public static Asset Resolve(Guid spawnAssetGuid, EAssetType legacyTargetAssetType, Func<string> errorContextCallback)
		{
			if (GuidExtension.IsEmpty(spawnAssetGuid))
			{
				return null;
			}
			SpawnAsset spawnAsset = Assets.find(spawnAssetGuid) as SpawnAsset;
			if (spawnAsset == null)
			{
				if (Assets.shouldLoadAnyAssets)
				{
					UnturnedLog.error(string.Format("Unable to find spawn table with guid {0} resolved by {1}", spawnAssetGuid, ((errorContextCallback != null) ? errorContextCallback.Invoke() : null) ?? "Unknown"));
				}
				return null;
			}
			return SpawnTableTool.Resolve(spawnAsset, legacyTargetAssetType, errorContextCallback);
		}

		// Token: 0x06003E06 RID: 15878 RVA: 0x0012D02C File Offset: 0x0012B22C
		public static Asset Resolve(ushort spawnAssetLegacyId, EAssetType legacyTargetAssetType, Func<string> errorContextCallback)
		{
			if (spawnAssetLegacyId == 0)
			{
				return null;
			}
			SpawnAsset spawnAsset = Assets.find(EAssetType.SPAWN, spawnAssetLegacyId) as SpawnAsset;
			if (spawnAsset == null)
			{
				if (Assets.shouldLoadAnyAssets)
				{
					UnturnedLog.error(string.Format("Unable to find spawn table with legacy ID {0} resolved by {1}", spawnAssetLegacyId, ((errorContextCallback != null) ? errorContextCallback.Invoke() : null) ?? "Unknown"));
				}
				return null;
			}
			return SpawnTableTool.Resolve(spawnAsset, legacyTargetAssetType, errorContextCallback);
		}

		/// <summary>
		/// For backwards compatibility with features that still need a legacy ID rather than asset.
		/// </summary>
		// Token: 0x06003E07 RID: 15879 RVA: 0x0012D08F File Offset: 0x0012B28F
		public static ushort ResolveLegacyId(SpawnAsset spawnAsset, EAssetType legacyTargetAssetType, Func<string> errorContextCallback)
		{
			Asset asset = SpawnTableTool.Resolve(spawnAsset, legacyTargetAssetType, errorContextCallback);
			if (asset == null)
			{
				return 0;
			}
			return asset.id;
		}

		/// <summary>
		/// For backwards compatibility with features that still need a legacy ID rather than asset.
		/// </summary>
		// Token: 0x06003E08 RID: 15880 RVA: 0x0012D0A4 File Offset: 0x0012B2A4
		public static ushort ResolveLegacyId(Guid spawnAssetGuid, EAssetType legacyTargetAssetType, Func<string> errorContextCallback)
		{
			Asset asset = SpawnTableTool.Resolve(spawnAssetGuid, legacyTargetAssetType, errorContextCallback);
			if (asset == null)
			{
				return 0;
			}
			return asset.id;
		}

		/// <summary>
		/// For backwards compatibility with features that still need a legacy ID rather than asset.
		/// </summary>
		// Token: 0x06003E09 RID: 15881 RVA: 0x0012D0B9 File Offset: 0x0012B2B9
		public static ushort ResolveLegacyId(ushort spawnAssetLegacyId, EAssetType legacyTargetAssetType, Func<string> errorContextCallback)
		{
			Asset asset = SpawnTableTool.Resolve(spawnAssetLegacyId, legacyTargetAssetType, errorContextCallback);
			if (asset == null)
			{
				return 0;
			}
			return asset.id;
		}

		// Token: 0x06003E0A RID: 15882 RVA: 0x0012D0D0 File Offset: 0x0012B2D0
		[Obsolete("Please update to the newer Resolve methods with legacyTargetAssetType parameter which support GUIDs")]
		public static ushort resolve(ushort id)
		{
			SpawnAsset spawnAsset = Assets.find(EAssetType.SPAWN, id) as SpawnAsset;
			if (spawnAsset == null)
			{
				if (Assets.shouldLoadAnyAssets)
				{
					UnturnedLog.error("Unable to find spawn table for resolve with id " + id.ToString());
				}
				return 0;
			}
			bool flag;
			spawnAsset.resolve(out id, out flag);
			if (flag)
			{
				id = SpawnTableTool.resolve(id);
			}
			return id;
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x0012D128 File Offset: 0x0012B328
		private static bool isVariantItemTier(ItemTier tier)
		{
			if (tier.table.Count < 6)
			{
				return false;
			}
			ItemAsset itemAsset = Assets.find(EAssetType.ITEM, tier.table[0].item) as ItemAsset;
			if (itemAsset == null)
			{
				return false;
			}
			int num = itemAsset.itemName.IndexOf(" ");
			if (num <= 0)
			{
				return false;
			}
			string text = itemAsset.itemName.Substring(num + 1);
			if (text.Length <= 1)
			{
				UnturnedLog.error(itemAsset.itemName + " name has a trailing space!");
				return false;
			}
			for (int i = 1; i < tier.table.Count; i++)
			{
				if (!(Assets.find(EAssetType.ITEM, tier.table[i].item) as ItemAsset).itemName.Contains(text))
				{
					return false;
				}
			}
			tier.name = text;
			return true;
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x0012D1F8 File Offset: 0x0012B3F8
		private static bool isVariantVehicleTier(VehicleTier tier)
		{
			if (tier.table.Count < 6)
			{
				return false;
			}
			VehicleAsset vehicleAsset = Assets.find(EAssetType.VEHICLE, tier.table[0].vehicle) as VehicleAsset;
			if (vehicleAsset == null)
			{
				return false;
			}
			int num = vehicleAsset.vehicleName.IndexOf(" ");
			if (num <= 0)
			{
				return false;
			}
			string text = vehicleAsset.vehicleName.Substring(num + 1);
			if (text.Length <= 1)
			{
				UnturnedLog.error(vehicleAsset.vehicleName + " name has a trailing space!");
				return false;
			}
			for (int i = 1; i < tier.table.Count; i++)
			{
				if (!(Assets.find(EAssetType.VEHICLE, tier.table[i].vehicle) as VehicleAsset).vehicleName.Contains(text))
				{
					return false;
				}
			}
			tier.name = text;
			return true;
		}

		// Token: 0x06003E0D RID: 15885 RVA: 0x0012D2C8 File Offset: 0x0012B4C8
		private static void exportItems(string path, Data spawnsData, ref ushort id, bool isLegacy)
		{
			for (int i = 0; i < LevelItems.tables.Count; i++)
			{
				ItemTable itemTable = LevelItems.tables[i];
				if (itemTable.tableID == 0)
				{
					itemTable.tableID = id;
					spawnsData.writeString(id.ToString(), Level.info.name + "_" + itemTable.name);
					Data data = new Data();
					data.writeString("Type", "Spawn");
					Data data2 = data;
					string key = "ID";
					ushort num = id;
					id = num + 1;
					data2.writeUInt16(key, num);
					if (ReadWrite.fileExists(string.Concat(new string[]
					{
						"/Bundles/Spawns/Items/",
						itemTable.name,
						"/",
						itemTable.name,
						".dat"
					}), false, true))
					{
						Data data3 = ReadWrite.readData(string.Concat(new string[]
						{
							"/Bundles/Spawns/Items/",
							itemTable.name,
							"/",
							itemTable.name,
							".dat"
						}), false, true);
						data.writeInt32("Tables", 1);
						data.writeUInt16("Table_0_Spawn_ID", data3.readUInt16("ID", 0));
						data.writeInt32("Table_0_Weight", 100);
					}
					else
					{
						data.writeInt32("Tables", 1);
						data.writeUInt16("Table_0_Spawn_ID", id);
						data.writeInt32("Table_0_Weight", 100);
						spawnsData.writeString(id.ToString(), itemTable.name);
						Data data4 = new Data();
						data4.writeString("Type", "Spawn");
						Data data5 = data4;
						string key2 = "ID";
						num = id;
						id = num + 1;
						data5.writeUInt16(key2, num);
						if (isLegacy)
						{
							if (itemTable.tiers.Count > 1)
							{
								float num2 = float.MaxValue;
								for (int j = 0; j < itemTable.tiers.Count; j++)
								{
									ItemTier itemTier = itemTable.tiers[j];
									if (itemTier.chance < num2)
									{
										num2 = itemTier.chance;
									}
								}
								int num3 = Mathf.CeilToInt(10f / num2);
								data4.writeInt32("Tables", itemTable.tiers.Count);
								for (int k = 0; k < itemTable.tiers.Count; k++)
								{
									ItemTier itemTier2 = itemTable.tiers[k];
									bool flag = SpawnTableTool.isVariantItemTier(itemTier2);
									if (flag && ReadWrite.fileExists(string.Concat(new string[]
									{
										"/Bundles/Spawns/Items/",
										itemTier2.name,
										"/",
										itemTier2.name,
										".dat"
									}), false, true))
									{
										Data data6 = ReadWrite.readData(string.Concat(new string[]
										{
											"/Bundles/Spawns/Items/",
											itemTier2.name,
											"/",
											itemTier2.name,
											".dat"
										}), false, true);
										data4.writeUInt16("Table_" + k.ToString() + "_Spawn_ID", data6.readUInt16("ID", 0));
										data4.writeInt32("Table_" + k.ToString() + "_Weight", (int)(itemTier2.chance * (float)num3));
									}
									else if (flag && ReadWrite.fileExists(string.Concat(new string[]
									{
										path,
										"/Items/",
										itemTier2.name,
										"/",
										itemTier2.name,
										".dat"
									}), false, false))
									{
										Data data7 = ReadWrite.readData(string.Concat(new string[]
										{
											path,
											"/Items/",
											itemTier2.name,
											"/",
											itemTier2.name,
											".dat"
										}), false, false);
										data4.writeUInt16("Table_" + k.ToString() + "_Spawn_ID", data7.readUInt16("ID", 0));
										data4.writeInt32("Table_" + k.ToString() + "_Weight", (int)(itemTier2.chance * (float)num3));
									}
									else
									{
										data4.writeUInt16("Table_" + k.ToString() + "_Spawn_ID", id);
										data4.writeInt32("Table_" + k.ToString() + "_Weight", (int)(itemTier2.chance * (float)num3));
										if (flag)
										{
											spawnsData.writeString(id.ToString(), itemTier2.name);
										}
										else
										{
											spawnsData.writeString(id.ToString(), itemTable.name + "_" + itemTier2.name);
										}
										Data data8 = new Data();
										data8.writeString("Type", "Spawn");
										Data data9 = data8;
										string key3 = "ID";
										num = id;
										id = num + 1;
										data9.writeUInt16(key3, num);
										data8.writeInt32("Tables", itemTier2.table.Count);
										for (int l = 0; l < itemTier2.table.Count; l++)
										{
											ItemSpawn itemSpawn = itemTier2.table[l];
											data8.writeUInt16("Table_" + l.ToString() + "_Asset_ID", itemSpawn.item);
											data8.writeInt32("Table_" + l.ToString() + "_Weight", 10);
										}
										if (flag)
										{
											ReadWrite.writeData(string.Concat(new string[]
											{
												path,
												"/Items/",
												itemTier2.name,
												"/",
												itemTier2.name,
												".dat"
											}), false, false, data8);
										}
										else
										{
											ReadWrite.writeData(string.Concat(new string[]
											{
												path,
												"/Items/",
												itemTable.name,
												"_",
												itemTier2.name,
												"/",
												itemTable.name,
												"_",
												itemTier2.name,
												".dat"
											}), false, false, data8);
										}
									}
								}
							}
							else
							{
								ItemTier itemTier3 = itemTable.tiers[0];
								data4.writeInt32("Tables", itemTier3.table.Count);
								for (int m = 0; m < itemTier3.table.Count; m++)
								{
									ItemSpawn itemSpawn2 = itemTier3.table[m];
									data4.writeUInt16("Table_" + m.ToString() + "_Asset_ID", itemSpawn2.item);
									data4.writeInt32("Table_" + m.ToString() + "_Weight", 10);
								}
							}
						}
						ReadWrite.writeData(string.Concat(new string[]
						{
							path,
							"/Items/",
							itemTable.name,
							"/",
							itemTable.name,
							".dat"
						}), false, false, data4);
					}
					ReadWrite.writeData(string.Concat(new string[]
					{
						path,
						"/Items/",
						Level.info.name,
						"_",
						itemTable.name,
						"/",
						Level.info.name,
						"_",
						itemTable.name,
						".dat"
					}), false, false, data);
				}
			}
		}

		// Token: 0x06003E0E RID: 15886 RVA: 0x0012DA30 File Offset: 0x0012BC30
		private static void exportVehicles(string path, Data spawnsData, ref ushort id, bool isLegacy)
		{
			for (int i = 0; i < LevelVehicles.tables.Count; i++)
			{
				VehicleTable vehicleTable = LevelVehicles.tables[i];
				if (vehicleTable.tableID == 0)
				{
					vehicleTable.tableID = id;
					spawnsData.writeString(id.ToString(), Level.info.name + "_" + vehicleTable.name);
					Data data = new Data();
					data.writeString("Type", "Spawn");
					Data data2 = data;
					string key = "ID";
					ushort num = id;
					id = num + 1;
					data2.writeUInt16(key, num);
					if (ReadWrite.fileExists(string.Concat(new string[]
					{
						"/Bundles/Spawns/Vehicles/",
						vehicleTable.name,
						"/",
						vehicleTable.name,
						".dat"
					}), false, true))
					{
						Data data3 = ReadWrite.readData(string.Concat(new string[]
						{
							"/Bundles/Spawns/Vehicles/",
							vehicleTable.name,
							"/",
							vehicleTable.name,
							".dat"
						}), false, true);
						data.writeInt32("Tables", 1);
						data.writeUInt16("Table_0_Spawn_ID", data3.readUInt16("ID", 0));
						data.writeInt32("Table_0_Weight", 100);
					}
					else
					{
						data.writeInt32("Tables", 1);
						data.writeUInt16("Table_0_Spawn_ID", id);
						data.writeInt32("Table_0_Weight", 100);
						spawnsData.writeString(id.ToString(), vehicleTable.name);
						Data data4 = new Data();
						data4.writeString("Type", "Spawn");
						Data data5 = data4;
						string key2 = "ID";
						num = id;
						id = num + 1;
						data5.writeUInt16(key2, num);
						if (isLegacy)
						{
							if (vehicleTable.tiers.Count > 1)
							{
								float num2 = float.MaxValue;
								for (int j = 0; j < vehicleTable.tiers.Count; j++)
								{
									VehicleTier vehicleTier = vehicleTable.tiers[j];
									if (vehicleTier.chance < num2)
									{
										num2 = vehicleTier.chance;
									}
								}
								int num3 = Mathf.CeilToInt(10f / num2);
								data4.writeInt32("Tables", vehicleTable.tiers.Count);
								for (int k = 0; k < vehicleTable.tiers.Count; k++)
								{
									VehicleTier vehicleTier2 = vehicleTable.tiers[k];
									bool flag = SpawnTableTool.isVariantVehicleTier(vehicleTier2);
									if (flag && ReadWrite.fileExists(string.Concat(new string[]
									{
										"/Bundles/Spawns/Vehicles/",
										vehicleTier2.name,
										"/",
										vehicleTier2.name,
										".dat"
									}), false, true))
									{
										Data data6 = ReadWrite.readData(string.Concat(new string[]
										{
											"/Bundles/Spawns/Vehicles/",
											vehicleTier2.name,
											"/",
											vehicleTier2.name,
											".dat"
										}), false, true);
										data4.writeUInt16("Table_" + k.ToString() + "_Spawn_ID", data6.readUInt16("ID", 0));
										data4.writeInt32("Table_" + k.ToString() + "_Weight", (int)(vehicleTier2.chance * (float)num3));
									}
									else if (flag && ReadWrite.fileExists(string.Concat(new string[]
									{
										path,
										"/Vehicles/",
										vehicleTier2.name,
										"/",
										vehicleTier2.name,
										".dat"
									}), false, false))
									{
										Data data7 = ReadWrite.readData(string.Concat(new string[]
										{
											path,
											"/Vehicles/",
											vehicleTier2.name,
											"/",
											vehicleTier2.name,
											".dat"
										}), false, false);
										data4.writeUInt16("Table_" + k.ToString() + "_Spawn_ID", data7.readUInt16("ID", 0));
										data4.writeInt32("Table_" + k.ToString() + "_Weight", (int)(vehicleTier2.chance * (float)num3));
									}
									else
									{
										data4.writeUInt16("Table_" + k.ToString() + "_Spawn_ID", id);
										data4.writeInt32("Table_" + k.ToString() + "_Weight", (int)(vehicleTier2.chance * (float)num3));
										if (flag)
										{
											spawnsData.writeString(id.ToString(), vehicleTier2.name);
										}
										else
										{
											spawnsData.writeString(id.ToString(), vehicleTable.name + "_" + vehicleTier2.name);
										}
										Data data8 = new Data();
										data8.writeString("Type", "Spawn");
										Data data9 = data8;
										string key3 = "ID";
										num = id;
										id = num + 1;
										data9.writeUInt16(key3, num);
										data8.writeInt32("Tables", vehicleTier2.table.Count);
										for (int l = 0; l < vehicleTier2.table.Count; l++)
										{
											VehicleSpawn vehicleSpawn = vehicleTier2.table[l];
											data8.writeUInt16("Table_" + l.ToString() + "_Asset_ID", vehicleSpawn.vehicle);
											data8.writeInt32("Table_" + l.ToString() + "_Weight", 10);
										}
										if (flag)
										{
											ReadWrite.writeData(string.Concat(new string[]
											{
												path,
												"/Vehicles/",
												vehicleTier2.name,
												"/",
												vehicleTier2.name,
												".dat"
											}), false, false, data8);
										}
										else
										{
											ReadWrite.writeData(string.Concat(new string[]
											{
												path,
												"/Vehicles/",
												vehicleTable.name,
												"_",
												vehicleTier2.name,
												"/",
												vehicleTable.name,
												"_",
												vehicleTier2.name,
												".dat"
											}), false, false, data8);
										}
									}
								}
							}
							else
							{
								VehicleTier vehicleTier3 = vehicleTable.tiers[0];
								data4.writeInt32("Tables", vehicleTier3.table.Count);
								for (int m = 0; m < vehicleTier3.table.Count; m++)
								{
									VehicleSpawn vehicleSpawn2 = vehicleTier3.table[m];
									data4.writeUInt16("Table_" + m.ToString() + "_Asset_ID", vehicleSpawn2.vehicle);
									data4.writeInt32("Table_" + m.ToString() + "_Weight", 10);
								}
							}
						}
						ReadWrite.writeData(string.Concat(new string[]
						{
							path,
							"/Vehicles/",
							vehicleTable.name,
							"/",
							vehicleTable.name,
							".dat"
						}), false, false, data4);
					}
					ReadWrite.writeData(string.Concat(new string[]
					{
						path,
						"/Vehicles/",
						Level.info.name,
						"_",
						vehicleTable.name,
						"/",
						Level.info.name,
						"_",
						vehicleTable.name,
						".dat"
					}), false, false, data);
				}
			}
		}

		// Token: 0x06003E0F RID: 15887 RVA: 0x0012E198 File Offset: 0x0012C398
		private static void exportZombies(string path, Data spawnsData, ref ushort id, bool isLegacy)
		{
			for (int i = 0; i < LevelZombies.tables.Count; i++)
			{
				ZombieTable zombieTable = LevelZombies.tables[i];
				if (zombieTable.lootID == 0 && (int)zombieTable.lootIndex < LevelItems.tables.Count)
				{
					zombieTable.lootID = LevelItems.tables[(int)zombieTable.lootIndex].tableID;
				}
			}
		}

		// Token: 0x06003E10 RID: 15888 RVA: 0x0012E1FC File Offset: 0x0012C3FC
		private static void exportAnimals(string path, Data spawnsData, ref ushort id, bool isLegacy)
		{
			for (int i = 0; i < LevelAnimals.tables.Count; i++)
			{
				AnimalTable animalTable = LevelAnimals.tables[i];
				if (animalTable.tableID == 0)
				{
					animalTable.tableID = id;
					spawnsData.writeString(id.ToString(), Level.info.name + "_" + animalTable.name);
					Data data = new Data();
					data.writeString("Type", "Spawn");
					Data data2 = data;
					string key = "ID";
					ushort num = id;
					id = num + 1;
					data2.writeUInt16(key, num);
					if (ReadWrite.fileExists(string.Concat(new string[]
					{
						"/Bundles/Spawns/Animals/",
						animalTable.name,
						"/",
						animalTable.name,
						".dat"
					}), false, true))
					{
						Data data3 = ReadWrite.readData(string.Concat(new string[]
						{
							"/Bundles/Spawns/Animals/",
							animalTable.name,
							"/",
							animalTable.name,
							".dat"
						}), false, true);
						data.writeInt32("Tables", 1);
						data.writeUInt16("Table_0_Spawn_ID", data3.readUInt16("ID", 0));
						data.writeInt32("Table_0_Weight", 100);
					}
					else
					{
						data.writeInt32("Tables", 1);
						data.writeUInt16("Table_0_Spawn_ID", id);
						data.writeInt32("Table_0_Weight", 100);
						spawnsData.writeString(id.ToString(), animalTable.name);
						Data data4 = new Data();
						data4.writeString("Type", "Spawn");
						Data data5 = data4;
						string key2 = "ID";
						num = id;
						id = num + 1;
						data5.writeUInt16(key2, num);
						if (isLegacy)
						{
							if (animalTable.tiers.Count > 1)
							{
								float num2 = float.MaxValue;
								for (int j = 0; j < animalTable.tiers.Count; j++)
								{
									AnimalTier animalTier = animalTable.tiers[j];
									if (animalTier.chance < num2)
									{
										num2 = animalTier.chance;
									}
								}
								int num3 = Mathf.CeilToInt(10f / num2);
								data4.writeInt32("Tables", animalTable.tiers.Count);
								for (int k = 0; k < animalTable.tiers.Count; k++)
								{
									AnimalTier animalTier2 = animalTable.tiers[k];
									data4.writeUInt16("Table_" + k.ToString() + "_Spawn_ID", id);
									data4.writeInt32("Table_" + k.ToString() + "_Weight", (int)(animalTier2.chance * (float)num3));
									spawnsData.writeString(id.ToString(), animalTable.name + "_" + animalTier2.name);
									Data data6 = new Data();
									data6.writeString("Type", "Spawn");
									Data data7 = data6;
									string key3 = "ID";
									num = id;
									id = num + 1;
									data7.writeUInt16(key3, num);
									data6.writeInt32("Tables", animalTier2.table.Count);
									for (int l = 0; l < animalTier2.table.Count; l++)
									{
										AnimalSpawn animalSpawn = animalTier2.table[l];
										data6.writeUInt16("Table_" + l.ToString() + "_Asset_ID", animalSpawn.animal);
										data6.writeInt32("Table_" + l.ToString() + "_Weight", 10);
									}
									ReadWrite.writeData(string.Concat(new string[]
									{
										path,
										"/Animals/",
										animalTable.name,
										"_",
										animalTier2.name,
										"/",
										animalTable.name,
										"_",
										animalTier2.name,
										".dat"
									}), false, false, data6);
								}
							}
							else
							{
								AnimalTier animalTier3 = animalTable.tiers[0];
								data4.writeInt32("Tables", animalTier3.table.Count);
								for (int m = 0; m < animalTier3.table.Count; m++)
								{
									AnimalSpawn animalSpawn2 = animalTier3.table[m];
									data4.writeUInt16("Table_" + m.ToString() + "_Asset_ID", animalSpawn2.animal);
									data4.writeInt32("Table_" + m.ToString() + "_Weight", 10);
								}
							}
						}
						ReadWrite.writeData(string.Concat(new string[]
						{
							path,
							"/Animals/",
							animalTable.name,
							"/",
							animalTable.name,
							".dat"
						}), false, false, data4);
					}
					ReadWrite.writeData(string.Concat(new string[]
					{
						path,
						"/Animals/",
						Level.info.name,
						"_",
						animalTable.name,
						"/",
						Level.info.name,
						"_",
						animalTable.name,
						".dat"
					}), false, false, data);
				}
			}
		}

		// Token: 0x06003E11 RID: 15889 RVA: 0x0012E72C File Offset: 0x0012C92C
		public static void export(ushort id, bool isLegacy)
		{
			string text = Level.info.path;
			if (isLegacy)
			{
				text += "/Exported_Legacy_Spawn_Tables";
			}
			else
			{
				text += "/Exported_Proxy_Spawn_Tables";
			}
			if (ReadWrite.folderExists(text, false))
			{
				ReadWrite.deleteFolder(text, false);
			}
			Data data = new Data();
			data.writeString("ID", "Spawn");
			SpawnTableTool.exportItems(text, data, ref id, isLegacy);
			SpawnTableTool.exportVehicles(text, data, ref id, isLegacy);
			SpawnTableTool.exportZombies(text, data, ref id, isLegacy);
			SpawnTableTool.exportAnimals(text, data, ref id, isLegacy);
			data.isCSV = true;
			ReadWrite.writeData(text + "/IDs.csv", false, false, data);
		}

		// Token: 0x06003E12 RID: 15890 RVA: 0x0012E7CC File Offset: 0x0012C9CC
		public static void LogAllSpawnTables()
		{
			List<SpawnAsset> list = new List<SpawnAsset>(1000);
			Assets.find<SpawnAsset>(list);
			UnturnedLog.info(string.Format("Dumping {0} spawn tables:", list.Count));
			for (int i = 0; i < list.Count; i++)
			{
				SpawnAsset spawnAsset = list[i];
				if (spawnAsset == null)
				{
					UnturnedLog.error("null entry in spawnAssets list???");
				}
				else if (spawnAsset.tables == null || spawnAsset.tables.Count < 1)
				{
					UnturnedLog.info(string.Format("[{0} of {1}] {2} is empty", i + 1, list.Count, spawnAsset.name));
				}
				else
				{
					UnturnedLog.info(string.Format("[{0} of {1}] {2} has {3} children:", new object[]
					{
						i + 1,
						list.Count,
						spawnAsset.name,
						spawnAsset.tables.Count
					}));
					for (int j = 0; j < spawnAsset.tables.Count; j++)
					{
						SpawnTable spawnTable = spawnAsset.tables[j];
						string text;
						if (spawnTable.legacySpawnId != 0)
						{
							SpawnAsset spawnAsset2 = Assets.find(EAssetType.SPAWN, spawnTable.legacySpawnId) as SpawnAsset;
							text = (((spawnAsset2 != null) ? spawnAsset2.name : null) ?? string.Format("Unknown ID {0}", spawnTable.legacySpawnId)) + " (Spawn)";
						}
						else if (spawnTable.legacyAssetId != 0)
						{
							ItemAsset itemAsset = Assets.find(EAssetType.ITEM, spawnTable.legacyAssetId) as ItemAsset;
							VehicleAsset vehicleAsset = VehicleTool.FindVehicleByLegacyIdAndHandleRedirects(spawnTable.legacyAssetId);
							AnimalAsset animalAsset = Assets.find(EAssetType.ANIMAL, spawnTable.legacyAssetId) as AnimalAsset;
							string text2 = ((itemAsset != null) ? itemAsset.FriendlyName : null) ?? string.Format("Unknown ID {0}", spawnTable.legacyAssetId);
							string text3 = ((vehicleAsset != null) ? vehicleAsset.FriendlyName : null) ?? string.Format("Unknown ID {0}", spawnTable.legacyAssetId);
							string text4 = ((animalAsset != null) ? animalAsset.FriendlyName : null) ?? string.Format("Unknown ID {0}", spawnTable.legacyAssetId);
							text = string.Concat(new string[]
							{
								text2,
								" (Item) or ",
								text3,
								" (Vehicle) or ",
								text4,
								" (Animal) depending on context"
							});
						}
						else if (!GuidExtension.IsEmpty(spawnTable.targetGuid))
						{
							Asset asset = Assets.find(spawnTable.targetGuid);
							if (asset != null)
							{
								text = asset.FriendlyName + " (" + asset.GetTypeFriendlyName() + ")";
							}
							else
							{
								text = string.Format("Unknown GUID {0}", spawnTable.targetGuid);
							}
						}
						else
						{
							text = "Empty";
						}
						float num = spawnTable.normalizedWeight;
						if (j > 0)
						{
							num -= spawnAsset.tables[j - 1].normalizedWeight;
						}
						UnturnedLog.info(string.Format("[{0} of {1}][{2} of {3}] {4:P} {5}", new object[]
						{
							i + 1,
							list.Count,
							j + 1,
							spawnAsset.tables.Count,
							num,
							text
						}));
					}
				}
			}
		}
	}
}
