using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000820 RID: 2080
	public static class TransformRecursiveFind
	{
		// Token: 0x060046FC RID: 18172 RVA: 0x001A7FD8 File Offset: 0x001A61D8
		public static Transform FindChildRecursive(this Transform parent, string name)
		{
			int childCount = parent.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform transform = parent.GetChild(i);
				if (transform.name == name)
				{
					return transform;
				}
				if (transform.childCount != 0)
				{
					transform = transform.FindChildRecursive(name);
					if (transform != null)
					{
						return transform;
					}
				}
			}
			return null;
		}
	}
}
