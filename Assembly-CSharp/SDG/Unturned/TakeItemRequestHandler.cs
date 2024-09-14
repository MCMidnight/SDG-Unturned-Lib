using System;

namespace SDG.Unturned
{
	// Token: 0x02000566 RID: 1382
	// (Invoke) Token: 0x06002C1F RID: 11295
	public delegate void TakeItemRequestHandler(Player player, byte x, byte y, uint instanceID, byte to_x, byte to_y, byte to_rot, byte to_page, ItemData itemData, ref bool shouldAllow);
}
