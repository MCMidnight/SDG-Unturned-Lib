using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SDG.Unturned
{
	// Token: 0x020001A5 RID: 421
	internal class GlazierToggle_UIToolkit : GlazierElementBase_UIToolkit, ISleekToggle, ISleekElement, ISleekWithTooltip
	{
		// Token: 0x14000068 RID: 104
		// (add) Token: 0x06000CFA RID: 3322 RVA: 0x0002B754 File Offset: 0x00029954
		// (remove) Token: 0x06000CFB RID: 3323 RVA: 0x0002B78C File Offset: 0x0002998C
		public event Toggled OnValueChanged;

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000CFC RID: 3324 RVA: 0x0002B7C1 File Offset: 0x000299C1
		// (set) Token: 0x06000CFD RID: 3325 RVA: 0x0002B7CE File Offset: 0x000299CE
		public bool Value
		{
			get
			{
				return this.control.value;
			}
			set
			{
				this.control.SetValueWithoutNotify(value);
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000CFE RID: 3326 RVA: 0x0002B7DC File Offset: 0x000299DC
		// (set) Token: 0x06000CFF RID: 3327 RVA: 0x0002B7E4 File Offset: 0x000299E4
		public string TooltipText { get; set; }

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x0002B7ED File Offset: 0x000299ED
		// (set) Token: 0x06000D01 RID: 3329 RVA: 0x0002B7F5 File Offset: 0x000299F5
		public SleekColor BackgroundColor
		{
			get
			{
				return this._backgroundColor;
			}
			set
			{
				this._backgroundColor = value;
				this.backgroundElement.style.unityBackgroundImageTintColor = this._backgroundColor;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x0002B819 File Offset: 0x00029A19
		// (set) Token: 0x06000D03 RID: 3331 RVA: 0x0002B821 File Offset: 0x00029A21
		public SleekColor ForegroundColor
		{
			get
			{
				return this._foregroundColor;
			}
			set
			{
				this._foregroundColor = value;
				this.checkmarkElement.style.unityBackgroundImageTintColor = this._foregroundColor;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000D04 RID: 3332 RVA: 0x0002B845 File Offset: 0x00029A45
		// (set) Token: 0x06000D05 RID: 3333 RVA: 0x0002B852 File Offset: 0x00029A52
		public bool IsInteractable
		{
			get
			{
				return this.control.enabledSelf;
			}
			set
			{
				this.control.SetEnabled(value);
			}
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x0002B860 File Offset: 0x00029A60
		public GlazierToggle_UIToolkit(Glazier_UIToolkit glazier) : base(glazier)
		{
			base.SizeOffset_X = 40f;
			base.SizeOffset_Y = 40f;
			this.control = new Toggle();
			this.control.userData = this;
			this.control.AddToClassList("unturned-toggle");
			INotifyValueChangedExtensions.RegisterValueChangedCallback<bool>(this.control, new EventCallback<ChangeEvent<bool>>(this.OnControlValueChanged));
			this.backgroundElement = UQueryExtensions.Q(this.control, null, "unity-toggle__input");
			this.checkmarkElement = UQueryExtensions.Q(this.control, "unity-checkmark", null);
			this.visualElement = this.control;
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x0002B919 File Offset: 0x00029B19
		internal override void SynchronizeColors()
		{
			this.backgroundElement.style.unityBackgroundImageTintColor = this._backgroundColor;
			this.checkmarkElement.style.unityBackgroundImageTintColor = this._foregroundColor;
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x0002B951 File Offset: 0x00029B51
		internal override bool GetTooltipParameters(out string tooltipText, out Color tooltipColor)
		{
			tooltipText = this.TooltipText;
			tooltipColor = new SleekColor(3);
			return true;
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x0002B96D File Offset: 0x00029B6D
		private void OnControlValueChanged(ChangeEvent<bool> changeEvent)
		{
			Toggled onValueChanged = this.OnValueChanged;
			if (onValueChanged == null)
			{
				return;
			}
			onValueChanged.Invoke(this, changeEvent.newValue);
		}

		// Token: 0x040004F2 RID: 1266
		private SleekColor _backgroundColor = GlazierConst.DefaultToggleBackgroundColor;

		// Token: 0x040004F3 RID: 1267
		private SleekColor _foregroundColor = GlazierConst.DefaultToggleForegroundColor;

		// Token: 0x040004F4 RID: 1268
		private Toggle control;

		// Token: 0x040004F5 RID: 1269
		private VisualElement backgroundElement;

		// Token: 0x040004F6 RID: 1270
		private VisualElement checkmarkElement;
	}
}
