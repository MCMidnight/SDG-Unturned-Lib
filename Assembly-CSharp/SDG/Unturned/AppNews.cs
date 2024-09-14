using System;

namespace SDG.Unturned
{
	// Token: 0x02000796 RID: 1942
	public class AppNews
	{
		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x06004087 RID: 16519 RVA: 0x0014EC59 File Offset: 0x0014CE59
		// (set) Token: 0x06004088 RID: 16520 RVA: 0x0014EC61 File Offset: 0x0014CE61
		public uint AppID { get; set; }

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x06004089 RID: 16521 RVA: 0x0014EC6A File Offset: 0x0014CE6A
		// (set) Token: 0x0600408A RID: 16522 RVA: 0x0014EC72 File Offset: 0x0014CE72
		public NewsItem[] NewsItems { get; set; }
	}
}
