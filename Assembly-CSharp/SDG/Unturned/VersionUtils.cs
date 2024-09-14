using System;
using System.Globalization;

namespace SDG.Unturned
{
	// Token: 0x02000516 RID: 1302
	public static class VersionUtils
	{
		/// <summary>
		/// Convert 32-bit version into 8-char string.
		/// String is advertised on server list for clients to filter their local map version.
		/// </summary>
		// Token: 0x060028BA RID: 10426 RVA: 0x000AD884 File Offset: 0x000ABA84
		public static string binaryToHexadecimal(uint binaryVersion)
		{
			return binaryVersion.ToString("X8");
		}

		/// <summary>
		/// Parse 32-bit version from 8-char string.
		/// String is advertised on server list for clients to filter their local map version.
		/// </summary>
		// Token: 0x060028BB RID: 10427 RVA: 0x000AD892 File Offset: 0x000ABA92
		public static bool hexadecimalToBinary(string hexadecimalVersion, out uint binaryVersion)
		{
			if (string.IsNullOrEmpty(hexadecimalVersion) || hexadecimalVersion.Length != 8)
			{
				binaryVersion = 0U;
				return false;
			}
			return uint.TryParse(hexadecimalVersion, 515, CultureInfo.CurrentCulture, ref binaryVersion);
		}
	}
}
