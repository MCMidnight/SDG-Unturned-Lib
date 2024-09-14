using System;
using System.Reflection.Emit;

namespace System.Collections.Generic
{
	// Token: 0x02000013 RID: 19
	public static class ListExtension
	{
		// Token: 0x06000044 RID: 68 RVA: 0x000031A4 File Offset: 0x000013A4
		public static void RemoveAtFast<T>(this List<T> list, int index)
		{
			list[index] = list[list.Count - 1];
			list.RemoveAt(list.Count - 1);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000031CC File Offset: 0x000013CC
		public static bool RemoveFast<T>(this List<T> list, T item)
		{
			int num = list.IndexOf(item);
			if (num < 0)
			{
				return false;
			}
			list.RemoveAtFast(num);
			return true;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000031EF File Offset: 0x000013EF
		public static T[] GetInternalArray<T>(this List<T> list)
		{
			return ListExtension.ListInternalArrayAccessor<T>.Getter.Invoke(list);
		}

		// Token: 0x0200082E RID: 2094
		private static class ListInternalArrayAccessor<T>
		{
			// Token: 0x06004761 RID: 18273 RVA: 0x001ADAAC File Offset: 0x001ABCAC
			static ListInternalArrayAccessor()
			{
				DynamicMethod dynamicMethod = new DynamicMethod("get", 22, 1, typeof(T[]), new Type[]
				{
					typeof(List<T>)
				}, typeof(ListExtension.ListInternalArrayAccessor<T>), true);
				ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldfld, typeof(List<T>).GetField("_items", 36));
				ilgenerator.Emit(OpCodes.Ret);
				ListExtension.ListInternalArrayAccessor<T>.Getter = (Func<List<T>, T[]>)dynamicMethod.CreateDelegate(typeof(Func<List<T>, T[]>));
			}

			// Token: 0x04003120 RID: 12576
			public static Func<List<T>, T[]> Getter;
		}
	}
}
