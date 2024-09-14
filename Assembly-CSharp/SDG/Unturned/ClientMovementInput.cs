using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000620 RID: 1568
	internal struct ClientMovementInput
	{
		// Token: 0x04001CF6 RID: 7414
		public uint frameNumber;

		// Token: 0x04001CF7 RID: 7415
		public bool crouch;

		// Token: 0x04001CF8 RID: 7416
		public bool prone;

		// Token: 0x04001CF9 RID: 7417
		public bool sprint;

		// Token: 0x04001CFA RID: 7418
		public int input_x;

		// Token: 0x04001CFB RID: 7419
		public int input_y;

		// Token: 0x04001CFC RID: 7420
		public bool jump;

		// Token: 0x04001CFD RID: 7421
		public Quaternion rotation;

		// Token: 0x04001CFE RID: 7422
		public Quaternion aimRotation;
	}
}
