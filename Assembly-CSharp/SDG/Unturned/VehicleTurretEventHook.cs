using System;
using UnityEngine;
using UnityEngine.Events;

namespace SDG.Unturned
{
	/// <summary>
	/// Can be added to Vehicle Turret_# GameObject to receive events.
	/// </summary>
	// Token: 0x020005DA RID: 1498
	[AddComponentMenu("Unturned/Vehicle Turret Event Hook")]
	public class VehicleTurretEventHook : MonoBehaviour
	{
		/// <summary>
		/// Invoked when turret gun is fired.
		/// </summary>
		// Token: 0x04001A28 RID: 6696
		public UnityEvent OnShotFired;

		/// <summary>
		/// Invoked when turret gun begins reload sequence.
		/// </summary>
		// Token: 0x04001A29 RID: 6697
		public UnityEvent OnReloadingStarted;

		/// <summary>
		/// Invoked when turret gun begins hammer sequence.
		/// </summary>
		// Token: 0x04001A2A RID: 6698
		public UnityEvent OnChamberingStarted;

		/// <summary>
		/// Invoked when turret gun begins aiming.
		/// </summary>
		// Token: 0x04001A2B RID: 6699
		public UnityEvent OnAimingStarted;

		/// <summary>
		/// Invoked when turret gun ends aiming.
		/// </summary>
		// Token: 0x04001A2C RID: 6700
		public UnityEvent OnAimingStopped;

		/// <summary>
		/// Invoked when turret gun controlled by a local player begins aiming.
		/// </summary>
		// Token: 0x04001A2D RID: 6701
		public UnityEvent OnAimingStarted_Local;

		/// <summary>
		/// Invoked when turret gun controlled by a local player ends aiming.
		/// </summary>
		// Token: 0x04001A2E RID: 6702
		public UnityEvent OnAimingStopped_Local;

		/// <summary>
		/// Invoked when turret gun controlled by a local player begins inspecting attachments.
		/// </summary>
		// Token: 0x04001A2F RID: 6703
		public UnityEvent OnInspectingAttachmentsStarted_Local;

		/// <summary>
		/// Invoked when turret gun controlled by a local player ends inspecting attachments.
		/// </summary>
		// Token: 0x04001A30 RID: 6704
		public UnityEvent OnInspectingAttachmentsStopped_Local;

		/// <summary>
		/// Invoked when any player enters the seat.
		/// </summary>
		// Token: 0x04001A31 RID: 6705
		public UnityEvent OnPassengerAdded;

		/// <summary>
		/// Invoked when any player exits the seat.
		/// </summary>
		// Token: 0x04001A32 RID: 6706
		public UnityEvent OnPassengerRemoved;

		/// <summary>
		/// Invoked when a locally controlled player enters the seat.
		/// </summary>
		// Token: 0x04001A33 RID: 6707
		public UnityEvent OnLocalPassengerAdded;

		/// <summary>
		/// Invoked when a locally controlled player exits the seat.
		/// </summary>
		// Token: 0x04001A34 RID: 6708
		public UnityEvent OnLocalPassengerRemoved;
	}
}
