using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000536 RID: 1334
	// (Invoke) Token: 0x060029D9 RID: 10713
	[Obsolete]
	public delegate void SalvageBarricadeRequestHandler(CSteamID steamID, byte x, byte y, ushort plant, ushort index, ref bool shouldAllow);
}
