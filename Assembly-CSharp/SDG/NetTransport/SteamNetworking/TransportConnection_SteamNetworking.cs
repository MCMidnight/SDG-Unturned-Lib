using System;
using System.Net;
using SDG.Unturned;
using Steamworks;

namespace SDG.NetTransport.SteamNetworking
{
	// Token: 0x02000074 RID: 116
	internal struct TransportConnection_SteamNetworking : ITransportConnection, IEquatable<ITransportConnection>
	{
		// Token: 0x060002B8 RID: 696 RVA: 0x0000B72B File Offset: 0x0000992B
		public TransportConnection_SteamNetworking(CSteamID steamId)
		{
			this.steamId = steamId;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000B734 File Offset: 0x00009934
		public bool TryGetIPv4Address(out uint address)
		{
			P2PSessionState_t p2PSessionState_t;
			if (SteamGameServerNetworking.GetP2PSessionState(this.steamId, out p2PSessionState_t) && p2PSessionState_t.m_bUsingRelay == 0)
			{
				address = p2PSessionState_t.m_nRemoteIP;
				return true;
			}
			address = 0U;
			return false;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000B768 File Offset: 0x00009968
		public bool TryGetPort(out ushort port)
		{
			P2PSessionState_t p2PSessionState_t;
			if (SteamGameServerNetworking.GetP2PSessionState(this.steamId, out p2PSessionState_t) && p2PSessionState_t.m_bUsingRelay == 0)
			{
				port = p2PSessionState_t.m_nRemotePort;
				return true;
			}
			port = 0;
			return false;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000B79C File Offset: 0x0000999C
		public IPAddress GetAddress()
		{
			P2PSessionState_t p2PSessionState_t;
			if (SteamGameServerNetworking.GetP2PSessionState(this.steamId, out p2PSessionState_t) && p2PSessionState_t.m_bUsingRelay == 0)
			{
				return new IPAddress((long)((ulong)p2PSessionState_t.m_nRemoteIP));
			}
			return null;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000B7D0 File Offset: 0x000099D0
		public string GetAddressString(bool withPort)
		{
			P2PSessionState_t p2PSessionState_t;
			if (SteamGameServerNetworking.GetP2PSessionState(this.steamId, out p2PSessionState_t) && p2PSessionState_t.m_bUsingRelay == 0)
			{
				string text = Parser.getIPFromUInt32(p2PSessionState_t.m_nRemoteIP);
				if (withPort)
				{
					text += ":";
					text += p2PSessionState_t.m_nRemotePort.ToString();
				}
				return text;
			}
			return null;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000B825 File Offset: 0x00009A25
		public void CloseConnection()
		{
			SteamGameServerNetworking.CloseP2PSessionWithUser(this.steamId);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000B834 File Offset: 0x00009A34
		public void Send(byte[] buffer, long size, ENetReliability reliability)
		{
			if (Provider.shouldNetIgnoreSteamId(this.steamId))
			{
				return;
			}
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
			SteamGameServerNetworking.SendP2PPacket(this.steamId, buffer, (uint)size, eP2PSendType, 0);
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000B86F File Offset: 0x00009A6F
		public override bool Equals(object obj)
		{
			return obj is TransportConnection_SteamNetworking && this.steamId == ((TransportConnection_SteamNetworking)obj).steamId;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000B891 File Offset: 0x00009A91
		public bool Equals(TransportConnection_SteamNetworking other)
		{
			return this.steamId == other.steamId;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000B8A4 File Offset: 0x00009AA4
		public bool Equals(ITransportConnection other)
		{
			return other is TransportConnection_SteamNetworking && this.steamId == ((TransportConnection_SteamNetworking)other).steamId;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000B8C6 File Offset: 0x00009AC6
		public override int GetHashCode()
		{
			return this.steamId.GetHashCode();
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000B8D9 File Offset: 0x00009AD9
		public override string ToString()
		{
			return this.steamId.ToString();
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000B8EC File Offset: 0x00009AEC
		public static implicit operator CSteamID(TransportConnection_SteamNetworking clientId)
		{
			return clientId.steamId;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000B8F4 File Offset: 0x00009AF4
		public static bool operator ==(TransportConnection_SteamNetworking lhs, TransportConnection_SteamNetworking rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000B8FE File Offset: 0x00009AFE
		public static bool operator !=(TransportConnection_SteamNetworking lhs, TransportConnection_SteamNetworking rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0400013F RID: 319
		public CSteamID steamId;
	}
}
