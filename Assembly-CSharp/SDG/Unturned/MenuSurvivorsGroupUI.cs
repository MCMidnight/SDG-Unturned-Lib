using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007B1 RID: 1969
	public class MenuSurvivorsGroupUI
	{
		// Token: 0x06004215 RID: 16917 RVA: 0x001675D1 File Offset: 0x001657D1
		public static void open()
		{
			if (MenuSurvivorsGroupUI.active)
			{
				return;
			}
			MenuSurvivorsGroupUI.active = true;
			MenuSurvivorsGroupUI.container.AnimateIntoView();
		}

		// Token: 0x06004216 RID: 16918 RVA: 0x001675EB File Offset: 0x001657EB
		public static void close()
		{
			if (!MenuSurvivorsGroupUI.active)
			{
				return;
			}
			MenuSurvivorsGroupUI.active = false;
			MenuSurvivorsGroupUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x06004217 RID: 16919 RVA: 0x00167610 File Offset: 0x00165810
		private static void onCharacterUpdated(byte index, Character character)
		{
			if (index == Characters.selected)
			{
				MenuSurvivorsGroupUI.markerColorPicker.state = character.markerColor;
				for (int i = 0; i < MenuSurvivorsGroupUI.groups.Length; i++)
				{
					if (MenuSurvivorsGroupUI.groups[i].steamID == character.group)
					{
						MenuSurvivorsGroupUI.groupButton.text = MenuSurvivorsGroupUI.groups[i].name;
						MenuSurvivorsGroupUI.groupButton.icon = MenuSurvivorsGroupUI.groups[i].icon;
						return;
					}
				}
				MenuSurvivorsGroupUI.groupButton.text = MenuSurvivorsGroupUI.localization.format("Group_Box");
				MenuSurvivorsGroupUI.groupButton.icon = null;
			}
		}

		// Token: 0x06004218 RID: 16920 RVA: 0x001676B4 File Offset: 0x001658B4
		private static void onTypedNickField(ISleekField field, string text)
		{
			Characters.renick(text);
		}

		// Token: 0x06004219 RID: 16921 RVA: 0x001676BC File Offset: 0x001658BC
		private static void onPickedMarkerColor(SleekColorPicker picker, Color state)
		{
			Characters.paintMarkerColor(state);
		}

		// Token: 0x0600421A RID: 16922 RVA: 0x001676C4 File Offset: 0x001658C4
		private static void onClickedGroupButton(ISleekElement button)
		{
			Characters.group(MenuSurvivorsGroupUI.groups[Mathf.FloorToInt(button.PositionOffset_Y / 40f)].steamID);
		}

		// Token: 0x0600421B RID: 16923 RVA: 0x001676E7 File Offset: 0x001658E7
		private static void onClickedUngroupButton(ISleekElement button)
		{
			Characters.ungroup();
		}

		// Token: 0x0600421C RID: 16924 RVA: 0x001676EE File Offset: 0x001658EE
		private static void onClickedBackButton(ISleekElement button)
		{
			MenuSurvivorsUI.open();
			MenuSurvivorsGroupUI.close();
		}

		// Token: 0x0600421D RID: 16925 RVA: 0x001676FA File Offset: 0x001658FA
		public void OnDestroy()
		{
			Characters.onCharacterUpdated = (CharacterUpdated)Delegate.Remove(Characters.onCharacterUpdated, new CharacterUpdated(MenuSurvivorsGroupUI.onCharacterUpdated));
		}

		// Token: 0x0600421E RID: 16926 RVA: 0x0016771C File Offset: 0x0016591C
		public MenuSurvivorsGroupUI()
		{
			MenuSurvivorsGroupUI.localization = Localization.read("/Menu/Survivors/MenuSurvivorsGroup.dat");
			MenuSurvivorsGroupUI.container = new SleekFullscreenBox();
			MenuSurvivorsGroupUI.container.PositionOffset_X = 10f;
			MenuSurvivorsGroupUI.container.PositionOffset_Y = 10f;
			MenuSurvivorsGroupUI.container.PositionScale_Y = 1f;
			MenuSurvivorsGroupUI.container.SizeOffset_X = -20f;
			MenuSurvivorsGroupUI.container.SizeOffset_Y = -20f;
			MenuSurvivorsGroupUI.container.SizeScale_X = 1f;
			MenuSurvivorsGroupUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuSurvivorsGroupUI.container);
			MenuSurvivorsGroupUI.active = false;
			MenuSurvivorsGroupUI.groups = Provider.provider.communityService.getGroups();
			MenuSurvivorsGroupUI.markerColorBox = Glazier.Get().CreateBox();
			MenuSurvivorsGroupUI.markerColorBox.PositionOffset_X = -120f;
			MenuSurvivorsGroupUI.markerColorBox.PositionOffset_Y = 100f;
			MenuSurvivorsGroupUI.markerColorBox.PositionScale_X = 0.75f;
			MenuSurvivorsGroupUI.markerColorBox.SizeOffset_X = 240f;
			MenuSurvivorsGroupUI.markerColorBox.SizeOffset_Y = 30f;
			MenuSurvivorsGroupUI.markerColorBox.Text = MenuSurvivorsGroupUI.localization.format("Marker_Color_Box");
			MenuSurvivorsGroupUI.container.AddChild(MenuSurvivorsGroupUI.markerColorBox);
			MenuSurvivorsGroupUI.markerColorPicker = new SleekColorPicker();
			MenuSurvivorsGroupUI.markerColorPicker.PositionOffset_X = -120f;
			MenuSurvivorsGroupUI.markerColorPicker.PositionOffset_Y = 140f;
			MenuSurvivorsGroupUI.markerColorPicker.PositionScale_X = 0.75f;
			MenuSurvivorsGroupUI.markerColorPicker.onColorPicked = new ColorPicked(MenuSurvivorsGroupUI.onPickedMarkerColor);
			MenuSurvivorsGroupUI.container.AddChild(MenuSurvivorsGroupUI.markerColorPicker);
			MenuSurvivorsGroupUI.groupButton = new SleekButtonIcon(null, 20);
			MenuSurvivorsGroupUI.groupButton.PositionOffset_X = -120f;
			MenuSurvivorsGroupUI.groupButton.PositionOffset_Y = 270f;
			MenuSurvivorsGroupUI.groupButton.PositionScale_X = 0.75f;
			MenuSurvivorsGroupUI.groupButton.SizeOffset_X = 240f;
			MenuSurvivorsGroupUI.groupButton.SizeOffset_Y = 30f;
			MenuSurvivorsGroupUI.groupButton.AddLabel(MenuSurvivorsGroupUI.localization.format("Group_Box_Label"), 0);
			MenuSurvivorsGroupUI.groupButton.onClickedButton += new ClickedButton(MenuSurvivorsGroupUI.onClickedUngroupButton);
			MenuSurvivorsGroupUI.container.AddChild(MenuSurvivorsGroupUI.groupButton);
			MenuSurvivorsGroupUI.groupsBox = Glazier.Get().CreateScrollView();
			MenuSurvivorsGroupUI.groupsBox.PositionOffset_X = -120f;
			MenuSurvivorsGroupUI.groupsBox.PositionOffset_Y = 310f;
			MenuSurvivorsGroupUI.groupsBox.PositionScale_X = 0.75f;
			MenuSurvivorsGroupUI.groupsBox.SizeOffset_X = 270f;
			MenuSurvivorsGroupUI.groupsBox.SizeOffset_Y = -410f;
			MenuSurvivorsGroupUI.groupsBox.SizeScale_Y = 1f;
			MenuSurvivorsGroupUI.groupsBox.ScaleContentToWidth = true;
			MenuSurvivorsGroupUI.groupsBox.ContentSizeOffset = new Vector2(0f, (float)(MenuSurvivorsGroupUI.groups.Length * 40 - 10));
			MenuSurvivorsGroupUI.container.AddChild(MenuSurvivorsGroupUI.groupsBox);
			for (int i = 0; i < MenuSurvivorsGroupUI.groups.Length; i++)
			{
				SleekButtonIcon sleekButtonIcon = new SleekButtonIcon(MenuSurvivorsGroupUI.groups[i].icon, 20);
				sleekButtonIcon.PositionOffset_Y = (float)(i * 40);
				sleekButtonIcon.SizeOffset_X = 240f;
				sleekButtonIcon.SizeOffset_Y = 30f;
				sleekButtonIcon.text = MenuSurvivorsGroupUI.groups[i].name;
				sleekButtonIcon.onClickedButton += new ClickedButton(MenuSurvivorsGroupUI.onClickedGroupButton);
				MenuSurvivorsGroupUI.groupsBox.AddChild(sleekButtonIcon);
			}
			Characters.onCharacterUpdated = (CharacterUpdated)Delegate.Combine(Characters.onCharacterUpdated, new CharacterUpdated(MenuSurvivorsGroupUI.onCharacterUpdated));
			MenuSurvivorsGroupUI.onCharacterUpdated(Characters.selected, Characters.list[(int)Characters.selected]);
			MenuSurvivorsGroupUI.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuSurvivorsGroupUI.backButton.PositionOffset_Y = -50f;
			MenuSurvivorsGroupUI.backButton.PositionScale_Y = 1f;
			MenuSurvivorsGroupUI.backButton.SizeOffset_X = 200f;
			MenuSurvivorsGroupUI.backButton.SizeOffset_Y = 50f;
			MenuSurvivorsGroupUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuSurvivorsGroupUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			MenuSurvivorsGroupUI.backButton.onClickedButton += new ClickedButton(MenuSurvivorsGroupUI.onClickedBackButton);
			MenuSurvivorsGroupUI.backButton.fontSize = 3;
			MenuSurvivorsGroupUI.backButton.iconColor = 2;
			MenuSurvivorsGroupUI.container.AddChild(MenuSurvivorsGroupUI.backButton);
		}

		// Token: 0x04002B42 RID: 11074
		private static Local localization;

		// Token: 0x04002B43 RID: 11075
		private static SleekFullscreenBox container;

		// Token: 0x04002B44 RID: 11076
		public static bool active;

		// Token: 0x04002B45 RID: 11077
		private static SleekButtonIcon backButton;

		// Token: 0x04002B46 RID: 11078
		private static SteamGroup[] groups;

		// Token: 0x04002B47 RID: 11079
		private static ISleekBox markerColorBox;

		// Token: 0x04002B48 RID: 11080
		private static SleekColorPicker markerColorPicker;

		// Token: 0x04002B49 RID: 11081
		private static SleekButtonIcon groupButton;

		// Token: 0x04002B4A RID: 11082
		private static ISleekScrollView groupsBox;
	}
}
