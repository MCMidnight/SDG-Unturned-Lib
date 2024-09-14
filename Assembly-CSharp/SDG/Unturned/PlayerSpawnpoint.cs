using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000501 RID: 1281
	public class PlayerSpawnpoint
	{
		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x06002829 RID: 10281 RVA: 0x000A9AB2 File Offset: 0x000A7CB2
		public Vector3 point
		{
			get
			{
				return this._point;
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x0600282A RID: 10282 RVA: 0x000A9ABA File Offset: 0x000A7CBA
		public float angle
		{
			get
			{
				return this._angle;
			}
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x0600282B RID: 10283 RVA: 0x000A9AC2 File Offset: 0x000A7CC2
		public bool isAlt
		{
			get
			{
				return this._isAlt;
			}
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x0600282C RID: 10284 RVA: 0x000A9ACA File Offset: 0x000A7CCA
		public Transform node
		{
			get
			{
				return this._node;
			}
		}

		// Token: 0x0600282D RID: 10285 RVA: 0x000A9AD2 File Offset: 0x000A7CD2
		public void setEnabled(bool isEnabled)
		{
			this.node.transform.gameObject.SetActive(isEnabled);
		}

		// Token: 0x0600282E RID: 10286 RVA: 0x000A9AEC File Offset: 0x000A7CEC
		public PlayerSpawnpoint(Vector3 newPoint, float newAngle, bool newIsAlt)
		{
			this._point = newPoint;
			this._angle = newAngle;
			this._isAlt = newIsAlt;
			if (Level.isEditor)
			{
				this._node = ((GameObject)Object.Instantiate(Resources.Load(this.isAlt ? "Edit/Player_Alt" : "Edit/Player"))).transform;
				this.node.name = "Player";
				this.node.position = this.point;
				this.node.rotation = Quaternion.Euler(0f, this.angle, 0f);
			}
		}

		// Token: 0x04001545 RID: 5445
		private Vector3 _point;

		// Token: 0x04001546 RID: 5446
		private float _angle;

		// Token: 0x04001547 RID: 5447
		private bool _isAlt;

		// Token: 0x04001548 RID: 5448
		private Transform _node;
	}
}
