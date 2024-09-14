using System;
using SDG.Provider.Services.Multiplayer.Client;
using SDG.Provider.Services.Multiplayer.Server;

namespace SDG.Provider.Services.Multiplayer
{
	// Token: 0x0200004F RID: 79
	public interface IMultiplayerService : IService
	{
		/// <summary>
		/// Current client multiplayer implementation.
		/// </summary>
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001F0 RID: 496
		IClientMultiplayerService clientMultiplayerService { get; }

		/// <summary>
		/// Current server multiplayer implementation.
		/// </summary>
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001F1 RID: 497
		IServerMultiplayerService serverMultiplayerService { get; }
	}
}
