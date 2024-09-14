using System;
using System.Net;
using System.Net.Sockets;

namespace SDG.NetTransport.SystemSockets
{
	// Token: 0x0200006C RID: 108
	internal class TransportConnection_SystemSocket : ITransportConnection, IEquatable<ITransportConnection>
	{
		// Token: 0x0600025B RID: 603 RVA: 0x00009981 File Offset: 0x00007B81
		public TransportConnection_SystemSocket(ServerTransport_SystemSockets serverTransport, Socket clientSocket)
		{
			this.serverTransport = serverTransport;
			this.clientSocket = clientSocket;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x000099A4 File Offset: 0x00007BA4
		public bool TryGetIPv4Address(out uint address)
		{
			IPEndPoint ipendPoint = this.clientSocket.RemoteEndPoint as IPEndPoint;
			if (ipendPoint != null)
			{
				byte[] addressBytes = ipendPoint.Address.GetAddressBytes();
				if (addressBytes.Length == 4)
				{
					address = (uint)(((int)addressBytes[0] << 24 & 255) | ((int)addressBytes[0] << 16 & 255) | ((int)addressBytes[0] << 8 & 255) | (int)(addressBytes[0] & byte.MaxValue));
					return true;
				}
			}
			address = 0U;
			return false;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00009A10 File Offset: 0x00007C10
		public bool TryGetPort(out ushort port)
		{
			IPEndPoint ipendPoint = this.clientSocket.RemoteEndPoint as IPEndPoint;
			if (ipendPoint != null)
			{
				port = (ushort)ipendPoint.Port;
				return true;
			}
			port = 0;
			return false;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00009A40 File Offset: 0x00007C40
		public IPAddress GetAddress()
		{
			IPEndPoint ipendPoint = this.clientSocket.RemoteEndPoint as IPEndPoint;
			if (ipendPoint != null)
			{
				return ipendPoint.Address;
			}
			return null;
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00009A6C File Offset: 0x00007C6C
		public string GetAddressString(bool withPort)
		{
			IPEndPoint ipendPoint = this.clientSocket.RemoteEndPoint as IPEndPoint;
			if (ipendPoint != null)
			{
				string text = ipendPoint.Address.ToString();
				if (withPort)
				{
					text += ":";
					text += ipendPoint.Port.ToString();
				}
				return text;
			}
			return null;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00009AC0 File Offset: 0x00007CC0
		public void CloseConnection()
		{
			this.serverTransport.CloseConnection(this);
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00009ACE File Offset: 0x00007CCE
		public void Send(byte[] buffer, long size, ENetReliability reliability)
		{
			this.messageQueue.SendMessage(this.clientSocket, buffer, (int)size);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00009AE4 File Offset: 0x00007CE4
		bool IEquatable<ITransportConnection>.Equals(ITransportConnection other)
		{
			return this == other;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00009AEA File Offset: 0x00007CEA
		public override int GetHashCode()
		{
			return this.clientSocket.GetHashCode();
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00009AF7 File Offset: 0x00007CF7
		public override string ToString()
		{
			if (this.clientSocket.RemoteEndPoint == null)
			{
				return "Invalid Socket";
			}
			return this.clientSocket.RemoteEndPoint.ToString();
		}

		// Token: 0x04000118 RID: 280
		public ServerTransport_SystemSockets serverTransport;

		// Token: 0x04000119 RID: 281
		public Socket clientSocket;

		// Token: 0x0400011A RID: 282
		public SocketMessageLayer messageQueue = new SocketMessageLayer();
	}
}
