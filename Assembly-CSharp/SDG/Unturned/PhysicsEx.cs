using System;
using SDG.Framework.Landscapes;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Extensions to the built-in Physics class.
	///
	/// Shares similar functionality to the SDG.Framework.Utilities.PhysicsUtility class, but that should be moved here
	/// because the "framework" is unused and and the long name is annoying.
	/// </summary>
	// Token: 0x0200080C RID: 2060
	public static class PhysicsEx
	{
		/// <summary>
		/// Wrapper that respects landscape hole volumes.
		/// </summary>
		// Token: 0x0600467D RID: 18045 RVA: 0x001A42EC File Offset: 0x001A24EC
		[Obsolete("Hole collision is handled by Unity now")]
		public static bool Raycast(Ray ray, out RaycastHit hit, float maxDistance = float.PositiveInfinity, int layerMask = -5, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			if ((layerMask & 1048576) == 1048576)
			{
				LandscapeHoleUtility.raycastIgnoreLandscapeIfNecessary(ray, maxDistance, ref layerMask);
			}
			return Physics.Raycast(ray, out hit, maxDistance, layerMask, queryTriggerInteraction);
		}

		/// <summary>
		/// Wrapper that respects landscape hole volumes.
		/// </summary>
		// Token: 0x0600467E RID: 18046 RVA: 0x001A4310 File Offset: 0x001A2510
		[Obsolete("Hole collision is handled by Unity now")]
		public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hit, float maxDistance = float.PositiveInfinity, int layerMask = -5, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			if ((layerMask & 1048576) == 1048576)
			{
				LandscapeHoleUtility.raycastIgnoreLandscapeIfNecessary(new Ray(origin, direction), maxDistance, ref layerMask);
			}
			return Physics.Raycast(origin, direction, out hit, maxDistance, layerMask, queryTriggerInteraction);
		}

		/// <summary>
		/// Wrapper that respects landscape hole volumes.
		/// </summary>
		// Token: 0x0600467F RID: 18047 RVA: 0x001A433D File Offset: 0x001A253D
		[Obsolete("Hole collision is handled by Unity now")]
		public static bool SphereCast(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance = float.PositiveInfinity, int layerMask = -5, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			if ((layerMask & 1048576) == 1048576)
			{
				LandscapeHoleUtility.spherecastIgnoreLandscapeIfNecessary(new Ray(origin, direction), radius, maxDistance, ref layerMask);
			}
			return Physics.SphereCast(origin, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}

		/// <summary>
		/// Wrapper that respects landscape hole volumes.
		/// </summary>
		// Token: 0x06004680 RID: 18048 RVA: 0x001A436E File Offset: 0x001A256E
		[Obsolete("Hole collision is handled by Unity now")]
		public static bool SphereCast(Ray ray, float radius, out RaycastHit hitInfo, float maxDistance = float.PositiveInfinity, int layerMask = -5, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			if ((layerMask & 1048576) == 1048576)
			{
				LandscapeHoleUtility.spherecastIgnoreLandscapeIfNecessary(ray, radius, maxDistance, ref layerMask);
			}
			return Physics.SphereCast(ray, radius, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
		}
	}
}
