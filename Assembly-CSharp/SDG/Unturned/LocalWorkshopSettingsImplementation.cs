using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000679 RID: 1657
	internal class LocalWorkshopSettingsImplementation : ILocalWorkshopSettings
	{
		// Token: 0x060036F4 RID: 14068 RVA: 0x00101730 File Offset: 0x000FF930
		public bool getEnabled(PublishedFileId_t fileId)
		{
			string key = this.formatEnabledKey(fileId);
			bool flag;
			return !ConvenientSavedata.get().read(key, out flag) || flag;
		}

		// Token: 0x060036F5 RID: 14069 RVA: 0x00101758 File Offset: 0x000FF958
		public void setEnabled(PublishedFileId_t fileId, bool newEnabled)
		{
			string key = this.formatEnabledKey(fileId);
			ConvenientSavedata.get().write(key, newEnabled);
		}

		// Token: 0x060036F6 RID: 14070 RVA: 0x0010177C File Offset: 0x000FF97C
		private string formatEnabledKey(PublishedFileId_t fileId)
		{
			string text = "Enabled_Workshop_Item_";
			PublishedFileId_t publishedFileId_t = fileId;
			return text + publishedFileId_t.ToString();
		}
	}
}
