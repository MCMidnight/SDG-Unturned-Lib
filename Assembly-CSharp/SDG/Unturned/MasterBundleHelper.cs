using System;
using System.IO;

namespace SDG.Unturned
{
	// Token: 0x02000315 RID: 789
	public static class MasterBundleHelper
	{
		// Token: 0x060017DA RID: 6106 RVA: 0x00057F61 File Offset: 0x00056161
		public static string getConfigPath(string absoluteDirectory)
		{
			return absoluteDirectory + "/MasterBundle.dat";
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x00057F6E File Offset: 0x0005616E
		public static bool containsMasterBundle(string absoluteDirectory)
		{
			return File.Exists(MasterBundleHelper.getConfigPath(absoluteDirectory));
		}

		/// <summary>
		/// Append suffix to name, or if name contains a '.' insert it before.
		/// </summary>
		// Token: 0x060017DC RID: 6108 RVA: 0x00057F7C File Offset: 0x0005617C
		public static string insertAssetBundleNameSuffix(string name, string suffix)
		{
			int num = name.IndexOf('.');
			if (num < 0)
			{
				return name + suffix;
			}
			return name.Insert(num, suffix);
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x00057FA6 File Offset: 0x000561A6
		public static string getLinuxAssetBundleName(string name)
		{
			return MasterBundleHelper.insertAssetBundleNameSuffix(name, "_linux");
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x00057FB3 File Offset: 0x000561B3
		public static string getMacAssetBundleName(string name)
		{
			return MasterBundleHelper.insertAssetBundleNameSuffix(name, "_mac");
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x00057FC0 File Offset: 0x000561C0
		public static string getHashFileName(string name)
		{
			return name + ".hash";
		}
	}
}
