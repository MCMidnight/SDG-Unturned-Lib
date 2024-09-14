using System;

namespace SDG.Unturned
{
	// Token: 0x020006FC RID: 1788
	public class SleekDismissWorkshopArticleButton : SleekWrapper
	{
		// Token: 0x17000AF9 RID: 2809
		// (set) Token: 0x06003B36 RID: 15158 RVA: 0x00115006 File Offset: 0x00113206
		public override bool UseManualLayout
		{
			set
			{
				base.UseManualLayout = value;
				this.internalButton.UseManualLayout = value;
			}
		}

		// Token: 0x06003B37 RID: 15159 RVA: 0x0011501C File Offset: 0x0011321C
		public SleekDismissWorkshopArticleButton()
		{
			this.internalButton = Glazier.Get().CreateButton();
			this.internalButton.SizeScale_X = 1f;
			this.internalButton.SizeScale_Y = 1f;
			this.internalButton.OnClicked += new ClickedButton(this.OnClicked);
			base.AddChild(this.internalButton);
		}

		// Token: 0x06003B38 RID: 15160 RVA: 0x00115082 File Offset: 0x00113282
		private void OnClicked(ISleekElement button)
		{
			this.targetContent.IsVisible = !this.targetContent.IsVisible;
			LocalNews.dismissWorkshopItem(this.articleId);
		}

		// Token: 0x04002521 RID: 9505
		public ulong articleId;

		// Token: 0x04002522 RID: 9506
		public ISleekElement targetContent;

		// Token: 0x04002523 RID: 9507
		internal ISleekButton internalButton;
	}
}
