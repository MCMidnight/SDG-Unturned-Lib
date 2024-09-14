using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SDG.Framework.Modules
{
	// Token: 0x02000092 RID: 146
	public class ModuleAssembly
	{
		// Token: 0x0600038A RID: 906 RVA: 0x0000DBAA File Offset: 0x0000BDAA
		public ModuleAssembly()
		{
			this.Path = string.Empty;
			this.Role = EModuleRole.None;
			this.Load_As_Byte_Array = false;
		}

		// Token: 0x04000180 RID: 384
		public string Path;

		// Token: 0x04000181 RID: 385
		[JsonConverter(typeof(StringEnumConverter))]
		public EModuleRole Role;

		/// <summary>
		/// Requested by Trojaner. LoadFile locks the file while in use which prevents OpenMod from updating itself.
		/// </summary>
		// Token: 0x04000182 RID: 386
		public bool Load_As_Byte_Array;
	}
}
