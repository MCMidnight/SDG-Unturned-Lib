using System;

namespace SDG.Provider.Services.Matchmaking
{
	// Token: 0x0200004C RID: 76
	public class MatchmakingFilter : IMatchmakingFilter
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001EB RID: 491 RVA: 0x00009352 File Offset: 0x00007552
		// (set) Token: 0x060001EC RID: 492 RVA: 0x0000935A File Offset: 0x0000755A
		public string key { get; protected set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001ED RID: 493 RVA: 0x00009363 File Offset: 0x00007563
		// (set) Token: 0x060001EE RID: 494 RVA: 0x0000936B File Offset: 0x0000756B
		public string value { get; protected set; }

		// Token: 0x060001EF RID: 495 RVA: 0x00009374 File Offset: 0x00007574
		public MatchmakingFilter(string newKey, string newValue)
		{
			this.key = newKey;
			this.value = newValue;
		}
	}
}
