using System;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.IO.FormattedFiles.KeyValueTables;

namespace SDG.Unturned
{
	// Token: 0x02000370 RID: 880
	public struct TypeReference<T> : ITypeReference, IFormattedFileReadable, IFormattedFileWritable, IDatParseable, IEquatable<TypeReference<T>>
	{
		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06001A99 RID: 6809 RVA: 0x000600C9 File Offset: 0x0005E2C9
		// (set) Token: 0x06001A9A RID: 6810 RVA: 0x000600D1 File Offset: 0x0005E2D1
		public string assemblyQualifiedName { readonly get; set; }

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06001A9B RID: 6811 RVA: 0x000600DA File Offset: 0x0005E2DA
		public Type type
		{
			get
			{
				if (!string.IsNullOrEmpty(this.assemblyQualifiedName) && this.assemblyQualifiedName.IndexOfAny(DatValue.INVALID_TYPE_CHARS) < 0)
				{
					return Type.GetType(this.assemblyQualifiedName);
				}
				return null;
			}
		}

		/// <summary>
		/// Whether the type has been asigned. Note that this doesn't mean an asset with <see cref="P:SDG.Unturned.TypeReference`1.assemblyQualifiedName" /> exists.
		/// </summary>
		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001A9C RID: 6812 RVA: 0x00060109 File Offset: 0x0005E309
		public bool isValid
		{
			get
			{
				return !string.IsNullOrEmpty(this.assemblyQualifiedName);
			}
		}

		/// <summary>
		/// True if resovling this type reference would get that type.
		/// </summary>
		// Token: 0x06001A9D RID: 6813 RVA: 0x00060119 File Offset: 0x0005E319
		public bool isReferenceTo(Type type)
		{
			return type != null && this.assemblyQualifiedName == type.FullName;
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x00060138 File Offset: 0x0005E338
		public bool TryParse(IDatNode node)
		{
			DatValue datValue = node as DatValue;
			if (datValue != null)
			{
				this.assemblyQualifiedName = datValue.value;
				return true;
			}
			DatDictionary datDictionary = node as DatDictionary;
			if (datDictionary != null)
			{
				this.assemblyQualifiedName = datDictionary.GetString("Type", null);
				return true;
			}
			return false;
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x0006017C File Offset: 0x0005E37C
		public void read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			if (reader == null)
			{
				return;
			}
			this.assemblyQualifiedName = reader.readValue("Type");
			this.assemblyQualifiedName = KeyValueTableTypeRedirectorRegistry.chase(this.assemblyQualifiedName);
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x000601AC File Offset: 0x0005E3AC
		public void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			writer.writeValue("Type", this.assemblyQualifiedName);
			writer.endObject();
		}

		// Token: 0x06001AA1 RID: 6817 RVA: 0x000601CB File Offset: 0x0005E3CB
		public static bool operator ==(TypeReference<T> a, TypeReference<T> b)
		{
			return a.assemblyQualifiedName == b.assemblyQualifiedName;
		}

		// Token: 0x06001AA2 RID: 6818 RVA: 0x000601E0 File Offset: 0x0005E3E0
		public static bool operator !=(TypeReference<T> a, TypeReference<T> b)
		{
			return !(a == b);
		}

		// Token: 0x06001AA3 RID: 6819 RVA: 0x000601EC File Offset: 0x0005E3EC
		public override int GetHashCode()
		{
			return this.assemblyQualifiedName.GetHashCode();
		}

		// Token: 0x06001AA4 RID: 6820 RVA: 0x000601FC File Offset: 0x0005E3FC
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			TypeReference<T> typeReference = (TypeReference<T>)obj;
			return this.assemblyQualifiedName == typeReference.assemblyQualifiedName;
		}

		// Token: 0x06001AA5 RID: 6821 RVA: 0x00060227 File Offset: 0x0005E427
		public override string ToString()
		{
			return this.assemblyQualifiedName;
		}

		// Token: 0x06001AA6 RID: 6822 RVA: 0x0006022F File Offset: 0x0005E42F
		public bool Equals(TypeReference<T> other)
		{
			return this.assemblyQualifiedName == other.assemblyQualifiedName;
		}

		// Token: 0x06001AA7 RID: 6823 RVA: 0x00060243 File Offset: 0x0005E443
		public TypeReference(string assemblyQualifiedName)
		{
			this.assemblyQualifiedName = assemblyQualifiedName;
		}

		// Token: 0x06001AA8 RID: 6824 RVA: 0x0006024C File Offset: 0x0005E44C
		public TypeReference(ITypeReference typeReference)
		{
			this.assemblyQualifiedName = typeReference.assemblyQualifiedName;
		}

		// Token: 0x04000C44 RID: 3140
		public static TypeReference<T> invalid = new TypeReference<T>(null);
	}
}
