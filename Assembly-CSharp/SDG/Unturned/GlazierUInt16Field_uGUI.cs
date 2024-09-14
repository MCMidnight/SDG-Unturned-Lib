using System;

namespace SDG.Unturned
{
	// Token: 0x0200018D RID: 397
	internal class GlazierUInt16Field_uGUI : GlazierNumericField_uGUI, ISleekUInt16Field, ISleekElement, ISleekNumericField, ISleekWithTooltip
	{
		// Token: 0x14000054 RID: 84
		// (add) Token: 0x06000B79 RID: 2937 RVA: 0x00026B08 File Offset: 0x00024D08
		// (remove) Token: 0x06000B7A RID: 2938 RVA: 0x00026B40 File Offset: 0x00024D40
		public event TypedUInt16 OnValueChanged;

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000B7B RID: 2939 RVA: 0x00026B75 File Offset: 0x00024D75
		// (set) Token: 0x06000B7C RID: 2940 RVA: 0x00026B7D File Offset: 0x00024D7D
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

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000B7D RID: 2941 RVA: 0x00026B8C File Offset: 0x00024D8C
		// (set) Token: 0x06000B7E RID: 2942 RVA: 0x00026B94 File Offset: 0x00024D94
		public ushort MinValue { get; set; }

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000B7F RID: 2943 RVA: 0x00026B9D File Offset: 0x00024D9D
		// (set) Token: 0x06000B80 RID: 2944 RVA: 0x00026BA5 File Offset: 0x00024DA5
		public ushort MaxValue { get; set; } = ushort.MaxValue;

		// Token: 0x06000B81 RID: 2945 RVA: 0x00026BAE File Offset: 0x00024DAE
		public GlazierUInt16Field_uGUI(Glazier_uGUI glazier) : base(glazier)
		{
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x00026BC2 File Offset: 0x00024DC2
		public override void ConstructNew()
		{
			base.ConstructNew();
			this.fieldComponent.contentType = 2;
			base.SynchronizeText();
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x00026BDC File Offset: 0x00024DDC
		protected override bool ParseNumericInput(string input)
		{
			if (ushort.TryParse(input, ref this._state))
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

		// Token: 0x06000B84 RID: 2948 RVA: 0x00026C30 File Offset: 0x00024E30
		protected override string NumberToString()
		{
			return this.Value.ToString();
		}

		// Token: 0x04000460 RID: 1120
		private ushort _state;
	}
}
