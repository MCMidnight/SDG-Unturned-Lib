using System;
using System.Diagnostics;

namespace SDG.Unturned
{
	// Token: 0x02000244 RID: 580
	[Conditional("UNITY_EDITOR")]
	[AttributeUsage(2048)]
	public class NetPakNormalAttribute : Attribute
	{
		// Token: 0x060011E5 RID: 4581 RVA: 0x0003D70D File Offset: 0x0003B90D
		public NetPakNormalAttribute(int bitsPerComponent = 9)
		{
			this.bitsPerComponent = bitsPerComponent;
		}

		// Token: 0x04000585 RID: 1413
		public readonly int bitsPerComponent;
	}
}
