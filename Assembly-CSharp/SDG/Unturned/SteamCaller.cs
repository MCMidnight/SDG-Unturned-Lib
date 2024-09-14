using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000686 RID: 1670
	public class SteamCaller : MonoBehaviour
	{
		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x06003830 RID: 14384 RVA: 0x00109D41 File Offset: 0x00107F41
		public SteamChannel channel
		{
			get
			{
				return this._channel;
			}
		}

		// Token: 0x06003831 RID: 14385 RVA: 0x00109D49 File Offset: 0x00107F49
		private void Awake()
		{
			this._channel = base.GetComponent<SteamChannel>();
		}

		// Token: 0x04002159 RID: 8537
		protected SteamChannel _channel;
	}
}
