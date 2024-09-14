using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000416 RID: 1046
	public class ReunObjectAdd : IReun
	{
		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06001EC1 RID: 7873 RVA: 0x000724B7 File Offset: 0x000706B7
		// (set) Token: 0x06001EC2 RID: 7874 RVA: 0x000724BF File Offset: 0x000706BF
		public int step { get; private set; }

		// Token: 0x06001EC3 RID: 7875 RVA: 0x000724C8 File Offset: 0x000706C8
		public Transform redo()
		{
			if (this.model == null)
			{
				if (this.objectAsset != null)
				{
					this.model = LevelObjects.addObject(this.position, this.rotation, this.scale, this.objectAsset.id, this.objectAsset.GUID, ELevelObjectPlacementOrigin.MANUAL);
				}
				else if (this.itemAsset != null)
				{
					this.model = LevelObjects.addBuildable(this.position, this.rotation, this.itemAsset.id);
				}
			}
			return this.model;
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x00072551 File Offset: 0x00070751
		public void undo()
		{
			if (this.model != null)
			{
				LevelObjects.removeObject(this.model);
				this.model = null;
			}
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x00072573 File Offset: 0x00070773
		public ReunObjectAdd(int newStep, ObjectAsset newObjectAsset, ItemAsset newItemAsset, Vector3 newPosition, Quaternion newRotation, Vector3 newScale)
		{
			this.step = newStep;
			this.model = null;
			this.objectAsset = newObjectAsset;
			this.itemAsset = newItemAsset;
			this.position = newPosition;
			this.rotation = newRotation;
			this.scale = newScale;
		}

		// Token: 0x04000F22 RID: 3874
		private Transform model;

		// Token: 0x04000F23 RID: 3875
		private ObjectAsset objectAsset;

		// Token: 0x04000F24 RID: 3876
		private ItemAsset itemAsset;

		// Token: 0x04000F25 RID: 3877
		private Vector3 position;

		// Token: 0x04000F26 RID: 3878
		private Quaternion rotation;

		// Token: 0x04000F27 RID: 3879
		private Vector3 scale;
	}
}
