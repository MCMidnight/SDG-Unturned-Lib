using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007F3 RID: 2035
	public static class ColliderGizmosEx
	{
		// Token: 0x060045FC RID: 17916 RVA: 0x001A23A8 File Offset: 0x001A05A8
		public static void DrawCapsuleGizmo(this CapsuleCollider collider, Color color, float lifespan = 0f)
		{
			Vector3 begin;
			Vector3 end;
			collider.GetCapsulePoints(out begin, out end);
			RuntimeGizmos.Get().Capsule(begin, end, collider.radius, color, lifespan, EGizmoLayer.World);
		}

		// Token: 0x060045FD RID: 17917 RVA: 0x001A23D4 File Offset: 0x001A05D4
		public static void DrawSphereGizmo(this SphereCollider collider, Color color, float lifespan = 0f)
		{
			Vector3 center = collider.TransformSphereCenter();
			RuntimeGizmos.Get().Sphere(center, collider.radius, color, lifespan, EGizmoLayer.World);
		}
	}
}
