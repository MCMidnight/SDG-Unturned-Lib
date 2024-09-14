using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004F9 RID: 1273
	public class LightLOD : MonoBehaviour
	{
		// Token: 0x06002800 RID: 10240 RVA: 0x000A90C0 File Offset: 0x000A72C0
		private void apply()
		{
			if (this.targetLight == null || this.targetLight.type == LightType.Area || this.targetLight.type == LightType.Directional)
			{
				return;
			}
			if (MainCamera.instance == null)
			{
				return;
			}
			Vector3 vector = base.transform.position - MainCamera.instance.transform.position;
			float sqrMagnitude = vector.sqrMagnitude;
			if (sqrMagnitude < this.sqrTransitionStart)
			{
				if (!this.targetLight.enabled)
				{
					this.targetLight.intensity = this.intensityStart;
					this.targetLight.enabled = true;
					return;
				}
			}
			else if (sqrMagnitude > this.sqrTransitionEnd)
			{
				if (this.targetLight.enabled)
				{
					this.targetLight.intensity = this.intensityEnd;
					this.targetLight.enabled = false;
					return;
				}
			}
			else
			{
				float t = (vector.magnitude - this.transitionStart) / this.transitionMagnitude;
				this.targetLight.intensity = Mathf.Lerp(this.intensityStart, this.intensityEnd, t);
				if (!this.targetLight.enabled)
				{
					this.targetLight.enabled = true;
				}
			}
		}

		// Token: 0x06002801 RID: 10241 RVA: 0x000A91E5 File Offset: 0x000A73E5
		private void Update()
		{
			this.apply();
		}

		// Token: 0x06002802 RID: 10242 RVA: 0x000A91F0 File Offset: 0x000A73F0
		private void Start()
		{
			if (this.targetLight == null || this.targetLight.type == LightType.Area || this.targetLight.type == LightType.Directional || LightLOD.HelperClass.disableLightLods)
			{
				base.enabled = false;
				return;
			}
			this.intensityStart = this.targetLight.intensity;
			this.intensityEnd = 0f;
			if (this.targetLight.type == LightType.Point)
			{
				this.transitionStart = this.targetLight.range * 13f;
				this.transitionEnd = this.targetLight.range * 15f;
			}
			else if (this.targetLight.type == LightType.Spot)
			{
				this.transitionStart = Mathf.Max(64f, this.targetLight.range) * 1.75f;
				this.transitionEnd = Mathf.Max(64f, this.targetLight.range) * 2f;
			}
			this.transitionMagnitude = this.transitionEnd - this.transitionStart;
			this.sqrTransitionStart = this.transitionStart * this.transitionStart;
			this.sqrTransitionEnd = this.transitionEnd * this.transitionEnd;
			this.apply();
		}

		// Token: 0x04001528 RID: 5416
		public Light targetLight;

		// Token: 0x04001529 RID: 5417
		private float intensityStart;

		// Token: 0x0400152A RID: 5418
		private float intensityEnd;

		// Token: 0x0400152B RID: 5419
		private float transitionStart;

		// Token: 0x0400152C RID: 5420
		private float transitionEnd;

		// Token: 0x0400152D RID: 5421
		private float transitionMagnitude;

		// Token: 0x0400152E RID: 5422
		private float sqrTransitionStart;

		// Token: 0x0400152F RID: 5423
		private float sqrTransitionEnd;

		/// <summary>
		/// Prevents static member from being initialized during MonoBehaviour construction. (Unity warning)
		/// </summary>
		// Token: 0x0200095D RID: 2397
		private static class HelperClass
		{
			// Token: 0x04003337 RID: 13111
			public static CommandLineFlag disableLightLods = new CommandLineFlag(false, "-DisableLightLODs");
		}
	}
}
