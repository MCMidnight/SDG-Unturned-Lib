using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007A2 RID: 1954
	public class MenuPlayServerListFiltersUI : SleekFullscreenBox
	{
		// Token: 0x060040F8 RID: 16632 RVA: 0x00154F1E File Offset: 0x0015311E
		public void open()
		{
			if (this.active)
			{
				return;
			}
			this.active = true;
			this.SynchronizeFilterButtons();
			base.AnimateIntoView();
		}

		// Token: 0x060040F9 RID: 16633 RVA: 0x00154F3C File Offset: 0x0015313C
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

		// Token: 0x060040FA RID: 16634 RVA: 0x00154F64 File Offset: 0x00153164
		public void OpenForMap(string map)
		{
			LevelInfo level = Level.getLevel(map);
			FilterSettings.activeFilters.ClearMaps();
			FilterSettings.activeFilters.ToggleMap(level);
			FilterSettings.MarkActiveFilterModified();
			this.open();
		}

		/// <summary>
		/// Synchronize widgets with their values.
		/// </summary>
		// Token: 0x060040FB RID: 16635 RVA: 0x00154F9C File Offset: 0x0015319C
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

		// Token: 0x060040FC RID: 16636 RVA: 0x00155128 File Offset: 0x00153328
		private void SynchronizeDeletePresetButtonVisible()
		{
			this.deletePresetButton.IsVisible = (FilterSettings.activeFilters.presetId > 0);
			if (this.deletePresetButton.IsVisible)
			{
				this.filtersScrollView.ContentSizeOffset = new Vector2(0f, this.deletePresetButton.PositionOffset_Y + this.deletePresetButton.SizeOffset_Y);
				return;
			}
			this.filtersScrollView.ContentSizeOffset = new Vector2(0f, this.presetNameField.PositionOffset_Y + this.presetNameField.SizeOffset_Y);
		}

		// Token: 0x060040FD RID: 16637 RVA: 0x001551B3 File Offset: 0x001533B3
		private void onTypedNameField(ISleekField field, string text)
		{
			FilterSettings.activeFilters.serverName = text;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x060040FE RID: 16638 RVA: 0x001551C5 File Offset: 0x001533C5
		private void OnClickedMapButton(ISleekElement button)
		{
			MenuPlayServersUI.mapFiltersUI.open(EMenuPlayMapFiltersUIOpenContext.Filters);
			this.close();
		}

		// Token: 0x060040FF RID: 16639 RVA: 0x001551D8 File Offset: 0x001533D8
		private void onSwappedMonetizationState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.monetization = index + EServerMonetizationTag.Any;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x06004100 RID: 16640 RVA: 0x001551EC File Offset: 0x001533EC
		private void onSwappedPasswordState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.password = (EPassword)index;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x06004101 RID: 16641 RVA: 0x001551FE File Offset: 0x001533FE
		private void onSwappedWorkshopState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.workshop = (EWorkshop)index;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x06004102 RID: 16642 RVA: 0x00155210 File Offset: 0x00153410
		private void onSwappedPluginsState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.plugins = (EPlugins)index;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x06004103 RID: 16643 RVA: 0x00155222 File Offset: 0x00153422
		private void onSwappedCheatsState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.cheats = (ECheats)index;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x06004104 RID: 16644 RVA: 0x00155234 File Offset: 0x00153434
		private void onSwappedAttendanceState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.attendance = (EAttendance)index;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x06004105 RID: 16645 RVA: 0x00155246 File Offset: 0x00153446
		private void OnSwappedNotFullState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.notFull = (index > 0);
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x06004106 RID: 16646 RVA: 0x0015525B File Offset: 0x0015345B
		private void onSwappedVACProtectionState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.vacProtection = (EVACProtectionFilter)index;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x06004107 RID: 16647 RVA: 0x0015526D File Offset: 0x0015346D
		private void onSwappedBattlEyeProtectionState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.battlEyeProtection = (EBattlEyeProtectionFilter)index;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x06004108 RID: 16648 RVA: 0x0015527F File Offset: 0x0015347F
		private void OnMaxPingChanged(ISleekInt32Field field, int value)
		{
			FilterSettings.activeFilters.maxPing = value;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x06004109 RID: 16649 RVA: 0x00155291 File Offset: 0x00153491
		private void onSwappedCombatState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.combat = (ECombat)index;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x0600410A RID: 16650 RVA: 0x001552A3 File Offset: 0x001534A3
		private void OnSwappedGoldFilterState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.gold = (EServerListGoldFilter)index;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x0600410B RID: 16651 RVA: 0x001552B5 File Offset: 0x001534B5
		private void onSwappedCameraState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.camera = (ECameraMode)index;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x0600410C RID: 16652 RVA: 0x001552C7 File Offset: 0x001534C7
		private void OnSwappedListSourceState(SleekButtonState button, int index)
		{
			FilterSettings.activeFilters.listSource = (ESteamServerList)index;
			FilterSettings.MarkActiveFilterModified();
		}

		// Token: 0x0600410D RID: 16653 RVA: 0x001552DC File Offset: 0x001534DC
		private void OnClickedCreatePreset(ISleekElement button)
		{
			if (string.IsNullOrWhiteSpace(this.presetNameField.Text))
			{
				return;
			}
			FilterSettings.activeFilters.presetName = this.presetNameField.Text.Trim();
			FilterSettings.activeFilters.presetId = FilterSettings.CreatePresetId();
			this.presetNameField.Text = string.Empty;
			ServerListFilters serverListFilters = new ServerListFilters();
			serverListFilters.CopyFrom(FilterSettings.activeFilters);
			FilterSettings.customPresets.Add(serverListFilters);
			FilterSettings.customPresets.Sort(delegate(ServerListFilters lhs, ServerListFilters rhs)
			{
				if (!string.IsNullOrEmpty(lhs.presetName) && !string.IsNullOrEmpty(rhs.presetName))
				{
					return lhs.presetName.CompareTo(rhs.presetName);
				}
				return 0;
			});
			FilterSettings.InvokeActiveFiltersReplaced();
			FilterSettings.InvokeCustomFiltersListChanged();
		}

		// Token: 0x0600410E RID: 16654 RVA: 0x00155384 File Offset: 0x00153584
		private void OnClickedDeletePreset(ISleekElement button)
		{
			if (FilterSettings.activeFilters.presetId <= 0)
			{
				return;
			}
			FilterSettings.RemovePreset(FilterSettings.activeFilters.presetId);
			FilterSettings.activeFilters.presetName = string.Empty;
			FilterSettings.activeFilters.presetId = -1;
			FilterSettings.InvokeActiveFiltersReplaced();
			FilterSettings.InvokeCustomFiltersListChanged();
		}

		// Token: 0x0600410F RID: 16655 RVA: 0x001553D2 File Offset: 0x001535D2
		private void onClickedBackButton(ISleekElement button)
		{
			MenuPlayUI.serverListUI.open(true);
			this.close();
		}

		// Token: 0x06004110 RID: 16656 RVA: 0x001553E8 File Offset: 0x001535E8
		private void SynchronizePresetTitle()
		{
			if (string.IsNullOrEmpty(FilterSettings.activeFilters.presetName))
			{
				this.filtersTitleBox.Text = this.localization.format("PresetName_Empty");
				return;
			}
			this.filtersTitleBox.Text = FilterSettings.activeFilters.presetName;
		}

		// Token: 0x06004111 RID: 16657 RVA: 0x00155438 File Offset: 0x00153638
		private void SynchronizePresetsList()
		{
			this.customPresetsContainer.RemoveAllChildren();
			float num = 0f;
			foreach (ServerListFilters preset in FilterSettings.customPresets)
			{
				SleekCustomServerListPresetButton sleekCustomServerListPresetButton = new SleekCustomServerListPresetButton(preset);
				sleekCustomServerListPresetButton.PositionOffset_Y = num;
				sleekCustomServerListPresetButton.SizeOffset_X = 200f;
				sleekCustomServerListPresetButton.SizeOffset_Y = 30f;
				this.customPresetsContainer.AddChild(sleekCustomServerListPresetButton);
				num += sleekCustomServerListPresetButton.SizeOffset_Y;
			}
			this.customPresetsContainer.SizeOffset_Y = num;
			if (num > 0f)
			{
				this.defaultPresetsContainer.PositionOffset_Y = this.customPresetsContainer.SizeOffset_Y + 10f;
			}
			else
			{
				this.defaultPresetsContainer.PositionOffset_Y = 0f;
			}
			this.presetsScrollView.ContentSizeOffset = new Vector2(0f, this.defaultPresetsContainer.PositionOffset_Y + this.defaultPresetsContainer.SizeOffset_Y);
		}

		// Token: 0x06004112 RID: 16658 RVA: 0x0015553C File Offset: 0x0015373C
		private void OnActiveFiltersModified()
		{
			this.SynchronizePresetTitle();
			this.SynchronizeDeletePresetButtonVisible();
		}

		// Token: 0x06004113 RID: 16659 RVA: 0x0015554A File Offset: 0x0015374A
		private void OnActiveFiltersReplaced()
		{
			this.SynchronizePresetTitle();
			this.SynchronizeFilterButtons();
			this.SynchronizeDeletePresetButtonVisible();
		}

		// Token: 0x06004114 RID: 16660 RVA: 0x0015555E File Offset: 0x0015375E
		private void OnCustomPresetsListChanged()
		{
			this.SynchronizePresetsList();
		}

		// Token: 0x06004115 RID: 16661 RVA: 0x00155566 File Offset: 0x00153766
		public override void OnDestroy()
		{
			base.OnDestroy();
			FilterSettings.OnActiveFiltersModified -= new Action(this.OnActiveFiltersModified);
			FilterSettings.OnActiveFiltersReplaced -= new Action(this.OnActiveFiltersReplaced);
			FilterSettings.OnCustomPresetsListChanged -= new Action(this.OnCustomPresetsListChanged);
		}

		// Token: 0x06004116 RID: 16662 RVA: 0x001555A4 File Offset: 0x001537A4
		public MenuPlayServerListFiltersUI(MenuPlayServersUI serverListUI)
		{
			this.localization = serverListUI.localization;
			this.icons = serverListUI.icons;
			this.active = false;
			ISleekElement sleekElement = Glazier.Get().CreateFrame();
			sleekElement.PositionOffset_X = -335f;
			sleekElement.PositionOffset_Y = 100f;
			sleekElement.PositionScale_X = 0.5f;
			sleekElement.SizeOffset_X = 230f;
			sleekElement.SizeOffset_Y = -200f;
			sleekElement.SizeScale_Y = 1f;
			base.AddChild(sleekElement);
			ISleekElement sleekElement2 = Glazier.Get().CreateFrame();
			sleekElement2.PositionOffset_X = -95f;
			sleekElement2.PositionOffset_Y = 100f;
			sleekElement2.PositionScale_X = 0.5f;
			sleekElement2.SizeOffset_X = 430f;
			sleekElement2.SizeOffset_Y = -200f;
			sleekElement2.SizeScale_Y = 1f;
			base.AddChild(sleekElement2);
			this.presetsTitleBox = Glazier.Get().CreateBox();
			this.presetsTitleBox.SizeOffset_X = 200f;
			this.presetsTitleBox.SizeOffset_Y = 50f;
			this.presetsTitleBox.FontSize = 3;
			this.presetsTitleBox.Text = this.localization.format("Presets_Label");
			this.presetsTitleBox.TooltipText = this.localization.format("Presets_Tooltip");
			sleekElement.AddChild(this.presetsTitleBox);
			this.presetsScrollView = Glazier.Get().CreateScrollView();
			this.presetsScrollView.PositionOffset_Y = 60f;
			this.presetsScrollView.SizeOffset_X = 230f;
			this.presetsScrollView.SizeOffset_Y = -60f;
			this.presetsScrollView.SizeScale_Y = 1f;
			this.presetsScrollView.ScaleContentToWidth = true;
			sleekElement.AddChild(this.presetsScrollView);
			this.customPresetsContainer = Glazier.Get().CreateFrame();
			this.customPresetsContainer.SizeScale_X = 1f;
			this.presetsScrollView.AddChild(this.customPresetsContainer);
			this.defaultPresetsContainer = Glazier.Get().CreateFrame();
			this.defaultPresetsContainer.SizeScale_X = 1f;
			this.presetsScrollView.AddChild(this.defaultPresetsContainer);
			float num = 0f;
			SleekDefaultServerListPresetButton sleekDefaultServerListPresetButton = new SleekDefaultServerListPresetButton(FilterSettings.defaultPresetInternet, this.localization, this.icons);
			sleekDefaultServerListPresetButton.PositionOffset_Y = num;
			sleekDefaultServerListPresetButton.SizeOffset_X = 200f;
			sleekDefaultServerListPresetButton.SizeOffset_Y = 30f;
			this.defaultPresetsContainer.AddChild(sleekDefaultServerListPresetButton);
			num += sleekDefaultServerListPresetButton.SizeOffset_Y + 0f;
			SleekDefaultServerListPresetButton sleekDefaultServerListPresetButton2 = new SleekDefaultServerListPresetButton(FilterSettings.defaultPresetLAN, this.localization, this.icons);
			sleekDefaultServerListPresetButton2.PositionOffset_Y = num;
			sleekDefaultServerListPresetButton2.SizeOffset_X = 200f;
			sleekDefaultServerListPresetButton2.SizeOffset_Y = 30f;
			this.defaultPresetsContainer.AddChild(sleekDefaultServerListPresetButton2);
			num += sleekDefaultServerListPresetButton2.SizeOffset_Y + 0f;
			SleekDefaultServerListPresetButton sleekDefaultServerListPresetButton3 = new SleekDefaultServerListPresetButton(FilterSettings.defaultPresetHistory, this.localization, this.icons);
			sleekDefaultServerListPresetButton3.PositionOffset_Y = num;
			sleekDefaultServerListPresetButton3.SizeOffset_X = 200f;
			sleekDefaultServerListPresetButton3.SizeOffset_Y = 30f;
			this.defaultPresetsContainer.AddChild(sleekDefaultServerListPresetButton3);
			num += sleekDefaultServerListPresetButton3.SizeOffset_Y + 0f;
			SleekDefaultServerListPresetButton sleekDefaultServerListPresetButton4 = new SleekDefaultServerListPresetButton(FilterSettings.defaultPresetFavorites, this.localization, this.icons);
			sleekDefaultServerListPresetButton4.PositionOffset_Y = num;
			sleekDefaultServerListPresetButton4.SizeOffset_X = 200f;
			sleekDefaultServerListPresetButton4.SizeOffset_Y = 30f;
			this.defaultPresetsContainer.AddChild(sleekDefaultServerListPresetButton4);
			num += sleekDefaultServerListPresetButton4.SizeOffset_Y + 0f;
			SleekDefaultServerListPresetButton sleekDefaultServerListPresetButton5 = new SleekDefaultServerListPresetButton(FilterSettings.defaultPresetFriends, this.localization, this.icons);
			sleekDefaultServerListPresetButton5.PositionOffset_Y = num;
			sleekDefaultServerListPresetButton5.SizeOffset_X = 200f;
			sleekDefaultServerListPresetButton5.SizeOffset_Y = 30f;
			this.defaultPresetsContainer.AddChild(sleekDefaultServerListPresetButton5);
			num += sleekDefaultServerListPresetButton5.SizeOffset_Y + 0f;
			this.defaultPresetsContainer.SizeOffset_Y = num;
			this.SynchronizePresetsList();
			this.filtersTitleBox = Glazier.Get().CreateBox();
			this.filtersTitleBox.SizeOffset_X = 400f;
			this.filtersTitleBox.SizeOffset_Y = 50f;
			this.filtersTitleBox.FontSize = 3;
			sleekElement2.AddChild(this.filtersTitleBox);
			this.filtersScrollView = Glazier.Get().CreateScrollView();
			this.filtersScrollView.PositionOffset_Y = 60f;
			this.filtersScrollView.SizeOffset_X = 430f;
			this.filtersScrollView.SizeOffset_Y = -60f;
			this.filtersScrollView.SizeScale_Y = 1f;
			this.filtersScrollView.ScaleContentToWidth = true;
			sleekElement2.AddChild(this.filtersScrollView);
			float num2 = 0f;
			this.listSourceButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("List_Internet_Label"), this.icons.load<Texture>("List_Internet"), this.localization.format("List_Internet_Tooltip")),
				new GUIContent(this.localization.format("List_LAN_Label"), this.icons.load<Texture>("List_LAN"), this.localization.format("List_LAN_Tooltip")),
				new GUIContent(this.localization.format("List_History_Label"), this.icons.load<Texture>("List_History"), this.localization.format("List_History_Tooltip")),
				new GUIContent(this.localization.format("List_Favorites_Label"), this.icons.load<Texture>("List_Favorites"), this.localization.format("List_Favorites_Tooltip")),
				new GUIContent(this.localization.format("List_Friends_Label"), this.icons.load<Texture2D>("List_Friends"), this.localization.format("List_Friends_Tooltip"))
			});
			this.listSourceButtonState.PositionOffset_Y = num2;
			this.listSourceButtonState.SizeOffset_X = 200f;
			this.listSourceButtonState.SizeOffset_Y = 30f;
			this.listSourceButtonState.onSwappedState = new SwappedState(this.OnSwappedListSourceState);
			this.listSourceButtonState.button.iconColor = 2;
			this.listSourceButtonState.UseContentTooltip = true;
			this.listSourceButtonState.AddLabel(this.localization.format("List_Label"), 1);
			this.filtersScrollView.AddChild(this.listSourceButtonState);
			num2 += this.listSourceButtonState.SizeOffset_Y + 10f;
			this.nameField = Glazier.Get().CreateStringField();
			this.nameField.PositionOffset_Y = num2;
			this.nameField.SizeOffset_X = 200f;
			this.nameField.SizeOffset_Y = 30f;
			this.nameField.PlaceholderText = this.localization.format("Name_Filter_Hint");
			this.nameField.OnTextChanged += new Typed(this.onTypedNameField);
			this.nameField.AddLabel(this.localization.format("Name_Filter_Label"), 1);
			this.nameField.TooltipText = this.localization.format("Name_Filter_Tooltip");
			this.filtersScrollView.AddChild(this.nameField);
			num2 += this.nameField.SizeOffset_Y + 10f;
			this.mapButton = new SleekButtonIcon(this.icons.load<Texture2D>("Map"), 20);
			this.mapButton.PositionOffset_Y = num2;
			this.mapButton.SizeOffset_X = 200f;
			this.mapButton.SizeOffset_Y = 30f;
			this.mapButton.AddLabel(this.localization.format("Map_Filter_Label"), 1);
			this.mapButton.onClickedButton += new ClickedButton(this.OnClickedMapButton);
			this.mapButton.iconColor = 2;
			this.filtersScrollView.AddChild(this.mapButton);
			num2 += this.mapButton.SizeOffset_Y + 10f;
			this.passwordButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("No_Password_Button"), this.icons.load<Texture2D>("NotPasswordProtected"), this.localization.format("Password_Filter_No_Tooltip")),
				new GUIContent(this.localization.format("Yes_Password_Button"), this.icons.load<Texture2D>("PasswordProtected"), this.localization.format("Password_Filter_Yes_Tooltip")),
				new GUIContent(this.localization.format("Any_Password_Button"), this.icons.load<Texture2D>("AnyFilter"), this.localization.format("Password_Filter_Any_Tooltip"))
			});
			this.passwordButtonState.PositionOffset_Y = num2;
			this.passwordButtonState.SizeOffset_X = 200f;
			this.passwordButtonState.SizeOffset_Y = 30f;
			this.passwordButtonState.onSwappedState = new SwappedState(this.onSwappedPasswordState);
			this.passwordButtonState.button.iconColor = 2;
			this.passwordButtonState.UseContentTooltip = true;
			this.passwordButtonState.AddLabel(this.localization.format("Password_Filter_Label"), 1);
			this.filtersScrollView.AddChild(this.passwordButtonState);
			num2 += this.passwordButtonState.SizeOffset_Y + 10f;
			this.attendanceButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("Empty_Button"), this.icons.load<Texture>("Empty"), this.localization.format("Attendance_Filter_Empty_Tooltip")),
				new GUIContent(this.localization.format("HasPlayers_Button"), this.icons.load<Texture>("HasPlayers"), this.localization.format("Attendance_Filter_HasPlayers_Tooltip")),
				new GUIContent(this.localization.format("Any_Attendance_Button"), this.icons.load<Texture2D>("AnyFilter"), this.localization.format("Attendance_Filter_Any_Tooltip"))
			});
			this.attendanceButtonState.PositionOffset_Y = num2;
			this.attendanceButtonState.SizeOffset_X = 200f;
			this.attendanceButtonState.SizeOffset_Y = 30f;
			this.attendanceButtonState.onSwappedState = new SwappedState(this.onSwappedAttendanceState);
			this.attendanceButtonState.button.iconColor = 2;
			this.attendanceButtonState.UseContentTooltip = true;
			this.attendanceButtonState.AddLabel(this.localization.format("Attendance_Filter_Label"), 1);
			this.filtersScrollView.AddChild(this.attendanceButtonState);
			num2 += this.attendanceButtonState.SizeOffset_Y + 10f;
			this.notFullButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("Any_Space_Button"), this.icons.load<Texture2D>("AnyFilter"), this.localization.format("Space_Filter_Any_Tooltip")),
				new GUIContent(this.localization.format("Space_Button"), this.icons.load<Texture>("Space"), this.localization.format("Space_Filter_HasSpace_Tooltip"))
			});
			this.notFullButtonState.PositionOffset_Y = num2;
			this.notFullButtonState.SizeOffset_X = 200f;
			this.notFullButtonState.SizeOffset_Y = 30f;
			this.notFullButtonState.onSwappedState = new SwappedState(this.OnSwappedNotFullState);
			this.notFullButtonState.button.iconColor = 2;
			this.notFullButtonState.UseContentTooltip = true;
			this.notFullButtonState.AddLabel(this.localization.format("Space_Filter_Label"), 1);
			this.filtersScrollView.AddChild(this.notFullButtonState);
			num2 += this.notFullButtonState.SizeOffset_Y + 10f;
			this.combatButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("PvP_Button"), this.icons.load<Texture>("PvP"), this.localization.format("Combat_Filter_PvP_Tooltip")),
				new GUIContent(this.localization.format("PvE_Button"), this.icons.load<Texture>("PvE"), this.localization.format("Combat_Filter_PvE_Tooltip")),
				new GUIContent(this.localization.format("Any_Combat_Button"), this.icons.load<Texture2D>("AnyFilter"), this.localization.format("Combat_Filter_Any_Tooltip"))
			});
			this.combatButtonState.PositionOffset_Y = num2;
			this.combatButtonState.SizeOffset_X = 200f;
			this.combatButtonState.SizeOffset_Y = 30f;
			this.combatButtonState.onSwappedState = new SwappedState(this.onSwappedCombatState);
			this.combatButtonState.button.iconColor = 2;
			this.combatButtonState.UseContentTooltip = true;
			this.combatButtonState.AddLabel(this.localization.format("Combat_Filter_Label"), 1);
			this.filtersScrollView.AddChild(this.combatButtonState);
			num2 += this.combatButtonState.SizeOffset_Y + 10f;
			this.cameraButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("First_Button"), this.icons.load<Texture>("Perspective_FirstPerson"), this.localization.format("Perspective_Filter_FirstPerson_Tooltip")),
				new GUIContent(this.localization.format("Third_Button"), this.icons.load<Texture>("Perspective_ThirdPerson"), this.localization.format("Perspective_Filter_ThirdPerson_Tooltip")),
				new GUIContent(this.localization.format("Both_Button"), this.icons.load<Texture>("Perspective_Both"), this.localization.format("Perspective_Filter_Both_Tooltip")),
				new GUIContent(this.localization.format("Vehicle_Button"), this.icons.load<Texture>("Perspective_Vehicle"), this.localization.format("Perspective_Filter_Vehicle_Tooltip")),
				new GUIContent(this.localization.format("Any_Camera_Button"), this.icons.load<Texture2D>("AnyFilter"), this.localization.format("Perspective_Filter_Any_Tooltip"))
			});
			this.cameraButtonState.PositionOffset_Y = num2;
			this.cameraButtonState.SizeOffset_X = 200f;
			this.cameraButtonState.SizeOffset_Y = 30f;
			this.cameraButtonState.onSwappedState = new SwappedState(this.onSwappedCameraState);
			this.cameraButtonState.button.iconColor = 2;
			this.cameraButtonState.UseContentTooltip = true;
			this.cameraButtonState.AddLabel(this.localization.format("Perspective_Filter_Label"), 1);
			this.filtersScrollView.AddChild(this.cameraButtonState);
			num2 += this.cameraButtonState.SizeOffset_Y + 10f;
			this.goldFilterButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("Gold_Filter_Any_Label"), this.icons.load<Texture>("AnyFilter"), this.localization.format("Gold_Filter_Any_Tooltip")),
				new GUIContent(this.localization.format("Gold_Filter_DoesNotRequireGold_Label"), this.icons.load<Texture>("GoldNotRequired"), this.localization.format("Gold_Filter_DoesNotRequireGold_Tooltip")),
				new GUIContent(this.localization.format("Gold_Filter_RequiresGold_Label"), this.icons.load<Texture>("GoldRequired"), this.localization.format("Gold_Filter_RequiresGold_Tooltip"))
			});
			this.goldFilterButtonState.PositionOffset_Y = num2;
			this.goldFilterButtonState.SizeOffset_X = 200f;
			this.goldFilterButtonState.SizeOffset_Y = 30f;
			this.goldFilterButtonState.UseContentTooltip = true;
			this.goldFilterButtonState.onSwappedState = new SwappedState(this.OnSwappedGoldFilterState);
			this.goldFilterButtonState.button.textColor = Palette.PRO;
			this.goldFilterButtonState.button.backgroundColor = SleekColor.BackgroundIfLight(Palette.PRO);
			this.goldFilterButtonState.button.iconColor = Palette.PRO;
			this.goldFilterButtonState.AddLabel(this.localization.format("Gold_Filter_Label"), 1);
			this.filtersScrollView.AddChild(this.goldFilterButtonState);
			num2 += this.goldFilterButtonState.SizeOffset_Y + 10f;
			this.monetizationButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("Monetization_Any_Button"), this.icons.load<Texture2D>("AnyFilter"), this.localization.format("Monetization_Filter_Any_Tooltip")),
				new GUIContent(this.localization.format("Monetization_None_Button"), this.icons.load<Texture2D>("Monetization_None"), this.localization.format("Monetization_Filter_None_Tooltip")),
				new GUIContent(this.localization.format("Monetization_NonGameplay_Button"), this.icons.load<Texture2D>("NonGameplayMonetization"), this.localization.format("Monetization_Filter_NonGameplay_Tooltip"))
			});
			this.monetizationButtonState.PositionOffset_Y = num2;
			this.monetizationButtonState.SizeOffset_X = 200f;
			this.monetizationButtonState.SizeOffset_Y = 30f;
			this.monetizationButtonState.onSwappedState = new SwappedState(this.onSwappedMonetizationState);
			this.monetizationButtonState.button.iconColor = 2;
			this.monetizationButtonState.UseContentTooltip = true;
			this.monetizationButtonState.AddLabel(this.localization.format("Monetization_Filter_Label"), 1);
			this.filtersScrollView.AddChild(this.monetizationButtonState);
			num2 += this.monetizationButtonState.SizeOffset_Y + 10f;
			this.workshopButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("No_Workshop_Button"), this.icons.load<Texture2D>("NoMods"), this.localization.format("Workshop_Filter_No_Tooltip")),
				new GUIContent(this.localization.format("Yes_Workshop_Button"), this.icons.load<Texture2D>("HasMods"), this.localization.format("Workshop_Filter_Yes_Tooltip")),
				new GUIContent(this.localization.format("Any_Workshop_Button"), this.icons.load<Texture2D>("AnyFilter"), this.localization.format("Workshop_Filter_Any_Tooltip"))
			});
			this.workshopButtonState.PositionOffset_Y = num2;
			this.workshopButtonState.SizeOffset_X = 200f;
			this.workshopButtonState.SizeOffset_Y = 30f;
			this.workshopButtonState.onSwappedState = new SwappedState(this.onSwappedWorkshopState);
			this.workshopButtonState.button.iconColor = 2;
			this.workshopButtonState.UseContentTooltip = true;
			this.workshopButtonState.AddLabel(this.localization.format("Workshop_Filter_Label"), 1);
			this.filtersScrollView.AddChild(this.workshopButtonState);
			num2 += this.workshopButtonState.SizeOffset_Y + 10f;
			this.pluginsButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("No_Plugins_Button"), this.icons.load<Texture2D>("Plugins_None"), this.localization.format("Plugins_Filter_No_Tooltip")),
				new GUIContent(this.localization.format("Yes_Plugins_Button"), this.icons.load<Texture2D>("Plugins"), this.localization.format("Plugins_Filter_Yes_Tooltip")),
				new GUIContent(this.localization.format("Any_Plugins_Button"), this.icons.load<Texture2D>("AnyFilter"), this.localization.format("Plugins_Filter_Any_Tooltip"))
			});
			this.pluginsButtonState.PositionOffset_Y = num2;
			this.pluginsButtonState.SizeOffset_X = 200f;
			this.pluginsButtonState.SizeOffset_Y = 30f;
			this.pluginsButtonState.onSwappedState = new SwappedState(this.onSwappedPluginsState);
			this.pluginsButtonState.button.iconColor = 2;
			this.pluginsButtonState.UseContentTooltip = true;
			this.pluginsButtonState.AddLabel(this.localization.format("Plugins_Filter_Label"), 1);
			this.filtersScrollView.AddChild(this.pluginsButtonState);
			num2 += this.pluginsButtonState.SizeOffset_Y + 10f;
			this.cheatsButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("No_Cheats_Button"), this.icons.load<Texture2D>("CheatCodes_None"), this.localization.format("Cheats_Filter_No_Tooltip")),
				new GUIContent(this.localization.format("Yes_Cheats_Button"), this.icons.load<Texture2D>("CheatCodes"), this.localization.format("Cheats_Filter_Yes_Tooltip")),
				new GUIContent(this.localization.format("Any_Cheats_Button"), this.icons.load<Texture2D>("AnyFilter"), this.localization.format("Cheats_Filter_Any_Tooltip"))
			});
			this.cheatsButtonState.PositionOffset_Y = num2;
			this.cheatsButtonState.SizeOffset_X = 200f;
			this.cheatsButtonState.SizeOffset_Y = 30f;
			this.cheatsButtonState.onSwappedState = new SwappedState(this.onSwappedCheatsState);
			this.cheatsButtonState.button.iconColor = 2;
			this.cheatsButtonState.UseContentTooltip = true;
			this.cheatsButtonState.AddLabel(this.localization.format("Cheats_Filter_Label"), 1);
			this.filtersScrollView.AddChild(this.cheatsButtonState);
			num2 += this.cheatsButtonState.SizeOffset_Y + 10f;
			this.VACProtectionButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("VAC_Secure_Button"), this.icons.load<Texture>("VAC"), this.localization.format("VAC_Filter_Secure_Tooltip")),
				new GUIContent(this.localization.format("VAC_Insecure_Button"), this.icons.load<Texture2D>("VAC_Off"), this.localization.format("VAC_Filter_Insecure_Tooltip")),
				new GUIContent(this.localization.format("VAC_Any_Button"), this.icons.load<Texture2D>("AnyFilter"), this.localization.format("VAC_Filter_Any_Tooltip"))
			});
			this.VACProtectionButtonState.PositionOffset_Y = num2;
			this.VACProtectionButtonState.SizeOffset_X = 200f;
			this.VACProtectionButtonState.SizeOffset_Y = 30f;
			this.VACProtectionButtonState.onSwappedState = new SwappedState(this.onSwappedVACProtectionState);
			this.VACProtectionButtonState.button.iconColor = 2;
			this.VACProtectionButtonState.UseContentTooltip = true;
			this.VACProtectionButtonState.AddLabel(this.localization.format("VAC_Filter_Label"), 1);
			this.filtersScrollView.AddChild(this.VACProtectionButtonState);
			num2 += this.VACProtectionButtonState.SizeOffset_Y + 10f;
			this.battlEyeProtectionButtonState = new SleekButtonState(20, new GUIContent[]
			{
				new GUIContent(this.localization.format("BattlEye_Secure_Button"), this.icons.load<Texture>("BattlEye"), this.localization.format("BattlEye_Filter_Secure_Tooltip")),
				new GUIContent(this.localization.format("BattlEye_Insecure_Button"), this.icons.load<Texture2D>("BattlEye_Off"), this.localization.format("BattlEye_Filter_Insecure_Tooltip")),
				new GUIContent(this.localization.format("BattlEye_Any_Button"), this.icons.load<Texture2D>("AnyFilter"), this.localization.format("BattlEye_Filter_Any_Tooltip"))
			});
			this.battlEyeProtectionButtonState.PositionOffset_Y = num2;
			this.battlEyeProtectionButtonState.SizeOffset_X = 200f;
			this.battlEyeProtectionButtonState.SizeOffset_Y = 30f;
			this.battlEyeProtectionButtonState.onSwappedState = new SwappedState(this.onSwappedBattlEyeProtectionState);
			this.battlEyeProtectionButtonState.button.iconColor = 2;
			this.battlEyeProtectionButtonState.UseContentTooltip = true;
			this.battlEyeProtectionButtonState.AddLabel(this.localization.format("BattlEye_Filter_Label"), 1);
			this.filtersScrollView.AddChild(this.battlEyeProtectionButtonState);
			num2 += this.battlEyeProtectionButtonState.SizeOffset_Y + 10f;
			this.maxPingField = Glazier.Get().CreateInt32Field();
			this.maxPingField.PositionOffset_Y = num2;
			this.maxPingField.SizeOffset_X = 200f;
			this.maxPingField.SizeOffset_Y = 30f;
			this.maxPingField.OnValueChanged += new TypedInt32(this.OnMaxPingChanged);
			this.maxPingField.AddLabel(this.localization.format("MaxPing_Filter_Label"), 1);
			this.maxPingField.TooltipText = this.localization.format("MaxPing_Filter_Tooltip");
			this.filtersScrollView.AddChild(this.maxPingField);
			num2 += this.maxPingField.SizeOffset_Y + 10f;
			num2 += 10f;
			num2 += 10f;
			this.presetNameField = Glazier.Get().CreateStringField();
			this.presetNameField.PositionOffset_Y = num2;
			this.presetNameField.SizeOffset_X = 200f;
			this.presetNameField.SizeOffset_Y = 30f;
			this.presetNameField.PlaceholderText = this.localization.format("PresetNameField_Hint");
			this.presetNameField.TooltipText = this.localization.format("PresetNameField_Tooltip");
			this.filtersScrollView.AddChild(this.presetNameField);
			SleekButtonIcon sleekButtonIcon = new SleekButtonIcon(this.icons.load<Texture2D>("NewPreset"), 20);
			sleekButtonIcon.PositionOffset_X = 200f;
			sleekButtonIcon.PositionOffset_Y = num2;
			sleekButtonIcon.SizeOffset_X = 200f;
			sleekButtonIcon.SizeOffset_Y = 30f;
			sleekButtonIcon.text = this.localization.format("NewPreset_Label");
			sleekButtonIcon.tooltip = this.localization.format("NewPreset_Tooltip");
			sleekButtonIcon.onClickedButton += new ClickedButton(this.OnClickedCreatePreset);
			sleekButtonIcon.iconColor = 2;
			this.filtersScrollView.AddChild(sleekButtonIcon);
			num2 += sleekButtonIcon.SizeOffset_Y + 10f;
			this.deletePresetButton = new SleekButtonIconConfirm(this.icons.load<Texture2D>("DeletePreset"), this.localization.format("DeletePreset_Confirm_Label"), this.localization.format("DeletePreset_Confirm_Tooltip"), this.localization.format("DeletePreset_Deny_Label"), this.localization.format("DeletePreset_Deny_Tooltip"), 20);
			this.deletePresetButton.PositionOffset_X = 100f;
			this.deletePresetButton.PositionOffset_Y = num2;
			this.deletePresetButton.SizeOffset_X = 200f;
			this.deletePresetButton.SizeOffset_Y = 30f;
			this.deletePresetButton.text = this.localization.format("DeletePreset_Label");
			this.deletePresetButton.tooltip = this.localization.format("DeletePreset_Tooltip");
			this.deletePresetButton.onConfirmed = new Confirm(this.OnClickedDeletePreset);
			this.deletePresetButton.iconColor = 2;
			this.filtersScrollView.AddChild(this.deletePresetButton);
			num2 += this.deletePresetButton.SizeOffset_Y + 10f;
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
			FilterSettings.OnActiveFiltersModified += new Action(this.OnActiveFiltersModified);
			FilterSettings.OnActiveFiltersReplaced += new Action(this.OnActiveFiltersReplaced);
			FilterSettings.OnCustomPresetsListChanged += new Action(this.OnCustomPresetsListChanged);
			this.SynchronizePresetTitle();
			this.SynchronizeFilterButtons();
			this.SynchronizeDeletePresetButtonVisible();
		}

		// Token: 0x040029FC RID: 10748
		public Local localization;

		// Token: 0x040029FD RID: 10749
		public Bundle icons;

		// Token: 0x040029FE RID: 10750
		public bool active;

		// Token: 0x040029FF RID: 10751
		private SleekButtonIcon backButton;

		// Token: 0x04002A00 RID: 10752
		private ISleekBox presetsTitleBox;

		// Token: 0x04002A01 RID: 10753
		private ISleekScrollView presetsScrollView;

		// Token: 0x04002A02 RID: 10754
		private ISleekElement customPresetsContainer;

		// Token: 0x04002A03 RID: 10755
		private ISleekElement defaultPresetsContainer;

		// Token: 0x04002A04 RID: 10756
		private ISleekBox filtersTitleBox;

		// Token: 0x04002A05 RID: 10757
		private ISleekScrollView filtersScrollView;

		// Token: 0x04002A06 RID: 10758
		private SleekButtonIconConfirm deletePresetButton;

		// Token: 0x04002A07 RID: 10759
		private ISleekField presetNameField;

		// Token: 0x04002A08 RID: 10760
		private ISleekField nameField;

		// Token: 0x04002A09 RID: 10761
		private SleekButtonIcon mapButton;

		// Token: 0x04002A0A RID: 10762
		private SleekButtonState monetizationButtonState;

		// Token: 0x04002A0B RID: 10763
		private SleekButtonState passwordButtonState;

		// Token: 0x04002A0C RID: 10764
		private SleekButtonState workshopButtonState;

		// Token: 0x04002A0D RID: 10765
		private SleekButtonState pluginsButtonState;

		// Token: 0x04002A0E RID: 10766
		private SleekButtonState cheatsButtonState;

		// Token: 0x04002A0F RID: 10767
		private SleekButtonState attendanceButtonState;

		// Token: 0x04002A10 RID: 10768
		private SleekButtonState notFullButtonState;

		// Token: 0x04002A11 RID: 10769
		private SleekButtonState VACProtectionButtonState;

		// Token: 0x04002A12 RID: 10770
		private SleekButtonState battlEyeProtectionButtonState;

		// Token: 0x04002A13 RID: 10771
		private SleekButtonState combatButtonState;

		// Token: 0x04002A14 RID: 10772
		private SleekButtonState goldFilterButtonState;

		// Token: 0x04002A15 RID: 10773
		private SleekButtonState cameraButtonState;

		// Token: 0x04002A16 RID: 10774
		private SleekButtonState listSourceButtonState;

		// Token: 0x04002A17 RID: 10775
		private ISleekInt32Field maxPingField;
	}
}
