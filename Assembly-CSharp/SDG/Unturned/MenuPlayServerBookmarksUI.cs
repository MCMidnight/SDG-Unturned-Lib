using System;

namespace SDG.Unturned
{
	// Token: 0x020007A0 RID: 1952
	public class MenuPlayServerBookmarksUI : SleekFullscreenBox
	{
		// Token: 0x060040D9 RID: 16601 RVA: 0x00151F80 File Offset: 0x00150180
		public void open()
		{
			if (this.active)
			{
				return;
			}
			this.active = true;
			base.AnimateIntoView();
		}

		// Token: 0x060040DA RID: 16602 RVA: 0x00151F98 File Offset: 0x00150198
		public void close()
		{
			if (!this.active)
			{
				return;
			}
			this.active = false;
			base.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x060040DB RID: 16603 RVA: 0x00151FBA File Offset: 0x001501BA
		public MenuPlayServerBookmarksUI()
		{
			this.active = false;
		}

		// Token: 0x040029C1 RID: 10689
		public bool active;
	}
}
