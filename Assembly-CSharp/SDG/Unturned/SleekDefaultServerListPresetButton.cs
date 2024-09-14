using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000712 RID: 1810
	public class SleekDefaultServerListPresetButton : SleekWrapper
	{
		// Token: 0x06003BDC RID: 15324 RVA: 0x00119114 File Offset: 0x00117314
		public SleekDefaultServerListPresetButton(ServerListFilters preset, Local localization, Bundle icons)
		{
			this.preset = preset;
			Texture2D newIcon;
			string text;
			string tooltip;
			if (preset.presetId == FilterSettings.defaultPresetInternet.presetId)
			{
				newIcon = icons.load<Texture2D>("List_Internet");
				text = localization.format("DefaultPreset_Internet_Label");
				tooltip = localization.format("List_Internet_Tooltip");
			}
			else if (preset.presetId == FilterSettings.defaultPresetLAN.presetId)
			{
				newIcon = icons.load<Texture2D>("List_LAN");
				text = localization.format("DefaultPreset_LAN_Label");
				tooltip = localization.format("List_LAN_Tooltip");
			}
			else if (preset.presetId == FilterSettings.defaultPresetHistory.presetId)
			{
				newIcon = icons.load<Texture2D>("List_History");
				text = localization.format("DefaultPreset_History_Label");
				tooltip = localization.format("List_History_Tooltip");
			}
			else if (preset.presetId == FilterSettings.defaultPresetFavorites.presetId)
			{
				newIcon = icons.load<Texture2D>("List_Favorites");
				text = localization.format("DefaultPreset_Favorites_Label");
				tooltip = localization.format("List_Favorites_Tooltip");
			}
			else if (preset.presetId == FilterSettings.defaultPresetFriends.presetId)
			{
				newIcon = icons.load<Texture2D>("List_Friends");
				text = localization.format("DefaultPreset_Friends_Label");
				tooltip = localization.format("List_Friends_Tooltip");
			}
			else
			{
				newIcon = null;
				text = string.Format("unknown preset ({0})", preset.presetId);
				tooltip = text;
			}
			this.internalButton = new SleekButtonIcon(newIcon, 20);
			this.internalButton.SizeScale_X = 1f;
			this.internalButton.SizeScale_Y = 1f;
			this.internalButton.text = text;
			this.internalButton.tooltip = tooltip;
			this.internalButton.onClickedButton += new ClickedButton(this.OnClicked);
			this.internalButton.iconColor = 2;
			base.AddChild(this.internalButton);
		}

		// Token: 0x06003BDD RID: 15325 RVA: 0x001192E4 File Offset: 0x001174E4
		private void OnClicked(ISleekElement button)
		{
			FilterSettings.activeFilters.CopyFrom(this.preset);
			if (this.preset.presetId == FilterSettings.defaultPresetInternet.presetId)
			{
				FilterSettings.activeFilters.presetName = MenuPlayUI.serverListUI.localization.format("DefaultPreset_Internet_Label");
			}
			else if (this.preset.presetId == FilterSettings.defaultPresetLAN.presetId)
			{
				FilterSettings.activeFilters.presetName = MenuPlayUI.serverListUI.localization.format("DefaultPreset_LAN_Label");
			}
			else if (this.preset.presetId == FilterSettings.defaultPresetHistory.presetId)
			{
				FilterSettings.activeFilters.presetName = MenuPlayUI.serverListUI.localization.format("DefaultPreset_History_Label");
			}
			else if (this.preset.presetId == FilterSettings.defaultPresetFavorites.presetId)
			{
				FilterSettings.activeFilters.presetName = MenuPlayUI.serverListUI.localization.format("DefaultPreset_Favorites_Label");
			}
			else if (this.preset.presetId == FilterSettings.defaultPresetFriends.presetId)
			{
				FilterSettings.activeFilters.presetName = MenuPlayUI.serverListUI.localization.format("DefaultPreset_Friends_Label");
			}
			else
			{
				FilterSettings.activeFilters.presetName = "unknown default preset";
			}
			FilterSettings.InvokeActiveFiltersReplaced();
		}

		// Token: 0x04002576 RID: 9590
		private SleekButtonIcon internalButton;

		// Token: 0x04002577 RID: 9591
		private ServerListFilters preset;
	}
}
