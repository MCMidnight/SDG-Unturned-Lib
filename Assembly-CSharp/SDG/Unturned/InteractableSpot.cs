using System;
using SDG.NetTransport;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200046D RID: 1133
	public class InteractableSpot : InteractablePower
	{
		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x060022A9 RID: 8873 RVA: 0x00087969 File Offset: 0x00085B69
		public bool isPowered
		{
			get
			{
				return this._isPowered;
			}
		}

		// Token: 0x060022AA RID: 8874 RVA: 0x00087974 File Offset: 0x00085B74
		private void updateLights()
		{
			bool flag = base.isWired && this.isPowered;
			if (this.material != null)
			{
				this.material.SetColor("_EmissionColor", flag ? new Color(2f, 2f, 2f) : Color.black);
			}
			if (this.spot != null)
			{
				this.spot.gameObject.SetActive(flag);
			}
		}

		// Token: 0x060022AB RID: 8875 RVA: 0x000879EE File Offset: 0x00085BEE
		protected override void updateWired()
		{
			this.updateLights();
		}

		// Token: 0x060022AC RID: 8876 RVA: 0x000879F6 File Offset: 0x00085BF6
		public void updatePowered(bool newPowered)
		{
			if (this._isPowered != newPowered)
			{
				this._isPowered = newPowered;
				this.updateLights();
			}
		}

		// Token: 0x060022AD RID: 8877 RVA: 0x00087A0E File Offset: 0x00085C0E
		public override void updateState(Asset asset, byte[] state)
		{
			base.updateState(asset, state);
			this._isPowered = (state[0] == 1);
			base.RefreshIsConnectedToPowerWithoutNotify();
			this.updateLights();
		}

		// Token: 0x060022AE RID: 8878 RVA: 0x00087A30 File Offset: 0x00085C30
		public override void use()
		{
			this.ClientToggle();
		}

		// Token: 0x060022AF RID: 8879 RVA: 0x00087A38 File Offset: 0x00085C38
		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			if (this.isPowered)
			{
				message = EPlayerMessage.SPOT_OFF;
			}
			else
			{
				message = EPlayerMessage.SPOT_ON;
			}
			text = "";
			color = Color.white;
			return true;
		}

		// Token: 0x060022B0 RID: 8880 RVA: 0x00087A5F File Offset: 0x00085C5F
		private void OnDestroy()
		{
			if (this.material != null)
			{
				Object.DestroyImmediate(this.material);
				this.material = null;
			}
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x00087A81 File Offset: 0x00085C81
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceivePowered(bool newPowered)
		{
			this.updatePowered(newPowered);
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x00087A8A File Offset: 0x00085C8A
		public void ClientToggle()
		{
			InteractableSpot.SendToggleRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, !this.isPowered);
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x00087AA8 File Offset: 0x00085CA8
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 2)]
		public void ReceiveToggleRequest(in ServerInvocationContext context, bool desiredPowered)
		{
			if (this.isPowered == desiredPowered)
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
			BarricadeManager.ServerSetSpotPoweredInternal(this, x, y, plant, region, !this.isPowered);
			EffectManager.TriggerFiremodeEffect(base.transform.position);
		}

		// Token: 0x04001129 RID: 4393
		private bool _isPowered;

		// Token: 0x0400112A RID: 4394
		private Material material;

		// Token: 0x0400112B RID: 4395
		private Transform spot;

		// Token: 0x0400112C RID: 4396
		internal static readonly ClientInstanceMethod<bool> SendPowered = ClientInstanceMethod<bool>.Get(typeof(InteractableSpot), "ReceivePowered");

		// Token: 0x0400112D RID: 4397
		private static readonly ServerInstanceMethod<bool> SendToggleRequest = ServerInstanceMethod<bool>.Get(typeof(InteractableSpot), "ReceiveToggleRequest");
	}
}
