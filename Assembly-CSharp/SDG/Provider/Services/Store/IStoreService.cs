using System;

namespace SDG.Provider.Services.Store
{
	// Token: 0x02000040 RID: 64
	public interface IStoreService : IService
	{
		/// <summary>
		/// View a package on the store.
		/// </summary>
		/// <param name="packageID">Package to view.</param>
		// Token: 0x060001CC RID: 460
		void open(IStorePackageID packageID);
	}
}
