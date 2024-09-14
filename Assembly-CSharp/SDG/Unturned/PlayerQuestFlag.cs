using System;

namespace SDG.Unturned
{
	// Token: 0x0200064B RID: 1611
	public class PlayerQuestFlag
	{
		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x060034C3 RID: 13507 RVA: 0x000F4352 File Offset: 0x000F2552
		// (set) Token: 0x060034C4 RID: 13508 RVA: 0x000F435A File Offset: 0x000F255A
		public ushort id { get; private set; }

		// Token: 0x060034C5 RID: 13509 RVA: 0x000F4363 File Offset: 0x000F2563
		public PlayerQuestFlag(ushort newID, short newValue)
		{
			this.id = newID;
			this.value = newValue;
		}

		// Token: 0x04001EA7 RID: 7847
		public short value;
	}
}
