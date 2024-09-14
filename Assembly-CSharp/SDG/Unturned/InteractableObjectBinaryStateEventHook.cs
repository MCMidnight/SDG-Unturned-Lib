using System;
using UnityEngine;
using UnityEngine.Events;
using Unturned.UnityEx;

namespace SDG.Unturned
{
	/// <summary>
	/// Can be added to any GameObject with an interactable binary state in its parents.
	///
	/// If players should not be allowed to interact with the object in the ordinary manner,
	/// add the Interactability_Remote flag to its asset to indicate only mod hooks should control it.
	/// </summary>
	// Token: 0x020005CB RID: 1483
	[AddComponentMenu("Unturned/IOBS Event Hook")]
	public class InteractableObjectBinaryStateEventHook : MonoBehaviour
	{
		/// <summary>
		/// Set state to Enabled if currently Disabled.
		///
		/// On dedicated server this directly changes the state,
		/// but as client this will apply the usual conditions and rewards.
		/// </summary>
		// Token: 0x06002FEA RID: 12266 RVA: 0x000D3CCC File Offset: 0x000D1ECC
		public void GotoEnabledState()
		{
			if (this.interactable != null)
			{
				this.interactable.SetUsedFromClientOrServer(true, this.ListenServerHostMode);
			}
		}

		/// <summary>
		/// Set state to Disabled if currently Enabled.
		///
		/// On dedicated server this directly changes the state,
		/// but as client this will apply the usual conditions and rewards.
		/// </summary>
		// Token: 0x06002FEB RID: 12267 RVA: 0x000D3CEE File Offset: 0x000D1EEE
		public void GotoDisabledState()
		{
			if (this.interactable != null)
			{
				this.interactable.SetUsedFromClientOrServer(false, this.ListenServerHostMode);
			}
		}

		/// <summary>
		/// Toggle between the Enabled and Disabled states.
		///
		/// On dedicated server this directly changes the state,
		/// but as client this will apply the usual conditions and rewards. 
		/// </summary>
		// Token: 0x06002FEC RID: 12268 RVA: 0x000D3D10 File Offset: 0x000D1F10
		public void ToggleState()
		{
			if (this.interactable != null)
			{
				this.interactable.SetUsedFromClientOrServer(!this.interactable.isUsed, this.ListenServerHostMode);
			}
		}

		// Token: 0x06002FED RID: 12269 RVA: 0x000D3D40 File Offset: 0x000D1F40
		protected void Start()
		{
			this.interactable = base.gameObject.GetComponentInParent<InteractableObjectBinaryState>();
			if (this.interactable == null)
			{
				UnturnedLog.warn("IOBS {0} unable to find interactable", new object[]
				{
					this.GetSceneHierarchyPath()
				});
				return;
			}
			this.interactable.modHookCounter++;
			this.interactable.onStateChanged += this.onStateChanged;
			if (this.InvokeWhenInitialized)
			{
				this.interactable.onStateInitialized += this.onStateChanged;
				this.onStateChanged(this.interactable);
			}
		}

		// Token: 0x06002FEE RID: 12270 RVA: 0x000D3DDC File Offset: 0x000D1FDC
		protected void OnDestroy()
		{
			if (this.interactable != null)
			{
				this.interactable.onStateInitialized -= this.onStateChanged;
				this.interactable.onStateChanged -= this.onStateChanged;
				this.interactable.modHookCounter--;
				this.interactable = null;
			}
		}

		// Token: 0x06002FEF RID: 12271 RVA: 0x000D3E3F File Offset: 0x000D203F
		protected void onStateChanged(InteractableObjectBinaryState sender)
		{
			if (sender.isUsed)
			{
				this.OnStateEnabled.TryInvoke(this);
				return;
			}
			this.OnStateDisabled.TryInvoke(this);
		}

		/// <summary>
		/// Invoked when interactable object enters the Used / On / Enabled state.
		/// </summary>
		// Token: 0x040019E9 RID: 6633
		public UnityEvent OnStateEnabled;

		/// <summary>
		/// Invoked when interactable object enters the Unused / Off / Disabled state.
		/// </summary>
		// Token: 0x040019EA RID: 6634
		public UnityEvent OnStateDisabled;

		/// <summary>
		/// Should the OnStateEnabled and OnStateDisabled events be invoked when the object is loaded, becomes relevant
		/// in multiplayer, and is reset? True is useful when visuals need to be kept in sync with the state, whereas
		/// false is useful for transient interactions.
		/// </summary>
		// Token: 0x040019EB RID: 6635
		public bool InvokeWhenInitialized = true;

		/// <summary>
		/// Controls how state change requests are performed when running as both client and server ("listen server").
		/// On the dedicated server, requesting a state change overrides the current state without processing NPC
		/// conditions, whereas when a client requests a state change NPC conditions apply. This option fixes the
		/// inconsistency in singleplayer of whether to treat as server or client. (public issue #4298)
		/// At the time of writing (2024-01-29) listen server only applies to singleplayer.
		/// </summary>
		// Token: 0x040019EC RID: 6636
		public InteractableObjectBinaryStateEventHook.EListenServerHostMode ListenServerHostMode;

		// Token: 0x040019ED RID: 6637
		private InteractableObjectBinaryState interactable;

		// Token: 0x02000998 RID: 2456
		public enum EListenServerHostMode
		{
			/// <summary>
			/// When a state change is requested in singleplayer it should be treated as if running as a client on a server.
			/// This is the default to match behavior from before this option was added.
			/// </summary>
			// Token: 0x040033C5 RID: 13253
			RequestAsClient,
			/// <summary>
			/// When a state change is requested in singleplayer it should be treated as if running as a dedicated server.
			/// </summary>
			// Token: 0x040033C6 RID: 13254
			OverrideState
		}
	}
}
