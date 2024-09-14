using System;
using System.Diagnostics;
using SDG.Unturned;

namespace SDG.NetTransport.SteamNetworking
{
	// Token: 0x02000073 RID: 115
	public abstract class TransportBase_SteamNetworking
	{
		// Token: 0x060002B6 RID: 694 RVA: 0x0000B71A File Offset: 0x0000991A
		[Conditional("LOG_NETTRANSPORT_STEAMNETWORKING")]
		internal static void Log(string format, params object[] args)
		{
			UnturnedLog.info(format, args);
		}
	}
}
