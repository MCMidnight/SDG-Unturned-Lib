using System;
using UnityEngine.UIElements;

namespace SDG.Unturned
{
	// Token: 0x0200019E RID: 414
	internal abstract class GlazierNumericField_UIToolkit : GlazierStringField_UIToolkit, ISleekNumericField, ISleekWithTooltip
	{
		// Token: 0x06000C71 RID: 3185 RVA: 0x00029EDE File Offset: 0x000280DE
		public GlazierNumericField_UIToolkit(Glazier_UIToolkit glazier) : base(glazier)
		{
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x00029EE7 File Offset: 0x000280E7
		protected void SynchronizeText()
		{
			base.Text = this.NumberToString();
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x00029EF5 File Offset: 0x000280F5
		protected override void OnControlValueChanged(ChangeEvent<string> changeEvent)
		{
			if (!this.ParseNumericInput(changeEvent.newValue))
			{
				this.SynchronizeText();
			}
		}

		// Token: 0x06000C74 RID: 3188
		protected abstract bool ParseNumericInput(string input);

		// Token: 0x06000C75 RID: 3189
		protected abstract string NumberToString();
	}
}
