using System;

namespace SDG.Unturned
{
	// Token: 0x020006EC RID: 1772
	public class ChatPreferenceData
	{
		// Token: 0x06003B0E RID: 15118 RVA: 0x00114573 File Offset: 0x00112773
		public ChatPreferenceData()
		{
			this.Fade_Delay = 10f;
			this.History_Length = 16;
			this.Preview_Length = 5;
		}

		// Token: 0x040024E0 RID: 9440
		public float Fade_Delay;

		// Token: 0x040024E1 RID: 9441
		public int History_Length;

		// Token: 0x040024E2 RID: 9442
		public int Preview_Length;
	}
}
