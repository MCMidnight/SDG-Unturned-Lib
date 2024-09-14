using System;
using SDG.Provider;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007A3 RID: 1955
	public class MenuPlayServersUI : SleekFullscreenBox
	{
		// Token: 0x06004117 RID: 16663 RVA: 0x001572E0 File Offset: 0x001554E0
		public void open(bool shouldRefresh)
		{
			if (this.active)
			{
				return;
			}
			this.active = true;
			this.SynchronizeFilterButtons();
			if (FilterSettings.activeFilters.presetId == 0)
			{
				FilterSettings.activeFilters.CopyFrom(FilterSettings.defaultPresetInternet);
				FilterSettings.activeFilters.presetName = this.localization.format("DefaultPreset_Internet_Label");
				FilterSettings.InvokeActiveFiltersReplaced();
			}
			else if (shouldRefresh)
			{
				this.CancelAndRefresh();
			}
			base.AnimateIntoView();
		}

		// Token: 0x06004118 RID: 16664 RVA: 0x0015734E File Offset: 0x0015554E
		public void close()
		{
			if (!this.active)
			{
				return;
			}
			this.active = false;
			MenuSettings.save();
			base.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x06004119 RID: 16665 RVA: 0x00157375 File Offset: 0x00155575
		private void onClickedServer(SleekServer server, SteamServerAdvertisement info)
		{
			if (info.isPro && !Provider.isPro)
			{
				return;
			}
			MenuSettings.save();
			MenuPlayServerInfoUI.open(info, string.Empty, MenuPlayServerInfoUI.EServerInfoOpenContext.SERVERS);
			this.close();
		}

		// Token: 0x0600411A RID: 16666 RVA: 0x0015739E File Offset: 0x0015559E
		private void onMasterServerAdded(int insert, SteamServerAdvertisement info)
		{
			this.serverBox.NotifyDataChanged();
		}

		// Token: 0x0600411B RID: 16667 RVA: 0x001573AB File Offset: 0x001555AB
		private void onMasterServerRemoved()
		{
			this.infoBox.IsVisible = false;
			this.serverBox.NotifyDataChanged();
		}

		// Token: 0x0600411C RID: 16668 RVA: 0x001573C4 File Offset: 0x001555C4
		private void onMasterServerResorted()
		{
			this.infoBox.IsVisible = false;
			this.serverBox.NotifyDataChanged();
		}

		// Token: 0x0600411D RID: 16669 RVA: 0x001573DD File Offset: 0x001555DD
		private void onMasterServerRefreshed(EMatchMakingServerResponse response)
		{
			this.SetIsRefreshing(false);
			if (Provider.provider.matchmakingService.serverList.Count == 0)
			{
				this.infoBox.IsVisible = true;
			}
		}

		// Token: 0x0600411E RID: 16670 RVA: 0x00157408 File Offset: 0x00155608
		private void CancelAndRefresh()
		{
			if (this.isRefreshing)
			{
				Provider.provider.matchmakingService.cancelRequest();
			}
			this.SetIsRefreshing(true);
			Provider.provider.matchmakingService.refreshMasterServer(FilterSettings.activeFilters);
		}

		// Token: 0x0600411F RID: 16671 RVA: 0x0015743C File Offset: 0x0015563C
		private void OnActiveFiltersModified()
		{
			this.SynchronizePresetsEditorButtonLabel();
		}

		// Token: 0x06004120 RID: 16672 RVA: 0x00157444 File Offset: 0x00155644
		private void OnActiveFiltersReplaced()
		{
			this.SynchronizeFilterButtons();
			this.SynchronizePresetsEditorButtonLabel();
			if (this.active)
			{
				this.CancelAndRefresh();
			}
		}

		// Token: 0x06004121 RID: 16673 RVA: 0x00157460 File Offset: 0x00155660
		private void OnCustomPresetsListChanged()
		{
			this.SynchronizePresetsList();
		}

		// Token: 0x06004122 RID: 16674 RVA: 0x00157468 File Offset: 0x00155668
		private void OnNameColumnClicked(ISleekElement button)
		{
			if (Provider.provider.matchmakingService.serverInfoComparer.GetType() == typeof(ServerListComparer_NameAscending))
			{
				Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_NameDescending());
				return;
			}
			Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_NameAscending());
		}

		// Token: 0x06004123 RID: 16675 RVA: 0x001574C4 File Offset: 0x001556C4
		private void OnMapColumnClicked(ISleekElement button)
		{
			if (Provider.provider.matchmakingService.serverInfoComparer.GetType() == typeof(ServerListComparer_MapAscending))
			{
				Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_MapDescending());
				return;
			}
			Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_MapAscending());
		}

		// Token: 0x06004124 RID: 16676 RVA: 0x00157520 File Offset: 0x00155720
		private void OnPlayersColumnClicked(ISleekElement button)
		{
			if (Provider.provider.matchmakingService.serverInfoComparer.GetType() == typeof(ServerListComparer_PlayersDefault))
			{
				Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_PlayersInverted());
				return;
			}
			Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_PlayersDefault());
		}

		// Token: 0x06004125 RID: 16677 RVA: 0x0015757C File Offset: 0x0015577C
		private void OnMaxPlayersColumnClicked(ISleekElement button)
		{
			if (Provider.provider.matchmakingService.serverInfoComparer.GetType() == typeof(ServerListComparer_MaxPlayersDefault))
			{
				Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_MaxPlayersInverted());
				return;
			}
			Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_MaxPlayersDefault());
		}

		// Token: 0x06004126 RID: 16678 RVA: 0x001575D8 File Offset: 0x001557D8
		private void OnFullnessColumnClicked(ISleekElement button)
		{
			if (Provider.provider.matchmakingService.serverInfoComparer.GetType() == typeof(ServerListComparer_FullnessDefault))
			{
				Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_FullnessInverted());
				return;
			}
			Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_FullnessDefault());
		}

		// Token: 0x06004127 RID: 16679 RVA: 0x00157634 File Offset: 0x00155834
		private void OnPingColumnClicked(ISleekElement button)
		{
			if (Provider.provider.matchmakingService.serverInfoComparer.GetType() == typeof(ServerListComparer_PingAscending))
			{
				Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_PingDescending());
				return;
			}
			Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_PingAscending());
		}

		// Token: 0x06004128 RID: 16680 RVA: 0x00157690 File Offset: 0x00155890
		private void OnAnticheatColumnClicked(ISleekElement button)
		{
			if (Provider.provider.matchmakingService.serverInfoComparer.GetType() == typeof(ServerListComparer_AnticheatDefault))
			{
				Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_AnticheatInverted());
				return;
			}
			Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_AnticheatDefault());
		}

		// Token: 0x06004129 RID: 16681 RVA: 0x001576EC File Offset: 0x001558EC
		private void OnPerspectiveColumnClicked(ISleekElement button)
		{
			if (Provider.provider.matchmakingService.serverInfoComparer.GetType() == typeof(ServerListComparer_PerspectiveDefault))
			{
				Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_PerspectiveInverted());
				return;
			}
			Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_PerspectiveDefault());
		}

		// Token: 0x0600412A RID: 16682 RVA: 0x00157748 File Offset: 0x00155948
		private void OnCombatColumnClicked(ISleekElement button)
		{
			if (Provider.provider.matchmakingService.serverInfoComparer.GetType() == typeof(ServerListComparer_CombatDefault))
			{
				Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_CombatInverted());
				return;
			}
			Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_CombatDefault());
		}

		// Token: 0x0600412B RID: 16683 RVA: 0x001577A4 File Offset: 0x001559A4
		private void OnPasswordColumnClicked(ISleekElement button)
		{
			if (Provider.provider.matchmakingService.serverInfoComparer.GetType() == typeof(ServerListComparer_PasswordDefault))
			{
				Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_PasswordInverted());
				return;
			}
			Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_PasswordDefault());
		}

		// Token: 0x0600412C RID: 16684 RVA: 0x00157800 File Offset: 0x00155A00
		private void OnWorkshopColumnClicked(ISleekElement button)
		{
			if (Provider.provider.matchmakingService.serverInfoComparer.GetType() == typeof(ServerListComparer_WorkshopDefault))
			{
				Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_WorkshopInverted());
				return;
			}
			Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_WorkshopDefault());
		}

		// Token: 0x0600412D RID: 16685 RVA: 0x0015785C File Offset: 0x00155A5C
		private void OnGoldColumnClicked(ISleekElement button)
		{
			if (Provider.provider.matchmakingService.serverInfoComparer.GetType() == typeof(ServerListComparer_GoldDefault))
			{
				Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_GoldInverted());
				return;
			}
			Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_GoldDefault());
		}

		// Token: 0x0600412E RID: 16686 RVA: 0x001578B8 File Offset: 0x00155AB8
		private void OnCheatsColumnClicked(ISleekElement button)
		{
			if (Provider.provider.matchmakingService.serverInfoComparer.GetType() == typeof(ServerListComparer_CheatsDefault))
			{
				Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_CheatsInverted());
				return;
			}
			Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_CheatsDefault());
		}

		// Token: 0x0600412F RID: 16687 RVA: 0x00157914 File Offset: 0x00155B14
		private void OnMonetizationColumnClicked(ISleekElement button)
		{
			if (Provider.provider.matchmakingService.serverInfoComparer.GetType() == typeof(ServerListComparer_MonetizationDefault))
			{
				Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_MonetizationInverted());
				return;
			}
			Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_MonetizationDefault());
		}

		// Token: 0x06004130 RID: 16688 RVA: 0x00157970 File Offset: 0x00155B70
		private void OnPluginsColumnClicked(ISleekElement button)
		{
			if (Provider.provider.matchmakingService.serverInfoComparer.GetType() == typeof(ServerListComparer_PluginsDefault))
			{
				Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_PluginsInverted());
				return;
			}
			Provider.provider.matchmakingService.sortMasterServer(new ServerListComparer_PluginsDefault());
		}

		// Token: 0x06004131 RID: 16689 RVA: 0x001579CB File Offset: 0x00155BCB
		private void OnClickedColumnsButton(ISleekElement button)
		{
			FilterSettings.isColumnsEditorOpen = !FilterSettings.isColumnsEditorOpen;
			this.AnimateOpenSubcontainers();
		}

		// Token: 0x06004132 RID: 16690 RVA: 0x001579E0 File Offset: 0x00155BE0
		private void OnClickedFiltersVisibilityButton(ISleekElement button)
		{
			FilterSettings.isQuickFiltersVisibilityEditorOpen = (!FilterSettings.isQuickFiltersVisibilityEditorOpen && FilterSettings.isQuickFiltersEditorOpen);
			this.SynchronizeVisibleFilters();
			this.AnimateOpenSubcontainers();
		}

		// Token: 0x06004133 RID: 16691 RVA: 0x00157A02 File Offset: 0x00155C02
		private void OnClickedOpenFiltersVisibilityButton(ISleekElement button)
		{
			FilterSettings.isQuickFiltersVisibilityEditorOpen = FilterSettings.isQuickFiltersEditorOpen;
			this.SynchronizeVisibleFilters();
			this.AnimateOpenSubcontainers();
		}

		// Token: 0x06004134 RID: 16692 RVA: 0x00157A1A File Offset: 0x00155C1A
		private void OnClickedCloseFiltersVisibilityButton(ISleekElement button)
		{
			FilterSettings.isQuickFiltersVisibilityEditorOpen = false;
			this.SynchronizeVisibleFilters();
			this.AnimateOpenSubcontainers();
		}

		// Token: 0x06004135 RID: 16693 RVA: 0x00157A2E File Offset: 0x00155C2E
		private void OnMapColumnToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.columns.map = value;
			this.SynchronizeVisibleColumns();
		}

		// Token: 0x06004136 RID: 16694 RVA: 0x00157A41 File Offset: 0x00155C41
		private void OnPlayersColumnToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.columns.players = value;
			this.SynchronizeVisibleColumns();
		}

		// Token: 0x06004137 RID: 16695 RVA: 0x00157A54 File Offset: 0x00155C54
		private void OnMaxPlayersColumnToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.columns.maxPlayers = value;
			this.SynchronizeVisibleColumns();
		}

		// Token: 0x06004138 RID: 16696 RVA: 0x00157A67 File Offset: 0x00155C67
		private void OnPingColumnToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.columns.ping = value;
			this.SynchronizeVisibleColumns();
		}

		// Token: 0x06004139 RID: 16697 RVA: 0x00157A7A File Offset: 0x00155C7A
		private void OnAnticheatColumnToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.columns.anticheat = value;
			this.SynchronizeVisibleColumns();
		}

		// Token: 0x0600413A RID: 16698 RVA: 0x00157A8D File Offset: 0x00155C8D
		private void OnPerspectiveColumnToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.columns.perspective = value;
			this.SynchronizeVisibleColumns();
		}

		// Token: 0x0600413B RID: 16699 RVA: 0x00157AA0 File Offset: 0x00155CA0
		private void OnCombatColumnToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.columns.combat = value;
			this.SynchronizeVisibleColumns();
		}

		// Token: 0x0600413C RID: 16700 RVA: 0x00157AB3 File Offset: 0x00155CB3
		private void OnPasswordColumnToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.columns.password = value;
			this.SynchronizeVisibleColumns();
		}

		// Token: 0x0600413D RID: 16701 RVA: 0x00157AC6 File Offset: 0x00155CC6
		private void OnWorkshopColumnToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.columns.workshop = value;
			this.SynchronizeVisibleColumns();
		}

		// Token: 0x0600413E RID: 16702 RVA: 0x00157AD9 File Offset: 0x00155CD9
		private void OnGoldColumnToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.columns.gold = value;
			this.SynchronizeVisibleColumns();
		}

		// Token: 0x0600413F RID: 16703 RVA: 0x00157AEC File Offset: 0x00155CEC
		private void OnCheatsColumnToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.columns.cheats = value;
			this.SynchronizeVisibleColumns();
		}

		// Token: 0x06004140 RID: 16704 RVA: 0x00157AFF File Offset: 0x00155CFF
		private void OnMonetizationColumnToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.columns.monetization = value;
			this.SynchronizeVisibleColumns();
		}

		// Token: 0x06004141 RID: 16705 RVA: 0x00157B12 File Offset: 0x00155D12
		private void OnPluginsColumnToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.columns.plugins = value;
			this.SynchronizeVisibleColumns();
		}

		// Token: 0x06004142 RID: 16706 RVA: 0x00157B25 File Offset: 0x00155D25
		private void OnFullnessColumnToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.columns.fullnessPercentage = value;
			this.SynchronizeVisibleColumns();
		}

		// Token: 0x06004143 RID: 16707 RVA: 0x00157B38 File Offset: 0x00155D38
		private void OnListSourceFilterToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.filterVisibility.listSource = value;
			this.SynchronizeVisibleFilters();
		}

		// Token: 0x06004144 RID: 16708 RVA: 0x00157B4B File Offset: 0x00155D4B
		private void OnNameFilterToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.filterVisibility.name = value;
			this.SynchronizeVisibleFilters();
		}

		// Token: 0x06004145 RID: 16709 RVA: 0x00157B5E File Offset: 0x00155D5E
		private void OnMapFilterToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.filterVisibility.map = value;
			this.SynchronizeVisibleFilters();
		}

		// Token: 0x06004146 RID: 16710 RVA: 0x00157B71 File Offset: 0x00155D71
		private void OnPasswordFilterToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.filterVisibility.password = value;
			this.SynchronizeVisibleFilters();
		}

		// Token: 0x06004147 RID: 16711 RVA: 0x00157B84 File Offset: 0x00155D84
		private void OnAttendanceFilterToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.filterVisibility.attendance = value;
			this.SynchronizeVisibleFilters();
		}

		// Token: 0x06004148 RID: 16712 RVA: 0x00157B97 File Offset: 0x00155D97
		private void OnSpaceFilterToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.filterVisibility.notFull = value;
			this.SynchronizeVisibleFilters();
		}

		// Token: 0x06004149 RID: 16713 RVA: 0x00157BAA File Offset: 0x00155DAA
		private void OnCombatFilterToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.filterVisibility.combat = value;
			this.SynchronizeVisibleFilters();
		}

		// Token: 0x0600414A RID: 16714 RVA: 0x00157BBD File Offset: 0x00155DBD
		private void OnCameraFilterToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.filterVisibility.camera = value;
			this.SynchronizeVisibleFilters();
		}

		// Token: 0x0600414B RID: 16715 RVA: 0x00157BD0 File Offset: 0x00155DD0
		private void OnGoldFilterToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.filterVisibility.gold = value;
			this.SynchronizeVisibleFilters();
		}

		// Token: 0x0600414C RID: 16716 RVA: 0x00157BE3 File Offset: 0x00155DE3
		private void OnMonetizationFilterToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.filterVisibility.monetization = value;
			this.SynchronizeVisibleFilters();
		}

		// Token: 0x0600414D RID: 16717 RVA: 0x00157BF6 File Offset: 0x00155DF6
		private void OnWorkshopFilterToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.filterVisibility.workshop = value;
			this.SynchronizeVisibleFilters();
		}

		// Token: 0x0600414E RID: 16718 RVA: 0x00157C09 File Offset: 0x00155E09
		private void OnPluginsFilterToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.filterVisibility.plugins = value;
			this.SynchronizeVisibleFilters();
		}

		// Token: 0x0600414F RID: 16719 RVA: 0x00157C1C File Offset: 0x00155E1C
		private void OnCheatsFilterToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.filterVisibility.cheats = value;
			this.SynchronizeVisibleFilters();
		}

		// Token: 0x06004150 RID: 16720 RVA: 0x00157C2F File Offset: 0x00155E2F
		private void OnVACFilterToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.filterVisibility.vacProtection = value;
			this.SynchronizeVisibleFilters();
		}

		// Token: 0x06004151 RID: 16721 RVA: 0x00157C42 File Offset: 0x00155E42
		private void OnBattlEyeFilterToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.filterVisibility.battlEyeProtection = value;
			this.SynchronizeVisibleFilters();
		}

		// Token: 0x06004152 RID: 16722 RVA: 0x00157C55 File Offset: 0x00155E55
		private void OnMaxPingFilterToggled(ISleekToggle toggle, bool value)
		{
			FilterSettings.filterVisibility.maxPing = value;
			this.SynchronizeVisibleFilters();
		}

		// Token: 0x06004153 RID: 16723 RVA: 0x00157C68 File Offset: 0x00155E68
		private void onTypedNameField(ISleekField field, string text)
		{
			FilterSettings.activeFilters.serverName = text;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x06004154 RID: 16724 RVA: 0x00157C7A File Offset: 0x00155E7A
		private void OnMaxPingChanged(ISleekInt32Field field, int value)
		{
			FilterSettings.activeFilters.maxPing = value;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x06004155 RID: 16725 RVA: 0x00157C8C File Offset: 0x00155E8C
		private void OnNameSubmitted(ISleekField field)
		{
			this.CancelAndRefresh();
		}

		// Token: 0x06004156 RID: 16726 RVA: 0x00157C94 File Offset: 0x00155E94
		private void OnClickedMapButton(ISleekElement button)
		{
			MenuPlayServersUI.mapFiltersUI.open(EMenuPlayMapFiltersUIOpenContext.ServerList);
			this.close();
		}

		// Token: 0x06004157 RID: 16727 RVA: 0x00157CA7 File Offset: 0x00155EA7
		private void onSwappedMonetizationState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.monetization = index + EServerMonetizationTag.Any;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x06004158 RID: 16728 RVA: 0x00157CBB File Offset: 0x00155EBB
		private void onSwappedPasswordState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.password = (EPassword)index;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x06004159 RID: 16729 RVA: 0x00157CCD File Offset: 0x00155ECD
		private void onSwappedWorkshopState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.workshop = (EWorkshop)index;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x0600415A RID: 16730 RVA: 0x00157CDF File Offset: 0x00155EDF
		private void onSwappedPluginsState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.plugins = (EPlugins)index;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x0600415B RID: 16731 RVA: 0x00157CF1 File Offset: 0x00155EF1
		private void onSwappedCheatsState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.cheats = (ECheats)index;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x0600415C RID: 16732 RVA: 0x00157D03 File Offset: 0x00155F03
		private void onSwappedAttendanceState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.attendance = (EAttendance)index;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x0600415D RID: 16733 RVA: 0x00157D15 File Offset: 0x00155F15
		private void OnSwappedNotFullState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.notFull = (index > 0);
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x0600415E RID: 16734 RVA: 0x00157D2A File Offset: 0x00155F2A
		private void onSwappedVACProtectionState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.vacProtection = (EVACProtectionFilter)index;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x0600415F RID: 16735 RVA: 0x00157D3C File Offset: 0x00155F3C
		private void onSwappedBattlEyeProtectionState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.battlEyeProtection = (EBattlEyeProtectionFilter)index;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x06004160 RID: 16736 RVA: 0x00157D4E File Offset: 0x00155F4E
		private void onSwappedCombatState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.combat = (ECombat)index;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x06004161 RID: 16737 RVA: 0x00157D60 File Offset: 0x00155F60
		private void OnSwappedGoldFilterState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.gold = (EServerListGoldFilter)index;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x06004162 RID: 16738 RVA: 0x00157D72 File Offset: 0x00155F72
		private void onSwappedCameraState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.camera = (ECameraMode)index;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x06004163 RID: 16739 RVA: 0x00157D84 File Offset: 0x00155F84
		private void OnSwappedListSourceState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.listSource = (ESteamServerList)index;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x06004164 RID: 16740 RVA: 0x00157D96 File Offset: 0x00155F96
		private void onClickedRefreshButton(ISleekElement button)
		{
			if (this.isRefreshing)
			{
				this.SetIsRefreshing(false);
				Provider.provider.matchmakingService.cancelRequest();
				return;
			}
			this.SetIsRefreshing(true);
			Provider.provider.matchmakingService.refreshMasterServer(FilterSettings.activeFilters);
		}

		// Token: 0x06004165 RID: 16741 RVA: 0x00157DD2 File Offset: 0x00155FD2
		private void onClickedHostingButton(ISleekElement button)
		{
			Provider.provider.browserService.open("https://docs.smartlydressedgames.com/en/stable/servers/server-hosting.html");
		}

		// Token: 0x06004166 RID: 16742 RVA: 0x00157DE8 File Offset: 0x00155FE8
		private void OnPresetsEditorButtonClicked(ISleekElement button)
		{
			MenuPlayServersUI.serverListFiltersUI.open();
			this.close();
		}

		// Token: 0x06004167 RID: 16743 RVA: 0x00157DFA File Offset: 0x00155FFA
		private void SynchronizePresetsButtonLabel()
		{
			if (FilterSettings.isPresetsListOpen)
			{
				this.presetsButton.text = this.localization.format("ViewPresetsButton_Close_Label");
				return;
			}
			this.presetsButton.text = this.localization.format("ViewPresetsButton_Open_Label");
		}

		// Token: 0x06004168 RID: 16744 RVA: 0x00157E3A File Offset: 0x0015603A
		private void SynchronizeQuickFiltersButtonLabel()
		{
			if (FilterSettings.isQuickFiltersEditorOpen)
			{
				this.quickFiltersButton.text = this.localization.format("QuickFiltersButton_Close_Label");
				return;
			}
			this.quickFiltersButton.text = this.localization.format("QuickFiltersButton_Open_Label");
		}

		// Token: 0x06004169 RID: 16745 RVA: 0x00157E7A File Offset: 0x0015607A
		private void onClickedPresetsButton(ISleekElement button)
		{
			FilterSettings.isPresetsListOpen = !FilterSettings.isPresetsListOpen;
			this.SynchronizePresetsButtonLabel();
			this.AnimateOpenSubcontainers();
		}

		// Token: 0x0600416A RID: 16746 RVA: 0x00157E95 File Offset: 0x00156095
		private void OnQuickFiltersButtonClicked(ISleekElement button)
		{
			FilterSettings.isQuickFiltersEditorOpen = !FilterSettings.isQuickFiltersEditorOpen;
			FilterSettings.isQuickFiltersVisibilityEditorOpen &= FilterSettings.isQuickFiltersEditorOpen;
			this.SynchronizeQuickFiltersButtonLabel();
			this.SynchronizeVisibleFilters();
			this.AnimateOpenSubcontainers();
		}

		// Token: 0x0600416B RID: 16747 RVA: 0x00157EC6 File Offset: 0x001560C6
		private ISleekElement onCreateServerElement(SteamServerAdvertisement server)
		{
			return new SleekServer(Provider.provider.matchmakingService.currentList, server)
			{
				onClickedServer = new ClickedServer(this.onClickedServer),
				SizeOffset_X = -30f
			};
		}

		/// <summary>
		/// Synchronize widgets with their values.
		/// </summary>
		// Token: 0x0600416C RID: 16748 RVA: 0x00157EFC File Offset: 0x001560FC
		private void SynchronizeFilterButtons()
		{
			this.nameField.Text = FilterSettings.activeFilters.serverName;
			string mapDisplayText = FilterSettings.activeFilters.GetMapDisplayText();
			if (string.IsNullOrEmpty(mapDisplayText))
			{
				this.mapButton.text = this.localization.format("MapFilter_Button_EmptyLabel");
			}
			else
			{
				this.mapButton.text = mapDisplayText;
			}
			this.passwordButtonState.state = (int)FilterSettings.activeFilters.password;
			this.workshopButtonState.state = (int)FilterSettings.activeFilters.workshop;
			this.pluginsButtonState.state = (int)FilterSettings.activeFilters.plugins;
			this.cheatsButtonState.state = (int)FilterSettings.activeFilters.cheats;
			this.attendanceButtonState.state = (int)FilterSettings.activeFilters.attendance;
			this.notFullButtonState.state = (FilterSettings.activeFilters.notFull ? 1 : 0);
			this.VACProtectionButtonState.state = (int)FilterSettings.activeFilters.vacProtection;
			this.battlEyeProtectionButtonState.state = (int)FilterSettings.activeFilters.battlEyeProtection;
			this.combatButtonState.state = (int)FilterSettings.activeFilters.combat;
			this.goldFilterButtonState.state = (int)FilterSettings.activeFilters.gold;
			this.cameraButtonState.state = (int)FilterSettings.activeFilters.camera;
			this.monetizationButtonState.state = FilterSettings.activeFilters.monetization - EServerMonetizationTag.Any;
			this.listSourceButtonState.state = (int)FilterSettings.activeFilters.listSource;
			this.maxPingField.Value = FilterSettings.activeFilters.maxPing;
		}

		// Token: 0x0600416D RID: 16749 RVA: 0x00158088 File Offset: 0x00156288
		private void SynchronizePresetsEditorButtonLabel()
		{
			string text = FilterSettings.activeFilters.presetName;
			if (string.IsNullOrEmpty(text))
			{
				text = this.localization.format("PresetName_Empty");
			}
			this.presetsEditorButton.text = this.localization.format("PresetsEditorButton_Label", text);
		}

		// Token: 0x0600416E RID: 16750 RVA: 0x001580D8 File Offset: 0x001562D8
		private void SynchronizePresetsList()
		{
			this.customPresetsContainer.RemoveAllChildren();
			int num = 0;
			foreach (ServerListFilters preset in FilterSettings.customPresets)
			{
				SleekCustomServerListPresetButton sleekCustomServerListPresetButton = new SleekCustomServerListPresetButton(preset);
				sleekCustomServerListPresetButton.PositionScale_X = (float)(num % 5) * 0.2f;
				sleekCustomServerListPresetButton.PositionOffset_Y = (float)(num / 5) * 30f;
				sleekCustomServerListPresetButton.SizeScale_X = 0.2f;
				sleekCustomServerListPresetButton.SizeOffset_Y = 30f;
				this.customPresetsContainer.AddChild(sleekCustomServerListPresetButton);
				num++;
			}
			this.customPresetsContainer.SizeOffset_Y = (float)((num - 1) / 5 + 1) * 30f;
			if (num > 0)
			{
				this.defaultPresetsContainer.PositionOffset_Y = this.customPresetsContainer.SizeOffset_Y + 10f;
			}
			else
			{
				this.defaultPresetsContainer.PositionOffset_Y = 0f;
			}
			float num2 = this.defaultPresetsContainer.PositionOffset_Y + this.defaultPresetsContainer.SizeOffset_Y;
			this.presetsScrollView.ContentSizeOffset = new Vector2(0f, num2);
			this.presetsScrollView.SizeOffset_Y = Mathf.Min(num2, 100f);
			this.presetsContainer.SizeOffset_Y = this.presetsScrollView.SizeOffset_Y + 20f;
			this.presetsContainer.PositionOffset_Y = -this.presetsContainer.SizeOffset_Y - 70f;
			this.AnimateOpenSubcontainers();
		}

		// Token: 0x0600416F RID: 16751 RVA: 0x0015824C File Offset: 0x0015644C
		private void CreateQuickFilterButtons()
		{
			this.listSourceButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("List_Internet_Label"), this.icons.load<Texture>("List_Internet"), this.localization.format("List_Internet_Tooltip")),
				new GUIContent(this.localization.format("List_LAN_Label"), this.icons.load<Texture>("List_LAN"), this.localization.format("List_LAN_Tooltip")),
				new GUIContent(this.localization.format("List_History_Label"), this.icons.load<Texture>("List_History"), this.localization.format("List_History_Tooltip")),
				new GUIContent(this.localization.format("List_Favorites_Label"), this.icons.load<Texture>("List_Favorites"), this.localization.format("List_Favorites_Tooltip")),
				new GUIContent(this.localization.format("List_Friends_Label"), this.icons.load<Texture2D>("List_Friends"), this.localization.format("List_Friends_Tooltip"))
			});
			this.listSourceButtonState.SizeScale_X = 0.2f;
			this.listSourceButtonState.SizeOffset_Y = 30f;
			this.listSourceButtonState.onSwappedState = new SwappedState(this.OnSwappedListSourceState);
			this.listSourceButtonState.button.iconColor = 2;
			this.listSourceButtonState.UseContentTooltip = true;
			this.filtersEditorContainer.AddChild(this.listSourceButtonState);
			this.nameField = Glazier.Get().CreateStringField();
			this.nameField.SizeScale_X = 0.2f;
			this.nameField.SizeOffset_Y = 30f;
			this.nameField.PlaceholderText = this.localization.format("Name_Filter_Hint");
			this.nameField.TooltipText = this.localization.format("Name_Filter_Tooltip");
			this.nameField.OnTextChanged += new Typed(this.onTypedNameField);
			this.nameField.OnTextSubmitted += new Entered(this.OnNameSubmitted);
			this.filtersEditorContainer.AddChild(this.nameField);
			this.mapButton = new SleekButtonIcon(this.icons.load<Texture2D>("Map"), 20);
			this.mapButton.SizeScale_X = 0.2f;
			this.mapButton.SizeOffset_Y = 30f;
			this.mapButton.tooltip = this.localization.format("MapFilter_Button_Tooltip");
			this.mapButton.onClickedButton += new ClickedButton(this.OnClickedMapButton);
			this.mapButton.iconColor = 2;
			this.filtersEditorContainer.AddChild(this.mapButton);
			this.passwordButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("No_Password_Button"), this.icons.load<Texture2D>("NotPasswordProtected"), this.localization.format("Password_Filter_No_Tooltip")),
				new GUIContent(this.localization.format("Yes_Password_Button"), this.icons.load<Texture2D>("PasswordProtected"), this.localization.format("Password_Filter_Yes_Tooltip")),
				new GUIContent(this.localization.format("Any_Password_Button"), this.icons.load<Texture2D>("AnyFilter"), this.localization.format("Password_Filter_Any_Tooltip"))
			});
			this.passwordButtonState.SizeScale_X = 0.2f;
			this.passwordButtonState.SizeOffset_Y = 30f;
			this.passwordButtonState.onSwappedState = new SwappedState(this.onSwappedPasswordState);
			this.passwordButtonState.button.iconColor = 2;
			this.passwordButtonState.UseContentTooltip = true;
			this.filtersEditorContainer.AddChild(this.passwordButtonState);
			this.attendanceButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("Empty_Button"), this.icons.load<Texture>("Empty"), this.localization.format("Attendance_Filter_Empty_Tooltip")),
				new GUIContent(this.localization.format("HasPlayers_Button"), this.icons.load<Texture>("HasPlayers"), this.localization.format("Attendance_Filter_HasPlayers_Tooltip")),
				new GUIContent(this.localization.format("Any_Attendance_Button"), this.icons.load<Texture2D>("AnyFilter"), this.localization.format("Attendance_Filter_Any_Tooltip"))
			});
			this.attendanceButtonState.SizeScale_X = 0.2f;
			this.attendanceButtonState.SizeOffset_Y = 30f;
			this.attendanceButtonState.onSwappedState = new SwappedState(this.onSwappedAttendanceState);
			this.attendanceButtonState.button.iconColor = 2;
			this.attendanceButtonState.UseContentTooltip = true;
			this.filtersEditorContainer.AddChild(this.attendanceButtonState);
			this.notFullButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("Any_Space_Button"), this.icons.load<Texture2D>("AnyFilter"), this.localization.format("Space_Filter_Any_Tooltip")),
				new GUIContent(this.localization.format("Space_Button"), this.icons.load<Texture>("Space"), this.localization.format("Space_Filter_HasSpace_Tooltip"))
			});
			this.notFullButtonState.SizeScale_X = 0.2f;
			this.notFullButtonState.SizeOffset_Y = 30f;
			this.notFullButtonState.onSwappedState = new SwappedState(this.OnSwappedNotFullState);
			this.notFullButtonState.button.iconColor = 2;
			this.notFullButtonState.UseContentTooltip = true;
			this.filtersEditorContainer.AddChild(this.notFullButtonState);
			this.combatButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("PvP_Button"), this.icons.load<Texture>("PvP"), this.localization.format("Combat_Filter_PvP_Tooltip")),
				new GUIContent(this.localization.format("PvE_Button"), this.icons.load<Texture>("PvE"), this.localization.format("Combat_Filter_PvE_Tooltip")),
				new GUIContent(this.localization.format("Any_Combat_Button"), this.icons.load<Texture2D>("AnyFilter"), this.localization.format("Combat_Filter_Any_Tooltip"))
			});
			this.combatButtonState.SizeScale_X = 0.2f;
			this.combatButtonState.SizeOffset_Y = 30f;
			this.combatButtonState.onSwappedState = new SwappedState(this.onSwappedCombatState);
			this.combatButtonState.button.iconColor = 2;
			this.combatButtonState.UseContentTooltip = true;
			this.filtersEditorContainer.AddChild(this.combatButtonState);
			this.cameraButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("First_Button"), this.icons.load<Texture>("Perspective_FirstPerson"), this.localization.format("Perspective_Filter_FirstPerson_Tooltip")),
				new GUIContent(this.localization.format("Third_Button"), this.icons.load<Texture>("Perspective_ThirdPerson"), this.localization.format("Perspective_Filter_ThirdPerson_Tooltip")),
				new GUIContent(this.localization.format("Both_Button"), this.icons.load<Texture>("Perspective_Both"), this.localization.format("Perspective_Filter_Both_Tooltip")),
				new GUIContent(this.localization.format("Vehicle_Button"), this.icons.load<Texture>("Perspective_Vehicle"), this.localization.format("Perspective_Filter_Vehicle_Tooltip")),
				new GUIContent(this.localization.format("Any_Camera_Button"), this.icons.load<Texture2D>("AnyFilter"), this.localization.format("Perspective_Filter_Any_Tooltip"))
			});
			this.cameraButtonState.SizeScale_X = 0.2f;
			this.cameraButtonState.SizeOffset_Y = 30f;
			this.cameraButtonState.onSwappedState = new SwappedState(this.onSwappedCameraState);
			this.cameraButtonState.button.iconColor = 2;
			this.cameraButtonState.UseContentTooltip = true;
			this.filtersEditorContainer.AddChild(this.cameraButtonState);
			this.goldFilterButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("Gold_Filter_Any_Label"), this.icons.load<Texture>("AnyFilter"), this.localization.format("Gold_Filter_Any_Tooltip")),
				new GUIContent(this.localization.format("Gold_Filter_DoesNotRequireGold_Label"), this.icons.load<Texture>("GoldNotRequired"), this.localization.format("Gold_Filter_DoesNotRequireGold_Tooltip")),
				new GUIContent(this.localization.format("Gold_Filter_RequiresGold_Label"), this.icons.load<Texture>("GoldRequired"), this.localization.format("Gold_Filter_RequiresGold_Tooltip"))
			});
			this.goldFilterButtonState.SizeScale_X = 0.2f;
			this.goldFilterButtonState.SizeOffset_Y = 30f;
			this.goldFilterButtonState.UseContentTooltip = true;
			this.goldFilterButtonState.onSwappedState = new SwappedState(this.OnSwappedGoldFilterState);
			this.goldFilterButtonState.button.textColor = Palette.PRO;
			this.goldFilterButtonState.button.backgroundColor = SleekColor.BackgroundIfLight(Palette.PRO);
			this.goldFilterButtonState.button.iconColor = Palette.PRO;
			this.filtersEditorContainer.AddChild(this.goldFilterButtonState);
			this.monetizationButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("Monetization_Any_Button"), this.icons.load<Texture2D>("AnyFilter"), this.localization.format("Monetization_Filter_Any_Tooltip")),
				new GUIContent(this.localization.format("Monetization_None_Button"), this.icons.load<Texture2D>("Monetization_None"), this.localization.format("Monetization_Filter_None_Tooltip")),
				new GUIContent(this.localization.format("Monetization_NonGameplay_Button"), this.icons.load<Texture2D>("NonGameplayMonetization"), this.localization.format("Monetization_Filter_NonGameplay_Tooltip"))
			});
			this.monetizationButtonState.SizeScale_X = 0.2f;
			this.monetizationButtonState.SizeOffset_Y = 30f;
			this.monetizationButtonState.onSwappedState = new SwappedState(this.onSwappedMonetizationState);
			this.monetizationButtonState.button.iconColor = 2;
			this.monetizationButtonState.UseContentTooltip = true;
			this.filtersEditorContainer.AddChild(this.monetizationButtonState);
			this.workshopButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("No_Workshop_Button"), this.icons.load<Texture2D>("NoMods"), this.localization.format("Workshop_Filter_No_Tooltip")),
				new GUIContent(this.localization.format("Yes_Workshop_Button"), this.icons.load<Texture2D>("HasMods"), this.localization.format("Workshop_Filter_Yes_Tooltip")),
				new GUIContent(this.localization.format("Any_Workshop_Button"), this.icons.load<Texture2D>("AnyFilter"), this.localization.format("Workshop_Filter_Any_Tooltip"))
			});
			this.workshopButtonState.SizeScale_X = 0.2f;
			this.workshopButtonState.SizeOffset_Y = 30f;
			this.workshopButtonState.onSwappedState = new SwappedState(this.onSwappedWorkshopState);
			this.workshopButtonState.button.iconColor = 2;
			this.workshopButtonState.UseContentTooltip = true;
			this.filtersEditorContainer.AddChild(this.workshopButtonState);
			this.pluginsButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("No_Plugins_Button"), this.icons.load<Texture2D>("Plugins_None"), this.localization.format("Plugins_Filter_No_Tooltip")),
				new GUIContent(this.localization.format("Yes_Plugins_Button"), this.icons.load<Texture2D>("Plugins"), this.localization.format("Plugins_Filter_Yes_Tooltip")),
				new GUIContent(this.localization.format("Any_Plugins_Button"), this.icons.load<Texture2D>("AnyFilter"), this.localization.format("Plugins_Filter_Any_Tooltip"))
			});
			this.pluginsButtonState.SizeScale_X = 0.2f;
			this.pluginsButtonState.SizeOffset_Y = 30f;
			this.pluginsButtonState.onSwappedState = new SwappedState(this.onSwappedPluginsState);
			this.pluginsButtonState.button.iconColor = 2;
			this.pluginsButtonState.UseContentTooltip = true;
			this.filtersEditorContainer.AddChild(this.pluginsButtonState);
			this.cheatsButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("No_Cheats_Button"), this.icons.load<Texture2D>("CheatCodes_None"), this.localization.format("Cheats_Filter_No_Tooltip")),
				new GUIContent(this.localization.format("Yes_Cheats_Button"), this.icons.load<Texture2D>("CheatCodes"), this.localization.format("Cheats_Filter_Yes_Tooltip")),
				new GUIContent(this.localization.format("Any_Cheats_Button"), this.icons.load<Texture2D>("AnyFilter"), this.localization.format("Cheats_Filter_Any_Tooltip"))
			});
			this.cheatsButtonState.SizeScale_X = 0.2f;
			this.cheatsButtonState.SizeOffset_Y = 30f;
			this.cheatsButtonState.onSwappedState = new SwappedState(this.onSwappedCheatsState);
			this.cheatsButtonState.button.iconColor = 2;
			this.cheatsButtonState.UseContentTooltip = true;
			this.filtersEditorContainer.AddChild(this.cheatsButtonState);
			this.VACProtectionButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("VAC_Secure_Button"), this.icons.load<Texture>("VAC"), this.localization.format("VAC_Filter_Secure_Tooltip")),
				new GUIContent(this.localization.format("VAC_Insecure_Button"), this.icons.load<Texture2D>("VAC_Off"), this.localization.format("VAC_Filter_Insecure_Tooltip")),
				new GUIContent(this.localization.format("VAC_Any_Button"), this.icons.load<Texture2D>("AnyFilter"), this.localization.format("VAC_Filter_Any_Tooltip"))
			});
			this.VACProtectionButtonState.SizeScale_X = 0.2f;
			this.VACProtectionButtonState.SizeOffset_Y = 30f;
			this.VACProtectionButtonState.onSwappedState = new SwappedState(this.onSwappedVACProtectionState);
			this.VACProtectionButtonState.button.iconColor = 2;
			this.VACProtectionButtonState.UseContentTooltip = true;
			this.filtersEditorContainer.AddChild(this.VACProtectionButtonState);
			this.battlEyeProtectionButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("BattlEye_Secure_Button"), this.icons.load<Texture>("BattlEye"), this.localization.format("BattlEye_Filter_Secure_Tooltip")),
				new GUIContent(this.localization.format("BattlEye_Insecure_Button"), this.icons.load<Texture2D>("BattlEye_Off"), this.localization.format("BattlEye_Filter_Insecure_Tooltip")),
				new GUIContent(this.localization.format("BattlEye_Any_Button"), this.icons.load<Texture2D>("AnyFilter"), this.localization.format("BattlEye_Filter_Any_Tooltip"))
			});
			this.battlEyeProtectionButtonState.SizeScale_X = 0.2f;
			this.battlEyeProtectionButtonState.SizeOffset_Y = 30f;
			this.battlEyeProtectionButtonState.onSwappedState = new SwappedState(this.onSwappedBattlEyeProtectionState);
			this.battlEyeProtectionButtonState.button.iconColor = 2;
			this.battlEyeProtectionButtonState.UseContentTooltip = true;
			this.filtersEditorContainer.AddChild(this.battlEyeProtectionButtonState);
			this.maxPingField = Glazier.Get().CreateInt32Field();
			this.maxPingField.SizeScale_X = 0.2f;
			this.maxPingField.SizeOffset_Y = 30f;
			this.maxPingField.TooltipText = this.localization.format("MaxPing_Filter_Tooltip");
			this.maxPingField.OnValueChanged += new TypedInt32(this.OnMaxPingChanged);
			this.filtersEditorContainer.AddChild(this.maxPingField);
			this.openFiltersVisibilityButton = Glazier.Get().CreateButton();
			this.openFiltersVisibilityButton.PositionScale_X = 0.5f;
			this.openFiltersVisibilityButton.PositionOffset_X = -50f;
			this.openFiltersVisibilityButton.PositionOffset_Y = 2f;
			this.openFiltersVisibilityButton.SizeOffset_X = 100f;
			this.openFiltersVisibilityButton.SizeOffset_Y = 16f;
			this.openFiltersVisibilityButton.OnClicked += new ClickedButton(this.OnClickedOpenFiltersVisibilityButton);
			this.openFiltersVisibilityButton.TooltipText = this.localization.format("QuickFiltersVisibilityButton_Open_Label");
			this.filtersEditorContainer.AddChild(this.openFiltersVisibilityButton);
			Texture2D texture2D = this.icons.load<Texture2D>("FilterVisibility_Open");
			Texture2D texture2D2 = this.icons.load<Texture2D>("FilterVisibility_Close");
			ISleekImage sleekImage = Glazier.Get().CreateImage(texture2D);
			sleekImage.PositionOffset_X = -8f;
			sleekImage.PositionScale_X = 0.5f;
			sleekImage.SizeOffset_X = 16f;
			sleekImage.SizeOffset_Y = 16f;
			sleekImage.TintColor = 2;
			this.openFiltersVisibilityButton.AddChild(sleekImage);
			this.closeFiltersVisibilityButton = Glazier.Get().CreateButton();
			this.closeFiltersVisibilityButton.PositionScale_X = 0.5f;
			this.closeFiltersVisibilityButton.PositionOffset_X = -50f;
			this.closeFiltersVisibilityButton.PositionOffset_Y = 2f;
			this.closeFiltersVisibilityButton.SizeOffset_X = 100f;
			this.closeFiltersVisibilityButton.SizeOffset_Y = 16f;
			this.closeFiltersVisibilityButton.OnClicked += new ClickedButton(this.OnClickedCloseFiltersVisibilityButton);
			this.closeFiltersVisibilityButton.TooltipText = this.localization.format("QuickFiltersVisibilityButton_Close_Label");
			this.filtersEditorContainer.AddChild(this.closeFiltersVisibilityButton);
			ISleekImage sleekImage2 = Glazier.Get().CreateImage(texture2D2);
			sleekImage2.PositionScale_X = 0.5f;
			sleekImage2.PositionOffset_X = -8f;
			sleekImage2.SizeOffset_X = 16f;
			sleekImage2.SizeOffset_Y = 16f;
			sleekImage2.TintColor = 2;
			this.closeFiltersVisibilityButton.AddChild(sleekImage2);
		}

		// Token: 0x06004170 RID: 16752 RVA: 0x001595CC File Offset: 0x001577CC
		private void CreateFilterVisibilityToggles()
		{
			this.listSourceToggle = Glazier.Get().CreateToggle();
			this.listSourceToggle.Value = FilterSettings.filterVisibility.listSource;
			this.listSourceToggle.AddLabel(this.localization.format("List_Label"), 1);
			this.listSourceToggle.TooltipText = this.localization.format("List_Toggle_Tooltip");
			this.listSourceToggle.OnValueChanged += new Toggled(this.OnListSourceFilterToggled);
			this.filtersEditorContainer.AddChild(this.listSourceToggle);
			this.nameToggle = Glazier.Get().CreateToggle();
			this.nameToggle.Value = FilterSettings.filterVisibility.name;
			this.nameToggle.AddLabel(this.localization.format("Name_Filter_Label"), 1);
			this.nameToggle.TooltipText = this.localization.format("Name_Filter_Toggle_Tooltip");
			this.nameToggle.OnValueChanged += new Toggled(this.OnNameFilterToggled);
			this.filtersEditorContainer.AddChild(this.nameToggle);
			this.mapToggle = Glazier.Get().CreateToggle();
			this.mapToggle.Value = FilterSettings.filterVisibility.map;
			this.mapToggle.AddLabel(this.localization.format("Map_Filter_Label"), 1);
			this.mapToggle.TooltipText = this.localization.format("Map_Filter_Toggle_Tooltip");
			this.mapToggle.OnValueChanged += new Toggled(this.OnMapFilterToggled);
			this.filtersEditorContainer.AddChild(this.mapToggle);
			this.passwordToggle = Glazier.Get().CreateToggle();
			this.passwordToggle.Value = FilterSettings.filterVisibility.password;
			this.passwordToggle.AddLabel(this.localization.format("Password_Filter_Label"), 1);
			this.passwordToggle.TooltipText = this.localization.format("Password_Filter_Toggle_Tooltip");
			this.passwordToggle.OnValueChanged += new Toggled(this.OnPasswordFilterToggled);
			this.filtersEditorContainer.AddChild(this.passwordToggle);
			this.attendanceToggle = Glazier.Get().CreateToggle();
			this.attendanceToggle.Value = FilterSettings.filterVisibility.attendance;
			this.attendanceToggle.AddLabel(this.localization.format("Attendance_Filter_Label"), 1);
			this.attendanceToggle.TooltipText = this.localization.format("Attendance_Filter_Toggle_Tooltip");
			this.attendanceToggle.OnValueChanged += new Toggled(this.OnAttendanceFilterToggled);
			this.filtersEditorContainer.AddChild(this.attendanceToggle);
			this.notFullToggle = Glazier.Get().CreateToggle();
			this.notFullToggle.Value = FilterSettings.filterVisibility.notFull;
			this.notFullToggle.AddLabel(this.localization.format("Space_Filter_Label"), 1);
			this.notFullToggle.TooltipText = this.localization.format("Space_Filter_Toggle_Tooltip");
			this.notFullToggle.OnValueChanged += new Toggled(this.OnSpaceFilterToggled);
			this.filtersEditorContainer.AddChild(this.notFullToggle);
			this.combatToggle = Glazier.Get().CreateToggle();
			this.combatToggle.Value = FilterSettings.filterVisibility.combat;
			this.combatToggle.AddLabel(this.localization.format("Combat_Filter_Label"), 1);
			this.combatToggle.TooltipText = this.localization.format("Combat_Filter_Toggle_Tooltip");
			this.combatToggle.OnValueChanged += new Toggled(this.OnCombatFilterToggled);
			this.filtersEditorContainer.AddChild(this.combatToggle);
			this.cameraToggle = Glazier.Get().CreateToggle();
			this.cameraToggle.Value = FilterSettings.filterVisibility.camera;
			this.cameraToggle.AddLabel(this.localization.format("Perspective_Filter_Label"), 1);
			this.cameraToggle.TooltipText = this.localization.format("Perspective_Filter_Toggle_Tooltip");
			this.cameraToggle.OnValueChanged += new Toggled(this.OnCameraFilterToggled);
			this.filtersEditorContainer.AddChild(this.cameraToggle);
			this.goldToggle = Glazier.Get().CreateToggle();
			this.goldToggle.Value = FilterSettings.filterVisibility.gold;
			this.goldToggle.AddLabel(this.localization.format("Gold_Filter_Label"), Palette.PRO, 1);
			this.goldToggle.TooltipText = this.localization.format("Gold_Filter_Toggle_Tooltip");
			this.goldToggle.OnValueChanged += new Toggled(this.OnGoldFilterToggled);
			this.filtersEditorContainer.AddChild(this.goldToggle);
			this.monetizationToggle = Glazier.Get().CreateToggle();
			this.monetizationToggle.Value = FilterSettings.filterVisibility.monetization;
			this.monetizationToggle.AddLabel(this.localization.format("Monetization_Filter_Label"), 1);
			this.monetizationToggle.TooltipText = this.localization.format("Monetization_Filter_Toggle_Tooltip");
			this.monetizationToggle.OnValueChanged += new Toggled(this.OnMonetizationFilterToggled);
			this.filtersEditorContainer.AddChild(this.monetizationToggle);
			this.workshopToggle = Glazier.Get().CreateToggle();
			this.workshopToggle.Value = FilterSettings.filterVisibility.workshop;
			this.workshopToggle.AddLabel(this.localization.format("Workshop_Filter_Label"), 1);
			this.workshopToggle.TooltipText = this.localization.format("Workshop_Filter_Toggle_Tooltip");
			this.workshopToggle.OnValueChanged += new Toggled(this.OnWorkshopFilterToggled);
			this.filtersEditorContainer.AddChild(this.workshopToggle);
			this.pluginsToggle = Glazier.Get().CreateToggle();
			this.pluginsToggle.Value = FilterSettings.filterVisibility.plugins;
			this.pluginsToggle.AddLabel(this.localization.format("Plugins_Filter_Label"), 1);
			this.pluginsToggle.TooltipText = this.localization.format("Plugins_Filter_Toggle_Tooltip");
			this.pluginsToggle.OnValueChanged += new Toggled(this.OnPluginsFilterToggled);
			this.filtersEditorContainer.AddChild(this.pluginsToggle);
			this.cheatsToggle = Glazier.Get().CreateToggle();
			this.cheatsToggle.Value = FilterSettings.filterVisibility.cheats;
			this.cheatsToggle.AddLabel(this.localization.format("Cheats_Filter_Label"), 1);
			this.cheatsToggle.TooltipText = this.localization.format("Cheats_Filter_Toggle_Tooltip");
			this.cheatsToggle.OnValueChanged += new Toggled(this.OnCheatsFilterToggled);
			this.filtersEditorContainer.AddChild(this.cheatsToggle);
			this.vacToggle = Glazier.Get().CreateToggle();
			this.vacToggle.Value = FilterSettings.filterVisibility.vacProtection;
			this.vacToggle.AddLabel(this.localization.format("VAC_Filter_Label"), 1);
			this.vacToggle.TooltipText = this.localization.format("VAC_Filter_Toggle_Tooltip");
			this.vacToggle.OnValueChanged += new Toggled(this.OnVACFilterToggled);
			this.filtersEditorContainer.AddChild(this.vacToggle);
			this.battlEyeToggle = Glazier.Get().CreateToggle();
			this.battlEyeToggle.Value = FilterSettings.filterVisibility.battlEyeProtection;
			this.battlEyeToggle.AddLabel(this.localization.format("BattlEye_Filter_Label"), 1);
			this.battlEyeToggle.TooltipText = this.localization.format("BattlEye_Filter_Toggle_Tooltip");
			this.battlEyeToggle.OnValueChanged += new Toggled(this.OnBattlEyeFilterToggled);
			this.filtersEditorContainer.AddChild(this.battlEyeToggle);
			this.maxPingToggle = Glazier.Get().CreateToggle();
			this.maxPingToggle.Value = FilterSettings.filterVisibility.maxPing;
			this.maxPingToggle.AddLabel(this.localization.format("MaxPing_Filter_Label"), 1);
			this.maxPingToggle.TooltipText = this.localization.format("MaxPing_Filter_Toggle_Tooltip");
			this.maxPingToggle.OnValueChanged += new Toggled(this.OnMaxPingFilterToggled);
			this.filtersEditorContainer.AddChild(this.maxPingToggle);
		}

		// Token: 0x06004171 RID: 16753 RVA: 0x00159E20 File Offset: 0x00158020
		private void AnimateOpenSubcontainers()
		{
			if (FilterSettings.isColumnsEditorOpen)
			{
				this.columnTogglesContainer.AnimatePositionOffset(0f, this.columnTogglesContainer.PositionOffset_Y, 1, 20f);
				this.columnTogglesContainer.AnimatePositionScale(0f, this.columnTogglesContainer.PositionScale_Y, 1, 20f);
			}
			else
			{
				this.columnTogglesContainer.AnimatePositionOffset(20f, this.columnTogglesContainer.PositionOffset_Y, 1, 20f);
				this.columnTogglesContainer.AnimatePositionScale(1f, this.columnTogglesContainer.PositionScale_Y, 1, 20f);
			}
			if (FilterSettings.isPresetsListOpen)
			{
				this.presetsContainer.AnimatePositionOffset(0f, this.presetsContainer.PositionOffset_Y, 1, 20f);
				this.presetsContainer.AnimatePositionScale(0f, this.presetsContainer.PositionScale_Y, 1, 20f);
			}
			else
			{
				this.presetsContainer.AnimatePositionOffset(20f, this.presetsContainer.PositionOffset_Y, 1, 20f);
				this.presetsContainer.AnimatePositionScale(1f, this.presetsContainer.PositionScale_Y, 1, 20f);
			}
			float num = FilterSettings.isPresetsListOpen ? this.presetsContainer.SizeOffset_Y : 0f;
			if (FilterSettings.isQuickFiltersEditorOpen)
			{
				this.filtersEditorContainer.AnimatePositionOffset(0f, -70f - this.filtersEditorContainer.SizeOffset_Y - num, 1, 20f);
				this.filtersEditorContainer.AnimatePositionScale(0f, this.filtersEditorContainer.PositionScale_Y, 1, 20f);
			}
			else
			{
				this.filtersEditorContainer.AnimatePositionOffset(20f, -70f - this.filtersEditorContainer.SizeOffset_Y - num, 1, 20f);
				this.filtersEditorContainer.AnimatePositionScale(1f, this.filtersEditorContainer.PositionScale_Y, 1, 20f);
			}
			float num2 = FilterSettings.isColumnsEditorOpen ? (this.columnTogglesContainer.SizeOffset_Y + 10f) : 0f;
			float num3 = FilterSettings.isQuickFiltersEditorOpen ? this.filtersEditorContainer.SizeOffset_Y : 0f;
			this.mainListContainer.AnimatePositionOffset(this.mainListContainer.PositionOffset_X, num2, 1, 20f);
			this.mainListContainer.AnimateSizeOffset(this.mainListContainer.SizeOffset_X, -num2 - num - num3, 1, 20f);
		}

		// Token: 0x06004172 RID: 16754 RVA: 0x0015A078 File Offset: 0x00158278
		private void SynchronizeVisibleColumns()
		{
			float num = -30f;
			if (FilterSettings.columns.anticheat)
			{
				num -= this.anticheatColumnButton.SizeOffset_X;
				this.anticheatColumnButton.PositionOffset_X = num;
				this.anticheatColumnButton.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.anticheatColumnButton.IsVisible = false;
			}
			if (FilterSettings.columns.cheats)
			{
				num -= this.cheatsColumnButton.SizeOffset_X;
				this.cheatsColumnButton.PositionOffset_X = num;
				this.cheatsColumnButton.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.cheatsColumnButton.IsVisible = false;
			}
			if (FilterSettings.columns.plugins)
			{
				num -= this.pluginsColumnButton.SizeOffset_X;
				this.pluginsColumnButton.PositionOffset_X = num;
				this.pluginsColumnButton.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.pluginsColumnButton.IsVisible = false;
			}
			if (FilterSettings.columns.workshop)
			{
				num -= this.workshopColumnButton.SizeOffset_X;
				this.workshopColumnButton.PositionOffset_X = num;
				this.workshopColumnButton.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.workshopColumnButton.IsVisible = false;
			}
			if (FilterSettings.columns.monetization)
			{
				num -= this.monetizationColumnButton.SizeOffset_X;
				this.monetizationColumnButton.PositionOffset_X = num;
				this.monetizationColumnButton.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.monetizationColumnButton.IsVisible = false;
			}
			if (FilterSettings.columns.gold)
			{
				num -= this.goldColumnButton.SizeOffset_X;
				this.goldColumnButton.PositionOffset_X = num;
				this.goldColumnButton.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.goldColumnButton.IsVisible = false;
			}
			if (FilterSettings.columns.perspective)
			{
				num -= this.perspectiveColumnButton.SizeOffset_X;
				this.perspectiveColumnButton.PositionOffset_X = num;
				this.perspectiveColumnButton.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.perspectiveColumnButton.IsVisible = false;
			}
			if (FilterSettings.columns.combat)
			{
				num -= this.combatColumnButton.SizeOffset_X;
				this.combatColumnButton.PositionOffset_X = num;
				this.combatColumnButton.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.combatColumnButton.IsVisible = false;
			}
			if (FilterSettings.columns.password)
			{
				num -= this.passwordColumnButton.SizeOffset_X;
				this.passwordColumnButton.PositionOffset_X = num;
				this.passwordColumnButton.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.passwordColumnButton.IsVisible = false;
			}
			if (FilterSettings.columns.fullnessPercentage)
			{
				num -= this.fullnessColumnButton.SizeOffset_X;
				this.fullnessColumnButton.PositionOffset_X = num;
				this.fullnessColumnButton.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.fullnessColumnButton.IsVisible = false;
			}
			if (FilterSettings.columns.maxPlayers)
			{
				num -= this.maxPlayersColumnButton.SizeOffset_X;
				this.maxPlayersColumnButton.PositionOffset_X = num;
				this.maxPlayersColumnButton.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.maxPlayersColumnButton.IsVisible = false;
			}
			if (FilterSettings.columns.players)
			{
				if (FilterSettings.columns.maxPlayers)
				{
					this.playersColumnButton.SizeOffset_X = 80f;
				}
				else
				{
					this.playersColumnButton.SizeOffset_X = 120f;
				}
				num -= this.playersColumnButton.SizeOffset_X;
				this.playersColumnButton.PositionOffset_X = num;
				this.playersColumnButton.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.playersColumnButton.IsVisible = false;
			}
			if (FilterSettings.columns.ping)
			{
				num -= this.pingColumnButton.SizeOffset_X;
				this.pingColumnButton.PositionOffset_X = num;
				this.pingColumnButton.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.pingColumnButton.IsVisible = false;
			}
			if (FilterSettings.columns.map)
			{
				num -= this.mapColumnButton.SizeOffset_X;
				this.mapColumnButton.PositionOffset_X = num;
				this.mapColumnButton.IsVisible = true;
				num -= 0f;
			}
			else
			{
				this.mapColumnButton.IsVisible = false;
			}
			num -= this.nameColumnButton.PositionOffset_X;
			this.nameColumnButton.SizeOffset_X = num;
			for (int i = this.serverBox.ElementCount - 1; i >= 0; i--)
			{
				(this.serverBox.GetElement(i) as SleekServer).SynchronizeVisibleColumns();
			}
		}

		// Token: 0x06004173 RID: 16755 RVA: 0x0015A4F4 File Offset: 0x001586F4
		private void SynchronizeVisibleFilters()
		{
			int num = 0;
			bool isQuickFiltersVisibilityEditorOpen = FilterSettings.isQuickFiltersVisibilityEditorOpen;
			float num2 = isQuickFiltersVisibilityEditorOpen ? 70f : 30f;
			float num3 = isQuickFiltersVisibilityEditorOpen ? 40f : 0f;
			float num4 = 20f;
			this.listSourceToggle.IsVisible = isQuickFiltersVisibilityEditorOpen;
			this.listSourceButtonState.IsVisible = (isQuickFiltersVisibilityEditorOpen || FilterSettings.filterVisibility.listSource);
			this.listSourceButtonState.isInteractable = FilterSettings.filterVisibility.listSource;
			if (this.listSourceButtonState.IsVisible)
			{
				this.listSourceToggle.PositionScale_X = (float)(num % 5) * 0.2f;
				this.listSourceToggle.PositionOffset_Y = num4 + (float)(num / 5) * num2;
				this.listSourceButtonState.PositionScale_X = (float)(num % 5) * 0.2f;
				this.listSourceButtonState.PositionOffset_Y = num4 + (float)(num / 5) * num2 + num3;
				num++;
			}
			this.nameToggle.IsVisible = isQuickFiltersVisibilityEditorOpen;
			this.nameField.IsVisible = (isQuickFiltersVisibilityEditorOpen || FilterSettings.filterVisibility.name);
			this.nameField.IsClickable = FilterSettings.filterVisibility.name;
			if (this.nameField.IsVisible)
			{
				this.nameToggle.PositionScale_X = (float)(num % 5) * 0.2f;
				this.nameToggle.PositionOffset_Y = num4 + (float)(num / 5) * num2;
				this.nameField.PositionScale_X = (float)(num % 5) * 0.2f;
				this.nameField.PositionOffset_Y = num4 + (float)(num / 5) * num2 + num3;
				num++;
			}
			this.mapToggle.IsVisible = isQuickFiltersVisibilityEditorOpen;
			this.mapButton.IsVisible = (isQuickFiltersVisibilityEditorOpen || FilterSettings.filterVisibility.map);
			this.mapButton.isClickable = FilterSettings.filterVisibility.map;
			if (this.mapButton.IsVisible)
			{
				this.mapToggle.PositionScale_X = (float)(num % 5) * 0.2f;
				this.mapToggle.PositionOffset_Y = num4 + (float)(num / 5) * num2;
				this.mapButton.PositionScale_X = (float)(num % 5) * 0.2f;
				this.mapButton.PositionOffset_Y = num4 + (float)(num / 5) * num2 + num3;
				num++;
			}
			this.passwordToggle.IsVisible = isQuickFiltersVisibilityEditorOpen;
			this.passwordButtonState.IsVisible = (isQuickFiltersVisibilityEditorOpen || FilterSettings.filterVisibility.password);
			this.passwordButtonState.isInteractable = FilterSettings.filterVisibility.password;
			if (this.passwordButtonState.IsVisible)
			{
				this.passwordToggle.PositionScale_X = (float)(num % 5) * 0.2f;
				this.passwordToggle.PositionOffset_Y = num4 + (float)(num / 5) * num2;
				this.passwordButtonState.PositionScale_X = (float)(num % 5) * 0.2f;
				this.passwordButtonState.PositionOffset_Y = num4 + (float)(num / 5) * num2 + num3;
				num++;
			}
			this.attendanceToggle.IsVisible = isQuickFiltersVisibilityEditorOpen;
			this.attendanceButtonState.IsVisible = (isQuickFiltersVisibilityEditorOpen || FilterSettings.filterVisibility.attendance);
			this.attendanceButtonState.isInteractable = FilterSettings.filterVisibility.attendance;
			if (this.attendanceButtonState.IsVisible)
			{
				this.attendanceToggle.PositionScale_X = (float)(num % 5) * 0.2f;
				this.attendanceToggle.PositionOffset_Y = num4 + (float)(num / 5) * num2;
				this.attendanceButtonState.PositionScale_X = (float)(num % 5) * 0.2f;
				this.attendanceButtonState.PositionOffset_Y = num4 + (float)(num / 5) * num2 + num3;
				num++;
			}
			this.notFullToggle.IsVisible = isQuickFiltersVisibilityEditorOpen;
			this.notFullButtonState.IsVisible = (isQuickFiltersVisibilityEditorOpen || FilterSettings.filterVisibility.notFull);
			this.notFullButtonState.isInteractable = FilterSettings.filterVisibility.notFull;
			if (this.notFullButtonState.IsVisible)
			{
				this.notFullToggle.PositionScale_X = (float)(num % 5) * 0.2f;
				this.notFullToggle.PositionOffset_Y = num4 + (float)(num / 5) * num2;
				this.notFullButtonState.PositionScale_X = (float)(num % 5) * 0.2f;
				this.notFullButtonState.PositionOffset_Y = num4 + (float)(num / 5) * num2 + num3;
				num++;
			}
			this.combatToggle.IsVisible = isQuickFiltersVisibilityEditorOpen;
			this.combatButtonState.IsVisible = (isQuickFiltersVisibilityEditorOpen || FilterSettings.filterVisibility.combat);
			this.combatButtonState.isInteractable = FilterSettings.filterVisibility.combat;
			if (this.combatButtonState.IsVisible)
			{
				this.combatToggle.PositionScale_X = (float)(num % 5) * 0.2f;
				this.combatToggle.PositionOffset_Y = num4 + (float)(num / 5) * num2;
				this.combatButtonState.PositionScale_X = (float)(num % 5) * 0.2f;
				this.combatButtonState.PositionOffset_Y = num4 + (float)(num / 5) * num2 + num3;
				num++;
			}
			this.cameraToggle.IsVisible = isQuickFiltersVisibilityEditorOpen;
			this.cameraButtonState.IsVisible = (isQuickFiltersVisibilityEditorOpen || FilterSettings.filterVisibility.camera);
			this.cameraButtonState.isInteractable = FilterSettings.filterVisibility.camera;
			if (this.cameraButtonState.IsVisible)
			{
				this.cameraToggle.PositionScale_X = (float)(num % 5) * 0.2f;
				this.cameraToggle.PositionOffset_Y = num4 + (float)(num / 5) * num2;
				this.cameraButtonState.PositionScale_X = (float)(num % 5) * 0.2f;
				this.cameraButtonState.PositionOffset_Y = num4 + (float)(num / 5) * num2 + num3;
				num++;
			}
			this.goldToggle.IsVisible = isQuickFiltersVisibilityEditorOpen;
			this.goldFilterButtonState.IsVisible = (isQuickFiltersVisibilityEditorOpen || FilterSettings.filterVisibility.gold);
			this.goldFilterButtonState.isInteractable = FilterSettings.filterVisibility.gold;
			if (this.goldFilterButtonState.IsVisible)
			{
				this.goldToggle.PositionScale_X = (float)(num % 5) * 0.2f;
				this.goldToggle.PositionOffset_Y = num4 + (float)(num / 5) * num2;
				this.goldFilterButtonState.PositionScale_X = (float)(num % 5) * 0.2f;
				this.goldFilterButtonState.PositionOffset_Y = num4 + (float)(num / 5) * num2 + num3;
				num++;
			}
			this.monetizationToggle.IsVisible = isQuickFiltersVisibilityEditorOpen;
			this.monetizationButtonState.IsVisible = (isQuickFiltersVisibilityEditorOpen || FilterSettings.filterVisibility.monetization);
			this.monetizationButtonState.isInteractable = FilterSettings.filterVisibility.monetization;
			if (this.monetizationButtonState.IsVisible)
			{
				this.monetizationToggle.PositionScale_X = (float)(num % 5) * 0.2f;
				this.monetizationToggle.PositionOffset_Y = num4 + (float)(num / 5) * num2;
				this.monetizationButtonState.PositionScale_X = (float)(num % 5) * 0.2f;
				this.monetizationButtonState.PositionOffset_Y = num4 + (float)(num / 5) * num2 + num3;
				num++;
			}
			this.workshopToggle.IsVisible = isQuickFiltersVisibilityEditorOpen;
			this.workshopButtonState.IsVisible = (isQuickFiltersVisibilityEditorOpen || FilterSettings.filterVisibility.workshop);
			this.workshopButtonState.isInteractable = FilterSettings.filterVisibility.workshop;
			if (this.workshopButtonState.IsVisible)
			{
				this.workshopToggle.PositionScale_X = (float)(num % 5) * 0.2f;
				this.workshopToggle.PositionOffset_Y = num4 + (float)(num / 5) * num2;
				this.workshopButtonState.PositionScale_X = (float)(num % 5) * 0.2f;
				this.workshopButtonState.PositionOffset_Y = num4 + (float)(num / 5) * num2 + num3;
				num++;
			}
			this.pluginsToggle.IsVisible = isQuickFiltersVisibilityEditorOpen;
			this.pluginsButtonState.IsVisible = (isQuickFiltersVisibilityEditorOpen || FilterSettings.filterVisibility.plugins);
			this.pluginsButtonState.isInteractable = FilterSettings.filterVisibility.plugins;
			if (this.pluginsButtonState.IsVisible)
			{
				this.pluginsToggle.PositionScale_X = (float)(num % 5) * 0.2f;
				this.pluginsToggle.PositionOffset_Y = num4 + (float)(num / 5) * num2;
				this.pluginsButtonState.PositionScale_X = (float)(num % 5) * 0.2f;
				this.pluginsButtonState.PositionOffset_Y = num4 + (float)(num / 5) * num2 + num3;
				num++;
			}
			this.cheatsToggle.IsVisible = isQuickFiltersVisibilityEditorOpen;
			this.cheatsButtonState.IsVisible = (isQuickFiltersVisibilityEditorOpen || FilterSettings.filterVisibility.cheats);
			this.cheatsButtonState.isInteractable = FilterSettings.filterVisibility.cheats;
			if (this.cheatsButtonState.IsVisible)
			{
				this.cheatsToggle.PositionScale_X = (float)(num % 5) * 0.2f;
				this.cheatsToggle.PositionOffset_Y = num4 + (float)(num / 5) * num2;
				this.cheatsButtonState.PositionScale_X = (float)(num % 5) * 0.2f;
				this.cheatsButtonState.PositionOffset_Y = num4 + (float)(num / 5) * num2 + num3;
				num++;
			}
			this.vacToggle.IsVisible = isQuickFiltersVisibilityEditorOpen;
			this.VACProtectionButtonState.IsVisible = (isQuickFiltersVisibilityEditorOpen || FilterSettings.filterVisibility.vacProtection);
			this.VACProtectionButtonState.isInteractable = FilterSettings.filterVisibility.vacProtection;
			if (this.VACProtectionButtonState.IsVisible)
			{
				this.vacToggle.PositionScale_X = (float)(num % 5) * 0.2f;
				this.vacToggle.PositionOffset_Y = num4 + (float)(num / 5) * num2;
				this.VACProtectionButtonState.PositionScale_X = (float)(num % 5) * 0.2f;
				this.VACProtectionButtonState.PositionOffset_Y = num4 + (float)(num / 5) * num2 + num3;
				num++;
			}
			this.battlEyeToggle.IsVisible = isQuickFiltersVisibilityEditorOpen;
			this.battlEyeProtectionButtonState.IsVisible = (isQuickFiltersVisibilityEditorOpen || FilterSettings.filterVisibility.battlEyeProtection);
			this.battlEyeProtectionButtonState.isInteractable = FilterSettings.filterVisibility.battlEyeProtection;
			if (this.battlEyeProtectionButtonState.IsVisible)
			{
				this.battlEyeToggle.PositionScale_X = (float)(num % 5) * 0.2f;
				this.battlEyeToggle.PositionOffset_Y = num4 + (float)(num / 5) * num2;
				this.battlEyeProtectionButtonState.PositionScale_X = (float)(num % 5) * 0.2f;
				this.battlEyeProtectionButtonState.PositionOffset_Y = num4 + (float)(num / 5) * num2 + num3;
				num++;
			}
			this.maxPingToggle.IsVisible = isQuickFiltersVisibilityEditorOpen;
			this.maxPingField.IsVisible = (isQuickFiltersVisibilityEditorOpen || FilterSettings.filterVisibility.maxPing);
			this.maxPingField.IsClickable = FilterSettings.filterVisibility.maxPing;
			if (this.maxPingField.IsVisible)
			{
				this.maxPingToggle.PositionScale_X = (float)(num % 5) * 0.2f;
				this.maxPingToggle.PositionOffset_Y = num4 + (float)(num / 5) * num2;
				this.maxPingField.PositionScale_X = (float)(num % 5) * 0.2f;
				this.maxPingField.PositionOffset_Y = num4 + (float)(num / 5) * num2 + num3;
				num++;
			}
			float num5 = (float)MathfEx.GetPageCount(num, 5) * num2;
			this.filtersEditorContainer.SizeOffset_Y = num5 + 20f;
			this.openFiltersVisibilityButton.IsVisible = !isQuickFiltersVisibilityEditorOpen;
			this.closeFiltersVisibilityButton.IsVisible = isQuickFiltersVisibilityEditorOpen;
		}

		// Token: 0x06004174 RID: 16756 RVA: 0x0015AF7A File Offset: 0x0015917A
		private void onClickedResetFilters(ISleekElement button)
		{
			FilterSettings.activeFilters.CopyFrom(FilterSettings.defaultPresetInternet);
			FilterSettings.activeFilters.presetName = this.localization.format("DefaultPreset_Internet_Label");
			FilterSettings.InvokeActiveFiltersReplaced();
		}

		// Token: 0x06004175 RID: 16757 RVA: 0x0015AFAA File Offset: 0x001591AA
		private void onClickedBackButton(ISleekElement button)
		{
			MenuPlayUI.open();
			this.close();
		}

		// Token: 0x06004176 RID: 16758 RVA: 0x0015AFB8 File Offset: 0x001591B8
		public override void OnDestroy()
		{
			base.OnDestroy();
			if (this.isRefreshing)
			{
				Provider.provider.matchmakingService.cancelRequest();
			}
			TempSteamworksMatchmaking matchmakingService = Provider.provider.matchmakingService;
			matchmakingService.onMasterServerAdded = (TempSteamworksMatchmaking.MasterServerAdded)Delegate.Remove(matchmakingService.onMasterServerAdded, new TempSteamworksMatchmaking.MasterServerAdded(this.onMasterServerAdded));
			TempSteamworksMatchmaking matchmakingService2 = Provider.provider.matchmakingService;
			matchmakingService2.onMasterServerRemoved = (TempSteamworksMatchmaking.MasterServerRemoved)Delegate.Remove(matchmakingService2.onMasterServerRemoved, new TempSteamworksMatchmaking.MasterServerRemoved(this.onMasterServerRemoved));
			TempSteamworksMatchmaking matchmakingService3 = Provider.provider.matchmakingService;
			matchmakingService3.onMasterServerResorted = (TempSteamworksMatchmaking.MasterServerResorted)Delegate.Remove(matchmakingService3.onMasterServerResorted, new TempSteamworksMatchmaking.MasterServerResorted(this.onMasterServerResorted));
			TempSteamworksMatchmaking matchmakingService4 = Provider.provider.matchmakingService;
			matchmakingService4.onMasterServerRefreshed = (TempSteamworksMatchmaking.MasterServerRefreshed)Delegate.Remove(matchmakingService4.onMasterServerRefreshed, new TempSteamworksMatchmaking.MasterServerRefreshed(this.onMasterServerRefreshed));
			FilterSettings.OnActiveFiltersModified -= new Action(this.OnActiveFiltersModified);
			FilterSettings.OnActiveFiltersReplaced -= new Action(this.OnActiveFiltersReplaced);
			FilterSettings.OnCustomPresetsListChanged -= new Action(this.OnCustomPresetsListChanged);
		}

		// Token: 0x06004177 RID: 16759 RVA: 0x0015B0C4 File Offset: 0x001592C4
		public override void OnUpdate()
		{
			base.OnUpdate();
			if (this.isRefreshing)
			{
				float num = this.refreshIcon.RotationAngle + Time.deltaTime * 90f;
				num %= 360f;
				this.refreshIcon.RotationAngle = num;
			}
		}

		// Token: 0x06004178 RID: 16760 RVA: 0x0015B10C File Offset: 0x0015930C
		private void SetIsRefreshing(bool value)
		{
			this.isRefreshing = value;
			if (this.isRefreshing)
			{
				this.refreshButton.Text = this.localization.format("Refresh_Cancel_Label");
				this.refreshButton.TooltipText = this.localization.format("Refresh_Cancel_Tooltip");
				return;
			}
			this.refreshButton.Text = this.localization.format("Refresh_Label");
			this.refreshButton.TooltipText = this.localization.format("Refresh_Tooltip");
			this.refreshIcon.RotationAngle = 0f;
		}

		// Token: 0x06004179 RID: 16761 RVA: 0x0015B1A8 File Offset: 0x001593A8
		public MenuPlayServersUI()
		{
			if (this.icons != null)
			{
				this.icons.unload();
			}
			this.localization = Localization.read("/Menu/Play/MenuPlayServers.dat");
			this.icons = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Play/MenuPlayServers/MenuPlayServers.unity3d");
			this.active = false;
			this.columnTogglesContainer = Glazier.Get().CreateFrame();
			this.columnTogglesContainer.PositionOffset_X = 20f;
			this.columnTogglesContainer.PositionScale_X = 1f;
			this.columnTogglesContainer.SizeScale_X = 1f;
			base.AddChild(this.columnTogglesContainer);
			int num = 0;
			ISleekToggle sleekToggle = Glazier.Get().CreateToggle();
			sleekToggle.PositionScale_X = (float)(num % 5) * 0.2f;
			sleekToggle.PositionOffset_Y = (float)(num / 5) * 40f;
			sleekToggle.Value = FilterSettings.columns.map;
			sleekToggle.AddLabel(this.localization.format("Map_Column_Toggle_Label"), 1);
			sleekToggle.TooltipText = this.localization.format("Map_Column_Toggle_Tooltip");
			sleekToggle.OnValueChanged += new Toggled(this.OnMapColumnToggled);
			this.columnTogglesContainer.AddChild(sleekToggle);
			num++;
			ISleekToggle sleekToggle2 = Glazier.Get().CreateToggle();
			sleekToggle2.PositionScale_X = (float)(num % 5) * 0.2f;
			sleekToggle2.PositionOffset_Y = (float)(num / 5) * 40f;
			sleekToggle2.Value = FilterSettings.columns.ping;
			sleekToggle2.AddLabel(this.localization.format("Ping_Column_Toggle_Label"), 1);
			sleekToggle2.TooltipText = this.localization.format("Ping_Column_Toggle_Tooltip");
			sleekToggle2.OnValueChanged += new Toggled(this.OnPingColumnToggled);
			this.columnTogglesContainer.AddChild(sleekToggle2);
			num++;
			ISleekToggle sleekToggle3 = Glazier.Get().CreateToggle();
			sleekToggle3.PositionScale_X = (float)(num % 5) * 0.2f;
			sleekToggle3.PositionOffset_Y = (float)(num / 5) * 40f;
			sleekToggle3.Value = FilterSettings.columns.players;
			sleekToggle3.AddLabel(this.localization.format("Players_Column_Toggle_Label"), 1);
			sleekToggle3.TooltipText = this.localization.format("Players_Column_Toggle_Tooltip");
			sleekToggle3.OnValueChanged += new Toggled(this.OnPlayersColumnToggled);
			this.columnTogglesContainer.AddChild(sleekToggle3);
			num++;
			ISleekToggle sleekToggle4 = Glazier.Get().CreateToggle();
			sleekToggle4.PositionScale_X = (float)(num % 5) * 0.2f;
			sleekToggle4.PositionOffset_Y = (float)(num / 5) * 40f;
			sleekToggle4.Value = FilterSettings.columns.maxPlayers;
			sleekToggle4.AddLabel(this.localization.format("MaxPlayers_Column_Toggle_Label"), 1);
			sleekToggle4.TooltipText = this.localization.format("MaxPlayers_Column_Toggle_Tooltip");
			sleekToggle4.OnValueChanged += new Toggled(this.OnMaxPlayersColumnToggled);
			this.columnTogglesContainer.AddChild(sleekToggle4);
			num++;
			ISleekToggle sleekToggle5 = Glazier.Get().CreateToggle();
			sleekToggle5.PositionScale_X = (float)(num % 5) * 0.2f;
			sleekToggle5.PositionOffset_Y = (float)(num / 5) * 40f;
			sleekToggle5.Value = FilterSettings.columns.password;
			sleekToggle5.AddLabel(this.localization.format("Password_Column_Toggle_Label"), 1);
			sleekToggle5.TooltipText = this.localization.format("Password_Column_Toggle_Tooltip");
			sleekToggle5.OnValueChanged += new Toggled(this.OnPasswordColumnToggled);
			this.columnTogglesContainer.AddChild(sleekToggle5);
			num++;
			ISleekToggle sleekToggle6 = Glazier.Get().CreateToggle();
			sleekToggle6.PositionScale_X = (float)(num % 5) * 0.2f;
			sleekToggle6.PositionOffset_Y = (float)(num / 5) * 40f;
			sleekToggle6.Value = FilterSettings.columns.combat;
			sleekToggle6.AddLabel(this.localization.format("Combat_Column_Toggle_Label"), 1);
			sleekToggle6.TooltipText = this.localization.format("Combat_Column_Toggle_Tooltip");
			sleekToggle6.OnValueChanged += new Toggled(this.OnCombatColumnToggled);
			this.columnTogglesContainer.AddChild(sleekToggle6);
			num++;
			ISleekToggle sleekToggle7 = Glazier.Get().CreateToggle();
			sleekToggle7.PositionScale_X = (float)(num % 5) * 0.2f;
			sleekToggle7.PositionOffset_Y = (float)(num / 5) * 40f;
			sleekToggle7.Value = FilterSettings.columns.perspective;
			sleekToggle7.AddLabel(this.localization.format("Perspective_Column_Toggle_Label"), 1);
			sleekToggle7.TooltipText = this.localization.format("Perspective_Column_Toggle_Tooltip");
			sleekToggle7.OnValueChanged += new Toggled(this.OnPerspectiveColumnToggled);
			this.columnTogglesContainer.AddChild(sleekToggle7);
			num++;
			ISleekToggle sleekToggle8 = Glazier.Get().CreateToggle();
			sleekToggle8.PositionScale_X = (float)(num % 5) * 0.2f;
			sleekToggle8.PositionOffset_Y = (float)(num / 5) * 40f;
			sleekToggle8.Value = FilterSettings.columns.maxPlayers;
			sleekToggle8.AddLabel(this.localization.format("Gold_Column_Toggle_Label"), Palette.PRO, 1);
			sleekToggle8.TooltipText = this.localization.format("Gold_Column_Toggle_Tooltip");
			sleekToggle8.OnValueChanged += new Toggled(this.OnGoldColumnToggled);
			this.columnTogglesContainer.AddChild(sleekToggle8);
			num++;
			ISleekToggle sleekToggle9 = Glazier.Get().CreateToggle();
			sleekToggle9.PositionScale_X = (float)(num % 5) * 0.2f;
			sleekToggle9.PositionOffset_Y = (float)(num / 5) * 40f;
			sleekToggle9.Value = FilterSettings.columns.cheats;
			sleekToggle9.AddLabel(this.localization.format("Monetization_Column_Toggle_Label"), 1);
			sleekToggle9.TooltipText = this.localization.format("Monetization_Column_Toggle_Tooltip");
			sleekToggle9.OnValueChanged += new Toggled(this.OnMonetizationColumnToggled);
			this.columnTogglesContainer.AddChild(sleekToggle9);
			num++;
			ISleekToggle sleekToggle10 = Glazier.Get().CreateToggle();
			sleekToggle10.PositionScale_X = (float)(num % 5) * 0.2f;
			sleekToggle10.PositionOffset_Y = (float)(num / 5) * 40f;
			sleekToggle10.Value = FilterSettings.columns.workshop;
			sleekToggle10.AddLabel(this.localization.format("Workshop_Column_Toggle_Label"), 1);
			sleekToggle10.TooltipText = this.localization.format("Workshop_Column_Toggle_Tooltip");
			sleekToggle10.OnValueChanged += new Toggled(this.OnWorkshopColumnToggled);
			this.columnTogglesContainer.AddChild(sleekToggle10);
			num++;
			ISleekToggle sleekToggle11 = Glazier.Get().CreateToggle();
			sleekToggle11.PositionScale_X = (float)(num % 5) * 0.2f;
			sleekToggle11.PositionOffset_Y = (float)(num / 5) * 40f;
			sleekToggle11.Value = FilterSettings.columns.cheats;
			sleekToggle11.AddLabel(this.localization.format("Plugins_Column_Toggle_Label"), 1);
			sleekToggle11.TooltipText = this.localization.format("Plugins_Column_Toggle_Tooltip");
			sleekToggle11.OnValueChanged += new Toggled(this.OnPluginsColumnToggled);
			this.columnTogglesContainer.AddChild(sleekToggle11);
			num++;
			ISleekToggle sleekToggle12 = Glazier.Get().CreateToggle();
			sleekToggle12.PositionScale_X = (float)(num % 5) * 0.2f;
			sleekToggle12.PositionOffset_Y = (float)(num / 5) * 40f;
			sleekToggle12.Value = FilterSettings.columns.cheats;
			sleekToggle12.AddLabel(this.localization.format("Cheats_Column_Toggle_Label"), 1);
			sleekToggle12.TooltipText = this.localization.format("Cheats_Column_Toggle_Tooltip");
			sleekToggle12.OnValueChanged += new Toggled(this.OnCheatsColumnToggled);
			this.columnTogglesContainer.AddChild(sleekToggle12);
			num++;
			ISleekToggle sleekToggle13 = Glazier.Get().CreateToggle();
			sleekToggle13.PositionScale_X = (float)(num % 5) * 0.2f;
			sleekToggle13.PositionOffset_Y = (float)(num / 5) * 40f;
			sleekToggle13.Value = FilterSettings.columns.anticheat;
			sleekToggle13.AddLabel(this.localization.format("Anticheat_Column_Toggle_Label"), 1);
			sleekToggle13.TooltipText = this.localization.format("Anticheat_Column_Toggle_Tooltip");
			sleekToggle13.OnValueChanged += new Toggled(this.OnAnticheatColumnToggled);
			this.columnTogglesContainer.AddChild(sleekToggle13);
			num++;
			ISleekToggle sleekToggle14 = Glazier.Get().CreateToggle();
			sleekToggle14.PositionScale_X = (float)(num % 5) * 0.2f;
			sleekToggle14.PositionOffset_Y = (float)(num / 5) * 40f;
			sleekToggle14.Value = FilterSettings.columns.fullnessPercentage;
			sleekToggle14.AddLabel(this.localization.format("Fullness_Column_Toggle_Label"), 1);
			sleekToggle14.TooltipText = this.localization.format("Fullness_Column_Toggle_Tooltip");
			sleekToggle14.OnValueChanged += new Toggled(this.OnFullnessColumnToggled);
			this.columnTogglesContainer.AddChild(sleekToggle14);
			num++;
			this.columnTogglesContainer.SizeOffset_Y = (float)((num - 1) / 5 + 1) * 40f;
			this.mainListContainer = Glazier.Get().CreateFrame();
			this.mainListContainer.SizeScale_X = 1f;
			this.mainListContainer.SizeScale_Y = 1f;
			base.AddChild(this.mainListContainer);
			this.filtersEditorContainer = Glazier.Get().CreateFrame();
			this.filtersEditorContainer.PositionOffset_X = 20f;
			this.filtersEditorContainer.PositionOffset_Y = -190f;
			this.filtersEditorContainer.PositionScale_X = 1f;
			this.filtersEditorContainer.PositionScale_Y = 1f;
			this.filtersEditorContainer.SizeScale_X = 1f;
			base.AddChild(this.filtersEditorContainer);
			ISleekImage sleekImage = Glazier.Get().CreateImage();
			sleekImage.PositionOffset_Y = 9f;
			sleekImage.SizeScale_X = 0.5f;
			sleekImage.SizeOffset_X = -60f;
			sleekImage.SizeOffset_Y = 2f;
			sleekImage.Texture = GlazierResources.PixelTexture;
			sleekImage.TintColor = new SleekColor(2, 0.5f);
			this.filtersEditorContainer.AddChild(sleekImage);
			ISleekImage sleekImage2 = Glazier.Get().CreateImage();
			sleekImage2.PositionScale_X = 0.5f;
			sleekImage2.PositionOffset_Y = 9f;
			sleekImage2.PositionOffset_X = 60f;
			sleekImage2.SizeScale_X = 0.5f;
			sleekImage2.SizeOffset_X = -60f;
			sleekImage2.SizeOffset_Y = 2f;
			sleekImage2.Texture = GlazierResources.PixelTexture;
			sleekImage2.TintColor = new SleekColor(2, 0.5f);
			this.filtersEditorContainer.AddChild(sleekImage2);
			this.CreateQuickFilterButtons();
			this.CreateFilterVisibilityToggles();
			this.SynchronizeVisibleFilters();
			this.presetsContainer = Glazier.Get().CreateFrame();
			this.presetsContainer.PositionOffset_X = 20f;
			this.presetsContainer.PositionScale_X = 1f;
			this.presetsContainer.PositionScale_Y = 1f;
			this.presetsContainer.SizeScale_X = 1f;
			base.AddChild(this.presetsContainer);
			ISleekImage sleekImage3 = Glazier.Get().CreateImage();
			sleekImage3.PositionOffset_Y = 9f;
			sleekImage3.SizeScale_X = 1f;
			sleekImage3.SizeOffset_Y = 2f;
			sleekImage3.Texture = GlazierResources.PixelTexture;
			sleekImage3.TintColor = new SleekColor(2, 0.5f);
			this.presetsContainer.AddChild(sleekImage3);
			this.presetsScrollView = Glazier.Get().CreateScrollView();
			this.presetsScrollView.PositionOffset_Y = 20f;
			this.presetsScrollView.SizeScale_X = 1f;
			this.presetsScrollView.ScaleContentToWidth = true;
			this.presetsContainer.AddChild(this.presetsScrollView);
			this.customPresetsContainer = Glazier.Get().CreateFrame();
			this.customPresetsContainer.SizeScale_X = 1f;
			this.presetsScrollView.AddChild(this.customPresetsContainer);
			this.defaultPresetsContainer = Glazier.Get().CreateFrame();
			this.defaultPresetsContainer.SizeScale_X = 1f;
			this.defaultPresetsContainer.SizeOffset_Y = 30f;
			this.presetsScrollView.AddChild(this.defaultPresetsContainer);
			SleekDefaultServerListPresetButton sleekDefaultServerListPresetButton = new SleekDefaultServerListPresetButton(FilterSettings.defaultPresetInternet, this.localization, this.icons);
			sleekDefaultServerListPresetButton.SizeOffset_Y = 30f;
			sleekDefaultServerListPresetButton.SizeScale_X = 0.2f;
			this.defaultPresetsContainer.AddChild(sleekDefaultServerListPresetButton);
			SleekDefaultServerListPresetButton sleekDefaultServerListPresetButton2 = new SleekDefaultServerListPresetButton(FilterSettings.defaultPresetLAN, this.localization, this.icons);
			sleekDefaultServerListPresetButton2.PositionScale_X = 0.2f;
			sleekDefaultServerListPresetButton2.SizeOffset_Y = 30f;
			sleekDefaultServerListPresetButton2.SizeScale_X = 0.2f;
			this.defaultPresetsContainer.AddChild(sleekDefaultServerListPresetButton2);
			SleekDefaultServerListPresetButton sleekDefaultServerListPresetButton3 = new SleekDefaultServerListPresetButton(FilterSettings.defaultPresetHistory, this.localization, this.icons);
			sleekDefaultServerListPresetButton3.PositionScale_X = 0.4f;
			sleekDefaultServerListPresetButton3.SizeOffset_Y = 30f;
			sleekDefaultServerListPresetButton3.SizeScale_X = 0.2f;
			this.defaultPresetsContainer.AddChild(sleekDefaultServerListPresetButton3);
			SleekDefaultServerListPresetButton sleekDefaultServerListPresetButton4 = new SleekDefaultServerListPresetButton(FilterSettings.defaultPresetFavorites, this.localization, this.icons);
			sleekDefaultServerListPresetButton4.PositionScale_X = 0.6f;
			sleekDefaultServerListPresetButton4.SizeOffset_Y = 30f;
			sleekDefaultServerListPresetButton4.SizeScale_X = 0.2f;
			this.defaultPresetsContainer.AddChild(sleekDefaultServerListPresetButton4);
			SleekDefaultServerListPresetButton sleekDefaultServerListPresetButton5 = new SleekDefaultServerListPresetButton(FilterSettings.defaultPresetFriends, this.localization, this.icons);
			sleekDefaultServerListPresetButton5.PositionScale_X = 0.8f;
			sleekDefaultServerListPresetButton5.SizeOffset_Y = 30f;
			sleekDefaultServerListPresetButton5.SizeScale_X = 0.2f;
			this.defaultPresetsContainer.AddChild(sleekDefaultServerListPresetButton5);
			SleekButtonIcon sleekButtonIcon = new SleekButtonIcon(this.icons.load<Texture2D>("Columns"));
			sleekButtonIcon.SizeOffset_X = 40f;
			sleekButtonIcon.SizeOffset_Y = 40f;
			sleekButtonIcon.iconPositionOffset = 10;
			sleekButtonIcon.iconColor = 2;
			sleekButtonIcon.tooltip = this.localization.format("Columns_Tooltip");
			sleekButtonIcon.onClickedButton += new ClickedButton(this.OnClickedColumnsButton);
			this.mainListContainer.AddChild(sleekButtonIcon);
			this.nameColumnButton = Glazier.Get().CreateButton();
			this.nameColumnButton.PositionOffset_X = 40f;
			this.nameColumnButton.SizeOffset_X = -310f;
			this.nameColumnButton.SizeOffset_Y = 40f;
			this.nameColumnButton.SizeScale_X = 1f;
			this.nameColumnButton.Text = this.localization.format("Sort_Name");
			this.nameColumnButton.TooltipText = this.localization.format("Sort_Name_Tooltip");
			this.nameColumnButton.OnClicked += new ClickedButton(this.OnNameColumnClicked);
			this.mainListContainer.AddChild(this.nameColumnButton);
			this.mapColumnButton = Glazier.Get().CreateButton();
			this.mapColumnButton.PositionOffset_X = -260f;
			this.mapColumnButton.PositionScale_X = 1f;
			this.mapColumnButton.SizeOffset_X = 153f;
			this.mapColumnButton.SizeOffset_Y = 40f;
			this.mapColumnButton.Text = this.localization.format("Sort_Map");
			this.mapColumnButton.TooltipText = this.localization.format("Sort_Map_Tooltip");
			this.mapColumnButton.OnClicked += new ClickedButton(this.OnMapColumnClicked);
			this.mainListContainer.AddChild(this.mapColumnButton);
			this.playersColumnButton = Glazier.Get().CreateButton();
			this.playersColumnButton.PositionOffset_X = -150f;
			this.playersColumnButton.PositionScale_X = 1f;
			this.playersColumnButton.SizeOffset_X = 80f;
			this.playersColumnButton.SizeOffset_Y = 40f;
			this.playersColumnButton.Text = this.localization.format("Sort_Players");
			this.playersColumnButton.TooltipText = this.localization.format("Sort_Players_Tooltip");
			this.playersColumnButton.OnClicked += new ClickedButton(this.OnPlayersColumnClicked);
			this.mainListContainer.AddChild(this.playersColumnButton);
			this.maxPlayersColumnButton = Glazier.Get().CreateButton();
			this.maxPlayersColumnButton.PositionOffset_X = -150f;
			this.maxPlayersColumnButton.PositionScale_X = 1f;
			this.maxPlayersColumnButton.SizeOffset_X = 80f;
			this.maxPlayersColumnButton.SizeOffset_Y = 40f;
			this.maxPlayersColumnButton.Text = this.localization.format("MaxPlayers_Column_Label");
			this.maxPlayersColumnButton.TooltipText = this.localization.format("MaxPlayers_Column_Tooltip");
			this.maxPlayersColumnButton.OnClicked += new ClickedButton(this.OnMaxPlayersColumnClicked);
			this.mainListContainer.AddChild(this.maxPlayersColumnButton);
			this.fullnessColumnButton = Glazier.Get().CreateButton();
			this.fullnessColumnButton.PositionOffset_X = -150f;
			this.fullnessColumnButton.PositionScale_X = 1f;
			this.fullnessColumnButton.SizeOffset_X = 80f;
			this.fullnessColumnButton.SizeOffset_Y = 40f;
			this.fullnessColumnButton.Text = this.localization.format("Fullness_Column_Label");
			this.fullnessColumnButton.TooltipText = this.localization.format("Fullness_Column_Tooltip");
			this.fullnessColumnButton.OnClicked += new ClickedButton(this.OnFullnessColumnClicked);
			this.mainListContainer.AddChild(this.fullnessColumnButton);
			this.pingColumnButton = Glazier.Get().CreateButton();
			this.pingColumnButton.PositionOffset_X = -80f;
			this.pingColumnButton.PositionScale_X = 1f;
			this.pingColumnButton.SizeOffset_X = 80f;
			this.pingColumnButton.SizeOffset_Y = 40f;
			this.pingColumnButton.Text = this.localization.format("Sort_Ping");
			this.pingColumnButton.TooltipText = this.localization.format("Sort_Ping_Tooltip");
			this.pingColumnButton.OnClicked += new ClickedButton(this.OnPingColumnClicked);
			this.mainListContainer.AddChild(this.pingColumnButton);
			this.anticheatColumnButton = Glazier.Get().CreateButton();
			this.anticheatColumnButton.PositionScale_X = 1f;
			this.anticheatColumnButton.SizeOffset_X = 80f;
			this.anticheatColumnButton.SizeOffset_Y = 40f;
			this.anticheatColumnButton.Text = this.localization.format("Anticheat_Column_Label");
			this.anticheatColumnButton.TooltipText = this.localization.format("Anticheat_Column_Tooltip");
			this.anticheatColumnButton.OnClicked += new ClickedButton(this.OnAnticheatColumnClicked);
			this.mainListContainer.AddChild(this.anticheatColumnButton);
			this.perspectiveColumnButton = new SleekButtonIcon(this.icons.load<Texture2D>("Perspective"), 20);
			this.perspectiveColumnButton.PositionScale_X = 1f;
			this.perspectiveColumnButton.SizeOffset_X = 40f;
			this.perspectiveColumnButton.SizeOffset_Y = 40f;
			this.perspectiveColumnButton.tooltip = this.localization.format("Perspective_Column_Tooltip");
			this.perspectiveColumnButton.onClickedButton += new ClickedButton(this.OnPerspectiveColumnClicked);
			this.perspectiveColumnButton.iconColor = 2;
			this.perspectiveColumnButton.iconPositionOffset = 10;
			this.mainListContainer.AddChild(this.perspectiveColumnButton);
			this.combatColumnButton = new SleekButtonIcon(this.icons.load<Texture2D>("Combat"), 20);
			this.combatColumnButton.PositionScale_X = 1f;
			this.combatColumnButton.SizeOffset_X = 40f;
			this.combatColumnButton.SizeOffset_Y = 40f;
			this.combatColumnButton.tooltip = this.localization.format("Combat_Column_Tooltip");
			this.combatColumnButton.onClickedButton += new ClickedButton(this.OnCombatColumnClicked);
			this.combatColumnButton.iconColor = 2;
			this.combatColumnButton.iconPositionOffset = 10;
			this.mainListContainer.AddChild(this.combatColumnButton);
			this.passwordColumnButton = new SleekButtonIcon(this.icons.load<Texture2D>("PasswordProtected"), 20);
			this.passwordColumnButton.PositionScale_X = 1f;
			this.passwordColumnButton.SizeOffset_X = 40f;
			this.passwordColumnButton.SizeOffset_Y = 40f;
			this.passwordColumnButton.tooltip = this.localization.format("Password_Column_Tooltip");
			this.passwordColumnButton.onClickedButton += new ClickedButton(this.OnPasswordColumnClicked);
			this.passwordColumnButton.iconColor = 2;
			this.passwordColumnButton.iconPositionOffset = 10;
			this.mainListContainer.AddChild(this.passwordColumnButton);
			this.workshopColumnButton = new SleekButtonIcon(this.icons.load<Texture2D>("HasMods"), 20);
			this.workshopColumnButton.PositionScale_X = 1f;
			this.workshopColumnButton.SizeOffset_X = 40f;
			this.workshopColumnButton.SizeOffset_Y = 40f;
			this.workshopColumnButton.tooltip = this.localization.format("Workshop_Column_Tooltip");
			this.workshopColumnButton.onClickedButton += new ClickedButton(this.OnWorkshopColumnClicked);
			this.workshopColumnButton.iconColor = 2;
			this.workshopColumnButton.iconPositionOffset = 10;
			this.mainListContainer.AddChild(this.workshopColumnButton);
			this.goldColumnButton = new SleekButtonIcon(this.icons.load<Texture2D>("GoldRequired"), 20);
			this.goldColumnButton.PositionScale_X = 1f;
			this.goldColumnButton.SizeOffset_X = 40f;
			this.goldColumnButton.SizeOffset_Y = 40f;
			this.goldColumnButton.tooltip = this.localization.format("Gold_Column_Tooltip");
			this.goldColumnButton.onClickedButton += new ClickedButton(this.OnGoldColumnClicked);
			this.goldColumnButton.textColor = Palette.PRO;
			this.goldColumnButton.backgroundColor = SleekColor.BackgroundIfLight(Palette.PRO);
			this.goldColumnButton.iconColor = Palette.PRO;
			this.goldColumnButton.iconPositionOffset = 10;
			this.mainListContainer.AddChild(this.goldColumnButton);
			this.cheatsColumnButton = new SleekButtonIcon(this.icons.load<Texture2D>("CheatCodes"), 20);
			this.cheatsColumnButton.PositionScale_X = 1f;
			this.cheatsColumnButton.SizeOffset_X = 40f;
			this.cheatsColumnButton.SizeOffset_Y = 40f;
			this.cheatsColumnButton.tooltip = this.localization.format("Cheats_Column_Tooltip");
			this.cheatsColumnButton.onClickedButton += new ClickedButton(this.OnCheatsColumnClicked);
			this.cheatsColumnButton.iconColor = 2;
			this.cheatsColumnButton.iconPositionOffset = 10;
			this.mainListContainer.AddChild(this.cheatsColumnButton);
			this.monetizationColumnButton = new SleekButtonIcon(this.icons.load<Texture2D>("Monetized"), 20);
			this.monetizationColumnButton.PositionOffset_X = -260f;
			this.monetizationColumnButton.PositionScale_X = 1f;
			this.monetizationColumnButton.SizeOffset_X = 40f;
			this.monetizationColumnButton.SizeOffset_Y = 40f;
			this.monetizationColumnButton.tooltip = this.localization.format("Monetization_Column_Tooltip");
			this.monetizationColumnButton.onClickedButton += new ClickedButton(this.OnMonetizationColumnClicked);
			this.monetizationColumnButton.iconColor = 2;
			this.monetizationColumnButton.iconPositionOffset = 10;
			this.mainListContainer.AddChild(this.monetizationColumnButton);
			this.pluginsColumnButton = new SleekButtonIcon(this.icons.load<Texture2D>("Plugins"), 20);
			this.pluginsColumnButton.PositionScale_X = 1f;
			this.pluginsColumnButton.SizeOffset_X = 40f;
			this.pluginsColumnButton.SizeOffset_Y = 40f;
			this.pluginsColumnButton.tooltip = this.localization.format("Plugins_Column_Tooltip");
			this.pluginsColumnButton.onClickedButton += new ClickedButton(this.OnPluginsColumnClicked);
			this.pluginsColumnButton.iconColor = 2;
			this.pluginsColumnButton.iconPositionOffset = 10;
			this.mainListContainer.AddChild(this.pluginsColumnButton);
			this.infoBox = Glazier.Get().CreateBox();
			this.infoBox.PositionOffset_Y = 50f;
			this.infoBox.SizeScale_X = 1f;
			this.infoBox.SizeOffset_X = -30f;
			this.infoBox.SizeOffset_Y = 50f;
			this.mainListContainer.AddChild(this.infoBox);
			this.infoBox.IsVisible = false;
			ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
			sleekLabel.SizeScale_X = 1f;
			sleekLabel.SizeOffset_Y = 30f;
			sleekLabel.Text = this.localization.format("No_Servers", Provider.APP_VERSION);
			sleekLabel.FontSize = 3;
			this.infoBox.AddChild(sleekLabel);
			ISleekLabel sleekLabel2 = Glazier.Get().CreateLabel();
			sleekLabel2.PositionOffset_Y = 20f;
			sleekLabel2.SizeScale_X = 1f;
			sleekLabel2.SizeOffset_Y = 30f;
			sleekLabel2.Text = this.localization.format("No_Servers_Hint");
			this.infoBox.AddChild(sleekLabel2);
			this.resetFiltersButton = Glazier.Get().CreateButton();
			this.resetFiltersButton.PositionOffset_X = -150f;
			this.resetFiltersButton.PositionOffset_Y = 10f;
			this.resetFiltersButton.PositionScale_X = 0.5f;
			this.resetFiltersButton.PositionScale_Y = 1f;
			this.resetFiltersButton.SizeOffset_X = 300f;
			this.resetFiltersButton.SizeOffset_Y = 30f;
			this.resetFiltersButton.Text = this.localization.format("Reset_Filters");
			this.resetFiltersButton.TooltipText = this.localization.format("Reset_Filters_Tooltip");
			this.resetFiltersButton.OnClicked += new ClickedButton(this.onClickedResetFilters);
			this.infoBox.AddChild(this.resetFiltersButton);
			TempSteamworksMatchmaking matchmakingService = Provider.provider.matchmakingService;
			matchmakingService.onMasterServerAdded = (TempSteamworksMatchmaking.MasterServerAdded)Delegate.Combine(matchmakingService.onMasterServerAdded, new TempSteamworksMatchmaking.MasterServerAdded(this.onMasterServerAdded));
			TempSteamworksMatchmaking matchmakingService2 = Provider.provider.matchmakingService;
			matchmakingService2.onMasterServerRemoved = (TempSteamworksMatchmaking.MasterServerRemoved)Delegate.Combine(matchmakingService2.onMasterServerRemoved, new TempSteamworksMatchmaking.MasterServerRemoved(this.onMasterServerRemoved));
			TempSteamworksMatchmaking matchmakingService3 = Provider.provider.matchmakingService;
			matchmakingService3.onMasterServerResorted = (TempSteamworksMatchmaking.MasterServerResorted)Delegate.Combine(matchmakingService3.onMasterServerResorted, new TempSteamworksMatchmaking.MasterServerResorted(this.onMasterServerResorted));
			TempSteamworksMatchmaking matchmakingService4 = Provider.provider.matchmakingService;
			matchmakingService4.onMasterServerRefreshed = (TempSteamworksMatchmaking.MasterServerRefreshed)Delegate.Combine(matchmakingService4.onMasterServerRefreshed, new TempSteamworksMatchmaking.MasterServerRefreshed(this.onMasterServerRefreshed));
			FilterSettings.OnActiveFiltersModified += new Action(this.OnActiveFiltersModified);
			FilterSettings.OnActiveFiltersReplaced += new Action(this.OnActiveFiltersReplaced);
			FilterSettings.OnCustomPresetsListChanged += new Action(this.OnCustomPresetsListChanged);
			this.refreshButton = Glazier.Get().CreateButton();
			this.refreshButton.PositionOffset_X = -200f;
			this.refreshButton.PositionOffset_Y = -50f;
			this.refreshButton.PositionScale_X = 1f;
			this.refreshButton.PositionScale_Y = 1f;
			this.refreshButton.SizeOffset_X = 200f;
			this.refreshButton.SizeOffset_Y = 50f;
			this.refreshButton.Text = this.localization.format("Refresh_Label");
			this.refreshButton.TooltipText = this.localization.format("Refresh_Tooltip");
			this.refreshButton.OnClicked += new ClickedButton(this.onClickedRefreshButton);
			this.refreshButton.FontSize = 3;
			base.AddChild(this.refreshButton);
			this.refreshIcon = Glazier.Get().CreateImage(this.icons.load<Texture2D>("Refresh"));
			this.refreshIcon.PositionOffset_X = 5f;
			this.refreshIcon.PositionOffset_Y = 5f;
			this.refreshIcon.SizeOffset_X = 40f;
			this.refreshIcon.SizeOffset_Y = 40f;
			this.refreshIcon.CanRotate = true;
			this.refreshIcon.TintColor = 2;
			this.refreshButton.AddChild(this.refreshIcon);
			ISleekElement sleekElement = Glazier.Get().CreateFrame();
			sleekElement.PositionOffset_X = 205f;
			sleekElement.PositionOffset_Y = -50f;
			sleekElement.PositionScale_Y = 1f;
			sleekElement.SizeOffset_X = -410f;
			sleekElement.SizeScale_X = 1f;
			sleekElement.SizeOffset_Y = 50f;
			base.AddChild(sleekElement);
			ISleekImage sleekImage4 = Glazier.Get().CreateImage();
			sleekImage4.PositionOffset_Y = -61f;
			sleekImage4.PositionScale_Y = 1f;
			sleekImage4.SizeScale_X = 1f;
			sleekImage4.SizeOffset_Y = 2f;
			sleekImage4.Texture = GlazierResources.PixelTexture;
			sleekImage4.TintColor = new SleekColor(2, 0.5f);
			base.AddChild(sleekImage4);
			SleekButtonIcon sleekButtonIcon2 = new SleekButtonIcon(this.icons.load<Texture2D>("Hosting"));
			sleekButtonIcon2.PositionOffset_X = 5f;
			sleekButtonIcon2.SizeOffset_X = -10f;
			sleekButtonIcon2.SizeOffset_Y = 50f;
			sleekButtonIcon2.SizeScale_X = 0.25f;
			sleekButtonIcon2.text = this.localization.format("HostingButtonText");
			sleekButtonIcon2.tooltip = this.localization.format("HostingButtonTooltip");
			sleekButtonIcon2.onClickedButton += new ClickedButton(this.onClickedHostingButton);
			sleekButtonIcon2.fontSize = 3;
			sleekButtonIcon2.iconColor = 2;
			sleekElement.AddChild(sleekButtonIcon2);
			this.presetsButton = new SleekButtonIcon(this.icons.load<Texture2D>("Presets"), 40);
			this.presetsButton.PositionOffset_X = 5f;
			this.presetsButton.PositionScale_X = 0.25f;
			this.presetsButton.SizeOffset_X = -10f;
			this.presetsButton.SizeOffset_Y = 50f;
			this.presetsButton.SizeScale_X = 0.25f;
			this.presetsButton.tooltip = this.localization.format("ViewPresetsButton_Tooltip");
			this.presetsButton.onClickedButton += new ClickedButton(this.onClickedPresetsButton);
			this.presetsButton.fontSize = 3;
			this.presetsButton.iconColor = 2;
			sleekElement.AddChild(this.presetsButton);
			this.SynchronizePresetsButtonLabel();
			this.quickFiltersButton = new SleekButtonIcon(this.icons.load<Texture2D>("Filters"), 40);
			this.quickFiltersButton.PositionOffset_X = 5f;
			this.quickFiltersButton.PositionScale_X = 0.5f;
			this.quickFiltersButton.SizeOffset_X = -10f;
			this.quickFiltersButton.SizeOffset_Y = 50f;
			this.quickFiltersButton.SizeScale_X = 0.25f;
			this.quickFiltersButton.tooltip = this.localization.format("QuickFiltersButton_Tooltip");
			this.quickFiltersButton.onClickedButton += new ClickedButton(this.OnQuickFiltersButtonClicked);
			this.quickFiltersButton.fontSize = 3;
			this.quickFiltersButton.iconColor = 2;
			sleekElement.AddChild(this.quickFiltersButton);
			this.SynchronizeQuickFiltersButtonLabel();
			this.presetsEditorButton = new SleekButtonIcon(this.icons.load<Texture2D>("PresetsEditor"), 40);
			this.presetsEditorButton.PositionOffset_X = 5f;
			this.presetsEditorButton.PositionScale_X = 0.75f;
			this.presetsEditorButton.SizeOffset_X = -10f;
			this.presetsEditorButton.SizeOffset_Y = 50f;
			this.presetsEditorButton.SizeScale_X = 0.25f;
			this.presetsEditorButton.tooltip = this.localization.format("PresetsEditorButton_Tooltip");
			this.presetsEditorButton.onClickedButton += new ClickedButton(this.OnPresetsEditorButtonClicked);
			this.presetsEditorButton.fontSize = 3;
			this.presetsEditorButton.iconColor = 2;
			sleekElement.AddChild(this.presetsEditorButton);
			this.serverBox = new SleekList<SteamServerAdvertisement>();
			this.serverBox.PositionOffset_Y = 50f;
			this.serverBox.SizeOffset_Y = -120f;
			this.serverBox.SizeScale_X = 1f;
			this.serverBox.SizeScale_Y = 1f;
			this.serverBox.itemHeight = 40;
			this.serverBox.scrollView.ReduceWidthWhenScrollbarVisible = false;
			this.serverBox.onCreateElement = new SleekList<SteamServerAdvertisement>.CreateElement(this.onCreateServerElement);
			this.serverBox.SetData(Provider.provider.matchmakingService.serverList);
			this.mainListContainer.AddChild(this.serverBox);
			this.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			this.backButton.PositionOffset_Y = -50f;
			this.backButton.PositionScale_Y = 1f;
			this.backButton.SizeOffset_X = 200f;
			this.backButton.SizeOffset_Y = 50f;
			this.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			this.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			this.backButton.onClickedButton += new ClickedButton(this.onClickedBackButton);
			this.backButton.fontSize = 3;
			this.backButton.iconColor = 2;
			base.AddChild(this.backButton);
			this.SynchronizeVisibleColumns();
			this.SynchronizePresetsList();
			this.SynchronizePresetsEditorButtonLabel();
			MenuPlayServersUI.mapFiltersUI = new MenuPlayMapFiltersUI(this);
			MenuPlayServersUI.mapFiltersUI.PositionOffset_X = 10f;
			MenuPlayServersUI.mapFiltersUI.PositionOffset_Y = 10f;
			MenuPlayServersUI.mapFiltersUI.PositionScale_Y = 1f;
			MenuPlayServersUI.mapFiltersUI.SizeOffset_X = -20f;
			MenuPlayServersUI.mapFiltersUI.SizeOffset_Y = -20f;
			MenuPlayServersUI.mapFiltersUI.SizeScale_X = 1f;
			MenuPlayServersUI.mapFiltersUI.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuPlayServersUI.mapFiltersUI);
			MenuPlayServersUI.serverListFiltersUI = new MenuPlayServerListFiltersUI(this);
			MenuPlayServersUI.serverListFiltersUI.PositionOffset_X = 10f;
			MenuPlayServersUI.serverListFiltersUI.PositionOffset_Y = 10f;
			MenuPlayServersUI.serverListFiltersUI.PositionScale_Y = 1f;
			MenuPlayServersUI.serverListFiltersUI.SizeOffset_X = -20f;
			MenuPlayServersUI.serverListFiltersUI.SizeOffset_Y = -20f;
			MenuPlayServersUI.serverListFiltersUI.SizeScale_X = 1f;
			MenuPlayServersUI.serverListFiltersUI.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuPlayServersUI.serverListFiltersUI);
		}

		// Token: 0x04002A18 RID: 10776
		public Local localization;

		// Token: 0x04002A19 RID: 10777
		public Bundle icons;

		/// <summary>
		/// Contains presetsScrollView which contains customPresetsContainer and defaultPresetsContainer.
		/// </summary>
		// Token: 0x04002A1A RID: 10778
		private ISleekElement presetsContainer;

		// Token: 0x04002A1B RID: 10779
		private ISleekScrollView presetsScrollView;

		// Token: 0x04002A1C RID: 10780
		private ISleekElement customPresetsContainer;

		// Token: 0x04002A1D RID: 10781
		private ISleekElement defaultPresetsContainer;

		// Token: 0x04002A1E RID: 10782
		private ISleekElement columnTogglesContainer;

		// Token: 0x04002A1F RID: 10783
		private ISleekElement filtersEditorContainer;

		/// <summary>
		/// Contains column buttons and server list itself.
		/// </summary>
		// Token: 0x04002A20 RID: 10784
		private ISleekElement mainListContainer;

		// Token: 0x04002A21 RID: 10785
		public bool active;

		// Token: 0x04002A22 RID: 10786
		private SleekButtonIcon backButton;

		// Token: 0x04002A23 RID: 10787
		private SleekList<SteamServerAdvertisement> serverBox;

		// Token: 0x04002A24 RID: 10788
		private ISleekBox infoBox;

		// Token: 0x04002A25 RID: 10789
		private ISleekButton resetFiltersButton;

		// Token: 0x04002A26 RID: 10790
		private ISleekButton nameColumnButton;

		// Token: 0x04002A27 RID: 10791
		private ISleekButton mapColumnButton;

		// Token: 0x04002A28 RID: 10792
		private ISleekButton playersColumnButton;

		// Token: 0x04002A29 RID: 10793
		private ISleekButton maxPlayersColumnButton;

		// Token: 0x04002A2A RID: 10794
		private ISleekButton fullnessColumnButton;

		// Token: 0x04002A2B RID: 10795
		private ISleekButton pingColumnButton;

		// Token: 0x04002A2C RID: 10796
		private ISleekButton anticheatColumnButton;

		// Token: 0x04002A2D RID: 10797
		private SleekButtonIcon perspectiveColumnButton;

		// Token: 0x04002A2E RID: 10798
		private SleekButtonIcon combatColumnButton;

		// Token: 0x04002A2F RID: 10799
		private SleekButtonIcon passwordColumnButton;

		// Token: 0x04002A30 RID: 10800
		private SleekButtonIcon workshopColumnButton;

		// Token: 0x04002A31 RID: 10801
		private SleekButtonIcon goldColumnButton;

		// Token: 0x04002A32 RID: 10802
		private SleekButtonIcon cheatsColumnButton;

		// Token: 0x04002A33 RID: 10803
		private SleekButtonIcon monetizationColumnButton;

		// Token: 0x04002A34 RID: 10804
		private SleekButtonIcon pluginsColumnButton;

		// Token: 0x04002A35 RID: 10805
		private ISleekField nameField;

		// Token: 0x04002A36 RID: 10806
		private SleekButtonIcon mapButton;

		// Token: 0x04002A37 RID: 10807
		private SleekButtonState monetizationButtonState;

		// Token: 0x04002A38 RID: 10808
		private SleekButtonState passwordButtonState;

		// Token: 0x04002A39 RID: 10809
		private SleekButtonState workshopButtonState;

		// Token: 0x04002A3A RID: 10810
		private SleekButtonState pluginsButtonState;

		// Token: 0x04002A3B RID: 10811
		private SleekButtonState cheatsButtonState;

		// Token: 0x04002A3C RID: 10812
		private SleekButtonState attendanceButtonState;

		// Token: 0x04002A3D RID: 10813
		private SleekButtonState notFullButtonState;

		// Token: 0x04002A3E RID: 10814
		private SleekButtonState VACProtectionButtonState;

		// Token: 0x04002A3F RID: 10815
		private SleekButtonState battlEyeProtectionButtonState;

		// Token: 0x04002A40 RID: 10816
		private SleekButtonState combatButtonState;

		// Token: 0x04002A41 RID: 10817
		private SleekButtonState goldFilterButtonState;

		// Token: 0x04002A42 RID: 10818
		private SleekButtonState cameraButtonState;

		// Token: 0x04002A43 RID: 10819
		private SleekButtonState listSourceButtonState;

		// Token: 0x04002A44 RID: 10820
		private ISleekInt32Field maxPingField;

		// Token: 0x04002A45 RID: 10821
		private SleekButtonIcon filtersVisibilityButton;

		// Token: 0x04002A46 RID: 10822
		private ISleekButton openFiltersVisibilityButton;

		// Token: 0x04002A47 RID: 10823
		private ISleekButton closeFiltersVisibilityButton;

		// Token: 0x04002A48 RID: 10824
		private ISleekToggle listSourceToggle;

		// Token: 0x04002A49 RID: 10825
		private ISleekToggle nameToggle;

		// Token: 0x04002A4A RID: 10826
		private ISleekToggle mapToggle;

		// Token: 0x04002A4B RID: 10827
		private ISleekToggle passwordToggle;

		// Token: 0x04002A4C RID: 10828
		private ISleekToggle attendanceToggle;

		// Token: 0x04002A4D RID: 10829
		private ISleekToggle notFullToggle;

		// Token: 0x04002A4E RID: 10830
		private ISleekToggle combatToggle;

		// Token: 0x04002A4F RID: 10831
		private ISleekToggle cameraToggle;

		// Token: 0x04002A50 RID: 10832
		private ISleekToggle goldToggle;

		// Token: 0x04002A51 RID: 10833
		private ISleekToggle monetizationToggle;

		// Token: 0x04002A52 RID: 10834
		private ISleekToggle workshopToggle;

		// Token: 0x04002A53 RID: 10835
		private ISleekToggle pluginsToggle;

		// Token: 0x04002A54 RID: 10836
		private ISleekToggle cheatsToggle;

		// Token: 0x04002A55 RID: 10837
		private ISleekToggle vacToggle;

		// Token: 0x04002A56 RID: 10838
		private ISleekToggle battlEyeToggle;

		// Token: 0x04002A57 RID: 10839
		private ISleekToggle maxPingToggle;

		// Token: 0x04002A58 RID: 10840
		private ISleekButton refreshButton;

		// Token: 0x04002A59 RID: 10841
		private SleekButtonIcon presetsButton;

		// Token: 0x04002A5A RID: 10842
		private SleekButtonIcon quickFiltersButton;

		// Token: 0x04002A5B RID: 10843
		private ISleekImage refreshIcon;

		// Token: 0x04002A5C RID: 10844
		private SleekButtonIcon presetsEditorButton;

		// Token: 0x04002A5D RID: 10845
		private bool isRefreshing;

		// Token: 0x04002A5E RID: 10846
		public static MenuPlayMapFiltersUI mapFiltersUI;

		// Token: 0x04002A5F RID: 10847
		public static MenuPlayServerListFiltersUI serverListFiltersUI;
	}
}
