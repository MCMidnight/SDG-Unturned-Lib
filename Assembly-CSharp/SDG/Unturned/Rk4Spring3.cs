using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Thanks to Glenn Fiedler for this RK4 implementation article:
	/// https://gafferongames.com/post/integration_basics/
	/// </summary>
	// Token: 0x02000814 RID: 2068
	[Serializable]
	public struct Rk4Spring3
	{
		// Token: 0x060046A3 RID: 18083 RVA: 0x001A5011 File Offset: 0x001A3211
		public Rk4Spring3(float stiffness, float damping)
		{
			this.currentPosition = default(Vector3);
			this.targetPosition = default(Vector3);
			this.stiffness = stiffness;
			this.damping = damping;
			this.currentVelocity = default(Vector3);
		}

		// Token: 0x060046A4 RID: 18084 RVA: 0x001A5045 File Offset: 0x001A3245
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

		// Token: 0x060046A5 RID: 18085 RVA: 0x001A5074 File Offset: 0x001A3274
		private void PrivateUpdate(float deltaTime)
		{
			Rk4Spring3.Rk4Derivative3 rk4Derivative = this.Evaluate(0f, default(Rk4Spring3.Rk4Derivative3));
			Rk4Spring3.Rk4Derivative3 rk4Derivative2 = this.Evaluate(deltaTime * 0.5f, rk4Derivative);
			Rk4Spring3.Rk4Derivative3 rk4Derivative3 = this.Evaluate(deltaTime * 0.5f, rk4Derivative2);
			Rk4Spring3.Rk4Derivative3 rk4Derivative4 = this.Evaluate(deltaTime, rk4Derivative3);
			Vector3 a = 0.16666667f * (rk4Derivative.velocity + 2f * (rk4Derivative2.velocity + rk4Derivative3.velocity) + rk4Derivative4.velocity);
			Vector3 a2 = 0.16666667f * (rk4Derivative.acceleration + 2f * (rk4Derivative2.acceleration + rk4Derivative3.acceleration) + rk4Derivative4.acceleration);
			this.currentPosition += a * deltaTime;
			this.currentVelocity += a2 * deltaTime;
		}

		// Token: 0x060046A6 RID: 18086 RVA: 0x001A516C File Offset: 0x001A336C
		private Rk4Spring3.Rk4Derivative3 Evaluate(float deltaTime, Rk4Spring3.Rk4Derivative3 initialDerivative)
		{
			Vector3 b = this.currentPosition + initialDerivative.velocity * deltaTime;
			Vector3 vector = this.currentVelocity + initialDerivative.acceleration * deltaTime;
			Rk4Spring3.Rk4Derivative3 result;
			result.velocity = vector;
			result.acceleration = this.stiffness * (this.targetPosition - b) - this.damping * vector;
			return result;
		}

		// Token: 0x0400301E RID: 12318
		public Vector3 currentPosition;

		// Token: 0x0400301F RID: 12319
		public Vector3 targetPosition;

		/// <summary>
		/// Higher values return to the target position faster.
		/// </summary>
		// Token: 0x04003020 RID: 12320
		public float stiffness;

		/// <summary>
		/// Higher values reduce bounciness and settle at the target position faster.
		/// e.g. a value of zero will bounce back and forth for a long time (indefinitely?)
		/// </summary>
		// Token: 0x04003021 RID: 12321
		public float damping;

		// Token: 0x04003022 RID: 12322
		private Vector3 currentVelocity;

		// Token: 0x02000A21 RID: 2593
		private struct Rk4Derivative3
		{
			// Token: 0x04003526 RID: 13606
			public Vector3 velocity;

			// Token: 0x04003527 RID: 13607
			public Vector3 acceleration;
		}
	}
}
