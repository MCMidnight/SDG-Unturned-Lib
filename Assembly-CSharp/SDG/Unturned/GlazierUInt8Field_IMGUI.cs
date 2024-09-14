using System;

namespace SDG.Unturned
{
	// Token: 0x02000173 RID: 371
	internal class GlazierUInt8Field_IMGUI : GlazierNumericField_IMGUI, ISleekUInt8Field, ISleekElement, ISleekNumericField, ISleekWithTooltip
	{
		// Token: 0x14000041 RID: 65
		// (add) Token: 0x060009CA RID: 2506 RVA: 0x0002131C File Offset: 0x0001F51C
		// (remove) Token: 0x060009CB RID: 2507 RVA: 0x00021354 File Offset: 0x0001F554
		public event TypedByte OnValueChanged;

		// Token: 0x060009CC RID: 2508 RVA: 0x00021389 File Offset: 0x0001F589
		public GlazierUInt8Field_IMGUI()
		{
			this.Value = 0;
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060009CD RID: 2509 RVA: 0x00021398 File Offset: 0x0001F598
		// (set) Token: 0x060009CE RID: 2510 RVA: 0x000213A0 File Offset: 0x0001F5A0
		public byte Value
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

		// Token: 0x060009CF RID: 2511 RVA: 0x000213C8 File Offset: 0x0001F5C8
		protected override bool ParseNumericInput(string input)
		{
			byte b;
			if (byte.TryParse(input, ref b))
			{
				if (this._state != b)
				{
					this._state = b;
					TypedByte onValueChanged = this.OnValueChanged;
					if (onValueChanged != null)
					{
						onValueChanged.Invoke(this, this._state);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x040003CF RID: 975
		private byte _state;
	}
}
