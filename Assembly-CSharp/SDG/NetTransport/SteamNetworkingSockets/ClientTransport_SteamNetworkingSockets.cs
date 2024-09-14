using System;
using System.Runtime.InteropServices;
using SDG.Framework.Utilities;
using SDG.Unturned;
using Steamworks;
using Unturned.SystemEx;

namespace SDG.NetTransport.SteamNetworkingSockets
{
	// Token: 0x0200006D RID: 109
	public class ClientTransport_SteamNetworkingSockets : TransportBase_SteamNetworkingSockets, IClientTransport
	{
		// Token: 0x06000265 RID: 613 RVA: 0x00009B1C File Offset: 0x00007D1C
		public void Initialize(ClientTransportReady callback, ClientTransportFailure failureCallback)
		{
			this.connectedCallback = callback;
			this.failureCallback = failureCallback;
			this.steamNetConnectionStatusChanged = Callback<SteamNetConnectionStatusChangedCallback_t>.Create(new Callback<SteamNetConnectionStatusChangedCallback_t>.DispatchDelegate(this.OnSteamNetConnectionStatusChanged));
			this.steamNetAuthenticationStatusChanged = Callback<SteamNetAuthenticationStatus_t>.Create(new Callback<SteamNetAuthenticationStatus_t>.DispatchDelegate(this.OnSteamNetAuthenticationStatusChanged));
			ESteamNetworkingSocketsDebugOutputType esteamNetworkingSocketsDebugOutputType = base.SelectDebugOutputDetailLevel();
			if (esteamNetworkingSocketsDebugOutputType != ESteamNetworkingSocketsDebugOutputType.k_ESteamNetworkingSocketsDebugOutputType_None)
			{
				this.didSetupDebugOutput = true;
				base.Log("Client set SNS debug output detail level to {0}", new object[]
				{
					esteamNetworkingSocketsDebugOutputType
				});
				SteamNetworkingUtils.SetDebugOutputFunction(esteamNetworkingSocketsDebugOutputType, base.GetDebugOutputFunction());
			}
			TimeUtility.updated += this.OnUpdate;
			if (TransportBase_SteamNetworkingSockets.clAllowWithoutAuth)
			{
				this.isWaitingForAuthAvailability = false;
				base.Log("Client bypassing test for Steam Networking availability", Array.Empty<object>());
				this.Connect();
				return;
			}
			this.isWaitingForAuthAvailability = true;
			ESteamNetworkingAvailability esteamNetworkingAvailability = SteamNetworkingSockets.InitAuthentication();
			if (esteamNetworkingAvailability != ESteamNetworkingAvailability.k_ESteamNetworkingAvailability_Current)
			{
				base.Log("Client testing for Steam Networking availability ({0})", new object[]
				{
					esteamNetworkingAvailability
				});
			}
			this.HandleAuth(esteamNetworkingAvailability);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00009C0C File Offset: 0x00007E0C
		public void TearDown()
		{
			this.steamNetConnectionStatusChanged.Dispose();
			this.steamNetAuthenticationStatusChanged.Dispose();
			if (!this.didCloseConnection && this.connection != HSteamNetConnection.Invalid)
			{
				this.didCloseConnection = true;
				bool flag = SteamNetworkingSockets.CloseConnection(this.connection, 0, null, true);
				base.Log("Client disconnect from {0} result: {1}", new object[]
				{
					this.connection,
					flag
				});
			}
			TimeUtility.updated -= this.OnUpdate;
			if (this.didSetupDebugOutput)
			{
				this.didSetupDebugOutput = false;
				SteamNetworkingUtils.SetDebugOutputFunction(ESteamNetworkingSocketsDebugOutputType.k_ESteamNetworkingSocketsDebugOutputType_None, null);
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00009CB0 File Offset: 0x00007EB0
		public unsafe void Send(byte[] buffer, long size, ENetReliability reliability)
		{
			if (!this.isConnected || this.didCloseConnection)
			{
				return;
			}
			int nSendFlags = base.ReliabilityToSendFlags(reliability);
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
				SteamNetworkingSockets.SendMessageToConnection(this.connection, pData, (uint)size, nSendFlags, out num);
			}
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00009D0C File Offset: 0x00007F0C
		public bool Receive(byte[] buffer, out long size)
		{
			size = 0L;
			if (!this.isConnected || this.didCloseConnection)
			{
				return false;
			}
			while (SteamNetworkingSockets.ReceiveMessagesOnConnection(this.connection, this.messageAddresses, this.messageAddresses.Length) >= 1)
			{
				IntPtr intPtr = this.messageAddresses[0];
				SteamNetworkingMessage_t steamNetworkingMessage_t = Marshal.PtrToStructure<SteamNetworkingMessage_t>(intPtr);
				if (!(steamNetworkingMessage_t.m_pData == IntPtr.Zero) && steamNetworkingMessage_t.m_cbSize >= 1)
				{
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
			return false;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00009DB0 File Offset: 0x00007FB0
		public bool TryGetIPv4Address(out IPv4Address address)
		{
			if (!this.isConnected || this.didCloseConnection)
			{
				address = IPv4Address.Zero;
				return false;
			}
			if (this.isRemoteUsingFakeIP)
			{
				address = Provider.CurrentServerConnectParameters.address;
				return true;
			}
			SteamNetConnectionInfo_t steamNetConnectionInfo_t;
			bool connectionInfo = SteamNetworkingSockets.GetConnectionInfo(this.connection, out steamNetConnectionInfo_t);
			uint num = connectionInfo ? steamNetConnectionInfo_t.m_addrRemote.GetIPv4() : 0U;
			address = new IPv4Address(num);
			return connectionInfo && num > 0U;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00009E28 File Offset: 0x00008028
		public bool TryGetConnectionPort(out ushort connectionPort)
		{
			if (!this.isConnected || this.didCloseConnection)
			{
				connectionPort = 0;
				return false;
			}
			if (this.isRemoteUsingFakeIP)
			{
				connectionPort = Provider.CurrentServerConnectParameters.connectionPort;
				return true;
			}
			SteamNetConnectionInfo_t steamNetConnectionInfo_t;
			bool connectionInfo = SteamNetworkingSockets.GetConnectionInfo(this.connection, out steamNetConnectionInfo_t);
			connectionPort = (connectionInfo ? steamNetConnectionInfo_t.m_addrRemote.m_port : 0);
			return connectionInfo && connectionPort > 0;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00009E8C File Offset: 0x0000808C
		public bool TryGetQueryPort(out ushort queryPort)
		{
			if (!this.isConnected || this.didCloseConnection)
			{
				queryPort = 0;
				return false;
			}
			if (this.isRemoteUsingFakeIP)
			{
				queryPort = Provider.CurrentServerConnectParameters.queryPort;
				return true;
			}
			SteamNetConnectionInfo_t steamNetConnectionInfo_t;
			bool connectionInfo = SteamNetworkingSockets.GetConnectionInfo(this.connection, out steamNetConnectionInfo_t);
			queryPort = (connectionInfo ? MathfEx.ClampToUShort((int)(steamNetConnectionInfo_t.m_addrRemote.m_port - 1)) : 0);
			return connectionInfo && queryPort > 0;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00009EF6 File Offset: 0x000080F6
		private void OnUpdate()
		{
			base.LogDebugOutput();
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00009F00 File Offset: 0x00008100
		private void Connect()
		{
			SteamNetworkingConfigValue_t[] array = this.BuildDefaultConfig().ToArray();
			if (!Provider.CurrentServerConnectParameters.address.IsZero)
			{
				SteamNetworkingIPAddr address = default(SteamNetworkingIPAddr);
				uint value = Provider.CurrentServerConnectParameters.address.value;
				if (SteamNetworkingUtils.IsFakeIPv4(value))
				{
					this.isRemoteUsingFakeIP = true;
					address.SetIPv4(value, Provider.CurrentServerConnectParameters.queryPort);
					base.Log("Client connecting to {0} (FakeIP)", new object[]
					{
						base.AddressToString(address, true)
					});
				}
				else
				{
					address.SetIPv4(value, Provider.CurrentServerConnectParameters.connectionPort);
					base.Log("Client connecting to {0}", new object[]
					{
						base.AddressToString(address, true)
					});
				}
				this.connection = SteamNetworkingSockets.ConnectByIPAddress(ref address, array.Length, array);
				return;
			}
			SteamNetworkingIdentity identity = default(SteamNetworkingIdentity);
			identity.SetSteamID(Provider.CurrentServerConnectParameters.steamId);
			this.connection = SteamNetworkingSockets.ConnectP2P(ref identity, 0, array.Length, array);
			base.Log("Client connecting to {0} (P2P)", new object[]
			{
				base.IdentityToString(identity)
			});
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000A00C File Offset: 0x0000820C
		private void OnSteamNetConnectionStatusChanged(SteamNetConnectionStatusChangedCallback_t callback)
		{
			if (callback.m_hConn != this.connection)
			{
				return;
			}
			switch (callback.m_info.m_eState)
			{
			case ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_None:
			case ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_Connecting:
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

		// Token: 0x0600026F RID: 623 RVA: 0x0000A074 File Offset: 0x00008274
		private void HandleState_Connected(ref SteamNetConnectionStatusChangedCallback_t callback)
		{
			if (this.connectedCallback == null)
			{
				return;
			}
			this.isConnected = true;
			base.Log("Client connection with {0} ready", new object[]
			{
				this.connection
			});
			this.connectedCallback();
			this.connectedCallback = null;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000A0C4 File Offset: 0x000082C4
		private string GetMessageForEndReason(int endReasonCode)
		{
			ESteamNetConnectionEnd esteamNetConnectionEnd;
			try
			{
				esteamNetConnectionEnd = (ESteamNetConnectionEnd)endReasonCode;
			}
			catch
			{
				esteamNetConnectionEnd = ESteamNetConnectionEnd.k_ESteamNetConnectionEnd_Invalid;
			}
			if (endReasonCode >= 1000 && endReasonCode <= 1999)
			{
				if (endReasonCode == 1001)
				{
					return base.GetMessageText("SteamNetworkingSockets_EndReason_App_1001");
				}
				if (endReasonCode == 1002)
				{
					return base.GetMessageText("SteamNetworkingSockets_EndReason_App_1002");
				}
				if (esteamNetConnectionEnd == ESteamNetConnectionEnd.k_ESteamNetConnectionEnd_App_Min)
				{
					return null;
				}
				return base.GetMessageText("SteamNetworkingSockets_EndReason_App_Unknown", new object[]
				{
					endReasonCode
				});
			}
			else if (endReasonCode >= 2000 && endReasonCode <= 2999)
			{
				if (esteamNetConnectionEnd == ESteamNetConnectionEnd.k_ESteamNetConnectionEnd_AppException_Min)
				{
					return base.GetMessageText("SteamNetworkingSockets_EndReason_AppException_Generic");
				}
				return base.GetMessageText("SteamNetworkingSockets_EndReason_AppException_Unknown", new object[]
				{
					endReasonCode
				});
			}
			else if (endReasonCode >= 3000 && endReasonCode <= 3999)
			{
				switch (esteamNetConnectionEnd)
				{
				case ESteamNetConnectionEnd.k_ESteamNetConnectionEnd_Local_OfflineMode:
					return base.GetMessageText("SteamNetworkingSockets_EndReason_Local_OfflineMode");
				case ESteamNetConnectionEnd.k_ESteamNetConnectionEnd_Local_ManyRelayConnectivity:
				case ESteamNetConnectionEnd.k_ESteamNetConnectionEnd_Local_HostedServerPrimaryRelay:
					return base.GetMessageText("SteamNetworkingSockets_EndReason_Local_RelayConnectivity");
				case ESteamNetConnectionEnd.k_ESteamNetConnectionEnd_Local_NetworkConfig:
					return base.GetMessageText("SteamNetworkingSockets_EndReason_Local_NetworkConfig");
				case ESteamNetConnectionEnd.k_ESteamNetConnectionEnd_Local_Rights:
					return base.GetMessageText("SteamNetworkingSockets_EndReason_Local_Rights");
				default:
					return base.GetMessageText("SteamNetworkingSockets_EndReason_Local_Unknown", new object[]
					{
						endReasonCode
					});
				}
			}
			else
			{
				if (endReasonCode >= 4000 && endReasonCode <= 4999)
				{
					switch (esteamNetConnectionEnd)
					{
					case ESteamNetConnectionEnd.k_ESteamNetConnectionEnd_Remote_Timeout:
						return base.GetMessageText("SteamNetworkingSockets_EndReason_Remote_Timeout");
					case ESteamNetConnectionEnd.k_ESteamNetConnectionEnd_Remote_BadCrypt:
						return base.GetMessageText("SteamNetworkingSockets_EndReason_Remote_BadCrypt");
					case ESteamNetConnectionEnd.k_ESteamNetConnectionEnd_Remote_BadCert:
						return base.GetMessageText("SteamNetworkingSockets_EndReason_Remote_BadCert");
					case ESteamNetConnectionEnd.k_ESteamNetConnectionEnd_Remote_BadProtocolVersion:
						return base.GetMessageText("SteamNetworkingSockets_EndReason_Remote_BadProtocolVersion");
					}
					return base.GetMessageText("SteamNetworkingSockets_EndReason_Remote_Unknown", new object[]
					{
						endReasonCode
					});
				}
				if (endReasonCode >= 5000 && endReasonCode <= 5999)
				{
					switch (esteamNetConnectionEnd)
					{
					case ESteamNetConnectionEnd.k_ESteamNetConnectionEnd_Misc_InternalError:
						return base.GetMessageText("SteamNetworkingSockets_EndReason_Misc_InternalError");
					case ESteamNetConnectionEnd.k_ESteamNetConnectionEnd_Misc_Timeout:
						return base.GetMessageText("SteamNetworkingSockets_EndReason_Misc_Timeout");
					case ESteamNetConnectionEnd.k_ESteamNetConnectionEnd_Misc_SteamConnectivity:
						return base.GetMessageText("SteamNetworkingSockets_EndReason_Misc_SteamConnectivity");
					}
					return base.GetMessageText("SteamNetworkingSockets_EndReason_Misc_Unknown", new object[]
					{
						endReasonCode
					});
				}
				return base.GetMessageText("SteamNetworkingSockets_EndReason_Unknown", new object[]
				{
					endReasonCode
				});
			}
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000A310 File Offset: 0x00008510
		private void InvokeFailureCallback(string message)
		{
			if (this.failureCallback == null)
			{
				return;
			}
			ClientTransportFailure clientTransportFailure = this.failureCallback;
			this.failureCallback = null;
			clientTransportFailure(message);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000A330 File Offset: 0x00008530
		private void InvokeFailureCallback(int endReasonCode)
		{
			if (this.failureCallback == null)
			{
				return;
			}
			string messageForEndReason = this.GetMessageForEndReason(endReasonCode);
			if (string.IsNullOrEmpty(messageForEndReason))
			{
				return;
			}
			this.InvokeFailureCallback(messageForEndReason);
		}

		/// <summary>
		/// Must close the handle to free up resources.
		/// </summary>
		// Token: 0x06000273 RID: 627 RVA: 0x0000A360 File Offset: 0x00008560
		private void HandleState_ClosedByPeer(ref SteamNetConnectionStatusChangedCallback_t callback)
		{
			base.Log("Client connection closed by peer ({0}) \"{1}\"", new object[]
			{
				callback.m_info.m_eEndReason,
				callback.m_info.m_szEndDebug
			});
			this.didCloseConnection = true;
			if (!SteamNetworkingSockets.CloseConnection(callback.m_hConn, 0, null, false))
			{
				base.Log("Client failed to release connection closed by peer", Array.Empty<object>());
			}
			this.InvokeFailureCallback(callback.m_info.m_eEndReason);
		}

		/// <summary>
		/// Must close the handle to free up resources.
		/// </summary>
		// Token: 0x06000274 RID: 628 RVA: 0x0000A3D8 File Offset: 0x000085D8
		private void HandleState_ProblemDetectedLocally(ref SteamNetConnectionStatusChangedCallback_t callback)
		{
			base.Log("Client connection problem detected locally ({0}) \"{1}\"", new object[]
			{
				callback.m_info.m_eEndReason,
				callback.m_info.m_szEndDebug
			});
			this.didCloseConnection = true;
			if (!SteamNetworkingSockets.CloseConnection(callback.m_hConn, 0, null, false))
			{
				base.Log("Client failed to release connection after problem detected locally", Array.Empty<object>());
			}
			this.InvokeFailureCallback(callback.m_info.m_eEndReason);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000A450 File Offset: 0x00008650
		private void OnSteamNetAuthenticationStatusChanged(SteamNetAuthenticationStatus_t callback)
		{
			if (string.IsNullOrEmpty(callback.m_debugMsg))
			{
				base.Log("Readiness to participate in authenticated communications changed to {0}", new object[]
				{
					callback.m_eAvail
				});
			}
			else
			{
				base.Log("Readiness to participate in authenticated communications changed to {0} \"{1}\"", new object[]
				{
					callback.m_eAvail,
					callback.m_debugMsg
				});
			}
			if (this.isWaitingForAuthAvailability)
			{
				this.HandleAuth(callback.m_eAvail);
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000A4CC File Offset: 0x000086CC
		private void HandleAuth(ESteamNetworkingAvailability authAvailability)
		{
			if (authAvailability <= ESteamNetworkingAvailability.k_ESteamNetworkingAvailability_Retrying)
			{
				switch (authAvailability)
				{
				case ESteamNetworkingAvailability.k_ESteamNetworkingAvailability_CannotTry:
					this.HandleAuth_CannotTry();
					return;
				case ESteamNetworkingAvailability.k_ESteamNetworkingAvailability_Failed:
					this.HandleAuth_Failed();
					return;
				case ESteamNetworkingAvailability.k_ESteamNetworkingAvailability_Previously:
					this.HandleAuth_Previously();
					return;
				default:
					return;
				}
			}
			else
			{
				if (authAvailability - ESteamNetworkingAvailability.k_ESteamNetworkingAvailability_NeverTried <= 2)
				{
					return;
				}
				if (authAvailability != ESteamNetworkingAvailability.k_ESteamNetworkingAvailability_Current)
				{
					return;
				}
				this.HandleAuth_Current();
				return;
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000A521 File Offset: 0x00008721
		private void HandleAuth_CannotTry()
		{
			this.isWaitingForAuthAvailability = false;
			this.InvokeFailureCallback(base.GetMessageText("SteamNetworkingSockets_Unavailable_CannotTry"));
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000A53B File Offset: 0x0000873B
		private void HandleAuth_Failed()
		{
			this.isWaitingForAuthAvailability = false;
			this.InvokeFailureCallback(base.GetMessageText("SteamNetworkingSockets_Unavailable_Failed"));
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000A555 File Offset: 0x00008755
		private void HandleAuth_Previously()
		{
			this.isWaitingForAuthAvailability = false;
			this.InvokeFailureCallback(base.GetMessageText("SteamNetworkingSockets_Unavailable_Previously"));
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000A56F File Offset: 0x0000876F
		private void HandleAuth_Current()
		{
			base.Log("Client Steam Networking available", Array.Empty<object>());
			this.isWaitingForAuthAvailability = false;
			this.Connect();
		}

		// Token: 0x0400011B RID: 283
		private Callback<SteamNetConnectionStatusChangedCallback_t> steamNetConnectionStatusChanged;

		// Token: 0x0400011C RID: 284
		private Callback<SteamNetAuthenticationStatus_t> steamNetAuthenticationStatusChanged;

		// Token: 0x0400011D RID: 285
		private ClientTransportReady connectedCallback;

		// Token: 0x0400011E RID: 286
		private ClientTransportFailure failureCallback;

		// Token: 0x0400011F RID: 287
		private HSteamNetConnection connection = HSteamNetConnection.Invalid;

		// Token: 0x04000120 RID: 288
		private bool isWaitingForAuthAvailability;

		// Token: 0x04000121 RID: 289
		private bool isConnected;

		// Token: 0x04000122 RID: 290
		private bool didCloseConnection;

		// Token: 0x04000123 RID: 291
		private bool didSetupDebugOutput;

		// Token: 0x04000124 RID: 292
		private bool isRemoteUsingFakeIP;

		/// <summary>
		/// Recycled array for every read call.
		/// </summary>
		// Token: 0x04000125 RID: 293
		private IntPtr[] messageAddresses = new IntPtr[1];
	}
}
