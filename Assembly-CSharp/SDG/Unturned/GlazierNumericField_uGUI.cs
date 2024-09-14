using System;

namespace SDG.Unturned
{
	// Token: 0x02000182 RID: 386
	internal abstract class GlazierNumericField_uGUI : GlazierStringField_uGUI, ISleekNumericField, ISleekWithTooltip
	{
		// Token: 0x06000ACA RID: 2762 RVA: 0x0002454E File Offset: 0x0002274E
		public GlazierNumericField_uGUI(Glazier_uGUI glazier) : base(glazier)
		{
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x00024557 File Offset: 0x00022757
		protected void SynchronizeText()
		{
			base.Text = this.NumberToString();
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x00024565 File Offset: 0x00022765
		protected override void OnUnityValueChanged(string input)
		{
			if (!this.ParseNumericInput(input))
			{
				this.SynchronizeText();
			}
		}

		// Token: 0x06000ACD RID: 2765
		protected abstract bool ParseNumericInput(string input);

		// Token: 0x06000ACE RID: 2766
		protected abstract string NumberToString();
	}
}
