using System;

namespace SDG.Unturned
{
	// Token: 0x02000246 RID: 582
	[AttributeUsage(4, AllowMultiple = false)]
	public class NetInvokableGeneratedClassAttribute : Attribute
	{
		// Token: 0x060011E7 RID: 4583 RVA: 0x0003D72B File Offset: 0x0003B92B
		public NetInvokableGeneratedClassAttribute(Type targetType)
		{
			this.targetType = targetType;
		}

		/// <summary>
		/// Type the annotated class was generated for.
		/// </summary>
		// Token: 0x04000587 RID: 1415
		public readonly Type targetType;
	}
}
