using System;
using Steamworks;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	/// <summary>
	/// Parameters for connecting to a game server.
	///
	/// Admittedly there are other parameters to the Connect method,
	/// but those are for detecting advertisement discrepencies and can be null.
	/// </summary>
	// Token: 0x02000681 RID: 1665
	public class ServerConnectParameters
	{
		// Token: 0x06003811 RID: 14353 RVA: 0x001094BB File Offset: 0x001076BB
		public ServerConnectParameters(IPv4Address address, ushort queryPort, ushort connectionPort, string password)
		{
			this.address = address;
			this.queryPort = queryPort;
			this.connectionPort = connectionPort;
			this.password = password;
		}

		// Token: 0x06003812 RID: 14354 RVA: 0x001094E0 File Offset: 0x001076E0
		public ServerConnectParameters(CSteamID steamId, string password)
		{
			this.steamId = steamId;
			this.password = password;
		}

		/// <summary>
		/// Server's public IP address of a Steam "Fake IP" address.
		/// </summary>
		// Token: 0x04002142 RID: 8514
		public IPv4Address address;

		/// <summary>
		/// Port for Steam's "A2S" query system. This the port we refer to when
		/// sharing a server's address (e.g., 127.0.0.1:queryport).
		/// </summary>
		// Token: 0x04002143 RID: 8515
		public ushort queryPort;

		/// <summary>
		/// Port for game traffic. i.e., Steam manages the query port socket while
		/// we manage the connection port socket. The game assumes it's the query
		/// port plus one. NOTE HOWEVER after the introduction of "Fake IP" support
		/// (2023-12-07) the connection port is the same as the query port for fake
		/// IPs. In keeping with the spirit of fake values to simplify existing code,
		/// we act as if the connection port is plus one except in the appropriate
		/// ClientTransport code when the fake IP is detected.
		/// </summary>
		// Token: 0x04002144 RID: 8516
		public ushort connectionPort;

		/// <summary>
		/// Referred to as "Server Code" in menus.
		/// Used if address is zero.
		/// </summary>
		// Token: 0x04002145 RID: 8517
		public CSteamID steamId;

		// Token: 0x04002146 RID: 8518
		public string password;
	}
}
