using System;

namespace SDG.Framework.Debug
{
	// Token: 0x02000153 RID: 339
	public struct InspectableFilePath : IInspectablePath
	{
		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000873 RID: 2163 RVA: 0x0001DD8B File Offset: 0x0001BF8B
		// (set) Token: 0x06000874 RID: 2164 RVA: 0x0001DD93 File Offset: 0x0001BF93
		public string absolutePath { readonly get; set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000875 RID: 2165 RVA: 0x0001DD9C File Offset: 0x0001BF9C
		// (set) Token: 0x06000876 RID: 2166 RVA: 0x0001DDA4 File Offset: 0x0001BFA4
		public string extension { readonly get; private set; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000877 RID: 2167 RVA: 0x0001DDAD File Offset: 0x0001BFAD
		public bool isValid
		{
			get
			{
				return !string.IsNullOrEmpty(this.absolutePath);
			}
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x0001DDBD File Offset: 0x0001BFBD
		public override string ToString()
		{
			return this.absolutePath;
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x0001DDC5 File Offset: 0x0001BFC5
		public InspectableFilePath(string newExtension)
		{
			this.absolutePath = string.Empty;
			this.extension = newExtension;
		}
	}
}
