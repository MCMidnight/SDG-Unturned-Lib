using System;
using UnityEngine;
using UnityEngine.Events;
using Unturned.UnityEx;

namespace SDG.Unturned
{
	/// <summary>
	/// Can be added to any GameObject to listen for the Event NPC reward type.
	/// </summary>
	// Token: 0x020005D0 RID: 1488
	[AddComponentMenu("Unturned/NPC Global Event Hook")]
	public class NpcGlobalEventHook : MonoBehaviour
	{
		// Token: 0x06002FFF RID: 12287 RVA: 0x000D4080 File Offset: 0x000D2280
		private void OnEnable()
		{
			if (this.AuthorityOnly && !Provider.isServer)
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(this.EventId))
			{
				UnturnedLog.warn("{0} EventId is empty", new object[]
				{
					base.transform.GetSceneHierarchyPath()
				});
				return;
			}
			NPCEventManager.onEvent += this.OnEvent;
			this.isListening = true;
		}

		// Token: 0x06003000 RID: 12288 RVA: 0x000D40E1 File Offset: 0x000D22E1
		private void OnDisable()
		{
			if (this.isListening)
			{
				NPCEventManager.onEvent -= this.OnEvent;
				this.isListening = false;
			}
		}

		// Token: 0x06003001 RID: 12289 RVA: 0x000D4103 File Offset: 0x000D2303
		private void OnEvent(Player instigatingPlayer, string eventId)
		{
			if (string.Equals(this.EventId, eventId, 5))
			{
				this.OnTriggered.TryInvoke(this);
			}
		}

		/// <summary>
		/// *_ID configured in NPC rewards list.
		/// </summary>
		// Token: 0x040019F6 RID: 6646
		public string EventId;

		/// <summary>
		/// If true the event will only be invoked in offline mode and on the server.
		/// </summary>
		// Token: 0x040019F7 RID: 6647
		public bool AuthorityOnly;

		/// <summary>
		/// Invoked when NPC global event matching EventId is processed.
		/// </summary>
		// Token: 0x040019F8 RID: 6648
		public UnityEvent OnTriggered;

		// Token: 0x040019F9 RID: 6649
		private bool isListening;
	}
}
