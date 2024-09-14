using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004A7 RID: 1191
	public class AnimalSpawnpoint
	{
		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x060024F2 RID: 9458 RVA: 0x00093233 File Offset: 0x00091433
		public Vector3 point
		{
			get
			{
				return this._point;
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x060024F3 RID: 9459 RVA: 0x0009323B File Offset: 0x0009143B
		public Transform node
		{
			get
			{
				return this._node;
			}
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x00093243 File Offset: 0x00091443
		public void setEnabled(bool isEnabled)
		{
			this.node.transform.gameObject.SetActive(isEnabled);
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x0009325C File Offset: 0x0009145C
		public AnimalSpawnpoint(byte newType, Vector3 newPoint)
		{
			this.type = newType;
			this._point = newPoint;
			if (Level.isEditor)
			{
				this._node = ((GameObject)Object.Instantiate(Resources.Load("Edit/Animal"))).transform;
				this.node.name = this.type.ToString();
				this.node.position = this.point;
				this.node.GetComponent<Renderer>().material.color = LevelAnimals.tables[(int)this.type].color;
			}
		}

		// Token: 0x040012C5 RID: 4805
		public byte type;

		// Token: 0x040012C6 RID: 4806
		private Vector3 _point;

		// Token: 0x040012C7 RID: 4807
		private Transform _node;
	}
}
