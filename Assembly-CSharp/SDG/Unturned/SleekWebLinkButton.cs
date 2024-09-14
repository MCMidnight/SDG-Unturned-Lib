using System;

namespace SDG.Unturned
{
	// Token: 0x02000734 RID: 1844
	public class SleekWebLinkButton : SleekWrapper
	{
		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x06003CB5 RID: 15541 RVA: 0x00121128 File Offset: 0x0011F328
		// (set) Token: 0x06003CB6 RID: 15542 RVA: 0x00121135 File Offset: 0x0011F335
		public string Text
		{
			get
			{
				return this.internalButton.Text;
			}
			set
			{
				this.internalButton.Text = value;
			}
		}

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x06003CB7 RID: 15543 RVA: 0x00121143 File Offset: 0x0011F343
		// (set) Token: 0x06003CB8 RID: 15544 RVA: 0x0012114B File Offset: 0x0011F34B
		public string Url
		{
			get
			{
				return this._url;
			}
			set
			{
				this._url = value;
				this.internalButton.TooltipText = this._url;
			}
		}

		// Token: 0x17000B3F RID: 2879
		// (set) Token: 0x06003CB9 RID: 15545 RVA: 0x00121165 File Offset: 0x0011F365
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

		// Token: 0x06003CBA RID: 15546 RVA: 0x0012119C File Offset: 0x0011F39C
		public SleekWebLinkButton()
		{
			this.internalButton = Glazier.Get().CreateButton();
			this.internalButton.SizeScale_X = 1f;
			this.internalButton.SizeScale_Y = 1f;
			this.internalButton.OnClicked += new ClickedButton(this.OnClicked);
			base.AddChild(this.internalButton);
		}

		// Token: 0x06003CBB RID: 15547 RVA: 0x0012120C File Offset: 0x0011F40C
		private void OnClicked(ISleekElement button)
		{
			int num = this._url.IndexOf("store.steampowered.com/itemstore/304930/detail/", 5);
			if (num >= 0)
			{
				int num2 = num + "store.steampowered.com/itemstore/304930/detail/".Length;
				int num3 = this._url.IndexOf('/', num2 + 1);
				string text;
				if (num3 >= 0)
				{
					text = this._url.Substring(num2, num3 - num2);
				}
				else
				{
					text = this._url.Substring(num2);
				}
				int num4;
				if (int.TryParse(text, ref num4))
				{
					UnturnedLog.info(string.Format("Parsed itemdefid {0} from web link url \"{1}\"", num4, this._url));
					ItemStoreSavedata.MarkNewListingSeen(num4);
					ItemStore.Get().ViewItem(num4);
					return;
				}
			}
			string url;
			if (WebUtils.ParseThirdPartyUrl(this._url, out url, true, this.useLinkFiltering))
			{
				Provider.openURL(url);
				return;
			}
			UnturnedLog.warn("Ignoring potentially unsafe web link button url {0}", new object[]
			{
				this._url
			});
		}

		// Token: 0x04002602 RID: 9730
		private string _url;

		// Token: 0x04002603 RID: 9731
		public bool useLinkFiltering = true;

		// Token: 0x04002604 RID: 9732
		private ISleekButton internalButton;
	}
}
