using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200044E RID: 1102
	public class InteractableClock : InteractablePower
	{
		// Token: 0x06002123 RID: 8483 RVA: 0x0007FD21 File Offset: 0x0007DF21
		public override void updateState(Asset asset, byte[] state)
		{
			this.handHourTransform = base.transform.Find("Hand_Hour");
			this.handMinuteTransform = base.transform.Find("Hand_Minute");
			base.updateState(asset, state);
			base.RefreshIsConnectedToPowerWithoutNotify();
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x0007FD5D File Offset: 0x0007DF5D
		public override bool checkUseable()
		{
			return base.isWired;
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x0007FD65 File Offset: 0x0007DF65
		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			text = "";
			color = Color.white;
			if (!base.isWired)
			{
				message = EPlayerMessage.POWER;
				return true;
			}
			message = EPlayerMessage.NONE;
			return false;
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x0007FD8C File Offset: 0x0007DF8C
		private void Update()
		{
			if (!base.isWired)
			{
				return;
			}
			if (this.handHourTransform == null || this.handMinuteTransform == null)
			{
				return;
			}
			float num;
			if (LightingManager.day < LevelLighting.bias)
			{
				num = LightingManager.day / LevelLighting.bias;
			}
			else
			{
				num = (LightingManager.day - LevelLighting.bias) / (1f - LevelLighting.bias);
			}
			float num2 = num - 0.5f;
			float num3 = num * 12f;
			this.handHourTransform.localRotation = Quaternion.Euler(0f, num2 * -360f, 0f);
			this.handMinuteTransform.localRotation = Quaternion.Euler(0f, num3 * -360f, 0f);
		}

		// Token: 0x04001048 RID: 4168
		private Transform handHourTransform;

		// Token: 0x04001049 RID: 4169
		private Transform handMinuteTransform;
	}
}
