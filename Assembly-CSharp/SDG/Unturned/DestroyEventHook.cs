using System;
using UnityEngine;
using UnityEngine.Events;

namespace SDG.Unturned
{
	// Token: 0x020005C7 RID: 1479
	[AddComponentMenu("Unturned/Destroy Event Hook")]
	public class DestroyEventHook : MonoBehaviour
	{
		// Token: 0x06002FE3 RID: 12259 RVA: 0x000D3B82 File Offset: 0x000D1D82
		private void OnDestroy()
		{
			if (this.AuthorityOnly && !Provider.isServer)
			{
				return;
			}
			this.OnDestroyed.Invoke();
		}

		/// <summary>
		/// If true the event will only be invoked in offline mode and on the server.
		/// </summary>
		// Token: 0x040019E0 RID: 6624
		public bool AuthorityOnly;

		// Token: 0x040019E1 RID: 6625
		public UnityEvent OnDestroyed;
	}
}
