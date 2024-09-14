using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000677 RID: 1655
	public interface ILocalWorkshopSettings
	{
		// Token: 0x060036F0 RID: 14064
		bool getEnabled(PublishedFileId_t fileId);

		// Token: 0x060036F1 RID: 14065
		void setEnabled(PublishedFileId_t fileId, bool newEnabled);
	}
}
