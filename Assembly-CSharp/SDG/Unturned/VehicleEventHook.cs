using System;
using UnityEngine;
using UnityEngine.Events;

namespace SDG.Unturned
{
	/// <summary>
	/// Can be added to Vehicle GameObject to receive events.
	/// </summary>
	// Token: 0x020005D9 RID: 1497
	[AddComponentMenu("Unturned/Vehicle Event Hook")]
	public class VehicleEventHook : MonoBehaviour
	{
		/// <summary>
		/// Invoked when any player enters the driver seat.
		/// </summary>
		// Token: 0x04001A1E RID: 6686
		public UnityEvent OnDriverAdded;

		/// <summary>
		/// Invoked when any player exits the driver seat.
		/// </summary>
		// Token: 0x04001A1F RID: 6687
		public UnityEvent OnDriverRemoved;

		/// <summary>
		/// Invoked when a locally controlled player enters the driver seat.
		/// </summary>
		// Token: 0x04001A20 RID: 6688
		public UnityEvent OnLocalDriverAdded;

		/// <summary>
		/// Invoked when a locally controlled player exits the driver seat.
		/// </summary>
		// Token: 0x04001A21 RID: 6689
		public UnityEvent OnLocalDriverRemoved;

		/// <summary>
		/// Invoked when a locally controlled player enters the vehicle.
		/// </summary>
		// Token: 0x04001A22 RID: 6690
		public UnityEvent OnLocalPassengerAdded;

		/// <summary>
		/// Invoked when a locally controlled player exits the vehicle.
		/// </summary>
		// Token: 0x04001A23 RID: 6691
		public UnityEvent OnLocalPassengerRemoved;

		/// <summary>
		/// Invoked when lock is engaged.
		/// </summary>
		// Token: 0x04001A24 RID: 6692
		public UnityEvent OnLocked;

		/// <summary>
		/// Invoked when lock is disengaged.
		/// </summary>
		// Token: 0x04001A25 RID: 6693
		public UnityEvent OnUnlocked;

		/// <summary>
		/// Invoked when horn is played.
		/// </summary>
		// Token: 0x04001A26 RID: 6694
		public UnityEvent OnHornUsed;

		/// <summary>
		/// Invoked after explosion plays.
		/// </summary>
		// Token: 0x04001A27 RID: 6695
		public UnityEvent OnExploded;
	}
}
