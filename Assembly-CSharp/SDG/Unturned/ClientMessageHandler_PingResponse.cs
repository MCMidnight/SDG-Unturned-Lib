using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200026E RID: 622
	internal static class ClientMessageHandler_PingResponse
	{
		// Token: 0x0600126E RID: 4718 RVA: 0x0003FB90 File Offset: 0x0003DD90
		internal static void ReadMessage(NetPakReader reader)
		{
			if (Provider.timeLastPingRequestWasSentToServer > 0f)
			{
				float deltaTime = Time.deltaTime;
				Provider.lag(Time.realtimeSinceStartup - Provider.timeLastPingRequestWasSentToServer - deltaTime);
				Provider.timeLastPingRequestWasSentToServer = -1f;
			}
		}
	}
}
