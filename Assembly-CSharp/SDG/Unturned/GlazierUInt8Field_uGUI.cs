using System;

namespace SDG.Unturned
{
	// Token: 0x0200018F RID: 399
	internal class GlazierUInt8Field_uGUI : GlazierNumericField_uGUI, ISleekUInt8Field, ISleekElement, ISleekNumericField, ISleekWithTooltip
	{
		// Token: 0x14000056 RID: 86
		// (add) Token: 0x06000B8D RID: 2957 RVA: 0x00026D3C File Offset: 0x00024F3C
		// (remove) Token: 0x06000B8E RID: 2958 RVA: 0x00026D74 File Offset: 0x00024F74
		public event TypedByte OnValueChanged;

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000B8F RID: 2959 RVA: 0x00026DA9 File Offset: 0x00024FA9
		// (set) Token: 0x06000B90 RID: 2960 RVA: 0x00026DB1 File Offset: 0x00024FB1
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

		// Token: 0x06000B91 RID: 2961 RVA: 0x00026DC0 File Offset: 0x00024FC0
		public GlazierUInt8Field_uGUI(Glazier_uGUI glazier) : base(glazier)
		{
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x00026DC9 File Offset: 0x00024FC9
		public override void ConstructNew()
		{
			base.ConstructNew();
			this.fieldComponent.contentType = 2;
			base.SynchronizeText();
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x00026DE3 File Offset: 0x00024FE3
		protected override bool ParseNumericInput(string input)
		{
			if (byte.TryParse(input, ref this._state))
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

		// Token: 0x06000B94 RID: 2964 RVA: 0x00026E10 File Offset: 0x00025010
		protected override string NumberToString()
		{
			return this.Value.ToString();
		}

		// Token: 0x04000466 RID: 1126
		private byte _state;
	}
}
