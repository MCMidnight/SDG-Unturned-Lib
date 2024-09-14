using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000161 RID: 353
	internal class GlazierButton_IMGUI : GlazierLabel_IMGUI, ISleekButton, ISleekElement, ISleekLabel, ISleekWithTooltip
	{
		// Token: 0x14000030 RID: 48
		// (add) Token: 0x060008D2 RID: 2258 RVA: 0x0001EE74 File Offset: 0x0001D074
		// (remove) Token: 0x060008D3 RID: 2259 RVA: 0x0001EEAC File Offset: 0x0001D0AC
		public event ClickedButton OnClicked;

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x060008D4 RID: 2260 RVA: 0x0001EEE4 File Offset: 0x0001D0E4
		// (remove) Token: 0x060008D5 RID: 2261 RVA: 0x0001EF1C File Offset: 0x0001D11C
		public event ClickedButton OnRightClicked;

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060008D6 RID: 2262 RVA: 0x0001EF51 File Offset: 0x0001D151
		// (set) Token: 0x060008D7 RID: 2263 RVA: 0x0001EF59 File Offset: 0x0001D159
		public bool IsClickable { get; set; } = true;

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060008D8 RID: 2264 RVA: 0x0001EF62 File Offset: 0x0001D162
		// (set) Token: 0x060008D9 RID: 2265 RVA: 0x0001EF6A File Offset: 0x0001D16A
		public bool IsRaycastTarget
		{
			get
			{
				return this._isRaycastTarget;
			}
			set
			{
				this._isRaycastTarget = value;
				this.calculateContent();
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060008DA RID: 2266 RVA: 0x0001EF79 File Offset: 0x0001D179
		// (set) Token: 0x060008DB RID: 2267 RVA: 0x0001EF81 File Offset: 0x0001D181
		public SleekColor BackgroundColor { get; set; }

		// Token: 0x060008DC RID: 2268 RVA: 0x0001EF8C File Offset: 0x0001D18C
		public override void OnGUI()
		{
			bool enabled = GUI.enabled;
			GUI.enabled = this.IsClickable;
			if (this.IsRaycastTarget)
			{
				if (GlazierUtils_IMGUI.drawButton(this.drawRect, this.BackgroundColor))
				{
					if (Event.current.button == 0)
					{
						ClickedButton onClicked = this.OnClicked;
						if (onClicked != null)
						{
							onClicked.Invoke(this);
						}
					}
					else if (Event.current.button == 1)
					{
						ClickedButton onRightClicked = this.OnRightClicked;
						if (onRightClicked != null)
						{
							onRightClicked.Invoke(this);
						}
					}
				}
			}
			else
			{
				GlazierUtils_IMGUI.drawBox(this.drawRect, this.BackgroundColor);
			}
			GUI.enabled = enabled;
			GlazierUtils_IMGUI.drawLabel(this.drawRect, base.FontStyle, base.TextAlignment, this.fontSizeInt, this.shadowContent, base.TextColor, this.content, base.TextContrastContext);
			base.ChildrenOnGUI();
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0001F065 File Offset: 0x0001D265
		protected override void calculateContent()
		{
			base.calculateContent();
			if (!this._isRaycastTarget)
			{
				this.content.tooltip = null;
			}
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0001F081 File Offset: 0x0001D281
		public GlazierButton_IMGUI()
		{
			this.BackgroundColor = GlazierConst.DefaultButtonBackgroundColor;
		}

		// Token: 0x04000364 RID: 868
		private bool _isRaycastTarget = true;
	}
}
