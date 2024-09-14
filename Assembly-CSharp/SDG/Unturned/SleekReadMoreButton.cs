using System;

namespace SDG.Unturned
{
	// Token: 0x020006FE RID: 1790
	public class SleekReadMoreButton : SleekWrapper
	{
		// Token: 0x06003B3D RID: 15165 RVA: 0x00115117 File Offset: 0x00113317
		public void Refresh()
		{
			this.internalButton.Text = (this.targetContent.IsVisible ? this.offText : this.onText);
		}

		// Token: 0x17000AFA RID: 2810
		// (set) Token: 0x06003B3E RID: 15166 RVA: 0x0011513F File Offset: 0x0011333F
		public override bool UseManualLayout
		{
			set
			{
				base.UseManualLayout = value;
				this.internalButton.UseManualLayout = value;
				this.internalButton.UseChildAutoLayout = (value ? 0 : 2);
				this.internalButton.ExpandChildren = !value;
			}
		}

		// Token: 0x06003B3F RID: 15167 RVA: 0x00115178 File Offset: 0x00113378
		public SleekReadMoreButton()
		{
			this.internalButton = Glazier.Get().CreateButton();
			this.internalButton.SizeScale_X = 1f;
			this.internalButton.SizeScale_Y = 1f;
			this.internalButton.OnClicked += new ClickedButton(this.OnClicked);
			base.AddChild(this.internalButton);
		}

		// Token: 0x06003B40 RID: 15168 RVA: 0x001151DE File Offset: 0x001133DE
		private void OnClicked(ISleekElement button)
		{
			this.targetContent.IsVisible = !this.targetContent.IsVisible;
			this.Refresh();
		}

		// Token: 0x04002528 RID: 9512
		public ISleekElement targetContent;

		// Token: 0x04002529 RID: 9513
		public string onText;

		// Token: 0x0400252A RID: 9514
		public string offText;

		// Token: 0x0400252B RID: 9515
		private ISleekButton internalButton;
	}
}
