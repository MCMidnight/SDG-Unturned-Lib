using System;
using UnityEngine;
using UnityEngine.Events;

namespace SDG.Unturned
{
	/// <summary>
	/// Can be added to EquipablePrefab item GameObject to receive events.
	/// </summary>
	// Token: 0x020005D7 RID: 1495
	[AddComponentMenu("Unturned/Useable Event Hook")]
	public class UseableEventHook : MonoBehaviour
	{
		/// <summary>
		/// Invoked when item begins inspect animation.
		/// </summary>
		// Token: 0x04001A18 RID: 6680
		public UnityEvent OnInspectStarted;
	}
}
