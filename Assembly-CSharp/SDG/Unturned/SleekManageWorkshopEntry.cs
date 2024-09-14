using System;
using SDG.Provider;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020007BA RID: 1978
	public class SleekManageWorkshopEntry : SleekWrapper
	{
		// Token: 0x0600428D RID: 17037 RVA: 0x0016EA40 File Offset: 0x0016CC40
		public SleekManageWorkshopEntry(PublishedFileId_t fileId)
		{
			this.fileId = fileId;
			CachedUGCDetails cachedUGCDetails;
			bool cachedDetails = TempSteamworksWorkshop.getCachedDetails(fileId, out cachedUGCDetails);
			string text = cachedDetails ? cachedUGCDetails.GetTitle() : fileId.ToString();
			ISleekBox sleekBox = Glazier.Get().CreateBox();
			sleekBox.SizeScale_X = 1f;
			sleekBox.SizeScale_Y = 1f;
			base.AddChild(sleekBox);
			ISleekToggle sleekToggle = Glazier.Get().CreateToggle();
			sleekToggle.PositionOffset_Y = -20f;
			sleekToggle.PositionScale_Y = 0.5f;
			sleekToggle.SizeOffset_X = 40f;
			sleekToggle.SizeOffset_Y = 40f;
			sleekToggle.OnValueChanged += new Toggled(this.onToggledEnabled);
			sleekToggle.Value = this.getEnabled();
			base.AddChild(sleekToggle);
			ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
			sleekLabel.PositionOffset_X = 40f;
			sleekLabel.PositionOffset_Y = -15f;
			sleekLabel.PositionScale_Y = 0.5f;
			sleekLabel.SizeOffset_X = -40f;
			sleekLabel.SizeOffset_Y = 30f;
			sleekLabel.SizeScale_X = 1f;
			sleekLabel.FontSize = 3;
			sleekLabel.TextAlignment = 3;
			sleekLabel.Text = text;
			sleekLabel.TextColor = (cachedUGCDetails.isBannedOrPrivate ? 6 : 3);
			base.AddChild(sleekLabel);
			float num = -5f;
			SleekWorkshopSubscriptionButton sleekWorkshopSubscriptionButton = new SleekWorkshopSubscriptionButton();
			sleekWorkshopSubscriptionButton.PositionOffset_Y = -15f;
			sleekWorkshopSubscriptionButton.PositionScale_X = 1f;
			sleekWorkshopSubscriptionButton.PositionScale_Y = 0.5f;
			sleekWorkshopSubscriptionButton.SizeOffset_X = 100f;
			sleekWorkshopSubscriptionButton.SizeOffset_Y = 30f;
			num -= sleekWorkshopSubscriptionButton.SizeOffset_X;
			sleekWorkshopSubscriptionButton.PositionOffset_X = num;
			sleekWorkshopSubscriptionButton.subscribeText = MenuWorkshopSubscriptionsUI.localization.format("Subscribe_Label");
			sleekWorkshopSubscriptionButton.unsubscribeText = MenuWorkshopSubscriptionsUI.localization.format("Unsubscribe_Label");
			sleekWorkshopSubscriptionButton.subscribeTooltip = MenuWorkshopSubscriptionsUI.localization.format("Subscribe_Tooltip", text);
			sleekWorkshopSubscriptionButton.unsubscribeTooltip = MenuWorkshopSubscriptionsUI.localization.format("Unsubscribe_Tooltip", text);
			sleekWorkshopSubscriptionButton.fileId = fileId;
			sleekWorkshopSubscriptionButton.synchronizeText();
			base.AddChild(sleekWorkshopSubscriptionButton);
			num -= 5f;
			ISleekButton sleekButton = Glazier.Get().CreateButton();
			sleekButton.PositionOffset_Y = -15f;
			sleekButton.PositionScale_X = 1f;
			sleekButton.PositionScale_Y = 0.5f;
			sleekButton.SizeOffset_X = 100f;
			sleekButton.SizeOffset_Y = 30f;
			num -= sleekButton.SizeOffset_X;
			sleekButton.PositionOffset_X = num;
			sleekButton.Text = MenuWorkshopSubscriptionsUI.localization.format("View_Label");
			sleekButton.TooltipText = MenuWorkshopSubscriptionsUI.localization.format("View_Tooltip", text);
			sleekButton.TextAlignment = 4;
			sleekButton.OnClicked += new ClickedButton(this.onClickedViewButton);
			base.AddChild(sleekButton);
			num -= 5f;
			if (ReadWrite.SupportsOpeningFileBrowser)
			{
				ulong num2;
				uint num3;
				if (SteamUGC.GetItemInstallInfo(fileId, out num2, out this.installPath, 1024U, out num3))
				{
					ISleekButton sleekButton2 = Glazier.Get().CreateButton();
					sleekButton2.PositionOffset_Y = -15f;
					sleekButton2.PositionScale_X = 1f;
					sleekButton2.PositionScale_Y = 0.5f;
					sleekButton2.SizeOffset_X = 100f;
					sleekButton2.SizeOffset_Y = 30f;
					num -= sleekButton2.SizeOffset_X;
					sleekButton2.PositionOffset_X = num;
					sleekButton2.Text = MenuWorkshopSubscriptionsUI.localization.format("BrowseFiles_Label");
					sleekButton2.TooltipText = MenuWorkshopSubscriptionsUI.localization.format("BrowseFiles_Tooltip", text);
					sleekButton2.OnClicked += new ClickedButton(this.OnClickedBrowseFilesButton);
					base.AddChild(sleekButton2);
					num -= 5f;
				}
				else
				{
					ISleekLabel sleekLabel2 = Glazier.Get().CreateLabel();
					sleekLabel2.PositionOffset_Y = -15f;
					sleekLabel2.PositionScale_X = 1f;
					sleekLabel2.PositionScale_Y = 0.5f;
					sleekLabel2.SizeOffset_X = 100f;
					sleekLabel2.SizeOffset_Y = 30f;
					num -= sleekLabel2.SizeOffset_X;
					sleekLabel2.PositionOffset_X = num;
					sleekLabel2.Text = MenuWorkshopSubscriptionsUI.localization.format("NotInstalledLabel");
					sleekLabel2.TextColor = 6;
					base.AddChild(sleekLabel2);
					num -= 5f;
				}
				ISleekLabel sleekLabel3 = Glazier.Get().CreateLabel();
				sleekLabel3.PositionScale_X = 1f;
				sleekLabel3.SizeOffset_X = 150f;
				sleekLabel3.SizeScale_Y = 1f;
				num -= sleekLabel3.SizeOffset_X;
				sleekLabel3.PositionOffset_X = num;
				sleekLabel3.Text = MenuWorkshopSubscriptionsUI.localization.format("LocalTimestampLabel") + "\n" + DateTimeEx.FromUtcUnixTimeSeconds(num3).ToLocalTime().ToString();
				sleekLabel3.FontSize = 1;
				base.AddChild(sleekLabel3);
				num -= 5f;
			}
			if (cachedDetails)
			{
				ISleekLabel sleekLabel4 = Glazier.Get().CreateLabel();
				sleekLabel4.PositionScale_X = 1f;
				sleekLabel4.SizeOffset_X = 150f;
				sleekLabel4.SizeScale_Y = 1f;
				num -= sleekLabel4.SizeOffset_X;
				sleekLabel4.PositionOffset_X = num;
				sleekLabel4.Text = MenuWorkshopSubscriptionsUI.localization.format("RemoteTimestampLabel") + "\n" + DateTimeEx.FromUtcUnixTimeSeconds(cachedUGCDetails.updateTimestamp).ToLocalTime().ToString();
				sleekLabel4.FontSize = 1;
				base.AddChild(sleekLabel4);
				num -= 5f;
			}
			if ((SteamUGC.GetItemState(fileId) & 8U) == 8U)
			{
				ISleekLabel sleekLabel5 = Glazier.Get().CreateLabel();
				sleekLabel5.PositionOffset_Y = -15f;
				sleekLabel5.PositionScale_X = 1f;
				sleekLabel5.PositionScale_Y = 0.5f;
				sleekLabel5.SizeOffset_X = 100f;
				sleekLabel5.SizeOffset_Y = 30f;
				num -= sleekLabel5.SizeOffset_X;
				sleekLabel5.PositionOffset_X = num;
				sleekLabel5.Text = MenuWorkshopSubscriptionsUI.localization.format("ItemState_NeedsUpdate");
				sleekLabel5.TextColor = 6;
				base.AddChild(sleekLabel5);
				num -= 5f;
			}
		}

		// Token: 0x0600428E RID: 17038 RVA: 0x0016F060 File Offset: 0x0016D260
		protected bool getEnabled()
		{
			return LocalWorkshopSettings.get().getEnabled(this.fileId);
		}

		// Token: 0x0600428F RID: 17039 RVA: 0x0016F072 File Offset: 0x0016D272
		protected void setEnabled(bool newEnabled)
		{
			LocalWorkshopSettings.get().setEnabled(this.fileId, newEnabled);
		}

		// Token: 0x06004290 RID: 17040 RVA: 0x0016F085 File Offset: 0x0016D285
		protected void onToggledEnabled(ISleekToggle toggle, bool state)
		{
			this.setEnabled(state);
		}

		// Token: 0x06004291 RID: 17041 RVA: 0x0016F08E File Offset: 0x0016D28E
		protected void OnClickedBrowseFilesButton(ISleekElement button)
		{
			ReadWrite.OpenFileBrowser(this.installPath);
		}

		// Token: 0x06004292 RID: 17042 RVA: 0x0016F09C File Offset: 0x0016D29C
		protected void onClickedViewButton(ISleekElement viewButton)
		{
			if (Provider.provider.browserService.canOpenBrowser)
			{
				string text = "http://steamcommunity.com/sharedfiles/filedetails/?id=";
				PublishedFileId_t publishedFileId_t = this.fileId;
				string url = text + publishedFileId_t.ToString();
				Provider.provider.browserService.open(url);
				return;
			}
			MenuUI.alert(MenuDashboardUI.localization.format("Overlay"));
		}

		// Token: 0x04002BCC RID: 11212
		public PublishedFileId_t fileId;

		// Token: 0x04002BCD RID: 11213
		private string installPath;
	}
}
