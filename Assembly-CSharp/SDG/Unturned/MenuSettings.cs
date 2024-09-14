using System;

namespace SDG.Unturned
{
	// Token: 0x020005BB RID: 1467
	public class MenuSettings
	{
		// Token: 0x06002FA8 RID: 12200 RVA: 0x000D269F File Offset: 0x000D089F
		public static void load()
		{
			FilterSettings.load();
			PlaySettings.load();
			GraphicsSettings.load();
			ControlsSettings.load();
			OptionsSettings.load();
			MenuSettings.hasLoaded = true;
		}

		// Token: 0x06002FA9 RID: 12201 RVA: 0x000D26C0 File Offset: 0x000D08C0
		public static void save()
		{
			if (!MenuSettings.hasLoaded)
			{
				return;
			}
			FilterSettings.save();
			PlaySettings.save();
			GraphicsSettings.save();
			ControlsSettings.save();
			OptionsSettings.save();
		}

		// Token: 0x06002FAA RID: 12202 RVA: 0x000D26E3 File Offset: 0x000D08E3
		public static void SaveGraphicsIfLoaded()
		{
			if (MenuSettings.hasLoaded)
			{
				GraphicsSettings.save();
			}
		}

		// Token: 0x06002FAB RID: 12203 RVA: 0x000D26F1 File Offset: 0x000D08F1
		public static void SaveControlsIfLoaded()
		{
			if (MenuSettings.hasLoaded)
			{
				ControlsSettings.save();
			}
		}

		// Token: 0x06002FAC RID: 12204 RVA: 0x000D26FF File Offset: 0x000D08FF
		public static void SaveOptionsIfLoaded()
		{
			if (MenuSettings.hasLoaded)
			{
				OptionsSettings.save();
			}
		}

		// Token: 0x040019B3 RID: 6579
		private static bool hasLoaded;
	}
}
