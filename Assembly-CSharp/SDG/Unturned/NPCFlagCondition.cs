using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x02000327 RID: 807
	public class NPCFlagCondition : NPCLogicCondition
	{
		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06001849 RID: 6217 RVA: 0x00058C64 File Offset: 0x00056E64
		// (set) Token: 0x0600184A RID: 6218 RVA: 0x00058C6C File Offset: 0x00056E6C
		public ushort id { get; protected set; }

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x0600184B RID: 6219 RVA: 0x00058C75 File Offset: 0x00056E75
		// (set) Token: 0x0600184C RID: 6220 RVA: 0x00058C7D File Offset: 0x00056E7D
		public bool allowUnset { get; protected set; }

		// Token: 0x0600184D RID: 6221 RVA: 0x00058C86 File Offset: 0x00056E86
		public override bool isAssociatedWithFlag(ushort flagID)
		{
			return flagID == this.id;
		}

		// Token: 0x0600184E RID: 6222 RVA: 0x00058C91 File Offset: 0x00056E91
		internal override void GatherAssociatedFlags(HashSet<ushort> associatedFlags)
		{
			associatedFlags.Add(this.id);
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x00058CA0 File Offset: 0x00056EA0
		public NPCFlagCondition(ushort newID, bool newAllowUnset, ENPCLogicType newLogicType, string newText, bool newShouldReset) : base(newLogicType, newText, newShouldReset)
		{
			this.id = newID;
			this.allowUnset = newAllowUnset;
		}
	}
}
