using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	/// <summary>
	/// Splits string and compares substrings ignoring case.
	/// Tokens containing a colon ':' are ignored so that they can represent special filters like MasterBundleSearchFilter.
	/// </summary>
	// Token: 0x0200036E RID: 878
	public struct TokenSearchFilter
	{
		// Token: 0x06001A90 RID: 6800 RVA: 0x0005FFC4 File Offset: 0x0005E1C4
		public static TokenSearchFilter? parse(string filter)
		{
			if (string.IsNullOrEmpty(filter))
			{
				return default(TokenSearchFilter?);
			}
			TokenSearchFilter.workingTokens.Clear();
			int num;
			for (int i = 0; i < filter.Length; i = num + 1)
			{
				num = filter.IndexOf(' ', i);
				int num2 = filter.IndexOf(':', i);
				if (num < 0)
				{
					num = filter.Length;
				}
				if ((num2 < 0 || num2 >= num) && num - i > 0)
				{
					string text = filter.Substring(i, num - i);
					TokenSearchFilter.workingTokens.Add(text);
				}
			}
			if (TokenSearchFilter.workingTokens.Count < 1)
			{
				return default(TokenSearchFilter?);
			}
			return new TokenSearchFilter?(new TokenSearchFilter(TokenSearchFilter.workingTokens.ToArray()));
		}

		// Token: 0x06001A91 RID: 6801 RVA: 0x00060074 File Offset: 0x0005E274
		public bool ignores(string name)
		{
			foreach (string text in this.tokens)
			{
				if (name.IndexOf(text, 3) < 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x000600A8 File Offset: 0x0005E2A8
		public bool matches(string name)
		{
			return !this.ignores(name);
		}

		// Token: 0x06001A93 RID: 6803 RVA: 0x000600B4 File Offset: 0x0005E2B4
		public TokenSearchFilter(string[] tokens)
		{
			this.tokens = tokens;
		}

		// Token: 0x04000C42 RID: 3138
		private string[] tokens;

		// Token: 0x04000C43 RID: 3139
		private static List<string> workingTokens = new List<string>();
	}
}
