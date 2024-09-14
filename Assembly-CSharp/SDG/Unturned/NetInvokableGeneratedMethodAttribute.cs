using System;

namespace SDG.Unturned
{
	// Token: 0x02000248 RID: 584
	[AttributeUsage(64, AllowMultiple = false)]
	public class NetInvokableGeneratedMethodAttribute : Attribute
	{
		// Token: 0x060011E8 RID: 4584 RVA: 0x0003D73A File Offset: 0x0003B93A
		public NetInvokableGeneratedMethodAttribute(string targetMethodName, ENetInvokableGeneratedMethodPurpose purpose)
		{
			this.targetMethodName = targetMethodName;
			this.purpose = purpose;
		}

		/// <summary>
		/// Method the annotated method was generated for.
		/// </summary>
		// Token: 0x0400058B RID: 1419
		public readonly string targetMethodName;

		// Token: 0x0400058C RID: 1420
		public readonly ENetInvokableGeneratedMethodPurpose purpose;
	}
}
