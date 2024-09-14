using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007B2 RID: 1970
	public class MenuSurvivorsUI
	{
		// Token: 0x0600421F RID: 16927 RVA: 0x00167B66 File Offset: 0x00165D66
		public static void open()
		{
			if (MenuSurvivorsUI.active)
			{
				return;
			}
			MenuSurvivorsUI.active = true;
			MenuSurvivorsUI.container.AnimateIntoView();
		}

		// Token: 0x06004220 RID: 16928 RVA: 0x00167B80 File Offset: 0x00165D80
		public static void close()
		{
			if (!MenuSurvivorsUI.active)
			{
				return;
			}
			MenuSurvivorsUI.active = false;
			Characters.save();
			MenuSurvivorsUI.container.AnimateOutOfView(0f, -1f);
		}

		// Token: 0x06004221 RID: 16929 RVA: 0x00167BA9 File Offset: 0x00165DA9
		private static void onClickedCharacterButton(ISleekElement button)
		{
			MenuSurvivorsCharacterUI.open();
			MenuSurvivorsUI.close();
		}

		// Token: 0x06004222 RID: 16930 RVA: 0x00167BB5 File Offset: 0x00165DB5
		private static void onClickedAppearanceButton(ISleekElement button)
		{
			MenuSurvivorsAppearanceUI.open();
			MenuSurvivorsUI.close();
		}

		// Token: 0x06004223 RID: 16931 RVA: 0x00167BC1 File Offset: 0x00165DC1
		private static void onClickedGroupButton(ISleekElement button)
		{
			MenuSurvivorsGroupUI.open();
			MenuSurvivorsUI.close();
		}

		// Token: 0x06004224 RID: 16932 RVA: 0x00167BCD File Offset: 0x00165DCD
		private static void onClickedClothingButton(ISleekElement button)
		{
			MenuSurvivorsClothingUI.open();
			MenuSurvivorsUI.close();
		}

		// Token: 0x06004225 RID: 16933 RVA: 0x00167BD9 File Offset: 0x00165DD9
		private static void onClickedBackButton(ISleekElement button)
		{
			MenuDashboardUI.open();
			MenuTitleUI.open();
			MenuSurvivorsUI.close();
		}

		// Token: 0x06004226 RID: 16934 RVA: 0x00167BEA File Offset: 0x00165DEA
		public void OnDestroy()
		{
			this.characterUI.OnDestroy();
			this.appearanceUI.OnDestroy();
			this.groupUI.OnDestroy();
			this.clothingUI.OnDestroy();
		}

		// Token: 0x06004227 RID: 16935 RVA: 0x00167C18 File Offset: 0x00165E18
		public MenuSurvivorsUI()
		{
			Local local = Localization.read("/Menu/Survivors/MenuSurvivors.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Survivors/MenuSurvivors/MenuSurvivors.unity3d");
			MenuSurvivorsUI.container = new SleekFullscreenBox();
			MenuSurvivorsUI.container.PositionOffset_X = 10f;
			MenuSurvivorsUI.container.PositionOffset_Y = 10f;
			MenuSurvivorsUI.container.PositionScale_Y = -1f;
			MenuSurvivorsUI.container.SizeOffset_X = -20f;
			MenuSurvivorsUI.container.SizeOffset_Y = -20f;
			MenuSurvivorsUI.container.SizeScale_X = 1f;
			MenuSurvivorsUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuSurvivorsUI.container);
			MenuSurvivorsUI.active = false;
			MenuSurvivorsUI.characterButton = new SleekButtonIcon(bundle.load<Texture2D>("Character"));
			MenuSurvivorsUI.characterButton.PositionOffset_X = -100f;
			MenuSurvivorsUI.characterButton.PositionOffset_Y = -145f;
			MenuSurvivorsUI.characterButton.PositionScale_X = 0.5f;
			MenuSurvivorsUI.characterButton.PositionScale_Y = 0.5f;
			MenuSurvivorsUI.characterButton.SizeOffset_X = 200f;
			MenuSurvivorsUI.characterButton.SizeOffset_Y = 50f;
			MenuSurvivorsUI.characterButton.text = local.format("CharacterButtonText");
			MenuSurvivorsUI.characterButton.tooltip = local.format("CharacterButtonTooltip");
			MenuSurvivorsUI.characterButton.onClickedButton += new ClickedButton(MenuSurvivorsUI.onClickedCharacterButton);
			MenuSurvivorsUI.characterButton.fontSize = 3;
			MenuSurvivorsUI.characterButton.iconColor = 2;
			MenuSurvivorsUI.container.AddChild(MenuSurvivorsUI.characterButton);
			MenuSurvivorsUI.appearanceButton = new SleekButtonIcon(bundle.load<Texture2D>("Appearance"));
			MenuSurvivorsUI.appearanceButton.PositionOffset_X = -100f;
			MenuSurvivorsUI.appearanceButton.PositionOffset_Y = -85f;
			MenuSurvivorsUI.appearanceButton.PositionScale_X = 0.5f;
			MenuSurvivorsUI.appearanceButton.PositionScale_Y = 0.5f;
			MenuSurvivorsUI.appearanceButton.SizeOffset_X = 200f;
			MenuSurvivorsUI.appearanceButton.SizeOffset_Y = 50f;
			MenuSurvivorsUI.appearanceButton.text = local.format("AppearanceButtonText");
			MenuSurvivorsUI.appearanceButton.tooltip = local.format("AppearanceButtonTooltip");
			MenuSurvivorsUI.appearanceButton.onClickedButton += new ClickedButton(MenuSurvivorsUI.onClickedAppearanceButton);
			MenuSurvivorsUI.appearanceButton.fontSize = 3;
			MenuSurvivorsUI.appearanceButton.iconColor = 2;
			MenuSurvivorsUI.container.AddChild(MenuSurvivorsUI.appearanceButton);
			MenuSurvivorsUI.groupButton = new SleekButtonIcon(bundle.load<Texture2D>("Group"));
			MenuSurvivorsUI.groupButton.PositionOffset_X = -100f;
			MenuSurvivorsUI.groupButton.PositionOffset_Y = -25f;
			MenuSurvivorsUI.groupButton.PositionScale_X = 0.5f;
			MenuSurvivorsUI.groupButton.PositionScale_Y = 0.5f;
			MenuSurvivorsUI.groupButton.SizeOffset_X = 200f;
			MenuSurvivorsUI.groupButton.SizeOffset_Y = 50f;
			MenuSurvivorsUI.groupButton.text = local.format("GroupButtonText");
			MenuSurvivorsUI.groupButton.tooltip = local.format("GroupButtonTooltip");
			MenuSurvivorsUI.groupButton.onClickedButton += new ClickedButton(MenuSurvivorsUI.onClickedGroupButton);
			MenuSurvivorsUI.groupButton.iconColor = 2;
			MenuSurvivorsUI.groupButton.fontSize = 3;
			MenuSurvivorsUI.container.AddChild(MenuSurvivorsUI.groupButton);
			MenuSurvivorsUI.clothingButton = new SleekButtonIcon(bundle.load<Texture2D>("Clothing"));
			MenuSurvivorsUI.clothingButton.PositionOffset_X = -100f;
			MenuSurvivorsUI.clothingButton.PositionOffset_Y = 35f;
			MenuSurvivorsUI.clothingButton.PositionScale_X = 0.5f;
			MenuSurvivorsUI.clothingButton.PositionScale_Y = 0.5f;
			MenuSurvivorsUI.clothingButton.SizeOffset_X = 200f;
			MenuSurvivorsUI.clothingButton.SizeOffset_Y = 50f;
			MenuSurvivorsUI.clothingButton.text = local.format("ClothingButtonText");
			MenuSurvivorsUI.clothingButton.tooltip = local.format("ClothingButtonTooltip");
			MenuSurvivorsUI.clothingButton.onClickedButton += new ClickedButton(MenuSurvivorsUI.onClickedClothingButton);
			MenuSurvivorsUI.clothingButton.fontSize = 3;
			MenuSurvivorsUI.clothingButton.iconColor = 2;
			MenuSurvivorsUI.container.AddChild(MenuSurvivorsUI.clothingButton);
			MenuSurvivorsUI.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuSurvivorsUI.backButton.PositionOffset_X = -100f;
			MenuSurvivorsUI.backButton.PositionOffset_Y = 95f;
			MenuSurvivorsUI.backButton.PositionScale_X = 0.5f;
			MenuSurvivorsUI.backButton.PositionScale_Y = 0.5f;
			MenuSurvivorsUI.backButton.SizeOffset_X = 200f;
			MenuSurvivorsUI.backButton.SizeOffset_Y = 50f;
			MenuSurvivorsUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuSurvivorsUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			MenuSurvivorsUI.backButton.onClickedButton += new ClickedButton(MenuSurvivorsUI.onClickedBackButton);
			MenuSurvivorsUI.backButton.fontSize = 3;
			MenuSurvivorsUI.backButton.iconColor = 2;
			MenuSurvivorsUI.container.AddChild(MenuSurvivorsUI.backButton);
			bundle.unload();
			this.characterUI = new MenuSurvivorsCharacterUI();
			this.appearanceUI = new MenuSurvivorsAppearanceUI();
			this.groupUI = new MenuSurvivorsGroupUI();
			this.clothingUI = new MenuSurvivorsClothingUI();
		}

		// Token: 0x04002B4B RID: 11083
		private static SleekFullscreenBox container;

		// Token: 0x04002B4C RID: 11084
		public static bool active;

		// Token: 0x04002B4D RID: 11085
		private static SleekButtonIcon characterButton;

		// Token: 0x04002B4E RID: 11086
		private static SleekButtonIcon appearanceButton;

		// Token: 0x04002B4F RID: 11087
		private static SleekButtonIcon groupButton;

		// Token: 0x04002B50 RID: 11088
		private static SleekButtonIcon clothingButton;

		// Token: 0x04002B51 RID: 11089
		private static SleekButtonIcon backButton;

		// Token: 0x04002B52 RID: 11090
		private MenuSurvivorsCharacterUI characterUI;

		// Token: 0x04002B53 RID: 11091
		private MenuSurvivorsAppearanceUI appearanceUI;

		// Token: 0x04002B54 RID: 11092
		private MenuSurvivorsGroupUI groupUI;

		// Token: 0x04002B55 RID: 11093
		private MenuSurvivorsClothingUI clothingUI;
	}
}
