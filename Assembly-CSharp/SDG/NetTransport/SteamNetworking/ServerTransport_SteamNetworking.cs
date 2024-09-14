using System;
using SDG.Unturned;
using Steamworks;

namespace SDG.NetTransport.SteamNetworking
{
	/// <summary>
	/// SteamNetworking is deprecated.
	/// </summary>
	// Token: 0x02000072 RID: 114
	public class ServerTransport_SteamNetworking : TransportBase_SteamNetworking, IServerTransport
	{
		// Token: 0x060002B1 RID: 689 RVA: 0x0000B675 File Offset: 0x00009875
		public void Initialize(ServerTransportConnectionFailureCallback connectionClosedCallback)
		{
			this.p2pSessionRequest = Callback<P2PSessionRequest_t>.CreateGameServer(new Callback<P2PSessionRequest_t>.DispatchDelegate(this.OnP2PSessionRequest));
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000B68E File Offset: 0x0000988E
		public void TearDown()
		{
			this.p2pSessionRequest.Dispose();
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000B69C File Offset: 0x0000989C
		public bool Receive(byte[] buffer, out long size, out ITransportConnection transportConnection)
		{
			transportConnection = null;
			size = 0L;
			int nChannel = 0;
			uint num;
			CSteamID steamId;
			if (!SteamGameServerNetworking.ReadP2PPacket(buffer, (uint)buffer.Length, out num, out steamId, nChannel))
			{
				return false;
			}
			if ((ulong)num > (ulong)((long)buffer.Length))
			{
				num = (uint)buffer.Length;
			}
			size = (long)((ulong)num);
			transportConnection = new TransportConnection_SteamNetworking(steamId);
			return true;
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000B6E4 File Offset: 0x000098E4
		private void OnP2PSessionRequest(P2PSessionRequest_t callback)
		{
			CSteamID steamIDRemote = callback.m_steamIDRemote;
			if (Provider.shouldNetIgnoreSteamId(steamIDRemote))
			{
				return;
			}
			if (!steamIDRemote.BIndividualAccount())
			{
				return;
			}
			SteamGameServerNetworking.AcceptP2PSessionWithUser(steamIDRemote);
		}

		// Token: 0x0400013E RID: 318
		private Callback<P2PSessionRequest_t> p2pSessionRequest;
	}
}
