using System;
using UnityEngine;

namespace SDG.Framework.Utilities
{
	// Token: 0x0200007F RID: 127
	public class MathUtility
	{
		// Token: 0x06000314 RID: 788 RVA: 0x0000C1EB File Offset: 0x0000A3EB
		public static void getPitchYawFromDirection(Vector3 direction, out float pitch, out float yaw)
		{
			pitch = 57.29578f * -Mathf.Sin(direction.y / direction.magnitude);
			yaw = 57.29578f * -Mathf.Atan2(direction.z, direction.x) + 90f;
		}

		// Token: 0x04000155 RID: 341
		public static readonly Quaternion IDENTITY_QUATERNION = Quaternion.identity;

		// Token: 0x04000156 RID: 342
		public static readonly Matrix4x4 IDENTITY_MATRIX = Matrix4x4.identity;
	}
}
