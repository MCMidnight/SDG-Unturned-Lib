using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000595 RID: 1429
	// (Invoke) Token: 0x06002DAB RID: 11691
	[Obsolete]
	public delegate void SalvageStructureRequestHandler(CSteamID steamID, byte x, byte y, ushort index, ref bool shouldAllow);
}
