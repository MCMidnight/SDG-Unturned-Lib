using System;

namespace SDG.Unturned
{
	// Token: 0x02000160 RID: 352
	internal class GlazierBox_IMGUI : GlazierLabel_IMGUI, ISleekBox, ISleekLabel, ISleekElement, ISleekWithTooltip
	{
		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060008CE RID: 2254 RVA: 0x0001EDE8 File Offset: 0x0001CFE8
		// (set) Token: 0x060008CF RID: 2255 RVA: 0x0001EDF0 File Offset: 0x0001CFF0
		public SleekColor BackgroundColor { get; set; } = GlazierConst.DefaultBoxBackgroundColor;

		// Token: 0x060008D0 RID: 2256 RVA: 0x0001EDFC File Offset: 0x0001CFFC
		public override void OnGUI()
		{
			GlazierUtils_IMGUI.drawBox(this.drawRect, this.BackgroundColor);
			GlazierUtils_IMGUI.drawLabel(this.drawRect, base.FontStyle, base.TextAlignment, this.fontSizeInt, this.shadowContent, base.TextColor, this.content, base.TextContrastContext);
			base.ChildrenOnGUI();
		}
	}
}
