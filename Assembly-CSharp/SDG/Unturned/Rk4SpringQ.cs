using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Thanks to Glenn Fiedler for this RK4 implementation article:
	/// https://gafferongames.com/post/integration_basics/
	/// </summary>
	// Token: 0x02000815 RID: 2069
	[Serializable]
	public struct Rk4SpringQ
	{
		// Token: 0x060046A7 RID: 18087 RVA: 0x001A51E1 File Offset: 0x001A33E1
		public Rk4SpringQ(float stiffness, float damping)
		{
			this.currentRotation = Quaternion.identity;
			this.targetRotation = Quaternion.identity;
			this.stiffness = stiffness;
			this.damping = damping;
			this.currentVelocity = default(Vector3);
		}

		// Token: 0x060046A8 RID: 18088 RVA: 0x001A5213 File Offset: 0x001A3413
		public void Update(float deltaTime)
		{
			while (deltaTime > 0.05f)
			{
				this.PrivateUpdate(0.05f);
				deltaTime -= 0.05f;
			}
			if (deltaTime > 0f)
			{
				this.PrivateUpdate(deltaTime);
			}
		}

		// Token: 0x060046A9 RID: 18089 RVA: 0x001A5244 File Offset: 0x001A3444
		private void PrivateUpdate(float deltaTime)
		{
			Rk4SpringQ.Rk4DerivativeQ rk4DerivativeQ = this.Evaluate(0f, default(Rk4SpringQ.Rk4DerivativeQ));
			Rk4SpringQ.Rk4DerivativeQ rk4DerivativeQ2 = this.Evaluate(deltaTime * 0.5f, rk4DerivativeQ);
			Rk4SpringQ.Rk4DerivativeQ rk4DerivativeQ3 = this.Evaluate(deltaTime * 0.5f, rk4DerivativeQ2);
			Rk4SpringQ.Rk4DerivativeQ rk4DerivativeQ4 = this.Evaluate(deltaTime, rk4DerivativeQ3);
			Vector3 a = 0.16666667f * (rk4DerivativeQ.velocity + 2f * (rk4DerivativeQ2.velocity + rk4DerivativeQ3.velocity) + rk4DerivativeQ4.velocity);
			Vector3 a2 = 0.16666667f * (rk4DerivativeQ.acceleration + 2f * (rk4DerivativeQ2.acceleration + rk4DerivativeQ3.acceleration) + rk4DerivativeQ4.acceleration);
			this.currentRotation *= Quaternion.Euler(a * deltaTime);
			this.currentVelocity += a2 * deltaTime;
		}

		// Token: 0x060046AA RID: 18090 RVA: 0x001A5340 File Offset: 0x001A3540
		private Rk4SpringQ.Rk4DerivativeQ Evaluate(float deltaTime, Rk4SpringQ.Rk4DerivativeQ initialDerivative)
		{
			Quaternion rotation = this.currentRotation * Quaternion.Euler(initialDerivative.velocity * deltaTime);
			Vector3 vector = this.currentVelocity + initialDerivative.acceleration * deltaTime;
			float d;
			Vector3 a;
			(Quaternion.Inverse(rotation) * this.targetRotation).ToAngleAxis(out d, out a);
			Rk4SpringQ.Rk4DerivativeQ result;
			result.velocity = vector;
			result.acceleration = this.stiffness * a * d - this.damping * vector;
			return result;
		}

		// Token: 0x04003023 RID: 12323
		public Quaternion currentRotation;

		// Token: 0x04003024 RID: 12324
		public Quaternion targetRotation;

		/// <summary>
		/// Higher values return to the target position faster.
		/// </summary>
		// Token: 0x04003025 RID: 12325
		public float stiffness;

		/// <summary>
		/// Higher values reduce bounciness and settle at the target position faster.
		/// e.g. a value of zero will bounce back and forth for a long time (indefinitely?)
		/// </summary>
		// Token: 0x04003026 RID: 12326
		public float damping;

		// Token: 0x04003027 RID: 12327
		private Vector3 currentVelocity;

		// Token: 0x02000A22 RID: 2594
		private struct Rk4DerivativeQ
		{
			// Token: 0x04003528 RID: 13608
			public Vector3 velocity;

			// Token: 0x04003529 RID: 13609
			public Vector3 acceleration;
		}
	}
}
