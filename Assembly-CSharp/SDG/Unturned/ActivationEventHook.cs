using System;
using UnityEngine;
using UnityEngine.Events;

namespace SDG.Unturned
{
	// Token: 0x020005BF RID: 1471
	[AddComponentMenu("Unturned/Activation Event Hook")]
	public class ActivationEventHook : MonoBehaviour
	{
		// Token: 0x06002FCC RID: 12236 RVA: 0x000D37BA File Offset: 0x000D19BA
		private void OnEnable()
		{
			this.OnEnabled.Invoke();
		}

		// Token: 0x06002FCD RID: 12237 RVA: 0x000D37C7 File Offset: 0x000D19C7
		private void OnDisable()
		{
			this.OnDisabled.Invoke();
		}

		/// <summary>
		/// Invoked when component is enabled and when the game object is activated.
		/// </summary>
		// Token: 0x040019C8 RID: 6600
		public UnityEvent OnEnabled;

		/// <summary>
		/// Invoked when component is disabled and when the game object is deactivated.
		/// Note that if the component or game object spawn deactivated this will not be immediately invoked.
		/// </summary>
		// Token: 0x040019C9 RID: 6601
		public UnityEvent OnDisabled;
	}
}
