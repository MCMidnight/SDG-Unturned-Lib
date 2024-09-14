using System;
using SDG.NetTransport;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000466 RID: 1126
	public class InteractableOven : InteractablePower
	{
		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06002244 RID: 8772 RVA: 0x00084CE8 File Offset: 0x00082EE8
		public bool isLit
		{
			get
			{
				return this._isLit;
			}
		}

		// Token: 0x06002245 RID: 8773 RVA: 0x00084CF0 File Offset: 0x00082EF0
		private void UpdateVisual()
		{
			if (this.fire != null)
			{
				this.fire.gameObject.SetActive(base.isWired && this.isLit);
			}
		}

		// Token: 0x06002246 RID: 8774 RVA: 0x00084D21 File Offset: 0x00082F21
		protected override void updateWired()
		{
			this.UpdateVisual();
		}

		// Token: 0x06002247 RID: 8775 RVA: 0x00084D29 File Offset: 0x00082F29
		public void updateLit(bool newLit)
		{
			if (this._isLit != newLit)
			{
				this._isLit = newLit;
				this.UpdateVisual();
			}
		}

		// Token: 0x06002248 RID: 8776 RVA: 0x00084D44 File Offset: 0x00082F44
		public override void updateState(Asset asset, byte[] state)
		{
			this._isLit = (state[0] == 1);
			if (this.fire == null)
			{
				this.fire = base.transform.Find("Fire");
				LightLODTool.applyLightLOD(this.fire);
			}
			base.RefreshIsConnectedToPowerWithoutNotify();
			this.UpdateVisual();
		}

		// Token: 0x06002249 RID: 8777 RVA: 0x00084D98 File Offset: 0x00082F98
		public override void use()
		{
			this.ClientToggle();
		}

		// Token: 0x0600224A RID: 8778 RVA: 0x00084DA0 File Offset: 0x00082FA0
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

		// Token: 0x0600224B RID: 8779 RVA: 0x00084DC7 File Offset: 0x00082FC7
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceiveLit(bool newLit)
		{
			this.updateLit(newLit);
		}

		// Token: 0x0600224C RID: 8780 RVA: 0x00084DD0 File Offset: 0x00082FD0
		public void ClientToggle()
		{
			InteractableOven.SendToggleRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, !this.isLit);
		}

		// Token: 0x0600224D RID: 8781 RVA: 0x00084DEC File Offset: 0x00082FEC
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
			BarricadeManager.ServerSetOvenLitInternal(this, x, y, plant, region, !this.isLit);
		}

		// Token: 0x040010E2 RID: 4322
		private bool _isLit;

		// Token: 0x040010E3 RID: 4323
		private Transform fire;

		// Token: 0x040010E4 RID: 4324
		internal static readonly ClientInstanceMethod<bool> SendLit = ClientInstanceMethod<bool>.Get(typeof(InteractableOven), "ReceiveLit");

		// Token: 0x040010E5 RID: 4325
		private static readonly ServerInstanceMethod<bool> SendToggleRequest = ServerInstanceMethod<bool>.Get(typeof(InteractableOven), "ReceiveToggleRequest");
	}
}
