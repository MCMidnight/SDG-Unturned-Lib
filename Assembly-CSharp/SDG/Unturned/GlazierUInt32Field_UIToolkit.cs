using System;

namespace SDG.Unturned
{
	// Token: 0x020001A7 RID: 423
	internal class GlazierUInt32Field_UIToolkit : GlazierNumericField_UIToolkit, ISleekUInt32Field, ISleekElement, ISleekNumericField, ISleekWithTooltip
	{
		// Token: 0x1400006A RID: 106
		// (add) Token: 0x06000D15 RID: 3349 RVA: 0x0002BAC8 File Offset: 0x00029CC8
		// (remove) Token: 0x06000D16 RID: 3350 RVA: 0x0002BB00 File Offset: 0x00029D00
		public event TypedUInt32 OnValueChanged;

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000D17 RID: 3351 RVA: 0x0002BB35 File Offset: 0x00029D35
		// (set) Token: 0x06000D18 RID: 3352 RVA: 0x0002BB3D File Offset: 0x00029D3D
		public uint Value
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

		// Token: 0x06000D19 RID: 3353 RVA: 0x0002BB4C File Offset: 0x00029D4C
		public GlazierUInt32Field_UIToolkit(Glazier_UIToolkit glazier) : base(glazier)
		{
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x0002BB58 File Offset: 0x00029D58
		protected override bool ParseNumericInput(string input)
		{
			bool flag;
			if (string.IsNullOrEmpty(input))
			{
				this._state = 0U;
				flag = true;
			}
			else
			{
				flag = uint.TryParse(input, ref this._state);
			}
			if (flag)
			{
				TypedUInt32 onValueChanged = this.OnValueChanged;
				if (onValueChanged != null)
				{
					onValueChanged.Invoke(this, this._state);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x0002BBA4 File Offset: 0x00029DA4
		protected override string NumberToString()
		{
			return this.Value.ToString();
		}

		// Token: 0x040004FC RID: 1276
		private uint _state;
	}
}
