using System;
using System.Collections.Generic;
using System.IO;

namespace SDG.Unturned
{
	/// <summary>
	/// Parses fv:X from input string and loads X.txt from game folder.
	/// </summary>
	// Token: 0x020002C8 RID: 712
	public struct FavoriteSearchFilter<T>
	{
		// Token: 0x060014C9 RID: 5321 RVA: 0x0004D2AC File Offset: 0x0004B4AC
		public static FavoriteSearchFilter<T>? parse(string filter, FavoriteSearchFilter<T>.SubFilterParser parseSubFilter)
		{
			string text;
			if (!SearchFilterUtil.parseKeyValue(filter, "fv:", out text))
			{
				return default(FavoriteSearchFilter<T>?);
			}
			string text2 = Path.Combine(ReadWrite.PATH, text) + ".txt";
			if (!File.Exists(text2))
			{
				return default(FavoriteSearchFilter<T>?);
			}
			List<T> list = new List<T>();
			foreach (string text3 in File.ReadAllLines(text2))
			{
				T t;
				if (!string.IsNullOrWhiteSpace(text3) && !text3.StartsWith("//") && parseSubFilter(text3, out t))
				{
					list.Add(t);
				}
			}
			if (list.Count < 1)
			{
				return default(FavoriteSearchFilter<T>?);
			}
			return new FavoriteSearchFilter<T>?(new FavoriteSearchFilter<T>(list.ToArray()));
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x0004D370 File Offset: 0x0004B570
		public FavoriteSearchFilter(T[] subFilters)
		{
			this.subFilters = subFilters;
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x060014CB RID: 5323 RVA: 0x0004D379 File Offset: 0x0004B579
		// (set) Token: 0x060014CC RID: 5324 RVA: 0x0004D381 File Offset: 0x0004B581
		public T[] subFilters { readonly get; private set; }

		// Token: 0x0200091C RID: 2332
		// (Invoke) Token: 0x06004A76 RID: 19062
		public delegate bool SubFilterParser(string input, out T subFilter);
	}
}
