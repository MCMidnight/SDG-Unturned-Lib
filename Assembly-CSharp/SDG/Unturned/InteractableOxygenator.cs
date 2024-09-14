using System;
using SDG.NetTransport;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000467 RID: 1127
	public class InteractableOxygenator : InteractablePower
	{
		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06002250 RID: 8784 RVA: 0x00084EB7 File Offset: 0x000830B7
		public bool isPowered
		{
			get
			{
				return this._isPowered;
			}
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x00084EBF File Offset: 0x000830BF
		private void UpdateEngine()
		{
			if (this.engine != null)
			{
				this.engine.gameObject.SetActive(base.isWired && this.isPowered);
			}
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x00084EF0 File Offset: 0x000830F0
		protected override void updateWired()
		{
			this.UpdateEngine();
			this.updateBubble();
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x00084EFE File Offset: 0x000830FE
		public void updatePowered(bool newPowered)
		{
			if (this._isPowered != newPowered)
			{
				this._isPowered = newPowered;
				this.UpdateEngine();
				this.updateBubble();
			}
		}

		// Token: 0x06002254 RID: 8788 RVA: 0x00084F1C File Offset: 0x0008311C
		private void updateBubble()
		{
			if (base.isWired && this.isPowered)
			{
				this.registerBubble();
				return;
			}
			this.deregisterBubble();
		}

		// Token: 0x06002255 RID: 8789 RVA: 0x00084F3B File Offset: 0x0008313B
		public override void updateState(Asset asset, byte[] state)
		{
			base.updateState(asset, state);
			this._isPowered = (state[0] == 1);
			this.engine = base.transform.Find("Engine");
			base.RefreshIsConnectedToPowerWithoutNotify();
			this.UpdateEngine();
			this.updateBubble();
		}

		// Token: 0x06002256 RID: 8790 RVA: 0x00084F79 File Offset: 0x00083179
		public override void use()
		{
			this.ClientToggle();
		}

		// Token: 0x06002257 RID: 8791 RVA: 0x00084F81 File Offset: 0x00083181
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

		// Token: 0x06002258 RID: 8792 RVA: 0x00084FA8 File Offset: 0x000831A8
		private void registerBubble()
		{
			if (this.bubble == null)
			{
				this.bubble = OxygenManager.registerBubble(base.transform, 24f);
			}
		}

		// Token: 0x06002259 RID: 8793 RVA: 0x00084FC8 File Offset: 0x000831C8
		private void deregisterBubble()
		{
			if (this.bubble != null)
			{
				OxygenManager.deregisterBubble(this.bubble);
				this.bubble = null;
			}
		}

		// Token: 0x0600225A RID: 8794 RVA: 0x00084FE4 File Offset: 0x000831E4
		private void OnDisable()
		{
			this.deregisterBubble();
		}

		// Token: 0x0600225B RID: 8795 RVA: 0x00084FEC File Offset: 0x000831EC
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceivePowered(bool newPowered)
		{
			this.updatePowered(newPowered);
		}

		// Token: 0x0600225C RID: 8796 RVA: 0x00084FF5 File Offset: 0x000831F5
		public void ClientToggle()
		{
			InteractableOxygenator.SendToggleRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, !this.isPowered);
		}

		// Token: 0x0600225D RID: 8797 RVA: 0x00085014 File Offset: 0x00083214
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
			BarricadeManager.ServerSetOxygenatorPoweredInternal(this, x, y, plant, region, !this.isPowered);
			EffectManager.TriggerFiremodeEffect(base.transform.position);
		}

		// Token: 0x040010E6 RID: 4326
		private bool _isPowered;

		// Token: 0x040010E7 RID: 4327
		private Transform engine;

		// Token: 0x040010E8 RID: 4328
		private OxygenBubble bubble;

		// Token: 0x040010E9 RID: 4329
		internal static readonly ClientInstanceMethod<bool> SendPowered = ClientInstanceMethod<bool>.Get(typeof(InteractableOxygenator), "ReceivePowered");

		// Token: 0x040010EA RID: 4330
		private static readonly ServerInstanceMethod<bool> SendToggleRequest = ServerInstanceMethod<bool>.Get(typeof(InteractableOxygenator), "ReceiveToggleRequest");
	}
}
