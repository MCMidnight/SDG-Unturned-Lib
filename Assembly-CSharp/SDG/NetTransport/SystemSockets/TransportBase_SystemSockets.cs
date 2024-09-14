using System;
using System.Diagnostics;
using SDG.Unturned;

namespace SDG.NetTransport.SystemSockets
{
	// Token: 0x0200006B RID: 107
	public abstract class TransportBase_SystemSockets
	{
		// Token: 0x06000259 RID: 601 RVA: 0x00009970 File Offset: 0x00007B70
		[Conditional("LOG_NETTRANSPORT_SYSTEMSOCKETS")]
		internal static void Log(string format, params object[] args)
		{
			UnturnedLog.info(format, args);
		}
	}
}
