using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200057F RID: 1407
	public class Managers : MonoBehaviour
	{
		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06002D03 RID: 11523 RVA: 0x000C3428 File Offset: 0x000C1628
		public static bool isInitialized
		{
			get
			{
				return Managers._isInitialized;
			}
		}

		// Token: 0x06002D04 RID: 11524 RVA: 0x000C342F File Offset: 0x000C162F
		private void Awake()
		{
			if (Managers.isInitialized)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			Managers._isInitialized = true;
			Object.DontDestroyOnLoad(base.gameObject);
			base.GetComponent<SteamChannel>().setup();
		}

		// Token: 0x04001852 RID: 6226
		private static bool _isInitialized;
	}
}
