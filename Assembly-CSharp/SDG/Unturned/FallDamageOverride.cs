using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Overrides how fall damage is calculated when landing on this game object or its descendants.
	/// </summary>
	// Token: 0x020005CA RID: 1482
	[AddComponentMenu("Unturned/Fall Damage Override")]
	public class FallDamageOverride : MonoBehaviour
	{
		// Token: 0x040019E8 RID: 6632
		public FallDamageOverride.EMode Mode = FallDamageOverride.EMode.PreventFallDamage;

		/// <summary>
		/// Could be extended in the future to increase, decrease, or set fall damage.
		/// </summary>
		// Token: 0x02000997 RID: 2455
		public enum EMode
		{
			/// <summary>
			/// Potentially useful for an event to toggle the override.
			/// </summary>
			// Token: 0x040033C2 RID: 13250
			None,
			/// <summary>
			/// Character will not take any fall damage.
			/// </summary>
			// Token: 0x040033C3 RID: 13251
			PreventFallDamage
		}
	}
}
