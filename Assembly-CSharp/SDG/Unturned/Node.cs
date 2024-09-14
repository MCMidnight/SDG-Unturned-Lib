using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004FE RID: 1278
	public class Node
	{
		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06002816 RID: 10262 RVA: 0x000A97CC File Offset: 0x000A79CC
		public Vector3 point
		{
			get
			{
				return this._point;
			}
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06002817 RID: 10263 RVA: 0x000A97D4 File Offset: 0x000A79D4
		public ENodeType type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06002818 RID: 10264 RVA: 0x000A97DC File Offset: 0x000A79DC
		public Transform model
		{
			get
			{
				return this._model;
			}
		}

		// Token: 0x06002819 RID: 10265 RVA: 0x000A97E4 File Offset: 0x000A79E4
		public void move(Vector3 newPoint)
		{
			this._point = newPoint;
			if (this._model != null)
			{
				this._model.position = this.point;
			}
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x000A980C File Offset: 0x000A7A0C
		public void setEnabled(bool isEnabled)
		{
			if (this._model != null)
			{
				this._model.gameObject.SetActive(isEnabled);
			}
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x000A982D File Offset: 0x000A7A2D
		public void remove()
		{
			if (this._model != null)
			{
				Object.Destroy(this._model.gameObject);
				this._model = null;
			}
		}

		// Token: 0x0400153F RID: 5439
		protected Vector3 _point;

		// Token: 0x04001540 RID: 5440
		protected ENodeType _type;

		// Token: 0x04001541 RID: 5441
		protected Transform _model;
	}
}
