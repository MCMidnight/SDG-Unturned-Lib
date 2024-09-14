using System;
using System.Net;
using Steamworks;

namespace SDG.NetTransport.SteamNetworkingSockets
{
	/// <summary>
	/// Implementing as a struct wrapping the connection handle would remove the cost of looking up the connection,
	/// but implementing as a class makes it cheap to cache information like the remote identity.
	/// </summary>
	// Token: 0x02000070 RID: 112
	internal class TransportConnection_SteamNetworkingSockets : ITransportConnection, IEquatable<ITransportConnection>
	{
		// Token: 0x0600029B RID: 667 RVA: 0x0000B333 File Offset: 0x00009533
		public TransportConnection_SteamNetworkingSockets(ServerTransport_SteamNetworkingSockets serverTransport, ref SteamNetConnectionStatusChangedCallback_t callback)
		{
			this.serverTransport = serverTransport;
			this.steamConnectionHandle = callback.m_hConn;
			this.steamIdentity = callback.m_info.m_identityRemote;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000B360 File Offset: 0x00009560
		public bool TryGetIPv4Address(out uint address)
		{
			SteamNetConnectionInfo_t steamNetConnectionInfo_t;
			if (SteamGameServerNetworkingSockets.GetConnectionInfo(this.steamConnectionHandle, out steamNetConnectionInfo_t))
			{
				address = steamNetConnectionInfo_t.m_addrRemote.GetIPv4();
				return address > 0U;
			}
			address = 0U;
			return false;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000B394 File Offset: 0x00009594
		public bool TryGetPort(out ushort port)
		{
			SteamNetConnectionInfo_t steamNetConnectionInfo_t;
			if (SteamGameServerNetworkingSockets.GetConnectionInfo(this.steamConnectionHandle, out steamNetConnectionInfo_t))
			{
				port = steamNetConnectionInfo_t.m_addrRemote.m_port;
				return port > 0;
			}
			port = 0;
			return false;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000B3C8 File Offset: 0x000095C8
		public IPAddress GetAddress()
		{
			SteamNetConnectionInfo_t steamNetConnectionInfo_t;
			if (SteamGameServerNetworkingSockets.GetConnectionInfo(this.steamConnectionHandle, out steamNetConnectionInfo_t))
			{
				return new IPAddress(steamNetConnectionInfo_t.m_addrRemote.m_ipv6);
			}
			return null;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000B3F8 File Offset: 0x000095F8
		public string GetAddressString(bool withPort)
		{
			SteamNetConnectionInfo_t steamNetConnectionInfo_t;
			if (SteamGameServerNetworkingSockets.GetConnectionInfo(this.steamConnectionHandle, out steamNetConnectionInfo_t))
			{
				string result;
				steamNetConnectionInfo_t.m_addrRemote.ToString(out result, withPort);
				return result;
			}
			return null;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000B426 File Offset: 0x00009626
		public override bool Equals(object obj)
		{
			return this.Equals(obj as TransportConnection_SteamNetworkingSockets);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000B434 File Offset: 0x00009634
		public bool Equals(TransportConnection_SteamNetworkingSockets other)
		{
			return other != null && this.steamConnectionHandle == other.steamConnectionHandle;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000B44C File Offset: 0x0000964C
		public bool Equals(ITransportConnection other)
		{
			return this.Equals(other as TransportConnection_SteamNetworkingSockets);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000B45A File Offset: 0x0000965A
		public override int GetHashCode()
		{
			return this.steamConnectionHandle.GetHashCode();
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000B46D File Offset: 0x0000966D
		public override string ToString()
		{
			return this.serverTransport.IdentityToString(this.steamIdentity);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000B480 File Offset: 0x00009680
		public void CloseConnection()
		{
			this.serverTransport.CloseConnection(this);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000B490 File Offset: 0x00009690
		public unsafe void Send(byte[] buffer, long size, ENetReliability reliability)
		{
			int nSendFlags = this.serverTransport.ReliabilityToSendFlags(reliability);
			fixed (byte[] array = buffer)
			{
				byte* ptr;
				if (buffer == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				IntPtr pData;
				pData..ctor((void*)ptr);
				long num;
				SteamGameServerNetworkingSockets.SendMessageToConnection(this.steamConnectionHandle, pData, (uint)size, nSendFlags, out num);
			}
		}

		// Token: 0x04000139 RID: 313
		internal bool wasClosed;

		// Token: 0x0400013A RID: 314
		internal HSteamNetConnection steamConnectionHandle;

		// Token: 0x0400013B RID: 315
		internal SteamNetworkingIdentity steamIdentity;

		// Token: 0x0400013C RID: 316
		private ServerTransport_SteamNetworkingSockets serverTransport;
	}
}
