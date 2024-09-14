using System;

namespace SDG.Unturned
{
	// Token: 0x0200016A RID: 362
	internal class GlazierProxy_IMGUI : GlazierElementBase_IMGUI
	{
		// Token: 0x06000940 RID: 2368 RVA: 0x00020093 File Offset: 0x0001E293
		public GlazierProxy_IMGUI(SleekWrapper owner)
		{
			this.owner = owner;
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x000200A2 File Offset: 0x0001E2A2
		public override void Update()
		{
			this.owner.OnUpdate();
			base.Update();
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x000200B5 File Offset: 0x0001E2B5
		public override void InternalDestroy()
		{
			this.owner.OnDestroy();
			base.InternalDestroy();
		}

		// Token: 0x0400038D RID: 909
		private SleekWrapper owner;
	}
}
