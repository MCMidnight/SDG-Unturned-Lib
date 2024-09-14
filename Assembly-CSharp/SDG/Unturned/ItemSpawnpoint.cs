using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004CD RID: 1229
	public class ItemSpawnpoint
	{
		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x0600258D RID: 9613 RVA: 0x00095584 File Offset: 0x00093784
		public Vector3 point
		{
			get
			{
				return this._point;
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x0600258E RID: 9614 RVA: 0x0009558C File Offset: 0x0009378C
		public Transform node
		{
			get
			{
				return this._node;
			}
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x00095594 File Offset: 0x00093794
		public void setEnabled(bool isEnabled)
		{
			this.node.transform.gameObject.SetActive(isEnabled);
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x000955AC File Offset: 0x000937AC
		public ItemSpawnpoint(byte newType, Vector3 newPoint)
		{
			this.type = newType;
			this._point = newPoint;
			if (Level.isEditor)
			{
				this._node = ((GameObject)Object.Instantiate(Resources.Load("Edit/Item"))).transform;
				this.node.name = this.type.ToString();
				this.node.position = this.point;
				this.node.GetComponent<Renderer>().material.color = LevelItems.tables[(int)this.type].color;
			}
		}

		// Token: 0x0400134C RID: 4940
		public byte type;

		// Token: 0x0400134D RID: 4941
		private Vector3 _point;

		// Token: 0x0400134E RID: 4942
		private Transform _node;
	}
}
