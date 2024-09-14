using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	/// <summary>
	/// Caches uint16 ID to ID redirects.
	/// </summary>
	// Token: 0x020004F1 RID: 1265
	internal class LegacyObjectRedirectorMap
	{
		// Token: 0x060027A8 RID: 10152 RVA: 0x000A6890 File Offset: 0x000A4A90
		public LegacyObjectRedirectorMap()
		{
			this.redirectedIds = new Dictionary<Guid, ObjectAsset>();
		}

		// Token: 0x060027A9 RID: 10153 RVA: 0x000A68A4 File Offset: 0x000A4AA4
		public ObjectAsset redirect(Guid originalGUID)
		{
			ObjectAsset objectAsset = null;
			if (!this.redirectedIds.TryGetValue(originalGUID, ref objectAsset))
			{
				ObjectAsset objectAsset2 = Assets.find(originalGUID) as ObjectAsset;
				if (objectAsset2 != null)
				{
					AssetReference<ObjectAsset> holidayRedirect = objectAsset2.getHolidayRedirect();
					if (holidayRedirect.isValid)
					{
						objectAsset = holidayRedirect.Find();
						if (objectAsset == null && Assets.shouldLoadAnyAssets)
						{
							UnturnedLog.error("Missing holiday redirect for object {0}", new object[]
							{
								objectAsset2
							});
						}
					}
					else
					{
						objectAsset = objectAsset2;
					}
				}
				this.redirectedIds.Add(originalGUID, objectAsset);
			}
			return objectAsset;
		}

		// Token: 0x04001503 RID: 5379
		private Dictionary<Guid, ObjectAsset> redirectedIds;
	}
}
