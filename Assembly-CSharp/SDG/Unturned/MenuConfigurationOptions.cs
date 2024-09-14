using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020005B8 RID: 1464
	public class MenuConfigurationOptions : MonoBehaviour
	{
		// Token: 0x06002F9E RID: 12190 RVA: 0x000D247A File Offset: 0x000D067A
		public static void apply()
		{
			if (!MenuConfigurationOptions.hasPlayed)
			{
				MenuConfigurationOptions.hasPlayed = true;
				if (MenuConfigurationOptions.music != null)
				{
					MenuConfigurationOptions.music.enabled = (OptionsSettings.MainMenuMusicVolume > 0f);
				}
			}
		}

		// Token: 0x06002F9F RID: 12191 RVA: 0x000D24AC File Offset: 0x000D06AC
		private void Start()
		{
			MenuConfigurationOptions.apply();
		}

		// Token: 0x06002FA0 RID: 12192 RVA: 0x000D24B3 File Offset: 0x000D06B3
		private void Awake()
		{
			MenuConfigurationOptions.music = base.GetComponent<AudioSource>();
		}

		// Token: 0x040019AF RID: 6575
		private static bool hasPlayed;

		// Token: 0x040019B0 RID: 6576
		private static AudioSource music;
	}
}
