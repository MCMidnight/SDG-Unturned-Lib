using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000169 RID: 361
	internal abstract class GlazierNumericField_IMGUI : GlazierElementBase_IMGUI, ISleekNumericField, ISleekWithTooltip
	{
		// Token: 0x06000934 RID: 2356 RVA: 0x0001FEE4 File Offset: 0x0001E0E4
		public GlazierNumericField_IMGUI()
		{
			this.controlName = GlazierUtils_IMGUI.CreateUniqueControlName();
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000935 RID: 2357 RVA: 0x0001FF3D File Offset: 0x0001E13D
		// (set) Token: 0x06000936 RID: 2358 RVA: 0x0001FF45 File Offset: 0x0001E145
		public string TooltipText { get; set; } = string.Empty;

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000937 RID: 2359 RVA: 0x0001FF4E File Offset: 0x0001E14E
		// (set) Token: 0x06000938 RID: 2360 RVA: 0x0001FF56 File Offset: 0x0001E156
		public SleekColor TextColor { get; set; } = GlazierConst.DefaultFieldForegroundColor;

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000939 RID: 2361 RVA: 0x0001FF5F File Offset: 0x0001E15F
		// (set) Token: 0x0600093A RID: 2362 RVA: 0x0001FF67 File Offset: 0x0001E167
		public SleekColor BackgroundColor { get; set; } = GlazierConst.DefaultFieldBackgroundColor;

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x0600093B RID: 2363 RVA: 0x0001FF70 File Offset: 0x0001E170
		// (set) Token: 0x0600093C RID: 2364 RVA: 0x0001FF78 File Offset: 0x0001E178
		public bool IsClickable { get; set; } = true;

		// Token: 0x0600093D RID: 2365 RVA: 0x0001FF84 File Offset: 0x0001E184
		public override void OnGUI()
		{
			bool enabled = GUI.enabled;
			GUI.enabled = this.IsClickable;
			GUI.SetNextControlName(this.controlName);
			string input = GlazierUtils_IMGUI.drawField(this.drawRect, this.fontStyle, this.fontAlignment, this.fontSizeInt, this.BackgroundColor, this.TextColor, this.text, 64, false, 0);
			GUI.enabled = enabled;
			if (GUI.changed && this.ParseNumericInput(input))
			{
				this.text = input;
			}
			if (GUI.GetNameOfFocusedControl() == this.controlName && Event.current.isKey && Event.current.type == 5)
			{
				if (Event.current.keyCode == KeyCode.Escape || Event.current.keyCode == ControlsSettings.dashboard)
				{
					GUI.FocusControl(string.Empty);
				}
				else if (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter)
				{
					this.OnReturnPressed();
					GUI.FocusControl(string.Empty);
				}
			}
			base.ChildrenOnGUI();
		}

		// Token: 0x0600093E RID: 2366
		protected abstract bool ParseNumericInput(string input);

		// Token: 0x0600093F RID: 2367 RVA: 0x00020091 File Offset: 0x0001E291
		protected virtual void OnReturnPressed()
		{
		}

		// Token: 0x04000388 RID: 904
		protected string text;

		// Token: 0x04000389 RID: 905
		public FontStyle fontStyle;

		// Token: 0x0400038A RID: 906
		public TextAnchor fontAlignment = 4;

		// Token: 0x0400038B RID: 907
		public int fontSizeInt = GlazierUtils_IMGUI.GetFontSize(2);

		// Token: 0x0400038C RID: 908
		private string controlName;
	}
}
