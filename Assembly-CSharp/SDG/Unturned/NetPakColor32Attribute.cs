using System;
using System.Diagnostics;

namespace SDG.Unturned
{
	// Token: 0x02000242 RID: 578
	[Conditional("UNITY_EDITOR")]
	[AttributeUsage(2048)]
	public class NetPakColor32Attribute : Attribute
	{
		// Token: 0x060011E2 RID: 4578 RVA: 0x0003D6E0 File Offset: 0x0003B8E0
		public NetPakColor32Attribute(bool withAlpha)
		{
			this.withAlpha = withAlpha;
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x0003D6EF File Offset: 0x0003B8EF
		private NetPakColor32Attribute()
		{
		}

		// Token: 0x04000582 RID: 1410
		public bool withAlpha;
	}
}
