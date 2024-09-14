using System;
using UnityEngine.UIElements;

namespace SDG.Unturned
{
	// Token: 0x02000198 RID: 408
	internal class GlazierEmpty_UIToolkit : GlazierElementBase_UIToolkit
	{
		// Token: 0x06000C29 RID: 3113 RVA: 0x00029453 File Offset: 0x00027653
		public GlazierEmpty_UIToolkit(Glazier_UIToolkit glazier) : base(glazier)
		{
			this.visualElement = new VisualElement();
			this.visualElement.userData = this;
			this.visualElement.pickingMode = 1;
			this.visualElement.AddToClassList("unturned-empty");
		}
	}
}
