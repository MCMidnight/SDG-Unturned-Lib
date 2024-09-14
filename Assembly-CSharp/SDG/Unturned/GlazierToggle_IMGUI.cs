using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000170 RID: 368
	internal class GlazierToggle_IMGUI : GlazierElementBase_IMGUI, ISleekToggle, ISleekElement, ISleekWithTooltip
	{
		// Token: 0x1400003E RID: 62
		// (add) Token: 0x060009AC RID: 2476 RVA: 0x00020F5C File Offset: 0x0001F15C
		// (remove) Token: 0x060009AD RID: 2477 RVA: 0x00020F94 File Offset: 0x0001F194
		public event Toggled OnValueChanged;

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060009AE RID: 2478 RVA: 0x00020FC9 File Offset: 0x0001F1C9
		// (set) Token: 0x060009AF RID: 2479 RVA: 0x00020FD1 File Offset: 0x0001F1D1
		public bool Value { get; set; }

		/// <summary>
		/// Tooltip text.
		/// </summary>
		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060009B0 RID: 2480 RVA: 0x00020FDA File Offset: 0x0001F1DA
		// (set) Token: 0x060009B1 RID: 2481 RVA: 0x00020FE2 File Offset: 0x0001F1E2
		public string TooltipText
		{
			get
			{
				return this._tooltip;
			}
			set
			{
				this._tooltip = value;
				this.content = new GUIContent(string.Empty, this._tooltip);
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060009B2 RID: 2482 RVA: 0x00021001 File Offset: 0x0001F201
		// (set) Token: 0x060009B3 RID: 2483 RVA: 0x00021009 File Offset: 0x0001F209
		public SleekColor BackgroundColor { get; set; } = GlazierConst.DefaultToggleBackgroundColor;

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060009B4 RID: 2484 RVA: 0x00021012 File Offset: 0x0001F212
		// (set) Token: 0x060009B5 RID: 2485 RVA: 0x0002101A File Offset: 0x0001F21A
		public SleekColor ForegroundColor { get; set; } = GlazierConst.DefaultToggleForegroundColor;

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060009B6 RID: 2486 RVA: 0x00021023 File Offset: 0x0001F223
		// (set) Token: 0x060009B7 RID: 2487 RVA: 0x0002102B File Offset: 0x0001F22B
		public bool IsInteractable { get; set; } = true;

		// Token: 0x060009B8 RID: 2488 RVA: 0x00021034 File Offset: 0x0001F234
		public override void OnGUI()
		{
			bool enabled = GUI.enabled;
			GUI.enabled = this.IsInteractable;
			bool flag = GlazierUtils_IMGUI.drawToggle(this.drawRect, this.BackgroundColor, this.Value, this.content);
			GUI.enabled = enabled;
			if (flag != this.Value)
			{
				Toggled onValueChanged = this.OnValueChanged;
				if (onValueChanged != null)
				{
					onValueChanged.Invoke(this, flag);
				}
			}
			this.Value = flag;
			base.ChildrenOnGUI();
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x000210A4 File Offset: 0x0001F2A4
		public GlazierToggle_IMGUI()
		{
			base.SizeOffset_X = 40f;
			base.SizeOffset_Y = 40f;
		}

		// Token: 0x040003C3 RID: 963
		private string _tooltip;

		/// <summary>
		/// Holds tooltip text
		/// </summary>
		// Token: 0x040003C7 RID: 967
		protected GUIContent content = new GUIContent();
	}
}
