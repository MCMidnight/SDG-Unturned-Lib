using System;

namespace SDG.Unturned
{
	// Token: 0x02000711 RID: 1809
	public class SleekCustomServerListPresetButton : SleekWrapper
	{
		// Token: 0x06003BDA RID: 15322 RVA: 0x0011907C File Offset: 0x0011727C
		public SleekCustomServerListPresetButton(ServerListFilters preset)
		{
			this.preset = preset;
			this.internalButton = Glazier.Get().CreateButton();
			this.internalButton.SizeScale_X = 1f;
			this.internalButton.SizeScale_Y = 1f;
			this.internalButton.Text = preset.presetName;
			this.internalButton.OnClicked += new ClickedButton(this.OnClicked);
			base.AddChild(this.internalButton);
		}

		// Token: 0x06003BDB RID: 15323 RVA: 0x001190FA File Offset: 0x001172FA
		private void OnClicked(ISleekElement button)
		{
			FilterSettings.activeFilters.CopyFrom(this.preset);
			FilterSettings.InvokeActiveFiltersReplaced();
		}

		// Token: 0x04002574 RID: 9588
		private ISleekButton internalButton;

		// Token: 0x04002575 RID: 9589
		private ServerListFilters preset;
	}
}
