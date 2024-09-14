using System;

namespace SDG.Unturned
{
	// Token: 0x020001A8 RID: 424
	internal class GlazierUInt8Field_UIToolkit : GlazierNumericField_UIToolkit, ISleekUInt8Field, ISleekElement, ISleekNumericField, ISleekWithTooltip
	{
		// Token: 0x1400006B RID: 107
		// (add) Token: 0x06000D1C RID: 3356 RVA: 0x0002BBC0 File Offset: 0x00029DC0
		// (remove) Token: 0x06000D1D RID: 3357 RVA: 0x0002BBF8 File Offset: 0x00029DF8
		public event TypedByte OnValueChanged;

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000D1E RID: 3358 RVA: 0x0002BC2D File Offset: 0x00029E2D
		// (set) Token: 0x06000D1F RID: 3359 RVA: 0x0002BC35 File Offset: 0x00029E35
		public byte Value
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

		// Token: 0x06000D20 RID: 3360 RVA: 0x0002BC44 File Offset: 0x00029E44
		public GlazierUInt8Field_UIToolkit(Glazier_UIToolkit glazier) : base(glazier)
		{
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x0002BC50 File Offset: 0x00029E50
		protected override bool ParseNumericInput(string input)
		{
			bool flag;
			if (string.IsNullOrEmpty(input))
			{
				this._state = 0;
				flag = true;
			}
			else
			{
				flag = byte.TryParse(input, ref this._state);
			}
			if (flag)
			{
				TypedByte onValueChanged = this.OnValueChanged;
				if (onValueChanged != null)
				{
					onValueChanged.Invoke(this, this._state);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x0002BC9C File Offset: 0x00029E9C
		protected override string NumberToString()
		{
			return this.Value.ToString();
		}

		// Token: 0x040004FE RID: 1278
		private byte _state;
	}
}
