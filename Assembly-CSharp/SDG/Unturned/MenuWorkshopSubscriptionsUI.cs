using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Repurposed from the Modules UI because it was unused.
	/// </summary>
	// Token: 0x020007B8 RID: 1976
	public class MenuWorkshopSubscriptionsUI
	{
		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x0600426C RID: 17004 RVA: 0x0016D3D3 File Offset: 0x0016B5D3
		// (set) Token: 0x0600426D RID: 17005 RVA: 0x0016D3DA File Offset: 0x0016B5DA
		public static MenuWorkshopSubscriptionsUI instance { get; private set; }

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x0600426E RID: 17006 RVA: 0x0016D3E2 File Offset: 0x0016B5E2
		// (set) Token: 0x0600426F RID: 17007 RVA: 0x0016D3E9 File Offset: 0x0016B5E9
		public static Local localization { get; private set; }

		// Token: 0x06004270 RID: 17008 RVA: 0x0016D3F1 File Offset: 0x0016B5F1
		public void open()
		{
			if (MenuWorkshopSubscriptionsUI.active)
			{
				return;
			}
			MenuWorkshopSubscriptionsUI.active = true;
			this.container.AnimateIntoView();
			this.synchronizeEntries();
		}

		// Token: 0x06004271 RID: 17009 RVA: 0x0016D412 File Offset: 0x0016B612
		public void close()
		{
			if (!MenuWorkshopSubscriptionsUI.active)
			{
				return;
			}
			MenuWorkshopSubscriptionsUI.active = false;
			this.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x06004272 RID: 17010 RVA: 0x0016D438 File Offset: 0x0016B638
		private bool hasEntry(PublishedFileId_t fileId)
		{
			return this.entryWidgets.FindIndex((SleekManageWorkshopEntry x) => x.fileId == fileId) >= 0;
		}

		// Token: 0x06004273 RID: 17011 RVA: 0x0016D470 File Offset: 0x0016B670
		private void addEntry(PublishedFileId_t fileId)
		{
			SleekManageWorkshopEntry sleekManageWorkshopEntry = new SleekManageWorkshopEntry(fileId);
			sleekManageWorkshopEntry.PositionOffset_Y = (float)(this.entryWidgets.Count * 50);
			sleekManageWorkshopEntry.SizeOffset_Y = 40f;
			sleekManageWorkshopEntry.SizeScale_X = 1f;
			this.moduleBox.AddChild(sleekManageWorkshopEntry);
			this.entryWidgets.Add(sleekManageWorkshopEntry);
		}

		// Token: 0x06004274 RID: 17012 RVA: 0x0016D4C8 File Offset: 0x0016B6C8
		private void synchronizeEntries()
		{
			if (this.entryWidgets == null)
			{
				this.entryWidgets = new List<SleekManageWorkshopEntry>();
			}
			List<SteamContent> ugc = Provider.provider.workshopService.ugc;
			if (ugc != null && this.entryWidgets.Count != ugc.Count)
			{
				foreach (SteamContent steamContent in ugc)
				{
					PublishedFileId_t publishedFileID = steamContent.publishedFileID;
					if (!this.hasEntry(publishedFileID))
					{
						this.addEntry(publishedFileID);
					}
				}
			}
			if (this.entryWidgets.Count > 0)
			{
				if (this.emptyBox != null)
				{
					this.container.RemoveChild(this.emptyBox);
					this.emptyBox = null;
				}
			}
			else
			{
				this.emptyBox = Glazier.Get().CreateBox();
				this.emptyBox.PositionOffset_Y = 60f;
				this.emptyBox.SizeOffset_Y = 50f;
				this.emptyBox.SizeScale_X = 1f;
				this.emptyBox.FontSize = 3;
				this.emptyBox.Text = MenuWorkshopSubscriptionsUI.localization.format("No_Subscriptions");
				this.container.AddChild(this.emptyBox);
			}
			this.moduleBox.ContentSizeOffset = new Vector2(0f, (float)(this.entryWidgets.Count * 50 - 10));
		}

		// Token: 0x06004275 RID: 17013 RVA: 0x0016D630 File Offset: 0x0016B830
		private void onClickedManageInOverlayButton(ISleekElement button)
		{
			if (Provider.provider.browserService.canOpenBrowser)
			{
				string url = string.Format("https://steamcommunity.com/my/myworkshopfiles/?appid={0}&browsefilter=mysubscriptions", Provider.APP_ID);
				Provider.provider.browserService.open(url);
				return;
			}
			MenuUI.alert(MenuDashboardUI.localization.format("Overlay"));
		}

		// Token: 0x06004276 RID: 17014 RVA: 0x0016D688 File Offset: 0x0016B888
		private void onClickedBackButton(ISleekElement button)
		{
			MenuWorkshopUI.open();
			this.close();
		}

		// Token: 0x06004277 RID: 17015 RVA: 0x0016D698 File Offset: 0x0016B898
		public MenuWorkshopSubscriptionsUI()
		{
			MenuWorkshopSubscriptionsUI.instance = this;
			MenuWorkshopSubscriptionsUI.localization = Localization.read("/Menu/Workshop/MenuWorkshopSubscriptions.dat");
			this.container = new SleekFullscreenBox();
			this.container.PositionOffset_X = 10f;
			this.container.PositionOffset_Y = 10f;
			this.container.PositionScale_Y = 1f;
			this.container.SizeOffset_X = -20f;
			this.container.SizeOffset_Y = -20f;
			this.container.SizeScale_X = 1f;
			this.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(this.container);
			MenuWorkshopSubscriptionsUI.active = false;
			this.headerBox = Glazier.Get().CreateBox();
			this.headerBox.SizeOffset_Y = 50f;
			this.headerBox.SizeScale_X = 1f;
			this.container.AddChild(this.headerBox);
			ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
			sleekLabel.PositionOffset_X = 10f;
			sleekLabel.SizeOffset_X = -210f;
			sleekLabel.SizeOffset_Y = 30f;
			sleekLabel.SizeScale_X = 1f;
			sleekLabel.FontSize = 3;
			sleekLabel.Text = MenuWorkshopSubscriptionsUI.localization.format("Header");
			this.headerBox.AddChild(sleekLabel);
			ISleekLabel sleekLabel2 = Glazier.Get().CreateLabel();
			sleekLabel2.PositionOffset_X = 10f;
			sleekLabel2.PositionOffset_Y = -30f;
			sleekLabel2.PositionScale_Y = 1f;
			sleekLabel2.SizeOffset_X = -210f;
			sleekLabel2.SizeOffset_Y = 30f;
			sleekLabel2.SizeScale_X = 1f;
			sleekLabel2.Text = MenuWorkshopSubscriptionsUI.localization.format("Enable_Warning");
			sleekLabel2.FontStyle = 2;
			this.headerBox.AddChild(sleekLabel2);
			ISleekButton sleekButton = Glazier.Get().CreateButton();
			sleekButton.PositionOffset_X = -210f;
			sleekButton.PositionOffset_Y = -15f;
			sleekButton.PositionScale_X = 1f;
			sleekButton.PositionScale_Y = 0.5f;
			sleekButton.SizeOffset_X = 200f;
			sleekButton.SizeOffset_Y = 30f;
			sleekButton.Text = MenuWorkshopSubscriptionsUI.localization.format("Manage_Label");
			sleekButton.TooltipText = MenuWorkshopSubscriptionsUI.localization.format("Manage_Tooltip");
			sleekButton.OnClicked += new ClickedButton(this.onClickedManageInOverlayButton);
			this.headerBox.AddChild(sleekButton);
			this.moduleBox = Glazier.Get().CreateScrollView();
			this.moduleBox.PositionOffset_Y = 60f;
			this.moduleBox.SizeOffset_Y = -120f;
			this.moduleBox.SizeScale_X = 1f;
			this.moduleBox.SizeScale_Y = 1f;
			this.moduleBox.ScaleContentToWidth = true;
			this.container.AddChild(this.moduleBox);
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
			this.container.AddChild(this.backButton);
		}

		// Token: 0x04002BA7 RID: 11175
		private SleekFullscreenBox container;

		// Token: 0x04002BA8 RID: 11176
		public static bool active;

		// Token: 0x04002BA9 RID: 11177
		private SleekButtonIcon backButton;

		// Token: 0x04002BAA RID: 11178
		private ISleekBox headerBox;

		// Token: 0x04002BAB RID: 11179
		private ISleekScrollView moduleBox;

		// Token: 0x04002BAC RID: 11180
		private List<SleekManageWorkshopEntry> entryWidgets;

		// Token: 0x04002BAD RID: 11181
		private ISleekBox emptyBox;
	}
}
