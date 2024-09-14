using System;

namespace SDG.Unturned
{
	// Token: 0x0200017C RID: 380
	internal class GlazierEmpty_uGUI : GlazierElementBase_uGUI
	{
		// Token: 0x06000A79 RID: 2681 RVA: 0x000238A3 File Offset: 0x00021AA3
		public GlazierEmpty_uGUI(Glazier_uGUI glazier) : base(glazier)
		{
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x000238AC File Offset: 0x00021AAC
		protected override bool ReleaseIntoPool()
		{
			if (base.transform == null || base.gameObject == null)
			{
				return false;
			}
			GlazierElementBase_uGUI.PoolData poolData = new GlazierElementBase_uGUI.PoolData();
			base.PopulateBasePoolData(poolData);
			base.glazier.ReleaseEmptyToPool(poolData);
			return true;
		}
	}
}
