using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200028F RID: 655
	internal class AssetLoadingStats
	{
		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06001373 RID: 4979 RVA: 0x00046D0D File Offset: 0x00044F0D
		public int RegisteredSearchLocations
		{
			get
			{
				return this.totalRegisteredSearchLocations - this.baselineRegisteredSearchLocations;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06001374 RID: 4980 RVA: 0x00046D1C File Offset: 0x00044F1C
		public int SearchLocationsFinishedSearching
		{
			get
			{
				return this.totalSearchLocationsFinishedSearching - this.baselineSearchLocationsFinishedSearching;
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06001375 RID: 4981 RVA: 0x00046D2B File Offset: 0x00044F2B
		public int AssetBundlesFound
		{
			get
			{
				return this.totalMasterBundlesFound - this.baselineMasterBundlesFound;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06001376 RID: 4982 RVA: 0x00046D3A File Offset: 0x00044F3A
		public int AssetBundlesLoaded
		{
			get
			{
				return this.totalMasterBundlesLoaded - this.baselineMasterBundlesLoaded;
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06001377 RID: 4983 RVA: 0x00046D49 File Offset: 0x00044F49
		public int FilesFound
		{
			get
			{
				return this.totalFilesFound - this.baselineFilesFound;
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06001378 RID: 4984 RVA: 0x00046D58 File Offset: 0x00044F58
		public int FilesRead
		{
			get
			{
				return this.totalFilesRead - this.baselineFilesRead;
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06001379 RID: 4985 RVA: 0x00046D67 File Offset: 0x00044F67
		public int FilesLoaded
		{
			get
			{
				return this.totalFilesLoaded - this.baselineFilesLoaded;
			}
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x00046D78 File Offset: 0x00044F78
		public float EstimateAssetBundleProgressPercentage()
		{
			float num = (this.RegisteredSearchLocations > 1) ? (1f / (float)this.RegisteredSearchLocations) : 1f;
			float num2 = (this.SearchLocationsFinishedSearching > 0) ? ((float)this.SearchLocationsFinishedSearching * num) : num;
			return Mathf.Clamp01(((this.AssetBundlesFound > 1) ? ((float)this.AssetBundlesLoaded / (float)this.AssetBundlesFound) : 0f) * num2);
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x00046DDF File Offset: 0x00044FDF
		public float EstimateSearchProgressPercentage()
		{
			if (this.RegisteredSearchLocations <= 0)
			{
				return 0f;
			}
			return (float)this.SearchLocationsFinishedSearching / (float)this.RegisteredSearchLocations;
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x00046E00 File Offset: 0x00045000
		public float EstimateReadProgressPercentage()
		{
			float num = (this.RegisteredSearchLocations > 1) ? (1f / (float)this.RegisteredSearchLocations) : 1f;
			float num2 = (this.SearchLocationsFinishedSearching > 0) ? ((float)this.SearchLocationsFinishedSearching * num) : num;
			return Mathf.Clamp01(((this.FilesFound > 1) ? ((float)this.FilesRead / (float)this.FilesFound) : 0f) * num2);
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x00046E68 File Offset: 0x00045068
		public float EstimateFileProgressPercentage()
		{
			float num = (this.RegisteredSearchLocations > 1) ? (1f / (float)this.RegisteredSearchLocations) : 1f;
			float num2 = (this.SearchLocationsFinishedSearching > 0) ? ((float)this.SearchLocationsFinishedSearching * num) : num;
			return Mathf.Clamp01(((this.FilesFound > 1) ? ((float)this.FilesLoaded / (float)this.FilesFound) : 0f) * num2);
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x00046ED0 File Offset: 0x000450D0
		public void Reset()
		{
			this.baselineRegisteredSearchLocations = this.totalRegisteredSearchLocations;
			this.baselineSearchLocationsFinishedSearching = this.totalSearchLocationsFinishedSearching;
			this.baselineMasterBundlesFound = this.totalMasterBundlesFound;
			this.baselineMasterBundlesLoaded = this.totalMasterBundlesLoaded;
			this.baselineFilesFound = this.totalFilesFound;
			this.baselineFilesRead = this.totalFilesRead;
			this.baselineFilesLoaded = this.totalFilesLoaded;
		}

		// Token: 0x04000695 RID: 1685
		public bool isLoadingAssetBundles;

		// Token: 0x04000696 RID: 1686
		public int totalRegisteredSearchLocations;

		// Token: 0x04000697 RID: 1687
		public int totalSearchLocationsFinishedSearching;

		// Token: 0x04000698 RID: 1688
		public int totalMasterBundlesFound;

		// Token: 0x04000699 RID: 1689
		public int totalMasterBundlesLoaded;

		// Token: 0x0400069A RID: 1690
		public int totalFilesFound;

		// Token: 0x0400069B RID: 1691
		public int totalFilesRead;

		// Token: 0x0400069C RID: 1692
		public int totalFilesLoaded;

		// Token: 0x0400069D RID: 1693
		private int baselineRegisteredSearchLocations;

		// Token: 0x0400069E RID: 1694
		private int baselineSearchLocationsFinishedSearching;

		// Token: 0x0400069F RID: 1695
		private int baselineMasterBundlesFound;

		// Token: 0x040006A0 RID: 1696
		private int baselineMasterBundlesLoaded;

		// Token: 0x040006A1 RID: 1697
		private int baselineFilesFound;

		// Token: 0x040006A2 RID: 1698
		private int baselineFilesRead;

		// Token: 0x040006A3 RID: 1699
		private int baselineFilesLoaded;
	}
}
