using System;

namespace SDG.Unturned
{
	// Token: 0x02000363 RID: 867
	public static class SearchFilterUtil
	{
		// Token: 0x06001A3B RID: 6715 RVA: 0x0005E450 File Offset: 0x0005C650
		public static bool parseKeyValue(string filter, string key, out string value)
		{
			value = null;
			if (string.IsNullOrEmpty(filter))
			{
				return false;
			}
			int num = filter.IndexOf(key, 3);
			if (num < 0)
			{
				return false;
			}
			int num2 = num + key.Length;
			int num3 = filter.IndexOf(' ', num2);
			if (num3 < 0)
			{
				value = filter.Substring(num2);
			}
			else
			{
				value = filter.Substring(num2, num3 - num2);
			}
			return !string.IsNullOrEmpty(value);
		}
	}
}
