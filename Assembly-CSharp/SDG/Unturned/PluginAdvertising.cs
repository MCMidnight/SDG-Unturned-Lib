using System;

namespace SDG.Unturned
{
	// Token: 0x0200067D RID: 1661
	public static class PluginAdvertising
	{
		// Token: 0x06003702 RID: 14082 RVA: 0x00101BD2 File Offset: 0x000FFDD2
		public static IPluginAdvertising Get()
		{
			return SteamPluginAdvertising.Get();
		}
	}
}
