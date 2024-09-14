using System;

namespace SDG.Unturned
{
	// Token: 0x02000613 RID: 1555
	// (Invoke) Token: 0x060031EE RID: 12782
	public delegate void PlayerCraftingRequestHandler(PlayerCrafting crafting, ref ushort itemID, ref byte blueprintIndex, ref bool shouldAllow);
}
