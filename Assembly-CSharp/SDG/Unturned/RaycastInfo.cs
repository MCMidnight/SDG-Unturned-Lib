using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000769 RID: 1897
	public class RaycastInfo
	{
		// Token: 0x06003E01 RID: 15873 RVA: 0x0012CC4C File Offset: 0x0012AE4C
		public RaycastInfo(RaycastHit hit)
		{
			this.transform = ((hit.collider != null) ? hit.collider.transform : null);
			this.collider = hit.collider;
			this.distance = hit.distance;
			this.point = hit.point;
			this.normal = hit.normal;
			this.section = byte.MaxValue;
		}

		// Token: 0x06003E02 RID: 15874 RVA: 0x0012CCC2 File Offset: 0x0012AEC2
		public RaycastInfo(Transform hit)
		{
			this.transform = hit;
			this.point = hit.position;
			this.section = byte.MaxValue;
		}

		// Token: 0x04002703 RID: 9987
		public Transform transform;

		// Token: 0x04002704 RID: 9988
		public Collider collider;

		// Token: 0x04002705 RID: 9989
		public float distance;

		// Token: 0x04002706 RID: 9990
		public Vector3 point;

		// Token: 0x04002707 RID: 9991
		public Vector3 direction;

		// Token: 0x04002708 RID: 9992
		public Vector3 normal;

		// Token: 0x04002709 RID: 9993
		public Player player;

		// Token: 0x0400270A RID: 9994
		public Zombie zombie;

		// Token: 0x0400270B RID: 9995
		public Animal animal;

		// Token: 0x0400270C RID: 9996
		public ELimb limb;

		// Token: 0x0400270D RID: 9997
		public string materialName;

		// Token: 0x0400270E RID: 9998
		[Obsolete]
		public EPhysicsMaterial material;

		// Token: 0x0400270F RID: 9999
		public InteractableVehicle vehicle;

		// Token: 0x04002710 RID: 10000
		public byte section;
	}
}
