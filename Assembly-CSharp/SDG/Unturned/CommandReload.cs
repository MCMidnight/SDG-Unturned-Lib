using System;
using System.IO;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003C3 RID: 963
	public class CommandReload : Command
	{
		// Token: 0x06001D2C RID: 7468 RVA: 0x00069C60 File Offset: 0x00067E60
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Provider.isServer)
			{
				return;
			}
			Guid guid;
			if (!Guid.TryParse(parameter, ref guid))
			{
				if (Directory.Exists(parameter))
				{
					Assets.reload(Path.GetFullPath(parameter));
				}
				return;
			}
			Asset asset = Assets.find(guid);
			if (asset == null)
			{
				return;
			}
			Assets.reload(Path.GetDirectoryName(asset.absoluteOriginFilePath));
		}

		// Token: 0x06001D2D RID: 7469 RVA: 0x00069CB0 File Offset: 0x00067EB0
		public CommandReload(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("ReloadCommandText");
			this._info = this.localization.format("ReloadInfoText");
			this._help = this.localization.format("ReloadHelpText");
		}
	}
}
