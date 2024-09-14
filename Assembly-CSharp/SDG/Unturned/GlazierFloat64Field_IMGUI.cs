using System;

namespace SDG.Unturned
{
	// Token: 0x02000165 RID: 357
	internal class GlazierFloat64Field_IMGUI : GlazierNumericField_IMGUI, ISleekFloat64Field, ISleekElement, ISleekNumericField, ISleekWithTooltip
	{
		// Token: 0x14000034 RID: 52
		// (add) Token: 0x06000902 RID: 2306 RVA: 0x0001F8AC File Offset: 0x0001DAAC
		// (remove) Token: 0x06000903 RID: 2307 RVA: 0x0001F8E4 File Offset: 0x0001DAE4
		public event TypedDouble OnValueChanged;

		// Token: 0x06000904 RID: 2308 RVA: 0x0001F919 File Offset: 0x0001DB19
		public GlazierFloat64Field_IMGUI()
		{
			this.Value = 0.0;
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000905 RID: 2309 RVA: 0x0001F930 File Offset: 0x0001DB30
		// (set) Token: 0x06000906 RID: 2310 RVA: 0x0001F938 File Offset: 0x0001DB38
		public double Value
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
				this.text = this.Value.ToString("F3");
			}
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x0001F968 File Offset: 0x0001DB68
		protected override bool ParseNumericInput(string input)
		{
			if (input.Length > 0 && !char.IsDigit(input, input.Length - 1))
			{
				input += "0";
			}
			double num;
			if (double.TryParse(input, ref num))
			{
				if (this._state != num)
				{
					this._state = num;
					TypedDouble onValueChanged = this.OnValueChanged;
					if (onValueChanged != null)
					{
						onValueChanged.Invoke(this, this._state);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0400036F RID: 879
		private double _state;
	}
}
