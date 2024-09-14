using System;

namespace SDG.Unturned
{
	// Token: 0x02000607 RID: 1543
	public class PlayerCaller : SteamCaller
	{
		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x0600312D RID: 12589 RVA: 0x000D9DA2 File Offset: 0x000D7FA2
		public Player player
		{
			get
			{
				return this._player;
			}
		}

		// Token: 0x0600312E RID: 12590 RVA: 0x000D9DAA File Offset: 0x000D7FAA
		public NetId GetNetId()
		{
			return this._netId;
		}

		// Token: 0x0600312F RID: 12591 RVA: 0x000D9DB2 File Offset: 0x000D7FB2
		internal void AssignNetId(NetId netId)
		{
			this._netId = netId;
			NetIdRegistry.Assign(netId, this);
		}

		// Token: 0x06003130 RID: 12592 RVA: 0x000D9DC2 File Offset: 0x000D7FC2
		internal void ReleaseNetId()
		{
			NetIdRegistry.Release(this._netId);
			this._netId.Clear();
		}

		// Token: 0x06003131 RID: 12593 RVA: 0x000D9DDB File Offset: 0x000D7FDB
		private void Awake()
		{
			this._channel = base.GetComponent<SteamChannel>();
			this._player = base.GetComponent<Player>();
		}

		// Token: 0x04001C1B RID: 7195
		protected Player _player;

		// Token: 0x04001C1C RID: 7196
		internal NetId _netId;
	}
}
