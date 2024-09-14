using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020005BA RID: 1466
	public class MenuMode : MonoBehaviour
	{
		// Token: 0x06002FA6 RID: 12198 RVA: 0x000D2672 File Offset: 0x000D0872
		public void Awake()
		{
			this.desktop.SetActive(!Dedicator.isVR);
			this.virtualReality.SetActive(Dedicator.isVR);
		}

		// Token: 0x040019B1 RID: 6577
		public GameObject desktop;

		// Token: 0x040019B2 RID: 6578
		public GameObject virtualReality;
	}
}
