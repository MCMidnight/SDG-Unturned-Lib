using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007A7 RID: 1959
	public class MenuSurvivorsAppearanceUI
	{
		// Token: 0x060041A4 RID: 16804 RVA: 0x0015FCBC File Offset: 0x0015DEBC
		public static void open()
		{
			if (MenuSurvivorsAppearanceUI.active)
			{
				return;
			}
			MenuSurvivorsAppearanceUI.active = true;
			Characters.apply(false, false);
			MenuSurvivorsAppearanceUI.container.AnimateIntoView();
		}

		// Token: 0x060041A5 RID: 16805 RVA: 0x0015FCDD File Offset: 0x0015DEDD
		public static void close()
		{
			if (!MenuSurvivorsAppearanceUI.active)
			{
				return;
			}
			MenuSurvivorsAppearanceUI.active = false;
			Characters.apply(true, true);
			MenuSurvivorsAppearanceUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x060041A6 RID: 16806 RVA: 0x0015FD08 File Offset: 0x0015DF08
		private static void updateFaces(Color color)
		{
			for (int i = 0; i < MenuSurvivorsAppearanceUI.faceButtons.Length; i++)
			{
				((ISleekImage)MenuSurvivorsAppearanceUI.faceButtons[i].GetChildAtIndex(0)).TintColor = color;
			}
		}

		// Token: 0x060041A7 RID: 16807 RVA: 0x0015FD44 File Offset: 0x0015DF44
		private static void updateColors(Color color)
		{
			for (int i = 1; i < MenuSurvivorsAppearanceUI.hairButtons.Length; i++)
			{
				((ISleekImage)MenuSurvivorsAppearanceUI.hairButtons[i].GetChildAtIndex(0)).TintColor = color;
			}
			for (int j = 1; j < MenuSurvivorsAppearanceUI.beardButtons.Length; j++)
			{
				((ISleekImage)MenuSurvivorsAppearanceUI.beardButtons[j].GetChildAtIndex(0)).TintColor = color;
			}
		}

		// Token: 0x060041A8 RID: 16808 RVA: 0x0015FDB0 File Offset: 0x0015DFB0
		private static void onCharacterUpdated(byte index, Character character)
		{
			if (index == Characters.selected)
			{
				MenuSurvivorsAppearanceUI.skinColorPicker.state = character.skin;
				MenuSurvivorsAppearanceUI.colorColorPicker.state = character.color;
				MenuSurvivorsAppearanceUI.updateFaces(character.skin);
				MenuSurvivorsAppearanceUI.updateColors(character.color);
				MenuSurvivorsAppearanceUI.handState.state = (character.hand ? 1 : 0);
			}
		}

		// Token: 0x060041A9 RID: 16809 RVA: 0x0015FE11 File Offset: 0x0015E011
		private static void onClickedFaceButton(ISleekElement button)
		{
			Characters.growFace((byte)Mathf.FloorToInt(button.PositionOffset_X / 50f + (button.PositionOffset_Y - 40f) / 50f * 5f));
		}

		// Token: 0x060041AA RID: 16810 RVA: 0x0015FE43 File Offset: 0x0015E043
		private static void onClickedHairButton(ISleekElement button)
		{
			Characters.growHair((byte)Mathf.FloorToInt(button.PositionOffset_X / 50f + (button.PositionOffset_Y - 40f) / 50f * 5f));
		}

		// Token: 0x060041AB RID: 16811 RVA: 0x0015FE75 File Offset: 0x0015E075
		private static void onClickedBeardButton(ISleekElement button)
		{
			Characters.growBeard((byte)Mathf.FloorToInt(button.PositionOffset_X / 50f + (button.PositionOffset_Y - 40f) / 50f * 5f));
		}

		// Token: 0x060041AC RID: 16812 RVA: 0x0015FEA8 File Offset: 0x0015E0A8
		private static void onClickedSkinButton(ISleekElement button)
		{
			int num = Mathf.FloorToInt(button.PositionOffset_X / 50f + (button.PositionOffset_Y - 40f) / 50f * 5f);
			Color color = Customization.SKINS[num];
			Characters.paintSkin(color);
			MenuSurvivorsAppearanceUI.skinColorPicker.state = color;
			MenuSurvivorsAppearanceUI.updateFaces(color);
		}

		// Token: 0x060041AD RID: 16813 RVA: 0x0015FF03 File Offset: 0x0015E103
		private static void onSkinColorPicked(SleekColorPicker picker, Color color)
		{
			Characters.paintSkin(color);
			MenuSurvivorsAppearanceUI.updateFaces(color);
		}

		// Token: 0x060041AE RID: 16814 RVA: 0x0015FF14 File Offset: 0x0015E114
		private static void onClickedColorButton(ISleekElement button)
		{
			int num = Mathf.FloorToInt(button.PositionOffset_X / 50f + (button.PositionOffset_Y - 40f) / 50f * 5f);
			Color color = Customization.COLORS[num];
			Characters.paintColor(color);
			MenuSurvivorsAppearanceUI.colorColorPicker.state = color;
			MenuSurvivorsAppearanceUI.updateColors(color);
		}

		// Token: 0x060041AF RID: 16815 RVA: 0x0015FF6F File Offset: 0x0015E16F
		private static void onColorColorPicked(SleekColorPicker picker, Color color)
		{
			Characters.paintColor(color);
			MenuSurvivorsAppearanceUI.updateColors(color);
		}

		// Token: 0x060041B0 RID: 16816 RVA: 0x0015FF7D File Offset: 0x0015E17D
		private static void onSwappedHandState(SleekButtonState button, int index)
		{
			Characters.hand(index != 0);
		}

		// Token: 0x060041B1 RID: 16817 RVA: 0x0015FF88 File Offset: 0x0015E188
		private static void onDraggedCharacterSlider(ISleekSlider slider, float state)
		{
			Characters.characterYaw = state * 360f;
		}

		// Token: 0x060041B2 RID: 16818 RVA: 0x0015FF96 File Offset: 0x0015E196
		private static void onClickedBackButton(ISleekElement button)
		{
			MenuSurvivorsUI.open();
			MenuSurvivorsAppearanceUI.close();
		}

		// Token: 0x060041B3 RID: 16819 RVA: 0x0015FFA2 File Offset: 0x0015E1A2
		public void OnDestroy()
		{
			Characters.onCharacterUpdated = (CharacterUpdated)Delegate.Remove(Characters.onCharacterUpdated, new CharacterUpdated(MenuSurvivorsAppearanceUI.onCharacterUpdated));
		}

		// Token: 0x060041B4 RID: 16820 RVA: 0x0015FFC4 File Offset: 0x0015E1C4
		public MenuSurvivorsAppearanceUI()
		{
			MenuSurvivorsAppearanceUI.localization = Localization.read("/Menu/Survivors/MenuSurvivorsAppearance.dat");
			MenuSurvivorsAppearanceUI.container = new SleekFullscreenBox();
			MenuSurvivorsAppearanceUI.container.PositionOffset_X = 10f;
			MenuSurvivorsAppearanceUI.container.PositionOffset_Y = 10f;
			MenuSurvivorsAppearanceUI.container.PositionScale_Y = 1f;
			MenuSurvivorsAppearanceUI.container.SizeOffset_X = -20f;
			MenuSurvivorsAppearanceUI.container.SizeOffset_Y = -20f;
			MenuSurvivorsAppearanceUI.container.SizeScale_X = 1f;
			MenuSurvivorsAppearanceUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuSurvivorsAppearanceUI.container);
			MenuSurvivorsAppearanceUI.active = false;
			MenuSurvivorsAppearanceUI.customizationBox = Glazier.Get().CreateScrollView();
			MenuSurvivorsAppearanceUI.customizationBox.PositionOffset_X = -140f;
			MenuSurvivorsAppearanceUI.customizationBox.PositionOffset_Y = 100f;
			MenuSurvivorsAppearanceUI.customizationBox.PositionScale_X = 0.75f;
			MenuSurvivorsAppearanceUI.customizationBox.SizeOffset_X = 270f;
			MenuSurvivorsAppearanceUI.customizationBox.SizeOffset_Y = -270f;
			MenuSurvivorsAppearanceUI.customizationBox.SizeScale_Y = 1f;
			MenuSurvivorsAppearanceUI.container.AddChild(MenuSurvivorsAppearanceUI.customizationBox);
			MenuSurvivorsAppearanceUI.faceBox = Glazier.Get().CreateBox();
			MenuSurvivorsAppearanceUI.faceBox.SizeOffset_X = 240f;
			MenuSurvivorsAppearanceUI.faceBox.SizeOffset_Y = 30f;
			MenuSurvivorsAppearanceUI.faceBox.Text = MenuSurvivorsAppearanceUI.localization.format("Face_Box");
			MenuSurvivorsAppearanceUI.faceBox.TooltipText = MenuSurvivorsAppearanceUI.localization.format("Face_Box_Tooltip");
			MenuSurvivorsAppearanceUI.customizationBox.AddChild(MenuSurvivorsAppearanceUI.faceBox);
			MenuSurvivorsAppearanceUI.faceButtons = new ISleekButton[(int)(Customization.FACES_FREE + Customization.FACES_PRO)];
			for (int i = 0; i < MenuSurvivorsAppearanceUI.faceButtons.Length; i++)
			{
				ISleekButton sleekButton = Glazier.Get().CreateButton();
				sleekButton.PositionOffset_X = (float)(i % 5 * 50);
				sleekButton.PositionOffset_Y = (float)(40 + Mathf.FloorToInt((float)i / 5f) * 50);
				sleekButton.SizeOffset_X = 40f;
				sleekButton.SizeOffset_Y = 40f;
				MenuSurvivorsAppearanceUI.faceBox.AddChild(sleekButton);
				ISleekImage sleekImage = Glazier.Get().CreateImage();
				sleekImage.PositionOffset_X = 10f;
				sleekImage.PositionOffset_Y = 10f;
				sleekImage.SizeOffset_X = 20f;
				sleekImage.SizeOffset_Y = 20f;
				sleekImage.Texture = GlazierResources.PixelTexture;
				sleekButton.AddChild(sleekImage);
				ISleekImage sleekImage2 = Glazier.Get().CreateImage();
				sleekImage2.PositionOffset_X = 2f;
				sleekImage2.PositionOffset_Y = 2f;
				sleekImage2.SizeOffset_X = 16f;
				sleekImage2.SizeOffset_Y = 16f;
				sleekImage2.Texture = (Texture2D)Resources.Load("Faces/" + i.ToString() + "/Texture");
				sleekImage.AddChild(sleekImage2);
				if (i >= (int)Customization.FACES_FREE)
				{
					if (Provider.isPro)
					{
						sleekButton.OnClicked += new ClickedButton(MenuSurvivorsAppearanceUI.onClickedFaceButton);
					}
					else
					{
						sleekButton.BackgroundColor = SleekColor.BackgroundIfLight(Palette.PRO);
						Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro/Pro.unity3d");
						ISleekImage sleekImage3 = Glazier.Get().CreateImage();
						sleekImage3.PositionOffset_X = -10f;
						sleekImage3.PositionOffset_Y = -10f;
						sleekImage3.PositionScale_X = 0.5f;
						sleekImage3.PositionScale_Y = 0.5f;
						sleekImage3.SizeOffset_X = 20f;
						sleekImage3.SizeOffset_Y = 20f;
						sleekImage3.Texture = bundle.load<Texture2D>("Lock_Small");
						sleekButton.AddChild(sleekImage3);
						bundle.unload();
					}
				}
				else
				{
					sleekButton.OnClicked += new ClickedButton(MenuSurvivorsAppearanceUI.onClickedFaceButton);
				}
				MenuSurvivorsAppearanceUI.faceButtons[i] = sleekButton;
			}
			MenuSurvivorsAppearanceUI.hairBox = Glazier.Get().CreateBox();
			MenuSurvivorsAppearanceUI.hairBox.PositionOffset_Y = (float)(80 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.faceButtons.Length / 5f) * 50);
			MenuSurvivorsAppearanceUI.hairBox.SizeOffset_X = 240f;
			MenuSurvivorsAppearanceUI.hairBox.SizeOffset_Y = 30f;
			MenuSurvivorsAppearanceUI.hairBox.Text = MenuSurvivorsAppearanceUI.localization.format("Hair_Box");
			MenuSurvivorsAppearanceUI.hairBox.TooltipText = MenuSurvivorsAppearanceUI.localization.format("Hair_Box_Tooltip");
			MenuSurvivorsAppearanceUI.customizationBox.AddChild(MenuSurvivorsAppearanceUI.hairBox);
			MenuSurvivorsAppearanceUI.hairButtons = new ISleekButton[(int)(Customization.HAIRS_FREE + Customization.HAIRS_PRO)];
			for (int j = 0; j < MenuSurvivorsAppearanceUI.hairButtons.Length; j++)
			{
				ISleekButton sleekButton2 = Glazier.Get().CreateButton();
				sleekButton2.PositionOffset_X = (float)(j % 5 * 50);
				sleekButton2.PositionOffset_Y = (float)(40 + Mathf.FloorToInt((float)j / 5f) * 50);
				sleekButton2.SizeOffset_X = 40f;
				sleekButton2.SizeOffset_Y = 40f;
				MenuSurvivorsAppearanceUI.hairBox.AddChild(sleekButton2);
				ISleekImage sleekImage4 = Glazier.Get().CreateImage();
				sleekImage4.PositionOffset_X = 10f;
				sleekImage4.PositionOffset_Y = 10f;
				sleekImage4.SizeOffset_X = 20f;
				sleekImage4.SizeOffset_Y = 20f;
				sleekImage4.Texture = (Texture2D)Resources.Load("Hairs/" + j.ToString() + "/Texture");
				sleekButton2.AddChild(sleekImage4);
				if (j >= (int)Customization.HAIRS_FREE)
				{
					if (Provider.isPro)
					{
						sleekButton2.OnClicked += new ClickedButton(MenuSurvivorsAppearanceUI.onClickedHairButton);
					}
					else
					{
						sleekButton2.BackgroundColor = SleekColor.BackgroundIfLight(Palette.PRO);
						Bundle bundle2 = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro/Pro.unity3d");
						ISleekImage sleekImage5 = Glazier.Get().CreateImage();
						sleekImage5.PositionOffset_X = -10f;
						sleekImage5.PositionOffset_Y = -10f;
						sleekImage5.PositionScale_X = 0.5f;
						sleekImage5.PositionScale_Y = 0.5f;
						sleekImage5.SizeOffset_X = 20f;
						sleekImage5.SizeOffset_Y = 20f;
						sleekImage5.Texture = bundle2.load<Texture2D>("Lock_Small");
						sleekButton2.AddChild(sleekImage5);
						bundle2.unload();
					}
				}
				else
				{
					sleekButton2.OnClicked += new ClickedButton(MenuSurvivorsAppearanceUI.onClickedHairButton);
				}
				MenuSurvivorsAppearanceUI.hairButtons[j] = sleekButton2;
			}
			MenuSurvivorsAppearanceUI.beardBox = Glazier.Get().CreateBox();
			MenuSurvivorsAppearanceUI.beardBox.PositionOffset_Y = (float)(160 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.faceButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.hairButtons.Length / 5f) * 50);
			MenuSurvivorsAppearanceUI.beardBox.SizeOffset_X = 240f;
			MenuSurvivorsAppearanceUI.beardBox.SizeOffset_Y = 30f;
			MenuSurvivorsAppearanceUI.beardBox.Text = MenuSurvivorsAppearanceUI.localization.format("Beard_Box");
			MenuSurvivorsAppearanceUI.beardBox.TooltipText = MenuSurvivorsAppearanceUI.localization.format("Beard_Box_Tooltip");
			MenuSurvivorsAppearanceUI.customizationBox.AddChild(MenuSurvivorsAppearanceUI.beardBox);
			MenuSurvivorsAppearanceUI.beardButtons = new ISleekButton[(int)(Customization.BEARDS_FREE + Customization.BEARDS_PRO)];
			for (int k = 0; k < MenuSurvivorsAppearanceUI.beardButtons.Length; k++)
			{
				ISleekButton sleekButton3 = Glazier.Get().CreateButton();
				sleekButton3.PositionOffset_X = (float)(k % 5 * 50);
				sleekButton3.PositionOffset_Y = (float)(40 + Mathf.FloorToInt((float)k / 5f) * 50);
				sleekButton3.SizeOffset_X = 40f;
				sleekButton3.SizeOffset_Y = 40f;
				MenuSurvivorsAppearanceUI.beardBox.AddChild(sleekButton3);
				ISleekImage sleekImage6 = Glazier.Get().CreateImage();
				sleekImage6.PositionOffset_X = 10f;
				sleekImage6.PositionOffset_Y = 10f;
				sleekImage6.SizeOffset_X = 20f;
				sleekImage6.SizeOffset_Y = 20f;
				sleekImage6.Texture = (Texture2D)Resources.Load("Beards/" + k.ToString() + "/Texture");
				sleekButton3.AddChild(sleekImage6);
				if (k >= (int)Customization.BEARDS_FREE)
				{
					if (Provider.isPro)
					{
						sleekButton3.OnClicked += new ClickedButton(MenuSurvivorsAppearanceUI.onClickedBeardButton);
					}
					else
					{
						sleekButton3.BackgroundColor = SleekColor.BackgroundIfLight(Palette.PRO);
						Bundle bundle3 = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro/Pro.unity3d");
						ISleekImage sleekImage7 = Glazier.Get().CreateImage();
						sleekImage7.PositionOffset_X = -10f;
						sleekImage7.PositionOffset_Y = -10f;
						sleekImage7.PositionScale_X = 0.5f;
						sleekImage7.PositionScale_Y = 0.5f;
						sleekImage7.SizeOffset_X = 20f;
						sleekImage7.SizeOffset_Y = 20f;
						sleekImage7.Texture = bundle3.load<Texture2D>("Lock_Small");
						sleekButton3.AddChild(sleekImage7);
						bundle3.unload();
					}
				}
				else
				{
					sleekButton3.OnClicked += new ClickedButton(MenuSurvivorsAppearanceUI.onClickedBeardButton);
				}
				MenuSurvivorsAppearanceUI.beardButtons[k] = sleekButton3;
			}
			MenuSurvivorsAppearanceUI.skinBox = Glazier.Get().CreateBox();
			MenuSurvivorsAppearanceUI.skinBox.PositionOffset_Y = (float)(240 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.faceButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.hairButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.beardButtons.Length / 5f) * 50);
			MenuSurvivorsAppearanceUI.skinBox.SizeOffset_X = 240f;
			MenuSurvivorsAppearanceUI.skinBox.SizeOffset_Y = 30f;
			MenuSurvivorsAppearanceUI.skinBox.Text = MenuSurvivorsAppearanceUI.localization.format("Skin_Box");
			MenuSurvivorsAppearanceUI.skinBox.TooltipText = MenuSurvivorsAppearanceUI.localization.format("Skin_Box_Tooltip");
			MenuSurvivorsAppearanceUI.customizationBox.AddChild(MenuSurvivorsAppearanceUI.skinBox);
			MenuSurvivorsAppearanceUI.skinButtons = new ISleekButton[Customization.SKINS.Length];
			for (int l = 0; l < MenuSurvivorsAppearanceUI.skinButtons.Length; l++)
			{
				ISleekButton sleekButton4 = Glazier.Get().CreateButton();
				sleekButton4.PositionOffset_X = (float)(l % 5 * 50);
				sleekButton4.PositionOffset_Y = (float)(40 + Mathf.FloorToInt((float)l / 5f) * 50);
				sleekButton4.SizeOffset_X = 40f;
				sleekButton4.SizeOffset_Y = 40f;
				sleekButton4.OnClicked += new ClickedButton(MenuSurvivorsAppearanceUI.onClickedSkinButton);
				MenuSurvivorsAppearanceUI.skinBox.AddChild(sleekButton4);
				ISleekImage sleekImage8 = Glazier.Get().CreateImage();
				sleekImage8.PositionOffset_X = 10f;
				sleekImage8.PositionOffset_Y = 10f;
				sleekImage8.SizeOffset_X = 20f;
				sleekImage8.SizeOffset_Y = 20f;
				sleekImage8.Texture = GlazierResources.PixelTexture;
				sleekImage8.TintColor = Customization.SKINS[l];
				sleekButton4.AddChild(sleekImage8);
				MenuSurvivorsAppearanceUI.skinButtons[l] = sleekButton4;
			}
			MenuSurvivorsAppearanceUI.skinColorPicker = new SleekColorPicker();
			MenuSurvivorsAppearanceUI.skinColorPicker.PositionOffset_Y = (float)(280 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.faceButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.hairButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.beardButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.skinButtons.Length / 5f) * 50);
			MenuSurvivorsAppearanceUI.customizationBox.AddChild(MenuSurvivorsAppearanceUI.skinColorPicker);
			if (Provider.isPro)
			{
				MenuSurvivorsAppearanceUI.skinColorPicker.onColorPicked = new ColorPicked(MenuSurvivorsAppearanceUI.onSkinColorPicked);
			}
			else
			{
				Bundle bundle4 = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro/Pro.unity3d");
				ISleekImage sleekImage9 = Glazier.Get().CreateImage();
				sleekImage9.PositionOffset_X = -40f;
				sleekImage9.PositionOffset_Y = -40f;
				sleekImage9.PositionScale_X = 0.5f;
				sleekImage9.PositionScale_Y = 0.5f;
				sleekImage9.SizeOffset_X = 80f;
				sleekImage9.SizeOffset_Y = 80f;
				sleekImage9.Texture = bundle4.load<Texture2D>("Lock_Large");
				MenuSurvivorsAppearanceUI.skinColorPicker.AddChild(sleekImage9);
				bundle4.unload();
			}
			MenuSurvivorsAppearanceUI.colorBox = Glazier.Get().CreateBox();
			MenuSurvivorsAppearanceUI.colorBox.PositionOffset_Y = (float)(440 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.faceButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.hairButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.beardButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.skinButtons.Length / 5f) * 50);
			MenuSurvivorsAppearanceUI.colorBox.SizeOffset_X = 240f;
			MenuSurvivorsAppearanceUI.colorBox.SizeOffset_Y = 30f;
			MenuSurvivorsAppearanceUI.colorBox.Text = MenuSurvivorsAppearanceUI.localization.format("Color_Box");
			MenuSurvivorsAppearanceUI.colorBox.TooltipText = MenuSurvivorsAppearanceUI.localization.format("Color_Box_Tooltip");
			MenuSurvivorsAppearanceUI.customizationBox.AddChild(MenuSurvivorsAppearanceUI.colorBox);
			MenuSurvivorsAppearanceUI.colorButtons = new ISleekButton[Customization.COLORS.Length];
			for (int m = 0; m < MenuSurvivorsAppearanceUI.colorButtons.Length; m++)
			{
				ISleekButton sleekButton5 = Glazier.Get().CreateButton();
				sleekButton5.PositionOffset_X = (float)(m % 5 * 50);
				sleekButton5.PositionOffset_Y = (float)(40 + Mathf.FloorToInt((float)m / 5f) * 50);
				sleekButton5.SizeOffset_X = 40f;
				sleekButton5.SizeOffset_Y = 40f;
				sleekButton5.OnClicked += new ClickedButton(MenuSurvivorsAppearanceUI.onClickedColorButton);
				MenuSurvivorsAppearanceUI.colorBox.AddChild(sleekButton5);
				ISleekImage sleekImage10 = Glazier.Get().CreateImage();
				sleekImage10.PositionOffset_X = 10f;
				sleekImage10.PositionOffset_Y = 10f;
				sleekImage10.SizeOffset_X = 20f;
				sleekImage10.SizeOffset_Y = 20f;
				sleekImage10.Texture = GlazierResources.PixelTexture;
				sleekImage10.TintColor = Customization.COLORS[m];
				sleekButton5.AddChild(sleekImage10);
				MenuSurvivorsAppearanceUI.colorButtons[m] = sleekButton5;
			}
			MenuSurvivorsAppearanceUI.colorColorPicker = new SleekColorPicker();
			MenuSurvivorsAppearanceUI.colorColorPicker.PositionOffset_Y = (float)(480 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.faceButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.hairButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.beardButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.skinButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.colorButtons.Length / 5f) * 50);
			MenuSurvivorsAppearanceUI.customizationBox.AddChild(MenuSurvivorsAppearanceUI.colorColorPicker);
			if (Provider.isPro)
			{
				MenuSurvivorsAppearanceUI.colorColorPicker.onColorPicked = new ColorPicked(MenuSurvivorsAppearanceUI.onColorColorPicked);
			}
			else
			{
				Bundle bundle5 = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro/Pro.unity3d");
				ISleekImage sleekImage11 = Glazier.Get().CreateImage();
				sleekImage11.PositionOffset_X = -40f;
				sleekImage11.PositionOffset_Y = -40f;
				sleekImage11.PositionScale_X = 0.5f;
				sleekImage11.PositionScale_Y = 0.5f;
				sleekImage11.SizeOffset_X = 80f;
				sleekImage11.SizeOffset_Y = 80f;
				sleekImage11.Texture = bundle5.load<Texture2D>("Lock_Large");
				MenuSurvivorsAppearanceUI.colorColorPicker.AddChild(sleekImage11);
				bundle5.unload();
			}
			MenuSurvivorsAppearanceUI.customizationBox.ScaleContentToWidth = true;
			MenuSurvivorsAppearanceUI.customizationBox.ContentSizeOffset = new Vector2(0f, (float)(600 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.faceButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.hairButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.beardButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.skinButtons.Length / 5f) * 50 + Mathf.CeilToInt((float)MenuSurvivorsAppearanceUI.colorButtons.Length / 5f) * 50));
			MenuSurvivorsAppearanceUI.handState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuSurvivorsAppearanceUI.localization.format("Right")),
				new GUIContent(MenuSurvivorsAppearanceUI.localization.format("Left"))
			});
			MenuSurvivorsAppearanceUI.handState.PositionOffset_X = -140f;
			MenuSurvivorsAppearanceUI.handState.PositionOffset_Y = -160f;
			MenuSurvivorsAppearanceUI.handState.PositionScale_X = 0.75f;
			MenuSurvivorsAppearanceUI.handState.PositionScale_Y = 1f;
			MenuSurvivorsAppearanceUI.handState.SizeOffset_X = 240f;
			MenuSurvivorsAppearanceUI.handState.SizeOffset_Y = 30f;
			MenuSurvivorsAppearanceUI.handState.onSwappedState = new SwappedState(MenuSurvivorsAppearanceUI.onSwappedHandState);
			MenuSurvivorsAppearanceUI.container.AddChild(MenuSurvivorsAppearanceUI.handState);
			MenuSurvivorsAppearanceUI.characterSlider = Glazier.Get().CreateSlider();
			MenuSurvivorsAppearanceUI.characterSlider.PositionOffset_X = -140f;
			MenuSurvivorsAppearanceUI.characterSlider.PositionOffset_Y = -120f;
			MenuSurvivorsAppearanceUI.characterSlider.PositionScale_X = 0.75f;
			MenuSurvivorsAppearanceUI.characterSlider.PositionScale_Y = 1f;
			MenuSurvivorsAppearanceUI.characterSlider.SizeOffset_X = 240f;
			MenuSurvivorsAppearanceUI.characterSlider.SizeOffset_Y = 20f;
			MenuSurvivorsAppearanceUI.characterSlider.Orientation = 0;
			MenuSurvivorsAppearanceUI.characterSlider.OnValueChanged += new Dragged(MenuSurvivorsAppearanceUI.onDraggedCharacterSlider);
			MenuSurvivorsAppearanceUI.container.AddChild(MenuSurvivorsAppearanceUI.characterSlider);
			Characters.onCharacterUpdated = (CharacterUpdated)Delegate.Combine(Characters.onCharacterUpdated, new CharacterUpdated(MenuSurvivorsAppearanceUI.onCharacterUpdated));
			MenuSurvivorsAppearanceUI.onCharacterUpdated(Characters.selected, Characters.list[(int)Characters.selected]);
			MenuSurvivorsAppearanceUI.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuSurvivorsAppearanceUI.backButton.PositionOffset_Y = -50f;
			MenuSurvivorsAppearanceUI.backButton.PositionScale_Y = 1f;
			MenuSurvivorsAppearanceUI.backButton.SizeOffset_X = 200f;
			MenuSurvivorsAppearanceUI.backButton.SizeOffset_Y = 50f;
			MenuSurvivorsAppearanceUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuSurvivorsAppearanceUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			MenuSurvivorsAppearanceUI.backButton.onClickedButton += new ClickedButton(MenuSurvivorsAppearanceUI.onClickedBackButton);
			MenuSurvivorsAppearanceUI.backButton.fontSize = 3;
			MenuSurvivorsAppearanceUI.backButton.iconColor = 2;
			MenuSurvivorsAppearanceUI.container.AddChild(MenuSurvivorsAppearanceUI.backButton);
		}

		// Token: 0x04002A95 RID: 10901
		private static Local localization;

		// Token: 0x04002A96 RID: 10902
		private static SleekFullscreenBox container;

		// Token: 0x04002A97 RID: 10903
		public static bool active;

		// Token: 0x04002A98 RID: 10904
		private static SleekButtonIcon backButton;

		// Token: 0x04002A99 RID: 10905
		private static ISleekScrollView customizationBox;

		// Token: 0x04002A9A RID: 10906
		private static ISleekBox faceBox;

		// Token: 0x04002A9B RID: 10907
		private static ISleekBox hairBox;

		// Token: 0x04002A9C RID: 10908
		private static ISleekBox beardBox;

		// Token: 0x04002A9D RID: 10909
		private static ISleekButton[] faceButtons;

		// Token: 0x04002A9E RID: 10910
		private static ISleekButton[] hairButtons;

		// Token: 0x04002A9F RID: 10911
		private static ISleekButton[] beardButtons;

		// Token: 0x04002AA0 RID: 10912
		private static ISleekBox skinBox;

		// Token: 0x04002AA1 RID: 10913
		private static ISleekBox colorBox;

		// Token: 0x04002AA2 RID: 10914
		private static ISleekButton[] skinButtons;

		// Token: 0x04002AA3 RID: 10915
		private static ISleekButton[] colorButtons;

		// Token: 0x04002AA4 RID: 10916
		private static SleekColorPicker skinColorPicker;

		// Token: 0x04002AA5 RID: 10917
		private static SleekColorPicker colorColorPicker;

		// Token: 0x04002AA6 RID: 10918
		private static SleekButtonState handState;

		// Token: 0x04002AA7 RID: 10919
		private static ISleekSlider characterSlider;
	}
}
