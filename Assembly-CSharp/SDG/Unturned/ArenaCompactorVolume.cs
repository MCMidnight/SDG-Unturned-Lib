using System;

namespace SDG.Unturned
{
	// Token: 0x020004AA RID: 1194
	public class ArenaCompactorVolume : LevelVolume<ArenaCompactorVolume, ArenaCompactorVolumeManager>
	{
		// Token: 0x06002508 RID: 9480 RVA: 0x00093A10 File Offset: 0x00091C10
		protected override void Awake()
		{
			this.supportsBoxShape = false;
			base.Awake();
		}
	}
}
