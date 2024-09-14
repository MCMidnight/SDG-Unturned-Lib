using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200070C RID: 1804
	public class SleekChatEntryV1 : SleekWrapper
	{
		/// <summary>
		/// Chat message values to show.
		/// </summary>
		// Token: 0x17000B17 RID: 2839
		// (get) Token: 0x06003BB7 RID: 15287 RVA: 0x00117CB4 File Offset: 0x00115EB4
		// (set) Token: 0x06003BB8 RID: 15288 RVA: 0x00117CBC File Offset: 0x00115EBC
		public ReceivedChatMessage representingChatMessage
		{
			get
			{
				return this._representingChatMessage;
			}
			set
			{
				this._representingChatMessage = value;
				if (string.IsNullOrEmpty(this._representingChatMessage.iconURL))
				{
					Texture2D texture;
					if (OptionsSettings.streamer || this._representingChatMessage.speaker == null)
					{
						texture = null;
					}
					else
					{
						texture = Provider.provider.communityService.getIcon(this._representingChatMessage.speaker.playerID.steamID, true);
					}
					this.avatarImage.Texture = texture;
					this.avatarImage.IsVisible = true;
					this.remoteImage.IsVisible = false;
				}
				else
				{
					this.remoteImage.Refresh(this._representingChatMessage.iconURL, true);
					this.avatarImage.IsVisible = false;
					this.remoteImage.IsVisible = true;
				}
				this.contentsLabel.TextColor = this._representingChatMessage.color;
				this.contentsLabel.AllowRichText = this._representingChatMessage.useRichTextFormatting;
				this.contentsLabel.Text = this._representingChatMessage.contents;
			}
		}

		// Token: 0x06003BB9 RID: 15289 RVA: 0x00117DBC File Offset: 0x00115FBC
		public override void OnUpdate()
		{
			if (!this.shouldFadeOutWithAge)
			{
				return;
			}
			float num = this.representingChatMessage.age - Provider.preferenceData.Chat.Fade_Delay;
			num = Mathf.Clamp01(num);
			float a = 1f - num;
			Color color = this.avatarImage.TintColor;
			color.a = a;
			this.avatarImage.TintColor = color;
			this.remoteImage.color = color;
			Color color2 = this.contentsLabel.TextColor;
			color2.a = a;
			this.contentsLabel.TextColor = color2;
		}

		// Token: 0x06003BBA RID: 15290 RVA: 0x00117E68 File Offset: 0x00116068
		public SleekChatEntryV1()
		{
			this.avatarImage = Glazier.Get().CreateImage();
			this.avatarImage.PositionOffset_Y = 4f;
			this.avatarImage.SizeOffset_X = 32f;
			this.avatarImage.SizeOffset_Y = 32f;
			this.avatarImage.IsVisible = false;
			base.AddChild(this.avatarImage);
			this.remoteImage = new SleekWebImage();
			this.remoteImage.PositionOffset_Y = 4f;
			this.remoteImage.SizeOffset_X = 32f;
			this.remoteImage.SizeOffset_Y = 32f;
			this.remoteImage.IsVisible = false;
			base.AddChild(this.remoteImage);
			this.contentsLabel = Glazier.Get().CreateLabel();
			this.contentsLabel.PositionOffset_X = 40f;
			this.contentsLabel.PositionOffset_Y = -4f;
			this.contentsLabel.SizeOffset_X = -40f;
			this.contentsLabel.SizeOffset_Y = 48f;
			this.contentsLabel.SizeScale_X = 1f;
			this.contentsLabel.FontSize = 3;
			this.contentsLabel.TextAlignment = 3;
			this.contentsLabel.TextContrastContext = 2;
			base.AddChild(this.contentsLabel);
		}

		/// <summary>
		/// Does this label fade out as the chat message gets older?
		/// </summary>
		// Token: 0x04002558 RID: 9560
		public bool shouldFadeOutWithAge;

		// Token: 0x04002559 RID: 9561
		protected ReceivedChatMessage _representingChatMessage;

		// Token: 0x0400255A RID: 9562
		private ISleekImage avatarImage;

		// Token: 0x0400255B RID: 9563
		private SleekWebImage remoteImage;

		// Token: 0x0400255C RID: 9564
		private ISleekLabel contentsLabel;
	}
}
