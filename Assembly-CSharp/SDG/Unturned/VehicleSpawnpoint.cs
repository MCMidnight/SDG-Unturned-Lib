using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000513 RID: 1299
	public class VehicleSpawnpoint
	{
		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x060028A1 RID: 10401 RVA: 0x000ACFBF File Offset: 0x000AB1BF
		public Vector3 point
		{
			get
			{
				return this._point;
			}
		}

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x060028A2 RID: 10402 RVA: 0x000ACFC7 File Offset: 0x000AB1C7
		public float angle
		{
			get
			{
				return this._angle;
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x060028A3 RID: 10403 RVA: 0x000ACFCF File Offset: 0x000AB1CF
		public Transform node
		{
			get
			{
				return this._node;
			}
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x000ACFD7 File Offset: 0x000AB1D7
		public void setEnabled(bool isEnabled)
		{
			this.node.transform.gameObject.SetActive(isEnabled);
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x000ACFF0 File Offset: 0x000AB1F0
		public VehicleSpawnpoint(byte newType, Vector3 newPoint, float newAngle)
		{
			this.type = newType;
			this._point = newPoint;
			this._angle = newAngle;
			if (Level.isEditor)
			{
				this._node = ((GameObject)Object.Instantiate(Resources.Load("Edit/Vehicle"))).transform;
				this.node.name = this.type.ToString();
				this.node.position = this.point;
				this.node.rotation = Quaternion.Euler(0f, this.angle, 0f);
				this.node.GetComponent<Renderer>().material.color = LevelVehicles.tables[(int)this.type].color;
				this.node.Find("Arrow").GetComponent<Renderer>().material.color = LevelVehicles.tables[(int)this.type].color;
			}
		}

		// Token: 0x0400159E RID: 5534
		public byte type;

		// Token: 0x0400159F RID: 5535
		private Vector3 _point;

		// Token: 0x040015A0 RID: 5536
		private float _angle;

		// Token: 0x040015A1 RID: 5537
		private Transform _node;
	}
}
