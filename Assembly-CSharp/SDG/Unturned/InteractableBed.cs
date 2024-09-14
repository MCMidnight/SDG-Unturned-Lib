using System;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200044B RID: 1099
	public class InteractableBed : Interactable
	{
		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x060020FE RID: 8446 RVA: 0x0007F638 File Offset: 0x0007D838
		public CSteamID owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x060020FF RID: 8447 RVA: 0x0007F640 File Offset: 0x0007D840
		public bool isClaimed
		{
			get
			{
				return this.owner != CSteamID.Nil;
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x06002100 RID: 8448 RVA: 0x0007F652 File Offset: 0x0007D852
		public bool isClaimable
		{
			get
			{
				return Time.realtimeSinceStartup - this.claimed > 0.75f;
			}
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x0007F667 File Offset: 0x0007D867
		public bool checkClaim(CSteamID enemy)
		{
			bool isServer = Provider.isServer;
			return !this.isClaimed || enemy == this.owner;
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x0007F685 File Offset: 0x0007D885
		public void updateClaim(CSteamID newOwner)
		{
			this.claimed = Time.realtimeSinceStartup;
			this._owner = newOwner;
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x0007F699 File Offset: 0x0007D899
		public override void updateState(Asset asset, byte[] state)
		{
			this._owner = new CSteamID(BitConverter.ToUInt64(state, 0));
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x0007F6AD File Offset: 0x0007D8AD
		public override bool checkUseable()
		{
			return this.checkClaim(Provider.client);
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x0007F6BA File Offset: 0x0007D8BA
		public override void use()
		{
			this.ClientClaim();
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x0007F6C2 File Offset: 0x0007D8C2
		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			text = "";
			color = Color.white;
			if (this.checkUseable())
			{
				if (this.isClaimed)
				{
					message = EPlayerMessage.BED_OFF;
				}
				else
				{
					message = EPlayerMessage.BED_ON;
				}
			}
			else
			{
				message = EPlayerMessage.BED_CLAIMED;
			}
			return true;
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x0007F6F7 File Offset: 0x0007D8F7
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceiveClaim(CSteamID newOwner)
		{
			this.updateClaim(newOwner);
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x0007F700 File Offset: 0x0007D900
		public void ClientClaim()
		{
			InteractableBed.SendClaimRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable);
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x0007F714 File Offset: 0x0007D914
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 1)]
		public void ReceiveClaimRequest(in ServerInvocationContext context)
		{
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
			if (this.isClaimable && this.checkClaim(player.channel.owner.playerID.steamID))
			{
				if (this.isClaimed)
				{
					BarricadeManager.ServerSetBedOwnerInternal(this, x, y, plant, region, CSteamID.Nil);
					return;
				}
				BarricadeManager.unclaimBeds(player.channel.owner.playerID.steamID);
				BarricadeManager.ServerSetBedOwnerInternal(this, x, y, plant, region, player.channel.owner.playerID.steamID);
			}
		}

		// Token: 0x0400102F RID: 4143
		private CSteamID _owner;

		// Token: 0x04001030 RID: 4144
		private float claimed;

		// Token: 0x04001031 RID: 4145
		internal static readonly ClientInstanceMethod<CSteamID> SendClaim = ClientInstanceMethod<CSteamID>.Get(typeof(InteractableBed), "ReceiveClaim");

		// Token: 0x04001032 RID: 4146
		private static readonly ServerInstanceMethod SendClaimRequest = ServerInstanceMethod.Get(typeof(InteractableBed), "ReceiveClaimRequest");
	}
}
