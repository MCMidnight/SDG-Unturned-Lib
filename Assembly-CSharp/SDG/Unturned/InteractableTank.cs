using System;
using SDG.NetTransport;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000471 RID: 1137
	public class InteractableTank : Interactable
	{
		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x060022E4 RID: 8932 RVA: 0x00088EAD File Offset: 0x000870AD
		public ETankSource source
		{
			get
			{
				return this._source;
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x060022E5 RID: 8933 RVA: 0x00088EB5 File Offset: 0x000870B5
		public ushort amount
		{
			get
			{
				return this._amount;
			}
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x060022E6 RID: 8934 RVA: 0x00088EBD File Offset: 0x000870BD
		public ushort capacity
		{
			get
			{
				return this._capacity;
			}
		}

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x060022E7 RID: 8935 RVA: 0x00088EC5 File Offset: 0x000870C5
		public bool isRefillable
		{
			get
			{
				return this.amount < this.capacity;
			}
		}

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x060022E8 RID: 8936 RVA: 0x00088ED5 File Offset: 0x000870D5
		public bool isSiphonable
		{
			get
			{
				return this.amount > 0;
			}
		}

		// Token: 0x060022E9 RID: 8937 RVA: 0x00088EE0 File Offset: 0x000870E0
		public void updateAmount(ushort newAmount)
		{
			this._amount = newAmount;
		}

		// Token: 0x060022EA RID: 8938 RVA: 0x00088EE9 File Offset: 0x000870E9
		public override void updateState(Asset asset, byte[] state)
		{
			this._amount = BitConverter.ToUInt16(state, 0);
			this._capacity = ((ItemTankAsset)asset).resource;
			this._source = ((ItemTankAsset)asset).source;
		}

		// Token: 0x060022EB RID: 8939 RVA: 0x00088F1A File Offset: 0x0008711A
		public override bool checkUseable()
		{
			return this.amount > 0;
		}

		// Token: 0x060022EC RID: 8940 RVA: 0x00088F28 File Offset: 0x00087128
		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			if (this.source == ETankSource.WATER)
			{
				message = EPlayerMessage.VOLUME_WATER;
				text = this.amount.ToString() + "/" + this.capacity.ToString();
			}
			else
			{
				message = EPlayerMessage.VOLUME_FUEL;
				text = "";
			}
			color = Color.white;
			return true;
		}

		// Token: 0x060022ED RID: 8941 RVA: 0x00088F83 File Offset: 0x00087183
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceiveAmount(ushort newAmount)
		{
			this.updateAmount(newAmount);
		}

		// Token: 0x060022EE RID: 8942 RVA: 0x00088F8C File Offset: 0x0008718C
		public void ServerSetAmount(ushort newAmount)
		{
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion barricadeRegion;
			if (BarricadeManager.tryGetRegion(base.transform, out x, out y, out plant, out barricadeRegion))
			{
				InteractableTank.SendAmount.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), newAmount);
				BarricadeDrop barricadeDrop = barricadeRegion.FindBarricadeByRootFast(base.transform);
				byte[] bytes = BitConverter.GetBytes(newAmount);
				barricadeDrop.serversideData.barricade.state[0] = bytes[0];
				barricadeDrop.serversideData.barricade.state[1] = bytes[1];
			}
		}

		// Token: 0x04001153 RID: 4435
		private ETankSource _source;

		// Token: 0x04001154 RID: 4436
		private ushort _amount;

		// Token: 0x04001155 RID: 4437
		private ushort _capacity;

		// Token: 0x04001156 RID: 4438
		private static readonly ClientInstanceMethod<ushort> SendAmount = ClientInstanceMethod<ushort>.Get(typeof(InteractableTank), "ReceiveAmount");
	}
}
