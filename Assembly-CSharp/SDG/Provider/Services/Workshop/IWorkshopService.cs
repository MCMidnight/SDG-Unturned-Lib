using System;
using Steamworks;

namespace SDG.Provider.Services.Workshop
{
	// Token: 0x0200003D RID: 61
	public interface IWorkshopService : IService
	{
		/// <summary>
		/// Whether the user has their overlay enabled.
		/// </summary>
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001C9 RID: 457
		bool canOpenWorkshop { get; }

		// Token: 0x060001CA RID: 458
		void open(PublishedFileId_t id);
	}
}
