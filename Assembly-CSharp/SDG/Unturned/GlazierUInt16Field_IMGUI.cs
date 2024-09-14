using System;

namespace SDG.Unturned
{
	// Token: 0x02000171 RID: 369
	internal class GlazierUInt16Field_IMGUI : GlazierNumericField_IMGUI, ISleekUInt16Field, ISleekElement, ISleekNumericField, ISleekWithTooltip
	{
		// Token: 0x1400003F RID: 63
		// (add) Token: 0x060009BA RID: 2490 RVA: 0x000210F8 File Offset: 0x0001F2F8
		// (remove) Token: 0x060009BB RID: 2491 RVA: 0x00021130 File Offset: 0x0001F330
		public event TypedUInt16 OnValueChanged;

		// Token: 0x060009BC RID: 2492 RVA: 0x00021165 File Offset: 0x0001F365
		public GlazierUInt16Field_IMGUI()
		{
			this.Value = 0;
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060009BD RID: 2493 RVA: 0x0002117F File Offset: 0x0001F37F
		// (set) Token: 0x060009BE RID: 2494 RVA: 0x00021188 File Offset: 0x0001F388
		public ushort Value
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

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060009BF RID: 2495 RVA: 0x000211B0 File Offset: 0x0001F3B0
		// (set) Token: 0x060009C0 RID: 2496 RVA: 0x000211B8 File Offset: 0x0001F3B8
		public ushort MinValue { get; set; }

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060009C1 RID: 2497 RVA: 0x000211C1 File Offset: 0x0001F3C1
		// (set) Token: 0x060009C2 RID: 2498 RVA: 0x000211C9 File Offset: 0x0001F3C9
		public ushort MaxValue { get; set; } = ushort.MaxValue;

		// Token: 0x060009C3 RID: 2499 RVA: 0x000211D4 File Offset: 0x0001F3D4
		protected override bool ParseNumericInput(string input)
		{
			ushort num;
			if (ushort.TryParse(input, ref num))
			{
				num = MathfEx.Clamp(num, this.MinValue, this.MaxValue);
				if (this._state != num)
				{
					this._state = num;
					TypedUInt16 onValueChanged = this.OnValueChanged;
					if (onValueChanged != null)
					{
						onValueChanged.Invoke(this, this._state);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x040003C9 RID: 969
		private ushort _state;
	}
}
