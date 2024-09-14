using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000312 RID: 786
	public struct DeferredMasterAsset<T> : IDeferredAsset<T> where T : Object
	{
		// Token: 0x060017B7 RID: 6071 RVA: 0x00057798 File Offset: 0x00055998
		public T getOrLoad()
		{
			if (!this.hasLoaded)
			{
				this.hasLoaded = true;
				this.loadedObject = this.masterBundle.load<T>(this.name);
				LoadedAssetDeferredCallback<T> loadedAssetDeferredCallback = this.callback;
				if (loadedAssetDeferredCallback != null)
				{
					loadedAssetDeferredCallback(this.loadedObject);
				}
			}
			return this.loadedObject;
		}

		// Token: 0x04000ABB RID: 2747
		public MasterBundle masterBundle;

		// Token: 0x04000ABC RID: 2748
		public string name;

		// Token: 0x04000ABD RID: 2749
		public LoadedAssetDeferredCallback<T> callback;

		// Token: 0x04000ABE RID: 2750
		public T loadedObject;

		// Token: 0x04000ABF RID: 2751
		public bool hasLoaded;
	}
}
