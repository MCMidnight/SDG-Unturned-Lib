using System;

namespace SDG.Unturned
{
	// Token: 0x0200019A RID: 410
	internal class GlazierFloat64Field_UIToolkit : GlazierNumericField_UIToolkit, ISleekFloat64Field, ISleekElement, ISleekNumericField, ISleekWithTooltip
	{
		// Token: 0x1400005B RID: 91
		// (add) Token: 0x06000C34 RID: 3124 RVA: 0x0002964C File Offset: 0x0002784C
		// (remove) Token: 0x06000C35 RID: 3125 RVA: 0x00029684 File Offset: 0x00027884
		public event TypedDouble OnValueChanged;

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000C36 RID: 3126 RVA: 0x000296B9 File Offset: 0x000278B9
		// (set) Token: 0x06000C37 RID: 3127 RVA: 0x000296C1 File Offset: 0x000278C1
		public double Value
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

		// Token: 0x06000C38 RID: 3128 RVA: 0x000296D0 File Offset: 0x000278D0
		public GlazierFloat64Field_UIToolkit(Glazier_UIToolkit glazier) : base(glazier)
		{
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x000296DC File Offset: 0x000278DC
		protected override bool ParseNumericInput(string input)
		{
			bool flag;
			if (string.IsNullOrEmpty(input) || string.Equals(input, "-"))
			{
				this._state = 0.0;
				flag = true;
			}
			else
			{
				if (input.Length > 0 && !char.IsDigit(input, input.Length - 1))
				{
					input += "0";
				}
				flag = double.TryParse(input, ref this._state);
			}
			if (flag)
			{
				TypedDouble onValueChanged = this.OnValueChanged;
				if (onValueChanged != null)
				{
					onValueChanged.Invoke(this, this._state);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x00029764 File Offset: 0x00027964
		protected override string NumberToString()
		{
			return this.Value.ToString("F3");
		}

		// Token: 0x040004A4 RID: 1188
		private double _state;
	}
}
