using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000563 RID: 1379
	public class ItemData
	{
		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06002C10 RID: 11280 RVA: 0x000BE3A0 File Offset: 0x000BC5A0
		public Item item
		{
			get
			{
				return this._item;
			}
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06002C11 RID: 11281 RVA: 0x000BE3A8 File Offset: 0x000BC5A8
		public uint instanceID
		{
			get
			{
				return this._instanceID;
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06002C12 RID: 11282 RVA: 0x000BE3B0 File Offset: 0x000BC5B0
		public Vector3 point
		{
			get
			{
				return this._point;
			}
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06002C13 RID: 11283 RVA: 0x000BE3B8 File Offset: 0x000BC5B8
		public bool isDropped
		{
			get
			{
				return this._isDropped;
			}
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06002C14 RID: 11284 RVA: 0x000BE3C0 File Offset: 0x000BC5C0
		public float lastDropped
		{
			get
			{
				return this._lastDropped;
			}
		}

		// Token: 0x06002C15 RID: 11285 RVA: 0x000BE3C8 File Offset: 0x000BC5C8
		public ItemData(Item newItem, uint newInstanceID, Vector3 newPoint, bool newDropped)
		{
			this._item = newItem;
			this._instanceID = newInstanceID;
			this._point = newPoint;
			this._isDropped = newDropped;
			this._lastDropped = Time.realtimeSinceStartup;
		}

		// Token: 0x040017A8 RID: 6056
		private Item _item;

		// Token: 0x040017A9 RID: 6057
		private uint _instanceID;

		// Token: 0x040017AA RID: 6058
		private Vector3 _point;

		// Token: 0x040017AB RID: 6059
		private bool _isDropped;

		// Token: 0x040017AC RID: 6060
		private float _lastDropped;
	}
}
