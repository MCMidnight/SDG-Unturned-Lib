using System;

namespace SDG.Unturned
{
	// Token: 0x0200018E RID: 398
	internal class GlazierUInt32Field_uGUI : GlazierNumericField_uGUI, ISleekUInt32Field, ISleekElement, ISleekNumericField, ISleekWithTooltip
	{
		// Token: 0x14000055 RID: 85
		// (add) Token: 0x06000B85 RID: 2949 RVA: 0x00026C4C File Offset: 0x00024E4C
		// (remove) Token: 0x06000B86 RID: 2950 RVA: 0x00026C84 File Offset: 0x00024E84
		public event TypedUInt32 OnValueChanged;

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000B87 RID: 2951 RVA: 0x00026CB9 File Offset: 0x00024EB9
		// (set) Token: 0x06000B88 RID: 2952 RVA: 0x00026CC1 File Offset: 0x00024EC1
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

		// Token: 0x06000B89 RID: 2953 RVA: 0x00026CD0 File Offset: 0x00024ED0
		public GlazierUInt32Field_uGUI(Glazier_uGUI glazier) : base(glazier)
		{
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x00026CD9 File Offset: 0x00024ED9
		public override void ConstructNew()
		{
			base.ConstructNew();
			this.fieldComponent.contentType = 2;
			base.SynchronizeText();
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x00026CF3 File Offset: 0x00024EF3
		protected override bool ParseNumericInput(string input)
		{
			if (uint.TryParse(input, ref this._state))
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

		// Token: 0x06000B8C RID: 2956 RVA: 0x00026D20 File Offset: 0x00024F20
		protected override string NumberToString()
		{
			return this.Value.ToString();
		}

		// Token: 0x04000464 RID: 1124
		private uint _state;
	}
}
