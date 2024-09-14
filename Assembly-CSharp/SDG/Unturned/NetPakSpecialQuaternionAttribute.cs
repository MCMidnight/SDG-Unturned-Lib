using System;
using System.Diagnostics;

namespace SDG.Unturned
{
	// Token: 0x02000245 RID: 581
	[Conditional("UNITY_EDITOR")]
	[AttributeUsage(2048)]
	public class NetPakSpecialQuaternionAttribute : Attribute
	{
		// Token: 0x060011E6 RID: 4582 RVA: 0x0003D71C File Offset: 0x0003B91C
		public NetPakSpecialQuaternionAttribute(int yawBitCount = 9)
		{
			this.yawBitCount = yawBitCount;
		}

		// Token: 0x04000586 RID: 1414
		public readonly int yawBitCount;
	}
}
