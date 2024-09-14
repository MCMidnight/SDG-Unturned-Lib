using System;
using Newtonsoft.Json;

namespace SDG.Framework.Modules
{
	// Token: 0x02000095 RID: 149
	public class ModuleDependency
	{
		// Token: 0x0600038E RID: 910 RVA: 0x0000DCA1 File Offset: 0x0000BEA1
		public ModuleDependency()
		{
			this.Name = string.Empty;
			this.Version = "1.0.0.0";
		}

		// Token: 0x0400018B RID: 395
		public string Name;

		/// <summary>
		/// Nicely formatted version, converted into <see cref="F:SDG.Framework.Modules.ModuleDependency.Version_Internal" />.
		/// </summary>
		// Token: 0x0400018C RID: 396
		public string Version;

		/// <summary>
		/// Used for module dependencies.
		/// </summary>
		// Token: 0x0400018D RID: 397
		[JsonIgnore]
		public uint Version_Internal;
	}
}
