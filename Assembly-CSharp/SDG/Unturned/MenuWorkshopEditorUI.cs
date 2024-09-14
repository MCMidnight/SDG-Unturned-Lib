using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007B3 RID: 1971
	public class MenuWorkshopEditorUI
	{
		// Token: 0x06004228 RID: 16936 RVA: 0x00168142 File Offset: 0x00166342
		public static void open()
		{
			if (MenuWorkshopEditorUI.active)
			{
				return;
			}
			MenuWorkshopEditorUI.active = true;
			MenuWorkshopEditorUI.removeButton.reset();
			MenuWorkshopEditorUI.container.AnimateIntoView();
		}

		// Token: 0x06004229 RID: 16937 RVA: 0x00168166 File Offset: 0x00166366
		public static void close()
		{
			if (!MenuWorkshopEditorUI.active)
			{
				return;
			}
			MenuWorkshopEditorUI.active = false;
			MenuWorkshopEditorUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x0600422A RID: 16938 RVA: 0x0016818C File Offset: 0x0016638C
		private static void updateSelection()
		{
			if (string.IsNullOrEmpty(PlaySettings.editorMap))
			{
				UnturnedLog.warn("Editor map selection empty");
				return;
			}
			LevelInfo levelInfo = null;
			foreach (LevelInfo levelInfo2 in MenuWorkshopEditorUI.levels)
			{
				if (string.Equals(levelInfo2.name, PlaySettings.editorMap, 3))
				{
					levelInfo = levelInfo2;
					break;
				}
			}
			if (levelInfo == null)
			{
				UnturnedLog.warn("Unable to find editor selected map '{0}'", new object[]
				{
					PlaySettings.editorMap
				});
				return;
			}
			Local localization = levelInfo.getLocalization();
			if (localization != null)
			{
				string text = localization.format("Description");
				text = ItemTool.filterRarityRichText(text);
				RichTextUtil.replaceNewlineMarkup(ref text);
				MenuWorkshopEditorUI.descriptionBox.Text = text;
			}
			if (localization != null && localization.has("Name"))
			{
				MenuWorkshopEditorUI.selectedBox.Text = localization.format("Name");
			}
			else
			{
				MenuWorkshopEditorUI.selectedBox.Text = PlaySettings.editorMap;
			}
			if (MenuWorkshopEditorUI.previewImage.Texture != null && MenuWorkshopEditorUI.previewImage.ShouldDestroyTexture)
			{
				Object.Destroy(MenuWorkshopEditorUI.previewImage.Texture);
				MenuWorkshopEditorUI.previewImage.Texture = null;
			}
			string previewImageFilePath = levelInfo.GetPreviewImageFilePath();
			if (!string.IsNullOrEmpty(previewImageFilePath))
			{
				MenuWorkshopEditorUI.previewImage.Texture = ReadWrite.readTextureFromFile(previewImageFilePath, EReadTextureFromFileMode.UI);
			}
		}

		// Token: 0x0600422B RID: 16939 RVA: 0x001682C6 File Offset: 0x001664C6
		private static void onClickedLevel(SleekLevel level, byte index)
		{
			if ((int)index < MenuWorkshopEditorUI.levels.Length && MenuWorkshopEditorUI.levels[(int)index] != null && MenuWorkshopEditorUI.levels[(int)index].isEditable)
			{
				PlaySettings.editorMap = MenuWorkshopEditorUI.levels[(int)index].name;
				MenuWorkshopEditorUI.updateSelection();
			}
		}

		// Token: 0x0600422C RID: 16940 RVA: 0x00168300 File Offset: 0x00166500
		private static void onClickedAddButton(ISleekElement button)
		{
			if (MenuWorkshopEditorUI.mapNameField.Text != "")
			{
				Level.add(MenuWorkshopEditorUI.mapNameField.Text, MenuWorkshopEditorUI.mapSizeState.state + ELevelSize.SMALL, (MenuWorkshopEditorUI.mapTypeState.state == 0) ? ELevelType.SURVIVAL : ELevelType.ARENA);
				MenuWorkshopEditorUI.mapNameField.Text = "";
			}
		}

		// Token: 0x0600422D RID: 16941 RVA: 0x00168360 File Offset: 0x00166560
		private static void onClickedRemoveButton(SleekButtonIconConfirm button)
		{
			if (PlaySettings.editorMap == null || PlaySettings.editorMap.Length == 0)
			{
				return;
			}
			for (int i = 0; i < MenuWorkshopEditorUI.levels.Length; i++)
			{
				if (MenuWorkshopEditorUI.levels[i] != null && MenuWorkshopEditorUI.levels[i].name == PlaySettings.editorMap && MenuWorkshopEditorUI.levels[i].isEditable)
				{
					Level.remove(MenuWorkshopEditorUI.levels[i].name);
				}
			}
		}

		// Token: 0x0600422E RID: 16942 RVA: 0x001683D4 File Offset: 0x001665D4
		private static void onClickedEditButton(ISleekElement button)
		{
			if (PlaySettings.editorMap == null || PlaySettings.editorMap.Length == 0)
			{
				return;
			}
			for (int i = 0; i < MenuWorkshopEditorUI.levels.Length; i++)
			{
				if (MenuWorkshopEditorUI.levels[i] != null && MenuWorkshopEditorUI.levels[i].name == PlaySettings.editorMap && MenuWorkshopEditorUI.levels[i].isEditable)
				{
					MenuSettings.save();
					Level.edit(MenuWorkshopEditorUI.levels[i]);
				}
			}
		}

		// Token: 0x0600422F RID: 16943 RVA: 0x00168448 File Offset: 0x00166648
		protected void OnClickedBrowseFilesButton(ISleekElement button)
		{
			if (PlaySettings.editorMap == null || PlaySettings.editorMap.Length == 0)
			{
				return;
			}
			for (int i = 0; i < MenuWorkshopEditorUI.levels.Length; i++)
			{
				if (MenuWorkshopEditorUI.levels[i] != null && MenuWorkshopEditorUI.levels[i].name == PlaySettings.editorMap && MenuWorkshopEditorUI.levels[i].isEditable)
				{
					ReadWrite.OpenFileBrowser(MenuWorkshopEditorUI.levels[i].path);
					return;
				}
			}
		}

		// Token: 0x06004230 RID: 16944 RVA: 0x001684BC File Offset: 0x001666BC
		private static void onLevelsRefreshed()
		{
			if (MenuWorkshopEditorUI.levelScrollBox == null)
			{
				return;
			}
			MenuWorkshopEditorUI.levelScrollBox.RemoveAllChildren();
			MenuWorkshopEditorUI.levels = Level.getLevels(ESingleplayerMapCategory.EDITABLE);
			bool flag = false;
			MenuWorkshopEditorUI.levelButtons = new SleekLevel[MenuWorkshopEditorUI.levels.Length];
			for (int i = 0; i < MenuWorkshopEditorUI.levels.Length; i++)
			{
				if (MenuWorkshopEditorUI.levels[i] != null)
				{
					SleekLevel sleekLevel = new SleekEditorLevel(MenuWorkshopEditorUI.levels[i]);
					sleekLevel.PositionOffset_Y = (float)(i * 110);
					sleekLevel.onClickedLevel = new ClickedLevel(MenuWorkshopEditorUI.onClickedLevel);
					MenuWorkshopEditorUI.levelScrollBox.AddChild(sleekLevel);
					MenuWorkshopEditorUI.levelButtons[i] = sleekLevel;
					if (!flag && string.Equals(MenuWorkshopEditorUI.levels[i].name, PlaySettings.editorMap, 3))
					{
						flag = true;
					}
				}
			}
			if (MenuWorkshopEditorUI.levels.Length == 0)
			{
				PlaySettings.editorMap = "";
			}
			else if (!flag || PlaySettings.editorMap == null || PlaySettings.editorMap.Length == 0)
			{
				PlaySettings.editorMap = MenuWorkshopEditorUI.levels[0].name;
			}
			MenuWorkshopEditorUI.updateSelection();
			MenuWorkshopEditorUI.levelScrollBox.ContentSizeOffset = new Vector2(0f, (float)(MenuWorkshopEditorUI.levels.Length * 110 - 10));
		}

		// Token: 0x06004231 RID: 16945 RVA: 0x001685D1 File Offset: 0x001667D1
		private static void onClickedBackButton(ISleekElement button)
		{
			MenuWorkshopUI.open();
			MenuWorkshopEditorUI.close();
		}

		// Token: 0x06004232 RID: 16946 RVA: 0x001685DD File Offset: 0x001667DD
		public void OnDestroy()
		{
			Level.onLevelsRefreshed = (LevelsRefreshed)Delegate.Remove(Level.onLevelsRefreshed, new LevelsRefreshed(MenuWorkshopEditorUI.onLevelsRefreshed));
		}

		// Token: 0x06004233 RID: 16947 RVA: 0x00168600 File Offset: 0x00166800
		public MenuWorkshopEditorUI()
		{
			if (MenuWorkshopEditorUI.icons != null)
			{
				MenuWorkshopEditorUI.icons.unload();
				MenuWorkshopEditorUI.icons = null;
			}
			Local local = Localization.read("/Menu/Workshop/MenuWorkshopEditor.dat");
			MenuWorkshopEditorUI.icons = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Workshop/MenuWorkshopEditor/MenuWorkshopEditor.unity3d");
			MenuWorkshopEditorUI.container = new SleekFullscreenBox();
			MenuWorkshopEditorUI.container.PositionOffset_X = 10f;
			MenuWorkshopEditorUI.container.PositionOffset_Y = 10f;
			MenuWorkshopEditorUI.container.PositionScale_Y = 1f;
			MenuWorkshopEditorUI.container.SizeOffset_X = -20f;
			MenuWorkshopEditorUI.container.SizeOffset_Y = -20f;
			MenuWorkshopEditorUI.container.SizeScale_X = 1f;
			MenuWorkshopEditorUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuWorkshopEditorUI.container);
			MenuWorkshopEditorUI.active = false;
			MenuWorkshopEditorUI.previewBox = Glazier.Get().CreateBox();
			MenuWorkshopEditorUI.previewBox.PositionOffset_X = -305f;
			MenuWorkshopEditorUI.previewBox.PositionOffset_Y = 80f;
			MenuWorkshopEditorUI.previewBox.PositionScale_X = 0.5f;
			MenuWorkshopEditorUI.previewBox.SizeOffset_X = 340f;
			MenuWorkshopEditorUI.previewBox.SizeOffset_Y = 200f;
			MenuWorkshopEditorUI.container.AddChild(MenuWorkshopEditorUI.previewBox);
			MenuWorkshopEditorUI.previewImage = Glazier.Get().CreateImage();
			MenuWorkshopEditorUI.previewImage.PositionOffset_X = 10f;
			MenuWorkshopEditorUI.previewImage.PositionOffset_Y = 10f;
			MenuWorkshopEditorUI.previewImage.SizeOffset_X = -20f;
			MenuWorkshopEditorUI.previewImage.SizeOffset_Y = -20f;
			MenuWorkshopEditorUI.previewImage.SizeScale_X = 1f;
			MenuWorkshopEditorUI.previewImage.SizeScale_Y = 1f;
			MenuWorkshopEditorUI.previewImage.ShouldDestroyTexture = true;
			MenuWorkshopEditorUI.previewBox.AddChild(MenuWorkshopEditorUI.previewImage);
			MenuWorkshopEditorUI.levelScrollBox = Glazier.Get().CreateScrollView();
			MenuWorkshopEditorUI.levelScrollBox.PositionOffset_X = -95f;
			MenuWorkshopEditorUI.levelScrollBox.PositionOffset_Y = 290f;
			MenuWorkshopEditorUI.levelScrollBox.PositionScale_X = 0.5f;
			MenuWorkshopEditorUI.levelScrollBox.SizeOffset_X = 430f;
			MenuWorkshopEditorUI.levelScrollBox.SizeOffset_Y = -390f;
			MenuWorkshopEditorUI.levelScrollBox.SizeScale_Y = 1f;
			MenuWorkshopEditorUI.levelScrollBox.ScaleContentToWidth = true;
			MenuWorkshopEditorUI.container.AddChild(MenuWorkshopEditorUI.levelScrollBox);
			MenuWorkshopEditorUI.selectedBox = Glazier.Get().CreateBox();
			MenuWorkshopEditorUI.selectedBox.PositionOffset_X = 45f;
			MenuWorkshopEditorUI.selectedBox.PositionOffset_Y = 80f;
			MenuWorkshopEditorUI.selectedBox.PositionScale_X = 0.5f;
			MenuWorkshopEditorUI.selectedBox.SizeOffset_X = 260f;
			MenuWorkshopEditorUI.selectedBox.SizeOffset_Y = 30f;
			MenuWorkshopEditorUI.container.AddChild(MenuWorkshopEditorUI.selectedBox);
			MenuWorkshopEditorUI.descriptionBox = Glazier.Get().CreateBox();
			MenuWorkshopEditorUI.descriptionBox.PositionOffset_X = 45f;
			MenuWorkshopEditorUI.descriptionBox.PositionOffset_Y = 120f;
			MenuWorkshopEditorUI.descriptionBox.PositionScale_X = 0.5f;
			MenuWorkshopEditorUI.descriptionBox.SizeOffset_X = 260f;
			MenuWorkshopEditorUI.descriptionBox.SizeOffset_Y = 160f;
			MenuWorkshopEditorUI.descriptionBox.TextAlignment = 1;
			MenuWorkshopEditorUI.descriptionBox.AllowRichText = true;
			MenuWorkshopEditorUI.descriptionBox.TextColor = 4;
			MenuWorkshopEditorUI.container.AddChild(MenuWorkshopEditorUI.descriptionBox);
			MenuWorkshopEditorUI.mapNameField = Glazier.Get().CreateStringField();
			MenuWorkshopEditorUI.mapNameField.PositionOffset_X = -305f;
			MenuWorkshopEditorUI.mapNameField.PositionOffset_Y = 370f;
			MenuWorkshopEditorUI.mapNameField.PositionScale_X = 0.5f;
			MenuWorkshopEditorUI.mapNameField.SizeOffset_X = 200f;
			MenuWorkshopEditorUI.mapNameField.SizeOffset_Y = 30f;
			MenuWorkshopEditorUI.mapNameField.MaxLength = 24;
			MenuWorkshopEditorUI.mapNameField.AddLabel(local.format("Name_Field_Label"), 0);
			MenuWorkshopEditorUI.container.AddChild(MenuWorkshopEditorUI.mapNameField);
			MenuWorkshopEditorUI.mapSizeState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuPlaySingleplayerUI.localization.format("Small")),
				new GUIContent(MenuPlaySingleplayerUI.localization.format("Medium")),
				new GUIContent(MenuPlaySingleplayerUI.localization.format("Large"))
			});
			MenuWorkshopEditorUI.mapSizeState.PositionOffset_X = -305f;
			MenuWorkshopEditorUI.mapSizeState.PositionOffset_Y = 410f;
			MenuWorkshopEditorUI.mapSizeState.PositionScale_X = 0.5f;
			MenuWorkshopEditorUI.mapSizeState.SizeOffset_X = 200f;
			MenuWorkshopEditorUI.mapSizeState.SizeOffset_Y = 30f;
			MenuWorkshopEditorUI.container.AddChild(MenuWorkshopEditorUI.mapSizeState);
			MenuWorkshopEditorUI.mapTypeState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuPlaySingleplayerUI.localization.format("Survival")),
				new GUIContent(MenuPlaySingleplayerUI.localization.format("Arena"))
			});
			MenuWorkshopEditorUI.mapTypeState.PositionOffset_X = -305f;
			MenuWorkshopEditorUI.mapTypeState.PositionOffset_Y = 450f;
			MenuWorkshopEditorUI.mapTypeState.PositionScale_X = 0.5f;
			MenuWorkshopEditorUI.mapTypeState.SizeOffset_X = 200f;
			MenuWorkshopEditorUI.mapTypeState.SizeOffset_Y = 30f;
			MenuWorkshopEditorUI.container.AddChild(MenuWorkshopEditorUI.mapTypeState);
			MenuWorkshopEditorUI.addButton = new SleekButtonIcon(MenuWorkshopEditorUI.icons.load<Texture2D>("Add"));
			MenuWorkshopEditorUI.addButton.PositionOffset_X = -305f;
			MenuWorkshopEditorUI.addButton.PositionOffset_Y = 490f;
			MenuWorkshopEditorUI.addButton.PositionScale_X = 0.5f;
			MenuWorkshopEditorUI.addButton.SizeOffset_X = 200f;
			MenuWorkshopEditorUI.addButton.SizeOffset_Y = 30f;
			MenuWorkshopEditorUI.addButton.text = local.format("Add_Button");
			MenuWorkshopEditorUI.addButton.tooltip = local.format("Add_Button_Tooltip");
			MenuWorkshopEditorUI.addButton.onClickedButton += new ClickedButton(MenuWorkshopEditorUI.onClickedAddButton);
			MenuWorkshopEditorUI.container.AddChild(MenuWorkshopEditorUI.addButton);
			MenuWorkshopEditorUI.removeButton = new SleekButtonIconConfirm(MenuWorkshopEditorUI.icons.load<Texture2D>("Remove"), local.format("Remove_Button_Confirm"), local.format("Remove_Button_Confirm_Tooltip"), local.format("Remove_Button_Deny"), local.format("Remove_Button_Deny_Tooltip"));
			MenuWorkshopEditorUI.removeButton.PositionOffset_X = -305f;
			MenuWorkshopEditorUI.removeButton.PositionOffset_Y = 530f;
			MenuWorkshopEditorUI.removeButton.PositionScale_X = 0.5f;
			MenuWorkshopEditorUI.removeButton.SizeOffset_X = 200f;
			MenuWorkshopEditorUI.removeButton.SizeOffset_Y = 30f;
			MenuWorkshopEditorUI.removeButton.text = local.format("Remove_Button");
			MenuWorkshopEditorUI.removeButton.tooltip = local.format("Remove_Button_Tooltip");
			MenuWorkshopEditorUI.removeButton.onConfirmed = new Confirm(MenuWorkshopEditorUI.onClickedRemoveButton);
			MenuWorkshopEditorUI.container.AddChild(MenuWorkshopEditorUI.removeButton);
			if (ReadWrite.SupportsOpeningFileBrowser)
			{
				ISleekButton sleekButton = Glazier.Get().CreateButton();
				sleekButton.PositionOffset_X = -305f;
				sleekButton.PositionOffset_Y = 330f;
				sleekButton.PositionScale_X = 0.5f;
				sleekButton.SizeOffset_X = 200f;
				sleekButton.SizeOffset_Y = 30f;
				sleekButton.Text = local.format("BrowseFiles_Label");
				sleekButton.OnClicked += new ClickedButton(this.OnClickedBrowseFilesButton);
				MenuWorkshopEditorUI.container.AddChild(sleekButton);
			}
			MenuWorkshopEditorUI.editButton = new SleekButtonIcon(MenuWorkshopEditorUI.icons.load<Texture2D>("Edit"));
			MenuWorkshopEditorUI.editButton.PositionOffset_X = -305f;
			MenuWorkshopEditorUI.editButton.PositionOffset_Y = 290f;
			MenuWorkshopEditorUI.editButton.PositionScale_X = 0.5f;
			MenuWorkshopEditorUI.editButton.SizeOffset_X = 200f;
			MenuWorkshopEditorUI.editButton.SizeOffset_Y = 30f;
			MenuWorkshopEditorUI.editButton.text = local.format("Edit_Button");
			MenuWorkshopEditorUI.editButton.tooltip = local.format("Edit_Button_Tooltip");
			MenuWorkshopEditorUI.editButton.iconColor = 2;
			MenuWorkshopEditorUI.editButton.onClickedButton += new ClickedButton(MenuWorkshopEditorUI.onClickedEditButton);
			MenuWorkshopEditorUI.container.AddChild(MenuWorkshopEditorUI.editButton);
			MenuWorkshopEditorUI.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuWorkshopEditorUI.backButton.PositionOffset_Y = -50f;
			MenuWorkshopEditorUI.backButton.PositionScale_Y = 1f;
			MenuWorkshopEditorUI.backButton.SizeOffset_X = 200f;
			MenuWorkshopEditorUI.backButton.SizeOffset_Y = 50f;
			MenuWorkshopEditorUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuWorkshopEditorUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			MenuWorkshopEditorUI.backButton.onClickedButton += new ClickedButton(MenuWorkshopEditorUI.onClickedBackButton);
			MenuWorkshopEditorUI.backButton.fontSize = 3;
			MenuWorkshopEditorUI.backButton.iconColor = 2;
			MenuWorkshopEditorUI.container.AddChild(MenuWorkshopEditorUI.backButton);
			MenuWorkshopEditorUI.onLevelsRefreshed();
			Level.onLevelsRefreshed = (LevelsRefreshed)Delegate.Combine(Level.onLevelsRefreshed, new LevelsRefreshed(MenuWorkshopEditorUI.onLevelsRefreshed));
		}

		// Token: 0x04002B56 RID: 11094
		public static Bundle icons;

		// Token: 0x04002B57 RID: 11095
		private static SleekFullscreenBox container;

		// Token: 0x04002B58 RID: 11096
		public static bool active;

		// Token: 0x04002B59 RID: 11097
		private static SleekButtonIcon backButton;

		// Token: 0x04002B5A RID: 11098
		private static LevelInfo[] levels;

		// Token: 0x04002B5B RID: 11099
		private static ISleekBox previewBox;

		// Token: 0x04002B5C RID: 11100
		private static ISleekImage previewImage;

		// Token: 0x04002B5D RID: 11101
		private static ISleekScrollView levelScrollBox;

		// Token: 0x04002B5E RID: 11102
		private static SleekLevel[] levelButtons;

		// Token: 0x04002B5F RID: 11103
		private static ISleekField mapNameField;

		// Token: 0x04002B60 RID: 11104
		private static SleekButtonState mapSizeState;

		// Token: 0x04002B61 RID: 11105
		private static SleekButtonState mapTypeState;

		// Token: 0x04002B62 RID: 11106
		private static SleekButtonIcon addButton;

		// Token: 0x04002B63 RID: 11107
		private static SleekButtonIconConfirm removeButton;

		// Token: 0x04002B64 RID: 11108
		private static SleekButtonIcon editButton;

		// Token: 0x04002B65 RID: 11109
		private static ISleekBox selectedBox;

		// Token: 0x04002B66 RID: 11110
		private static ISleekBox descriptionBox;
	}
}
