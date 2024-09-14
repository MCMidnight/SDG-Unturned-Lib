using System;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	// Token: 0x020000F6 RID: 246
	public class FoliageSurfaceComponent : MonoBehaviour, IFoliageSurface
	{
		// Token: 0x0600062A RID: 1578 RVA: 0x00018294 File Offset: 0x00016494
		public FoliageBounds getFoliageSurfaceBounds()
		{
			bool activeSelf = base.gameObject.activeSelf;
			if (!activeSelf)
			{
				base.gameObject.SetActive(true);
			}
			FoliageBounds result = new FoliageBounds(this.surfaceCollider.bounds);
			if (!activeSelf)
			{
				base.gameObject.SetActive(false);
			}
			return result;
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x000182DC File Offset: 0x000164DC
		public bool getFoliageSurfaceInfo(Vector3 position, out Vector3 surfacePosition, out Vector3 surfaceNormal)
		{
			RaycastHit raycastHit;
			if (this.surfaceCollider.Raycast(new Ray(position, Vector3.down), out raycastHit, 1024f))
			{
				surfacePosition = raycastHit.point;
				surfaceNormal = raycastHit.normal;
				return true;
			}
			surfacePosition = Vector3.zero;
			surfaceNormal = Vector3.up;
			return false;
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0001833C File Offset: 0x0001653C
		public void bakeFoliageSurface(FoliageBakeSettings bakeSettings, FoliageTile foliageTile)
		{
			FoliageInfoCollectionAsset foliageInfoCollectionAsset = Assets.find<FoliageInfoCollectionAsset>(this.foliage);
			if (foliageInfoCollectionAsset == null)
			{
				return;
			}
			bool activeSelf = base.gameObject.activeSelf;
			if (!activeSelf)
			{
				base.gameObject.SetActive(true);
			}
			Bounds worldBounds = foliageTile.worldBounds;
			Vector3 min = worldBounds.min;
			Vector3 max = worldBounds.max;
			Bounds bounds = this.surfaceCollider.bounds;
			Vector3 min2 = bounds.min;
			Vector3 max2 = bounds.max;
			foliageInfoCollectionAsset.bakeFoliage(bakeSettings, this, new Bounds
			{
				min = new Vector3(Mathf.Max(min.x, min2.x), min2.y, Mathf.Max(min.z, min2.z)),
				max = new Vector3(Mathf.Min(max.x, max2.x), max2.y, Mathf.Min(max.z, max2.z))
			}, 1f);
			if (!activeSelf)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0001843C File Offset: 0x0001663C
		protected void OnEnable()
		{
			if (this.isRegistered)
			{
				return;
			}
			this.isRegistered = true;
			FoliageSystem.addSurface(this);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00018454 File Offset: 0x00016654
		protected void OnDestroy()
		{
			if (!this.isRegistered)
			{
				return;
			}
			this.isRegistered = false;
			FoliageSystem.removeSurface(this);
		}

		// Token: 0x0400025A RID: 602
		public AssetReference<FoliageInfoCollectionAsset> foliage;

		// Token: 0x0400025B RID: 603
		public Collider surfaceCollider;

		// Token: 0x0400025C RID: 604
		protected bool isRegistered;
	}
}
