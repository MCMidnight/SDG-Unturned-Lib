using System;

namespace SDG.Unturned
{
	// Token: 0x0200017D RID: 381
	internal class GlazierFloat32Field_uGUI : GlazierNumericField_uGUI, ISleekFloat32Field, ISleekElement, ISleekNumericField, ISleekWithTooltip
	{
		// Token: 0x14000044 RID: 68
		// (add) Token: 0x06000A7B RID: 2683 RVA: 0x000238F4 File Offset: 0x00021AF4
		// (remove) Token: 0x06000A7C RID: 2684 RVA: 0x0002392C File Offset: 0x00021B2C
		public event TypedSingle OnValueSubmitted;

		// Token: 0x14000045 RID: 69
		// (add) Token: 0x06000A7D RID: 2685 RVA: 0x00023964 File Offset: 0x00021B64
		// (remove) Token: 0x06000A7E RID: 2686 RVA: 0x0002399C File Offset: 0x00021B9C
		public event TypedSingle OnValueChanged;

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000A7F RID: 2687 RVA: 0x000239D1 File Offset: 0x00021BD1
		// (set) Token: 0x06000A80 RID: 2688 RVA: 0x000239D9 File Offset: 0x00021BD9
		public float Value
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
				base.SynchronizeText();
			}
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x000239E8 File Offset: 0x00021BE8
		public GlazierFloat32Field_uGUI(Glazier_uGUI glazier) : base(glazier)
		{
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x000239F1 File Offset: 0x00021BF1
		public override void ConstructNew()
		{
			base.ConstructNew();
			this.fieldComponent.contentType = 3;
			base.SynchronizeText();
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x00023A0B File Offset: 0x00021C0B
		protected override void OnUnitySubmit(string input)
		{
			TypedSingle onValueSubmitted = this.OnValueSubmitted;
			if (onValueSubmitted == null)
			{
				return;
			}
			onValueSubmitted.Invoke(this, this._state);
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x00023A24 File Offset: 0x00021C24
		protected override bool ParseNumericInput(string input)
		{
			if (input.Length > 0 && !char.IsDigit(input, input.Length - 1))
			{
				input += "0";
			}
			if (float.TryParse(input, ref this._state))
			{
				TypedSingle onValueChanged = this.OnValueChanged;
				if (onValueChanged != null)
				{
					onValueChanged.Invoke(this, this._state);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x00023A80 File Offset: 0x00021C80
		protected override string NumberToString()
		{
			return this.Value.ToString("F3");
		}

		// Token: 0x04000401 RID: 1025
		private float _state;
	}
}
