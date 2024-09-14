using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Thanks to Glenn Fiedler for this RK4 implementation article:
	/// https://gafferongames.com/post/integration_basics/
	/// </summary>
	// Token: 0x02000812 RID: 2066
	[Serializable]
	public struct Rk4Spring
	{
		// Token: 0x0600469B RID: 18075 RVA: 0x001A4CC9 File Offset: 0x001A2EC9
		public Rk4Spring(float stiffness, float damping)
		{
			this.currentPosition = 0f;
			this.targetPosition = 0f;
			this.stiffness = stiffness;
			this.damping = damping;
			this.currentVelocity = 0f;
		}

		// Token: 0x0600469C RID: 18076 RVA: 0x001A4CFA File Offset: 0x001A2EFA
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

		// Token: 0x0600469D RID: 18077 RVA: 0x001A4D2C File Offset: 0x001A2F2C
		private void PrivateUpdate(float deltaTime)
		{
			Rk4Spring.Rk4Derivative rk4Derivative = this.Evaluate(0f, default(Rk4Spring.Rk4Derivative));
			Rk4Spring.Rk4Derivative rk4Derivative2 = this.Evaluate(deltaTime * 0.5f, rk4Derivative);
			Rk4Spring.Rk4Derivative rk4Derivative3 = this.Evaluate(deltaTime * 0.5f, rk4Derivative2);
			Rk4Spring.Rk4Derivative rk4Derivative4 = this.Evaluate(deltaTime, rk4Derivative3);
			float num = 0.16666667f * (rk4Derivative.velocity + 2f * (rk4Derivative2.velocity + rk4Derivative3.velocity) + rk4Derivative4.velocity);
			float num2 = 0.16666667f * (rk4Derivative.acceleration + 2f * (rk4Derivative2.acceleration + rk4Derivative3.acceleration) + rk4Derivative4.acceleration);
			this.currentPosition += num * deltaTime;
			this.currentVelocity += num2 * deltaTime;
		}

		// Token: 0x0600469E RID: 18078 RVA: 0x001A4DEC File Offset: 0x001A2FEC
		private Rk4Spring.Rk4Derivative Evaluate(float deltaTime, Rk4Spring.Rk4Derivative initialDerivative)
		{
			float num = this.currentPosition + initialDerivative.velocity * deltaTime;
			float num2 = this.currentVelocity + initialDerivative.acceleration * deltaTime;
			Rk4Spring.Rk4Derivative result;
			result.velocity = num2;
			result.acceleration = this.stiffness * (this.targetPosition - num) - this.damping * num2;
			return result;
		}

		// Token: 0x04003013 RID: 12307
		public float currentPosition;

		// Token: 0x04003014 RID: 12308
		public float targetPosition;

		/// <summary>
		/// Higher values return to the target position faster.
		/// </summary>
		// Token: 0x04003015 RID: 12309
		public float stiffness;

		/// <summary>
		/// Higher values reduce bounciness and settle at the target position faster.
		/// e.g. a value of zero will bounce back and forth for a long time (indefinitely?)
		/// </summary>
		// Token: 0x04003016 RID: 12310
		public float damping;

		// Token: 0x04003017 RID: 12311
		private float currentVelocity;

		/// <summary>
		/// At low framerate deltaTime can be so high the spring explodes unless we use a fixed timestep.
		/// </summary>
		// Token: 0x04003018 RID: 12312
		internal const float MAX_TIMESTEP = 0.05f;

		// Token: 0x02000A1F RID: 2591
		private struct Rk4Derivative
		{
			// Token: 0x04003522 RID: 13602
			public float velocity;

			// Token: 0x04003523 RID: 13603
			public float acceleration;
		}
	}
}
