using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Component for the tactical laser attachment's red dot.
	/// Resizes itself per-camera to maintain a constant on-screen size.
	/// </summary>
	// Token: 0x02000442 RID: 1090
	public class TacticalLaserScale : MonoBehaviour
	{
		// Token: 0x060020CC RID: 8396 RVA: 0x0007E4AC File Offset: 0x0007C6AC
		public void OnWillRenderObject()
		{
			Camera current = Camera.current;
			float magnitude = (base.transform.position - current.transform.position).magnitude;
			float num = current.fieldOfView * 0.5f;
			float num2 = Mathf.Tan(0.017453292f * num);
			float num3 = this.scalingCurve.Evaluate(magnitude * num2) * this.scaleMultiplier;
			base.transform.localScale = new Vector3(num3, num3, num3);
		}

		// Token: 0x04001008 RID: 4104
		public float scaleMultiplier = 0.1f;

		/// <summary>
		/// Used to tune the scale by distance so that far away laser is not quite as comically large.
		/// </summary>
		// Token: 0x04001009 RID: 4105
		public AnimationCurve scalingCurve;
	}
}
