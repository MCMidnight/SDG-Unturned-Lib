using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Parses mb:X from input string and filters assets using X master bundle.
	/// </summary>
	// Token: 0x02000317 RID: 791
	public struct MasterBundleSearchFilter
	{
		// Token: 0x060017EA RID: 6122 RVA: 0x000582E4 File Offset: 0x000564E4
		public static MasterBundleSearchFilter? parse(string filter)
		{
			string name;
			if (!SearchFilterUtil.parseKeyValue(filter, "mb:", out name))
			{
				return default(MasterBundleSearchFilter?);
			}
			MasterBundleConfig masterBundleConfig = Assets.findMasterBundleByName(name, false);
			if (masterBundleConfig == null)
			{
				return default(MasterBundleSearchFilter?);
			}
			return new MasterBundleSearchFilter?(new MasterBundleSearchFilter(masterBundleConfig));
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x0005832A File Offset: 0x0005652A
		public bool ignores(Asset asset)
		{
			return asset == null || asset.originMasterBundle != this.masterBundle;
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x00058342 File Offset: 0x00056542
		public bool matches(Asset asset)
		{
			return !this.ignores(asset);
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x0005834E File Offset: 0x0005654E
		public MasterBundleSearchFilter(MasterBundleConfig masterBundle)
		{
			this.masterBundle = masterBundle;
		}

		// Token: 0x04000AD3 RID: 2771
		private MasterBundleConfig masterBundle;
	}
}
