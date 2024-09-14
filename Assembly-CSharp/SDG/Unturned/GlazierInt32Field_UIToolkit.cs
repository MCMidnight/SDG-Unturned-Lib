using System;

namespace SDG.Unturned
{
	// Token: 0x0200019C RID: 412
	internal class GlazierInt32Field_UIToolkit : GlazierNumericField_UIToolkit, ISleekInt32Field, ISleekElement, ISleekNumericField, ISleekWithTooltip
	{
		// Token: 0x14000060 RID: 96
		// (add) Token: 0x06000C56 RID: 3158 RVA: 0x00029B74 File Offset: 0x00027D74
		// (remove) Token: 0x06000C57 RID: 3159 RVA: 0x00029BAC File Offset: 0x00027DAC
		public event TypedInt32 OnValueChanged;

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000C58 RID: 3160 RVA: 0x00029BE1 File Offset: 0x00027DE1
		// (set) Token: 0x06000C59 RID: 3161 RVA: 0x00029BE9 File Offset: 0x00027DE9
		public int Value
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

		// Token: 0x06000C5A RID: 3162 RVA: 0x00029BF8 File Offset: 0x00027DF8
		public GlazierInt32Field_UIToolkit(Glazier_UIToolkit glazier) : base(glazier)
		{
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x00029C04 File Offset: 0x00027E04
		protected override bool ParseNumericInput(string input)
		{
			bool flag;
			if (string.IsNullOrEmpty(input) || string.Equals(input, "-"))
			{
				this._state = 0;
				flag = true;
			}
			else
			{
				flag = int.TryParse(input, ref this._state);
			}
			if (flag)
			{
				TypedInt32 onValueChanged = this.OnValueChanged;
				if (onValueChanged != null)
				{
					onValueChanged.Invoke(this, this._state);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x00029C5C File Offset: 0x00027E5C
		protected override string NumberToString()
		{
			return this.Value.ToString();
		}

		// Token: 0x040004B0 RID: 1200
		private int _state;
	}
}
