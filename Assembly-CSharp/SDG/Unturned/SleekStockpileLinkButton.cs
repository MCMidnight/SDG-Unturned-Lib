using System;

namespace SDG.Unturned
{
	// Token: 0x02000737 RID: 1847
	public class SleekStockpileLinkButton : SleekWrapper
	{
		// Token: 0x17000B41 RID: 2881
		// (set) Token: 0x06003CC3 RID: 15555 RVA: 0x00121472 File Offset: 0x0011F672
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

		// Token: 0x06003CC4 RID: 15556 RVA: 0x001214A8 File Offset: 0x0011F6A8
		public SleekStockpileLinkButton()
		{
			this.internalButton = Glazier.Get().CreateButton();
			this.internalButton.SizeScale_X = 1f;
			this.internalButton.SizeScale_Y = 1f;
			this.internalButton.OnClicked += new ClickedButton(this.OnClicked);
			base.AddChild(this.internalButton);
		}

		// Token: 0x06003CC5 RID: 15557 RVA: 0x0012150E File Offset: 0x0011F70E
		private void OnClicked(ISleekElement button)
		{
			ItemStore.Get().ViewItem(this.itemdefid);
		}

		// Token: 0x0400260D RID: 9741
		public int itemdefid;

		// Token: 0x0400260E RID: 9742
		internal ISleekButton internalButton;
	}
}
