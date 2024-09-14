using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SDG.Framework.Modules
{
	/// <summary>
	/// Holds module configuration.
	/// </summary>
	// Token: 0x02000094 RID: 148
	public class ModuleConfig
	{
		// Token: 0x0600038D RID: 909 RVA: 0x0000DC64 File Offset: 0x0000BE64
		public ModuleConfig()
		{
			this.IsEnabled = true;
			this.Name = string.Empty;
			this.Version = "1.0.0.0";
			this.Dependencies = new List<ModuleDependency>(0);
			this.Assemblies = new List<ModuleAssembly>(0);
		}

		/// <summary>
		/// Whether to load assemblies.
		/// </summary>
		// Token: 0x04000183 RID: 387
		public bool IsEnabled;

		/// <summary>
		/// Directory containing Module file, set when loading.
		/// </summary>
		// Token: 0x04000184 RID: 388
		[JsonIgnore]
		public string DirectoryPath;

		/// <summary>
		/// Path to the Module file, set when loading.
		/// </summary>
		// Token: 0x04000185 RID: 389
		[JsonIgnore]
		public string FilePath;

		/// <summary>
		/// Used for module dependencies.
		/// </summary>
		// Token: 0x04000186 RID: 390
		public string Name;

		/// <summary>
		/// Nicely formatted version, converted into <see cref="F:SDG.Framework.Modules.ModuleConfig.Version_Internal" />.
		/// </summary>
		// Token: 0x04000187 RID: 391
		public string Version;

		/// <summary>
		/// Used for module dependencies.
		/// </summary>
		// Token: 0x04000188 RID: 392
		[JsonIgnore]
		public uint Version_Internal;

		/// <summary>
		/// Modules that must be loaded before this module.
		/// </summary>
		// Token: 0x04000189 RID: 393
		public List<ModuleDependency> Dependencies;

		/// <summary>
		/// Relative file paths of .dlls to load.
		/// </summary>
		// Token: 0x0400018A RID: 394
		public List<ModuleAssembly> Assemblies;
	}
}
