using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Allows Unity events to broadcast Event NPC rewards.
	/// </summary>
	// Token: 0x020005D1 RID: 1489
	[AddComponentMenu("Unturned/NPC Global Event Messenger")]
	public class NpcGlobalEventMessenger : MonoBehaviour
	{
		// Token: 0x06003003 RID: 12291 RVA: 0x000D4128 File Offset: 0x000D2328
		public void SendEventId(string eventId)
		{
			if (Provider.isServer && !string.IsNullOrEmpty(eventId))
			{
				NPCEventManager.broadcastEvent(null, eventId, this.ShouldReplicate);
			}
		}

		// Token: 0x06003004 RID: 12292 RVA: 0x000D4146 File Offset: 0x000D2346
		public void SendDefaultEventId()
		{
			this.SendEventId(this.DefaultEventId);
		}

		/// <summary>
		/// Event ID to use when SendDefaultEventId is invoked.
		/// </summary>
		// Token: 0x040019FA RID: 6650
		public string DefaultEventId;

		/// <summary>
		/// The event messenger can only be triggered on the authority (server).
		/// If true, the server will replicate the event to clients.
		/// </summary>
		// Token: 0x040019FB RID: 6651
		public bool ShouldReplicate;
	}
}
