using System;
using SDG.Framework.Landscapes;
using UnityEngine;

namespace SDG.Framework.Utilities
{
	// Token: 0x02000081 RID: 129
	public class PhysicsUtility
	{
		// Token: 0x0600031A RID: 794 RVA: 0x0000C4DB File Offset: 0x0000A6DB
		[Obsolete("Hole collision is handled by Unity now")]
		public static bool raycast(Ray ray, out RaycastHit hit, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			if ((layerMask & 1048576) == 1048576)
			{
				LandscapeHoleUtility.raycastIgnoreLandscapeIfNecessary(ray, maxDistance, ref layerMask);
			}
			return Physics.Raycast(ray, out hit, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000C4FF File Offset: 0x0000A6FF
		[Obsolete("Hole collision is handled by Unity now")]
		public static RaycastHit[] raycastAll(Ray ray, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			if ((layerMask & 1048576) == 1048576)
			{
				LandscapeHoleUtility.raycastIgnoreLandscapeIfNecessary(ray, maxDistance, ref layerMask);
			}
			return Physics.RaycastAll(ray, maxDistance, layerMask, queryTriggerInteraction);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000C521 File Offset: 0x0000A721
		[Obsolete("Hole collision is handled by Unity now")]
		public static int sphereCastNonAlloc(Ray ray, float radius, RaycastHit[] results, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			if ((layerMask & 1048576) == 1048576)
			{
				LandscapeHoleUtility.spherecastIgnoreLandscapeIfNecessary(ray, radius, maxDistance, ref layerMask);
			}
			return Physics.SphereCastNonAlloc(ray, radius, results, maxDistance, layerMask, queryTriggerInteraction);
		}
	}
}
