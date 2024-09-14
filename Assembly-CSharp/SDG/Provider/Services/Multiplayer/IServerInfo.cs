using System;
using SDG.Provider.Services.Community;

namespace SDG.Provider.Services.Multiplayer
{
	// Token: 0x02000050 RID: 80
	public interface IServerInfo
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001F2 RID: 498
		ICommunityEntity entity { get; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001F3 RID: 499
		string name { get; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001F4 RID: 500
		byte players { get; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001F5 RID: 501
		byte capacity { get; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001F6 RID: 502
		int ping { get; }
	}
}
