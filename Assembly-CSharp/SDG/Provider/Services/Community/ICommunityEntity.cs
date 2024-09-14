using System;
using SDG.Framework.IO.Streams;

namespace SDG.Provider.Services.Community
{
	// Token: 0x02000063 RID: 99
	public interface ICommunityEntity : INetworkStreamable
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000235 RID: 565
		bool isValid { get; }
	}
}
