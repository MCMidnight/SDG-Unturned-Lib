using System;

namespace SDG.Unturned
{
	// Token: 0x020006EA RID: 1770
	public class GraphicsPreferenceData
	{
		// Token: 0x06003B0B RID: 15115 RVA: 0x001143C4 File Offset: 0x001125C4
		public GraphicsPreferenceData()
		{
			this.Use_Skybox_Ambience = false;
			this.Use_Lens_Dirt = true;
			this.Chromatic_Aberration_Intensity = 0.2f;
			this.LOD_Bias = 0f;
			this.Override_Resolution_Width = -1;
			this.Override_Resolution_Height = -1;
			this.Override_Refresh_Rate = -1;
			this.Override_UI_Scale = -1f;
			this.Override_Fullscreen_Mode = -1;
			this.Override_Vertical_Field_Of_View = -1f;
			this.Inconspicuous_Text_Contrast = 0;
			this.Colorful_Text_Contrast = 0;
		}

		// Token: 0x040024CE RID: 9422
		public bool Use_Skybox_Ambience;

		// Token: 0x040024CF RID: 9423
		public bool Use_Lens_Dirt;

		// Token: 0x040024D0 RID: 9424
		public float Chromatic_Aberration_Intensity;

		// Token: 0x040024D1 RID: 9425
		public float LOD_Bias;

		// Token: 0x040024D2 RID: 9426
		public int Override_Resolution_Width;

		// Token: 0x040024D3 RID: 9427
		public int Override_Resolution_Height;

		// Token: 0x040024D4 RID: 9428
		public float Override_UI_Scale;

		// Token: 0x040024D5 RID: 9429
		public int Override_Fullscreen_Mode;

		// Token: 0x040024D6 RID: 9430
		public int Override_Refresh_Rate;

		// Token: 0x040024D7 RID: 9431
		public float Override_Vertical_Field_Of_View;

		// Token: 0x040024D8 RID: 9432
		public ETextContrastPreference Default_Text_Contrast;

		// Token: 0x040024D9 RID: 9433
		public ETextContrastPreference Inconspicuous_Text_Contrast;

		// Token: 0x040024DA RID: 9434
		public ETextContrastPreference Colorful_Text_Contrast;
	}
}
