using System;

namespace SDG.Unturned
{
	// Token: 0x02000550 RID: 1360
	public class ClaimBase
	{
		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06002B20 RID: 11040 RVA: 0x000B818B File Offset: 0x000B638B
		public bool hasOwnership
		{
			get
			{
				return OwnershipTool.checkToggle(this.owner, this.group);
			}
		}

		// Token: 0x06002B21 RID: 11041 RVA: 0x000B819E File Offset: 0x000B639E
		public ClaimBase(ulong newOwner, ulong newGroup)
		{
			this.owner = newOwner;
			this.group = newGroup;
		}

		// Token: 0x040016FE RID: 5886
		public ulong owner;

		// Token: 0x040016FF RID: 5887
		public ulong group;
	}
}
