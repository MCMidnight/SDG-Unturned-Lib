using System;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000457 RID: 1111
	public class InteractableLibrary : Interactable
	{
		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x0600218E RID: 8590 RVA: 0x0008174A File Offset: 0x0007F94A
		public CSteamID owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x0600218F RID: 8591 RVA: 0x00081752 File Offset: 0x0007F952
		public CSteamID group
		{
			get
			{
				return this._group;
			}
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06002190 RID: 8592 RVA: 0x0008175A File Offset: 0x0007F95A
		public uint amount
		{
			get
			{
				return this._amount;
			}
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06002191 RID: 8593 RVA: 0x00081762 File Offset: 0x0007F962
		public uint capacity
		{
			get
			{
				return this._capacity;
			}
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06002192 RID: 8594 RVA: 0x0008176A File Offset: 0x0007F96A
		public byte tax
		{
			get
			{
				return this._tax;
			}
		}

		// Token: 0x06002193 RID: 8595 RVA: 0x00081772 File Offset: 0x0007F972
		public bool checkTransfer(CSteamID enemyPlayer, CSteamID enemyGroup)
		{
			bool isServer = Provider.isServer;
			return !this.isLocked || enemyPlayer == this.owner || (this.group != CSteamID.Nil && enemyGroup == this.group);
		}

		// Token: 0x06002194 RID: 8596 RVA: 0x000817B2 File Offset: 0x0007F9B2
		public void updateAmount(uint newAmount)
		{
			this._amount = newAmount;
		}

		// Token: 0x06002195 RID: 8597 RVA: 0x000817BC File Offset: 0x0007F9BC
		public override void updateState(Asset asset, byte[] state)
		{
			this.isLocked = ((ItemBarricadeAsset)asset).isLocked;
			this._capacity = ((ItemLibraryAsset)asset).capacity;
			this._tax = ((ItemLibraryAsset)asset).tax;
			this._owner = new CSteamID(BitConverter.ToUInt64(state, 0));
			this._group = new CSteamID(BitConverter.ToUInt64(state, 8));
			this._amount = BitConverter.ToUInt32(state, 16);
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x0008182E File Offset: 0x0007FA2E
		public override bool checkUseable()
		{
			return this.checkTransfer(Provider.client, Player.player.quests.groupID) && !PlayerUI.window.showCursor;
		}

		// Token: 0x06002197 RID: 8599 RVA: 0x0008185B File Offset: 0x0007FA5B
		public override void use()
		{
			PlayerBarricadeLibraryUI.open(this);
			PlayerLifeUI.close();
		}

		// Token: 0x06002198 RID: 8600 RVA: 0x00081868 File Offset: 0x0007FA68
		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			if (this.checkUseable())
			{
				message = EPlayerMessage.USE;
			}
			else
			{
				message = EPlayerMessage.LOCKED;
			}
			text = "";
			color = Color.white;
			return !PlayerUI.window.showCursor;
		}

		// Token: 0x06002199 RID: 8601 RVA: 0x0008189B File Offset: 0x0007FA9B
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceiveAmount(uint newAmount)
		{
			this.updateAmount(newAmount);
		}

		// Token: 0x0600219A RID: 8602 RVA: 0x000818A4 File Offset: 0x0007FAA4
		public void ClientTransfer(byte transaction, uint delta)
		{
			InteractableLibrary.SendTransferLibraryRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, transaction, delta);
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x000818BC File Offset: 0x0007FABC
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 2)]
		public void ReceiveTransferLibraryRequest(in ServerInvocationContext context, byte transaction, uint delta)
		{
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
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion barricadeRegion;
			if (!BarricadeManager.tryGetRegion(base.transform, out x, out y, out plant, out barricadeRegion))
			{
				return;
			}
			if (this.checkTransfer(player.channel.owner.playerID.steamID, player.quests.groupID))
			{
				uint num3;
				if (transaction == 0)
				{
					uint num = (uint)Math.Ceiling(delta * ((double)this.tax / 100.0));
					uint num2 = delta - num;
					if (delta > player.skills.experience || num2 + this.amount > this.capacity)
					{
						return;
					}
					num3 = this.amount + num2;
					player.skills.askSpend(delta);
				}
				else
				{
					if (delta > this.amount)
					{
						return;
					}
					num3 = this.amount - delta;
					player.skills.askAward(delta);
				}
				InteractableLibrary.SendAmount.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), num3);
				BarricadeDrop barricadeDrop = barricadeRegion.FindBarricadeByRootFast(base.transform);
				Buffer.BlockCopy(BitConverter.GetBytes(num3), 0, barricadeDrop.serversideData.barricade.state, 16, 4);
			}
		}

		// Token: 0x04001078 RID: 4216
		private CSteamID _owner;

		// Token: 0x04001079 RID: 4217
		private CSteamID _group;

		// Token: 0x0400107A RID: 4218
		private uint _amount;

		// Token: 0x0400107B RID: 4219
		private uint _capacity;

		// Token: 0x0400107C RID: 4220
		private byte _tax;

		// Token: 0x0400107D RID: 4221
		private bool isLocked;

		// Token: 0x0400107E RID: 4222
		private static readonly ClientInstanceMethod<uint> SendAmount = ClientInstanceMethod<uint>.Get(typeof(InteractableLibrary), "ReceiveAmount");

		// Token: 0x0400107F RID: 4223
		private static readonly ServerInstanceMethod<byte, uint> SendTransferLibraryRequest = ServerInstanceMethod<byte, uint>.Get(typeof(InteractableLibrary), "ReceiveTransferLibraryRequest");
	}
}
