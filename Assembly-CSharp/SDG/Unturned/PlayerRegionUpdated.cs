using System;

namespace SDG.Unturned
{
	// Token: 0x020006BF RID: 1727
	// (Invoke) Token: 0x060039A4 RID: 14756
	public delegate void PlayerRegionUpdated(Player player, byte old_x, byte old_y, byte new_x, byte new_y, byte index, ref bool canIncrementIndex);
}
