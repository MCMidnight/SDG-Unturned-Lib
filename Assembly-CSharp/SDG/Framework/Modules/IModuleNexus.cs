using System;

namespace SDG.Framework.Modules
{
	/// <summary>
	/// ModuleHook looks for module entry/exit points, then calls <see cref="M:SDG.Framework.Modules.IModuleNexus.initialize" /> when enabled and <see cref="M:SDG.Framework.Modules.IModuleNexus.shutdown" /> when disabled.
	/// </summary>
	// Token: 0x0200008D RID: 141
	public interface IModuleNexus
	{
		/// <summary>
		/// Register components of this module.
		/// </summary>
		// Token: 0x06000367 RID: 871
		void initialize();

		/// <summary>
		/// Cleanup after this module.
		/// </summary>
		// Token: 0x06000368 RID: 872
		void shutdown();
	}
}
