using System;

namespace SDG.Unturned
{
	// Token: 0x020007C0 RID: 1984
	internal class PlayerBrowserRequestUI : SleekFullscreenBox
	{
		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x060042BA RID: 17082 RVA: 0x00170EBB File Offset: 0x0016F0BB
		// (set) Token: 0x060042BB RID: 17083 RVA: 0x00170EC3 File Offset: 0x0016F0C3
		public bool isActive { get; private set; }

		// Token: 0x060042BC RID: 17084 RVA: 0x00170ECC File Offset: 0x0016F0CC
		public void open(string msg, string url)
		{
			if (this.isActive)
			{
				return;
			}
			this.isActive = true;
			this.url = url;
			this.textBox.Text = string.Concat(new string[]
			{
				this.localization.format("Request"),
				"\n",
				url,
				"\n\n\"",
				msg,
				"\""
			});
			base.AnimateIntoView();
		}

		// Token: 0x060042BD RID: 17085 RVA: 0x00170F3F File Offset: 0x0016F13F
		public void close()
		{
			if (!this.isActive)
			{
				return;
			}
			this.isActive = false;
			this.url = null;
			base.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x060042BE RID: 17086 RVA: 0x00170F68 File Offset: 0x0016F168
		private void onClickedYesButton(ISleekElement button)
		{
			if (!string.IsNullOrEmpty(this.url) && Provider.provider.browserService.canOpenBrowser)
			{
				string text;
				if (WebUtils.ParseThirdPartyUrl(this.url, out text, true, true))
				{
					Provider.provider.browserService.open(text);
				}
				else
				{
					UnturnedLog.error("Ignoring potentially unsafe browser request URL \"" + this.url + "\" (Error: Prompt shouldn't have been displayed if this is the case?)");
				}
			}
			PlayerLifeUI.open();
			this.close();
		}

		// Token: 0x060042BF RID: 17087 RVA: 0x00170FDB File Offset: 0x0016F1DB
		private void onClickedNoButton(ISleekElement button)
		{
			PlayerLifeUI.open();
			this.close();
		}

		// Token: 0x060042C0 RID: 17088 RVA: 0x00170FE8 File Offset: 0x0016F1E8
		public PlayerBrowserRequestUI()
		{
			this.localization = Localization.read("/Player/PlayerBrowserRequest.dat");
			base.PositionScale_Y = 1f;
			base.PositionOffset_X = 10f;
			base.PositionOffset_Y = 10f;
			base.SizeOffset_X = -20f;
			base.SizeOffset_Y = -20f;
			base.SizeScale_X = 1f;
			base.SizeScale_Y = 1f;
			this.isActive = false;
			this.url = null;
			this.textBox = Glazier.Get().CreateBox();
			this.textBox.PositionOffset_X = -200f;
			this.textBox.PositionOffset_Y = -50f;
			this.textBox.PositionScale_X = 0.5f;
			this.textBox.PositionScale_Y = 0.5f;
			this.textBox.SizeOffset_X = 400f;
			this.textBox.SizeOffset_Y = 100f;
			base.AddChild(this.textBox);
			this.yesButton = Glazier.Get().CreateButton();
			this.yesButton.PositionOffset_X = -200f;
			this.yesButton.PositionOffset_Y = 60f;
			this.yesButton.PositionScale_X = 0.5f;
			this.yesButton.PositionScale_Y = 0.5f;
			this.yesButton.SizeOffset_X = 195f;
			this.yesButton.SizeOffset_Y = 30f;
			this.yesButton.Text = this.localization.format("Yes_Button");
			this.yesButton.TooltipText = this.localization.format("Yes_Button_Tooltip");
			this.yesButton.OnClicked += new ClickedButton(this.onClickedYesButton);
			base.AddChild(this.yesButton);
			this.noButton = Glazier.Get().CreateButton();
			this.noButton.PositionOffset_X = 5f;
			this.noButton.PositionOffset_Y = 60f;
			this.noButton.PositionScale_X = 0.5f;
			this.noButton.PositionScale_Y = 0.5f;
			this.noButton.SizeOffset_X = 195f;
			this.noButton.SizeOffset_Y = 30f;
			this.noButton.Text = this.localization.format("No_Button");
			this.noButton.TooltipText = this.localization.format("No_Button_Tooltip");
			this.noButton.OnClicked += new ClickedButton(this.onClickedNoButton);
			base.AddChild(this.noButton);
		}

		// Token: 0x04002BFA RID: 11258
		private Local localization;

		// Token: 0x04002BFC RID: 11260
		private ISleekBox textBox;

		// Token: 0x04002BFD RID: 11261
		private ISleekButton yesButton;

		// Token: 0x04002BFE RID: 11262
		private ISleekButton noButton;

		/// <summary>
		/// Nelson 2024-08-19: This link has been checked with WebUtils.CanParseThirdPartyUrl, but is not the
		/// potentially altered link to go through Steam's link filter. This way the UI shows the original link.
		/// </summary>
		// Token: 0x04002BFF RID: 11263
		private string url;
	}
}
