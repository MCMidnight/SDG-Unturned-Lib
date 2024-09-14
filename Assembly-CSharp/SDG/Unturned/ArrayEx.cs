using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007F2 RID: 2034
	public static class ArrayEx
	{
		// Token: 0x060045FA RID: 17914 RVA: 0x001A236F File Offset: 0x001A056F
		public static int GetRandomIndex<T>(this T[] array)
		{
			return Random.Range(0, array.Length);
		}

		// Token: 0x060045FB RID: 17915 RVA: 0x001A237C File Offset: 0x001A057C
		public static T RandomOrDefault<T>(this T[] array)
		{
			if (array.Length != 0)
			{
				return array[Random.Range(0, array.Length)];
			}
			return default(T);
		}
	}
}
