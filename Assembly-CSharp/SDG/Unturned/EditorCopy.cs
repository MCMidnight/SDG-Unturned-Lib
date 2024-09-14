using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020003FE RID: 1022
	public class EditorCopy
	{
		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06001E31 RID: 7729 RVA: 0x0006DDEF File Offset: 0x0006BFEF
		public Vector3 position
		{
			get
			{
				return this._position;
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06001E32 RID: 7730 RVA: 0x0006DDF7 File Offset: 0x0006BFF7
		public Quaternion rotation
		{
			get
			{
				return this._rotation;
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x06001E33 RID: 7731 RVA: 0x0006DDFF File Offset: 0x0006BFFF
		public Vector3 scale
		{
			get
			{
				return this._scale;
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06001E34 RID: 7732 RVA: 0x0006DE07 File Offset: 0x0006C007
		public ObjectAsset objectAsset
		{
			get
			{
				return this._objectAsset;
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06001E35 RID: 7733 RVA: 0x0006DE0F File Offset: 0x0006C00F
		public ItemAsset itemAsset
		{
			get
			{
				return this._itemAsset;
			}
		}

		// Token: 0x06001E36 RID: 7734 RVA: 0x0006DE17 File Offset: 0x0006C017
		public EditorCopy(Vector3 newPosition, Quaternion newRotation, Vector3 newScale, ObjectAsset newObjectAsset, ItemAsset newItemAsset)
		{
			this._position = newPosition;
			this._rotation = newRotation;
			this._scale = newScale;
			this._objectAsset = newObjectAsset;
			this._itemAsset = newItemAsset;
		}

		// Token: 0x04000E75 RID: 3701
		private Vector3 _position;

		// Token: 0x04000E76 RID: 3702
		private Quaternion _rotation;

		// Token: 0x04000E77 RID: 3703
		private Vector3 _scale;

		// Token: 0x04000E78 RID: 3704
		private ObjectAsset _objectAsset;

		// Token: 0x04000E79 RID: 3705
		private ItemAsset _itemAsset;
	}
}
