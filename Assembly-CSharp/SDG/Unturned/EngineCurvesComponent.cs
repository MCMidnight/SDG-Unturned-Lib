using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020005C9 RID: 1481
	public class EngineCurvesComponent : MonoBehaviour
	{
		// Token: 0x040019E7 RID: 6631
		[Tooltip("Maps normalized engine RPM to torque multiplier.\nIdle RPM is zero and max RPM is one on the X axis.")]
		public AnimationCurve engineRpmToTorqueCurve;
	}
}
