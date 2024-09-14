using System;

namespace SDG.Unturned
{
	// Token: 0x02000614 RID: 1556
	public class IgnoredCraftingBlueprint
	{
		// Token: 0x060031F1 RID: 12785 RVA: 0x000DD829 File Offset: 0x000DBA29
		public bool matchesBlueprint(Blueprint blueprint)
		{
			return this.itemId == blueprint.sourceItem.id && this.blueprintIndex == blueprint.id;
		}

		// Token: 0x04001C61 RID: 7265
		public ushort itemId;

		// Token: 0x04001C62 RID: 7266
		public byte blueprintIndex;
	}
}
