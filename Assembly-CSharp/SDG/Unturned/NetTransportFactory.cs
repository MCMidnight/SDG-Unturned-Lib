using System;
using SDG.NetTransport;
using SDG.NetTransport.SteamNetworking;
using SDG.NetTransport.SteamNetworkingSockets;
using SDG.NetTransport.SystemSockets;

namespace SDG.Unturned
{
	/// <summary>
	/// Not extendable until transport API is better finalized.
	/// </summary>
	// Token: 0x0200067C RID: 1660
	internal static class NetTransportFactory
	{
		// Token: 0x060036FE RID: 14078 RVA: 0x00101A40 File Offset: 0x000FFC40
		internal static string GetTag(IServerTransport serverTransport)
		{
			Type type = serverTransport.GetType();
			if (type == typeof(ServerTransport_SystemSockets))
			{
				return "sys";
			}
			if (type == typeof(ServerTransport_SteamNetworkingSockets))
			{
				return "sns";
			}
			if (type == typeof(ServerTransport_SteamNetworking))
			{
				return "def";
			}
			UnturnedLog.warn("Unknown net transport \"{0}\", using default tag", new object[]
			{
				type.Name
			});
			return "sns";
		}

		// Token: 0x060036FF RID: 14079 RVA: 0x00101ABC File Offset: 0x000FFCBC
		internal static IClientTransport CreateClientTransport(string tag)
		{
			if (string.Equals(tag, "sys", 5))
			{
				return new ClientTransport_SystemSockets();
			}
			if (string.Equals(tag, "sns", 5))
			{
				return new ClientTransport_SteamNetworkingSockets();
			}
			if (string.Equals(tag, "def", 5))
			{
				return new ClientTransport_SteamNetworking();
			}
			UnturnedLog.warn("Unknown net transport tag \"{0}\", using default", new object[]
			{
				tag
			});
			return new ClientTransport_SteamNetworkingSockets();
		}

		// Token: 0x06003700 RID: 14080 RVA: 0x00101B20 File Offset: 0x000FFD20
		internal static IServerTransport CreateServerTransport()
		{
			if (NetTransportFactory.clImpl.hasValue)
			{
				string value = NetTransportFactory.clImpl.value;
				if (string.Equals(value, "SystemSockets", 5))
				{
					return new ServerTransport_SystemSockets();
				}
				if (string.Equals(value, "SteamNetworkingSockets", 5))
				{
					return new ServerTransport_SteamNetworkingSockets();
				}
				if (string.Equals(value, "SteamNetworking", 5))
				{
					if (NetTransportFactory.clBypassEnableOldSteamNetworking)
					{
						return new ServerTransport_SteamNetworking();
					}
					UnturnedLog.warn("Old Steam networking is no longer supported. Please remove this option from your command-line arguments.");
				}
				else
				{
					UnturnedLog.warn("Unknown net transport implementation \"{0}\"", new object[]
					{
						value
					});
				}
			}
			return new ServerTransport_SteamNetworkingSockets();
		}

		// Token: 0x04002083 RID: 8323
		internal const string SystemSocketsTag = "sys";

		// Token: 0x04002084 RID: 8324
		internal const string SteamNetworkingSocketsTag = "sns";

		// Token: 0x04002085 RID: 8325
		internal const string SteamNetworkingTag = "def";

		// Token: 0x04002086 RID: 8326
		private static CommandLineString clImpl = new CommandLineString("-NetTransport");

		// Token: 0x04002087 RID: 8327
		private static CommandLineFlag clBypassEnableOldSteamNetworking = new CommandLineFlag(false, "-BypassEnableOldSteamNetworking");
	}
}
