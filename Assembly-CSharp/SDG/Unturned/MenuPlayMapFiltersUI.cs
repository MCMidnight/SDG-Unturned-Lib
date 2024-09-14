using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200079D RID: 1949
	public class MenuPlayMapFiltersUI : SleekFullscreenBox
	{
		// Token: 0x060040C0 RID: 16576 RVA: 0x00150AA4 File Offset: 0x0014ECA4
		public void open(EMenuPlayMapFiltersUIOpenContext openContext)
		{
			if (this.active)
			{
				return;
			}
			this.active = true;
			this.openContext = openContext;
			int widthForLayout = ScreenEx.GetWidthForLayout();
			if (this.levels == null || this.levels.Length < 1 || widthForLayout != this.previousLayoutWidth)
			{
				this.PopulateLevelButtons();
			}
			this.UpdateFiltersLabel();
			this.SynchronizeCheckBoxes();
			base.AnimateIntoView();
		}

		// Token: 0x060040C1 RID: 16577 RVA: 0x00150B02 File Offset: 0x0014ED02
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

		// Token: 0x060040C2 RID: 16578 RVA: 0x00150B2C File Offset: 0x0014ED2C
		public void OpenPreviousMenu()
		{
			EMenuPlayMapFiltersUIOpenContext emenuPlayMapFiltersUIOpenContext = this.openContext;
			if (emenuPlayMapFiltersUIOpenContext == EMenuPlayMapFiltersUIOpenContext.ServerList)
			{
				MenuPlayUI.serverListUI.open(true);
				return;
			}
			if (emenuPlayMapFiltersUIOpenContext != EMenuPlayMapFiltersUIOpenContext.Filters)
			{
				return;
			}
			MenuPlayServersUI.serverListFiltersUI.open();
		}

		// Token: 0x060040C3 RID: 16579 RVA: 0x00150B5E File Offset: 0x0014ED5E
		public override void OnDestroy()
		{
			Level.onLevelsRefreshed = (LevelsRefreshed)Delegate.Remove(Level.onLevelsRefreshed, new LevelsRefreshed(this.OnLevelsRefreshed));
			base.OnDestroy();
		}

		// Token: 0x060040C4 RID: 16580 RVA: 0x00150B88 File Offset: 0x0014ED88
		private void UpdateFiltersLabel()
		{
			string mapDisplayText = FilterSettings.activeFilters.GetMapDisplayText();
			if (string.IsNullOrEmpty(mapDisplayText))
			{
				this.filtersLabel.Text = this.localization.format("MapFilter_Button_EmptyLabel");
				this.resetButton.IsClickable = false;
				return;
			}
			this.filtersLabel.Text = mapDisplayText;
			this.resetButton.IsClickable = true;
		}

		// Token: 0x060040C5 RID: 16581 RVA: 0x00150BE8 File Offset: 0x0014EDE8
		private void OnClickedResetButton(ISleekElement button)
		{
			FilterSettings.activeFilters.ClearMaps();
			FilterSettings.MarkActiveFilterModified();
			this.UpdateFiltersLabel();
			this.SynchronizeCheckBoxes();
		}

		// Token: 0x060040C6 RID: 16582 RVA: 0x00150C05 File Offset: 0x0014EE05
		private void OnClickedBackButton(ISleekElement button)
		{
			this.OpenPreviousMenu();
			this.close();
		}

		// Token: 0x060040C7 RID: 16583 RVA: 0x00150C14 File Offset: 0x0014EE14
		private void OnClickedLevel(SleekLevel levelButton, byte index)
		{
			bool isIncludedInFilter = FilterSettings.activeFilters.ToggleMap(levelButton.level);
			((SleekFilterLevel)levelButton).IsIncludedInFilter = isIncludedInFilter;
			FilterSettings.MarkActiveFilterModified();
			this.UpdateFiltersLabel();
		}

		// Token: 0x060040C8 RID: 16584 RVA: 0x00150C4C File Offset: 0x0014EE4C
		private void PopulateLevelButtons()
		{
			int widthForLayout = ScreenEx.GetWidthForLayout();
			this.previousLayoutWidth = widthForLayout;
			this.levelScrollBox.RemoveAllChildren();
			this.levels = Level.getLevels(ESingleplayerMapCategory.ALL);
			int num = Mathf.Max(1, (widthForLayout - 200) / 410);
			int num2 = 0;
			int num3 = 0;
			this.levelButtons = new SleekFilterLevel[this.levels.Length];
			for (int i = 0; i < this.levels.Length; i++)
			{
				if (this.levels[i] != null)
				{
					SleekFilterLevel sleekFilterLevel = new SleekFilterLevel(this.levels[i]);
					sleekFilterLevel.PositionOffset_X = (float)(num2 % num * 410);
					sleekFilterLevel.PositionOffset_Y = (float)(num2 / num * 110);
					SleekFilterLevel sleekFilterLevel2 = sleekFilterLevel;
					sleekFilterLevel2.onClickedLevel = (ClickedLevel)Delegate.Combine(sleekFilterLevel2.onClickedLevel, new ClickedLevel(this.OnClickedLevel));
					this.levelScrollBox.AddChild(sleekFilterLevel);
					num3 += 110;
					this.levelButtons[i] = sleekFilterLevel;
					num2++;
				}
			}
			float num4 = (float)(MathfEx.GetPageCount(num2, num) * 110);
			this.levelScrollBox.ContentSizeOffset = new Vector2(0f, num4 - 10f);
			int num5 = num * 410 - 10;
			this.headerBox.PositionOffset_X = (float)(-(float)num5 / 2);
			this.headerBox.SizeOffset_X = (float)num5;
			this.resetButton.PositionOffset_X = (float)(-(float)num5 / 2);
			this.resetButton.SizeOffset_X = (float)num5;
			this.levelScrollBox.PositionOffset_X = (float)(-(float)num5 / 2);
			this.levelScrollBox.SizeOffset_X = (float)(num5 + 30);
		}

		// Token: 0x060040C9 RID: 16585 RVA: 0x00150DDC File Offset: 0x0014EFDC
		private void SynchronizeCheckBoxes()
		{
			if (this.levelButtons == null)
			{
				return;
			}
			List<LevelInfo> list = new List<LevelInfo>();
			FilterSettings.activeFilters.GetLevels(list);
			foreach (SleekFilterLevel sleekFilterLevel in this.levelButtons)
			{
				bool isIncludedInFilter = list.Contains(sleekFilterLevel.level);
				sleekFilterLevel.IsIncludedInFilter = isIncludedInFilter;
			}
		}

		// Token: 0x060040CA RID: 16586 RVA: 0x00150E32 File Offset: 0x0014F032
		private void OnLevelsRefreshed()
		{
			this.PopulateLevelButtons();
		}

		// Token: 0x060040CB RID: 16587 RVA: 0x00150E3C File Offset: 0x0014F03C
		public MenuPlayMapFiltersUI(MenuPlayServersUI serverListUI)
		{
			this.localization = serverListUI.localization;
			this.icons = serverListUI.icons;
			this.active = false;
			this.headerBox = Glazier.Get().CreateBox();
			this.headerBox.PositionOffset_Y = 100f;
			this.headerBox.PositionScale_X = 0.5f;
			this.headerBox.SizeOffset_Y = 100f;
			this.headerBox.TooltipText = this.localization.format("MapFilter_Header_Tooltip");
			base.AddChild(this.headerBox);
			this.titleLabel = Glazier.Get().CreateLabel();
			this.titleLabel.SizeScale_X = 1f;
			this.titleLabel.SizeOffset_Y = 50f;
			this.titleLabel.Text = this.localization.format("MapFilter_Header_Label");
			this.titleLabel.FontSize = 3;
			this.headerBox.AddChild(this.titleLabel);
			this.filtersLabel = Glazier.Get().CreateLabel();
			this.filtersLabel.SizeScale_X = 1f;
			this.filtersLabel.PositionOffset_Y = 50f;
			this.filtersLabel.SizeOffset_Y = 50f;
			this.headerBox.AddChild(this.filtersLabel);
			this.resetButton = Glazier.Get().CreateButton();
			this.resetButton.PositionOffset_Y = 210f;
			this.resetButton.PositionScale_X = 0.5f;
			this.resetButton.SizeOffset_Y = 50f;
			this.resetButton.Text = this.localization.format("MapFilter_ResetButton_Label");
			this.resetButton.TooltipText = this.localization.format("MapFilter_ResetButton_Tooltip");
			this.resetButton.FontSize = 3;
			this.resetButton.OnClicked += new ClickedButton(this.OnClickedResetButton);
			base.AddChild(this.resetButton);
			this.levelScrollBox = Glazier.Get().CreateScrollView();
			this.levelScrollBox.PositionOffset_Y = 270f;
			this.levelScrollBox.PositionScale_X = 0.5f;
			this.levelScrollBox.SizeOffset_Y = -370f;
			this.levelScrollBox.SizeScale_Y = 1f;
			this.levelScrollBox.ScaleContentToWidth = true;
			base.AddChild(this.levelScrollBox);
			this.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			this.backButton.PositionOffset_Y = -50f;
			this.backButton.PositionScale_Y = 1f;
			this.backButton.SizeOffset_X = 200f;
			this.backButton.SizeOffset_Y = 50f;
			this.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			this.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			this.backButton.onClickedButton += new ClickedButton(this.OnClickedBackButton);
			this.backButton.fontSize = 3;
			this.backButton.iconColor = 2;
			base.AddChild(this.backButton);
			Level.onLevelsRefreshed = (LevelsRefreshed)Delegate.Combine(Level.onLevelsRefreshed, new LevelsRefreshed(this.OnLevelsRefreshed));
		}

		// Token: 0x040029A1 RID: 10657
		public Local localization;

		// Token: 0x040029A2 RID: 10658
		public Bundle icons;

		// Token: 0x040029A3 RID: 10659
		public bool active;

		// Token: 0x040029A4 RID: 10660
		private EMenuPlayMapFiltersUIOpenContext openContext;

		// Token: 0x040029A5 RID: 10661
		private LevelInfo[] levels;

		// Token: 0x040029A6 RID: 10662
		private SleekFilterLevel[] levelButtons;

		// Token: 0x040029A7 RID: 10663
		private int previousLayoutWidth = -1;

		// Token: 0x040029A8 RID: 10664
		private ISleekBox headerBox;

		// Token: 0x040029A9 RID: 10665
		private ISleekLabel titleLabel;

		// Token: 0x040029AA RID: 10666
		private ISleekLabel filtersLabel;

		// Token: 0x040029AB RID: 10667
		private ISleekButton resetButton;

		// Token: 0x040029AC RID: 10668
		private ISleekScrollView levelScrollBox;

		// Token: 0x040029AD RID: 10669
		private SleekButtonIcon backButton;
	}
}
