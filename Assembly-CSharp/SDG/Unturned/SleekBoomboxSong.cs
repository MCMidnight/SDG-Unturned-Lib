using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007BE RID: 1982
	public class SleekBoomboxSong : SleekWrapper
	{
		// Token: 0x060042AD RID: 17069 RVA: 0x0017079C File Offset: 0x0016E99C
		public SleekBoomboxSong(StereoSongAsset songAsset, PlayerBarricadeStereoUI owningUI)
		{
			this.songAsset = songAsset;
			this.owningUI = owningUI;
			ISleekButton sleekButton = Glazier.Get().CreateButton();
			sleekButton.SizeOffset_Y = 30f;
			sleekButton.SizeScale_X = 1f;
			sleekButton.OnClicked += new ClickedButton(this.OnClickedPlayButton);
			sleekButton.TextColor = 4;
			sleekButton.TextContrastContext = 1;
			sleekButton.AllowRichText = true;
			base.AddChild(sleekButton);
			if (!string.IsNullOrEmpty(songAsset.titleText))
			{
				sleekButton.Text = songAsset.titleText;
			}
			else
			{
				sleekButton.Text = "Sorry, I broke some song names. :( -Nelson";
			}
			if (!string.IsNullOrEmpty(songAsset.linkURL) && WebUtils.CanParseThirdPartyUrl(songAsset.linkURL, true, true))
			{
				sleekButton.SizeOffset_X -= 30f;
				SleekButtonIcon sleekButtonIcon = new SleekButtonIcon(MenuDashboardUI.icons.load<Texture2D>("External_Link"));
				sleekButtonIcon.PositionOffset_X = -30f;
				sleekButtonIcon.PositionScale_X = 1f;
				sleekButtonIcon.SizeOffset_X = 30f;
				sleekButtonIcon.SizeOffset_Y = 30f;
				sleekButtonIcon.tooltip = songAsset.linkURL;
				sleekButtonIcon.onClickedButton += new ClickedButton(this.OnClickedLinkButton);
				base.AddChild(sleekButtonIcon);
			}
		}

		// Token: 0x060042AE RID: 17070 RVA: 0x001708CE File Offset: 0x0016EACE
		private void OnClickedPlayButton(ISleekElement button)
		{
			if (this.owningUI.stereo != null)
			{
				this.owningUI.stereo.ClientSetTrack(this.songAsset.GUID);
			}
		}

		// Token: 0x060042AF RID: 17071 RVA: 0x00170900 File Offset: 0x0016EB00
		private void OnClickedLinkButton(ISleekElement button)
		{
			string text;
			if (WebUtils.ParseThirdPartyUrl(this.songAsset.linkURL, out text, true, true))
			{
				Provider.openURL(this.songAsset.linkURL);
				return;
			}
			UnturnedLog.warn("Ignoring potentially unsafe song link url {0}", new object[]
			{
				this.songAsset.linkURL
			});
		}

		// Token: 0x04002BED RID: 11245
		public StereoSongAsset songAsset;

		// Token: 0x04002BEE RID: 11246
		private PlayerBarricadeStereoUI owningUI;
	}
}
