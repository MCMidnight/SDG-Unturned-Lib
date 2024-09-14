using System;

namespace SDG.Framework.Devkit.Tools
{
	// Token: 0x02000142 RID: 322
	public enum EDevkitLandscapeToolHeightmapFlattenMethod
	{
		/// <summary>
		/// Directly blend current value toward target value.
		/// </summary>
		// Token: 0x040002FE RID: 766
		REGULAR,
		/// <summary>
		/// Only blend current value toward target value if current is greater than target.
		/// </summary>
		// Token: 0x040002FF RID: 767
		MIN,
		/// <summary>
		/// Only blend current value toward target value if current is less than target.
		/// </summary>
		// Token: 0x04000300 RID: 768
		MAX
	}
}
