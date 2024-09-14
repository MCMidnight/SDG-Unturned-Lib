using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007B9 RID: 1977
	public class MenuWorkshopUI
	{
		// Token: 0x06004278 RID: 17016 RVA: 0x0016DA3B File Offset: 0x0016BC3B
		public static void open()
		{
			if (MenuWorkshopUI.active)
			{
				return;
			}
			MenuWorkshopUI.active = true;
			MenuWorkshopUI.container.AnimateIntoView();
		}

		// Token: 0x06004279 RID: 17017 RVA: 0x0016DA55 File Offset: 0x0016BC55
		public static void close()
		{
			if (!MenuWorkshopUI.active)
			{
				return;
			}
			MenuWorkshopUI.active = false;
			MenuWorkshopUI.container.AnimateOutOfView(0f, -1f);
		}

		// Token: 0x0600427A RID: 17018 RVA: 0x0016DA79 File Offset: 0x0016BC79
		private static void onClickedBrowseButton(ISleekElement button)
		{
			if (!Provider.provider.browserService.canOpenBrowser)
			{
				MenuUI.alert(MenuWorkshopUI.localization.format("Overlay"));
				return;
			}
			Provider.provider.browserService.open("http://steamcommunity.com/app/304930/workshop/");
		}

		// Token: 0x0600427B RID: 17019 RVA: 0x0016DAB5 File Offset: 0x0016BCB5
		private static void onClickedSubmitButton(ISleekElement button)
		{
			MenuWorkshopSubmitUI.open();
			MenuWorkshopUI.close();
		}

		// Token: 0x0600427C RID: 17020 RVA: 0x0016DAC1 File Offset: 0x0016BCC1
		private static void onClickedEditorButton(ISleekElement button)
		{
			MenuWorkshopEditorUI.open();
			MenuWorkshopUI.close();
		}

		// Token: 0x0600427D RID: 17021 RVA: 0x0016DACD File Offset: 0x0016BCCD
		private static void onClickedErrorButton(ISleekElement button)
		{
			MenuWorkshopErrorUI.open();
			MenuWorkshopUI.close();
		}

		// Token: 0x0600427E RID: 17022 RVA: 0x0016DAD9 File Offset: 0x0016BCD9
		private static void onClickedLocalizationButton(ISleekElement button)
		{
			MenuWorkshopLocalizationUI.open();
			MenuWorkshopUI.close();
		}

		// Token: 0x0600427F RID: 17023 RVA: 0x0016DAE5 File Offset: 0x0016BCE5
		private static void onClickedSpawnsButton(ISleekElement button)
		{
			MenuWorkshopSpawnsUI.open();
			MenuWorkshopUI.close();
		}

		// Token: 0x06004280 RID: 17024 RVA: 0x0016DAF1 File Offset: 0x0016BCF1
		private static void onClickedSubscriptionsButton(ISleekElement button)
		{
			MenuWorkshopSubscriptionsUI.instance.open();
			MenuWorkshopUI.close();
		}

		// Token: 0x06004281 RID: 17025 RVA: 0x0016DB02 File Offset: 0x0016BD02
		private static void onClickedDocsButton(ISleekElement button)
		{
			Provider.provider.browserService.open("https://docs.smartlydressedgames.com");
		}

		// Token: 0x06004282 RID: 17026 RVA: 0x0016DB18 File Offset: 0x0016BD18
		private static void onClickedBackButton(ISleekElement button)
		{
			MenuDashboardUI.open();
			MenuTitleUI.open();
			MenuWorkshopUI.close();
		}

		// Token: 0x06004283 RID: 17027 RVA: 0x0016DB29 File Offset: 0x0016BD29
		private static void onClickedCaptureItemIconButton(ISleekElement button)
		{
			IconUtils.CreateExtrasDirectory();
			IconUtils.captureItemIcon(Assets.find(EAssetType.ITEM, MenuWorkshopUI.itemIDField.Value) as ItemAsset);
		}

		// Token: 0x06004284 RID: 17028 RVA: 0x0016DB4A File Offset: 0x0016BD4A
		private static void onClickedCaptureAllItemIconsButton(ISleekElement button)
		{
			IconUtils.CreateExtrasDirectory();
			IconUtils.captureAllItemIcons();
		}

		// Token: 0x06004285 RID: 17029 RVA: 0x0016DB56 File Offset: 0x0016BD56
		private static void onClickedCaptureAllSkinIconsButton(ISleekElement button)
		{
			IconUtils.CreateExtrasDirectory();
			IconUtils.CaptureAllSkinIcons();
		}

		// Token: 0x06004286 RID: 17030 RVA: 0x0016DB64 File Offset: 0x0016BD64
		private static void onClickedCaptureItemDefIconButton(ISleekElement button)
		{
			IconUtils.CreateExtrasDirectory();
			Guid guid;
			if (Guid.TryParse(MenuWorkshopUI.guidField.Text, ref guid))
			{
				Asset asset = Assets.find(guid);
				ItemAsset itemAsset = asset as ItemAsset;
				VehicleAsset vehicleAsset = asset as VehicleAsset;
				if (itemAsset != null || vehicleAsset != null)
				{
					IconUtils.getItemDefIcon(itemAsset, vehicleAsset, MenuWorkshopUI.skinIDField.Value);
					return;
				}
			}
			IconUtils.getItemDefIcon(MenuWorkshopUI.itemIDField.Value, MenuWorkshopUI.vehicleIDField.Value, MenuWorkshopUI.skinIDField.Value);
		}

		// Token: 0x06004287 RID: 17031 RVA: 0x0016DBD9 File Offset: 0x0016BDD9
		private static void OnCaptureOutfitPreviewClicked(ISleekElement button)
		{
			IconUtils.CreateExtrasDirectory();
			IconUtils.CaptureOutfitPreview(new Guid(MenuWorkshopUI.guidField.Text));
		}

		// Token: 0x06004288 RID: 17032 RVA: 0x0016DBF4 File Offset: 0x0016BDF4
		private static void OnCaptureCosmeticPreviewsClicked(ISleekElement button)
		{
			IconUtils.CreateExtrasDirectory();
			IconUtils.CaptureCosmeticPreviews();
		}

		// Token: 0x06004289 RID: 17033 RVA: 0x0016DC00 File Offset: 0x0016BE00
		private static void OnCaptureAllOutfitPreviewsClicked(ISleekElement button)
		{
			IconUtils.CreateExtrasDirectory();
			IconUtils.CaptureAllOutfitPreviews();
		}

		// Token: 0x0600428A RID: 17034 RVA: 0x0016DC0C File Offset: 0x0016BE0C
		public static void toggleIconTools()
		{
			MenuWorkshopUI.iconToolsContainer.IsVisible = !MenuWorkshopUI.iconToolsContainer.IsVisible;
		}

		// Token: 0x0600428B RID: 17035 RVA: 0x0016DC25 File Offset: 0x0016BE25
		public void OnDestroy()
		{
			this.editorUI.OnDestroy();
			this.submitUI.OnDestroy();
		}

		// Token: 0x0600428C RID: 17036 RVA: 0x0016DC40 File Offset: 0x0016BE40
		public MenuWorkshopUI()
		{
			MenuWorkshopUI.localization = Localization.read("/Menu/Workshop/MenuWorkshop.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Workshop/MenuWorkshop/MenuWorkshop.unity3d");
			MenuWorkshopUI.container = new SleekFullscreenBox();
			MenuWorkshopUI.container.PositionOffset_X = 10f;
			MenuWorkshopUI.container.PositionOffset_Y = 10f;
			MenuWorkshopUI.container.PositionScale_Y = -1f;
			MenuWorkshopUI.container.SizeOffset_X = -20f;
			MenuWorkshopUI.container.SizeOffset_Y = -20f;
			MenuWorkshopUI.container.SizeScale_X = 1f;
			MenuWorkshopUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuWorkshopUI.container);
			MenuWorkshopUI.active = false;
			MenuWorkshopUI.browseButton = new SleekButtonIcon(bundle.load<Texture2D>("Browse"));
			MenuWorkshopUI.browseButton.PositionOffset_X = -205f;
			MenuWorkshopUI.browseButton.PositionOffset_Y = -115f;
			MenuWorkshopUI.browseButton.PositionScale_X = 0.5f;
			MenuWorkshopUI.browseButton.PositionScale_Y = 0.5f;
			MenuWorkshopUI.browseButton.SizeOffset_X = 200f;
			MenuWorkshopUI.browseButton.SizeOffset_Y = 50f;
			MenuWorkshopUI.browseButton.text = MenuWorkshopUI.localization.format("BrowseButtonText");
			MenuWorkshopUI.browseButton.tooltip = MenuWorkshopUI.localization.format("BrowseButtonTooltip");
			MenuWorkshopUI.browseButton.onClickedButton += new ClickedButton(MenuWorkshopUI.onClickedBrowseButton);
			MenuWorkshopUI.browseButton.fontSize = 3;
			MenuWorkshopUI.browseButton.iconColor = 2;
			MenuWorkshopUI.container.AddChild(MenuWorkshopUI.browseButton);
			MenuWorkshopUI.submitButton = new SleekButtonIcon(bundle.load<Texture2D>("Submit"));
			MenuWorkshopUI.submitButton.PositionOffset_X = -205f;
			MenuWorkshopUI.submitButton.PositionOffset_Y = -55f;
			MenuWorkshopUI.submitButton.PositionScale_X = 0.5f;
			MenuWorkshopUI.submitButton.PositionScale_Y = 0.5f;
			MenuWorkshopUI.submitButton.SizeOffset_X = 200f;
			MenuWorkshopUI.submitButton.SizeOffset_Y = 50f;
			MenuWorkshopUI.submitButton.text = MenuWorkshopUI.localization.format("SubmitButtonText");
			MenuWorkshopUI.submitButton.tooltip = MenuWorkshopUI.localization.format("SubmitButtonTooltip");
			MenuWorkshopUI.submitButton.onClickedButton += new ClickedButton(MenuWorkshopUI.onClickedSubmitButton);
			MenuWorkshopUI.submitButton.fontSize = 3;
			MenuWorkshopUI.submitButton.iconColor = 2;
			MenuWorkshopUI.container.AddChild(MenuWorkshopUI.submitButton);
			MenuWorkshopUI.editorButton = new SleekButtonIcon(bundle.load<Texture2D>("Editor"));
			MenuWorkshopUI.editorButton.PositionOffset_X = 5f;
			MenuWorkshopUI.editorButton.PositionOffset_Y = -55f;
			MenuWorkshopUI.editorButton.PositionScale_X = 0.5f;
			MenuWorkshopUI.editorButton.PositionScale_Y = 0.5f;
			MenuWorkshopUI.editorButton.SizeOffset_X = 200f;
			MenuWorkshopUI.editorButton.SizeOffset_Y = 50f;
			MenuWorkshopUI.editorButton.text = MenuWorkshopUI.localization.format("EditorButtonText");
			MenuWorkshopUI.editorButton.tooltip = MenuWorkshopUI.localization.format("EditorButtonTooltip");
			MenuWorkshopUI.editorButton.onClickedButton += new ClickedButton(MenuWorkshopUI.onClickedEditorButton);
			MenuWorkshopUI.editorButton.fontSize = 3;
			MenuWorkshopUI.editorButton.iconColor = 2;
			MenuWorkshopUI.container.AddChild(MenuWorkshopUI.editorButton);
			MenuWorkshopUI.errorButton = new SleekButtonIcon(bundle.load<Texture2D>("Error"));
			MenuWorkshopUI.errorButton.PositionOffset_X = -205f;
			MenuWorkshopUI.errorButton.PositionOffset_Y = 5f;
			MenuWorkshopUI.errorButton.PositionScale_X = 0.5f;
			MenuWorkshopUI.errorButton.PositionScale_Y = 0.5f;
			MenuWorkshopUI.errorButton.SizeOffset_X = 200f;
			MenuWorkshopUI.errorButton.SizeOffset_Y = 50f;
			MenuWorkshopUI.errorButton.text = MenuWorkshopUI.localization.format("ErrorButtonText");
			MenuWorkshopUI.errorButton.tooltip = MenuWorkshopUI.localization.format("ErrorButtonTooltip");
			MenuWorkshopUI.errorButton.onClickedButton += new ClickedButton(MenuWorkshopUI.onClickedErrorButton);
			MenuWorkshopUI.errorButton.fontSize = 3;
			MenuWorkshopUI.errorButton.iconColor = 2;
			MenuWorkshopUI.container.AddChild(MenuWorkshopUI.errorButton);
			MenuWorkshopUI.localizationButton = new SleekButtonIcon(bundle.load<Texture2D>("Localization"));
			MenuWorkshopUI.localizationButton.PositionOffset_X = 5f;
			MenuWorkshopUI.localizationButton.PositionOffset_Y = 65f;
			MenuWorkshopUI.localizationButton.PositionScale_X = 0.5f;
			MenuWorkshopUI.localizationButton.PositionScale_Y = 0.5f;
			MenuWorkshopUI.localizationButton.SizeOffset_X = 200f;
			MenuWorkshopUI.localizationButton.SizeOffset_Y = 50f;
			MenuWorkshopUI.localizationButton.text = MenuWorkshopUI.localization.format("LocalizationButtonText");
			MenuWorkshopUI.localizationButton.tooltip = MenuWorkshopUI.localization.format("LocalizationButtonTooltip");
			MenuWorkshopUI.localizationButton.onClickedButton += new ClickedButton(MenuWorkshopUI.onClickedLocalizationButton);
			MenuWorkshopUI.localizationButton.fontSize = 3;
			MenuWorkshopUI.localizationButton.iconColor = 2;
			MenuWorkshopUI.container.AddChild(MenuWorkshopUI.localizationButton);
			MenuWorkshopUI.spawnsButton = new SleekButtonIcon(bundle.load<Texture2D>("Spawns"));
			MenuWorkshopUI.spawnsButton.PositionOffset_X = -205f;
			MenuWorkshopUI.spawnsButton.PositionOffset_Y = 65f;
			MenuWorkshopUI.spawnsButton.PositionScale_X = 0.5f;
			MenuWorkshopUI.spawnsButton.PositionScale_Y = 0.5f;
			MenuWorkshopUI.spawnsButton.SizeOffset_X = 200f;
			MenuWorkshopUI.spawnsButton.SizeOffset_Y = 50f;
			MenuWorkshopUI.spawnsButton.text = MenuWorkshopUI.localization.format("SpawnsButtonText");
			MenuWorkshopUI.spawnsButton.tooltip = MenuWorkshopUI.localization.format("SpawnsButtonTooltip");
			MenuWorkshopUI.spawnsButton.onClickedButton += new ClickedButton(MenuWorkshopUI.onClickedSpawnsButton);
			MenuWorkshopUI.spawnsButton.fontSize = 3;
			MenuWorkshopUI.spawnsButton.iconColor = 2;
			MenuWorkshopUI.container.AddChild(MenuWorkshopUI.spawnsButton);
			MenuWorkshopUI.subscriptionsButton = new SleekButtonIcon(bundle.load<Texture2D>("Subscriptions"));
			MenuWorkshopUI.subscriptionsButton.PositionOffset_X = 5f;
			MenuWorkshopUI.subscriptionsButton.PositionOffset_Y = -115f;
			MenuWorkshopUI.subscriptionsButton.PositionScale_X = 0.5f;
			MenuWorkshopUI.subscriptionsButton.PositionScale_Y = 0.5f;
			MenuWorkshopUI.subscriptionsButton.SizeOffset_X = 200f;
			MenuWorkshopUI.subscriptionsButton.SizeOffset_Y = 50f;
			MenuWorkshopUI.subscriptionsButton.text = MenuWorkshopUI.localization.format("SubscriptionsButtonText");
			MenuWorkshopUI.subscriptionsButton.tooltip = MenuWorkshopUI.localization.format("SubscriptionsButtonTooltip");
			MenuWorkshopUI.subscriptionsButton.onClickedButton += new ClickedButton(MenuWorkshopUI.onClickedSubscriptionsButton);
			MenuWorkshopUI.subscriptionsButton.fontSize = 3;
			MenuWorkshopUI.subscriptionsButton.iconColor = 2;
			MenuWorkshopUI.container.AddChild(MenuWorkshopUI.subscriptionsButton);
			MenuWorkshopUI.docsButton = new SleekButtonIcon(bundle.load<Texture2D>("Docs"));
			MenuWorkshopUI.docsButton.PositionOffset_X = 5f;
			MenuWorkshopUI.docsButton.PositionOffset_Y = 5f;
			MenuWorkshopUI.docsButton.PositionScale_X = 0.5f;
			MenuWorkshopUI.docsButton.PositionScale_Y = 0.5f;
			MenuWorkshopUI.docsButton.SizeOffset_X = 200f;
			MenuWorkshopUI.docsButton.SizeOffset_Y = 50f;
			MenuWorkshopUI.docsButton.text = MenuWorkshopUI.localization.format("DocsButtonText");
			MenuWorkshopUI.docsButton.tooltip = MenuWorkshopUI.localization.format("DocsButtonTooltip");
			MenuWorkshopUI.docsButton.onClickedButton += new ClickedButton(MenuWorkshopUI.onClickedDocsButton);
			MenuWorkshopUI.docsButton.fontSize = 3;
			MenuWorkshopUI.docsButton.iconColor = 2;
			MenuWorkshopUI.container.AddChild(MenuWorkshopUI.docsButton);
			MenuWorkshopUI.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuWorkshopUI.backButton.PositionOffset_X = -100f;
			MenuWorkshopUI.backButton.PositionOffset_Y = 125f;
			MenuWorkshopUI.backButton.PositionScale_X = 0.5f;
			MenuWorkshopUI.backButton.PositionScale_Y = 0.5f;
			MenuWorkshopUI.backButton.SizeOffset_X = 200f;
			MenuWorkshopUI.backButton.SizeOffset_Y = 50f;
			MenuWorkshopUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuWorkshopUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			MenuWorkshopUI.backButton.onClickedButton += new ClickedButton(MenuWorkshopUI.onClickedBackButton);
			MenuWorkshopUI.backButton.fontSize = 3;
			MenuWorkshopUI.backButton.iconColor = 2;
			MenuWorkshopUI.container.AddChild(MenuWorkshopUI.backButton);
			bundle.unload();
			MenuWorkshopUI.iconToolsContainer = Glazier.Get().CreateFrame();
			MenuWorkshopUI.iconToolsContainer.PositionOffset_X = 40f;
			MenuWorkshopUI.iconToolsContainer.PositionOffset_Y = 40f;
			MenuWorkshopUI.iconToolsContainer.SizeOffset_X = -80f;
			MenuWorkshopUI.iconToolsContainer.SizeOffset_Y = -80f;
			MenuWorkshopUI.iconToolsContainer.SizeScale_X = 1f;
			MenuWorkshopUI.iconToolsContainer.SizeScale_Y = 1f;
			MenuWorkshopUI.container.AddChild(MenuWorkshopUI.iconToolsContainer);
			MenuWorkshopUI.iconToolsContainer.IsVisible = false;
			int num = 0;
			MenuWorkshopUI.itemIDField = Glazier.Get().CreateUInt16Field();
			MenuWorkshopUI.itemIDField.PositionOffset_Y = (float)num;
			MenuWorkshopUI.itemIDField.SizeOffset_X = 150f;
			MenuWorkshopUI.itemIDField.SizeOffset_Y = 25f;
			MenuWorkshopUI.itemIDField.AddLabel("Item ID", 1);
			MenuWorkshopUI.iconToolsContainer.AddChild(MenuWorkshopUI.itemIDField);
			num += 25;
			MenuWorkshopUI.vehicleIDField = Glazier.Get().CreateUInt16Field();
			MenuWorkshopUI.vehicleIDField.PositionOffset_Y = (float)num;
			MenuWorkshopUI.vehicleIDField.SizeOffset_X = 150f;
			MenuWorkshopUI.vehicleIDField.SizeOffset_Y = 25f;
			MenuWorkshopUI.vehicleIDField.AddLabel("Vehicle ID", 1);
			MenuWorkshopUI.iconToolsContainer.AddChild(MenuWorkshopUI.vehicleIDField);
			num += 25;
			MenuWorkshopUI.skinIDField = Glazier.Get().CreateUInt16Field();
			MenuWorkshopUI.skinIDField.PositionOffset_Y = (float)num;
			MenuWorkshopUI.skinIDField.SizeOffset_X = 150f;
			MenuWorkshopUI.skinIDField.SizeOffset_Y = 25f;
			MenuWorkshopUI.skinIDField.AddLabel("Skin ID", 1);
			MenuWorkshopUI.iconToolsContainer.AddChild(MenuWorkshopUI.skinIDField);
			num += 25;
			MenuWorkshopUI.captureItemIconButton = Glazier.Get().CreateButton();
			MenuWorkshopUI.captureItemIconButton.PositionOffset_Y = (float)num;
			MenuWorkshopUI.captureItemIconButton.SizeOffset_X = 150f;
			MenuWorkshopUI.captureItemIconButton.SizeOffset_Y = 25f;
			MenuWorkshopUI.captureItemIconButton.Text = "Item Icon";
			MenuWorkshopUI.captureItemIconButton.OnClicked += new ClickedButton(MenuWorkshopUI.onClickedCaptureItemIconButton);
			MenuWorkshopUI.iconToolsContainer.AddChild(MenuWorkshopUI.captureItemIconButton);
			num += 25;
			MenuWorkshopUI.captureAllItemIconsButton = Glazier.Get().CreateButton();
			MenuWorkshopUI.captureAllItemIconsButton.PositionOffset_Y = (float)num;
			MenuWorkshopUI.captureAllItemIconsButton.SizeOffset_X = 150f;
			MenuWorkshopUI.captureAllItemIconsButton.SizeOffset_Y = 25f;
			MenuWorkshopUI.captureAllItemIconsButton.Text = "All Item Icons";
			MenuWorkshopUI.captureAllItemIconsButton.OnClicked += new ClickedButton(MenuWorkshopUI.onClickedCaptureAllItemIconsButton);
			MenuWorkshopUI.iconToolsContainer.AddChild(MenuWorkshopUI.captureAllItemIconsButton);
			num += 25;
			MenuWorkshopUI.captureAllSkinIconsButton = Glazier.Get().CreateButton();
			MenuWorkshopUI.captureAllSkinIconsButton.PositionOffset_Y = (float)num;
			MenuWorkshopUI.captureAllSkinIconsButton.SizeOffset_X = 150f;
			MenuWorkshopUI.captureAllSkinIconsButton.SizeOffset_Y = 25f;
			MenuWorkshopUI.captureAllSkinIconsButton.Text = "All Skin Icons";
			MenuWorkshopUI.captureAllSkinIconsButton.OnClicked += new ClickedButton(MenuWorkshopUI.onClickedCaptureAllSkinIconsButton);
			MenuWorkshopUI.iconToolsContainer.AddChild(MenuWorkshopUI.captureAllSkinIconsButton);
			num += 25;
			MenuWorkshopUI.captureItemDefIconButton = Glazier.Get().CreateButton();
			MenuWorkshopUI.captureItemDefIconButton.PositionOffset_Y = (float)num;
			MenuWorkshopUI.captureItemDefIconButton.SizeOffset_X = 150f;
			MenuWorkshopUI.captureItemDefIconButton.SizeOffset_Y = 25f;
			MenuWorkshopUI.captureItemDefIconButton.Text = "Econ Icon";
			MenuWorkshopUI.captureItemDefIconButton.OnClicked += new ClickedButton(MenuWorkshopUI.onClickedCaptureItemDefIconButton);
			MenuWorkshopUI.iconToolsContainer.AddChild(MenuWorkshopUI.captureItemDefIconButton);
			num += 25;
			MenuWorkshopUI.guidField = Glazier.Get().CreateStringField();
			MenuWorkshopUI.guidField.PositionOffset_Y = (float)num;
			MenuWorkshopUI.guidField.SizeOffset_X = 150f;
			MenuWorkshopUI.guidField.SizeOffset_Y = 25f;
			MenuWorkshopUI.guidField.AddLabel("GUID", 1);
			MenuWorkshopUI.iconToolsContainer.AddChild(MenuWorkshopUI.guidField);
			num += 25;
			MenuWorkshopUI.captureOutfitPreviewButton = Glazier.Get().CreateButton();
			MenuWorkshopUI.captureOutfitPreviewButton.PositionOffset_Y = (float)num;
			MenuWorkshopUI.captureOutfitPreviewButton.SizeOffset_X = 150f;
			MenuWorkshopUI.captureOutfitPreviewButton.SizeOffset_Y = 25f;
			MenuWorkshopUI.captureOutfitPreviewButton.Text = "Outfit Preview";
			MenuWorkshopUI.captureOutfitPreviewButton.OnClicked += new ClickedButton(MenuWorkshopUI.OnCaptureOutfitPreviewClicked);
			MenuWorkshopUI.iconToolsContainer.AddChild(MenuWorkshopUI.captureOutfitPreviewButton);
			num += 25;
			MenuWorkshopUI.captureCosmeticPreviewsButton = Glazier.Get().CreateButton();
			MenuWorkshopUI.captureCosmeticPreviewsButton.PositionOffset_Y = (float)num;
			MenuWorkshopUI.captureCosmeticPreviewsButton.SizeOffset_X = 150f;
			MenuWorkshopUI.captureCosmeticPreviewsButton.SizeOffset_Y = 25f;
			MenuWorkshopUI.captureCosmeticPreviewsButton.Text = "All Cosmetic Previews";
			MenuWorkshopUI.captureCosmeticPreviewsButton.OnClicked += new ClickedButton(MenuWorkshopUI.OnCaptureCosmeticPreviewsClicked);
			MenuWorkshopUI.iconToolsContainer.AddChild(MenuWorkshopUI.captureCosmeticPreviewsButton);
			num += 25;
			MenuWorkshopUI.captureAllOutfitPreviewsButton = Glazier.Get().CreateButton();
			MenuWorkshopUI.captureAllOutfitPreviewsButton.PositionOffset_Y = (float)num;
			MenuWorkshopUI.captureAllOutfitPreviewsButton.SizeOffset_X = 150f;
			MenuWorkshopUI.captureAllOutfitPreviewsButton.SizeOffset_Y = 25f;
			MenuWorkshopUI.captureAllOutfitPreviewsButton.Text = "All Outfit Previews";
			MenuWorkshopUI.captureAllOutfitPreviewsButton.OnClicked += new ClickedButton(MenuWorkshopUI.OnCaptureAllOutfitPreviewsClicked);
			MenuWorkshopUI.iconToolsContainer.AddChild(MenuWorkshopUI.captureAllOutfitPreviewsButton);
			num += 25;
			this.submitUI = new MenuWorkshopSubmitUI();
			this.editorUI = new MenuWorkshopEditorUI();
			this.errorUI = new MenuWorkshopErrorUI();
			this.localizationUI = new MenuWorkshopLocalizationUI();
			this.spawnsUI = new MenuWorkshopSpawnsUI();
			this.subscriptionsUI = new MenuWorkshopSubscriptionsUI();
		}

		// Token: 0x04002BAE RID: 11182
		private static SleekFullscreenBox container;

		// Token: 0x04002BAF RID: 11183
		private static Local localization;

		// Token: 0x04002BB0 RID: 11184
		public static bool active;

		// Token: 0x04002BB1 RID: 11185
		private static SleekButtonIcon browseButton;

		// Token: 0x04002BB2 RID: 11186
		private static SleekButtonIcon submitButton;

		// Token: 0x04002BB3 RID: 11187
		private static SleekButtonIcon editorButton;

		// Token: 0x04002BB4 RID: 11188
		private static SleekButtonIcon errorButton;

		// Token: 0x04002BB5 RID: 11189
		private static SleekButtonIcon localizationButton;

		// Token: 0x04002BB6 RID: 11190
		private static SleekButtonIcon spawnsButton;

		// Token: 0x04002BB7 RID: 11191
		private static SleekButtonIcon subscriptionsButton;

		// Token: 0x04002BB8 RID: 11192
		private static SleekButtonIcon docsButton;

		// Token: 0x04002BB9 RID: 11193
		private static SleekButtonIcon backButton;

		// Token: 0x04002BBA RID: 11194
		private static ISleekElement iconToolsContainer;

		// Token: 0x04002BBB RID: 11195
		private static ISleekUInt16Field itemIDField;

		// Token: 0x04002BBC RID: 11196
		private static ISleekUInt16Field vehicleIDField;

		// Token: 0x04002BBD RID: 11197
		private static ISleekUInt16Field skinIDField;

		// Token: 0x04002BBE RID: 11198
		private static ISleekField guidField;

		// Token: 0x04002BBF RID: 11199
		private static ISleekButton captureItemIconButton;

		// Token: 0x04002BC0 RID: 11200
		private static ISleekButton captureAllItemIconsButton;

		// Token: 0x04002BC1 RID: 11201
		private static ISleekButton captureAllSkinIconsButton;

		// Token: 0x04002BC2 RID: 11202
		private static ISleekButton captureItemDefIconButton;

		// Token: 0x04002BC3 RID: 11203
		private static ISleekButton captureOutfitPreviewButton;

		// Token: 0x04002BC4 RID: 11204
		private static ISleekButton captureCosmeticPreviewsButton;

		// Token: 0x04002BC5 RID: 11205
		private static ISleekButton captureAllOutfitPreviewsButton;

		// Token: 0x04002BC6 RID: 11206
		private MenuWorkshopSubmitUI submitUI;

		// Token: 0x04002BC7 RID: 11207
		private MenuWorkshopEditorUI editorUI;

		// Token: 0x04002BC8 RID: 11208
		private MenuWorkshopErrorUI errorUI;

		// Token: 0x04002BC9 RID: 11209
		private MenuWorkshopLocalizationUI localizationUI;

		// Token: 0x04002BCA RID: 11210
		private MenuWorkshopSpawnsUI spawnsUI;

		// Token: 0x04002BCB RID: 11211
		private MenuWorkshopSubscriptionsUI subscriptionsUI;
	}
}
