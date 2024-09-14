using System;

namespace SDG.Provider.Services.Matchmaking
{
	// Token: 0x02000046 RID: 70
	public interface IMatchmakingService : IService
	{
		// Token: 0x060001E3 RID: 483
		IServerInfoRequestHandle requestServerInfo(uint ip, ushort port, ServerInfoRequestReadyCallback callback);
	}
}
