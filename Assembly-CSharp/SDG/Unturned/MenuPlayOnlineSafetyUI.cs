using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200079F RID: 1951
	public class MenuPlayOnlineSafetyUI : SleekFullscreenBox
	{
		// Token: 0x060040CC RID: 16588 RVA: 0x00151190 File Offset: 0x0014F390
		public void OpenIfNecessary(EOnlineSafetyDestination destination)
		{
			if (OptionsSettings.ShouldShowOnlineSafetyMenu)
			{
				this.open(destination);
				return;
			}
			this.destination = destination;
			this.ProceedToDestination();
		}

		// Token: 0x060040CD RID: 16589 RVA: 0x001511AE File Offset: 0x0014F3AE
		public void open(EOnlineSafetyDestination destination)
		{
			if (this.active)
			{
				return;
			}
			this.active = true;
			this.destination = destination;
			this.SynchronizeValues();
			base.AnimateIntoView();
		}

		// Token: 0x060040CE RID: 16590 RVA: 0x001511D3 File Offset: 0x0014F3D3
		public void close()
		{
			if (!this.active)
			{
				return;
			}
			MenuSettings.SaveOptionsIfLoaded();
			this.active = false;
			base.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x060040CF RID: 16591 RVA: 0x001511FC File Offset: 0x0014F3FC
		private void ProceedToDestination()
		{
			switch (this.destination)
			{
			case EOnlineSafetyDestination.Connect:
				MenuPlayConnectUI.open();
				return;
			case EOnlineSafetyDestination.ServerList:
				MenuPlayUI.serverListUI.open(true);
				return;
			case EOnlineSafetyDestination.Bookmarks:
				MenuPlayUI.serverBookmarksUI.open();
				return;
			case EOnlineSafetyDestination.Lobby:
				MenuPlayLobbiesUI.open();
				return;
			default:
				return;
			}
		}

		// Token: 0x060040D0 RID: 16592 RVA: 0x00151249 File Offset: 0x0014F449
		private void OnBackClicked(ISleekElement button)
		{
			MenuPlayUI.open();
			this.close();
		}

		// Token: 0x060040D1 RID: 16593 RVA: 0x00151256 File Offset: 0x0014F456
		private void OnContinueClicked(ISleekElement button)
		{
			OptionsSettings.onlineSafetyMenuProceedCount++;
			OptionsSettings.didProceedThroughOnlineSafetyMenuThisSession = true;
			this.ProceedToDestination();
			this.close();
		}

		// Token: 0x060040D2 RID: 16594 RVA: 0x00151276 File Offset: 0x0014F476
		private void OnProfanityFilterToggled(ISleekToggle toggle, bool state)
		{
			OptionsSettings.filter = state;
			this.SynchronizeValues();
		}

		// Token: 0x060040D3 RID: 16595 RVA: 0x00151284 File Offset: 0x0014F484
		private void OnInboundVoiceChatToggled(ISleekToggle toggle, bool state)
		{
			OptionsSettings.chatVoiceIn = state;
			OptionsSettings.chatVoiceOut = (OptionsSettings.chatVoiceOut && state);
			this.SynchronizeValues();
		}

		// Token: 0x060040D4 RID: 16596 RVA: 0x0015129E File Offset: 0x0014F49E
		private void OnOutboundVoiceChatToggled(ISleekToggle toggle, bool state)
		{
			OptionsSettings.chatVoiceOut = state;
			this.SynchronizeValues();
		}

		// Token: 0x060040D5 RID: 16597 RVA: 0x001512AC File Offset: 0x0014F4AC
		private void OnStreamerModeToggled(ISleekToggle toggle, bool state)
		{
			OptionsSettings.streamer = state;
			this.SynchronizeValues();
		}

		// Token: 0x060040D6 RID: 16598 RVA: 0x001512BA File Offset: 0x0014F4BA
		private void OnDontShowAgainToggled(ISleekToggle toggle, bool state)
		{
			OptionsSettings.wantsToHideOnlineSafetyMenu = state;
		}

		// Token: 0x060040D7 RID: 16599 RVA: 0x001512C4 File Offset: 0x0014F4C4
		private void SynchronizeValues()
		{
			this.profanityFilterToggle.Value = OptionsSettings.filter;
			this.profanityFilter_Header.Text = this.localization.format("ProfanityFilter_Header", this.localization.format(OptionsSettings.filter ? "Feature_On" : "Feature_Off"));
			this.inboundVoiceChatToggle.Value = OptionsSettings.chatVoiceIn;
			this.inboundVoiceChat_Header.Text = this.localization.format("InboundVoiceChat_Header", this.localization.format(OptionsSettings.chatVoiceIn ? "Feature_On" : "Feature_Off"));
			this.outboundVoiceChatToggle.Value = OptionsSettings.chatVoiceOut;
			this.outboundVoiceChat_Header.Text = this.localization.format("OutboundVoiceChat_Header", this.localization.format(OptionsSettings.chatVoiceOut ? "Feature_On" : "Feature_Off"));
			this.outboundVoiceChat_Description.Text = this.localization.format("OutboundVoiceChat_Description", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.voice));
			this.outboundVoiceChatToggle.IsInteractable = OptionsSettings.chatVoiceIn;
			this.outboundVoiceChat_Header.TextColor = new SleekColor(3, OptionsSettings.chatVoiceIn ? 1f : 0.5f);
			this.outboundVoiceChat_Description.TextColor = new SleekColor(4, OptionsSettings.chatVoiceIn ? 1f : 0.5f);
			this.streamerModeToggle.Value = OptionsSettings.streamer;
			this.streamerMode_Header.Text = this.localization.format("StreamerMode_Header", this.localization.format(OptionsSettings.streamer ? "Feature_On" : "Feature_Off"));
			this.dontShowAgainToggle.Value = OptionsSettings.wantsToHideOnlineSafetyMenu;
		}

		// Token: 0x060040D8 RID: 16600 RVA: 0x00151484 File Offset: 0x0014F684
		public MenuPlayOnlineSafetyUI()
		{
			this.active = false;
			this.localization = Localization.read("/Menu/Play/MenuPlayOnlineSafety.dat");
			Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Play/MenuPlayOnlineSafety/MenuPlayOnlineSafety.unity3d");
			ISleekBox sleekBox = Glazier.Get().CreateBox();
			sleekBox.SizeScale_X = 1f;
			sleekBox.SizeScale_Y = 1f;
			sleekBox.BackgroundColor = new SleekColor(1, 0.5f);
			base.AddChild(sleekBox);
			ISleekScrollView sleekScrollView = Glazier.Get().CreateScrollView();
			sleekScrollView.PositionOffset_X = -380f;
			sleekScrollView.PositionScale_X = 0.5f;
			sleekScrollView.PositionScale_Y = 0.1f;
			sleekScrollView.SizeOffset_X = 790f;
			sleekScrollView.SizeScale_Y = 0.8f;
			sleekScrollView.ScaleContentToWidth = true;
			base.AddChild(sleekScrollView);
			float num = 0f;
			ISleekImage sleekImage = Glazier.Get().CreateImage(bundle.load<Texture2D>("OnlineSafetyAlert"));
			sleekImage.PositionScale_X = 0.5f;
			sleekImage.PositionOffset_X = -64f;
			sleekImage.PositionOffset_Y = num;
			sleekImage.SizeOffset_X = 128f;
			sleekImage.SizeOffset_Y = 128f;
			sleekImage.TintColor = 2;
			sleekScrollView.AddChild(sleekImage);
			num += 128f;
			ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
			sleekLabel.PositionOffset_Y = num;
			sleekLabel.SizeScale_X = 1f;
			sleekLabel.SizeOffset_Y = 50f;
			sleekLabel.Text = this.localization.format("Header");
			sleekLabel.FontSize = 4;
			sleekLabel.TextContrastContext = 2;
			sleekScrollView.AddChild(sleekLabel);
			num += 40f;
			ISleekLabel sleekLabel2 = Glazier.Get().CreateLabel();
			sleekLabel2.PositionOffset_Y = num;
			sleekLabel2.SizeScale_X = 1f;
			sleekLabel2.SizeOffset_Y = 70f;
			sleekLabel2.Text = this.localization.format("Warning");
			sleekLabel2.TextContrastContext = 2;
			sleekScrollView.AddChild(sleekLabel2);
			num += sleekLabel2.SizeOffset_Y + 10f;
			this.profanityFilterToggle = Glazier.Get().CreateToggle();
			this.profanityFilterToggle.PositionOffset_X = -240f;
			this.profanityFilterToggle.PositionOffset_Y = num;
			this.profanityFilterToggle.PositionScale_X = 0.5f;
			this.profanityFilterToggle.SizeOffset_X = 40f;
			this.profanityFilterToggle.SizeOffset_Y = 40f;
			this.profanityFilterToggle.OnValueChanged += new Toggled(this.OnProfanityFilterToggled);
			sleekScrollView.AddChild(this.profanityFilterToggle);
			this.profanityFilter_Header = Glazier.Get().CreateLabel();
			this.profanityFilter_Header.PositionOffset_X = -190f;
			this.profanityFilter_Header.PositionOffset_Y = num - 10f;
			this.profanityFilter_Header.PositionScale_X = 0.5f;
			this.profanityFilter_Header.SizeOffset_X = 400f;
			this.profanityFilter_Header.SizeOffset_Y = 30f;
			this.profanityFilter_Header.TextAlignment = 6;
			this.profanityFilter_Header.TextContrastContext = 2;
			sleekScrollView.AddChild(this.profanityFilter_Header);
			ISleekLabel sleekLabel3 = Glazier.Get().CreateLabel();
			sleekLabel3.PositionOffset_X = -190f;
			sleekLabel3.PositionOffset_Y = num + 20f;
			sleekLabel3.PositionScale_X = 0.5f;
			sleekLabel3.SizeOffset_X = 400f;
			sleekLabel3.SizeOffset_Y = 50f;
			sleekLabel3.TextAlignment = 0;
			sleekLabel3.TextColor = 4;
			sleekLabel3.TextContrastContext = 2;
			sleekLabel3.Text = this.localization.format("ProfanityFilter_Description");
			sleekScrollView.AddChild(sleekLabel3);
			num += 60f;
			this.inboundVoiceChatToggle = Glazier.Get().CreateToggle();
			this.inboundVoiceChatToggle.PositionOffset_X = -240f;
			this.inboundVoiceChatToggle.PositionOffset_Y = num;
			this.inboundVoiceChatToggle.PositionScale_X = 0.5f;
			this.inboundVoiceChatToggle.SizeOffset_X = 40f;
			this.inboundVoiceChatToggle.SizeOffset_Y = 40f;
			this.inboundVoiceChatToggle.OnValueChanged += new Toggled(this.OnInboundVoiceChatToggled);
			sleekScrollView.AddChild(this.inboundVoiceChatToggle);
			this.inboundVoiceChat_Header = Glazier.Get().CreateLabel();
			this.inboundVoiceChat_Header.PositionOffset_X = -190f;
			this.inboundVoiceChat_Header.PositionOffset_Y = num - 10f;
			this.inboundVoiceChat_Header.PositionScale_X = 0.5f;
			this.inboundVoiceChat_Header.SizeOffset_X = 400f;
			this.inboundVoiceChat_Header.SizeOffset_Y = 30f;
			this.inboundVoiceChat_Header.TextAlignment = 6;
			this.inboundVoiceChat_Header.TextContrastContext = 2;
			sleekScrollView.AddChild(this.inboundVoiceChat_Header);
			ISleekLabel sleekLabel4 = Glazier.Get().CreateLabel();
			sleekLabel4.PositionOffset_X = -190f;
			sleekLabel4.PositionOffset_Y = num + 20f;
			sleekLabel4.PositionScale_X = 0.5f;
			sleekLabel4.SizeOffset_X = 400f;
			sleekLabel4.SizeOffset_Y = 50f;
			sleekLabel4.TextAlignment = 0;
			sleekLabel4.TextColor = 4;
			sleekLabel4.TextContrastContext = 2;
			sleekLabel4.Text = this.localization.format("InboundVoiceChat_Description");
			sleekScrollView.AddChild(sleekLabel4);
			num += 60f;
			this.outboundVoiceChatToggle = Glazier.Get().CreateToggle();
			this.outboundVoiceChatToggle.PositionOffset_X = -240f;
			this.outboundVoiceChatToggle.PositionOffset_Y = num;
			this.outboundVoiceChatToggle.PositionScale_X = 0.5f;
			this.outboundVoiceChatToggle.SizeOffset_X = 40f;
			this.outboundVoiceChatToggle.SizeOffset_Y = 40f;
			this.outboundVoiceChatToggle.OnValueChanged += new Toggled(this.OnOutboundVoiceChatToggled);
			sleekScrollView.AddChild(this.outboundVoiceChatToggle);
			this.outboundVoiceChat_Header = Glazier.Get().CreateLabel();
			this.outboundVoiceChat_Header.PositionOffset_X = -190f;
			this.outboundVoiceChat_Header.PositionOffset_Y = num - 10f;
			this.outboundVoiceChat_Header.PositionScale_X = 0.5f;
			this.outboundVoiceChat_Header.SizeOffset_X = 400f;
			this.outboundVoiceChat_Header.SizeOffset_Y = 30f;
			this.outboundVoiceChat_Header.TextAlignment = 6;
			this.outboundVoiceChat_Header.TextContrastContext = 2;
			sleekScrollView.AddChild(this.outboundVoiceChat_Header);
			this.outboundVoiceChat_Description = Glazier.Get().CreateLabel();
			this.outboundVoiceChat_Description.PositionOffset_X = -190f;
			this.outboundVoiceChat_Description.PositionOffset_Y = num + 20f;
			this.outboundVoiceChat_Description.PositionScale_X = 0.5f;
			this.outboundVoiceChat_Description.SizeOffset_X = 400f;
			this.outboundVoiceChat_Description.SizeOffset_Y = 50f;
			this.outboundVoiceChat_Description.TextAlignment = 0;
			this.outboundVoiceChat_Description.TextColor = 4;
			this.outboundVoiceChat_Description.TextContrastContext = 2;
			sleekScrollView.AddChild(this.outboundVoiceChat_Description);
			num += 60f;
			this.streamerModeToggle = Glazier.Get().CreateToggle();
			this.streamerModeToggle.PositionOffset_X = -240f;
			this.streamerModeToggle.PositionOffset_Y = num;
			this.streamerModeToggle.PositionScale_X = 0.5f;
			this.streamerModeToggle.SizeOffset_X = 40f;
			this.streamerModeToggle.SizeOffset_Y = 40f;
			this.streamerModeToggle.OnValueChanged += new Toggled(this.OnStreamerModeToggled);
			sleekScrollView.AddChild(this.streamerModeToggle);
			this.streamerMode_Header = Glazier.Get().CreateLabel();
			this.streamerMode_Header.PositionOffset_X = -190f;
			this.streamerMode_Header.PositionOffset_Y = num - 10f;
			this.streamerMode_Header.PositionScale_X = 0.5f;
			this.streamerMode_Header.SizeOffset_X = 400f;
			this.streamerMode_Header.SizeOffset_Y = 30f;
			this.streamerMode_Header.TextAlignment = 6;
			this.streamerMode_Header.TextContrastContext = 2;
			sleekScrollView.AddChild(this.streamerMode_Header);
			ISleekLabel sleekLabel5 = Glazier.Get().CreateLabel();
			sleekLabel5.PositionOffset_X = -190f;
			sleekLabel5.PositionOffset_Y = num + 20f;
			sleekLabel5.PositionScale_X = 0.5f;
			sleekLabel5.SizeOffset_X = 400f;
			sleekLabel5.SizeOffset_Y = 50f;
			sleekLabel5.TextAlignment = 0;
			sleekLabel5.TextColor = 4;
			sleekLabel5.TextContrastContext = 2;
			sleekLabel5.Text = this.localization.format("StreamerMode_Description");
			sleekScrollView.AddChild(sleekLabel5);
			num += 60f;
			ISleekLabel sleekLabel6 = Glazier.Get().CreateLabel();
			sleekLabel6.PositionOffset_Y = num;
			sleekLabel6.SizeScale_X = 1f;
			sleekLabel6.SizeOffset_Y = 30f;
			sleekLabel6.Text = this.localization.format("OptionsNote");
			sleekLabel6.TextContrastContext = 2;
			sleekLabel6.TextColor = 4;
			sleekScrollView.AddChild(sleekLabel6);
			num += sleekLabel6.SizeOffset_Y + 10f;
			SleekButtonIcon sleekButtonIcon = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Exit"));
			sleekButtonIcon.PositionOffset_X = -205f;
			sleekButtonIcon.PositionOffset_Y = num;
			sleekButtonIcon.PositionScale_X = 0.5f;
			sleekButtonIcon.SizeOffset_X = 200f;
			sleekButtonIcon.SizeOffset_Y = 50f;
			sleekButtonIcon.text = MenuDashboardUI.localization.format("BackButtonText");
			sleekButtonIcon.tooltip = MenuDashboardUI.localization.format("BackButtonTooltip");
			sleekButtonIcon.onClickedButton += new ClickedButton(this.OnBackClicked);
			sleekButtonIcon.fontSize = 3;
			sleekButtonIcon.iconColor = 2;
			sleekScrollView.AddChild(sleekButtonIcon);
			SleekButtonIcon sleekButtonIcon2 = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("Play"));
			sleekButtonIcon2.PositionOffset_X = 5f;
			sleekButtonIcon2.PositionOffset_Y = num;
			sleekButtonIcon2.PositionScale_X = 0.5f;
			sleekButtonIcon2.SizeOffset_X = 200f;
			sleekButtonIcon2.SizeOffset_Y = 50f;
			sleekButtonIcon2.text = this.localization.format("ContinueButton_Label");
			sleekButtonIcon2.tooltip = this.localization.format("ContinueButton_Tooltip");
			sleekButtonIcon2.onClickedButton += new ClickedButton(this.OnContinueClicked);
			sleekButtonIcon2.fontSize = 3;
			sleekButtonIcon2.iconColor = 2;
			sleekScrollView.AddChild(sleekButtonIcon2);
			num += 60f;
			this.dontShowAgainToggle = Glazier.Get().CreateToggle();
			this.dontShowAgainToggle.PositionOffset_X = 5f;
			this.dontShowAgainToggle.PositionOffset_Y = num;
			this.dontShowAgainToggle.PositionScale_X = 0.5f;
			this.dontShowAgainToggle.SizeOffset_X = 40f;
			this.dontShowAgainToggle.SizeOffset_Y = 40f;
			this.dontShowAgainToggle.AddLabel(this.localization.format("DontShowAgain_Label"), 1);
			this.dontShowAgainToggle.TooltipText = this.localization.format("DontShowAgain_Tooltip");
			this.dontShowAgainToggle.OnValueChanged += new Toggled(this.OnDontShowAgainToggled);
			sleekScrollView.AddChild(this.dontShowAgainToggle);
			num += 50f;
			sleekScrollView.ContentSizeOffset = new Vector2(0f, num - 10f);
			bundle.unload();
		}

		// Token: 0x040029B3 RID: 10675
		public Local localization;

		// Token: 0x040029B4 RID: 10676
		public Bundle icons;

		// Token: 0x040029B5 RID: 10677
		public bool active;

		// Token: 0x040029B6 RID: 10678
		private EOnlineSafetyDestination destination;

		// Token: 0x040029B7 RID: 10679
		private ISleekToggle profanityFilterToggle;

		// Token: 0x040029B8 RID: 10680
		private ISleekLabel profanityFilter_Header;

		// Token: 0x040029B9 RID: 10681
		private ISleekToggle inboundVoiceChatToggle;

		// Token: 0x040029BA RID: 10682
		private ISleekLabel inboundVoiceChat_Header;

		// Token: 0x040029BB RID: 10683
		private ISleekToggle outboundVoiceChatToggle;

		// Token: 0x040029BC RID: 10684
		private ISleekLabel outboundVoiceChat_Header;

		// Token: 0x040029BD RID: 10685
		private ISleekLabel outboundVoiceChat_Description;

		// Token: 0x040029BE RID: 10686
		private ISleekToggle streamerModeToggle;

		// Token: 0x040029BF RID: 10687
		private ISleekLabel streamerMode_Header;

		// Token: 0x040029C0 RID: 10688
		private ISleekToggle dontShowAgainToggle;
	}
}
