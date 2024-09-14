using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000529 RID: 1321
	public class ZombieSpawnpoint
	{
		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x06002945 RID: 10565 RVA: 0x000AFC96 File Offset: 0x000ADE96
		public Vector3 point
		{
			get
			{
				return this._point;
			}
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06002946 RID: 10566 RVA: 0x000AFC9E File Offset: 0x000ADE9E
		public Transform node
		{
			get
			{
				return this._node;
			}
		}

		// Token: 0x06002947 RID: 10567 RVA: 0x000AFCA6 File Offset: 0x000ADEA6
		public void setEnabled(bool isEnabled)
		{
			this.node.transform.gameObject.SetActive(isEnabled);
		}

		// Token: 0x06002948 RID: 10568 RVA: 0x000AFCC0 File Offset: 0x000ADEC0
		public ZombieSpawnpoint(byte newType, Vector3 newPoint)
		{
			this.type = newType;
			this._point = newPoint;
			if (Level.isEditor)
			{
				this._node = ((GameObject)Object.Instantiate(Resources.Load("Edit/Zombie"))).transform;
				this.node.name = this.type.ToString();
				this.node.position = this.point + Vector3.up;
				this.node.GetComponent<Renderer>().material.color = LevelZombies.tables[(int)this.type].color;
			}
		}

		// Token: 0x04001608 RID: 5640
		public byte type;

		// Token: 0x04001609 RID: 5641
		private Vector3 _point;

		// Token: 0x0400160A RID: 5642
		private Transform _node;
	}
}
