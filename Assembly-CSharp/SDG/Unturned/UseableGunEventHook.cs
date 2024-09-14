using System;
using UnityEngine;
using UnityEngine.Events;

namespace SDG.Unturned
{
	/// <summary>
	/// Can be added to EquipablePrefab item GameObject to receive events.
	/// </summary>
	// Token: 0x020005D8 RID: 1496
	[AddComponentMenu("Unturned/Useable Gun Event Hook")]
	public class UseableGunEventHook : MonoBehaviour
	{
		/// <summary>
		/// Invoked when gun is fired.
		/// </summary>
		// Token: 0x04001A19 RID: 6681
		public UnityEvent OnShotFired;

		/// <summary>
		/// Invoked when gun begins reload sequence.
		/// </summary>
		// Token: 0x04001A1A RID: 6682
		public UnityEvent OnReloadingStarted;

		/// <summary>
		/// Invoked when gun begins hammer sequence.
		/// </summary>
		// Token: 0x04001A1B RID: 6683
		public UnityEvent OnChamberingStarted;

		/// <summary>
		/// Invoked when gun begins aiming.
		/// </summary>
		// Token: 0x04001A1C RID: 6684
		public UnityEvent OnAimingStarted;

		/// <summary>
		/// Invoked when gun ends aiming.
		/// </summary>
		// Token: 0x04001A1D RID: 6685
		public UnityEvent OnAimingStopped;
	}
}
