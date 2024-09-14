using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200070D RID: 1805
	public class SleekChatEntryV2 : SleekWrapper
	{
		/// <summary>
		/// Chat message values to show.
		/// </summary>
		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x06003BBB RID: 15291 RVA: 0x00117FB6 File Offset: 0x001161B6
		// (set) Token: 0x06003BBC RID: 15292 RVA: 0x00117FC0 File Offset: 0x001161C0
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

		// Token: 0x06003BBD RID: 15293 RVA: 0x001180C0 File Offset: 0x001162C0
		public override void OnUpdate()
		{
			if (!this.shouldFadeOutWithAge)
			{
				return;
			}
			float num = this.representingChatMessage.age - Provider.preferenceData.Chat.Fade_Delay;
			num = Mathf.Clamp01(num);
			float a = 1f - num;
			if (this.forceVisibleWhileBrowsingChatHistory)
			{
				a = 1f;
			}
			Color color = this.avatarImage.TintColor;
			color.a = a;
			this.avatarImage.TintColor = color;
			this.remoteImage.color = color;
			Color color2 = this.contentsLabel.TextColor;
			color2.a = a;
			this.contentsLabel.TextColor = color2;
		}

		// Token: 0x06003BBE RID: 15294 RVA: 0x00118178 File Offset: 0x00116378
		public SleekChatEntryV2()
		{
			this.UseManualLayout = false;
			base.UseChildAutoLayout = 2;
			base.ChildPerpendicularAlignment = 1;
			ISleekElement sleekElement = Glazier.Get().CreateFrame();
			sleekElement.UseManualLayout = false;
			sleekElement.UseWidthLayoutOverride = true;
			sleekElement.UseHeightLayoutOverride = true;
			sleekElement.SizeOffset_X = 40f;
			sleekElement.SizeOffset_Y = 40f;
			base.AddChild(sleekElement);
			this.avatarImage = Glazier.Get().CreateImage();
			this.avatarImage.PositionOffset_X = 4f;
			this.avatarImage.PositionOffset_Y = 4f;
			this.avatarImage.SizeOffset_X = 32f;
			this.avatarImage.SizeOffset_Y = 32f;
			this.avatarImage.IsVisible = false;
			sleekElement.AddChild(this.avatarImage);
			this.remoteImage = new SleekWebImage();
			this.remoteImage.PositionOffset_X = 4f;
			this.remoteImage.PositionOffset_Y = 4f;
			this.remoteImage.SizeOffset_X = 32f;
			this.remoteImage.SizeOffset_Y = 32f;
			this.remoteImage.IsVisible = false;
			sleekElement.AddChild(this.remoteImage);
			ISleekElement sleekElement2 = Glazier.Get().CreateFrame();
			sleekElement2.UseManualLayout = false;
			sleekElement2.UseChildAutoLayout = 1;
			sleekElement2.ExpandChildren = true;
			base.AddChild(sleekElement2);
			this.contentsLabel = Glazier.Get().CreateLabel();
			this.contentsLabel.UseManualLayout = false;
			this.contentsLabel.FontSize = 3;
			this.contentsLabel.TextAlignment = 3;
			this.contentsLabel.TextContrastContext = 2;
			sleekElement2.AddChild(this.contentsLabel);
		}

		/// <summary>
		/// Does this label fade out as the chat message gets older?
		/// </summary>
		// Token: 0x0400255D RID: 9565
		public bool shouldFadeOutWithAge;

		// Token: 0x0400255E RID: 9566
		public bool forceVisibleWhileBrowsingChatHistory;

		// Token: 0x0400255F RID: 9567
		protected ReceivedChatMessage _representingChatMessage;

		// Token: 0x04002560 RID: 9568
		private ISleekImage avatarImage;

		// Token: 0x04002561 RID: 9569
		private SleekWebImage remoteImage;

		// Token: 0x04002562 RID: 9570
		private ISleekLabel contentsLabel;
	}
}
