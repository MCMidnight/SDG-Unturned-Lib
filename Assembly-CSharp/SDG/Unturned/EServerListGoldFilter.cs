using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Filter for whether the Permanent Gold Upgrade DLC is required to join a server.
	/// </summary>
	// Token: 0x0200043F RID: 1087
	public enum EServerListGoldFilter
	{
		/// <summary>
		/// All servers pass the filter.
		/// </summary>
		// Token: 0x04000FFF RID: 4095
		Any,
		/// <summary>
		/// Only non-gold servers pass the filter.
		/// </summary>
		// Token: 0x04001000 RID: 4096
		DoesNotRequireGold,
		/// <summary>
		/// Only gold servers pass the filter.
		/// </summary>
		// Token: 0x04001001 RID: 4097
		RequiresGold
	}
}
