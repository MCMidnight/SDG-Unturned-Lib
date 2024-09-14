using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x020006CE RID: 1742
	public static class FilterSettings
	{
		// Token: 0x140000D5 RID: 213
		// (add) Token: 0x06003A09 RID: 14857 RVA: 0x0010FB6C File Offset: 0x0010DD6C
		// (remove) Token: 0x06003A0A RID: 14858 RVA: 0x0010FBA0 File Offset: 0x0010DDA0
		public static event Action OnActiveFiltersModified;

		// Token: 0x140000D6 RID: 214
		// (add) Token: 0x06003A0B RID: 14859 RVA: 0x0010FBD4 File Offset: 0x0010DDD4
		// (remove) Token: 0x06003A0C RID: 14860 RVA: 0x0010FC08 File Offset: 0x0010DE08
		public static event Action OnActiveFiltersReplaced;

		// Token: 0x06003A0D RID: 14861 RVA: 0x0010FC3B File Offset: 0x0010DE3B
		public static void InvokeActiveFiltersReplaced()
		{
			Action onActiveFiltersReplaced = FilterSettings.OnActiveFiltersReplaced;
			if (onActiveFiltersReplaced == null)
			{
				return;
			}
			onActiveFiltersReplaced.Invoke();
		}

		// Token: 0x140000D7 RID: 215
		// (add) Token: 0x06003A0E RID: 14862 RVA: 0x0010FC4C File Offset: 0x0010DE4C
		// (remove) Token: 0x06003A0F RID: 14863 RVA: 0x0010FC80 File Offset: 0x0010DE80
		public static event Action OnCustomPresetsListChanged;

		// Token: 0x06003A10 RID: 14864 RVA: 0x0010FCB3 File Offset: 0x0010DEB3
		public static void InvokeCustomFiltersListChanged()
		{
			Action onCustomPresetsListChanged = FilterSettings.OnCustomPresetsListChanged;
			if (onCustomPresetsListChanged == null)
			{
				return;
			}
			onCustomPresetsListChanged.Invoke();
		}

		// Token: 0x06003A11 RID: 14865 RVA: 0x0010FCC4 File Offset: 0x0010DEC4
		public static int CreatePresetId()
		{
			int result = FilterSettings.nextCustomPresetId;
			FilterSettings.nextCustomPresetId++;
			return result;
		}

		// Token: 0x06003A12 RID: 14866 RVA: 0x0010FCD8 File Offset: 0x0010DED8
		public static void RemovePreset(int presetId)
		{
			for (int i = FilterSettings.customPresets.Count - 1; i >= 0; i--)
			{
				if (FilterSettings.customPresets[i].presetId == presetId)
				{
					FilterSettings.customPresets.RemoveAt(i);
				}
			}
		}

		// Token: 0x06003A13 RID: 14867 RVA: 0x0010FD1C File Offset: 0x0010DF1C
		public static void MarkActiveFilterModified()
		{
			if (FilterSettings.activeFilters.presetId != -1)
			{
				if (FilterSettings.activeFilters.presetId < -1)
				{
					if (FilterSettings.activeFilters.presetId == FilterSettings.defaultPresetInternet.presetId)
					{
						FilterSettings.activeFilters.presetName = MenuPlayUI.serverListUI.localization.format("List_Internet_Label");
					}
					else if (FilterSettings.activeFilters.presetId == FilterSettings.defaultPresetLAN.presetId)
					{
						FilterSettings.activeFilters.presetName = MenuPlayUI.serverListUI.localization.format("List_LAN_Label");
					}
					else if (FilterSettings.activeFilters.presetId == FilterSettings.defaultPresetHistory.presetId)
					{
						FilterSettings.activeFilters.presetName = MenuPlayUI.serverListUI.localization.format("List_History_Label");
					}
					else if (FilterSettings.activeFilters.presetId == FilterSettings.defaultPresetFavorites.presetId)
					{
						FilterSettings.activeFilters.presetName = MenuPlayUI.serverListUI.localization.format("List_Favorites_Label");
					}
					else if (FilterSettings.activeFilters.presetId == FilterSettings.defaultPresetFriends.presetId)
					{
						FilterSettings.activeFilters.presetName = MenuPlayUI.serverListUI.localization.format("List_Friends_Label");
					}
					else
					{
						UnturnedLog.warn(string.Format("Marking active filter modified unknown default preset ID ({0})", FilterSettings.activeFilters.presetId));
					}
				}
				if (!string.IsNullOrEmpty(FilterSettings.activeFilters.presetName))
				{
					FilterSettings.activeFilters.presetName = MenuPlayUI.serverListUI.localization.format("PresetName_Modified", FilterSettings.activeFilters.presetName);
				}
				FilterSettings.activeFilters.presetId = -1;
				Action onActiveFiltersModified = FilterSettings.OnActiveFiltersModified;
				if (onActiveFiltersModified == null)
				{
					return;
				}
				onActiveFiltersModified.Invoke();
			}
		}

		// Token: 0x06003A14 RID: 14868 RVA: 0x0010FED4 File Offset: 0x0010E0D4
		public static void load()
		{
			if (ReadWrite.fileExists("/Filters.dat", true))
			{
				Block block = ReadWrite.readBlock("/Filters.dat", true, 0);
				if (block != null)
				{
					byte b = block.readByte();
					if (b > 2)
					{
						if (b >= 20)
						{
							int num = block.readInt32();
							for (int i = 0; i < num; i++)
							{
								string text = block.readString();
								if (!string.IsNullOrEmpty(text))
								{
									FilterSettings.activeFilters.mapNames.Add(text);
								}
							}
						}
						else
						{
							string text2 = block.readString();
							if (!string.IsNullOrEmpty(text2))
							{
								FilterSettings.activeFilters.mapNames.Add(text2);
							}
						}
						if (b > 5)
						{
							FilterSettings.activeFilters.password = (EPassword)block.readByte();
							FilterSettings.activeFilters.workshop = (EWorkshop)block.readByte();
							if (b < 12)
							{
								FilterSettings.activeFilters.workshop = EWorkshop.ANY;
							}
						}
						else
						{
							block.readBoolean();
							block.readBoolean();
							FilterSettings.activeFilters.password = EPassword.NO;
							FilterSettings.activeFilters.workshop = EWorkshop.ANY;
						}
						if (b < 7)
						{
							FilterSettings.activeFilters.plugins = EPlugins.ANY;
						}
						else
						{
							FilterSettings.activeFilters.plugins = (EPlugins)block.readByte();
						}
						FilterSettings.activeFilters.attendance = (EAttendance)block.readByte();
						if (b >= 14)
						{
							FilterSettings.activeFilters.notFull = block.readBoolean();
						}
						else
						{
							FilterSettings.activeFilters.notFull = true;
						}
						FilterSettings.activeFilters.vacProtection = (EVACProtectionFilter)block.readByte();
						if (b > 10)
						{
							FilterSettings.activeFilters.battlEyeProtection = (EBattlEyeProtectionFilter)block.readByte();
						}
						else
						{
							FilterSettings.activeFilters.battlEyeProtection = EBattlEyeProtectionFilter.Secure;
						}
						FilterSettings.activeFilters.combat = (ECombat)block.readByte();
						if (b < 8)
						{
							FilterSettings.activeFilters.cheats = ECheats.NO;
						}
						else
						{
							FilterSettings.activeFilters.cheats = (ECheats)block.readByte();
						}
						if (b < 15)
						{
							block.readByte();
						}
						if (b > 3)
						{
							FilterSettings.activeFilters.camera = (ECameraMode)block.readByte();
						}
						else
						{
							FilterSettings.activeFilters.camera = ECameraMode.ANY;
						}
						if (b >= 13)
						{
							FilterSettings.activeFilters.monetization = (EServerMonetizationTag)block.readByte();
						}
						else
						{
							FilterSettings.activeFilters.monetization = EServerMonetizationTag.Any;
						}
						if (b >= 15)
						{
							FilterSettings.activeFilters.gold = (EServerListGoldFilter)block.readByte();
						}
						else
						{
							FilterSettings.activeFilters.gold = EServerListGoldFilter.Any;
						}
						if (b >= 16)
						{
							FilterSettings.activeFilters.serverName = block.readString();
						}
						else
						{
							FilterSettings.activeFilters.serverName = string.Empty;
						}
						if (b >= 17)
						{
							FilterSettings.activeFilters.listSource = (ESteamServerList)block.readByte();
							FilterSettings.activeFilters.presetName = block.readString();
							FilterSettings.activeFilters.presetId = block.readInt32();
						}
						else
						{
							FilterSettings.activeFilters.listSource = ESteamServerList.INTERNET;
							FilterSettings.activeFilters.presetName = string.Empty;
							FilterSettings.activeFilters.presetId = -1;
						}
						if (b >= 22)
						{
							FilterSettings.activeFilters.maxPing = block.readInt32();
							if (b < 24 && FilterSettings.activeFilters.maxPing == 200)
							{
								FilterSettings.activeFilters.maxPing = 300;
							}
						}
						else
						{
							FilterSettings.activeFilters.maxPing = 300;
						}
						if (b >= 17)
						{
							FilterSettings.nextCustomPresetId = block.readInt32();
							int num2 = block.readInt32();
							for (int j = 0; j < num2; j++)
							{
								ServerListFilters serverListFilters = new ServerListFilters();
								serverListFilters.Read(b, block);
								FilterSettings.customPresets.Add(serverListFilters);
							}
						}
						else
						{
							FilterSettings.nextCustomPresetId = 1;
						}
						if (b >= 18)
						{
							FilterSettings.columns.Read(b, block);
						}
						if (b >= 19)
						{
							FilterSettings.isColumnsEditorOpen = block.readBoolean();
							FilterSettings.isPresetsListOpen = block.readBoolean();
							FilterSettings.isQuickFiltersEditorOpen = block.readBoolean();
						}
						else
						{
							FilterSettings.isColumnsEditorOpen = false;
							FilterSettings.isPresetsListOpen = true;
							FilterSettings.isQuickFiltersEditorOpen = false;
						}
						if (b >= 21)
						{
							FilterSettings.isQuickFiltersVisibilityEditorOpen = block.readBoolean();
							FilterSettings.filterVisibility.Read(b, block);
							return;
						}
						FilterSettings.isQuickFiltersVisibilityEditorOpen = false;
						return;
					}
				}
			}
			FilterSettings.isPresetsListOpen = true;
		}

		// Token: 0x06003A15 RID: 14869 RVA: 0x00110284 File Offset: 0x0010E484
		public static void save()
		{
			Block block = new Block();
			block.writeByte(24);
			FilterSettings.activeFilters.Write(block);
			block.writeInt32(FilterSettings.nextCustomPresetId);
			block.writeInt32(FilterSettings.customPresets.Count);
			foreach (ServerListFilters serverListFilters in FilterSettings.customPresets)
			{
				serverListFilters.Write(block);
			}
			FilterSettings.columns.Write(block);
			block.writeBoolean(FilterSettings.isColumnsEditorOpen);
			block.writeBoolean(FilterSettings.isPresetsListOpen);
			block.writeBoolean(FilterSettings.isQuickFiltersEditorOpen);
			block.writeBoolean(FilterSettings.isQuickFiltersVisibilityEditorOpen);
			FilterSettings.filterVisibility.Write(block);
			ReadWrite.writeBlock("/Filters.dat", true, block);
		}

		// Token: 0x06003A16 RID: 14870 RVA: 0x00110358 File Offset: 0x0010E558
		static FilterSettings()
		{
			FilterSettings.defaultPresetInternet.presetId = -2;
			FilterSettings.defaultPresetLAN.presetId = -3;
			FilterSettings.defaultPresetLAN.listSource = ESteamServerList.LAN;
			FilterSettings.defaultPresetLAN.password = EPassword.ANY;
			FilterSettings.defaultPresetLAN.vacProtection = EVACProtectionFilter.Any;
			FilterSettings.defaultPresetLAN.battlEyeProtection = EBattlEyeProtectionFilter.Any;
			FilterSettings.defaultPresetLAN.cheats = ECheats.ANY;
			FilterSettings.defaultPresetLAN.attendance = EAttendance.Any;
			FilterSettings.defaultPresetLAN.notFull = false;
			FilterSettings.defaultPresetLAN.maxPing = 0;
			FilterSettings.defaultPresetHistory.presetId = -4;
			FilterSettings.defaultPresetHistory.listSource = ESteamServerList.HISTORY;
			FilterSettings.defaultPresetHistory.password = EPassword.ANY;
			FilterSettings.defaultPresetHistory.vacProtection = EVACProtectionFilter.Any;
			FilterSettings.defaultPresetHistory.battlEyeProtection = EBattlEyeProtectionFilter.Any;
			FilterSettings.defaultPresetHistory.cheats = ECheats.ANY;
			FilterSettings.defaultPresetHistory.attendance = EAttendance.Any;
			FilterSettings.defaultPresetHistory.notFull = false;
			FilterSettings.defaultPresetHistory.maxPing = 0;
			FilterSettings.defaultPresetFavorites.presetId = -5;
			FilterSettings.defaultPresetFavorites.listSource = ESteamServerList.FAVORITES;
			FilterSettings.defaultPresetFavorites.password = EPassword.ANY;
			FilterSettings.defaultPresetFavorites.vacProtection = EVACProtectionFilter.Any;
			FilterSettings.defaultPresetFavorites.battlEyeProtection = EBattlEyeProtectionFilter.Any;
			FilterSettings.defaultPresetFavorites.cheats = ECheats.ANY;
			FilterSettings.defaultPresetFavorites.attendance = EAttendance.Any;
			FilterSettings.defaultPresetFavorites.notFull = false;
			FilterSettings.defaultPresetFavorites.maxPing = 0;
			FilterSettings.defaultPresetFriends.presetId = -6;
			FilterSettings.defaultPresetFriends.listSource = ESteamServerList.FRIENDS;
			FilterSettings.defaultPresetFriends.password = EPassword.ANY;
			FilterSettings.defaultPresetFriends.vacProtection = EVACProtectionFilter.Any;
			FilterSettings.defaultPresetFriends.battlEyeProtection = EBattlEyeProtectionFilter.Any;
			FilterSettings.defaultPresetFriends.cheats = ECheats.ANY;
			FilterSettings.defaultPresetFriends.attendance = EAttendance.Any;
			FilterSettings.defaultPresetFriends.notFull = false;
			FilterSettings.defaultPresetFriends.maxPing = 0;
		}

		/// <summary>
		/// Version before named version constants were introduced. (2023-11-13)
		/// </summary>
		// Token: 0x040022E7 RID: 8935
		public const byte SAVEDATA_VERSION_INITIAL = 14;

		// Token: 0x040022E8 RID: 8936
		public const byte SAVEDATA_VERSION_ADDED_GOLD_FILTER = 15;

		// Token: 0x040022E9 RID: 8937
		public const byte SAVEDATA_VERSION_MOVED_SERVER_NAME_FILTER = 16;

		// Token: 0x040022EA RID: 8938
		public const byte SAVEDATA_VERSION_ADDED_PRESETS = 17;

		// Token: 0x040022EB RID: 8939
		public const byte SAVEDATA_VERSION_SAVE_COLUMNS = 18;

		// Token: 0x040022EC RID: 8940
		public const byte SAVEDATA_VERSION_SAVE_SUBMENUS_OPEN = 19;

		// Token: 0x040022ED RID: 8941
		public const byte SAVEDATA_VERSION_MULTIPLE_MAPS = 20;

		// Token: 0x040022EE RID: 8942
		public const byte SAVEDATA_VERSION_FILTER_VISIBILITY = 21;

		// Token: 0x040022EF RID: 8943
		public const byte SAVEDATA_VERSION_MAX_PING = 22;

		// Token: 0x040022F0 RID: 8944
		public const byte SAVEDATA_VERSION_ADDED_FULLNESS_COLUMN = 23;

		// Token: 0x040022F1 RID: 8945
		public const byte SAVEDATA_VERSION_INCREASED_DEFAULT_MAX_PING = 24;

		// Token: 0x040022F2 RID: 8946
		private const byte SAVEDATA_VERSION_NEWEST = 24;

		// Token: 0x040022F3 RID: 8947
		public static readonly byte SAVEDATA_VERSION = 24;

		// Token: 0x040022F4 RID: 8948
		public const int DEFAULT_MAX_PING = 300;

		// Token: 0x040022F5 RID: 8949
		public static ServerListFilters activeFilters = new ServerListFilters();

		// Token: 0x040022F6 RID: 8950
		public static bool isColumnsEditorOpen;

		// Token: 0x040022F7 RID: 8951
		public static bool isPresetsListOpen;

		// Token: 0x040022F8 RID: 8952
		public static bool isQuickFiltersEditorOpen;

		// Token: 0x040022F9 RID: 8953
		public static bool isQuickFiltersVisibilityEditorOpen;

		// Token: 0x040022FD RID: 8957
		public static List<ServerListFilters> customPresets = new List<ServerListFilters>();

		// Token: 0x040022FE RID: 8958
		private static int nextCustomPresetId = 1;

		// Token: 0x040022FF RID: 8959
		public static ServerListFilters defaultPresetInternet = new ServerListFilters();

		// Token: 0x04002300 RID: 8960
		public static ServerListFilters defaultPresetLAN = new ServerListFilters();

		// Token: 0x04002301 RID: 8961
		public static ServerListFilters defaultPresetHistory = new ServerListFilters();

		// Token: 0x04002302 RID: 8962
		public static ServerListFilters defaultPresetFavorites = new ServerListFilters();

		// Token: 0x04002303 RID: 8963
		public static ServerListFilters defaultPresetFriends = new ServerListFilters();

		// Token: 0x04002304 RID: 8964
		public static FilterSettings.ServerBrowserColumns columns = new FilterSettings.ServerBrowserColumns();

		// Token: 0x04002305 RID: 8965
		public static FilterSettings.ServerBrowserFilterVisibility filterVisibility = new FilterSettings.ServerBrowserFilterVisibility();

		// Token: 0x020009EB RID: 2539
		public class ServerBrowserColumns
		{
			// Token: 0x06004CE3 RID: 19683 RVA: 0x001B7FC0 File Offset: 0x001B61C0
			public void Read(byte version, Block block)
			{
				this.map = block.readBoolean();
				this.players = block.readBoolean();
				this.maxPlayers = block.readBoolean();
				this.ping = block.readBoolean();
				this.anticheat = block.readBoolean();
				this.perspective = block.readBoolean();
				this.combat = block.readBoolean();
				this.password = block.readBoolean();
				this.workshop = block.readBoolean();
				this.gold = block.readBoolean();
				this.cheats = block.readBoolean();
				this.monetization = block.readBoolean();
				this.plugins = block.readBoolean();
				if (version >= 23)
				{
					this.fullnessPercentage = block.readBoolean();
					return;
				}
				this.fullnessPercentage = false;
			}

			// Token: 0x06004CE4 RID: 19684 RVA: 0x001B8084 File Offset: 0x001B6284
			public void Write(Block block)
			{
				block.writeBoolean(this.map);
				block.writeBoolean(this.players);
				block.writeBoolean(this.maxPlayers);
				block.writeBoolean(this.ping);
				block.writeBoolean(this.anticheat);
				block.writeBoolean(this.perspective);
				block.writeBoolean(this.combat);
				block.writeBoolean(this.password);
				block.writeBoolean(this.workshop);
				block.writeBoolean(this.gold);
				block.writeBoolean(this.cheats);
				block.writeBoolean(this.monetization);
				block.writeBoolean(this.plugins);
				block.writeBoolean(this.fullnessPercentage);
			}

			// Token: 0x04003498 RID: 13464
			public bool map = true;

			// Token: 0x04003499 RID: 13465
			public bool players = true;

			// Token: 0x0400349A RID: 13466
			public bool maxPlayers;

			// Token: 0x0400349B RID: 13467
			public bool ping = true;

			// Token: 0x0400349C RID: 13468
			public bool anticheat;

			// Token: 0x0400349D RID: 13469
			public bool perspective;

			// Token: 0x0400349E RID: 13470
			public bool combat;

			// Token: 0x0400349F RID: 13471
			public bool password;

			// Token: 0x040034A0 RID: 13472
			public bool workshop;

			// Token: 0x040034A1 RID: 13473
			public bool gold;

			// Token: 0x040034A2 RID: 13474
			public bool cheats;

			// Token: 0x040034A3 RID: 13475
			public bool monetization;

			// Token: 0x040034A4 RID: 13476
			public bool plugins;

			/// <summary>
			/// % Full
			/// </summary>
			// Token: 0x040034A5 RID: 13477
			public bool fullnessPercentage;
		}

		// Token: 0x020009EC RID: 2540
		public class ServerBrowserFilterVisibility
		{
			// Token: 0x06004CE6 RID: 19686 RVA: 0x001B8158 File Offset: 0x001B6358
			public void Read(byte version, Block block)
			{
				this.name = block.readBoolean();
				this.map = block.readBoolean();
				this.password = block.readBoolean();
				this.workshop = block.readBoolean();
				this.plugins = block.readBoolean();
				this.attendance = block.readBoolean();
				this.notFull = block.readBoolean();
				this.vacProtection = block.readBoolean();
				this.battlEyeProtection = block.readBoolean();
				this.combat = block.readBoolean();
				this.cheats = block.readBoolean();
				this.camera = block.readBoolean();
				this.monetization = block.readBoolean();
				this.gold = block.readBoolean();
				this.listSource = block.readBoolean();
				if (version >= 22)
				{
					this.maxPing = block.readBoolean();
					return;
				}
				this.maxPing = false;
			}

			// Token: 0x06004CE7 RID: 19687 RVA: 0x001B8234 File Offset: 0x001B6434
			public void Write(Block block)
			{
				block.writeBoolean(this.name);
				block.writeBoolean(this.map);
				block.writeBoolean(this.password);
				block.writeBoolean(this.workshop);
				block.writeBoolean(this.plugins);
				block.writeBoolean(this.attendance);
				block.writeBoolean(this.notFull);
				block.writeBoolean(this.vacProtection);
				block.writeBoolean(this.battlEyeProtection);
				block.writeBoolean(this.combat);
				block.writeBoolean(this.cheats);
				block.writeBoolean(this.camera);
				block.writeBoolean(this.monetization);
				block.writeBoolean(this.gold);
				block.writeBoolean(this.listSource);
				block.writeBoolean(this.maxPing);
			}

			// Token: 0x040034A6 RID: 13478
			public bool name = true;

			// Token: 0x040034A7 RID: 13479
			public bool map = true;

			// Token: 0x040034A8 RID: 13480
			public bool password;

			// Token: 0x040034A9 RID: 13481
			public bool workshop;

			// Token: 0x040034AA RID: 13482
			public bool plugins;

			// Token: 0x040034AB RID: 13483
			public bool attendance;

			// Token: 0x040034AC RID: 13484
			public bool notFull;

			// Token: 0x040034AD RID: 13485
			public bool vacProtection;

			// Token: 0x040034AE RID: 13486
			public bool battlEyeProtection;

			// Token: 0x040034AF RID: 13487
			public bool combat = true;

			// Token: 0x040034B0 RID: 13488
			public bool cheats;

			// Token: 0x040034B1 RID: 13489
			public bool camera = true;

			// Token: 0x040034B2 RID: 13490
			public bool monetization;

			// Token: 0x040034B3 RID: 13491
			public bool gold;

			// Token: 0x040034B4 RID: 13492
			public bool listSource = true;

			// Token: 0x040034B5 RID: 13493
			public bool maxPing;
		}
	}
}
