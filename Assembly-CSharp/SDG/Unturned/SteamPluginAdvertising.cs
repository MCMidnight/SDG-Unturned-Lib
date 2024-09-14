using System;
using System.Collections.Generic;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x0200067F RID: 1663
	internal class SteamPluginAdvertising : IPluginAdvertising
	{
		// Token: 0x0600370A RID: 14090 RVA: 0x00101BD9 File Offset: 0x000FFDD9
		public static SteamPluginAdvertising Get()
		{
			if (SteamPluginAdvertising.instance == null)
			{
				SteamPluginAdvertising.instance = new SteamPluginAdvertising();
			}
			return SteamPluginAdvertising.instance;
		}

		// Token: 0x0600370B RID: 14091 RVA: 0x00101BF1 File Offset: 0x000FFDF1
		public void AddPlugin(string name)
		{
			if (this.pluginNames.Add(name))
			{
				this.UpdateKeyValue();
			}
		}

		// Token: 0x0600370C RID: 14092 RVA: 0x00101C08 File Offset: 0x000FFE08
		public void AddPlugins(IEnumerable<string> names)
		{
			int count = this.pluginNames.Count;
			this.pluginNames.UnionWith(names);
			if (this.pluginNames.Count > count)
			{
				this.UpdateKeyValue();
			}
		}

		// Token: 0x0600370D RID: 14093 RVA: 0x00101C41 File Offset: 0x000FFE41
		public void RemovePlugin(string name)
		{
			if (this.pluginNames.Remove(name))
			{
				this.UpdateKeyValue();
			}
		}

		// Token: 0x0600370E RID: 14094 RVA: 0x00101C58 File Offset: 0x000FFE58
		public void RemovePlugins(IEnumerable<string> names)
		{
			int count = this.pluginNames.Count;
			this.pluginNames.ExceptWith(names);
			if (this.pluginNames.Count < count)
			{
				this.UpdateKeyValue();
			}
		}

		// Token: 0x0600370F RID: 14095 RVA: 0x00101C91 File Offset: 0x000FFE91
		public IEnumerable<string> GetPluginNames()
		{
			return this.pluginNames;
		}

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x06003710 RID: 14096 RVA: 0x00101C99 File Offset: 0x000FFE99
		// (set) Token: 0x06003711 RID: 14097 RVA: 0x00101CA4 File Offset: 0x000FFEA4
		public string PluginFrameworkName
		{
			get
			{
				return this.pluginFrameworkName;
			}
			set
			{
				if (this.isGameServerReady)
				{
					UnturnedLog.warn("Cannot change advertised plugin framework after server startup");
					return;
				}
				this.pluginFrameworkName = value;
				if (string.IsNullOrEmpty(this.pluginFrameworkName))
				{
					this.PluginFrameworkTag = null;
					return;
				}
				if (this.pluginFrameworkName.Equals("rocket"))
				{
					this.PluginFrameworkTag = "rm";
					return;
				}
				if (this.pluginFrameworkName.Equals("openmod"))
				{
					this.PluginFrameworkTag = "om";
					return;
				}
				this.PluginFrameworkTag = null;
				UnturnedLog.warn("Cannot advertise unknown plugin framework name \"{0}\"", new object[]
				{
					this.pluginFrameworkName
				});
			}
		}

		/// <summary>
		/// Called once key/values can be set.
		/// </summary>
		// Token: 0x06003712 RID: 14098 RVA: 0x00101D3C File Offset: 0x000FFF3C
		public void NotifyGameServerReady()
		{
			this.isGameServerReady = true;
			this.UpdateKeyValue();
		}

		// Token: 0x06003713 RID: 14099 RVA: 0x00101D4B File Offset: 0x000FFF4B
		public void UpdateKeyValue()
		{
			if (!this.isGameServerReady)
			{
				return;
			}
			SteamGameServer.SetKeyValue("rocketplugins", string.Join(",", this.pluginNames));
		}

		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x06003714 RID: 14100 RVA: 0x00101D70 File Offset: 0x000FFF70
		// (set) Token: 0x06003715 RID: 14101 RVA: 0x00101D78 File Offset: 0x000FFF78
		public string PluginFrameworkTag { get; private set; }

		// Token: 0x04002089 RID: 8329
		private bool isGameServerReady;

		// Token: 0x0400208A RID: 8330
		private HashSet<string> pluginNames = new HashSet<string>();

		// Token: 0x0400208B RID: 8331
		private string pluginFrameworkName = string.Empty;

		// Token: 0x0400208C RID: 8332
		private static SteamPluginAdvertising instance;
	}
}
