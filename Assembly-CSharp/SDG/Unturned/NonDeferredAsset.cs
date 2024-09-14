using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Legacy implementation that preloads assets.
	/// </summary>
	// Token: 0x02000296 RID: 662
	public struct NonDeferredAsset<T> : IDeferredAsset<T> where T : Object
	{
		// Token: 0x060013F6 RID: 5110 RVA: 0x0004A540 File Offset: 0x00048740
		public T getOrLoad()
		{
			return this.loadedObject;
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x0004A548 File Offset: 0x00048748
		public NonDeferredAsset(T loadedObject)
		{
			this.loadedObject = loadedObject;
		}

		// Token: 0x040006E0 RID: 1760
		public T loadedObject;
	}
}
