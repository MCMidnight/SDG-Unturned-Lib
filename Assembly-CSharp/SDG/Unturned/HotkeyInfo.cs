using System;

namespace SDG.Unturned
{
	// Token: 0x0200061A RID: 1562
	public class HotkeyInfo
	{
		// Token: 0x06003216 RID: 12822 RVA: 0x000DEB20 File Offset: 0x000DCD20
		public HotkeyInfo()
		{
			this.id = 0;
			this.page = byte.MaxValue;
			this.x = byte.MaxValue;
			this.y = byte.MaxValue;
		}

		/// <summary>
		/// Which item ID we thought was there. If the item ID currently at the coordinates doesn't match we clear this hotkey.
		/// </summary>
		// Token: 0x04001C6E RID: 7278
		public ushort id;

		// Token: 0x04001C6F RID: 7279
		public byte page;

		// Token: 0x04001C70 RID: 7280
		public byte x;

		// Token: 0x04001C71 RID: 7281
		public byte y;
	}
}
