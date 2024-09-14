using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x02000372 RID: 882
	public class TypeRegistryList
	{
		// Token: 0x06001AAE RID: 6830 RVA: 0x0006030D File Offset: 0x0005E50D
		public List<Type> getTypes()
		{
			return this.typesList;
		}

		// Token: 0x06001AAF RID: 6831 RVA: 0x00060318 File Offset: 0x0005E518
		public void addType(Type type)
		{
			if (this.baseType.IsAssignableFrom(type))
			{
				this.typesList.Add(type);
				return;
			}
			Type type2 = this.baseType;
			throw new ArgumentException(((type2 != null) ? type2.ToString() : null) + " is not assignable from " + ((type != null) ? type.ToString() : null), "type");
		}

		// Token: 0x06001AB0 RID: 6832 RVA: 0x00060373 File Offset: 0x0005E573
		public void removeType(Type type)
		{
			this.typesList.RemoveFast(type);
		}

		// Token: 0x06001AB1 RID: 6833 RVA: 0x00060382 File Offset: 0x0005E582
		public TypeRegistryList(Type newBaseType)
		{
			this.baseType = newBaseType;
		}

		// Token: 0x04000C48 RID: 3144
		private Type baseType;

		// Token: 0x04000C49 RID: 3145
		private List<Type> typesList = new List<Type>();
	}
}
