using System;

namespace SDG.Framework.Devkit
{
	// Token: 0x0200011A RID: 282
	public interface IDirtyable
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000741 RID: 1857
		// (set) Token: 0x06000742 RID: 1858
		bool isDirty { get; set; }

		// Token: 0x06000743 RID: 1859
		void save();
	}
}
