using System;

namespace SDG.Unturned
{
	// Token: 0x02000172 RID: 370
	internal class GlazierUInt32Field_IMGUI : GlazierNumericField_IMGUI, ISleekUInt32Field, ISleekElement, ISleekNumericField, ISleekWithTooltip
	{
		// Token: 0x14000040 RID: 64
		// (add) Token: 0x060009C4 RID: 2500 RVA: 0x0002122C File Offset: 0x0001F42C
		// (remove) Token: 0x060009C5 RID: 2501 RVA: 0x00021264 File Offset: 0x0001F464
		public event TypedUInt32 OnValueChanged;

		// Token: 0x060009C6 RID: 2502 RVA: 0x00021299 File Offset: 0x0001F499
		public GlazierUInt32Field_IMGUI()
		{
			this.Value = 0U;
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060009C7 RID: 2503 RVA: 0x000212A8 File Offset: 0x0001F4A8
		// (set) Token: 0x060009C8 RID: 2504 RVA: 0x000212B0 File Offset: 0x0001F4B0
		public uint Value
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

		// Token: 0x060009C9 RID: 2505 RVA: 0x000212D8 File Offset: 0x0001F4D8
		protected override bool ParseNumericInput(string input)
		{
			uint num;
			if (uint.TryParse(input, ref num))
			{
				if (this._state != num)
				{
					this._state = num;
					TypedUInt32 onValueChanged = this.OnValueChanged;
					if (onValueChanged != null)
					{
						onValueChanged.Invoke(this, this._state);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x040003CD RID: 973
		private uint _state;
	}
}
