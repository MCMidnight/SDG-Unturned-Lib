using System;

namespace SDG.Unturned
{
	// Token: 0x02000710 RID: 1808
	public class SleekCuratedLevelLink : SleekWrapper
	{
		// Token: 0x06003BD2 RID: 15314 RVA: 0x00118BB0 File Offset: 0x00116DB0
		private void onClickedViewButton(ISleekElement button)
		{
			if (this.curatedMap == null)
			{
				return;
			}
			if (Provider.provider.browserService.canOpenBrowser)
			{
				string url = "http://steamcommunity.com/sharedfiles/filedetails/?id=" + this.curatedMap.Workshop_File_Id.ToString();
				Provider.provider.browserService.open(url);
				return;
			}
			MenuUI.alert(MenuDashboardUI.localization.format("Overlay"));
		}

		// Token: 0x06003BD3 RID: 15315 RVA: 0x00118C17 File Offset: 0x00116E17
		private bool getSubscribed()
		{
			return Provider.provider.workshopService.getSubscribed(this.curatedMap.Workshop_File_Id);
		}

		// Token: 0x06003BD4 RID: 15316 RVA: 0x00118C34 File Offset: 0x00116E34
		private void setSubscribed(bool subscribe)
		{
			Provider.provider.workshopService.setSubscribed(this.curatedMap.Workshop_File_Id, subscribe);
			if (subscribe)
			{
				foreach (ulong fileId in this.curatedMap.Required_Workshop_File_Ids)
				{
					Provider.provider.workshopService.setSubscribed(fileId, subscribe);
				}
			}
		}

		// Token: 0x06003BD5 RID: 15317 RVA: 0x00118C90 File Offset: 0x00116E90
		private void onClickedManageButton(ISleekElement button)
		{
			if (this.curatedMap == null)
			{
				return;
			}
			bool subscribed = !this.getSubscribed();
			this.updateManageLabel(subscribed);
			this.setSubscribed(subscribed);
		}

		// Token: 0x06003BD6 RID: 15318 RVA: 0x00118CC0 File Offset: 0x00116EC0
		private void updateManageLabel()
		{
			if (this.curatedMap == null)
			{
				return;
			}
			bool subscribed = this.getSubscribed();
			this.updateManageLabel(subscribed);
		}

		// Token: 0x06003BD7 RID: 15319 RVA: 0x00118CE4 File Offset: 0x00116EE4
		private void updateManageLabel(bool subscribed)
		{
			this.manageButton.Text = MenuPlaySingleplayerUI.localization.format(subscribed ? "Retired_Manage_Unsub" : "Retired_Manage_Sub");
		}

		// Token: 0x06003BD8 RID: 15320 RVA: 0x00118D0A File Offset: 0x00116F0A
		public override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x06003BD9 RID: 15321 RVA: 0x00118D14 File Offset: 0x00116F14
		public SleekCuratedLevelLink(CuratedMapLink curatedMap)
		{
			this.curatedMap = curatedMap;
			base.SizeOffset_X = 400f;
			base.SizeOffset_Y = 100f;
			this.backdrop = Glazier.Get().CreateBox();
			this.backdrop.SizeOffset_X = 0f;
			this.backdrop.SizeOffset_Y = 0f;
			this.backdrop.SizeScale_X = 1f;
			this.backdrop.SizeScale_Y = 1f;
			base.AddChild(this.backdrop);
			string path = "/CuratedMapIcons/" + curatedMap.Workshop_File_Id.ToString() + ".png";
			if (ReadWrite.fileExists(path, false, true))
			{
				this.icon = Glazier.Get().CreateImage();
				this.icon.PositionOffset_X = 10f;
				this.icon.PositionOffset_Y = 10f;
				this.icon.SizeOffset_X = -20f;
				this.icon.SizeOffset_Y = -20f;
				this.icon.SizeScale_X = 1f;
				this.icon.SizeScale_Y = 1f;
				this.icon.Texture = ReadWrite.readTextureFromFile(path, true, EReadTextureFromFileMode.UI);
				this.icon.ShouldDestroyTexture = true;
				this.backdrop.AddChild(this.icon);
			}
			this.nameLabel = Glazier.Get().CreateLabel();
			this.nameLabel.PositionOffset_Y = 10f;
			this.nameLabel.SizeScale_X = 1f;
			this.nameLabel.SizeOffset_Y = 50f;
			this.nameLabel.TextAlignment = 4;
			this.nameLabel.FontSize = 3;
			this.nameLabel.Text = curatedMap.Name;
			this.nameLabel.TextContrastContext = 2;
			this.backdrop.AddChild(this.nameLabel);
			this.viewOnWorkshopButton = Glazier.Get().CreateButton();
			this.viewOnWorkshopButton.PositionOffset_X = 15f;
			this.viewOnWorkshopButton.PositionOffset_Y = -45f;
			this.viewOnWorkshopButton.PositionScale_Y = 1f;
			this.viewOnWorkshopButton.SizeOffset_X = 150f;
			this.viewOnWorkshopButton.SizeOffset_Y = 30f;
			this.viewOnWorkshopButton.FontSize = 1;
			this.viewOnWorkshopButton.Text = MenuPlaySingleplayerUI.localization.format("Retired_View_Label");
			this.viewOnWorkshopButton.TooltipText = MenuPlaySingleplayerUI.localization.format("Retired_View_Tooltip");
			this.viewOnWorkshopButton.OnClicked += new ClickedButton(this.onClickedViewButton);
			this.backdrop.AddChild(this.viewOnWorkshopButton);
			this.manageButton = Glazier.Get().CreateButton();
			this.manageButton.PositionOffset_X = -165f;
			this.manageButton.PositionOffset_Y = -45f;
			this.manageButton.PositionScale_X = 1f;
			this.manageButton.PositionScale_Y = 1f;
			this.manageButton.SizeOffset_X = 150f;
			this.manageButton.SizeOffset_Y = 30f;
			this.manageButton.FontSize = 1;
			this.manageButton.TooltipText = MenuPlaySingleplayerUI.localization.format("Retired_Manage_Tooltip");
			this.updateManageLabel();
			this.manageButton.OnClicked += new ClickedButton(this.onClickedManageButton);
			this.backdrop.AddChild(this.manageButton);
		}

		// Token: 0x0400256E RID: 9582
		private CuratedMapLink curatedMap;

		// Token: 0x0400256F RID: 9583
		private ISleekBox backdrop;

		// Token: 0x04002570 RID: 9584
		private ISleekButton viewOnWorkshopButton;

		// Token: 0x04002571 RID: 9585
		private ISleekButton manageButton;

		// Token: 0x04002572 RID: 9586
		private ISleekImage icon;

		// Token: 0x04002573 RID: 9587
		private ISleekLabel nameLabel;
	}
}
