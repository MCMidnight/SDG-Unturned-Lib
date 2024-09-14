using System;

namespace SDG.Unturned
{
	// Token: 0x02000199 RID: 409
	internal class GlazierFloat32Field_UIToolkit : GlazierNumericField_UIToolkit, ISleekFloat32Field, ISleekElement, ISleekNumericField, ISleekWithTooltip
	{
		// Token: 0x14000059 RID: 89
		// (add) Token: 0x06000C2A RID: 3114 RVA: 0x00029490 File Offset: 0x00027690
		// (remove) Token: 0x06000C2B RID: 3115 RVA: 0x000294C8 File Offset: 0x000276C8
		public event TypedSingle OnValueSubmitted;

		// Token: 0x1400005A RID: 90
		// (add) Token: 0x06000C2C RID: 3116 RVA: 0x00029500 File Offset: 0x00027700
		// (remove) Token: 0x06000C2D RID: 3117 RVA: 0x00029538 File Offset: 0x00027738
		public event TypedSingle OnValueChanged;

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000C2E RID: 3118 RVA: 0x0002956D File Offset: 0x0002776D
		// (set) Token: 0x06000C2F RID: 3119 RVA: 0x00029575 File Offset: 0x00027775
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

		// Token: 0x06000C30 RID: 3120 RVA: 0x00029584 File Offset: 0x00027784
		public GlazierFloat32Field_UIToolkit(Glazier_UIToolkit glazier) : base(glazier)
		{
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x0002958D File Offset: 0x0002778D
		protected override void OnSubmitted()
		{
			TypedSingle onValueSubmitted = this.OnValueSubmitted;
			if (onValueSubmitted == null)
			{
				return;
			}
			onValueSubmitted.Invoke(this, this.Value);
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x000295A8 File Offset: 0x000277A8
		protected override bool ParseNumericInput(string input)
		{
			bool flag;
			if (string.IsNullOrEmpty(input) || string.Equals(input, "-"))
			{
				this._state = 0f;
				flag = true;
			}
			else
			{
				if (input.Length > 0 && !char.IsDigit(input, input.Length - 1))
				{
					input += "0";
				}
				flag = float.TryParse(input, ref this._state);
			}
			if (flag)
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

		// Token: 0x06000C33 RID: 3123 RVA: 0x0002962C File Offset: 0x0002782C
		protected override string NumberToString()
		{
			return this.Value.ToString("F3");
		}

		// Token: 0x040004A2 RID: 1186
		private float _state;
	}
}
