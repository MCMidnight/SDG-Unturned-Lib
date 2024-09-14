using System;
using SDG.NetTransport;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000452 RID: 1106
	public class InteractableFire : Interactable
	{
		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06002157 RID: 8535 RVA: 0x00080A6C File Offset: 0x0007EC6C
		public bool isLit
		{
			get
			{
				return this._isLit;
			}
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x00080A74 File Offset: 0x0007EC74
		private void updateFire()
		{
			if (this.material != null)
			{
				this.material.SetColor("_EmissionColor", this.isLit ? new Color(2f, 2f, 2f) : Color.black);
			}
			if (this.fire != null)
			{
				this.fire.gameObject.SetActive(this.isLit);
			}
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x00080AE6 File Offset: 0x0007ECE6
		public void updateLit(bool newLit)
		{
			this._isLit = newLit;
			this.updateFire();
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x00080AF8 File Offset: 0x0007ECF8
		public override void updateState(Asset asset, byte[] state)
		{
			this._isLit = (state[0] == 1);
			if (this.material == null)
			{
				this.material = HighlighterTool.getMaterialInstance(base.transform);
			}
			if (this.fire == null)
			{
				this.fire = base.transform.Find("Fire");
				LightLODTool.applyLightLOD(this.fire);
			}
			this.updateFire();
		}

		// Token: 0x0600215B RID: 8539 RVA: 0x00080B65 File Offset: 0x0007ED65
		public override void use()
		{
			this.ClientToggle();
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x00080B6D File Offset: 0x0007ED6D
		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			if (this.isLit)
			{
				message = EPlayerMessage.FIRE_OFF;
			}
			else
			{
				message = EPlayerMessage.FIRE_ON;
			}
			text = "";
			color = Color.white;
			return true;
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x00080B94 File Offset: 0x0007ED94
		private void OnDestroy()
		{
			if (this.material != null)
			{
				Object.DestroyImmediate(this.material);
				this.material = null;
			}
		}

		// Token: 0x0600215E RID: 8542 RVA: 0x00080BB6 File Offset: 0x0007EDB6
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceiveLit(bool newLit)
		{
			this.updateLit(newLit);
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x00080BBF File Offset: 0x0007EDBF
		public void ClientToggle()
		{
			InteractableFire.SendToggleRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, !this.isLit);
		}

		// Token: 0x06002160 RID: 8544 RVA: 0x00080BDC File Offset: 0x0007EDDC
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 2)]
		public void ReceiveToggleRequest(in ServerInvocationContext context, bool desiredLit)
		{
			if (this.isLit == desiredLit)
			{
				return;
			}
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion region;
			if (!BarricadeManager.tryGetRegion(base.transform, out x, out y, out plant, out region))
			{
				return;
			}
			Player player = context.GetPlayer();
			if (player == null)
			{
				return;
			}
			if (player.life.isDead)
			{
				return;
			}
			if ((base.transform.position - player.transform.position).sqrMagnitude > 400f)
			{
				return;
			}
			BarricadeManager.ServerSetFireLitInternal(this, x, y, plant, region, !this.isLit);
		}

		// Token: 0x0400105F RID: 4191
		private bool _isLit;

		// Token: 0x04001060 RID: 4192
		private Material material;

		// Token: 0x04001061 RID: 4193
		private Transform fire;

		// Token: 0x04001062 RID: 4194
		internal static readonly ClientInstanceMethod<bool> SendLit = ClientInstanceMethod<bool>.Get(typeof(InteractableFire), "ReceiveLit");

		// Token: 0x04001063 RID: 4195
		private static readonly ServerInstanceMethod<bool> SendToggleRequest = ServerInstanceMethod<bool>.Get(typeof(InteractableFire), "ReceiveToggleRequest");
	}
}
