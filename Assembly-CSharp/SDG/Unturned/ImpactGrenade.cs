using System;

namespace SDG.Unturned
{
	// Token: 0x02000743 RID: 1859
	public class ImpactGrenade : TriggerGrenadeBase
	{
		// Token: 0x06003CEF RID: 15599 RVA: 0x001222C6 File Offset: 0x001204C6
		protected override void GrenadeTriggered()
		{
			base.GrenadeTriggered();
			if (this.explodable == null)
			{
				UnturnedLog.warn("Missing explodable", new object[]
				{
					this
				});
				return;
			}
			this.explodable.Explode();
		}

		// Token: 0x04002640 RID: 9792
		public IExplodableThrowable explodable;
	}
}
