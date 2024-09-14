using System;

namespace SDG.Unturned
{
	// Token: 0x02000678 RID: 1656
	public class LocalWorkshopSettings
	{
		// Token: 0x060036F2 RID: 14066 RVA: 0x0010170E File Offset: 0x000FF90E
		public static ILocalWorkshopSettings get()
		{
			if (LocalWorkshopSettings.instance == null)
			{
				LocalWorkshopSettings.instance = new LocalWorkshopSettingsImplementation();
			}
			return LocalWorkshopSettings.instance;
		}

		// Token: 0x0400207F RID: 8319
		private static ILocalWorkshopSettings instance;
	}
}
