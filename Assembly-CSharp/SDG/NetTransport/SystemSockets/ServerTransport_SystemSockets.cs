using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using SDG.Framework.Utilities;
using SDG.Unturned;

namespace SDG.NetTransport.SystemSockets
{
	/// <summary>
	/// Implementation using .NET Berkeley sockets.
	/// </summary>
	// Token: 0x02000069 RID: 105
	public class ServerTransport_SystemSockets : TransportBase_SystemSockets, IServerTransport
	{
		// Token: 0x0600024E RID: 590 RVA: 0x00009554 File Offset: 0x00007754
		public void Initialize(ServerTransportConnectionFailureCallback connectionClosedCallback)
		{
			int serverConnectionPort = (int)Provider.GetServerConnectionPort();
			IPAddress any;
			if (!IPAddress.TryParse(Provider.bindAddress, ref any))
			{
				any = IPAddress.Any;
			}
			IPEndPoint ipendPoint = new IPEndPoint(any, serverConnectionPort);
			this.listenSocket = new Socket(2, 1, 6);
			this.listenSocket.Blocking = false;
			this.listenSocket.Bind(ipendPoint);
			this.listenSocket.Listen(10);
			TimeUtility.updated += this.OnUpdate;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x000095C8 File Offset: 0x000077C8
		public void TearDown()
		{
			TimeUtility.updated -= this.OnUpdate;
			this.listenSocket.Close();
			this.listenSocket = null;
			foreach (TransportConnection_SystemSocket transportConnection_SystemSocket in this.connections)
			{
				transportConnection_SystemSocket.clientSocket.Close();
			}
			this.connections.Clear();
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000964C File Offset: 0x0000784C
		public bool Receive(byte[] buffer, out long size, out ITransportConnection transportConnection)
		{
			if (this.listenSocket == null)
			{
				size = 0L;
				transportConnection = null;
				return false;
			}
			if (this.messages.Count > 0)
			{
				ServerTransport_SystemSockets.PendingMessage pendingMessage = this.messages.Dequeue();
				pendingMessage.buffer.CopyTo(buffer, 0);
				size = (long)pendingMessage.buffer.Length;
				transportConnection = pendingMessage.transportConnection;
				return true;
			}
			transportConnection = null;
			size = 0L;
			return false;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x000096AE File Offset: 0x000078AE
		internal void CloseConnection(TransportConnection_SystemSocket connection)
		{
			connection.clientSocket.Close();
			this.connections.RemoveFast(connection);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x000096C8 File Offset: 0x000078C8
		private void OnUpdate()
		{
			foreach (TransportConnection_SystemSocket transportConnection_SystemSocket in this.connections)
			{
				transportConnection_SystemSocket.messageQueue.ReceiveMessages(transportConnection_SystemSocket.clientSocket);
				byte[] buffer;
				while (transportConnection_SystemSocket.messageQueue.DequeueMessage(out buffer))
				{
					ServerTransport_SystemSockets.PendingMessage pendingMessage = default(ServerTransport_SystemSockets.PendingMessage);
					pendingMessage.transportConnection = transportConnection_SystemSocket;
					pendingMessage.buffer = buffer;
					this.messages.Enqueue(pendingMessage);
				}
			}
			if (Provider.hasRoomForNewConnection)
			{
				try
				{
					Socket socket = this.listenSocket.Accept();
					socket.Blocking = false;
					TransportConnection_SystemSocket transportConnection_SystemSocket2 = new TransportConnection_SystemSocket(this, socket);
					this.connections.Add(transportConnection_SystemSocket2);
				}
				catch
				{
				}
			}
		}

		// Token: 0x04000110 RID: 272
		private Socket listenSocket;

		// Token: 0x04000111 RID: 273
		private List<TransportConnection_SystemSocket> connections = new List<TransportConnection_SystemSocket>();

		// Token: 0x04000112 RID: 274
		private Queue<ServerTransport_SystemSockets.PendingMessage> messages = new Queue<ServerTransport_SystemSockets.PendingMessage>();

		// Token: 0x02000850 RID: 2128
		internal struct PendingMessage
		{
			// Token: 0x04003149 RID: 12617
			public TransportConnection_SystemSocket transportConnection;

			// Token: 0x0400314A RID: 12618
			public byte[] buffer;
		}
	}
}
