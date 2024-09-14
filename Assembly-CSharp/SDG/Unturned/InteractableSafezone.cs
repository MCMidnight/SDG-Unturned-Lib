using System;
using SDG.NetTransport;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200046A RID: 1130
	public class InteractableSafezone : InteractablePower
	{
		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06002272 RID: 8818 RVA: 0x0008533A File Offset: 0x0008353A
		public bool isPowered
		{
			get
			{
				return this._isPowered;
			}
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x00085342 File Offset: 0x00083542
		private void UpdateEngine()
		{
			if (this.engine != null)
			{
				this.engine.gameObject.SetActive(base.isWired && this.isPowered);
			}
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x00085373 File Offset: 0x00083573
		protected override void updateWired()
		{
			this.UpdateEngine();
			this.updateBubble();
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x00085381 File Offset: 0x00083581
		public void updatePowered(bool newPowered)
		{
			if (this._isPowered != newPowered)
			{
				this._isPowered = newPowered;
				this.UpdateEngine();
				this.updateBubble();
			}
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x0008539F File Offset: 0x0008359F
		private void updateBubble()
		{
			if (base.isWired && this.isPowered)
			{
				this.registerBubble();
				return;
			}
			this.deregisterBubble();
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x000853BE File Offset: 0x000835BE
		public override void updateState(Asset asset, byte[] state)
		{
			base.updateState(asset, state);
			this._isPowered = (state[0] == 1);
			this.engine = base.transform.Find("Engine");
			base.RefreshIsConnectedToPowerWithoutNotify();
			this.UpdateEngine();
			this.updateBubble();
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x000853FC File Offset: 0x000835FC
		public override void use()
		{
			this.ClientToggle();
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x00085404 File Offset: 0x00083604
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

		// Token: 0x0600227A RID: 8826 RVA: 0x0008542B File Offset: 0x0008362B
		private void registerBubble()
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (this.bubble != null)
			{
				return;
			}
			if (base.IsChildOfVehicle)
			{
				return;
			}
			this.bubble = SafezoneManager.registerBubble(base.transform.position, 24f);
		}

		// Token: 0x0600227B RID: 8827 RVA: 0x00085462 File Offset: 0x00083662
		private void deregisterBubble()
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (this.bubble == null)
			{
				return;
			}
			SafezoneManager.deregisterBubble(this.bubble);
			this.bubble = null;
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x00085487 File Offset: 0x00083687
		private void OnDisable()
		{
			this.deregisterBubble();
		}

		// Token: 0x0600227D RID: 8829 RVA: 0x0008548F File Offset: 0x0008368F
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceivePowered(bool newPowered)
		{
			this.updatePowered(newPowered);
		}

		// Token: 0x0600227E RID: 8830 RVA: 0x00085498 File Offset: 0x00083698
		public void ClientToggle()
		{
			InteractableSafezone.SendToggleRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, !this.isPowered);
		}

		// Token: 0x0600227F RID: 8831 RVA: 0x000854B4 File Offset: 0x000836B4
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
			BarricadeManager.ServerSetSafezonePoweredInternal(this, x, y, plant, region, !this.isPowered);
			EffectManager.TriggerFiremodeEffect(base.transform.position);
		}

		// Token: 0x040010EE RID: 4334
		private bool _isPowered;

		// Token: 0x040010EF RID: 4335
		private Transform engine;

		// Token: 0x040010F0 RID: 4336
		private SafezoneBubble bubble;

		// Token: 0x040010F1 RID: 4337
		internal static readonly ClientInstanceMethod<bool> SendPowered = ClientInstanceMethod<bool>.Get(typeof(InteractableSafezone), "ReceivePowered");

		// Token: 0x040010F2 RID: 4338
		private static readonly ServerInstanceMethod<bool> SendToggleRequest = ServerInstanceMethod<bool>.Get(typeof(InteractableSafezone), "ReceiveToggleRequest");
	}
}
