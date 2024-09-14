using System;

namespace SDG.Unturned
{
	// Token: 0x020002B0 RID: 688
	public class EditorObjectSearchFilter
	{
		// Token: 0x060014A8 RID: 5288 RVA: 0x0004CD32 File Offset: 0x0004AF32
		public static EditorObjectSearchFilter parse(string filter)
		{
			if (string.IsNullOrEmpty(filter))
			{
				return null;
			}
			return new EditorObjectSearchFilter(filter);
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x0004CD44 File Offset: 0x0004AF44
		public bool matches(ObjectAsset objectAsset)
		{
			if (this.mbFilter != null && this.mbFilter.Value.ignores(objectAsset))
			{
				return false;
			}
			if (this.tokenFilter != null && this.tokenFilter.Value.ignores(objectAsset.objectName))
			{
				return false;
			}
			if (this.fvFilter != null)
			{
				EditorObjectSearchFilter[] subFilters = this.fvFilter.Value.subFilters;
				for (int i = 0; i < subFilters.Length; i++)
				{
					if (subFilters[i].matches(objectAsset))
					{
						return true;
					}
				}
				return false;
			}
			return true;
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x0004CDE0 File Offset: 0x0004AFE0
		public bool ignores(ObjectAsset objectAsset)
		{
			return !this.matches(objectAsset);
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x0004CDEC File Offset: 0x0004AFEC
		public bool matches(ItemAsset itemAsset)
		{
			if (this.mbFilter != null && this.mbFilter.Value.ignores(itemAsset))
			{
				return false;
			}
			if (this.tokenFilter != null && this.tokenFilter.Value.ignores(itemAsset.itemName))
			{
				return false;
			}
			if (this.fvFilter != null)
			{
				EditorObjectSearchFilter[] subFilters = this.fvFilter.Value.subFilters;
				for (int i = 0; i < subFilters.Length; i++)
				{
					if (subFilters[i].matches(itemAsset))
					{
						return true;
					}
				}
				return false;
			}
			return true;
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x0004CE88 File Offset: 0x0004B088
		public bool ignores(ItemAsset itemAsset)
		{
			return !this.matches(itemAsset);
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x0004CE94 File Offset: 0x0004B094
		private EditorObjectSearchFilter(string filter)
		{
			this.tokenFilter = TokenSearchFilter.parse(filter);
			this.mbFilter = MasterBundleSearchFilter.parse(filter);
			this.fvFilter = FavoriteSearchFilter<EditorObjectSearchFilter>.parse(filter, new FavoriteSearchFilter<EditorObjectSearchFilter>.SubFilterParser(this.parseFavoriteSubFilter));
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x0004CECC File Offset: 0x0004B0CC
		private bool parseFavoriteSubFilter(string filter, out EditorObjectSearchFilter subFilter)
		{
			subFilter = EditorObjectSearchFilter.parse(filter);
			return subFilter != null;
		}

		// Token: 0x04000765 RID: 1893
		private TokenSearchFilter? tokenFilter;

		// Token: 0x04000766 RID: 1894
		private MasterBundleSearchFilter? mbFilter;

		// Token: 0x04000767 RID: 1895
		private FavoriteSearchFilter<EditorObjectSearchFilter>? fvFilter;
	}
}
