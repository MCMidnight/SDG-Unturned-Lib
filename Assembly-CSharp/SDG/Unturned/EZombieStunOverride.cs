using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Used when damaging zombies to override in which situations they are stunned.
	/// </summary>
	// Token: 0x02000828 RID: 2088
	public enum EZombieStunOverride
	{
		/// <summary>
		/// Default stun behaviour determined by damage dealt.
		/// </summary>
		// Token: 0x04003073 RID: 12403
		None,
		/// <summary>
		/// Don't stun even if damage is over threshold.
		/// </summary>
		// Token: 0x04003074 RID: 12404
		Never,
		/// <summary>
		/// Stun regardless of damage.
		/// </summary>
		// Token: 0x04003075 RID: 12405
		Always
	}
}
