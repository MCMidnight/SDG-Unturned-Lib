using System;

namespace SDG.Unturned
{
	// Token: 0x020006E8 RID: 1768
	public class PreferenceData
	{
		// Token: 0x06003B09 RID: 15113 RVA: 0x00114374 File Offset: 0x00112574
		public PreferenceData()
		{
			this.Allow_Ctrl_Shift_Alt_Salvage = false;
			this.Audio = new AudioPreferenceData();
			this.Graphics = new GraphicsPreferenceData();
			this.Viewmodel = new ViewmodelPreferenceData();
			this.Chat = new ChatPreferenceData();
		}

		// Token: 0x040024C8 RID: 9416
		public bool Allow_Ctrl_Shift_Alt_Salvage;

		// Token: 0x040024C9 RID: 9417
		public AudioPreferenceData Audio;

		// Token: 0x040024CA RID: 9418
		public GraphicsPreferenceData Graphics;

		// Token: 0x040024CB RID: 9419
		public ViewmodelPreferenceData Viewmodel;

		// Token: 0x040024CC RID: 9420
		public ChatPreferenceData Chat;
	}
}
