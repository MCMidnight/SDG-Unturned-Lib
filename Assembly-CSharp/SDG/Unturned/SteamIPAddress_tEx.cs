using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x0200081B RID: 2075
	public static class SteamIPAddress_tEx
	{
		/// <summary>
		/// Steam APIs returned uint32 IPv4 addresses in the past, so Unturned code depends on them in some places.
		/// Ideally these uses should be updated for IPv6 support going forward.
		/// For the meantime this method converts from the new format to the old format for backwards compatibility.
		/// </summary>
		// Token: 0x060046E6 RID: 18150 RVA: 0x001A7C10 File Offset: 0x001A5E10
		public static bool TryGetIPv4Address(this SteamIPAddress_t steamIPAddress, out uint address)
		{
			if (steamIPAddress.GetIPType() == ESteamIPType.k_ESteamIPTypeIPv4)
			{
				byte[] addressBytes = steamIPAddress.ToIPAddress().GetAddressBytes();
				address = (uint)((int)addressBytes[0] << 24 | (int)addressBytes[1] << 16 | (int)addressBytes[2] << 8 | (int)addressBytes[3]);
				return true;
			}
			address = 0U;
			return false;
		}
	}
}
