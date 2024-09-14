using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	/// <summary>
	/// Replacement for enum origin.
	/// </summary>
	// Token: 0x0200028A RID: 650
	public class AssetOrigin
	{
		// Token: 0x06001355 RID: 4949 RVA: 0x00046ACE File Offset: 0x00044CCE
		public IReadOnlyList<Asset> GetAssets()
		{
			return this.assets;
		}

		/// <summary>
		/// Hardcoded built-in name, or name of workshop file if known.
		/// </summary>
		// Token: 0x0400068A RID: 1674
		public string name;

		/// <summary>
		/// Steam file ID if loaded from the workshop, zero otherwise.
		/// </summary>
		// Token: 0x0400068B RID: 1675
		public ulong workshopFileId;

		/// <summary>
		/// If true, when added to asset mapping the new assets will override existing ones.
		/// This ensures workshop files installed by servers take priority and disables warnings about overlapping IDs.
		/// </summary>
		// Token: 0x0400068C RID: 1676
		internal bool shouldAssetsOverrideExistingIds;

		// Token: 0x0400068D RID: 1677
		internal List<Asset> assets = new List<Asset>();
	}
}
