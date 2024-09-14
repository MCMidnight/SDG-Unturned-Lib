using System;
using System.Net;
using System.Net.Sockets;
using SDG.Framework.Utilities;
using SDG.Unturned;
using Unturned.SystemEx;

namespace SDG.NetTransport.SystemSockets
{
	/// <summary>
	/// Implementation using .NET Berkeley sockets.
	/// </summary>
	// Token: 0x02000068 RID: 104
	public class ClientTransport_SystemSockets : TransportBase_SystemSockets, IClientTransport
	{
		// Token: 0x06000245 RID: 581 RVA: 0x000093C4 File Offset: 0x000075C4
		public void Initialize(ClientTransportReady callback, ClientTransportFailure failureCallback)
		{
			uint value = Provider.CurrentServerConnectParameters.address.value;
			long num = (long)((ulong)((value & 255U) << 24 | (value >> 8 & 255U) << 16 | (value >> 16 & 255U) << 8 | (value >> 24 & 255U)));
			int connectionPort = (int)Provider.CurrentServerConnectParameters.connectionPort;
			this.remoteAddress = value;
			this.remotePort = Provider.CurrentServerConnectParameters.connectionPort;
			IPEndPoint ipendPoint = new IPEndPoint(num, connectionPort);
			this.socket = new Socket(2, 1, 6);
			this.socket.Connect(ipendPoint);
			this.socket.Blocking = false;
			this.messageQueue = new SocketMessageLayer();
			TimeUtility.updated += this.OnUpdate;
			callback();
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00009482 File Offset: 0x00007682
		public void TearDown()
		{
			TimeUtility.updated -= this.OnUpdate;
			this.socket.Close();
			this.socket = null;
		}

		// Token: 0x06000247 RID: 583 RVA: 0x000094A7 File Offset: 0x000076A7
		public void Send(byte[] buffer, long size, ENetReliability reliability)
		{
			if (this.socket == null)
			{
				return;
			}
			this.messageQueue.SendMessage(this.socket, buffer, (int)size);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x000094C8 File Offset: 0x000076C8
		public bool Receive(byte[] buffer, out long size)
		{
			if (this.socket == null)
			{
				size = 0L;
				return false;
			}
			byte[] array;
			if (this.messageQueue.DequeueMessage(out array))
			{
				array.CopyTo(buffer, 0);
				size = (long)array.Length;
				return true;
			}
			size = 0L;
			return false;
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00009507 File Offset: 0x00007707
		public bool TryGetIPv4Address(out IPv4Address address)
		{
			address = new IPv4Address(this.remoteAddress);
			return true;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000951B File Offset: 0x0000771B
		public bool TryGetConnectionPort(out ushort connectionPort)
		{
			connectionPort = this.remotePort;
			return true;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00009526 File Offset: 0x00007726
		public bool TryGetQueryPort(out ushort queryPort)
		{
			queryPort = MathfEx.ClampToUShort((int)(this.remotePort - 1));
			return true;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00009538 File Offset: 0x00007738
		private void OnUpdate()
		{
			this.messageQueue.ReceiveMessages(this.socket);
		}

		// Token: 0x0400010C RID: 268
		private Socket socket;

		// Token: 0x0400010D RID: 269
		private SocketMessageLayer messageQueue;

		// Token: 0x0400010E RID: 270
		private uint remoteAddress;

		// Token: 0x0400010F RID: 271
		private ushort remotePort;
	}
}
