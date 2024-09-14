using System;

namespace SDG.Unturned
{
	// Token: 0x020007AE RID: 1966
	public class EconCraftOption
	{
		// Token: 0x060041EC RID: 16876 RVA: 0x00165184 File Offset: 0x00163384
		public EconCraftOption(string newToken, int newGenerate, ushort newScrapsNeeded)
		{
			this.token = newToken;
			this.generate = newGenerate;
			this.scrapsNeeded = newScrapsNeeded;
		}

		// Token: 0x04002B0A RID: 11018
		public string token;

		// Token: 0x04002B0B RID: 11019
		public int generate;

		// Token: 0x04002B0C RID: 11020
		public ushort scrapsNeeded;
	}
}
