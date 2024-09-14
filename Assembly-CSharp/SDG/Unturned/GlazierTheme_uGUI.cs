using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000184 RID: 388
	internal class GlazierTheme_uGUI
	{
		// Token: 0x06000AD4 RID: 2772 RVA: 0x0002460C File Offset: 0x0002280C
		public GlazierTheme_uGUI(string prefix)
		{
			this.BoxSprite = new StaticResourceRef<Sprite>(prefix + "/Box");
			this.BoxHighlightedSprite = new StaticResourceRef<Sprite>(prefix + "/Box_Highlighted");
			this.BoxPressedSprite = new StaticResourceRef<Sprite>(prefix + "/Box_Pressed");
			this.SliderBackgroundSprite = new StaticResourceRef<Sprite>(prefix + "/Slider_Background");
			this.ToggleForegroundSprite = new StaticResourceRef<Sprite>(prefix + "/Toggle_Foreground");
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x0002468D File Offset: 0x0002288D
		// (set) Token: 0x06000AD6 RID: 2774 RVA: 0x00024695 File Offset: 0x00022895
		public StaticResourceRef<Sprite> BoxSprite { get; private set; }

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000AD7 RID: 2775 RVA: 0x0002469E File Offset: 0x0002289E
		// (set) Token: 0x06000AD8 RID: 2776 RVA: 0x000246A6 File Offset: 0x000228A6
		public StaticResourceRef<Sprite> BoxHighlightedSprite { get; private set; }

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x000246AF File Offset: 0x000228AF
		public Sprite BoxSelectedSprite
		{
			get
			{
				return this.BoxHighlightedSprite;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000ADA RID: 2778 RVA: 0x000246BC File Offset: 0x000228BC
		// (set) Token: 0x06000ADB RID: 2779 RVA: 0x000246C4 File Offset: 0x000228C4
		public StaticResourceRef<Sprite> BoxPressedSprite { get; private set; }

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000ADC RID: 2780 RVA: 0x000246CD File Offset: 0x000228CD
		// (set) Token: 0x06000ADD RID: 2781 RVA: 0x000246D5 File Offset: 0x000228D5
		public StaticResourceRef<Sprite> SliderBackgroundSprite { get; private set; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000ADE RID: 2782 RVA: 0x000246DE File Offset: 0x000228DE
		// (set) Token: 0x06000ADF RID: 2783 RVA: 0x000246E6 File Offset: 0x000228E6
		public StaticResourceRef<Sprite> ToggleForegroundSprite { get; private set; }
	}
}
