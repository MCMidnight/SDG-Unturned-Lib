using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000756 RID: 1878
	public class InputInfo
	{
		// Token: 0x040026A8 RID: 9896
		public ERaycastInfoType type;

		// Token: 0x040026A9 RID: 9897
		public ERaycastInfoUsage usage;

		// Token: 0x040026AA RID: 9898
		public Vector3 point;

		// Token: 0x040026AB RID: 9899
		public Vector3 direction;

		// Token: 0x040026AC RID: 9900
		public Vector3 normal;

		// Token: 0x040026AD RID: 9901
		public Player player;

		// Token: 0x040026AE RID: 9902
		public Zombie zombie;

		// Token: 0x040026AF RID: 9903
		public Animal animal;

		// Token: 0x040026B0 RID: 9904
		public ELimb limb;

		// Token: 0x040026B1 RID: 9905
		public string materialName;

		// Token: 0x040026B2 RID: 9906
		[Obsolete("Replaced by materialName")]
		public EPhysicsMaterial material;

		// Token: 0x040026B3 RID: 9907
		public InteractableVehicle vehicle;

		/// <summary>
		/// Root transform.
		/// </summary>
		// Token: 0x040026B4 RID: 9908
		public Transform transform;

		/// <summary>
		/// Hit collider's transform. Can be null.
		/// </summary>
		// Token: 0x040026B5 RID: 9909
		public Transform colliderTransform;

		// Token: 0x040026B6 RID: 9910
		public byte section;
	}
}
