using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	/// <summary>
	/// Caches uint16 ID to ID redirects.
	/// </summary>
	// Token: 0x020004DF RID: 1247
	internal class TreeRedirectorMap
	{
		// Token: 0x06002680 RID: 9856 RVA: 0x0009D3FB File Offset: 0x0009B5FB
		public TreeRedirectorMap()
		{
			this.redirectedIds = new Dictionary<Guid, ResourceAsset>();
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x0009D410 File Offset: 0x0009B610
		public ResourceAsset redirect(Guid originalId)
		{
			ResourceAsset resourceAsset;
			if (!this.redirectedIds.TryGetValue(originalId, ref resourceAsset))
			{
				ResourceAsset resourceAsset2 = Assets.find(originalId) as ResourceAsset;
				if (resourceAsset2 != null)
				{
					AssetReference<ResourceAsset> holidayRedirect = resourceAsset2.getHolidayRedirect();
					if (holidayRedirect.isValid)
					{
						resourceAsset = holidayRedirect.Find();
						if (resourceAsset == null && Assets.shouldLoadAnyAssets)
						{
							UnturnedLog.error("Missing holiday redirect for tree {0}", new object[]
							{
								resourceAsset2
							});
						}
					}
					else
					{
						resourceAsset = resourceAsset2;
					}
				}
				this.redirectedIds.Add(originalId, resourceAsset);
			}
			return resourceAsset;
		}

		// Token: 0x040013F2 RID: 5106
		private Dictionary<Guid, ResourceAsset> redirectedIds;
	}
}
