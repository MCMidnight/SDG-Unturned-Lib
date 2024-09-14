using System;

namespace SDG.Unturned
{
	// Token: 0x0200048D RID: 1165
	public class BlueprintOutput
	{
		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06002482 RID: 9346 RVA: 0x00091BFA File Offset: 0x0008FDFA
		public ushort id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x00091C02 File Offset: 0x0008FE02
		public BlueprintOutput(ushort newID, byte newAmount, EItemOrigin newOrigin)
		{
			this._id = newID;
			this.amount = (ushort)newAmount;
			this.origin = newOrigin;
		}

		// Token: 0x0400126F RID: 4719
		private ushort _id;

		// Token: 0x04001270 RID: 4720
		public ushort amount;

		// Token: 0x04001271 RID: 4721
		public EItemOrigin origin;
	}
}
