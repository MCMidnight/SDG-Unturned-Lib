using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000417 RID: 1047
	public class ReunObjectRemove : IReun
	{
		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06001EC6 RID: 7878 RVA: 0x000725AF File Offset: 0x000707AF
		// (set) Token: 0x06001EC7 RID: 7879 RVA: 0x000725B7 File Offset: 0x000707B7
		public int step { get; private set; }

		// Token: 0x06001EC8 RID: 7880 RVA: 0x000725C0 File Offset: 0x000707C0
		public Transform redo()
		{
			if (this.model != null)
			{
				if (this.objectAsset != null)
				{
					LevelObjects.removeObject(this.model);
					this.model = null;
				}
				else if (this.itemAsset != null)
				{
					LevelObjects.removeBuildable(this.model);
					this.model = null;
				}
			}
			return null;
		}

		// Token: 0x06001EC9 RID: 7881 RVA: 0x00072614 File Offset: 0x00070814
		public void undo()
		{
			if (this.model == null)
			{
				if (this.objectAsset != null)
				{
					this.model = LevelObjects.addObject(this.position, this.rotation, this.scale, this.objectAsset.id, this.objectAsset.GUID, ELevelObjectPlacementOrigin.MANUAL);
					return;
				}
				if (this.itemAsset != null)
				{
					this.model = LevelObjects.addBuildable(this.position, this.rotation, this.itemAsset.id);
				}
			}
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x00072696 File Offset: 0x00070896
		public ReunObjectRemove(int newStep, Transform newModel, ObjectAsset newObjectAsset, ItemAsset newItemAsset, Vector3 newPosition, Quaternion newRotation, Vector3 newScale)
		{
			this.step = newStep;
			this.model = newModel;
			this.objectAsset = newObjectAsset;
			this.itemAsset = newItemAsset;
			this.position = newPosition;
			this.rotation = newRotation;
			this.scale = newScale;
		}

		// Token: 0x04000F29 RID: 3881
		private Transform model;

		// Token: 0x04000F2A RID: 3882
		private ObjectAsset objectAsset;

		// Token: 0x04000F2B RID: 3883
		private ItemAsset itemAsset;

		// Token: 0x04000F2C RID: 3884
		private Vector3 position;

		// Token: 0x04000F2D RID: 3885
		private Quaternion rotation;

		// Token: 0x04000F2E RID: 3886
		private Vector3 scale;
	}
}
