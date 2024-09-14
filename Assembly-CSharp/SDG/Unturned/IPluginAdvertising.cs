using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x0200067E RID: 1662
	public interface IPluginAdvertising
	{
		// Token: 0x06003703 RID: 14083
		void AddPlugin(string name);

		// Token: 0x06003704 RID: 14084
		void AddPlugins(IEnumerable<string> names);

		// Token: 0x06003705 RID: 14085
		void RemovePlugin(string name);

		// Token: 0x06003706 RID: 14086
		void RemovePlugins(IEnumerable<string> names);

		// Token: 0x06003707 RID: 14087
		IEnumerable<string> GetPluginNames();

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x06003708 RID: 14088
		// (set) Token: 0x06003709 RID: 14089
		string PluginFrameworkName { get; set; }
	}
}
