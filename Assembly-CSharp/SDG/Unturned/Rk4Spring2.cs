using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Thanks to Glenn Fiedler for this RK4 implementation article:
	/// https://gafferongames.com/post/integration_basics/
	/// </summary>
	// Token: 0x02000813 RID: 2067
	[Serializable]
	public struct Rk4Spring2
	{
		// Token: 0x0600469F RID: 18079 RVA: 0x001A4E41 File Offset: 0x001A3041
		public Rk4Spring2(float stiffness, float damping)
		{
			this.currentPosition = default(Vector2);
			this.targetPosition = default(Vector2);
			this.stiffness = stiffness;
			this.damping = damping;
			this.currentVelocity = default(Vector2);
		}

		// Token: 0x060046A0 RID: 18080 RVA: 0x001A4E75 File Offset: 0x001A3075
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

		// Token: 0x060046A1 RID: 18081 RVA: 0x001A4EA4 File Offset: 0x001A30A4
		private void PrivateUpdate(float deltaTime)
		{
			Rk4Spring2.Rk4Derivative2 rk4Derivative = this.Evaluate(0f, default(Rk4Spring2.Rk4Derivative2));
			Rk4Spring2.Rk4Derivative2 rk4Derivative2 = this.Evaluate(deltaTime * 0.5f, rk4Derivative);
			Rk4Spring2.Rk4Derivative2 rk4Derivative3 = this.Evaluate(deltaTime * 0.5f, rk4Derivative2);
			Rk4Spring2.Rk4Derivative2 rk4Derivative4 = this.Evaluate(deltaTime, rk4Derivative3);
			Vector2 a = 0.16666667f * (rk4Derivative.velocity + 2f * (rk4Derivative2.velocity + rk4Derivative3.velocity) + rk4Derivative4.velocity);
			Vector2 a2 = 0.16666667f * (rk4Derivative.acceleration + 2f * (rk4Derivative2.acceleration + rk4Derivative3.acceleration) + rk4Derivative4.acceleration);
			this.currentPosition += a * deltaTime;
			this.currentVelocity += a2 * deltaTime;
		}

		// Token: 0x060046A2 RID: 18082 RVA: 0x001A4F9C File Offset: 0x001A319C
		private Rk4Spring2.Rk4Derivative2 Evaluate(float deltaTime, Rk4Spring2.Rk4Derivative2 initialDerivative)
		{
			Vector2 b = this.currentPosition + initialDerivative.velocity * deltaTime;
			Vector2 vector = this.currentVelocity + initialDerivative.acceleration * deltaTime;
			Rk4Spring2.Rk4Derivative2 result;
			result.velocity = vector;
			result.acceleration = this.stiffness * (this.targetPosition - b) - this.damping * vector;
			return result;
		}

		// Token: 0x04003019 RID: 12313
		public Vector2 currentPosition;

		// Token: 0x0400301A RID: 12314
		public Vector2 targetPosition;

		/// <summary>
		/// Higher values return to the target position faster.
		/// </summary>
		// Token: 0x0400301B RID: 12315
		public float stiffness;

		/// <summary>
		/// Higher values reduce bounciness and settle at the target position faster.
		/// e.g. a value of zero will bounce back and forth for a long time (indefinitely?)
		/// </summary>
		// Token: 0x0400301C RID: 12316
		public float damping;

		// Token: 0x0400301D RID: 12317
		private Vector2 currentVelocity;

		// Token: 0x02000A20 RID: 2592
		private struct Rk4Derivative2
		{
			// Token: 0x04003524 RID: 13604
			public Vector2 velocity;

			// Token: 0x04003525 RID: 13605
			public Vector2 acceleration;
		}
	}
}
