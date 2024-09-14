using System;
using SDG.Unturned;

namespace SDG.Framework.Devkit
{
	// Token: 0x02000122 RID: 290
	public class NavClipVolume : LevelVolume<NavClipVolume, NavClipVolumeManager>
	{
		// Token: 0x0600077D RID: 1917 RVA: 0x0001B7FC File Offset: 0x000199FC
		protected override void Awake()
		{
			this.forceShouldAddCollider = true;
			base.Awake();
			this.volumeCollider.isTrigger = false;
			base.gameObject.layer = 22;
		}
	}
}
