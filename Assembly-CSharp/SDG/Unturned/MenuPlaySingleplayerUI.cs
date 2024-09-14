using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007A4 RID: 1956
	public class MenuPlaySingleplayerUI
	{
		// Token: 0x0600417A RID: 16762 RVA: 0x0015D42E File Offset: 0x0015B62E
		public static void open()
		{
			if (MenuPlaySingleplayerUI.active)
			{
				return;
			}
			MenuPlaySingleplayerUI.active = true;
			MenuPlaySingleplayerUI.browseServersButton.IsVisible = !OptionsSettings.ShouldShowOnlineSafetyMenu;
			MenuPlaySingleplayerUI.container.AnimateIntoView();
		}

		// Token: 0x0600417B RID: 16763 RVA: 0x0015D45A File Offset: 0x0015B65A
		public static void close()
		{
			if (!MenuPlaySingleplayerUI.active)
			{
				return;
			}
			MenuPlaySingleplayerUI.active = false;
			MenuSettings.save();
			MenuPlaySingleplayerUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x0600417C RID: 16764 RVA: 0x0015D484 File Offset: 0x0015B684
		private static void updateSelection()
		{
			if (string.IsNullOrEmpty(PlaySettings.singleplayerMap))
			{
				UnturnedLog.warn("Singleplayer map selection empty");
				return;
			}
			LevelInfo levelInfo = null;
			foreach (LevelInfo levelInfo2 in MenuPlaySingleplayerUI.levels)
			{
				if (string.Equals(levelInfo2.name, PlaySettings.singleplayerMap, 3))
				{
					levelInfo = levelInfo2;
					break;
				}
			}
			if (levelInfo == null)
			{
				UnturnedLog.warn("Unable to find singleplayer selected map '{0}'", new object[]
				{
					PlaySettings.singleplayerMap
				});
				return;
			}
			Local local = levelInfo.getLocalization();
			if (local != null)
			{
				string text = local.format("Description");
				text = ItemTool.filterRarityRichText(text);
				RichTextUtil.replaceNewlineMarkup(ref text);
				MenuPlaySingleplayerUI.descriptionBox.Text = text;
			}
			if (local != null && local.has("Name"))
			{
				MenuPlaySingleplayerUI.selectedBox.Text = local.format("Name");
			}
			else
			{
				MenuPlaySingleplayerUI.selectedBox.Text = PlaySettings.singleplayerMap;
			}
			if (MenuPlaySingleplayerUI.previewImage.Texture != null && MenuPlaySingleplayerUI.previewImage.ShouldDestroyTexture)
			{
				Object.Destroy(MenuPlaySingleplayerUI.previewImage.Texture);
				MenuPlaySingleplayerUI.previewImage.Texture = null;
			}
			string previewImageFilePath = levelInfo.GetPreviewImageFilePath();
			if (!string.IsNullOrEmpty(previewImageFilePath))
			{
				MenuPlaySingleplayerUI.previewImage.Texture = ReadWrite.readTextureFromFile(previewImageFilePath, EReadTextureFromFileMode.UI);
			}
			float num = MenuPlaySingleplayerUI.creditsBox.PositionOffset_Y;
			if (levelInfo.configData.Creators.Length != 0 || levelInfo.configData.Collaborators.Length != 0 || levelInfo.configData.Thanks.Length != 0)
			{
				int num2 = 0;
				string text2 = string.Empty;
				if (levelInfo.configData.Creators.Length != 0)
				{
					text2 += MenuPlaySingleplayerUI.localization.format("Creators");
					num2 += 20;
					for (int j = 0; j < levelInfo.configData.Creators.Length; j++)
					{
						text2 = text2 + "\n" + levelInfo.configData.Creators[j];
						num2 += 20;
					}
				}
				if (levelInfo.configData.Collaborators.Length != 0)
				{
					if (text2.Length > 0)
					{
						text2 += "\n\n";
						num2 += 30;
					}
					text2 += MenuPlaySingleplayerUI.localization.format("Collaborators");
					num2 += 20;
					for (int k = 0; k < levelInfo.configData.Collaborators.Length; k++)
					{
						text2 = text2 + "\n" + levelInfo.configData.Collaborators[k];
						num2 += 20;
					}
				}
				if (levelInfo.configData.Thanks.Length != 0)
				{
					if (text2.Length > 0)
					{
						text2 += "\n\n";
						num2 += 30;
					}
					text2 += MenuPlaySingleplayerUI.localization.format("Thanks");
					num2 += 20;
					for (int l = 0; l < levelInfo.configData.Thanks.Length; l++)
					{
						text2 = text2 + "\n" + levelInfo.configData.Thanks[l];
						num2 += 20;
					}
				}
				num2 = Mathf.Max(num2, 40);
				MenuPlaySingleplayerUI.creditsBox.SizeOffset_Y = (float)num2;
				MenuPlaySingleplayerUI.creditsBox.Text = text2;
				MenuPlaySingleplayerUI.creditsBox.IsVisible = true;
				num += (float)(num2 + 10);
			}
			else
			{
				MenuPlaySingleplayerUI.creditsBox.IsVisible = false;
			}
			List<int> list = new List<int>(4);
			if (levelInfo.configData.Item > 0 && !Provider.provider.economyService.isItemHiddenByCountryRestrictions(levelInfo.configData.Item))
			{
				list.Add(levelInfo.configData.Item);
			}
			if (levelInfo.configData.Associated_Stockpile_Items.Length != 0)
			{
				foreach (int num3 in levelInfo.configData.Associated_Stockpile_Items)
				{
					if (num3 > 0 && !Provider.provider.economyService.isItemHiddenByCountryRestrictions(num3))
					{
						list.Add(num3);
					}
				}
			}
			MenuPlaySingleplayerUI.featuredItemDefId = list.RandomOrDefault<int>();
			if (MenuPlaySingleplayerUI.featuredItemDefId > 0)
			{
				MenuPlaySingleplayerUI.itemButton.PositionOffset_Y = num;
				MenuPlaySingleplayerUI.itemButton.Text = MenuPlaySingleplayerUI.localization.format("Credits_Text", MenuPlaySingleplayerUI.selectedBox.Text, string.Concat(new string[]
				{
					"<color=",
					Palette.hex(Provider.provider.economyService.getInventoryColor(MenuPlaySingleplayerUI.featuredItemDefId)),
					">",
					Provider.provider.economyService.getInventoryName(MenuPlaySingleplayerUI.featuredItemDefId),
					"</color>"
				}));
				MenuPlaySingleplayerUI.itemButton.TooltipText = MenuPlaySingleplayerUI.localization.format("Credits_Tooltip");
				MenuPlaySingleplayerUI.itemButton.IsVisible = true;
				num += MenuPlaySingleplayerUI.itemButton.SizeOffset_Y + 10f;
			}
			else
			{
				MenuPlaySingleplayerUI.itemButton.IsVisible = false;
			}
			string text3 = levelInfo.feedbackUrl;
			if (!string.IsNullOrEmpty(text3) && !WebUtils.CanParseThirdPartyUrl(text3, true, true))
			{
				UnturnedLog.warn("Ignoring potentially unsafe level feedback url {0}", new object[]
				{
					text3
				});
				text3 = null;
			}
			if (string.IsNullOrEmpty(text3))
			{
				MenuPlaySingleplayerUI.feedbackButton.IsVisible = false;
				return;
			}
			MenuPlaySingleplayerUI.feedbackButton.PositionOffset_Y = num;
			MenuPlaySingleplayerUI.feedbackButton.IsVisible = true;
			num += MenuPlaySingleplayerUI.feedbackButton.SizeOffset_Y + 10f;
		}

		// Token: 0x0600417D RID: 16765 RVA: 0x0015D9BA File Offset: 0x0015BBBA
		private static void onClickedLevel(SleekLevel level, byte index)
		{
			if ((int)index < MenuPlaySingleplayerUI.levels.Length && MenuPlaySingleplayerUI.levels[(int)index] != null)
			{
				PlaySettings.singleplayerMap = MenuPlaySingleplayerUI.levels[(int)index].name;
				MenuPlaySingleplayerUI.updateSelection();
			}
		}

		// Token: 0x0600417E RID: 16766 RVA: 0x0015D9E5 File Offset: 0x0015BBE5
		private static void onClickedPlayButton(ISleekElement button)
		{
			if (string.IsNullOrEmpty(PlaySettings.singleplayerMap))
			{
				return;
			}
			Provider.map = PlaySettings.singleplayerMap;
			MenuSettings.save();
			Provider.singleplayer(PlaySettings.singleplayerMode, PlaySettings.singleplayerCheats);
		}

		// Token: 0x0600417F RID: 16767 RVA: 0x0015DA12 File Offset: 0x0015BC12
		private static void onClickedOfficialMapsButton(ISleekElement button)
		{
			PlaySettings.singleplayerCategory = ESingleplayerMapCategory.OFFICIAL;
			MenuPlaySingleplayerUI.refreshLevels();
		}

		// Token: 0x06004180 RID: 16768 RVA: 0x0015DA1F File Offset: 0x0015BC1F
		private static void onClickedCuratedMapsButton(ISleekElement button)
		{
			if (MenuPlaySingleplayerUI.curatedStatusLabel != null)
			{
				MenuPlaySingleplayerUI.curatedMapsButton.RemoveChild(MenuPlaySingleplayerUI.curatedStatusLabel);
				MenuPlaySingleplayerUI.curatedStatusLabel = null;
			}
			PlaySettings.singleplayerCategory = ESingleplayerMapCategory.CURATED;
			MenuPlaySingleplayerUI.refreshLevels();
		}

		// Token: 0x06004181 RID: 16769 RVA: 0x0015DA48 File Offset: 0x0015BC48
		private static void onClickedWorkshopMapsButton(ISleekElement button)
		{
			PlaySettings.singleplayerCategory = ESingleplayerMapCategory.WORKSHOP;
			MenuPlaySingleplayerUI.refreshLevels();
		}

		// Token: 0x06004182 RID: 16770 RVA: 0x0015DA55 File Offset: 0x0015BC55
		private static void onClickedMiscMapsButton(ISleekElement button)
		{
			PlaySettings.singleplayerCategory = ESingleplayerMapCategory.MISC;
			MenuPlaySingleplayerUI.refreshLevels();
		}

		// Token: 0x06004183 RID: 16771 RVA: 0x0015DA62 File Offset: 0x0015BC62
		private static void onClickedManageSubscriptionsButton(ISleekElement button)
		{
			MenuUI.closeAll();
			MenuWorkshopSubscriptionsUI.instance.open();
		}

		// Token: 0x06004184 RID: 16772 RVA: 0x0015DA73 File Offset: 0x0015BC73
		private static void onSwappedModeState(SleekButtonState button, int index)
		{
			PlaySettings.singleplayerMode = (EGameMode)index;
		}

		// Token: 0x06004185 RID: 16773 RVA: 0x0015DA7B File Offset: 0x0015BC7B
		private static void onClickedConfigButton(ISleekElement button)
		{
			if (PlaySettings.singleplayerMap == null || PlaySettings.singleplayerMap.Length == 0)
			{
				return;
			}
			MenuPlayConfigUI.open();
			MenuPlaySingleplayerUI.close();
		}

		// Token: 0x06004186 RID: 16774 RVA: 0x0015DA9B File Offset: 0x0015BC9B
		private static void onClickedBrowseServersButton(ISleekElement button)
		{
			if (string.IsNullOrEmpty(PlaySettings.singleplayerMap))
			{
				return;
			}
			MenuPlayServersUI.serverListFiltersUI.OpenForMap(PlaySettings.singleplayerMap);
			MenuPlaySingleplayerUI.close();
		}

		// Token: 0x06004187 RID: 16775 RVA: 0x0015DAC0 File Offset: 0x0015BCC0
		private static void onClickedResetButton(SleekButtonIconConfirm button)
		{
			if (PlaySettings.singleplayerMap == null || PlaySettings.singleplayerMap.Length == 0)
			{
				return;
			}
			if (ReadWrite.folderExists("/Worlds/Singleplayer_" + Characters.selected.ToString() + "/Level/" + PlaySettings.singleplayerMap))
			{
				ReadWrite.deleteFolder("/Worlds/Singleplayer_" + Characters.selected.ToString() + "/Level/" + PlaySettings.singleplayerMap);
			}
			if (ReadWrite.folderExists(string.Concat(new string[]
			{
				"/Worlds/Singleplayer_",
				Characters.selected.ToString(),
				"/Players/",
				Provider.user.ToString(),
				"_",
				Characters.selected.ToString(),
				"/",
				PlaySettings.singleplayerMap
			})))
			{
				ReadWrite.deleteFolder(string.Concat(new string[]
				{
					"/Worlds/Singleplayer_",
					Characters.selected.ToString(),
					"/Players/",
					Provider.user.ToString(),
					"_",
					Characters.selected.ToString(),
					"/",
					PlaySettings.singleplayerMap
				}));
			}
		}

		// Token: 0x06004188 RID: 16776 RVA: 0x0015DC0D File Offset: 0x0015BE0D
		private static void onToggledCheatsToggle(ISleekToggle toggle, bool state)
		{
			PlaySettings.singleplayerCheats = state;
		}

		// Token: 0x06004189 RID: 16777 RVA: 0x0015DC18 File Offset: 0x0015BE18
		private static void refreshLevels()
		{
			if (MenuPlaySingleplayerUI.levelScrollBox == null)
			{
				return;
			}
			MenuPlaySingleplayerUI.levelScrollBox.RemoveAllChildren();
			MenuPlaySingleplayerUI.levels = Level.getLevels(PlaySettings.singleplayerCategory);
			bool flag = false;
			int num = 0;
			MenuPlaySingleplayerUI.levelButtons = new SleekLevel[MenuPlaySingleplayerUI.levels.Length];
			for (int i = 0; i < MenuPlaySingleplayerUI.levels.Length; i++)
			{
				if (MenuPlaySingleplayerUI.levels[i] != null)
				{
					SleekLevel sleekLevel = new SleekLevel(MenuPlaySingleplayerUI.levels[i]);
					sleekLevel.PositionOffset_Y = (float)num;
					sleekLevel.onClickedLevel = new ClickedLevel(MenuPlaySingleplayerUI.onClickedLevel);
					MenuPlaySingleplayerUI.levelScrollBox.AddChild(sleekLevel);
					num += 110;
					MenuPlaySingleplayerUI.levelButtons[i] = sleekLevel;
					if (!flag && string.Equals(MenuPlaySingleplayerUI.levels[i].name, PlaySettings.singleplayerMap, 3))
					{
						flag = true;
					}
				}
			}
			if (MenuPlaySingleplayerUI.levels.Length == 0)
			{
				PlaySettings.singleplayerMap = "";
			}
			else if (!flag || PlaySettings.singleplayerMap == null || PlaySettings.singleplayerMap.Length == 0)
			{
				PlaySettings.singleplayerMap = MenuPlaySingleplayerUI.levels[0].name;
			}
			MenuPlaySingleplayerUI.updateSelection();
			if (PlaySettings.singleplayerCategory == ESingleplayerMapCategory.CURATED)
			{
				foreach (CuratedMapLink curatedMapLink in Provider.statusData.Maps.Curated_Map_Links)
				{
					if (!Provider.provider.workshopService.getSubscribed(curatedMapLink.Workshop_File_Id) && curatedMapLink.Visible_In_Singleplayer_Recommendations_List)
					{
						SleekCuratedLevelLink sleekCuratedLevelLink = new SleekCuratedLevelLink(curatedMapLink);
						sleekCuratedLevelLink.PositionOffset_Y = (float)num;
						MenuPlaySingleplayerUI.levelScrollBox.AddChild(sleekCuratedLevelLink);
						num += 110;
					}
				}
			}
			if (PlaySettings.singleplayerCategory == ESingleplayerMapCategory.WORKSHOP)
			{
				ISleekButton sleekButton = Glazier.Get().CreateButton();
				sleekButton.PositionOffset_Y = (float)num;
				sleekButton.SizeOffset_X = 400f;
				sleekButton.SizeOffset_Y = 30f;
				sleekButton.Text = MenuPlaySingleplayerUI.localization.format("Manage_Workshop_Label");
				sleekButton.TooltipText = MenuPlaySingleplayerUI.localization.format("Manage_Workshop_Tooltip");
				sleekButton.OnClicked += new ClickedButton(MenuPlaySingleplayerUI.onClickedManageSubscriptionsButton);
				MenuPlaySingleplayerUI.levelScrollBox.AddChild(sleekButton);
				num += 40;
			}
			MenuPlaySingleplayerUI.levelScrollBox.ContentSizeOffset = new Vector2(0f, (float)(num - 10));
		}

		// Token: 0x0600418A RID: 16778 RVA: 0x0015DE48 File Offset: 0x0015C048
		private static void onLevelsRefreshed()
		{
			MenuPlaySingleplayerUI.refreshLevels();
		}

		// Token: 0x0600418B RID: 16779 RVA: 0x0015DE4F File Offset: 0x0015C04F
		private static void onClickedItemButton(ISleekElement button)
		{
			if (PlaySettings.singleplayerMap == null || PlaySettings.singleplayerMap.Length == 0)
			{
				return;
			}
			if (Level.getLevel(PlaySettings.singleplayerMap) == null)
			{
				return;
			}
			if (MenuPlaySingleplayerUI.featuredItemDefId <= 0)
			{
				return;
			}
			ItemStore.Get().ViewItem(MenuPlaySingleplayerUI.featuredItemDefId);
		}

		// Token: 0x0600418C RID: 16780 RVA: 0x0015DE8C File Offset: 0x0015C08C
		private static void onClickedFeedbackButton(ISleekElement button)
		{
			if (PlaySettings.singleplayerMap == null || PlaySettings.singleplayerMap.Length == 0)
			{
				return;
			}
			LevelInfo level = Level.getLevel(PlaySettings.singleplayerMap);
			if (level == null)
			{
				return;
			}
			string url;
			if (WebUtils.ParseThirdPartyUrl(level.feedbackUrl, out url, true, true))
			{
				Provider.provider.browserService.open(url);
				return;
			}
			UnturnedLog.warn("Ignoring potentially unsafe level feedback url {0}", new object[]
			{
				level.feedbackUrl
			});
		}

		// Token: 0x0600418D RID: 16781 RVA: 0x0015DEF7 File Offset: 0x0015C0F7
		private static void onClickedNewsButton(ISleekElement button)
		{
		}

		// Token: 0x0600418E RID: 16782 RVA: 0x0015DEF9 File Offset: 0x0015C0F9
		private static void onClickedBackButton(ISleekElement button)
		{
			MenuPlayUI.open();
			MenuPlaySingleplayerUI.close();
		}

		// Token: 0x0600418F RID: 16783 RVA: 0x0015DF05 File Offset: 0x0015C105
		public void OnDestroy()
		{
			Level.onLevelsRefreshed = (LevelsRefreshed)Delegate.Remove(Level.onLevelsRefreshed, new LevelsRefreshed(MenuPlaySingleplayerUI.onLevelsRefreshed));
		}

		// Token: 0x06004190 RID: 16784 RVA: 0x0015DF28 File Offset: 0x0015C128
		public MenuPlaySingleplayerUI()
		{
			MenuPlaySingleplayerUI.localization = Localization.read("/Menu/Play/MenuPlaySingleplayer.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Play/MenuPlaySingleplayer/MenuPlaySingleplayer.unity3d");
			MenuPlaySingleplayerUI.container = new SleekFullscreenBox();
			MenuPlaySingleplayerUI.container.PositionOffset_X = 10f;
			MenuPlaySingleplayerUI.container.PositionOffset_Y = 10f;
			MenuPlaySingleplayerUI.container.PositionScale_Y = 1f;
			MenuPlaySingleplayerUI.container.SizeOffset_X = -20f;
			MenuPlaySingleplayerUI.container.SizeOffset_Y = -20f;
			MenuPlaySingleplayerUI.container.SizeScale_X = 1f;
			MenuPlaySingleplayerUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuPlaySingleplayerUI.container);
			MenuPlaySingleplayerUI.active = false;
			MenuPlaySingleplayerUI.previewBox = Glazier.Get().CreateBox();
			MenuPlaySingleplayerUI.previewBox.PositionOffset_X = -305f;
			MenuPlaySingleplayerUI.previewBox.PositionOffset_Y = 80f;
			MenuPlaySingleplayerUI.previewBox.PositionScale_X = 0.5f;
			MenuPlaySingleplayerUI.previewBox.SizeOffset_X = 340f;
			MenuPlaySingleplayerUI.previewBox.SizeOffset_Y = 200f;
			MenuPlaySingleplayerUI.container.AddChild(MenuPlaySingleplayerUI.previewBox);
			MenuPlaySingleplayerUI.previewImage = Glazier.Get().CreateImage();
			MenuPlaySingleplayerUI.previewImage.PositionOffset_X = 10f;
			MenuPlaySingleplayerUI.previewImage.PositionOffset_Y = 10f;
			MenuPlaySingleplayerUI.previewImage.SizeOffset_X = -20f;
			MenuPlaySingleplayerUI.previewImage.SizeOffset_Y = -20f;
			MenuPlaySingleplayerUI.previewImage.SizeScale_X = 1f;
			MenuPlaySingleplayerUI.previewImage.SizeScale_Y = 1f;
			MenuPlaySingleplayerUI.previewImage.ShouldDestroyTexture = true;
			MenuPlaySingleplayerUI.previewBox.AddChild(MenuPlaySingleplayerUI.previewImage);
			MenuPlaySingleplayerUI.levelScrollBox = Glazier.Get().CreateScrollView();
			MenuPlaySingleplayerUI.levelScrollBox.PositionOffset_X = -95f;
			MenuPlaySingleplayerUI.levelScrollBox.PositionOffset_Y = 340f;
			MenuPlaySingleplayerUI.levelScrollBox.PositionScale_X = 0.5f;
			MenuPlaySingleplayerUI.levelScrollBox.SizeOffset_X = 430f;
			MenuPlaySingleplayerUI.levelScrollBox.SizeOffset_Y = -440f;
			MenuPlaySingleplayerUI.levelScrollBox.SizeScale_Y = 1f;
			MenuPlaySingleplayerUI.levelScrollBox.ScaleContentToWidth = true;
			MenuPlaySingleplayerUI.container.AddChild(MenuPlaySingleplayerUI.levelScrollBox);
			MenuPlaySingleplayerUI.officalMapsButton = Glazier.Get().CreateButton();
			MenuPlaySingleplayerUI.officalMapsButton.PositionOffset_X = -95f;
			MenuPlaySingleplayerUI.officalMapsButton.PositionOffset_Y = 290f;
			MenuPlaySingleplayerUI.officalMapsButton.PositionScale_X = 0.5f;
			MenuPlaySingleplayerUI.officalMapsButton.SizeOffset_X = 100f;
			MenuPlaySingleplayerUI.officalMapsButton.SizeOffset_Y = 50f;
			MenuPlaySingleplayerUI.officalMapsButton.Text = MenuPlaySingleplayerUI.localization.format("Maps_Official");
			MenuPlaySingleplayerUI.officalMapsButton.TooltipText = MenuPlaySingleplayerUI.localization.format("Maps_Official_Tooltip");
			MenuPlaySingleplayerUI.officalMapsButton.OnClicked += new ClickedButton(MenuPlaySingleplayerUI.onClickedOfficialMapsButton);
			MenuPlaySingleplayerUI.officalMapsButton.FontSize = 3;
			MenuPlaySingleplayerUI.container.AddChild(MenuPlaySingleplayerUI.officalMapsButton);
			MenuPlaySingleplayerUI.curatedMapsButton = Glazier.Get().CreateButton();
			MenuPlaySingleplayerUI.curatedMapsButton.PositionOffset_X = 5f;
			MenuPlaySingleplayerUI.curatedMapsButton.PositionOffset_Y = 290f;
			MenuPlaySingleplayerUI.curatedMapsButton.PositionScale_X = 0.5f;
			MenuPlaySingleplayerUI.curatedMapsButton.SizeOffset_X = 100f;
			MenuPlaySingleplayerUI.curatedMapsButton.SizeOffset_Y = 50f;
			MenuPlaySingleplayerUI.curatedMapsButton.Text = MenuPlaySingleplayerUI.localization.format("Maps_Curated");
			MenuPlaySingleplayerUI.curatedMapsButton.TooltipText = MenuPlaySingleplayerUI.localization.format("Maps_Curated_Tooltip");
			MenuPlaySingleplayerUI.curatedMapsButton.OnClicked += new ClickedButton(MenuPlaySingleplayerUI.onClickedCuratedMapsButton);
			MenuPlaySingleplayerUI.curatedMapsButton.FontSize = 3;
			MenuPlaySingleplayerUI.container.AddChild(MenuPlaySingleplayerUI.curatedMapsButton);
			MenuPlaySingleplayerUI.workshopMapsButton = Glazier.Get().CreateButton();
			MenuPlaySingleplayerUI.workshopMapsButton.PositionOffset_X = 105f;
			MenuPlaySingleplayerUI.workshopMapsButton.PositionOffset_Y = 290f;
			MenuPlaySingleplayerUI.workshopMapsButton.PositionScale_X = 0.5f;
			MenuPlaySingleplayerUI.workshopMapsButton.SizeOffset_X = 100f;
			MenuPlaySingleplayerUI.workshopMapsButton.SizeOffset_Y = 50f;
			MenuPlaySingleplayerUI.workshopMapsButton.Text = MenuPlaySingleplayerUI.localization.format("Maps_Workshop");
			MenuPlaySingleplayerUI.workshopMapsButton.TooltipText = MenuPlaySingleplayerUI.localization.format("Maps_Workshop_Tooltip");
			MenuPlaySingleplayerUI.workshopMapsButton.OnClicked += new ClickedButton(MenuPlaySingleplayerUI.onClickedWorkshopMapsButton);
			MenuPlaySingleplayerUI.workshopMapsButton.FontSize = 3;
			MenuPlaySingleplayerUI.container.AddChild(MenuPlaySingleplayerUI.workshopMapsButton);
			MenuPlaySingleplayerUI.miscMapsButton = Glazier.Get().CreateButton();
			MenuPlaySingleplayerUI.miscMapsButton.PositionOffset_X = 205f;
			MenuPlaySingleplayerUI.miscMapsButton.PositionOffset_Y = 290f;
			MenuPlaySingleplayerUI.miscMapsButton.PositionScale_X = 0.5f;
			MenuPlaySingleplayerUI.miscMapsButton.SizeOffset_X = 100f;
			MenuPlaySingleplayerUI.miscMapsButton.SizeOffset_Y = 50f;
			MenuPlaySingleplayerUI.miscMapsButton.Text = MenuPlaySingleplayerUI.localization.format("Maps_Misc");
			MenuPlaySingleplayerUI.miscMapsButton.TooltipText = MenuPlaySingleplayerUI.localization.format("Maps_Misc_Tooltip");
			MenuPlaySingleplayerUI.miscMapsButton.OnClicked += new ClickedButton(MenuPlaySingleplayerUI.onClickedMiscMapsButton);
			MenuPlaySingleplayerUI.miscMapsButton.FontSize = 3;
			MenuPlaySingleplayerUI.container.AddChild(MenuPlaySingleplayerUI.miscMapsButton);
			MenuPlaySingleplayerUI.selectedBox = Glazier.Get().CreateBox();
			MenuPlaySingleplayerUI.selectedBox.PositionOffset_X = 45f;
			MenuPlaySingleplayerUI.selectedBox.PositionOffset_Y = 80f;
			MenuPlaySingleplayerUI.selectedBox.PositionScale_X = 0.5f;
			MenuPlaySingleplayerUI.selectedBox.SizeOffset_X = 260f;
			MenuPlaySingleplayerUI.selectedBox.SizeOffset_Y = 30f;
			MenuPlaySingleplayerUI.container.AddChild(MenuPlaySingleplayerUI.selectedBox);
			MenuPlaySingleplayerUI.descriptionBox = Glazier.Get().CreateBox();
			MenuPlaySingleplayerUI.descriptionBox.PositionOffset_X = 45f;
			MenuPlaySingleplayerUI.descriptionBox.PositionOffset_Y = 120f;
			MenuPlaySingleplayerUI.descriptionBox.PositionScale_X = 0.5f;
			MenuPlaySingleplayerUI.descriptionBox.SizeOffset_X = 260f;
			MenuPlaySingleplayerUI.descriptionBox.SizeOffset_Y = 160f;
			MenuPlaySingleplayerUI.descriptionBox.TextAlignment = 1;
			MenuPlaySingleplayerUI.descriptionBox.AllowRichText = true;
			MenuPlaySingleplayerUI.descriptionBox.TextColor = 4;
			MenuPlaySingleplayerUI.container.AddChild(MenuPlaySingleplayerUI.descriptionBox);
			MenuPlaySingleplayerUI.creditsBox = Glazier.Get().CreateBox();
			MenuPlaySingleplayerUI.creditsBox.PositionOffset_X = 345f;
			MenuPlaySingleplayerUI.creditsBox.PositionOffset_Y = 100f;
			MenuPlaySingleplayerUI.creditsBox.PositionScale_X = 0.5f;
			MenuPlaySingleplayerUI.creditsBox.SizeOffset_X = 250f;
			MenuPlaySingleplayerUI.container.AddChild(MenuPlaySingleplayerUI.creditsBox);
			MenuPlaySingleplayerUI.creditsBox.IsVisible = false;
			MenuPlaySingleplayerUI.itemButton = Glazier.Get().CreateButton();
			MenuPlaySingleplayerUI.itemButton.AllowRichText = true;
			MenuPlaySingleplayerUI.itemButton.PositionOffset_X = 345f;
			MenuPlaySingleplayerUI.itemButton.PositionOffset_Y = 100f;
			MenuPlaySingleplayerUI.itemButton.PositionScale_X = 0.5f;
			MenuPlaySingleplayerUI.itemButton.SizeOffset_X = 250f;
			MenuPlaySingleplayerUI.itemButton.SizeOffset_Y = 100f;
			MenuPlaySingleplayerUI.itemButton.TextColor = 4;
			MenuPlaySingleplayerUI.itemButton.TextContrastContext = 1;
			MenuPlaySingleplayerUI.itemButton.OnClicked += new ClickedButton(MenuPlaySingleplayerUI.onClickedItemButton);
			MenuPlaySingleplayerUI.container.AddChild(MenuPlaySingleplayerUI.itemButton);
			MenuPlaySingleplayerUI.itemButton.IsVisible = false;
			MenuPlaySingleplayerUI.feedbackButton = Glazier.Get().CreateButton();
			MenuPlaySingleplayerUI.feedbackButton.PositionOffset_X = 345f;
			MenuPlaySingleplayerUI.feedbackButton.PositionOffset_Y = 100f;
			MenuPlaySingleplayerUI.feedbackButton.PositionScale_X = 0.5f;
			MenuPlaySingleplayerUI.feedbackButton.SizeOffset_X = 250f;
			MenuPlaySingleplayerUI.feedbackButton.SizeOffset_Y = 30f;
			MenuPlaySingleplayerUI.feedbackButton.Text = MenuPlaySingleplayerUI.localization.format("Feedback_Button");
			MenuPlaySingleplayerUI.feedbackButton.TooltipText = MenuPlaySingleplayerUI.localization.format("Feedback_Button_Tooltip");
			MenuPlaySingleplayerUI.feedbackButton.OnClicked += new ClickedButton(MenuPlaySingleplayerUI.onClickedFeedbackButton);
			MenuPlaySingleplayerUI.container.AddChild(MenuPlaySingleplayerUI.feedbackButton);
			MenuPlaySingleplayerUI.feedbackButton.IsVisible = false;
			MenuPlaySingleplayerUI.newsButton = Glazier.Get().CreateButton();
			MenuPlaySingleplayerUI.newsButton.PositionOffset_X = 345f;
			MenuPlaySingleplayerUI.newsButton.PositionOffset_Y = 100f;
			MenuPlaySingleplayerUI.newsButton.PositionScale_X = 0.5f;
			MenuPlaySingleplayerUI.newsButton.SizeOffset_X = 250f;
			MenuPlaySingleplayerUI.newsButton.SizeOffset_Y = 30f;
			MenuPlaySingleplayerUI.newsButton.OnClicked += new ClickedButton(MenuPlaySingleplayerUI.onClickedNewsButton);
			MenuPlaySingleplayerUI.container.AddChild(MenuPlaySingleplayerUI.newsButton);
			MenuPlaySingleplayerUI.newsButton.IsVisible = false;
			MenuPlaySingleplayerUI.playButton = new SleekButtonIcon(bundle.load<Texture2D>("Play"));
			MenuPlaySingleplayerUI.playButton.PositionOffset_X = -305f;
			MenuPlaySingleplayerUI.playButton.PositionOffset_Y = 290f;
			MenuPlaySingleplayerUI.playButton.PositionScale_X = 0.5f;
			MenuPlaySingleplayerUI.playButton.SizeOffset_X = 200f;
			MenuPlaySingleplayerUI.playButton.SizeOffset_Y = 30f;
			MenuPlaySingleplayerUI.playButton.text = MenuPlaySingleplayerUI.localization.format("Play_Button");
			MenuPlaySingleplayerUI.playButton.tooltip = MenuPlaySingleplayerUI.localization.format("Play_Button_Tooltip");
			MenuPlaySingleplayerUI.playButton.iconColor = 2;
			MenuPlaySingleplayerUI.playButton.onClickedButton += new ClickedButton(MenuPlaySingleplayerUI.onClickedPlayButton);
			MenuPlaySingleplayerUI.container.AddChild(MenuPlaySingleplayerUI.playButton);
			MenuPlaySingleplayerUI.browseServersButton = Glazier.Get().CreateButton();
			MenuPlaySingleplayerUI.browseServersButton.PositionOffset_X = -305f;
			MenuPlaySingleplayerUI.browseServersButton.PositionOffset_Y = 420f;
			MenuPlaySingleplayerUI.browseServersButton.PositionScale_X = 0.5f;
			MenuPlaySingleplayerUI.browseServersButton.SizeOffset_X = 200f;
			MenuPlaySingleplayerUI.browseServersButton.SizeOffset_Y = 30f;
			MenuPlaySingleplayerUI.browseServersButton.Text = MenuPlaySingleplayerUI.localization.format("Browse_Servers_Label");
			MenuPlaySingleplayerUI.browseServersButton.TooltipText = MenuPlaySingleplayerUI.localization.format("Browse_Servers_Tooltip");
			MenuPlaySingleplayerUI.browseServersButton.OnClicked += new ClickedButton(MenuPlaySingleplayerUI.onClickedBrowseServersButton);
			MenuPlaySingleplayerUI.container.AddChild(MenuPlaySingleplayerUI.browseServersButton);
			MenuPlaySingleplayerUI.modeButtonState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuPlaySingleplayerUI.localization.format("Easy_Button"), bundle.load<Texture>("Easy")),
				new GUIContent(MenuPlaySingleplayerUI.localization.format("Normal_Button"), bundle.load<Texture>("Normal")),
				new GUIContent(MenuPlaySingleplayerUI.localization.format("Hard_Button"), bundle.load<Texture>("Hard"))
			});
			MenuPlaySingleplayerUI.modeButtonState.PositionOffset_X = -305f;
			MenuPlaySingleplayerUI.modeButtonState.PositionOffset_Y = 330f;
			MenuPlaySingleplayerUI.modeButtonState.PositionScale_X = 0.5f;
			MenuPlaySingleplayerUI.modeButtonState.SizeOffset_X = 105f;
			MenuPlaySingleplayerUI.modeButtonState.SizeOffset_Y = 30f;
			MenuPlaySingleplayerUI.modeButtonState.state = (int)PlaySettings.singleplayerMode;
			MenuPlaySingleplayerUI.modeButtonState.onSwappedState = new SwappedState(MenuPlaySingleplayerUI.onSwappedModeState);
			MenuPlaySingleplayerUI.container.AddChild(MenuPlaySingleplayerUI.modeButtonState);
			MenuPlaySingleplayerUI.configButton = Glazier.Get().CreateButton();
			MenuPlaySingleplayerUI.configButton.PositionOffset_X = -195f;
			MenuPlaySingleplayerUI.configButton.PositionOffset_Y = 330f;
			MenuPlaySingleplayerUI.configButton.PositionScale_X = 0.5f;
			MenuPlaySingleplayerUI.configButton.SizeOffset_X = 85f;
			MenuPlaySingleplayerUI.configButton.SizeOffset_Y = 30f;
			MenuPlaySingleplayerUI.configButton.Text = MenuPlaySingleplayerUI.localization.format("Config_Button");
			MenuPlaySingleplayerUI.configButton.TooltipText = MenuPlaySingleplayerUI.localization.format("Config_Button_Tooltip");
			MenuPlaySingleplayerUI.configButton.OnClicked += new ClickedButton(MenuPlaySingleplayerUI.onClickedConfigButton);
			MenuPlaySingleplayerUI.container.AddChild(MenuPlaySingleplayerUI.configButton);
			MenuPlaySingleplayerUI.cheatsToggle = Glazier.Get().CreateToggle();
			MenuPlaySingleplayerUI.cheatsToggle.PositionOffset_X = -305f;
			MenuPlaySingleplayerUI.cheatsToggle.PositionOffset_Y = 370f;
			MenuPlaySingleplayerUI.cheatsToggle.PositionScale_X = 0.5f;
			MenuPlaySingleplayerUI.cheatsToggle.SizeOffset_X = 40f;
			MenuPlaySingleplayerUI.cheatsToggle.SizeOffset_Y = 40f;
			MenuPlaySingleplayerUI.cheatsToggle.AddLabel(MenuPlaySingleplayerUI.localization.format("Cheats_Label"), 1);
			MenuPlaySingleplayerUI.cheatsToggle.Value = PlaySettings.singleplayerCheats;
			MenuPlaySingleplayerUI.cheatsToggle.OnValueChanged += new Toggled(MenuPlaySingleplayerUI.onToggledCheatsToggle);
			MenuPlaySingleplayerUI.container.AddChild(MenuPlaySingleplayerUI.cheatsToggle);
			MenuPlaySingleplayerUI.resetButton = new SleekButtonIconConfirm(null, MenuPlaySingleplayerUI.localization.format("Reset_Button_Confirm"), MenuPlaySingleplayerUI.localization.format("Reset_Button_Confirm_Tooltip"), MenuPlaySingleplayerUI.localization.format("Reset_Button_Deny"), MenuPlaySingleplayerUI.localization.format("Reset_Button_Deny_Tooltip"));
			MenuPlaySingleplayerUI.resetButton.PositionOffset_X = -305f;
			MenuPlaySingleplayerUI.resetButton.PositionOffset_Y = 480f;
			MenuPlaySingleplayerUI.resetButton.PositionScale_X = 0.5f;
			MenuPlaySingleplayerUI.resetButton.SizeOffset_X = 200f;
			MenuPlaySingleplayerUI.resetButton.SizeOffset_Y = 30f;
			MenuPlaySingleplayerUI.resetButton.text = MenuPlaySingleplayerUI.localization.format("Reset_Button");
			MenuPlaySingleplayerUI.resetButton.tooltip = MenuPlaySingleplayerUI.localization.format("Reset_Button_Tooltip");
			MenuPlaySingleplayerUI.resetButton.onConfirmed = new Confirm(MenuPlaySingleplayerUI.onClickedResetButton);
			MenuPlaySingleplayerUI.container.AddChild(MenuPlaySingleplayerUI.resetButton);
			bundle.unload();
			MenuPlaySingleplayerUI.refreshLevels();
			Level.onLevelsRefreshed = (LevelsRefreshed)Delegate.Combine(Level.onLevelsRefreshed, new LevelsRefreshed(MenuPlaySingleplayerUI.onLevelsRefreshed));
			MenuPlaySingleplayerUI.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuPlaySingleplayerUI.backButton.PositionOffset_Y = -50f;
			MenuPlaySingleplayerUI.backButton.PositionScale_Y = 1f;
			MenuPlaySingleplayerUI.backButton.SizeOffset_X = 200f;
			MenuPlaySingleplayerUI.backButton.SizeOffset_Y = 50f;
			MenuPlaySingleplayerUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuPlaySingleplayerUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			MenuPlaySingleplayerUI.backButton.onClickedButton += new ClickedButton(MenuPlaySingleplayerUI.onClickedBackButton);
			MenuPlaySingleplayerUI.backButton.fontSize = 3;
			MenuPlaySingleplayerUI.backButton.iconColor = 2;
			MenuPlaySingleplayerUI.container.AddChild(MenuPlaySingleplayerUI.backButton);
			new MenuPlayConfigUI();
		}

		// Token: 0x04002A60 RID: 10848
		public static Local localization;

		// Token: 0x04002A61 RID: 10849
		private static SleekFullscreenBox container;

		// Token: 0x04002A62 RID: 10850
		public static bool active;

		// Token: 0x04002A63 RID: 10851
		private static SleekButtonIcon backButton;

		// Token: 0x04002A64 RID: 10852
		private static LevelInfo[] levels;

		// Token: 0x04002A65 RID: 10853
		private static ISleekBox previewBox;

		// Token: 0x04002A66 RID: 10854
		private static ISleekImage previewImage;

		// Token: 0x04002A67 RID: 10855
		private static ISleekScrollView levelScrollBox;

		// Token: 0x04002A68 RID: 10856
		private static SleekLevel[] levelButtons;

		// Token: 0x04002A69 RID: 10857
		private static SleekButtonIcon playButton;

		// Token: 0x04002A6A RID: 10858
		private static SleekButtonState modeButtonState;

		// Token: 0x04002A6B RID: 10859
		private static ISleekButton configButton;

		// Token: 0x04002A6C RID: 10860
		private static SleekButtonIconConfirm resetButton;

		// Token: 0x04002A6D RID: 10861
		private static ISleekButton browseServersButton;

		// Token: 0x04002A6E RID: 10862
		private static ISleekBox selectedBox;

		// Token: 0x04002A6F RID: 10863
		private static ISleekBox descriptionBox;

		// Token: 0x04002A70 RID: 10864
		private static ISleekToggle cheatsToggle;

		// Token: 0x04002A71 RID: 10865
		private static ISleekBox creditsBox;

		// Token: 0x04002A72 RID: 10866
		private static ISleekButton itemButton;

		// Token: 0x04002A73 RID: 10867
		private static ISleekButton feedbackButton;

		// Token: 0x04002A74 RID: 10868
		private static ISleekButton newsButton;

		// Token: 0x04002A75 RID: 10869
		private static ISleekButton officalMapsButton;

		// Token: 0x04002A76 RID: 10870
		private static ISleekButton curatedMapsButton;

		// Token: 0x04002A77 RID: 10871
		private static ISleekButton workshopMapsButton;

		// Token: 0x04002A78 RID: 10872
		private static ISleekButton miscMapsButton;

		// Token: 0x04002A79 RID: 10873
		private static SleekNew curatedStatusLabel;

		/// <summary>
		/// Stockpile item definition id with rev-share for the level creators.
		/// Randomly selected from associated items list.
		/// </summary>
		// Token: 0x04002A7A RID: 10874
		private static int featuredItemDefId;
	}
}
