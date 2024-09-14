using System;
using SDG.Unturned;

namespace SDG.Framework.Landscapes
{
	// Token: 0x020000A5 RID: 165
	public class LandscapeHoleVolume : LevelVolume<LandscapeHoleVolume, LandscapeHoleVolumeManager>
	{
		// Token: 0x06000448 RID: 1096 RVA: 0x000113B7 File Offset: 0x0000F5B7
		protected override void Awake()
		{
			this.supportsSphereShape = false;
			base.Awake();
		}
	}
}
