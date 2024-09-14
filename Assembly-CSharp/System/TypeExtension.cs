using System;

namespace System
{
	// Token: 0x02000012 RID: 18
	public static class TypeExtension
	{
		// Token: 0x06000043 RID: 67 RVA: 0x00003192 File Offset: 0x00001392
		public static object getDefaultValue(this Type type)
		{
			if (type.IsValueType)
			{
				return Activator.CreateInstance(type);
			}
			return null;
		}
	}
}
