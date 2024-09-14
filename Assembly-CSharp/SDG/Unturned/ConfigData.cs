using System;

namespace SDG.Unturned
{
	// Token: 0x020006D7 RID: 1751
	public class ConfigData
	{
		// Token: 0x06003AE8 RID: 15080 RVA: 0x00112FD0 File Offset: 0x001111D0
		private ConfigData()
		{
			this.Browser = new BrowserConfigData();
			this.Server = new ServerConfigData();
			this.UnityEvents = new UnityEventConfigData();
			this.Easy = new ModeConfigData(EGameMode.EASY);
			this.Normal = new ModeConfigData(EGameMode.NORMAL);
			this.Hard = new ModeConfigData(EGameMode.HARD);
		}

		// Token: 0x06003AE9 RID: 15081 RVA: 0x00113028 File Offset: 0x00111228
		public void InitSingleplayerDefaults()
		{
			this.Easy.InitSingleplayerDefaults();
			this.Normal.InitSingleplayerDefaults();
			this.Hard.InitSingleplayerDefaults();
		}

		// Token: 0x06003AEA RID: 15082 RVA: 0x0011304B File Offset: 0x0011124B
		public void InitDedicatedServerDefaults()
		{
		}

		// Token: 0x06003AEB RID: 15083 RVA: 0x0011304D File Offset: 0x0011124D
		public ModeConfigData getModeConfig(EGameMode mode)
		{
			switch (mode)
			{
			case EGameMode.EASY:
				return this.Easy;
			case EGameMode.NORMAL:
				return this.Normal;
			case EGameMode.HARD:
				return this.Hard;
			default:
				return null;
			}
		}

		// Token: 0x06003AEC RID: 15084 RVA: 0x0011307C File Offset: 0x0011127C
		public static ConfigData CreateDefault(bool singleplayer)
		{
			ConfigData configData = new ConfigData();
			if (singleplayer)
			{
				configData.InitSingleplayerDefaults();
			}
			else
			{
				configData.InitDedicatedServerDefaults();
			}
			return configData;
		}

		// Token: 0x040023A7 RID: 9127
		public BrowserConfigData Browser;

		// Token: 0x040023A8 RID: 9128
		public ServerConfigData Server;

		// Token: 0x040023A9 RID: 9129
		public UnityEventConfigData UnityEvents;

		// Token: 0x040023AA RID: 9130
		public ModeConfigData Easy;

		// Token: 0x040023AB RID: 9131
		public ModeConfigData Normal;

		// Token: 0x040023AC RID: 9132
		public ModeConfigData Hard;
	}
}
