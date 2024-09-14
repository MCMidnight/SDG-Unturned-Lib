using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000482 RID: 1154
	public class Passenger
	{
		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06002413 RID: 9235 RVA: 0x0008FF76 File Offset: 0x0008E176
		public Transform seat
		{
			get
			{
				return this._seat;
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06002414 RID: 9236 RVA: 0x0008FF7E File Offset: 0x0008E17E
		public Transform obj
		{
			get
			{
				return this._obj;
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x06002415 RID: 9237 RVA: 0x0008FF86 File Offset: 0x0008E186
		// (set) Token: 0x06002416 RID: 9238 RVA: 0x0008FF8E File Offset: 0x0008E18E
		public Quaternion rotationYaw { get; private set; }

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06002417 RID: 9239 RVA: 0x0008FF97 File Offset: 0x0008E197
		public Transform turretYaw
		{
			get
			{
				return this._turretYaw;
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06002418 RID: 9240 RVA: 0x0008FF9F File Offset: 0x0008E19F
		// (set) Token: 0x06002419 RID: 9241 RVA: 0x0008FFA7 File Offset: 0x0008E1A7
		public Quaternion rotationPitch { get; private set; }

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x0600241A RID: 9242 RVA: 0x0008FFB0 File Offset: 0x0008E1B0
		public Transform turretPitch
		{
			get
			{
				return this._turretPitch;
			}
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x0600241B RID: 9243 RVA: 0x0008FFB8 File Offset: 0x0008E1B8
		public Transform turretAim
		{
			get
			{
				return this._turretAim;
			}
		}

		// Token: 0x0600241C RID: 9244 RVA: 0x0008FFC0 File Offset: 0x0008E1C0
		public Passenger(Transform newSeat, Transform newObj, Transform newTurretYaw, Transform newTurretPitch, Transform newTurretAim)
		{
			this._seat = newSeat;
			this._obj = newObj;
			this._turretYaw = newTurretYaw;
			this._turretPitch = newTurretPitch;
			this._turretAim = newTurretAim;
			if (this.turretYaw != null)
			{
				this.rotationYaw = this.turretYaw.localRotation;
			}
			if (this.turretPitch != null)
			{
				this.rotationPitch = this.turretPitch.localRotation;
			}
		}

		// Token: 0x04001215 RID: 4629
		public SteamPlayer player;

		// Token: 0x04001216 RID: 4630
		public TurretInfo turret;

		// Token: 0x04001217 RID: 4631
		private Transform _seat;

		// Token: 0x04001218 RID: 4632
		private Transform _obj;

		/// <summary>
		/// Optional component on Turret_# GameObject for modding UnityEvents.
		/// </summary>
		// Token: 0x04001219 RID: 4633
		public VehicleTurretEventHook turretEventHook;

		// Token: 0x0400121B RID: 4635
		private Transform _turretYaw;

		// Token: 0x0400121D RID: 4637
		private Transform _turretPitch;

		// Token: 0x0400121E RID: 4638
		private Transform _turretAim;

		// Token: 0x0400121F RID: 4639
		public byte[] state;

		/// <summary>
		/// Optional collider matching the player capsule to prevent short vehicles (e.g. bikes) from clipping into walls.
		/// </summary>
		// Token: 0x04001220 RID: 4640
		internal CapsuleCollider collider;
	}
}
