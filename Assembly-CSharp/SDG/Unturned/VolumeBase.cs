using System;
using SDG.Framework.Devkit;

namespace SDG.Unturned
{
	// Token: 0x02000517 RID: 1303
	public abstract class VolumeBase : DevkitHierarchyWorldItem
	{
		// Token: 0x060028BC RID: 10428 RVA: 0x000AD8BB File Offset: 0x000ABABB
		public virtual ISleekElement CreateMenu()
		{
			return null;
		}
	}
}
