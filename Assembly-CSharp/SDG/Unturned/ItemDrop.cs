using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000564 RID: 1380
	public class ItemDrop
	{
		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06002C16 RID: 11286 RVA: 0x000BE3F8 File Offset: 0x000BC5F8
		public Transform model
		{
			get
			{
				return this._model;
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06002C17 RID: 11287 RVA: 0x000BE400 File Offset: 0x000BC600
		public InteractableItem interactableItem
		{
			get
			{
				return this._interactableItem;
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06002C18 RID: 11288 RVA: 0x000BE408 File Offset: 0x000BC608
		public uint instanceID
		{
			get
			{
				return this._instanceID;
			}
		}

		// Token: 0x06002C19 RID: 11289 RVA: 0x000BE410 File Offset: 0x000BC610
		public ItemDrop(Transform newModel, InteractableItem newInteractableItem, uint newInstanceID)
		{
			this._model = newModel;
			this._interactableItem = newInteractableItem;
			this._instanceID = newInstanceID;
		}

		// Token: 0x040017AD RID: 6061
		private Transform _model;

		// Token: 0x040017AE RID: 6062
		private InteractableItem _interactableItem;

		// Token: 0x040017AF RID: 6063
		private uint _instanceID;
	}
}
