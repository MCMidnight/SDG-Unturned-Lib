using System;
using SDG.Provider.Services;
using SDG.Provider.Services.Translation;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Translation
{
	// Token: 0x02000017 RID: 23
	public class SteamworksTranslationService : Service, ITranslationService, IService
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000071 RID: 113 RVA: 0x000036EB File Offset: 0x000018EB
		// (set) Token: 0x06000072 RID: 114 RVA: 0x000036F3 File Offset: 0x000018F3
		public string language { get; protected set; }

		// Token: 0x06000073 RID: 115 RVA: 0x000036FC File Offset: 0x000018FC
		public override void initialize()
		{
			this.language = SteamUtils.GetSteamUILanguage();
		}
	}
}
