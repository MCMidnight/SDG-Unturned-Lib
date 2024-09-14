using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200038E RID: 910
	public class HumanAnimator : CharacterAnimator
	{
		// Token: 0x06001C47 RID: 7239 RVA: 0x00064F1C File Offset: 0x0006311C
		public void force()
		{
			this._lean = Mathf.Clamp(this.lean, -1f, 1f);
			this._pitch = Mathf.Clamp(this.pitch, 1f, 179f) - 90f;
			this._offset = this.offset;
		}

		// Token: 0x06001C48 RID: 7240 RVA: 0x00064F74 File Offset: 0x00063174
		public void apply()
		{
			bool animationPlaying = base.getAnimationPlaying();
			if (animationPlaying)
			{
				this.leftShoulder.parent = this.skull;
				this.rightShoulder.parent = this.skull;
				this.spineHook.parent = this.skull;
			}
			this.spine.Rotate(0f, this._pitch * 0.5f, this._lean * HumanAnimator.LEAN);
			this.skull.Rotate(0f, this._pitch * 0.5f, 0f);
			this.skull.position += this.skull.forward * this.offset;
			if (animationPlaying)
			{
				this.skull.Rotate(0f, -this.spine.localRotation.eulerAngles.x + this._pitch * 0.5f, 0f);
				this.leftShoulder.parent = this.spine;
				this.rightShoulder.parent = this.spine;
				this.spineHook.parent = this.spine;
				this.skull.Rotate(0f, this.spine.localRotation.eulerAngles.x - this._pitch * 0.5f, 0f);
			}
		}

		// Token: 0x06001C49 RID: 7241 RVA: 0x000650E0 File Offset: 0x000632E0
		private void LateUpdate()
		{
			this._lean = Mathf.LerpAngle(this._lean, Mathf.Clamp(this.lean, -1f, 1f), 4f * Time.deltaTime);
			this._pitch = Mathf.LerpAngle(this._pitch, Mathf.Clamp(this.pitch, 1f, 179f) - 90f, 8f * Time.deltaTime);
			this._offset = Mathf.Lerp(this._offset, this.offset, 4f * Time.deltaTime);
			this.apply();
		}

		// Token: 0x06001C4A RID: 7242 RVA: 0x0006517D File Offset: 0x0006337D
		private void Awake()
		{
			base.init();
		}

		// Token: 0x04000D63 RID: 3427
		public static readonly float LEAN = 20f;

		// Token: 0x04000D64 RID: 3428
		private float _lean;

		// Token: 0x04000D65 RID: 3429
		public float lean;

		// Token: 0x04000D66 RID: 3430
		private float _pitch = 90f;

		// Token: 0x04000D67 RID: 3431
		public float pitch = 90f;

		// Token: 0x04000D68 RID: 3432
		private float _offset;

		// Token: 0x04000D69 RID: 3433
		public float offset;
	}
}
