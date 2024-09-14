using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x0200068A RID: 1674
	// (Invoke) Token: 0x06003839 RID: 14393
	[Obsolete]
	public delegate void TriggerReceive(SteamChannel channel, CSteamID steamID, byte[] packet, int offset, int size);
}
