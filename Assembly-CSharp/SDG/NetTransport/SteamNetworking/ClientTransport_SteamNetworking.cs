using System;
using SDG.Unturned;
using Steamworks;
using Unturned.SystemEx;

namespace SDG.NetTransport.SteamNetworking
{
	/// <summary>
	/// SteamNetworking is deprecated.
	/// </summary>
	// Token: 0x02000071 RID: 113
	public class ClientTransport_SteamNetworking : TransportBase_SteamNetworking, IClientTransport
	{
		// Token: 0x060002A7 RID: 679 RVA: 0x0000B4E0 File Offset: 0x000096E0
		public void Initialize(ClientTransportReady callback, ClientTransportFailure failureCallback)
		{
			ClientTransport_SteamNetworking.p2pSessionRequest = Callback<P2PSessionRequest_t>.Create(new Callback<P2PSessionRequest_t>.DispatchDelegate(this.OnP2PSessionRequest));
			callback();
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000B4FE File Offset: 0x000096FE
		public void TearDown()
		{
			ClientTransport_SteamNetworking.p2pSessionRequest.Dispose();
			SteamNetworking.CloseP2PSessionWithUser(this.serverId);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000B518 File Offset: 0x00009718
		public void Send(byte[] buffer, long size, ENetReliability reliability)
		{
			EP2PSend eP2PSendType;
			if (reliability != ENetReliability.Reliable)
			{
				if (reliability != ENetReliability.Unreliable)
				{
				}
				eP2PSendType = EP2PSend.k_EP2PSendUnreliable;
			}
			else
			{
				eP2PSendType = EP2PSend.k_EP2PSendReliableWithBuffering;
			}
			SteamNetworking.SendP2PPacket(this.serverId, buffer, (uint)size, eP2PSendType, 0);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000B548 File Offset: 0x00009748
		public bool Receive(byte[] buffer, out long size)
		{
			size = 0L;
			int nChannel = 0;
			uint num;
			CSteamID x;
			if (!SteamNetworking.ReadP2PPacket(buffer, (uint)buffer.Length, out num, out x, nChannel))
			{
				return false;
			}
			if (x != this.serverId)
			{
				return false;
			}
			size = (long)((ulong)num);
			return true;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000B584 File Offset: 0x00009784
		public bool TryGetIPv4Address(out IPv4Address address)
		{
			P2PSessionState_t p2PSessionState_t;
			if (SteamNetworking.GetP2PSessionState(this.serverId, out p2PSessionState_t) && p2PSessionState_t.m_bUsingRelay == 0)
			{
				address = new IPv4Address(p2PSessionState_t.m_nRemoteIP);
				return address.value > 0U;
			}
			address = IPv4Address.Zero;
			return false;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000B5D0 File Offset: 0x000097D0
		public bool TryGetConnectionPort(out ushort connectionPort)
		{
			P2PSessionState_t p2PSessionState_t;
			if (SteamNetworking.GetP2PSessionState(this.serverId, out p2PSessionState_t) && p2PSessionState_t.m_bUsingRelay == 0)
			{
				connectionPort = p2PSessionState_t.m_nRemotePort;
				return connectionPort > 0;
			}
			connectionPort = 0;
			return false;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000B608 File Offset: 0x00009808
		public bool TryGetQueryPort(out ushort queryPort)
		{
			P2PSessionState_t p2PSessionState_t;
			if (SteamNetworking.GetP2PSessionState(this.serverId, out p2PSessionState_t) && p2PSessionState_t.m_bUsingRelay == 0)
			{
				queryPort = MathfEx.ClampToUShort((int)(p2PSessionState_t.m_nRemotePort - 1));
				return queryPort > 0;
			}
			queryPort = 0;
			return false;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000B645 File Offset: 0x00009845
		private void OnP2PSessionRequest(P2PSessionRequest_t callback)
		{
			if (callback.m_steamIDRemote == this.serverId)
			{
				SteamNetworking.AcceptP2PSessionWithUser(this.serverId);
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000B666 File Offset: 0x00009866
		private CSteamID serverId
		{
			get
			{
				return Provider.server;
			}
		}

		// Token: 0x0400013D RID: 317
		private static Callback<P2PSessionRequest_t> p2pSessionRequest;
	}
}
