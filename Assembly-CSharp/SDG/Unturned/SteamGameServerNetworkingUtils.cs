using System;
using SDG.NetTransport;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x0200081A RID: 2074
	[Obsolete("Should not be specific to SteamGameServerNetworking after NetTransport rewrite")]
	public static class SteamGameServerNetworkingUtils
	{
		/// <summary>
		/// Get real IPv4 address of remote player NOT the relay server.
		/// </summary>
		/// <returns>True if address was available, and not flagged as a relay server.</returns>
		// Token: 0x060046E4 RID: 18148 RVA: 0x001A7BD4 File Offset: 0x001A5DD4
		[Obsolete("Should not be specific to SteamGameServerNetworking")]
		public static bool getIPv4Address(CSteamID steamIDRemote, out uint address)
		{
			ITransportConnection transportConnection = Provider.findTransportConnection(steamIDRemote);
			if (transportConnection != null)
			{
				return transportConnection.TryGetIPv4Address(out address);
			}
			address = 0U;
			return false;
		}

		/// <summary>
		/// See above, returns zero if failed.
		/// </summary>
		// Token: 0x060046E5 RID: 18149 RVA: 0x001A7BF8 File Offset: 0x001A5DF8
		[Obsolete("Should not be specific to SteamGameServerNetworking")]
		public static uint getIPv4AddressOrZero(CSteamID steamIDRemote)
		{
			uint result;
			SteamGameServerNetworkingUtils.getIPv4Address(steamIDRemote, out result);
			return result;
		}
	}
}
