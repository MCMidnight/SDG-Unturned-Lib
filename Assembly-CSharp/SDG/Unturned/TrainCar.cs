using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200047F RID: 1151
	public class TrainCar
	{
		// Token: 0x04001173 RID: 4467
		public float trackPositionOffset;

		// Token: 0x04001174 RID: 4468
		public Vector3 currentFrontPosition;

		// Token: 0x04001175 RID: 4469
		public Vector3 currentFrontNormal;

		// Token: 0x04001176 RID: 4470
		public Vector3 currentFrontDirection;

		// Token: 0x04001177 RID: 4471
		public Vector3 currentBackPosition;

		// Token: 0x04001178 RID: 4472
		public Vector3 currentBackNormal;

		// Token: 0x04001179 RID: 4473
		public Vector3 currentBackDirection;

		// Token: 0x0400117A RID: 4474
		public Transform root;

		// Token: 0x0400117B RID: 4475
		public Transform trackFront;

		// Token: 0x0400117C RID: 4476
		public Transform trackBack;

		/// <summary>
		/// Rigidbody component on the root game object.
		/// </summary>
		// Token: 0x0400117D RID: 4477
		public Rigidbody rootRigidbody;
	}
}
