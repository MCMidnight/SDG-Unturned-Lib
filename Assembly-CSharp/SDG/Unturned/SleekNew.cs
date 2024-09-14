using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000728 RID: 1832
	public class SleekNew : SleekWrapper
	{
		// Token: 0x06003C68 RID: 15464 RVA: 0x0011C7C4 File Offset: 0x0011A9C4
		public SleekNew(bool isUpdate = false)
		{
			base.PositionOffset_X = -105f;
			base.PositionScale_X = 1f;
			base.SizeOffset_X = 100f;
			base.SizeOffset_Y = 30f;
			this.label = Glazier.Get().CreateLabel();
			this.label.SizeScale_X = 1f;
			this.label.SizeScale_Y = 1f;
			this.label.TextAlignment = 5;
			this.label.Text = Provider.localization.format(isUpdate ? "Updated" : "New");
			this.label.TextColor = Color.green;
			this.label.TextContrastContext = 1;
			base.AddChild(this.label);
		}

		// Token: 0x040025BE RID: 9662
		internal ISleekLabel label;
	}
}
