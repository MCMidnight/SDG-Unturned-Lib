using System;
using System.Reflection;

namespace SDG.Unturned
{
	// Token: 0x0200024C RID: 588
	public class ServerMethodInfo
	{
		// Token: 0x060011F3 RID: 4595 RVA: 0x0003D760 File Offset: 0x0003B960
		public override string ToString()
		{
			return this.debugName;
		}

		// Token: 0x04000594 RID: 1428
		internal Type declaringType;

		// Token: 0x04000595 RID: 1429
		internal string name;

		// Token: 0x04000596 RID: 1430
		internal string debugName;

		// Token: 0x04000597 RID: 1431
		internal SteamCall customAttribute;

		// Token: 0x04000598 RID: 1432
		internal ServerMethodReceive readMethod;

		// Token: 0x04000599 RID: 1433
		internal MethodInfo writeMethodInfo;

		// Token: 0x0400059A RID: 1434
		internal uint methodIndex;

		/// <summary>
		/// Index into per-connection rate limiting array.
		/// </summary>
		// Token: 0x0400059B RID: 1435
		internal int rateLimitIndex;
	}
}
