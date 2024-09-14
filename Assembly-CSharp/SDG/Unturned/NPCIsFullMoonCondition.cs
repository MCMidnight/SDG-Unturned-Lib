using System;

namespace SDG.Unturned
{
	// Token: 0x0200032D RID: 813
	public class NPCIsFullMoonCondition : INPCCondition
	{
		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x0600186A RID: 6250 RVA: 0x00058F31 File Offset: 0x00057131
		// (set) Token: 0x0600186B RID: 6251 RVA: 0x00058F39 File Offset: 0x00057139
		public bool value { get; protected set; }

		// Token: 0x0600186C RID: 6252 RVA: 0x00058F42 File Offset: 0x00057142
		public override bool isConditionMet(Player player)
		{
			return LightingManager.isFullMoon == this.value;
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x00058F51 File Offset: 0x00057151
		public NPCIsFullMoonCondition(bool newValue, string newText) : base(newText, false)
		{
			this.value = newValue;
		}
	}
}
