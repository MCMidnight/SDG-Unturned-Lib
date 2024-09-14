using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Used for item placement in displays / holsters, and whether useable can be placed in primary/secondary slot.
	/// </summary>
	// Token: 0x02000495 RID: 1173
	public enum ESlotType
	{
		/// <summary>
		/// Cannot be placed in primary nor secondary slots, but can be equipped from bag.
		/// </summary>
		// Token: 0x04001289 RID: 4745
		NONE,
		/// <summary>
		/// Can be placed in primary slot, but cannot be equipped in secondary or bag.
		/// </summary>
		// Token: 0x0400128A RID: 4746
		PRIMARY,
		/// <summary>
		/// Can be placed in primary or secondary slot, but cannot be equipped from bag.
		/// </summary>
		// Token: 0x0400128B RID: 4747
		SECONDARY,
		/// <summary>
		/// Only used by NPCs.
		/// </summary>
		// Token: 0x0400128C RID: 4748
		TERTIARY,
		/// <summary>
		/// Can be placed in primary, secondary, or equipped while in bag.
		/// </summary>
		// Token: 0x0400128D RID: 4749
		ANY
	}
}
