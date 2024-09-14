using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000276 RID: 630
	internal static class NetMessages
	{
		// Token: 0x06001278 RID: 4728 RVA: 0x00040530 File Offset: 0x0003E730
		public static void SendMessageToClient(EClientMessage index, ENetReliability reliability, ITransportConnection transportConnection, NetMessages.ClientWriteHandler callback)
		{
			NetMessages.writer.Reset();
			NetMessages.writer.WriteEnum(index);
			callback(NetMessages.writer);
			NetMessages.writer.Flush();
			transportConnection.Send(NetMessages.writer.buffer, (long)NetMessages.writer.writeByteIndex, reliability);
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x00040588 File Offset: 0x0003E788
		public static void SendMessageToClients(EClientMessage index, ENetReliability reliability, List<ITransportConnection> transportConnections, NetMessages.ClientWriteHandler callback)
		{
			NetMessages.writer.Reset();
			NetMessages.writer.WriteEnum(index);
			callback(NetMessages.writer);
			NetMessages.writer.Flush();
			foreach (ITransportConnection transportConnection in transportConnections)
			{
				transportConnection.Send(NetMessages.writer.buffer, (long)NetMessages.writer.writeByteIndex, reliability);
			}
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x00040618 File Offset: 0x0003E818
		[Obsolete]
		public static void SendMessageToClients(EClientMessage index, ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, NetMessages.ClientWriteHandler callback)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				NetMessages.SendMessageToClients(index, reliability, list, callback);
				return;
			}
			throw new ArgumentException("should be a list", "transportConnections");
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x00040648 File Offset: 0x0003E848
		public static void SendMessageToServer(EServerMessage index, ENetReliability reliability, NetMessages.ClientWriteHandler callback)
		{
			if (!Provider.isConnected)
			{
				UnturnedLog.warn(string.Format("Ignoring request to send message {0} to server because we are not connected", index));
				return;
			}
			NetMessages.writer.Reset();
			NetMessages.writer.WriteEnum(index);
			callback(NetMessages.writer);
			NetMessages.writer.Flush();
			Provider.clientTransport.Send(NetMessages.writer.buffer, (long)NetMessages.writer.writeByteIndex, reliability);
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x000406C0 File Offset: 0x0003E8C0
		public static void ReceiveMessageFromClient(ITransportConnection transportConnection, byte[] packet, int offset, int size)
		{
			NetMessages.reader.SetBufferSegment(packet, size);
			NetMessages.reader.Reset();
			EServerMessage eserverMessage;
			if (!NetMessages.reader.ReadEnum(out eserverMessage))
			{
				UnturnedLog.warn("Received invalid packet index from {0}, so we're refusing them", new object[]
				{
					transportConnection
				});
				Provider.refuseGarbageConnection(transportConnection, "sv invalid packet index");
				return;
			}
			try
			{
				NetMessages.ServerReadHandler serverReadHandler = NetMessages.serverReadCallbacks[(int)eserverMessage];
				if (serverReadHandler != null)
				{
					serverReadHandler(transportConnection, NetMessages.reader);
				}
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Exception reading message {0} from client {1}:", new object[]
				{
					eserverMessage,
					transportConnection
				});
			}
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x0004075C File Offset: 0x0003E95C
		public static void ReceiveMessageFromServer(byte[] packet, int offset, int size)
		{
			NetMessages.reader.SetBufferSegment(packet, size);
			NetMessages.reader.Reset();
			EClientMessage eclientMessage;
			if (!NetMessages.reader.ReadEnum(out eclientMessage))
			{
				UnturnedLog.error("Client received invalid message index from server");
				return;
			}
			try
			{
				if (eclientMessage <= EClientMessage.UPDATE_UNRELIABLE_BUFFER)
				{
					NetMessages.reader.AlignToByte();
					Provider.legacyReceiveClient(packet, offset, size);
				}
				else
				{
					Provider.timeLastPacketWasReceivedFromServer = Time.realtimeSinceStartup;
					NetMessages.ClientReadHandler clientReadHandler = NetMessages.clientReadCallbacks[(int)eclientMessage];
					if (clientReadHandler != null)
					{
						clientReadHandler(NetMessages.reader);
					}
				}
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Exception reading message {0} from server:", new object[]
				{
					eclientMessage
				});
			}
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x00040800 File Offset: 0x0003EA00
		static NetMessages()
		{
			NetMessages.writer.buffer = Block.buffer;
			NetMessages.clientReadCallbacks = new NetMessages.ClientReadHandler[18];
			NetMessages.clientReadCallbacks[2] = new NetMessages.ClientReadHandler(ClientMessageHandler_PingRequest.ReadMessage);
			NetMessages.clientReadCallbacks[3] = new NetMessages.ClientReadHandler(ClientMessageHandler_PingResponse.ReadMessage);
			NetMessages.clientReadCallbacks[4] = new NetMessages.ClientReadHandler(ClientMessageHandler_Shutdown.ReadMessage);
			NetMessages.clientReadCallbacks[5] = new NetMessages.ClientReadHandler(ClientMessageHandler_PlayerConnected.ReadMessage);
			NetMessages.clientReadCallbacks[6] = new NetMessages.ClientReadHandler(ClientMessageHandler_PlayerDisconnected.ReadMessage);
			NetMessages.clientReadCallbacks[7] = new NetMessages.ClientReadHandler(ClientMessageHandler_DownloadWorkshopFiles.ReadMessage);
			NetMessages.clientReadCallbacks[8] = new NetMessages.ClientReadHandler(ClientMessageHandler_Verify.ReadMessage);
			NetMessages.clientReadCallbacks[9] = new NetMessages.ClientReadHandler(ClientMessageHandler_Accepted.ReadMessage);
			NetMessages.clientReadCallbacks[10] = new NetMessages.ClientReadHandler(ClientMessageHandler_Rejected.ReadMessage);
			NetMessages.clientReadCallbacks[11] = new NetMessages.ClientReadHandler(ClientMessageHandler_Banned.ReadMessage);
			NetMessages.clientReadCallbacks[12] = new NetMessages.ClientReadHandler(ClientMessageHandler_Kicked.ReadMessage);
			NetMessages.clientReadCallbacks[13] = new NetMessages.ClientReadHandler(ClientMessageHandler_Admined.ReadMessage);
			NetMessages.clientReadCallbacks[14] = new NetMessages.ClientReadHandler(ClientMessageHandler_Unadmined.ReadMessage);
			NetMessages.clientReadCallbacks[15] = new NetMessages.ClientReadHandler(ClientMessageHandler_BattlEye.ReadMessage);
			NetMessages.clientReadCallbacks[16] = new NetMessages.ClientReadHandler(ClientMessageHandler_QueuePositionChanged.ReadMessage);
			NetMessages.clientReadCallbacks[17] = new NetMessages.ClientReadHandler(ClientMessageHandler_InvokeMethod.ReadMessage);
			NetMessages.serverReadCallbacks = new NetMessages.ServerReadHandler[9];
			NetMessages.serverReadCallbacks[0] = new NetMessages.ServerReadHandler(ServerMessageHandler_GetWorkshopFiles.ReadMessage);
			NetMessages.serverReadCallbacks[1] = new NetMessages.ServerReadHandler(ServerMessageHandler_ReadyToConnect.ReadMessage);
			NetMessages.serverReadCallbacks[2] = new NetMessages.ServerReadHandler(ServerMessageHandler_Authenticate.ReadMessage);
			NetMessages.serverReadCallbacks[3] = new NetMessages.ServerReadHandler(ServerMessageHandler_BattlEye.ReadMessage);
			NetMessages.serverReadCallbacks[4] = new NetMessages.ServerReadHandler(ServerMessageHandler_PingRequest.ReadMessage);
			NetMessages.serverReadCallbacks[5] = new NetMessages.ServerReadHandler(ServerMessageHandler_PingResponse.ReadMessage);
			NetMessages.serverReadCallbacks[6] = new NetMessages.ServerReadHandler(ServerMessageHandler_InvokeMethod.ReadMessage);
			NetMessages.serverReadCallbacks[7] = new NetMessages.ServerReadHandler(ServerMessageHandler_ValidateAssets.ReadMessage);
			NetMessages.serverReadCallbacks[8] = new NetMessages.ServerReadHandler(ServerMessageHandler_GracefullyDisconnect.ReadMessage);
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x00040A3C File Offset: 0x0003EC3C
		internal static NetPakReader GetInvokableReader()
		{
			return NetMessages.reader;
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x00040A43 File Offset: 0x0003EC43
		internal static NetPakWriter GetInvokableWriter()
		{
			return NetMessages.writer;
		}

		// Token: 0x040005D7 RID: 1495
		internal static CommandLineFlag shouldLogBadMessages = new CommandLineFlag(false, "-LogBadMessages");

		// Token: 0x040005D8 RID: 1496
		private static NetPakReader reader = new NetPakReader();

		// Token: 0x040005D9 RID: 1497
		private static NetPakWriter writer = new NetPakWriter();

		// Token: 0x040005DA RID: 1498
		private static NetMessages.ClientReadHandler[] clientReadCallbacks;

		// Token: 0x040005DB RID: 1499
		private static NetMessages.ServerReadHandler[] serverReadCallbacks;

		// Token: 0x02000907 RID: 2311
		// (Invoke) Token: 0x06004A35 RID: 18997
		public delegate void ClientWriteHandler(NetPakWriter writer);

		// Token: 0x02000908 RID: 2312
		// (Invoke) Token: 0x06004A39 RID: 19001
		public delegate void ClientReadHandler(NetPakReader reader);

		// Token: 0x02000909 RID: 2313
		// (Invoke) Token: 0x06004A3D RID: 19005
		public delegate void ServerReadHandler(ITransportConnection transportConnection, NetPakReader reader);
	}
}
