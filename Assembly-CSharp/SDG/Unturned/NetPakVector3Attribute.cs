using System;
using System.Diagnostics;

namespace SDG.Unturned
{
	// Token: 0x02000243 RID: 579
	[Conditional("UNITY_EDITOR")]
	[AttributeUsage(2048)]
	public class NetPakVector3Attribute : Attribute
	{
		// Token: 0x060011E4 RID: 4580 RVA: 0x0003D6F7 File Offset: 0x0003B8F7
		public NetPakVector3Attribute(int intBitCount = 13, int fracBitCount = 9)
		{
			this.intBitCount = intBitCount;
			this.fracBitCount = fracBitCount;
		}

		// Token: 0x04000583 RID: 1411
		public readonly int intBitCount;

		// Token: 0x04000584 RID: 1412
		public readonly int fracBitCount;
	}
}
