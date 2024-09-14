using System;

namespace SDG.Unturned
{
	// Token: 0x0200017E RID: 382
	internal class GlazierFloat64Field_uGUI : GlazierNumericField_uGUI, ISleekFloat64Field, ISleekElement, ISleekNumericField, ISleekWithTooltip
	{
		// Token: 0x14000046 RID: 70
		// (add) Token: 0x06000A86 RID: 2694 RVA: 0x00023AA0 File Offset: 0x00021CA0
		// (remove) Token: 0x06000A87 RID: 2695 RVA: 0x00023AD8 File Offset: 0x00021CD8
		public event TypedDouble OnValueChanged;

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000A88 RID: 2696 RVA: 0x00023B0D File Offset: 0x00021D0D
		// (set) Token: 0x06000A89 RID: 2697 RVA: 0x00023B15 File Offset: 0x00021D15
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

		// Token: 0x06000A8A RID: 2698 RVA: 0x00023B24 File Offset: 0x00021D24
		public GlazierFloat64Field_uGUI(Glazier_uGUI glazier) : base(glazier)
		{
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x00023B2D File Offset: 0x00021D2D
		public override void ConstructNew()
		{
			base.ConstructNew();
			this.fieldComponent.contentType = 3;
			base.SynchronizeText();
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x00023B48 File Offset: 0x00021D48
		protected override bool ParseNumericInput(string input)
		{
			if (input.Length > 0 && !char.IsDigit(input, input.Length - 1))
			{
				input += "0";
			}
			if (double.TryParse(input, ref this._state))
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

		// Token: 0x06000A8D RID: 2701 RVA: 0x00023BA4 File Offset: 0x00021DA4
		protected override string NumberToString()
		{
			return this.Value.ToString("F3");
		}

		// Token: 0x04000403 RID: 1027
		private double _state;
	}
}
