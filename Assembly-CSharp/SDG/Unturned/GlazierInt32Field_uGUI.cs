using System;

namespace SDG.Unturned
{
	// Token: 0x02000180 RID: 384
	internal class GlazierInt32Field_uGUI : GlazierNumericField_uGUI, ISleekInt32Field, ISleekElement, ISleekNumericField, ISleekWithTooltip
	{
		// Token: 0x1400004B RID: 75
		// (add) Token: 0x06000AAD RID: 2733 RVA: 0x000241EC File Offset: 0x000223EC
		// (remove) Token: 0x06000AAE RID: 2734 RVA: 0x00024224 File Offset: 0x00022424
		public event TypedInt32 OnValueChanged;

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000AAF RID: 2735 RVA: 0x00024259 File Offset: 0x00022459
		// (set) Token: 0x06000AB0 RID: 2736 RVA: 0x00024261 File Offset: 0x00022461
		public int Value
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

		// Token: 0x06000AB1 RID: 2737 RVA: 0x00024270 File Offset: 0x00022470
		public GlazierInt32Field_uGUI(Glazier_uGUI glazier) : base(glazier)
		{
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x00024279 File Offset: 0x00022479
		public override void ConstructNew()
		{
			base.ConstructNew();
			this.fieldComponent.contentType = 2;
			base.SynchronizeText();
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x00024293 File Offset: 0x00022493
		protected override bool ParseNumericInput(string input)
		{
			if (int.TryParse(input, ref this._state))
			{
				TypedInt32 onValueChanged = this.OnValueChanged;
				if (onValueChanged != null)
				{
					onValueChanged.Invoke(this, this._state);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x000242C0 File Offset: 0x000224C0
		protected override string NumberToString()
		{
			return this.Value.ToString();
		}

		// Token: 0x0400040F RID: 1039
		private int _state;
	}
}
