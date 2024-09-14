using System;

namespace SDG.Unturned
{
	/// <summary>
	/// 32-bit mask indicating to the server which admin powers are being used.
	/// Does not control which admin powers are available.
	/// </summary>
	// Token: 0x02000602 RID: 1538
	[Flags]
	public enum EPlayerAdminUsageFlags
	{
		// Token: 0x04001B86 RID: 7046
		None = 0,
		/// <summary>
		/// Player is using spectator camera.
		/// </summary>
		// Token: 0x04001B87 RID: 7047
		Freecam = 1,
		/// <summary>
		/// Player is using barricade/structure transform tools.
		/// </summary>
		// Token: 0x04001B88 RID: 7048
		Workzone = 2,
		/// <summary>
		/// Player is using overlay showing player names and positions.
		/// </summary>
		// Token: 0x04001B89 RID: 7049
		SpectatorStatsOverlay = 4
	}
}
