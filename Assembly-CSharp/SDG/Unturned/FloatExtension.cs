using System;

namespace SDG.Unturned
{
	// Token: 0x020007F8 RID: 2040
	public static class FloatExtension
	{
		// Token: 0x06004615 RID: 17941 RVA: 0x001A2D84 File Offset: 0x001A0F84
		public static bool IsFinite(this float value)
		{
			return !float.IsInfinity(value) && !float.IsNaN(value);
		}
	}
}
