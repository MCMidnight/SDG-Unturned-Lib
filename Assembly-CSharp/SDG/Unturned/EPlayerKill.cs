using System;

namespace SDG.Unturned
{
	/// <summary>
	/// 2023-01-25: fixing killing self with explosive to track kill under
	/// the assumption that this is only used for tracking stats. (public issue #2692)
	/// </summary>
	// Token: 0x020005EA RID: 1514
	public enum EPlayerKill
	{
		// Token: 0x04001A99 RID: 6809
		NONE,
		// Token: 0x04001A9A RID: 6810
		PLAYER,
		// Token: 0x04001A9B RID: 6811
		ZOMBIE,
		// Token: 0x04001A9C RID: 6812
		MEGA,
		// Token: 0x04001A9D RID: 6813
		ANIMAL,
		// Token: 0x04001A9E RID: 6814
		RESOURCE,
		// Token: 0x04001A9F RID: 6815
		OBJECT
	}
}
