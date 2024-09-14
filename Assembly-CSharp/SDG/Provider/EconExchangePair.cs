using System;

namespace SDG.Provider
{
	// Token: 0x02000030 RID: 48
	public struct EconExchangePair
	{
		// Token: 0x06000126 RID: 294 RVA: 0x000049FC File Offset: 0x00002BFC
		public EconExchangePair(ulong newInstanceId, ushort newQuantity)
		{
			this.instanceId = newInstanceId;
			this.quantity = newQuantity;
		}

		// Token: 0x04000071 RID: 113
		public ulong instanceId;

		// Token: 0x04000072 RID: 114
		public ushort quantity;
	}
}
