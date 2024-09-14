using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007F4 RID: 2036
	public static class CollisionUtil
	{
		/// <summary>
		/// Find colliders in gameObject and encapsulate their bounding boxes together.
		/// </summary>
		/// <returns>True if bounds were determined, false otherwise.</returns>
		// Token: 0x060045FE RID: 17918 RVA: 0x001A23FC File Offset: 0x001A05FC
		public static bool EncapsulateColliderBounds(GameObject gameObject, bool includeInactive, out Bounds bounds)
		{
			CollisionUtil.getBoundsWorkingList.Clear();
			gameObject.GetComponentsInChildren<Collider>(includeInactive, CollisionUtil.getBoundsWorkingList);
			if (CollisionUtil.getBoundsWorkingList.Count > 0)
			{
				bounds = CollisionUtil.getBoundsWorkingList[0].bounds;
				for (int i = 1; i < CollisionUtil.getBoundsWorkingList.Count; i++)
				{
					bounds.Encapsulate(CollisionUtil.getBoundsWorkingList[i].bounds);
				}
				return true;
			}
			bounds = default(Bounds);
			return false;
		}

		/// <summary>
		/// Find colliders in gameObject and the point closest to position, otherwise use gameObject position.
		/// </summary>
		// Token: 0x060045FF RID: 17919 RVA: 0x001A2478 File Offset: 0x001A0678
		public static Vector3 ClosestPoint(GameObject gameObject, Vector3 position, bool includeInactive)
		{
			CollisionUtil.getBoundsWorkingList.Clear();
			gameObject.GetComponentsInChildren<Collider>(includeInactive, CollisionUtil.getBoundsWorkingList);
			Vector3 result;
			if (CollisionUtil.getBoundsWorkingList.Count > 0 && CollisionUtil.ClosestPoint(CollisionUtil.getBoundsWorkingList, position, out result))
			{
				return result;
			}
			return gameObject.transform.position;
		}

		// Token: 0x06004600 RID: 17920 RVA: 0x001A24C4 File Offset: 0x001A06C4
		public static bool ClosestPoint(List<Collider> colliders, Vector3 position, out Vector3 result)
		{
			bool flag = false;
			result = default(Vector3);
			float num = -1f;
			foreach (Collider collider in colliders)
			{
				if (!(collider == null) && collider.enabled && !collider.isTrigger)
				{
					MeshCollider meshCollider = collider as MeshCollider;
					if (meshCollider != null)
					{
						if (!meshCollider.convex)
						{
							continue;
						}
					}
					else if (!(collider is BoxCollider) && !(collider is SphereCollider) && !(collider is CapsuleCollider))
					{
						continue;
					}
					Vector3 vector = collider.ClosestPoint(position);
					float sqrMagnitude = (vector - position).sqrMagnitude;
					if (flag)
					{
						if (sqrMagnitude < num)
						{
							result = vector;
							num = sqrMagnitude;
						}
					}
					else
					{
						flag = true;
						result = vector;
						num = sqrMagnitude;
					}
				}
			}
			return flag;
		}

		// Token: 0x06004601 RID: 17921 RVA: 0x001A25B0 File Offset: 0x001A07B0
		public static Vector3 ClosestPoint(List<Collider> colliders, Vector3 position)
		{
			Vector3 result;
			if (CollisionUtil.ClosestPoint(colliders, position, out result))
			{
				return result;
			}
			return position;
		}

		// Token: 0x06004602 RID: 17922 RVA: 0x001A25CB File Offset: 0x001A07CB
		public static int OverlapBoxColliderNonAlloc(BoxCollider collider, Collider[] results, int mask, QueryTriggerInteraction queryTriggerInteraction)
		{
			return collider.OverlapBoxNonAlloc(results, mask, queryTriggerInteraction);
		}

		/// <summary>
		/// Does sphere overlap anything?
		/// </summary>
		// Token: 0x06004603 RID: 17923 RVA: 0x001A25D6 File Offset: 0x001A07D6
		public static bool OverlapSphere(Vector3 position, float radius, int layerMask, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			return Physics.OverlapSphereNonAlloc(position, radius, CollisionUtil.results, layerMask, queryTriggerInteraction) > 0;
		}

		// Token: 0x04002F2F RID: 12079
		private static List<Collider> getBoundsWorkingList = new List<Collider>();

		// Token: 0x04002F30 RID: 12080
		private static Collider[] results = new Collider[1];
	}
}
