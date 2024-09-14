using System;
using UnityEngine.UIElements;

namespace SDG.Unturned
{
	// Token: 0x0200019F RID: 415
	internal class GlazierProxy_UIToolkit : GlazierElementBase_UIToolkit
	{
		// Token: 0x06000C76 RID: 3190 RVA: 0x00029F0C File Offset: 0x0002810C
		public GlazierProxy_UIToolkit(Glazier_UIToolkit glazier, SleekWrapper owner) : base(glazier)
		{
			this.owner = owner;
			this.visualElement = new VisualElement();
			this.visualElement.userData = this;
			this.visualElement.AddToClassList("unturned-empty");
			this.visualElement.pickingMode = 1;
			this.visualElement.name = owner.GetType().Name;
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x00029F70 File Offset: 0x00028170
		public override void Update()
		{
			this.owner.OnUpdate();
			base.Update();
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x00029F83 File Offset: 0x00028183
		public override void InternalDestroy()
		{
			this.owner.OnDestroy();
			base.InternalDestroy();
		}

		// Token: 0x040004BA RID: 1210
		private SleekWrapper owner;
	}
}
