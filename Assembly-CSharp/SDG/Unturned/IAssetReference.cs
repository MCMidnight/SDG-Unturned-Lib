using System;

namespace SDG.Unturned
{
	// Token: 0x0200028B RID: 651
	public interface IAssetReference
	{
		/// <summary>
		/// GUID of the asset this is referring to.
		/// </summary>
		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06001357 RID: 4951
		// (set) Token: 0x06001358 RID: 4952
		Guid GUID { get; set; }

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06001359 RID: 4953
		bool isValid { get; }
	}
}
