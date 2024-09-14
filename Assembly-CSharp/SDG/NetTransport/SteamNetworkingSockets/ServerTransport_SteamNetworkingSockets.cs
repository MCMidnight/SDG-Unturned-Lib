using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SDG.Framework.Utilities;
using SDG.Unturned;
using Steamworks;
using Unturned.SystemEx;

namespace SDG.NetTransport.SteamNetworkingSockets
{
	// Token: 0x0200006E RID: 110
	public class ServerTransport_SteamNetworkingSockets : TransportBase_SteamNetworkingSockets, IServerTransport
	{
		// Token: 0x0600027C RID: 636 RVA: 0x0000A5B0 File Offset: 0x000087B0
		public void Initialize(ServerTransportConnectionFailureCallback connectionFailureCallback)
		{
			this.connectionFailureCallback = connectionFailureCallback;
			this.steamNetConnectionStatusChanged = Callback<SteamNetConnectionStatusChangedCallback_t>.CreateGameServer(new Callback<SteamNetConnectionStatusChangedCallback_t>.DispatchDelegate(this.OnSteamNetConnectionStatusChanged));
			this.steamNetAuthenticationStatusChanged = Callback<SteamNetAuthenticationStatus_t>.CreateGameServer(new Callback<SteamNetAuthenticationStatus_t>.DispatchDelegate(this.OnSteamNetAuthenticationStatusChanged));
			ESteamNetworkingSocketsDebugOutputType esteamNetworkingSocketsDebugOutputType = base.SelectDebugOutputDetailLevel();
			if (esteamNetworkingSocketsDebugOutputType != ESteamNetworkingSocketsDebugOutputType.k_ESteamNetworkingSocketsDebugOutputType_None)
			{
				this.didSetupDebugOutput = true;
				base.Log("Server set SNS debug output detail level to {0}", new object[]
				{
					esteamNetworkingSocketsDebugOutputType
				});
				Steamworks.SteamGameServerNetworkingUtils.SetDebugOutputFunction(esteamNetworkingSocketsDebugOutputType, base.GetDebugOutputFunction());
			}
			TimeUtility.updated += this.OnUpdate;
			CommandLogMemoryUsage.OnExecuted = (Action<List<string>>)Delegate.Combine(CommandLogMemoryUsage.OnExecuted, new Action<List<string>>(this.OnLogMemoryUsage));
			SteamNetworkingConfigValue_t[] array = this.BuildDefaultConfig().ToArray();
			SteamNetworkingIPAddr address = default(SteamNetworkingIPAddr);
			if (string.IsNullOrEmpty(Provider.bindAddress))
			{
				address.Clear();
			}
			else if (Provider.ip > 0U)
			{
				address.SetIPv4(Provider.ip, 0);
			}
			else
			{
				base.Log("Unable to parse \"{0}\" as listen bind address", new object[]
				{
					Provider.bindAddress
				});
				address.Clear();
			}
			address.m_port = Provider.GetServerConnectionPort();
			if (ServerTransport_SteamNetworkingSockets.clUseIpSocket)
			{
				this.ipListenSocket = SteamGameServerNetworkingSockets.CreateListenSocketIP(ref address, array.Length, array);
				base.Log("Server listen socket bound to {0}", new object[]
				{
					base.AddressToString(address, true)
				});
			}
			else
			{
				base.Log("Server skipping creation of IP listen socket", Array.Empty<object>());
			}
			if (Provider.configData.Server.Use_FakeIP)
			{
				this.fakeIpListenSocket = SteamGameServerNetworkingSockets.CreateListenSocketP2PFakeIP(0, array.Length, array);
				base.Log("Server FakeIP listen socket: {0}", new object[]
				{
					this.fakeIpListenSocket
				});
				SteamNetworkingFakeIPResult_t steamNetworkingFakeIPResult_t;
				SteamGameServerNetworkingSockets.GetFakeIP(0, out steamNetworkingFakeIPResult_t);
				if (steamNetworkingFakeIPResult_t.m_eResult == EResult.k_EResultBusy)
				{
					this.steamNetworkingFakeIpResultCallback = Callback<SteamNetworkingFakeIPResult_t>.CreateGameServer(new Callback<SteamNetworkingFakeIPResult_t>.DispatchDelegate(this.OnSteamNetworkingFakeIpResultCallback));
					base.Log("Waiting for FakeIP callback...", Array.Empty<object>());
				}
				else
				{
					this.OnSteamNetworkingFakeIpResultCallback(steamNetworkingFakeIPResult_t);
				}
			}
			if (ServerTransport_SteamNetworkingSockets.clUseP2pSocket)
			{
				this.p2pListenSocket = SteamGameServerNetworkingSockets.CreateListenSocketP2P(0, array.Length, array);
				base.Log("Server P2P listen socket: {0}", new object[]
				{
					this.p2pListenSocket
				});
			}
			else
			{
				base.Log("Server skipping creation of P2P listen socket", Array.Empty<object>());
			}
			if (this.ipListenSocket == HSteamListenSocket.Invalid && this.fakeIpListenSocket == HSteamListenSocket.Invalid && this.p2pListenSocket == HSteamListenSocket.Invalid)
			{
				base.Log("SNS did not create any sockets! This will probably not work properly!", Array.Empty<object>());
			}
			this.pollGroup = SteamGameServerNetworkingSockets.CreatePollGroup();
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000A834 File Offset: 0x00008A34
		public void TearDown()
		{
			TimeUtility.updated -= this.OnUpdate;
			CommandLogMemoryUsage.OnExecuted = (Action<List<string>>)Delegate.Remove(CommandLogMemoryUsage.OnExecuted, new Action<List<string>>(this.OnLogMemoryUsage));
			this.steamNetConnectionStatusChanged.Dispose();
			this.steamNetAuthenticationStatusChanged.Dispose();
			Callback<SteamNetworkingFakeIPResult_t> callback = this.steamNetworkingFakeIpResultCallback;
			if (callback != null)
			{
				callback.Dispose();
			}
			if (this.ipListenSocket != HSteamListenSocket.Invalid && !SteamGameServerNetworkingSockets.CloseListenSocket(this.ipListenSocket))
			{
				base.Log("Server failed to close IP listen socket {0}", new object[]
				{
					this.ipListenSocket
				});
			}
			if (this.fakeIpListenSocket != HSteamListenSocket.Invalid)
			{
				bool flag = SteamGameServerNetworkingSockets.CloseListenSocket(this.fakeIpListenSocket);
				if (!flag)
				{
					base.Log("Server failed to close \"Fake IP\" listen socket {0}", new object[]
					{
						flag
					});
				}
			}
			if (this.p2pListenSocket != HSteamListenSocket.Invalid && !SteamGameServerNetworkingSockets.CloseListenSocket(this.p2pListenSocket))
			{
				base.Log("Server failed to close P2P listen socket {0}", new object[]
				{
					this.p2pListenSocket
				});
			}
			if (!SteamGameServerNetworkingSockets.DestroyPollGroup(this.pollGroup))
			{
				base.Log("Server failed to destroy poll group {0}", new object[]
				{
					this.pollGroup
				});
			}
			if (this.didSetupDebugOutput)
			{
				this.didSetupDebugOutput = false;
				Steamworks.SteamGameServerNetworkingUtils.SetDebugOutputFunction(ESteamNetworkingSocketsDebugOutputType.k_ESteamNetworkingSocketsDebugOutputType_None, null);
			}
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000A994 File Offset: 0x00008B94
		public bool Receive(byte[] buffer, out long size, out ITransportConnection transportConnection)
		{
			while (SteamGameServerNetworkingSockets.ReceiveMessagesOnPollGroup(this.pollGroup, this.messageAddresses, this.messageAddresses.Length) >= 1)
			{
				IntPtr intPtr = this.messageAddresses[0];
				SteamNetworkingMessage_t steamNetworkingMessage_t = Marshal.PtrToStructure<SteamNetworkingMessage_t>(intPtr);
				if (steamNetworkingMessage_t.m_pData == IntPtr.Zero || steamNetworkingMessage_t.m_cbSize < 1)
				{
					SteamNetworkingMessage_t.Release(intPtr);
				}
				else
				{
					TransportConnection_SteamNetworkingSockets transportConnection_SteamNetworkingSockets = this.FindConnection(steamNetworkingMessage_t.m_conn);
					if (transportConnection_SteamNetworkingSockets != null && !transportConnection_SteamNetworkingSockets.wasClosed)
					{
						transportConnection = transportConnection_SteamNetworkingSockets;
						size = (long)steamNetworkingMessage_t.m_cbSize;
						if (size > (long)buffer.Length)
						{
							size = (long)buffer.Length;
						}
						Marshal.Copy(steamNetworkingMessage_t.m_pData, buffer, 0, (int)size);
						SteamNetworkingMessage_t.Release(intPtr);
						return true;
					}
					SteamNetworkingMessage_t.Release(intPtr);
				}
			}
			size = 0L;
			transportConnection = null;
			return false;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000AA4D File Offset: 0x00008C4D
		internal void CloseConnection(TransportConnection_SteamNetworkingSockets transportConnection)
		{
			if (transportConnection.wasClosed)
			{
				return;
			}
			transportConnection.wasClosed = true;
			SteamGameServerNetworkingSockets.CloseConnection(transportConnection.steamConnectionHandle, 0, null, true);
			this.transportConnections.RemoveFast(transportConnection);
		}

		/// <summary>
		/// Find game connection associated with Steam connection.
		/// </summary>
		// Token: 0x06000280 RID: 640 RVA: 0x0000AA7C File Offset: 0x00008C7C
		private TransportConnection_SteamNetworkingSockets FindConnection(HSteamNetConnection steamConnectionHandle)
		{
			foreach (TransportConnection_SteamNetworkingSockets transportConnection_SteamNetworkingSockets in this.transportConnections)
			{
				if (transportConnection_SteamNetworkingSockets.steamConnectionHandle == steamConnectionHandle)
				{
					return transportConnection_SteamNetworkingSockets;
				}
			}
			return null;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000AAE0 File Offset: 0x00008CE0
		private void OnUpdate()
		{
			base.LogDebugOutput();
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000AAE8 File Offset: 0x00008CE8
		private void OnLogMemoryUsage(List<string> results)
		{
			results.Add(string.Format("Steam networking sockets transport connections: {0}", this.transportConnections.Count));
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000AB0C File Offset: 0x00008D0C
		private void OnSteamNetworkingFakeIpResultCallback(SteamNetworkingFakeIPResult_t callback)
		{
			if (callback.m_eResult == EResult.k_EResultOK)
			{
				CommandWindow.Log("//////////////////////////////////////////////////////");
				Local localization = Provider.localization;
				string text = new IPv4Address(callback.m_unIP).ToString();
				string arg = string.Format("{0}:{1}", text, callback.m_unPorts[0]);
				CommandWindow.Log(localization.format("FakeIPHeader", arg));
				CommandWindow.Log(localization.format("FakeIPDetails"));
				CommandWindow.Log(localization.format("FakeIPCopy", "CopyFakeIP"));
				CommandWindow.Log("//////////////////////////////////////////////////////");
				return;
			}
			CommandWindow.LogError(string.Format("Fatal FakeIP result: {0}", callback.m_eResult));
			Provider.QuitGame(string.Format("fatal fake IP result ({0})", callback.m_eResult));
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000ABDC File Offset: 0x00008DDC
		private void OnSteamNetConnectionStatusChanged(SteamNetConnectionStatusChangedCallback_t callback)
		{
			switch (callback.m_info.m_eState)
			{
			case ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_None:
				return;
			case ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_Connecting:
				this.HandleState_Connecting(ref callback);
				return;
			case ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_FindingRoute:
				return;
			case ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_Connected:
				this.HandleState_Connected(ref callback);
				return;
			case ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_ClosedByPeer:
				this.HandleState_ClosedByPeer(ref callback);
				return;
			case ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_ProblemDetectedLocally:
				this.HandleState_ProblemDetectedLocally(ref callback);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000AC3C File Offset: 0x00008E3C
		private void HandleState_Connecting(ref SteamNetConnectionStatusChangedCallback_t callback)
		{
			if (Provider.didServerShutdownTimerReachZero)
			{
				if (!SteamGameServerNetworkingSockets.CloseConnection(callback.m_hConn, 1002, null, false))
				{
					base.Log("Server failed to close connecting connection while shutdown from {0} (End Reason: {1})", new object[]
					{
						base.IdentityToString(ref callback),
						1002
					});
					return;
				}
			}
			else if (Provider.hasRoomForNewConnection)
			{
				bool flag = SteamGameServerNetworkingSockets.SetConnectionPollGroup(callback.m_hConn, this.pollGroup);
				EResult eresult = SteamGameServerNetworkingSockets.AcceptConnection(callback.m_hConn);
				if (!flag || eresult != EResult.k_EResultOK)
				{
					SteamGameServerNetworkingSockets.CloseConnection(callback.m_hConn, 0, null, false);
					return;
				}
			}
			else if (!SteamGameServerNetworkingSockets.CloseConnection(callback.m_hConn, 1001, null, false))
			{
				base.Log("Server failed to close connecting connection from {0} (End Reason: {1})", new object[]
				{
					base.IdentityToString(ref callback),
					1001
				});
			}
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000AD0C File Offset: 0x00008F0C
		private void HandleState_Connected(ref SteamNetConnectionStatusChangedCallback_t callback)
		{
			TransportConnection_SteamNetworkingSockets transportConnection_SteamNetworkingSockets = new TransportConnection_SteamNetworkingSockets(this, ref callback);
			this.transportConnections.Add(transportConnection_SteamNetworkingSockets);
		}

		/// <summary>
		/// Must close the handle to free up resources.
		/// </summary>
		// Token: 0x06000287 RID: 647 RVA: 0x0000AD30 File Offset: 0x00008F30
		private void HandleState_ClosedByPeer(ref SteamNetConnectionStatusChangedCallback_t callback)
		{
			TransportConnection_SteamNetworkingSockets transportConnection_SteamNetworkingSockets = this.FindConnection(callback.m_hConn);
			if (transportConnection_SteamNetworkingSockets != null)
			{
				try
				{
					string debugString = string.Format("ClosedByPeer Reason: {0} Message: \"{1}\"", callback.m_info.m_eEndReason, callback.m_info.m_szEndDebug);
					bool isError = callback.m_info.m_eEndReason != 1000;
					this.connectionFailureCallback(transportConnection_SteamNetworkingSockets, debugString, isError);
				}
				catch (Exception e)
				{
					UnturnedLog.exception(e, "SteamNetworkingSockets caught exception during closed by peer failure callback:");
				}
				transportConnection_SteamNetworkingSockets.CloseConnection();
				return;
			}
			SteamGameServerNetworkingSockets.CloseConnection(callback.m_hConn, 0, null, false);
		}

		/// <summary>
		/// Must close the handle to free up resources.
		/// </summary>
		// Token: 0x06000288 RID: 648 RVA: 0x0000ADCC File Offset: 0x00008FCC
		private void HandleState_ProblemDetectedLocally(ref SteamNetConnectionStatusChangedCallback_t callback)
		{
			TransportConnection_SteamNetworkingSockets transportConnection_SteamNetworkingSockets = this.FindConnection(callback.m_hConn);
			if (transportConnection_SteamNetworkingSockets != null)
			{
				try
				{
					string debugString = string.Format("ProblemDetectedLocally Reason: {0} Message: \"{1}\"", callback.m_info.m_eEndReason, callback.m_info.m_szEndDebug);
					this.connectionFailureCallback(transportConnection_SteamNetworkingSockets, debugString, true);
				}
				catch (Exception e)
				{
					UnturnedLog.exception(e, "SteamNetworkingSockets caught exception during problem detected locally failure callback:");
				}
				transportConnection_SteamNetworkingSockets.CloseConnection();
				return;
			}
			SteamGameServerNetworkingSockets.CloseConnection(callback.m_hConn, 0, null, false);
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000AE54 File Offset: 0x00009054
		private void OnSteamNetAuthenticationStatusChanged(SteamNetAuthenticationStatus_t callback)
		{
			if (string.IsNullOrEmpty(callback.m_debugMsg))
			{
				base.Log("Readiness to participate in authenticated communications changed to {0}", new object[]
				{
					callback.m_eAvail
				});
				return;
			}
			base.Log("Readiness to participate in authenticated communications changed to {0} \"{1}\"", new object[]
			{
				callback.m_eAvail,
				callback.m_debugMsg
			});
		}

		// Token: 0x04000126 RID: 294
		private Callback<SteamNetworkingFakeIPResult_t> steamNetworkingFakeIpResultCallback;

		// Token: 0x04000127 RID: 295
		private Callback<SteamNetConnectionStatusChangedCallback_t> steamNetConnectionStatusChanged;

		// Token: 0x04000128 RID: 296
		private Callback<SteamNetAuthenticationStatus_t> steamNetAuthenticationStatusChanged;

		// Token: 0x04000129 RID: 297
		private ServerTransportConnectionFailureCallback connectionFailureCallback;

		// Token: 0x0400012A RID: 298
		private HSteamListenSocket ipListenSocket = HSteamListenSocket.Invalid;

		// Token: 0x0400012B RID: 299
		private HSteamListenSocket fakeIpListenSocket = HSteamListenSocket.Invalid;

		// Token: 0x0400012C RID: 300
		private HSteamListenSocket p2pListenSocket = HSteamListenSocket.Invalid;

		// Token: 0x0400012D RID: 301
		private HSteamNetPollGroup pollGroup;

		// Token: 0x0400012E RID: 302
		private List<TransportConnection_SteamNetworkingSockets> transportConnections = new List<TransportConnection_SteamNetworkingSockets>();

		// Token: 0x0400012F RID: 303
		private IntPtr[] messageAddresses = new IntPtr[1];

		// Token: 0x04000130 RID: 304
		private bool didSetupDebugOutput;

		/// <summary>
		/// Defaults to true. If false, skip Steam Networking Sockets creation of regular IP socket.
		/// </summary>
		// Token: 0x04000131 RID: 305
		private static CommandLineFlag clUseIpSocket = new CommandLineFlag(true, "-SNS_DisableIPSocket");

		/// <summary>
		/// Defaults to true. If false, skip Steam Networking Sockets creation of non-FakeIP P2P socket.
		/// (this is the socket used by "server codes")
		/// </summary>
		// Token: 0x04000132 RID: 306
		private static CommandLineFlag clUseP2pSocket = new CommandLineFlag(true, "-SNS_DisableP2PSocket");
	}
}
