using System;

namespace SDG.Unturned
{
	// Token: 0x0200048E RID: 1166
	public class BlueprintSupply
	{
		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06002484 RID: 9348 RVA: 0x00091C1F File Offset: 0x0008FE1F
		public ushort id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06002485 RID: 9349 RVA: 0x00091C27 File Offset: 0x0008FE27
		public bool isCritical
		{
			get
			{
				return this._isCritical;
			}
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x00091C2F File Offset: 0x0008FE2F
		public BlueprintSupply(ushort newID, bool newCritical, byte newAmount)
		{
			this._id = newID;
			this._isCritical = newCritical;
			this.amount = (ushort)newAmount;
			this.hasAmount = 0;
		}

		// Token: 0x04001272 RID: 4722
		private ushort _id;

		// Token: 0x04001273 RID: 4723
		private bool _isCritical;

		// Token: 0x04001274 RID: 4724
		public ushort amount;

		// Token: 0x04001275 RID: 4725
		public ushort hasAmount;
	}
}
