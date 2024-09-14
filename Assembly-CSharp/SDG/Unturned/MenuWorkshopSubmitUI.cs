using System;
using System.Collections.Generic;
using System.IO;
using SDG.Provider;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007B7 RID: 1975
	public class MenuWorkshopSubmitUI
	{
		// Token: 0x06004259 RID: 16985 RVA: 0x0016B622 File Offset: 0x00169822
		public static void open()
		{
			if (MenuWorkshopSubmitUI.active)
			{
				return;
			}
			MenuWorkshopSubmitUI.active = true;
			MenuWorkshopSubmitUI.container.AnimateIntoView();
		}

		// Token: 0x0600425A RID: 16986 RVA: 0x0016B63C File Offset: 0x0016983C
		public static void close()
		{
			if (!MenuWorkshopSubmitUI.active)
			{
				return;
			}
			MenuWorkshopSubmitUI.active = false;
			MenuWorkshopSubmitUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x0600425B RID: 16987 RVA: 0x0016B660 File Offset: 0x00169860
		private static string tag
		{
			get
			{
				switch (MenuWorkshopSubmitUI.typeState.state)
				{
				case 0:
					return MenuWorkshopSubmitUI.mapTypeState.states[MenuWorkshopSubmitUI.mapTypeState.state].text;
				case 2:
					return MenuWorkshopSubmitUI.objectTypeState.states[MenuWorkshopSubmitUI.objectTypeState.state].text;
				case 3:
					return MenuWorkshopSubmitUI.itemTypeState.states[MenuWorkshopSubmitUI.itemTypeState.state].text;
				case 4:
					return MenuWorkshopSubmitUI.vehicleTypeState.states[MenuWorkshopSubmitUI.vehicleTypeState.state].text;
				case 5:
					return MenuWorkshopSubmitUI.skinTypeState.states[MenuWorkshopSubmitUI.skinTypeState.state].text;
				}
				return "";
			}
		}

		// Token: 0x0600425C RID: 16988 RVA: 0x0016B728 File Offset: 0x00169928
		private static void refreshPathFieldNotification()
		{
			string text = MenuWorkshopSubmitUI.pathField.Text;
			string text2 = null;
			if (string.IsNullOrEmpty(text))
			{
				text2 = MenuWorkshopSubmitUI.localization.format("PathFieldNotification_Empty");
			}
			else if (!ReadWrite.folderExists(text, false))
			{
				text2 = MenuWorkshopSubmitUI.localization.format("PathFieldNotification_MissingFolder");
			}
			else if (!ReadWrite.hasDirectoryWritePermission(text))
			{
				text2 = MenuWorkshopSubmitUI.localization.format("PathFieldNotification_NoWritePermission");
			}
			else
			{
				ESteamUGCType state = (ESteamUGCType)MenuWorkshopSubmitUI.typeState.state;
				if (state == ESteamUGCType.MAP)
				{
					if (!WorkshopTool.checkMapValid(text, false))
					{
						text2 = MenuWorkshopSubmitUI.localization.format("PathFieldNotification_Map");
					}
				}
				else if (state == ESteamUGCType.LOCALIZATION)
				{
					if (!WorkshopTool.checkLocalizationValid(text, false))
					{
						text2 = MenuWorkshopSubmitUI.localization.format("PathFieldNotification_Localization");
					}
				}
				else if (!WorkshopTool.checkBundleValid(text, false))
				{
					text2 = MenuWorkshopSubmitUI.localization.format("PathFieldNotification_Bundle");
				}
			}
			MenuWorkshopSubmitUI.pathNotification.IsVisible = !string.IsNullOrEmpty(text2);
			MenuWorkshopSubmitUI.pathNotification.TooltipText = text2;
		}

		// Token: 0x0600425D RID: 16989 RVA: 0x0016B813 File Offset: 0x00169A13
		private static void onPathFieldTyped(ISleekField field, string text)
		{
			MenuWorkshopSubmitUI.refreshPathFieldNotification();
		}

		// Token: 0x0600425E RID: 16990 RVA: 0x0016B81C File Offset: 0x00169A1C
		private static void refreshPreviewFieldNotification()
		{
			string text = MenuWorkshopSubmitUI.previewField.Text;
			string text2 = null;
			if (string.IsNullOrEmpty(text))
			{
				text2 = MenuWorkshopSubmitUI.localization.format("PreviewFieldNotification_Empty");
			}
			else if (!ReadWrite.fileExists(text, false, false))
			{
				text2 = MenuWorkshopSubmitUI.localization.format("PreviewFieldNotification_MissingFile");
			}
			else if (text.EndsWith(".png", 3) || text.EndsWith(".jpg", 3))
			{
				if (new FileInfo(text).Length > 1000000L)
				{
					text2 = MenuWorkshopSubmitUI.localization.format("PreviewFieldNotification_FileSize");
				}
			}
			else
			{
				text2 = MenuWorkshopSubmitUI.localization.format("PreviewFieldNotification_Extension");
			}
			MenuWorkshopSubmitUI.previewNotification.IsVisible = !string.IsNullOrEmpty(text2);
			MenuWorkshopSubmitUI.previewNotification.TooltipText = text2;
		}

		// Token: 0x0600425F RID: 16991 RVA: 0x0016B8DB File Offset: 0x00169ADB
		private static void onPreviewFieldTyped(ISleekField field, string text)
		{
			MenuWorkshopSubmitUI.refreshPreviewFieldNotification();
		}

		// Token: 0x06004260 RID: 16992 RVA: 0x0016B8E4 File Offset: 0x00169AE4
		private static void onClickedCreateButton(ISleekElement button)
		{
			if (MenuWorkshopSubmitUI.checkEntered() && MenuWorkshopSubmitUI.checkValid())
			{
				Provider.provider.workshopService.prepareUGC(MenuWorkshopSubmitUI.nameField.Text, MenuWorkshopSubmitUI.descriptionField.Text, MenuWorkshopSubmitUI.pathField.Text, MenuWorkshopSubmitUI.previewField.Text, MenuWorkshopSubmitUI.changeField.Text, (ESteamUGCType)MenuWorkshopSubmitUI.typeState.state, MenuWorkshopSubmitUI.tag, MenuWorkshopSubmitUI.allowedIPsField.Text, (ESteamUGCVisibility)MenuWorkshopSubmitUI.visibilityState.state);
				Provider.provider.workshopService.createUGC(MenuWorkshopSubmitUI.forState.state == 1);
				MenuWorkshopSubmitUI.resetFields();
			}
		}

		// Token: 0x06004261 RID: 16993 RVA: 0x0016B98A File Offset: 0x00169B8A
		private static void onClickedLegalButton(ISleekElement button)
		{
			if (!Provider.provider.browserService.canOpenBrowser)
			{
				MenuUI.alert(MenuWorkshopSubmitUI.localization.format("Overlay"));
				return;
			}
			Provider.provider.browserService.open("http://steamcommunity.com/sharedfiles/workshoplegalagreement/?appid=304930");
		}

		// Token: 0x06004262 RID: 16994 RVA: 0x0016B9C8 File Offset: 0x00169BC8
		private static void onClickedPublished(ISleekElement button)
		{
			int num = Mathf.FloorToInt(button.PositionOffset_Y / 40f);
			if (MenuWorkshopSubmitUI.checkValid())
			{
				Provider.provider.workshopService.prepareUGC(MenuWorkshopSubmitUI.nameField.Text, MenuWorkshopSubmitUI.descriptionField.Text, MenuWorkshopSubmitUI.pathField.Text, MenuWorkshopSubmitUI.previewField.Text, MenuWorkshopSubmitUI.changeField.Text, (ESteamUGCType)MenuWorkshopSubmitUI.typeState.state, MenuWorkshopSubmitUI.tag, MenuWorkshopSubmitUI.allowedIPsField.Text, (ESteamUGCVisibility)MenuWorkshopSubmitUI.visibilityState.state);
				Provider.provider.workshopService.prepareUGC(Provider.provider.workshopService.published[num].id);
				Provider.provider.workshopService.updateUGC();
				MenuWorkshopSubmitUI.resetFields();
			}
		}

		// Token: 0x06004263 RID: 16995 RVA: 0x0016BA94 File Offset: 0x00169C94
		private static void onPublishedAdded()
		{
			for (int i = 0; i < Provider.provider.workshopService.published.Count; i++)
			{
				SteamPublished steamPublished = Provider.provider.workshopService.published[i];
				ISleekButton sleekButton = Glazier.Get().CreateButton();
				sleekButton.PositionOffset_Y = (float)(i * 40);
				sleekButton.SizeOffset_Y = 30f;
				sleekButton.SizeScale_X = 1f;
				sleekButton.Text = steamPublished.name;
				sleekButton.OnClicked += new ClickedButton(MenuWorkshopSubmitUI.onClickedPublished);
				MenuWorkshopSubmitUI.publishedBox.AddChild(sleekButton);
				MenuWorkshopSubmitUI.publishedButtons.Add(sleekButton);
				MenuWorkshopSubmitUI.publishedBox.ContentSizeOffset = new Vector2(0f, (float)(MenuWorkshopSubmitUI.publishedButtons.Count * 40 - 10));
			}
		}

		// Token: 0x06004264 RID: 16996 RVA: 0x0016BB61 File Offset: 0x00169D61
		private static void onPublishedRemoved()
		{
			MenuWorkshopSubmitUI.publishedBox.RemoveAllChildren();
			MenuWorkshopSubmitUI.publishedButtons.Clear();
		}

		// Token: 0x06004265 RID: 16997 RVA: 0x0016BB78 File Offset: 0x00169D78
		private static bool checkEntered()
		{
			if (MenuWorkshopSubmitUI.nameField.Text.Length == 0)
			{
				MenuUI.alert(MenuWorkshopSubmitUI.localization.format("Alert_Name"));
				return false;
			}
			if (MenuWorkshopSubmitUI.previewField.Text.Length == 0 || !ReadWrite.fileExists(MenuWorkshopSubmitUI.previewField.Text, false, false) || new FileInfo(MenuWorkshopSubmitUI.previewField.Text).Length > 1000000L)
			{
				MenuUI.alert(MenuWorkshopSubmitUI.localization.format("Alert_Preview"));
				return false;
			}
			return true;
		}

		// Token: 0x06004266 RID: 16998 RVA: 0x0016BC04 File Offset: 0x00169E04
		private static bool checkValid()
		{
			if (MenuWorkshopSubmitUI.pathField.Text.Length == 0 || !ReadWrite.folderExists(MenuWorkshopSubmitUI.pathField.Text, false))
			{
				MenuUI.alert(MenuWorkshopSubmitUI.localization.format("Alert_Path"));
				return false;
			}
			ESteamUGCType state = (ESteamUGCType)MenuWorkshopSubmitUI.typeState.state;
			if (MenuWorkshopSubmitUI.forState.state == 1)
			{
				if (state != ESteamUGCType.ITEM && state != ESteamUGCType.SKIN)
				{
					MenuUI.alert(MenuWorkshopSubmitUI.localization.format("Alert_Curated"));
					return false;
				}
			}
			else if (state == ESteamUGCType.SKIN)
			{
				MenuUI.alert(MenuWorkshopSubmitUI.localization.format("Alert_Curated"));
				return false;
			}
			bool flag = false;
			if (state == ESteamUGCType.MAP)
			{
				flag = WorkshopTool.checkMapValid(MenuWorkshopSubmitUI.pathField.Text, false);
				if (!flag)
				{
					MenuUI.alert(MenuWorkshopSubmitUI.localization.format("Alert_Map"));
				}
			}
			else if (state == ESteamUGCType.LOCALIZATION)
			{
				flag = WorkshopTool.checkLocalizationValid(MenuWorkshopSubmitUI.pathField.Text, false);
				if (!flag)
				{
					MenuUI.alert(MenuWorkshopSubmitUI.localization.format("Alert_Localization"));
				}
			}
			else if (state == ESteamUGCType.OBJECT || state == ESteamUGCType.ITEM || state == ESteamUGCType.VEHICLE || state == ESteamUGCType.SKIN)
			{
				flag = WorkshopTool.checkBundleValid(MenuWorkshopSubmitUI.pathField.Text, false);
				if (!flag)
				{
					MenuUI.alert(MenuWorkshopSubmitUI.localization.format("Alert_Object"));
				}
			}
			return flag;
		}

		// Token: 0x06004267 RID: 16999 RVA: 0x0016BD34 File Offset: 0x00169F34
		private static void resetFields()
		{
			MenuWorkshopSubmitUI.nameField.Text = "";
			MenuWorkshopSubmitUI.descriptionField.Text = "";
			MenuWorkshopSubmitUI.pathField.Text = "";
			MenuWorkshopSubmitUI.previewField.Text = "";
			MenuWorkshopSubmitUI.changeField.Text = "";
			MenuWorkshopSubmitUI.allowedIPsField.Text = "";
			MenuWorkshopSubmitUI.refreshPathFieldNotification();
			MenuWorkshopSubmitUI.refreshPreviewFieldNotification();
		}

		// Token: 0x06004268 RID: 17000 RVA: 0x0016BDA8 File Offset: 0x00169FA8
		private static void onSwappedTypeState(SleekButtonState button, int state)
		{
			MenuWorkshopSubmitUI.mapTypeState.IsVisible = (state == 0);
			MenuWorkshopSubmitUI.itemTypeState.IsVisible = (state == 3);
			MenuWorkshopSubmitUI.vehicleTypeState.IsVisible = (state == 4);
			MenuWorkshopSubmitUI.skinTypeState.IsVisible = (state == 5);
			MenuWorkshopSubmitUI.objectTypeState.IsVisible = (state == 2);
			MenuWorkshopSubmitUI.refreshPathFieldNotification();
		}

		// Token: 0x06004269 RID: 17001 RVA: 0x0016BE02 File Offset: 0x0016A002
		private static void onClickedBackButton(ISleekElement button)
		{
			MenuWorkshopUI.open();
			MenuWorkshopSubmitUI.close();
		}

		// Token: 0x0600426A RID: 17002 RVA: 0x0016BE10 File Offset: 0x0016A010
		public void OnDestroy()
		{
			TempSteamworksWorkshop workshopService = Provider.provider.workshopService;
			workshopService.onPublishedAdded = (TempSteamworksWorkshop.PublishedAdded)Delegate.Remove(workshopService.onPublishedAdded, new TempSteamworksWorkshop.PublishedAdded(MenuWorkshopSubmitUI.onPublishedAdded));
			TempSteamworksWorkshop workshopService2 = Provider.provider.workshopService;
			workshopService2.onPublishedRemoved = (TempSteamworksWorkshop.PublishedRemoved)Delegate.Remove(workshopService2.onPublishedRemoved, new TempSteamworksWorkshop.PublishedRemoved(MenuWorkshopSubmitUI.onPublishedRemoved));
		}

		// Token: 0x0600426B RID: 17003 RVA: 0x0016BE74 File Offset: 0x0016A074
		public MenuWorkshopSubmitUI()
		{
			MenuWorkshopSubmitUI.localization = Localization.read("/Menu/Workshop/MenuWorkshopSubmit.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Workshop/MenuWorkshopSubmit/MenuWorkshopSubmit.unity3d");
			MenuWorkshopSubmitUI.publishedButtons = new List<ISleekButton>();
			TempSteamworksWorkshop workshopService = Provider.provider.workshopService;
			workshopService.onPublishedAdded = (TempSteamworksWorkshop.PublishedAdded)Delegate.Combine(workshopService.onPublishedAdded, new TempSteamworksWorkshop.PublishedAdded(MenuWorkshopSubmitUI.onPublishedAdded));
			TempSteamworksWorkshop workshopService2 = Provider.provider.workshopService;
			workshopService2.onPublishedRemoved = (TempSteamworksWorkshop.PublishedRemoved)Delegate.Combine(workshopService2.onPublishedRemoved, new TempSteamworksWorkshop.PublishedRemoved(MenuWorkshopSubmitUI.onPublishedRemoved));
			MenuWorkshopSubmitUI.container = new SleekFullscreenBox();
			MenuWorkshopSubmitUI.container.PositionOffset_X = 10f;
			MenuWorkshopSubmitUI.container.PositionOffset_Y = 10f;
			MenuWorkshopSubmitUI.container.PositionScale_Y = 1f;
			MenuWorkshopSubmitUI.container.SizeOffset_X = -20f;
			MenuWorkshopSubmitUI.container.SizeOffset_Y = -20f;
			MenuWorkshopSubmitUI.container.SizeScale_X = 1f;
			MenuWorkshopSubmitUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuWorkshopSubmitUI.container);
			MenuWorkshopSubmitUI.active = false;
			MenuWorkshopSubmitUI.nameField = Glazier.Get().CreateStringField();
			MenuWorkshopSubmitUI.nameField.PositionOffset_X = -200f;
			MenuWorkshopSubmitUI.nameField.PositionOffset_Y = 140f;
			MenuWorkshopSubmitUI.nameField.PositionScale_X = 0.5f;
			MenuWorkshopSubmitUI.nameField.SizeOffset_X = 200f;
			MenuWorkshopSubmitUI.nameField.SizeOffset_Y = 30f;
			MenuWorkshopSubmitUI.nameField.MaxLength = 24;
			MenuWorkshopSubmitUI.nameField.AddLabel(MenuWorkshopSubmitUI.localization.format("Name_Field_Label"), 1);
			MenuWorkshopSubmitUI.container.AddChild(MenuWorkshopSubmitUI.nameField);
			MenuWorkshopSubmitUI.descriptionField = Glazier.Get().CreateStringField();
			MenuWorkshopSubmitUI.descriptionField.PositionOffset_X = -200f;
			MenuWorkshopSubmitUI.descriptionField.PositionOffset_Y = 140f;
			MenuWorkshopSubmitUI.descriptionField.PositionScale_X = 0.5f;
			MenuWorkshopSubmitUI.descriptionField.SizeOffset_X = 400f;
			MenuWorkshopSubmitUI.descriptionField.SizeOffset_Y = 30f;
			MenuWorkshopSubmitUI.descriptionField.MaxLength = 128;
			MenuWorkshopSubmitUI.descriptionField.Text = "";
			MenuWorkshopSubmitUI.descriptionField.AddLabel(MenuWorkshopSubmitUI.localization.format("Description_Field_Label"), 1);
			MenuWorkshopSubmitUI.container.AddChild(MenuWorkshopSubmitUI.descriptionField);
			MenuWorkshopSubmitUI.descriptionField.IsVisible = false;
			MenuWorkshopSubmitUI.pathField = Glazier.Get().CreateStringField();
			MenuWorkshopSubmitUI.pathField.PositionOffset_X = -200f;
			MenuWorkshopSubmitUI.pathField.PositionOffset_Y = 180f;
			MenuWorkshopSubmitUI.pathField.PositionScale_X = 0.5f;
			MenuWorkshopSubmitUI.pathField.SizeOffset_X = 400f;
			MenuWorkshopSubmitUI.pathField.SizeOffset_Y = 30f;
			MenuWorkshopSubmitUI.pathField.MaxLength = 128;
			MenuWorkshopSubmitUI.pathField.AddLabel(MenuWorkshopSubmitUI.localization.format("Path_Field_Label"), 1);
			MenuWorkshopSubmitUI.pathField.OnTextChanged += new Typed(MenuWorkshopSubmitUI.onPathFieldTyped);
			MenuWorkshopSubmitUI.container.AddChild(MenuWorkshopSubmitUI.pathField);
			MenuWorkshopSubmitUI.pathNotification = Glazier.Get().CreateBox();
			MenuWorkshopSubmitUI.pathNotification.PositionOffset_X = -240f;
			MenuWorkshopSubmitUI.pathNotification.PositionOffset_Y = 180f;
			MenuWorkshopSubmitUI.pathNotification.PositionScale_X = 0.5f;
			MenuWorkshopSubmitUI.pathNotification.SizeOffset_X = 30f;
			MenuWorkshopSubmitUI.pathNotification.SizeOffset_Y = 30f;
			MenuWorkshopSubmitUI.pathNotification.Text = "!";
			MenuWorkshopSubmitUI.container.AddChild(MenuWorkshopSubmitUI.pathNotification);
			MenuWorkshopSubmitUI.pathNotification.IsVisible = false;
			MenuWorkshopSubmitUI.previewField = Glazier.Get().CreateStringField();
			MenuWorkshopSubmitUI.previewField.PositionOffset_X = -200f;
			MenuWorkshopSubmitUI.previewField.PositionOffset_Y = 220f;
			MenuWorkshopSubmitUI.previewField.PositionScale_X = 0.5f;
			MenuWorkshopSubmitUI.previewField.SizeOffset_X = 400f;
			MenuWorkshopSubmitUI.previewField.SizeOffset_Y = 30f;
			MenuWorkshopSubmitUI.previewField.MaxLength = 128;
			MenuWorkshopSubmitUI.previewField.AddLabel(MenuWorkshopSubmitUI.localization.format("Preview_Field_Label"), 1);
			MenuWorkshopSubmitUI.previewField.OnTextChanged += new Typed(MenuWorkshopSubmitUI.onPreviewFieldTyped);
			MenuWorkshopSubmitUI.container.AddChild(MenuWorkshopSubmitUI.previewField);
			MenuWorkshopSubmitUI.previewNotification = Glazier.Get().CreateBox();
			MenuWorkshopSubmitUI.previewNotification.PositionOffset_X = -240f;
			MenuWorkshopSubmitUI.previewNotification.PositionOffset_Y = 220f;
			MenuWorkshopSubmitUI.previewNotification.PositionScale_X = 0.5f;
			MenuWorkshopSubmitUI.previewNotification.SizeOffset_X = 30f;
			MenuWorkshopSubmitUI.previewNotification.SizeOffset_Y = 30f;
			MenuWorkshopSubmitUI.previewNotification.Text = "!";
			MenuWorkshopSubmitUI.container.AddChild(MenuWorkshopSubmitUI.previewNotification);
			MenuWorkshopSubmitUI.previewNotification.IsVisible = false;
			MenuWorkshopSubmitUI.changeField = Glazier.Get().CreateStringField();
			MenuWorkshopSubmitUI.changeField.PositionOffset_X = -200f;
			MenuWorkshopSubmitUI.changeField.PositionOffset_Y = 260f;
			MenuWorkshopSubmitUI.changeField.PositionScale_X = 0.5f;
			MenuWorkshopSubmitUI.changeField.SizeOffset_X = 400f;
			MenuWorkshopSubmitUI.changeField.SizeOffset_Y = 30f;
			MenuWorkshopSubmitUI.changeField.MaxLength = 128;
			MenuWorkshopSubmitUI.changeField.AddLabel(MenuWorkshopSubmitUI.localization.format("Change_Field_Label"), 1);
			MenuWorkshopSubmitUI.container.AddChild(MenuWorkshopSubmitUI.changeField);
			MenuWorkshopSubmitUI.typeState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Map")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Localization")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Object")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Vehicle")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin"))
			});
			MenuWorkshopSubmitUI.typeState.PositionOffset_X = -200f;
			MenuWorkshopSubmitUI.typeState.PositionOffset_Y = 300f;
			MenuWorkshopSubmitUI.typeState.PositionScale_X = 0.5f;
			MenuWorkshopSubmitUI.typeState.SizeOffset_X = 195f;
			MenuWorkshopSubmitUI.typeState.SizeOffset_Y = 30f;
			MenuWorkshopSubmitUI.typeState.onSwappedState = new SwappedState(MenuWorkshopSubmitUI.onSwappedTypeState);
			MenuWorkshopSubmitUI.container.AddChild(MenuWorkshopSubmitUI.typeState);
			MenuWorkshopSubmitUI.mapTypeState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Map_Type_Survival")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Map_Type_Horde")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Map_Type_Arena")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Map_Type_Custom"))
			});
			MenuWorkshopSubmitUI.mapTypeState.PositionOffset_X = 5f;
			MenuWorkshopSubmitUI.mapTypeState.PositionOffset_Y = 300f;
			MenuWorkshopSubmitUI.mapTypeState.PositionScale_X = 0.5f;
			MenuWorkshopSubmitUI.mapTypeState.SizeOffset_X = 195f;
			MenuWorkshopSubmitUI.mapTypeState.SizeOffset_Y = 30f;
			MenuWorkshopSubmitUI.container.AddChild(MenuWorkshopSubmitUI.mapTypeState);
			MenuWorkshopSubmitUI.mapTypeState.IsVisible = true;
			MenuWorkshopSubmitUI.itemTypeState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Backpack")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Barrel")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Barricade")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Fisher")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Food")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Fuel")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Glasses")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Grip")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Grower")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Gun")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Hat")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Magazine")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Mask")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Medical")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Melee")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Optic")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Shirt")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Sight")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Structure")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Supply")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Tactical")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Throwable")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Tool")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Vest")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Item_Type_Water"))
			});
			MenuWorkshopSubmitUI.itemTypeState.PositionOffset_X = 5f;
			MenuWorkshopSubmitUI.itemTypeState.PositionOffset_Y = 300f;
			MenuWorkshopSubmitUI.itemTypeState.PositionScale_X = 0.5f;
			MenuWorkshopSubmitUI.itemTypeState.SizeOffset_X = 195f;
			MenuWorkshopSubmitUI.itemTypeState.SizeOffset_Y = 30f;
			MenuWorkshopSubmitUI.container.AddChild(MenuWorkshopSubmitUI.itemTypeState);
			MenuWorkshopSubmitUI.itemTypeState.IsVisible = false;
			MenuWorkshopSubmitUI.vehicleTypeState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Vehicle_Type_Wheels_2")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Vehicle_Type_Wheels_4")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Vehicle_Type_Plane")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Vehicle_Type_Helicopter")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Vehicle_Type_Boat")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Vehicle_Type_Train"))
			});
			MenuWorkshopSubmitUI.vehicleTypeState.PositionOffset_X = 5f;
			MenuWorkshopSubmitUI.vehicleTypeState.PositionOffset_Y = 300f;
			MenuWorkshopSubmitUI.vehicleTypeState.PositionScale_X = 0.5f;
			MenuWorkshopSubmitUI.vehicleTypeState.SizeOffset_X = 195f;
			MenuWorkshopSubmitUI.vehicleTypeState.SizeOffset_Y = 30f;
			MenuWorkshopSubmitUI.container.AddChild(MenuWorkshopSubmitUI.vehicleTypeState);
			MenuWorkshopSubmitUI.vehicleTypeState.IsVisible = false;
			MenuWorkshopSubmitUI.skinTypeState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Generic_Pattern")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Ace")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Augewehr")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Avenger")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Bluntforce")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Bulldog")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Butterfly_Knife")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Calling_Card")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Cobra")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Colt")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Compound_Bow")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Crossbow")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Desert_Falcon")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Dragonfang")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Eaglefire")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Ekho")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Fusilaut")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Grizzly")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Hawkhound")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Heartbreaker")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Hell_Fury")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Honeybadger")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Katana")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Kryzkarek")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Machete")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Maplestrike")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Maschinengewehr")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Masterkey")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Matamorez")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Military_Knife")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Nightraider")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Nykorev")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Peacemaker")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Rocket_Launcher")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Sabertooth")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Scalar")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Schofield")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Shadowstalker")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Snayperskya")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Sportshot")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Teklowvka")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Timberwolf")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Viper")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Vonya")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Yuri")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Skin_Type_Zubeknakov"))
			});
			MenuWorkshopSubmitUI.skinTypeState.PositionOffset_X = 5f;
			MenuWorkshopSubmitUI.skinTypeState.PositionOffset_Y = 300f;
			MenuWorkshopSubmitUI.skinTypeState.PositionScale_X = 0.5f;
			MenuWorkshopSubmitUI.skinTypeState.SizeOffset_X = 195f;
			MenuWorkshopSubmitUI.skinTypeState.SizeOffset_Y = 30f;
			MenuWorkshopSubmitUI.container.AddChild(MenuWorkshopSubmitUI.skinTypeState);
			MenuWorkshopSubmitUI.skinTypeState.IsVisible = false;
			MenuWorkshopSubmitUI.objectTypeState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Object_Type_Model")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Object_Type_Resource")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Object_Type_Effect")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Object_Type_Animal"))
			});
			MenuWorkshopSubmitUI.objectTypeState.PositionOffset_X = 5f;
			MenuWorkshopSubmitUI.objectTypeState.PositionOffset_Y = 300f;
			MenuWorkshopSubmitUI.objectTypeState.PositionScale_X = 0.5f;
			MenuWorkshopSubmitUI.objectTypeState.SizeOffset_X = 195f;
			MenuWorkshopSubmitUI.objectTypeState.SizeOffset_Y = 30f;
			MenuWorkshopSubmitUI.container.AddChild(MenuWorkshopSubmitUI.objectTypeState);
			MenuWorkshopSubmitUI.objectTypeState.IsVisible = false;
			MenuWorkshopSubmitUI.visibilityState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Public")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Friends_Only")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Private")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Unlisted"))
			});
			MenuWorkshopSubmitUI.visibilityState.PositionOffset_X = -200f;
			MenuWorkshopSubmitUI.visibilityState.PositionOffset_Y = 340f;
			MenuWorkshopSubmitUI.visibilityState.PositionScale_X = 0.5f;
			MenuWorkshopSubmitUI.visibilityState.SizeOffset_X = 195f;
			MenuWorkshopSubmitUI.visibilityState.SizeOffset_Y = 30f;
			MenuWorkshopSubmitUI.container.AddChild(MenuWorkshopSubmitUI.visibilityState);
			MenuWorkshopSubmitUI.forState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Community")),
				new GUIContent(MenuWorkshopSubmitUI.localization.format("Review"))
			});
			MenuWorkshopSubmitUI.forState.PositionOffset_X = 5f;
			MenuWorkshopSubmitUI.forState.PositionOffset_Y = 340f;
			MenuWorkshopSubmitUI.forState.PositionScale_X = 0.5f;
			MenuWorkshopSubmitUI.forState.SizeOffset_X = 195f;
			MenuWorkshopSubmitUI.forState.SizeOffset_Y = 30f;
			MenuWorkshopSubmitUI.container.AddChild(MenuWorkshopSubmitUI.forState);
			MenuWorkshopSubmitUI.allowedIPsField = Glazier.Get().CreateStringField();
			MenuWorkshopSubmitUI.allowedIPsField.PositionOffset_X = -200f;
			MenuWorkshopSubmitUI.allowedIPsField.PositionOffset_Y = 380f;
			MenuWorkshopSubmitUI.allowedIPsField.PositionScale_X = 0.5f;
			MenuWorkshopSubmitUI.allowedIPsField.SizeOffset_X = 400f;
			MenuWorkshopSubmitUI.allowedIPsField.SizeOffset_Y = 30f;
			MenuWorkshopSubmitUI.allowedIPsField.MaxLength = 255;
			MenuWorkshopSubmitUI.allowedIPsField.TooltipText = MenuWorkshopSubmitUI.localization.format("Allowed_IPs_Tooltip");
			MenuWorkshopSubmitUI.allowedIPsField.PlaceholderText = MenuWorkshopSubmitUI.localization.format("Allowed_IPs_Hint");
			MenuWorkshopSubmitUI.allowedIPsField.AddLabel(MenuWorkshopSubmitUI.localization.format("Allowed_IPs_Label"), 1);
			MenuWorkshopSubmitUI.container.AddChild(MenuWorkshopSubmitUI.allowedIPsField);
			MenuWorkshopSubmitUI.createButton = new SleekButtonIcon(bundle.load<Texture2D>("Create"));
			MenuWorkshopSubmitUI.createButton.PositionOffset_X = -200f;
			MenuWorkshopSubmitUI.createButton.PositionOffset_Y = 420f;
			MenuWorkshopSubmitUI.createButton.PositionScale_X = 0.5f;
			MenuWorkshopSubmitUI.createButton.SizeOffset_X = 195f;
			MenuWorkshopSubmitUI.createButton.SizeOffset_Y = 30f;
			MenuWorkshopSubmitUI.createButton.text = MenuWorkshopSubmitUI.localization.format("Create_Button");
			MenuWorkshopSubmitUI.createButton.tooltip = MenuWorkshopSubmitUI.localization.format("Create_Button_Tooltip");
			MenuWorkshopSubmitUI.createButton.onClickedButton += new ClickedButton(MenuWorkshopSubmitUI.onClickedCreateButton);
			MenuWorkshopSubmitUI.createButton.iconColor = 2;
			MenuWorkshopSubmitUI.container.AddChild(MenuWorkshopSubmitUI.createButton);
			MenuWorkshopSubmitUI.legalButton = Glazier.Get().CreateButton();
			MenuWorkshopSubmitUI.legalButton.PositionOffset_X = 5f;
			MenuWorkshopSubmitUI.legalButton.PositionOffset_Y = 420f;
			MenuWorkshopSubmitUI.legalButton.PositionScale_X = 0.5f;
			MenuWorkshopSubmitUI.legalButton.SizeOffset_X = 195f;
			MenuWorkshopSubmitUI.legalButton.SizeOffset_Y = 30f;
			MenuWorkshopSubmitUI.legalButton.FontSize = 1;
			MenuWorkshopSubmitUI.legalButton.Text = MenuWorkshopSubmitUI.localization.format("Legal_Button");
			MenuWorkshopSubmitUI.legalButton.TooltipText = MenuWorkshopSubmitUI.localization.format("Legal_Button_Tooltip");
			MenuWorkshopSubmitUI.legalButton.OnClicked += new ClickedButton(MenuWorkshopSubmitUI.onClickedLegalButton);
			MenuWorkshopSubmitUI.container.AddChild(MenuWorkshopSubmitUI.legalButton);
			MenuWorkshopSubmitUI.publishedBox = Glazier.Get().CreateScrollView();
			MenuWorkshopSubmitUI.publishedBox.PositionOffset_X = -200f;
			MenuWorkshopSubmitUI.publishedBox.PositionOffset_Y = 460f;
			MenuWorkshopSubmitUI.publishedBox.PositionScale_X = 0.5f;
			MenuWorkshopSubmitUI.publishedBox.SizeOffset_X = 430f;
			MenuWorkshopSubmitUI.publishedBox.SizeOffset_Y = -460f;
			MenuWorkshopSubmitUI.publishedBox.SizeScale_Y = 1f;
			MenuWorkshopSubmitUI.publishedBox.ScaleContentToWidth = true;
			MenuWorkshopSubmitUI.container.AddChild(MenuWorkshopSubmitUI.publishedBox);
			MenuWorkshopSubmitUI.backButton = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			MenuWorkshopSubmitUI.backButton.PositionOffset_Y = -50f;
			MenuWorkshopSubmitUI.backButton.PositionScale_Y = 1f;
			MenuWorkshopSubmitUI.backButton.SizeOffset_X = 200f;
			MenuWorkshopSubmitUI.backButton.SizeOffset_Y = 50f;
			MenuWorkshopSubmitUI.backButton.text = MenuDashboardUI.localization.format("BackButtonText");
			MenuWorkshopSubmitUI.backButton.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			MenuWorkshopSubmitUI.backButton.onClickedButton += new ClickedButton(MenuWorkshopSubmitUI.onClickedBackButton);
			MenuWorkshopSubmitUI.backButton.fontSize = 3;
			MenuWorkshopSubmitUI.backButton.iconColor = 2;
			MenuWorkshopSubmitUI.container.AddChild(MenuWorkshopSubmitUI.backButton);
			MenuWorkshopSubmitUI.onPublishedAdded();
			bundle.unload();
		}

		// Token: 0x04002B8D RID: 11149
		private static Local localization;

		// Token: 0x04002B8E RID: 11150
		private static SleekFullscreenBox container;

		// Token: 0x04002B8F RID: 11151
		public static bool active;

		// Token: 0x04002B90 RID: 11152
		private static SleekButtonIcon backButton;

		// Token: 0x04002B91 RID: 11153
		private static ISleekField nameField;

		// Token: 0x04002B92 RID: 11154
		private static ISleekField descriptionField;

		// Token: 0x04002B93 RID: 11155
		private static ISleekField pathField;

		// Token: 0x04002B94 RID: 11156
		private static ISleekBox pathNotification;

		// Token: 0x04002B95 RID: 11157
		private static ISleekField previewField;

		// Token: 0x04002B96 RID: 11158
		private static ISleekBox previewNotification;

		// Token: 0x04002B97 RID: 11159
		private static ISleekField changeField;

		// Token: 0x04002B98 RID: 11160
		private static ISleekField allowedIPsField;

		// Token: 0x04002B99 RID: 11161
		private static SleekButtonState typeState;

		// Token: 0x04002B9A RID: 11162
		private static SleekButtonState mapTypeState;

		// Token: 0x04002B9B RID: 11163
		private static SleekButtonState itemTypeState;

		// Token: 0x04002B9C RID: 11164
		private static SleekButtonState vehicleTypeState;

		// Token: 0x04002B9D RID: 11165
		private static SleekButtonState skinTypeState;

		// Token: 0x04002B9E RID: 11166
		private static SleekButtonState objectTypeState;

		// Token: 0x04002B9F RID: 11167
		private static SleekButtonState visibilityState;

		// Token: 0x04002BA0 RID: 11168
		private static SleekButtonState forState;

		// Token: 0x04002BA1 RID: 11169
		private static SleekButtonIcon createButton;

		// Token: 0x04002BA2 RID: 11170
		private static ISleekButton legalButton;

		// Token: 0x04002BA3 RID: 11171
		private static ISleekScrollView publishedBox;

		// Token: 0x04002BA4 RID: 11172
		private static List<ISleekButton> publishedButtons;
	}
}
