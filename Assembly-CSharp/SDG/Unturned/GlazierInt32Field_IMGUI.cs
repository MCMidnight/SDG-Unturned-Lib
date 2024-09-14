using System;

namespace SDG.Unturned
{
	// Token: 0x02000167 RID: 359
	internal class GlazierInt32Field_IMGUI : GlazierNumericField_IMGUI, ISleekInt32Field, ISleekElement, ISleekNumericField, ISleekWithTooltip
	{
		// Token: 0x14000037 RID: 55
		// (add) Token: 0x0600091B RID: 2331 RVA: 0x0001FC88 File Offset: 0x0001DE88
		// (remove) Token: 0x0600091C RID: 2332 RVA: 0x0001FCC0 File Offset: 0x0001DEC0
		public event TypedInt32 OnValueChanged;

		// Token: 0x0600091D RID: 2333 RVA: 0x0001FCF5 File Offset: 0x0001DEF5
		public GlazierInt32Field_IMGUI()
		{
			this.Value = 0;
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x0600091E RID: 2334 RVA: 0x0001FD04 File Offset: 0x0001DF04
		// (set) Token: 0x0600091F RID: 2335 RVA: 0x0001FD0C File Offset: 0x0001DF0C
		public int Value
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
				this.text = this.Value.ToString();
			}
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0001FD34 File Offset: 0x0001DF34
		protected override bool ParseNumericInput(string input)
		{
			int num;
			if (int.TryParse(input, ref num))
			{
				if (this._state != num)
				{
					this._state = num;
					TypedInt32 onValueChanged = this.OnValueChanged;
					if (onValueChanged != null)
					{
						onValueChanged.Invoke(this, this._state);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x04000378 RID: 888
		private int _state;
	}
}
