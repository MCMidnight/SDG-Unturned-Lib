using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000532 RID: 1330
	public class BarricadeData
	{
		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x060029B7 RID: 10679 RVA: 0x000B2227 File Offset: 0x000B0427
		public Barricade barricade
		{
			get
			{
				return this._barricade;
			}
		}

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x060029B8 RID: 10680 RVA: 0x000B222F File Offset: 0x000B042F
		// (set) Token: 0x060029B9 RID: 10681 RVA: 0x000B2237 File Offset: 0x000B0437
		public uint instanceID { get; private set; }

		// Token: 0x060029BA RID: 10682 RVA: 0x000B2240 File Offset: 0x000B0440
		public BarricadeData(Barricade newBarricade, Vector3 newPoint, Quaternion newRotation, ulong newOwner, ulong newGroup, uint newObjActiveDate, uint newInstanceID)
		{
			this._barricade = newBarricade;
			this.point = newPoint;
			this.rotation = newRotation;
			this.owner = newOwner;
			this.group = newGroup;
			this.objActiveDate = newObjActiveDate;
			this.instanceID = newInstanceID;
		}

		// Token: 0x060029BB RID: 10683 RVA: 0x000B2280 File Offset: 0x000B0480
		[Obsolete]
		public BarricadeData(Barricade newBarricade, Vector3 newPoint, byte newAngle_X, byte newAngle_Y, byte newAngle_Z, ulong newOwner, ulong newGroup, uint newObjActiveDate, uint newInstanceID)
		{
			this._barricade = newBarricade;
			this.point = newPoint;
			this.rotation = Quaternion.Euler((float)newAngle_X * 2f, (float)newAngle_Y * 2f, (float)newAngle_Z * 2f);
			this.angle_x = newAngle_X;
			this.angle_y = newAngle_Y;
			this.angle_z = newAngle_Z;
			this.owner = newOwner;
			this.group = newGroup;
			this.objActiveDate = newObjActiveDate;
			this.instanceID = newInstanceID;
		}

		// Token: 0x0400167B RID: 5755
		private Barricade _barricade;

		// Token: 0x0400167C RID: 5756
		public Vector3 point;

		/// <summary>
		/// Note: If barricade is attached to a vehicle this is the local rotation.
		/// </summary>
		// Token: 0x0400167D RID: 5757
		public Quaternion rotation;

		// Token: 0x0400167E RID: 5758
		[Obsolete("Replaced by rotation quaternion, but you should probably not be accessing either of these directly.")]
		public byte angle_x;

		// Token: 0x0400167F RID: 5759
		[Obsolete("Replaced by rotation quaternion, but you should probably not be accessing either of these directly.")]
		public byte angle_y;

		// Token: 0x04001680 RID: 5760
		[Obsolete("Replaced by rotation quaternion, but you should probably not be accessing either of these directly.")]
		public byte angle_z;

		// Token: 0x04001681 RID: 5761
		public ulong owner;

		// Token: 0x04001682 RID: 5762
		public ulong group;

		// Token: 0x04001683 RID: 5763
		public uint objActiveDate;
	}
}
