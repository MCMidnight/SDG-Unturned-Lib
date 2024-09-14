using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200016F RID: 367
	internal class GlazierStringField_IMGUI : GlazierElementBase_IMGUI, ISleekField, ISleekElement, ISleekLabel, ISleekWithTooltip
	{
		// Token: 0x1400003B RID: 59
		// (add) Token: 0x06000986 RID: 2438 RVA: 0x00020ACC File Offset: 0x0001ECCC
		// (remove) Token: 0x06000987 RID: 2439 RVA: 0x00020B04 File Offset: 0x0001ED04
		public event Entered OnTextSubmitted;

		// Token: 0x1400003C RID: 60
		// (add) Token: 0x06000988 RID: 2440 RVA: 0x00020B3C File Offset: 0x0001ED3C
		// (remove) Token: 0x06000989 RID: 2441 RVA: 0x00020B74 File Offset: 0x0001ED74
		public event Typed OnTextChanged;

		// Token: 0x1400003D RID: 61
		// (add) Token: 0x0600098A RID: 2442 RVA: 0x00020BAC File Offset: 0x0001EDAC
		// (remove) Token: 0x0600098B RID: 2443 RVA: 0x00020BE4 File Offset: 0x0001EDE4
		public event Escaped OnTextEscaped;

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x0600098C RID: 2444 RVA: 0x00020C19 File Offset: 0x0001EE19
		// (set) Token: 0x0600098D RID: 2445 RVA: 0x00020C21 File Offset: 0x0001EE21
		public bool IsPasswordField { get; set; }

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x0600098E RID: 2446 RVA: 0x00020C2A File Offset: 0x0001EE2A
		// (set) Token: 0x0600098F RID: 2447 RVA: 0x00020C32 File Offset: 0x0001EE32
		public string PlaceholderText { get; set; } = string.Empty;

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000990 RID: 2448 RVA: 0x00020C3B File Offset: 0x0001EE3B
		// (set) Token: 0x06000991 RID: 2449 RVA: 0x00020C43 File Offset: 0x0001EE43
		public bool IsMultiline { get; set; }

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000992 RID: 2450 RVA: 0x00020C4C File Offset: 0x0001EE4C
		// (set) Token: 0x06000993 RID: 2451 RVA: 0x00020C54 File Offset: 0x0001EE54
		public string Text { get; set; } = string.Empty;

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000994 RID: 2452 RVA: 0x00020C5D File Offset: 0x0001EE5D
		// (set) Token: 0x06000995 RID: 2453 RVA: 0x00020C65 File Offset: 0x0001EE65
		public string TooltipText { get; set; } = string.Empty;

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000996 RID: 2454 RVA: 0x00020C6E File Offset: 0x0001EE6E
		// (set) Token: 0x06000997 RID: 2455 RVA: 0x00020C76 File Offset: 0x0001EE76
		public FontStyle FontStyle { get; set; }

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000998 RID: 2456 RVA: 0x00020C7F File Offset: 0x0001EE7F
		// (set) Token: 0x06000999 RID: 2457 RVA: 0x00020C87 File Offset: 0x0001EE87
		public TextAnchor TextAlignment { get; set; } = 4;

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600099A RID: 2458 RVA: 0x00020C90 File Offset: 0x0001EE90
		// (set) Token: 0x0600099B RID: 2459 RVA: 0x00020C98 File Offset: 0x0001EE98
		public ESleekFontSize FontSize
		{
			get
			{
				return this.fontSizeEnum;
			}
			set
			{
				this.fontSizeEnum = value;
				this.fontSizeInt = GlazierUtils_IMGUI.GetFontSize(this.fontSizeEnum);
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x0600099C RID: 2460 RVA: 0x00020CB2 File Offset: 0x0001EEB2
		// (set) Token: 0x0600099D RID: 2461 RVA: 0x00020CBA File Offset: 0x0001EEBA
		public ETextContrastContext TextContrastContext { get; set; }

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x0600099E RID: 2462 RVA: 0x00020CC3 File Offset: 0x0001EEC3
		// (set) Token: 0x0600099F RID: 2463 RVA: 0x00020CCB File Offset: 0x0001EECB
		public SleekColor TextColor { get; set; } = GlazierConst.DefaultFieldForegroundColor;

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060009A0 RID: 2464 RVA: 0x00020CD4 File Offset: 0x0001EED4
		// (set) Token: 0x060009A1 RID: 2465 RVA: 0x00020CD7 File Offset: 0x0001EED7
		public bool AllowRichText
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060009A2 RID: 2466 RVA: 0x00020CD9 File Offset: 0x0001EED9
		// (set) Token: 0x060009A3 RID: 2467 RVA: 0x00020CE1 File Offset: 0x0001EEE1
		public SleekColor BackgroundColor { get; set; } = GlazierConst.DefaultFieldBackgroundColor;

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060009A4 RID: 2468 RVA: 0x00020CEA File Offset: 0x0001EEEA
		// (set) Token: 0x060009A5 RID: 2469 RVA: 0x00020CF2 File Offset: 0x0001EEF2
		public int MaxLength { get; set; } = 100;

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x00020CFB File Offset: 0x0001EEFB
		// (set) Token: 0x060009A7 RID: 2471 RVA: 0x00020D03 File Offset: 0x0001EF03
		public bool IsClickable { get; set; } = true;

		// Token: 0x060009A8 RID: 2472 RVA: 0x00020D0C File Offset: 0x0001EF0C
		public void FocusControl()
		{
			GUI.FocusControl(this.controlName);
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x00020D19 File Offset: 0x0001EF19
		public void ClearFocus()
		{
			if (GUI.GetNameOfFocusedControl() == this.controlName)
			{
				GUI.FocusControl(string.Empty);
			}
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x00020D38 File Offset: 0x0001EF38
		public override void OnGUI()
		{
			bool enabled = GUI.enabled;
			GUI.enabled = this.IsClickable;
			GUI.SetNextControlName(this.controlName);
			if (this.IsPasswordField)
			{
				this.Text = GlazierUtils_IMGUI.DrawPasswordField(this.drawRect, this.FontStyle, this.TextAlignment, this.fontSizeInt, this.BackgroundColor, this.TextColor, this.Text, this.MaxLength, this.PlaceholderText, '*', this.TextContrastContext);
			}
			else
			{
				this.Text = GlazierUtils_IMGUI.DrawTextInputField(this.drawRect, this.FontStyle, this.TextAlignment, this.fontSizeInt, this.BackgroundColor, this.TextColor, this.Text, this.MaxLength, this.PlaceholderText, this.IsMultiline, this.TextContrastContext);
			}
			GUI.enabled = enabled;
			if (GUI.changed)
			{
				Typed onTextChanged = this.OnTextChanged;
				if (onTextChanged != null)
				{
					onTextChanged.Invoke(this, this.Text);
				}
			}
			if (GUI.GetNameOfFocusedControl() == this.controlName && Event.current.isKey && Event.current.type == 5)
			{
				if (Event.current.keyCode == KeyCode.Escape)
				{
					GUI.FocusControl(string.Empty);
					Escaped onTextEscaped = this.OnTextEscaped;
					if (onTextEscaped != null)
					{
						onTextEscaped.Invoke(this);
					}
				}
				else if ((Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter) && !this.IsMultiline)
				{
					Entered onTextSubmitted = this.OnTextSubmitted;
					if (onTextSubmitted != null)
					{
						onTextSubmitted.Invoke(this);
					}
					GUI.FocusControl(string.Empty);
				}
			}
			base.ChildrenOnGUI();
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x00020EDC File Offset: 0x0001F0DC
		public GlazierStringField_IMGUI()
		{
			this.BackgroundColor = GlazierConst.DefaultFieldBackgroundColor;
			this.controlName = GlazierUtils_IMGUI.CreateUniqueControlName();
			this.FontSize = 2;
		}

		// Token: 0x040003B9 RID: 953
		private int fontSizeInt;

		// Token: 0x040003BA RID: 954
		private ESleekFontSize fontSizeEnum;

		// Token: 0x040003C0 RID: 960
		private string controlName;
	}
}
