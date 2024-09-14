using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004DD RID: 1245
	public class LevelBuildableObject
	{
		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x06002637 RID: 9783 RVA: 0x0009A85D File Offset: 0x00098A5D
		public Transform transform
		{
			get
			{
				return this._transform;
			}
		}

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x06002638 RID: 9784 RVA: 0x0009A865 File Offset: 0x00098A65
		public ushort id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06002639 RID: 9785 RVA: 0x0009A86D File Offset: 0x00098A6D
		public ItemAsset asset
		{
			get
			{
				return this._asset;
			}
		}

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x0600263A RID: 9786 RVA: 0x0009A875 File Offset: 0x00098A75
		// (set) Token: 0x0600263B RID: 9787 RVA: 0x0009A87D File Offset: 0x00098A7D
		public bool isEnabled { get; private set; }

		// Token: 0x0600263C RID: 9788 RVA: 0x0009A886 File Offset: 0x00098A86
		public void enable()
		{
			this.isEnabled = true;
			if (this.transform != null)
			{
				this.transform.gameObject.SetActive(true);
			}
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x0009A8AE File Offset: 0x00098AAE
		public void disable()
		{
			this.isEnabled = false;
			if (this.transform != null)
			{
				this.transform.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x0009A8D6 File Offset: 0x00098AD6
		public void destroy()
		{
			if (this.transform != null)
			{
				Object.Destroy(this.transform.gameObject);
			}
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x0009A8F8 File Offset: 0x00098AF8
		public LevelBuildableObject(Vector3 newPoint, Quaternion newRotation, ushort newID)
		{
			this.point = newPoint;
			this.rotation = newRotation;
			this._id = newID;
			this._asset = (Assets.find(EAssetType.ITEM, this.id) as ItemAsset);
			if (this.asset == null || this.asset.id != this.id)
			{
				this._asset = (Assets.find(EAssetType.ITEM, this.id) as ItemAsset);
				if (this.asset == null)
				{
					return;
				}
			}
			if (Level.isEditor)
			{
				ItemBarricadeAsset itemBarricadeAsset = this.asset as ItemBarricadeAsset;
				ItemStructureAsset itemStructureAsset = this.asset as ItemStructureAsset;
				GameObject gameObject = null;
				if (itemBarricadeAsset != null)
				{
					gameObject = itemBarricadeAsset.barricade;
				}
				else if (itemStructureAsset != null)
				{
					gameObject = itemStructureAsset.structure;
				}
				if (gameObject != null)
				{
					GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, newPoint, newRotation);
					this._transform = gameObject2.transform;
					gameObject2.name = this.id.ToString();
					if (this.transform.GetComponent<Rigidbody>() == null)
					{
						Rigidbody rigidbody = this.transform.gameObject.AddComponent<Rigidbody>();
						rigidbody.useGravity = false;
						rigidbody.isKinematic = true;
					}
					this.transform.gameObject.SetActive(false);
					LevelBuildableObject.colliders.Clear();
					this.transform.GetComponentsInChildren<Collider>(true, LevelBuildableObject.colliders);
					for (int i = 0; i < LevelBuildableObject.colliders.Count; i++)
					{
						if (LevelBuildableObject.colliders[i].gameObject.layer != 27 && LevelBuildableObject.colliders[i].gameObject.layer != 28)
						{
							Object.Destroy(LevelBuildableObject.colliders[i].gameObject);
						}
					}
				}
			}
		}

		// Token: 0x040013C7 RID: 5063
		private static List<Collider> colliders = new List<Collider>();

		// Token: 0x040013C8 RID: 5064
		public Vector3 point;

		// Token: 0x040013C9 RID: 5065
		public Quaternion rotation;

		// Token: 0x040013CA RID: 5066
		private Transform _transform;

		// Token: 0x040013CB RID: 5067
		private ushort _id;

		// Token: 0x040013CC RID: 5068
		private ItemAsset _asset;
	}
}
