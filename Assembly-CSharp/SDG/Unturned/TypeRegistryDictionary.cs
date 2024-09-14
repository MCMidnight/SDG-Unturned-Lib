using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x02000371 RID: 881
	public class TypeRegistryDictionary
	{
		// Token: 0x06001AAA RID: 6826 RVA: 0x00060268 File Offset: 0x0005E468
		public Type getType(string key)
		{
			Type result = null;
			this.typesDictionary.TryGetValue(key, ref result);
			return result;
		}

		// Token: 0x06001AAB RID: 6827 RVA: 0x00060288 File Offset: 0x0005E488
		public void addType(string key, Type value)
		{
			if (this.baseType.IsAssignableFrom(value))
			{
				this.typesDictionary.Add(key, value);
				return;
			}
			Type type = this.baseType;
			throw new ArgumentException(((type != null) ? type.ToString() : null) + " is not assignable from " + ((value != null) ? value.ToString() : null), "value");
		}

		// Token: 0x06001AAC RID: 6828 RVA: 0x000602E4 File Offset: 0x0005E4E4
		public void removeType(string key)
		{
			this.typesDictionary.Remove(key);
		}

		// Token: 0x06001AAD RID: 6829 RVA: 0x000602F3 File Offset: 0x0005E4F3
		public TypeRegistryDictionary(Type newBaseType)
		{
			this.baseType = newBaseType;
		}

		// Token: 0x04000C46 RID: 3142
		private Type baseType;

		// Token: 0x04000C47 RID: 3143
		private Dictionary<string, Type> typesDictionary = new Dictionary<string, Type>();
	}
}
