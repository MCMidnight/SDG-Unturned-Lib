using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000807 RID: 2055
	public static class ListExtension
	{
		/// <summary>
		/// Get index within bounds assuming list is not empty.
		/// </summary>
		// Token: 0x0600465B RID: 18011 RVA: 0x001A3D08 File Offset: 0x001A1F08
		public static int GetRandomIndex<T>(this List<T> list)
		{
			return Random.Range(0, list.Count);
		}

		// Token: 0x0600465C RID: 18012 RVA: 0x001A3D18 File Offset: 0x001A1F18
		public static T RandomOrDefault<T>(this List<T> list)
		{
			if (list.Count > 0)
			{
				return list[Random.Range(0, list.Count)];
			}
			return default(T);
		}

		/// <summary>
		/// Add a new item using its default constructor.
		/// </summary>
		// Token: 0x0600465D RID: 18013 RVA: 0x001A3D4C File Offset: 0x001A1F4C
		public static T AddDefaulted<T>(this List<T> list) where T : class, new()
		{
			T t = Activator.CreateInstance<T>();
			list.Add(t);
			return t;
		}

		// Token: 0x0600465E RID: 18014 RVA: 0x001A3D67 File Offset: 0x001A1F67
		public static bool IsEmpty<T>(this List<T> list)
		{
			return list.Count < 1;
		}

		// Token: 0x0600465F RID: 18015 RVA: 0x001A3D74 File Offset: 0x001A1F74
		public static T HeadOrDefault<T>(this List<T> list)
		{
			if (list.Count > 0)
			{
				return list[0];
			}
			return default(T);
		}

		// Token: 0x06004660 RID: 18016 RVA: 0x001A3D9C File Offset: 0x001A1F9C
		public static T TailOrDefault<T>(this List<T> list)
		{
			if (list.Count > 0)
			{
				return list[list.Count - 1];
			}
			return default(T);
		}

		// Token: 0x06004661 RID: 18017 RVA: 0x001A3DCA File Offset: 0x001A1FCA
		public static T GetTail<T>(this List<T> list)
		{
			return list[list.Count - 1];
		}

		// Token: 0x06004662 RID: 18018 RVA: 0x001A3DDC File Offset: 0x001A1FDC
		public static T GetAndRemoveTail<T>(this List<T> list)
		{
			int num = list.Count - 1;
			T result = list[num];
			list.RemoveAt(num);
			return result;
		}

		// Token: 0x06004663 RID: 18019 RVA: 0x001A3E00 File Offset: 0x001A2000
		public static void RemoveTail<T>(this List<T> list)
		{
			list.RemoveAt(list.Count - 1);
		}

		// Token: 0x06004664 RID: 18020 RVA: 0x001A3E10 File Offset: 0x001A2010
		internal static int FindInsertionIndex<T>(this List<T> list, T item)
		{
			int num = list.BinarySearch(item);
			if (num < 0)
			{
				num = ~num;
			}
			return num;
		}

		// Token: 0x06004665 RID: 18021 RVA: 0x001A3E30 File Offset: 0x001A2030
		internal static int FindInsertionIndex<T>(this List<T> list, T item, IComparer<T> comparer)
		{
			int num = list.BinarySearch(item, comparer);
			if (num < 0)
			{
				num = ~num;
			}
			return num;
		}
	}
}
