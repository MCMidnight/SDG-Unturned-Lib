using System;
using System.Reflection;

namespace SDG.Unturned
{
	// Token: 0x0200024B RID: 587
	public class ClientMethodInfo
	{
		// Token: 0x060011F1 RID: 4593 RVA: 0x0003D750 File Offset: 0x0003B950
		public override string ToString()
		{
			return this.debugName;
		}

		// Token: 0x0400058D RID: 1421
		internal Type declaringType;

		// Token: 0x0400058E RID: 1422
		internal string name;

		// Token: 0x0400058F RID: 1423
		internal string debugName;

		// Token: 0x04000590 RID: 1424
		internal SteamCall customAttribute;

		// Token: 0x04000591 RID: 1425
		internal ClientMethodReceive readMethod;

		// Token: 0x04000592 RID: 1426
		internal MethodInfo writeMethodInfo;

		// Token: 0x04000593 RID: 1427
		internal uint methodIndex;
	}
}
