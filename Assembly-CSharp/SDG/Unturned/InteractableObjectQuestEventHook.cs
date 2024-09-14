using System;
using SDG.NetTransport;
using UnityEngine;
using UnityEngine.Events;
using Unturned.UnityEx;

namespace SDG.Unturned
{
	/// <summary>
	/// Can be added to any GameObject with a Dropper, Note, or Quest interactable object in its parents.
	/// </summary>
	// Token: 0x020005CC RID: 1484
	[AddComponentMenu("Unturned/Interactable Object Quest Event Hook")]
	public class InteractableObjectQuestEventHook : MonoBehaviour
	{
		// Token: 0x06002FF1 RID: 12273 RVA: 0x000D3E74 File Offset: 0x000D2074
		protected void Start()
		{
			this.interactable = base.gameObject.GetComponentInParent<InteractableObjectTriggerableBase>();
			if (this.interactable == null)
			{
				UnturnedLog.warn("InteractableObjectQuestEventHook {0} unable to find interactable", new object[]
				{
					this.GetSceneHierarchyPath()
				});
				return;
			}
			this.interactable.OnUsedForModHooks += new Action(this.OnUsedInternal);
		}

		// Token: 0x06002FF2 RID: 12274 RVA: 0x000D3ED1 File Offset: 0x000D20D1
		protected void OnDestroy()
		{
			if (this.interactable != null)
			{
				this.interactable.OnUsedForModHooks -= new Action(this.OnUsedInternal);
				this.interactable = null;
			}
		}

		// Token: 0x06002FF3 RID: 12275 RVA: 0x000D3F00 File Offset: 0x000D2100
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveUsedNotification(Transform eventHookTransform)
		{
			if (eventHookTransform == null)
			{
				UnturnedLog.info("Received InteractableObjectQuestEventHook.OnUsed event from server, but matching transform doesn't exist on client! Server prefab is likely different from client prefab.");
				return;
			}
			InteractableObjectQuestEventHook component = eventHookTransform.GetComponent<InteractableObjectQuestEventHook>();
			if (component == null)
			{
				UnturnedLog.info("Received InteractableObjectQuestEventHook.OnUsed event from server, but matching transform doesn't have component on client! Server prefab is likely different from client prefab. (" + eventHookTransform.GetSceneHierarchyPath() + ")");
				return;
			}
			component.OnUsed.TryInvoke(component);
		}

		// Token: 0x06002FF4 RID: 12276 RVA: 0x000D3F58 File Offset: 0x000D2158
		private void OnUsedInternal()
		{
			this.OnUsed.TryInvoke(this);
			if (this.ShouldReplicate)
			{
				ENetReliability reliability = this.Reliable ? ENetReliability.Reliable : ENetReliability.Unreliable;
				float radius = (this.OverrideRelevantDistance > 0.01f) ? this.OverrideRelevantDistance : 128f;
				PooledTransportConnectionList transportConnections = Provider.GatherClientConnectionsWithinSphere(base.transform.position, radius);
				InteractableObjectQuestEventHook.SendUsedNotification.Invoke(reliability, transportConnections, base.transform);
			}
		}

		/// <summary>
		/// Invoked on authority when interactable object is used successfully.
		/// Only invoked on clients if ShouldReplicate is true.
		/// </summary>
		// Token: 0x040019EE RID: 6638
		public UnityEvent OnUsed;

		/// <summary>
		/// If true, the server will replicate the OnUsed event to clients as well.
		/// </summary>
		// Token: 0x040019EF RID: 6639
		public bool ShouldReplicate;

		/// <summary>
		/// If ShouldReplicate is enabled, should the RPC be called in reliable mode?
		/// Unreliable might not be received by clients.
		/// </summary>
		// Token: 0x040019F0 RID: 6640
		public bool Reliable = true;

		/// <summary>
		/// Applied if greater than zero. Defaults to 128.
		/// </summary>
		// Token: 0x040019F1 RID: 6641
		public float OverrideRelevantDistance;

		// Token: 0x040019F2 RID: 6642
		private static readonly ClientStaticMethod<Transform> SendUsedNotification = ClientStaticMethod<Transform>.Get(new ClientStaticMethod<Transform>.ReceiveDelegate(InteractableObjectQuestEventHook.ReceiveUsedNotification));

		// Token: 0x040019F3 RID: 6643
		private InteractableObjectTriggerableBase interactable;
	}
}
