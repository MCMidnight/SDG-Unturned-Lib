using System;

namespace SDG.Provider.Services.Browser
{
	// Token: 0x02000066 RID: 102
	public interface IBrowserService : IService
	{
		/// <summary>
		/// Whether the user has their overlay enabled.
		/// </summary>
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000241 RID: 577
		bool canOpenBrowser { get; }

		// Token: 0x06000242 RID: 578
		void open(string url);
	}
}
