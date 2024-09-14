using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using SDG.Unturned;
using Steamworks;

namespace SDG.NetTransport.SteamNetworkingSockets
{
	// Token: 0x0200006F RID: 111
	public abstract class TransportBase_SteamNetworkingSockets : TransportBase
	{
		/// <summary>
		/// Log verbose information that should not be included in release builds.
		/// </summary>
		// Token: 0x0600028C RID: 652 RVA: 0x0000AF1A File Offset: 0x0000911A
		[Conditional("LOG_NETTRANSPORT_STEAMNETWORKINGSOCKETS")]
		internal void DebugLog(string format, params object[] args)
		{
			UnturnedLog.info(format, args);
		}

		/// <summary>
		/// Log helpful information that should be included in release builds.
		/// </summary>
		// Token: 0x0600028D RID: 653 RVA: 0x0000AF23 File Offset: 0x00009123
		internal void Log(string format, params object[] args)
		{
			UnturnedLog.info(format, args);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000AF2C File Offset: 0x0000912C
		internal string AddressToString(SteamNetworkingIPAddr address, bool withPort = true)
		{
			string result;
			address.ToString(out result, withPort);
			return result;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000AF44 File Offset: 0x00009144
		internal string IdentityToString(SteamNetworkingIdentity identity)
		{
			string result;
			identity.ToString(out result);
			return result;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000AF5B File Offset: 0x0000915B
		internal string IdentityToString(ref SteamNetworkingMessage_t message)
		{
			return this.IdentityToString(message.m_identityPeer);
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000AF69 File Offset: 0x00009169
		internal string IdentityToString(ref SteamNetConnectionStatusChangedCallback_t callback)
		{
			return this.IdentityToString(callback.m_info.m_identityRemote);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000AF7C File Offset: 0x0000917C
		protected void DumpSteamNetworkingMessage(SteamNetworkingMessage_t message)
		{
			this.Log("Message Number {0}", new object[]
			{
				message.m_nMessageNumber
			});
			this.Log("\tData: {0}", new object[]
			{
				message.m_pData
			});
			this.Log("\tSize: {0}", new object[]
			{
				message.m_cbSize
			});
			this.Log("\tConnection: {0}", new object[]
			{
				message.m_conn
			});
			this.Log("\tPeer Identity: {0}", new object[]
			{
				this.IdentityToString(message.m_identityPeer)
			});
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000B028 File Offset: 0x00009228
		protected void LogDebugOutput()
		{
			TransportBase_SteamNetworkingSockets.DebugOutput debugOutput;
			while (this.debugOutputQueue.TryDequeue(ref debugOutput))
			{
				string text;
				switch (debugOutput.type)
				{
				case ESteamNetworkingSocketsDebugOutputType.k_ESteamNetworkingSocketsDebugOutputType_Bug:
					text = "Bug";
					break;
				case ESteamNetworkingSocketsDebugOutputType.k_ESteamNetworkingSocketsDebugOutputType_Error:
					text = "Error";
					break;
				case ESteamNetworkingSocketsDebugOutputType.k_ESteamNetworkingSocketsDebugOutputType_Important:
					text = "Important";
					break;
				case ESteamNetworkingSocketsDebugOutputType.k_ESteamNetworkingSocketsDebugOutputType_Warning:
					text = "Warning";
					break;
				default:
					text = null;
					break;
				}
				if (string.IsNullOrEmpty(text))
				{
					UnturnedLog.info("SteamNetworkingSockets: " + debugOutput.message);
				}
				else
				{
					UnturnedLog.info("SteamNetworkingSockets " + text + ": " + debugOutput.message);
				}
			}
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000B0C6 File Offset: 0x000092C6
		internal int ReliabilityToSendFlags(ENetReliability reliability)
		{
			if (reliability == ENetReliability.Reliable || reliability != ENetReliability.Unreliable)
			{
				return 8;
			}
			return 0;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000B0D4 File Offset: 0x000092D4
		protected ESteamNetworkingSocketsDebugOutputType SelectDebugOutputDetailLevel()
		{
			if (TransportBase_SteamNetworkingSockets.clLogSteamNetworkingSockets.hasValue)
			{
				try
				{
					return (ESteamNetworkingSocketsDebugOutputType)TransportBase_SteamNetworkingSockets.clLogSteamNetworkingSockets.value;
				}
				catch
				{
					this.Log("Unable to match {0} with a SNS output type", new object[]
					{
						TransportBase_SteamNetworkingSockets.clLogSteamNetworkingSockets.value
					});
				}
				return ESteamNetworkingSocketsDebugOutputType.k_ESteamNetworkingSocketsDebugOutputType_None;
			}
			return ESteamNetworkingSocketsDebugOutputType.k_ESteamNetworkingSocketsDebugOutputType_None;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000B134 File Offset: 0x00009334
		protected virtual List<SteamNetworkingConfigValue_t> BuildDefaultConfig()
		{
			List<SteamNetworkingConfigValue_t> list = new List<SteamNetworkingConfigValue_t>();
			if (TransportBase_SteamNetworkingSockets.clAllowWithoutAuth)
			{
				SteamNetworkingConfigValue_t steamNetworkingConfigValue_t = default(SteamNetworkingConfigValue_t);
				steamNetworkingConfigValue_t.m_eDataType = ESteamNetworkingConfigDataType.k_ESteamNetworkingConfig_Int32;
				steamNetworkingConfigValue_t.m_eValue = ESteamNetworkingConfigValue.k_ESteamNetworkingConfig_IP_AllowWithoutAuth;
				steamNetworkingConfigValue_t.m_val.m_int32 = 1;
				list.Add(steamNetworkingConfigValue_t);
			}
			if (TransportBase_SteamNetworkingSockets.clSendBufferSize.hasValue && TransportBase_SteamNetworkingSockets.clSendBufferSize.value > 0)
			{
				SteamNetworkingConfigValue_t steamNetworkingConfigValue_t2 = default(SteamNetworkingConfigValue_t);
				steamNetworkingConfigValue_t2.m_eDataType = ESteamNetworkingConfigDataType.k_ESteamNetworkingConfig_Int32;
				steamNetworkingConfigValue_t2.m_eValue = ESteamNetworkingConfigValue.k_ESteamNetworkingConfig_SendBufferSize;
				steamNetworkingConfigValue_t2.m_val.m_int32 = TransportBase_SteamNetworkingSockets.clSendBufferSize.value;
				list.Add(steamNetworkingConfigValue_t2);
			}
			if (TransportBase_SteamNetworkingSockets.clEnableDiagnosticsUI)
			{
				SteamNetworkingConfigValue_t steamNetworkingConfigValue_t3 = default(SteamNetworkingConfigValue_t);
				steamNetworkingConfigValue_t3.m_eDataType = ESteamNetworkingConfigDataType.k_ESteamNetworkingConfig_Int32;
				steamNetworkingConfigValue_t3.m_eValue = ESteamNetworkingConfigValue.k_ESteamNetworkingConfig_EnableDiagnosticsUI;
				steamNetworkingConfigValue_t3.m_val.m_int32 = 1;
				list.Add(steamNetworkingConfigValue_t3);
			}
			SteamNetworkingConfigValue_t steamNetworkingConfigValue_t4 = default(SteamNetworkingConfigValue_t);
			steamNetworkingConfigValue_t4.m_eDataType = ESteamNetworkingConfigDataType.k_ESteamNetworkingConfig_Int32;
			steamNetworkingConfigValue_t4.m_eValue = ESteamNetworkingConfigValue.k_ESteamNetworkingConfig_TimeoutInitial;
			steamNetworkingConfigValue_t4.m_val.m_int32 = 30000;
			list.Add(steamNetworkingConfigValue_t4);
			SteamNetworkingConfigValue_t steamNetworkingConfigValue_t5 = default(SteamNetworkingConfigValue_t);
			steamNetworkingConfigValue_t5.m_eDataType = ESteamNetworkingConfigDataType.k_ESteamNetworkingConfig_Int32;
			steamNetworkingConfigValue_t5.m_eValue = ESteamNetworkingConfigValue.k_ESteamNetworkingConfig_TimeoutConnected;
			steamNetworkingConfigValue_t5.m_val.m_int32 = 30000;
			list.Add(steamNetworkingConfigValue_t5);
			return list;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000B26D File Offset: 0x0000946D
		protected FSteamNetworkingSocketsDebugOutput GetDebugOutputFunction()
		{
			this.debugOutputFunc = new FSteamNetworkingSocketsDebugOutput(this.OnDebugOutput);
			return this.debugOutputFunc;
		}

		/// <summary>
		/// This callback may be called from a service thread. It must be threadsafe and fast! Do not make any other
		/// Steamworks calls from within the handler.
		/// </summary>
		// Token: 0x06000298 RID: 664 RVA: 0x0000B288 File Offset: 0x00009488
		private void OnDebugOutput(ESteamNetworkingSocketsDebugOutputType nType, IntPtr pszMsg)
		{
			try
			{
				string text = InteropHelp.PtrToStringUTF8(pszMsg);
				if (!string.IsNullOrEmpty(text))
				{
					TransportBase_SteamNetworkingSockets.DebugOutput debugOutput = default(TransportBase_SteamNetworkingSockets.DebugOutput);
					debugOutput.type = nType;
					debugOutput.message = text;
					this.debugOutputQueue.Enqueue(debugOutput);
				}
			}
			catch
			{
			}
		}

		/// <summary>
		/// Should certificate authentication be disabled for UDP connections?
		/// </summary>
		// Token: 0x04000133 RID: 307
		protected static CommandLineFlag clAllowWithoutAuth = new CommandLineFlag(false, "-SNS_AllowWithoutAuth");

		/// <summary>
		/// Thanks DiFFoZ! Ensures GC does not release the delegate.
		/// </summary>
		// Token: 0x04000134 RID: 308
		private FSteamNetworkingSocketsDebugOutput debugOutputFunc;

		// Token: 0x04000135 RID: 309
		private ConcurrentQueue<TransportBase_SteamNetworkingSockets.DebugOutput> debugOutputQueue = new ConcurrentQueue<TransportBase_SteamNetworkingSockets.DebugOutput>();

		/// <summary>
		/// Does host want extra debug output?
		/// </summary>
		// Token: 0x04000136 RID: 310
		private static CommandLineInt clLogSteamNetworkingSockets = new CommandLineInt("-LogSteamNetworkingSockets");

		/// <summary>
		/// Overrides k_ESteamNetworkingConfig_SendBufferSize.
		/// </summary>
		// Token: 0x04000137 RID: 311
		private static CommandLineInt clSendBufferSize = new CommandLineInt("-SNS_SendBufferSize");

		/// <summary>
		/// Overrides k_ESteamNetworkingConfig_EnableDiagnosticsUI.
		/// </summary>
		// Token: 0x04000138 RID: 312
		private static CommandLineFlag clEnableDiagnosticsUI = new CommandLineFlag(false, "-SNS_EnableDiagnosticsUI");

		// Token: 0x02000851 RID: 2129
		private struct DebugOutput
		{
			// Token: 0x0400314B RID: 12619
			public ESteamNetworkingSocketsDebugOutputType type;

			// Token: 0x0400314C RID: 12620
			public string message;
		}
	}
}
