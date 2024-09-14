using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000591 RID: 1425
	public class StructureData
	{
		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x06002D8C RID: 11660 RVA: 0x000C67BD File Offset: 0x000C49BD
		public Structure structure
		{
			get
			{
				return this._structure;
			}
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x06002D8D RID: 11661 RVA: 0x000C67C5 File Offset: 0x000C49C5
		// (set) Token: 0x06002D8E RID: 11662 RVA: 0x000C67CD File Offset: 0x000C49CD
		public uint instanceID { get; private set; }

		// Token: 0x06002D8F RID: 11663 RVA: 0x000C67D6 File Offset: 0x000C49D6
		public StructureData(Structure newStructure, Vector3 newPoint, Quaternion newRotation, ulong newOwner, ulong newGroup, uint newObjActiveDate, uint newInstanceID)
		{
			this._structure = newStructure;
			this.point = newPoint;
			this.rotation = newRotation;
			this.owner = newOwner;
			this.group = newGroup;
			this.objActiveDate = newObjActiveDate;
			this.instanceID = newInstanceID;
		}

		// Token: 0x06002D90 RID: 11664 RVA: 0x000C6814 File Offset: 0x000C4A14
		[Obsolete]
		public StructureData(Structure newStructure, Vector3 newPoint, byte newAngle_X, byte newAngle_Y, byte newAngle_Z, ulong newOwner, ulong newGroup, uint newObjActiveDate, uint newInstanceID)
		{
			this._structure = newStructure;
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

		// Token: 0x0400188D RID: 6285
		private Structure _structure;

		// Token: 0x0400188E RID: 6286
		public Vector3 point;

		// Token: 0x0400188F RID: 6287
		public Quaternion rotation;

		// Token: 0x04001890 RID: 6288
		[Obsolete("Replaced by rotation quaternion, but you should probably not be accessing either of these directly.")]
		public byte angle_x;

		// Token: 0x04001891 RID: 6289
		[Obsolete("Replaced by rotation quaternion, but you should probably not be accessing either of these directly.")]
		public byte angle_y;

		// Token: 0x04001892 RID: 6290
		[Obsolete("Replaced by rotation quaternion, but you should probably not be accessing either of these directly.")]
		public byte angle_z;

		// Token: 0x04001893 RID: 6291
		public ulong owner;

		// Token: 0x04001894 RID: 6292
		public ulong group;

		// Token: 0x04001895 RID: 6293
		public uint objActiveDate;
	}
}
