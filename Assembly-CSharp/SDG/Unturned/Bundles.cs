using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000298 RID: 664
	public class Bundles : MonoBehaviour
	{
		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x0600140E RID: 5134 RVA: 0x0004A993 File Offset: 0x00048B93
		public static bool isInitialized
		{
			get
			{
				return Bundles._isInitialized;
			}
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x0004A99A File Offset: 0x00048B9A
		public static Bundle getBundle(string path)
		{
			return Bundles.getBundle(path, true);
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x0004A9A3 File Offset: 0x00048BA3
		public static Bundle getBundle(string path, bool prependRoot)
		{
			return new Bundle(path, prependRoot, null);
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x0004A9AD File Offset: 0x00048BAD
		[Obsolete]
		public static Bundle getBundle(string path, bool prependRoot, bool loadFromResources)
		{
			return Bundles.getBundle(path, prependRoot);
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x0004A9B6 File Offset: 0x00048BB6
		private void Awake()
		{
			if (Bundles.isInitialized)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			Bundles._isInitialized = true;
			Object.DontDestroyOnLoad(base.gameObject);
		}

		// Token: 0x040006E8 RID: 1768
		private static bool _isInitialized;
	}
}
