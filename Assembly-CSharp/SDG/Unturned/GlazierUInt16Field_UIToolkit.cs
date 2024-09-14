using System;

namespace SDG.Unturned
{
	// Token: 0x020001A6 RID: 422
	internal class GlazierUInt16Field_UIToolkit : GlazierNumericField_UIToolkit, ISleekUInt16Field, ISleekElement, ISleekNumericField, ISleekWithTooltip
	{
		// Token: 0x14000069 RID: 105
		// (add) Token: 0x06000D0A RID: 3338 RVA: 0x0002B988 File Offset: 0x00029B88
		// (remove) Token: 0x06000D0B RID: 3339 RVA: 0x0002B9C0 File Offset: 0x00029BC0
		public event TypedUInt16 OnValueChanged;

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000D0C RID: 3340 RVA: 0x0002B9F5 File Offset: 0x00029BF5
		// (set) Token: 0x06000D0D RID: 3341 RVA: 0x0002B9FD File Offset: 0x00029BFD
		public ushort Value
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

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000D0E RID: 3342 RVA: 0x0002BA0C File Offset: 0x00029C0C
		// (set) Token: 0x06000D0F RID: 3343 RVA: 0x0002BA14 File Offset: 0x00029C14
		public ushort MinValue { get; set; }

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000D10 RID: 3344 RVA: 0x0002BA1D File Offset: 0x00029C1D
		// (set) Token: 0x06000D11 RID: 3345 RVA: 0x0002BA25 File Offset: 0x00029C25
		public ushort MaxValue { get; set; } = ushort.MaxValue;

		// Token: 0x06000D12 RID: 3346 RVA: 0x0002BA2E File Offset: 0x00029C2E
		public GlazierUInt16Field_UIToolkit(Glazier_UIToolkit glazier) : base(glazier)
		{
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x0002BA44 File Offset: 0x00029C44
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
				flag = ushort.TryParse(input, ref this._state);
			}
			if (flag)
			{
				this._state = MathfEx.Clamp(this._state, this.MinValue, this.MaxValue);
				TypedUInt16 onValueChanged = this.OnValueChanged;
				if (onValueChanged != null)
				{
					onValueChanged.Invoke(this, this._state);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x0002BAAC File Offset: 0x00029CAC
		protected override string NumberToString()
		{
			return this.Value.ToString();
		}

		// Token: 0x040004F8 RID: 1272
		private ushort _state;
	}
}
