using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000735 RID: 1845
	public class SleekWorkshopSubscriptionButton : SleekWrapper
	{
		// Token: 0x17000B40 RID: 2880
		// (set) Token: 0x06003CBC RID: 15548 RVA: 0x001212E3 File Offset: 0x0011F4E3
		public override bool UseManualLayout
		{
			set
			{
				base.UseManualLayout = value;
				this.button.UseManualLayout = value;
				this.button.UseChildAutoLayout = (value ? 0 : 2);
				this.button.ExpandChildren = !value;
			}
		}

		// Token: 0x06003CBD RID: 15549 RVA: 0x0012131C File Offset: 0x0011F51C
		public void synchronizeText()
		{
			bool subscribed = Provider.provider.workshopService.getSubscribed(this.fileId.m_PublishedFileId);
			this.button.Text = (subscribed ? this.unsubscribeText : this.subscribeText);
			this.button.TooltipText = (subscribed ? this.unsubscribeTooltip : this.subscribeTooltip);
		}

		// Token: 0x06003CBE RID: 15550 RVA: 0x0012137C File Offset: 0x0011F57C
		protected void handleClickedButton(ISleekElement thisButton)
		{
			bool subscribe = !Provider.provider.workshopService.getSubscribed(this.fileId.m_PublishedFileId);
			Provider.provider.workshopService.setSubscribed(this.fileId.m_PublishedFileId, subscribe);
			this.synchronizeText();
		}

		// Token: 0x06003CBF RID: 15551 RVA: 0x001213C8 File Offset: 0x0011F5C8
		public SleekWorkshopSubscriptionButton()
		{
			this.button = Glazier.Get().CreateButton();
			this.button.SizeScale_X = 1f;
			this.button.SizeScale_Y = 1f;
			this.button.TextAlignment = 4;
			this.button.OnClicked += new ClickedButton(this.handleClickedButton);
			base.AddChild(this.button);
		}

		// Token: 0x04002605 RID: 9733
		public PublishedFileId_t fileId;

		// Token: 0x04002606 RID: 9734
		public string subscribeText;

		// Token: 0x04002607 RID: 9735
		public string unsubscribeText;

		// Token: 0x04002608 RID: 9736
		public string subscribeTooltip;

		// Token: 0x04002609 RID: 9737
		public string unsubscribeTooltip;

		// Token: 0x0400260A RID: 9738
		private ISleekButton button;
	}
}
