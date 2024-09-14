using System;

namespace SDG.Unturned
{
	// Token: 0x02000183 RID: 387
	internal class GlazierProxy_uGUI : GlazierElementBase_uGUI
	{
		// Token: 0x06000ACF RID: 2767 RVA: 0x00024576 File Offset: 0x00022776
		public GlazierProxy_uGUI(Glazier_uGUI glazier) : base(glazier)
		{
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x0002457F File Offset: 0x0002277F
		public void InitOwner(SleekWrapper owner)
		{
			this.owner = owner;
			base.gameObject.name = owner.GetType().Name;
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x0002459E File Offset: 0x0002279E
		public override void Update()
		{
			this.owner.OnUpdate();
			base.Update();
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x000245B1 File Offset: 0x000227B1
		public override void InternalDestroy()
		{
			this.owner.OnDestroy();
			base.InternalDestroy();
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x000245C4 File Offset: 0x000227C4
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

		// Token: 0x04000416 RID: 1046
		private SleekWrapper owner;
	}
}
