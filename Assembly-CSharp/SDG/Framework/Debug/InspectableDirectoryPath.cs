using System;

namespace SDG.Framework.Debug
{
	// Token: 0x02000152 RID: 338
	public struct InspectableDirectoryPath : IInspectablePath
	{
		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600086F RID: 2159 RVA: 0x0001DD62 File Offset: 0x0001BF62
		// (set) Token: 0x06000870 RID: 2160 RVA: 0x0001DD6A File Offset: 0x0001BF6A
		public string absolutePath { readonly get; set; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000871 RID: 2161 RVA: 0x0001DD73 File Offset: 0x0001BF73
		public bool isValid
		{
			get
			{
				return !string.IsNullOrEmpty(this.absolutePath);
			}
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x0001DD83 File Offset: 0x0001BF83
		public override string ToString()
		{
			return this.absolutePath;
		}
	}
}
