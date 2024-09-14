using System;

namespace SDG.Provider.Services
{
	// Token: 0x0200003A RID: 58
	public interface IService
	{
		/// <summary>
		/// Initialize this service's external API. Should be called before using.
		/// </summary>
		// Token: 0x060001C2 RID: 450
		void initialize();

		/// <summary>
		/// Update this service's external API. Should be called every frame.
		/// </summary>
		// Token: 0x060001C3 RID: 451
		void update();

		/// <summary>
		/// Shutdown this service's external API. Should be called before closing the program.
		/// </summary>
		// Token: 0x060001C4 RID: 452
		void shutdown();
	}
}
