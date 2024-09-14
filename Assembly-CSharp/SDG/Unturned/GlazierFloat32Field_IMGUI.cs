using System;

namespace SDG.Unturned
{
	// Token: 0x02000164 RID: 356
	internal class GlazierFloat32Field_IMGUI : GlazierNumericField_IMGUI, ISleekFloat32Field, ISleekElement, ISleekNumericField, ISleekWithTooltip
	{
		// Token: 0x14000032 RID: 50
		// (add) Token: 0x060008F9 RID: 2297 RVA: 0x0001F700 File Offset: 0x0001D900
		// (remove) Token: 0x060008FA RID: 2298 RVA: 0x0001F738 File Offset: 0x0001D938
		public event TypedSingle OnValueSubmitted;

		// Token: 0x14000033 RID: 51
		// (add) Token: 0x060008FB RID: 2299 RVA: 0x0001F770 File Offset: 0x0001D970
		// (remove) Token: 0x060008FC RID: 2300 RVA: 0x0001F7A8 File Offset: 0x0001D9A8
		public event TypedSingle OnValueChanged;

		// Token: 0x060008FD RID: 2301 RVA: 0x0001F7DD File Offset: 0x0001D9DD
		public GlazierFloat32Field_IMGUI()
		{
			this.Value = 0f;
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060008FE RID: 2302 RVA: 0x0001F7F0 File Offset: 0x0001D9F0
		// (set) Token: 0x060008FF RID: 2303 RVA: 0x0001F7F8 File Offset: 0x0001D9F8
		public float Value
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

		// Token: 0x06000900 RID: 2304 RVA: 0x0001F828 File Offset: 0x0001DA28
		protected override bool ParseNumericInput(string input)
		{
			if (input.Length > 0 && !char.IsDigit(input, input.Length - 1))
			{
				input += "0";
			}
			float num;
			if (float.TryParse(input, ref num))
			{
				if (this._state != num)
				{
					this._state = num;
					TypedSingle onValueChanged = this.OnValueChanged;
					if (onValueChanged != null)
					{
						onValueChanged.Invoke(this, this._state);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x0001F890 File Offset: 0x0001DA90
		protected override void OnReturnPressed()
		{
			TypedSingle onValueSubmitted = this.OnValueSubmitted;
			if (onValueSubmitted == null)
			{
				return;
			}
			onValueSubmitted.Invoke(this, this._state);
		}

		// Token: 0x0400036D RID: 877
		private float _state;
	}
}
