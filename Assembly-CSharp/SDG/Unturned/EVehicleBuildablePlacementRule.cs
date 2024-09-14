using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Controls whether vehicle allows barricades to be attached to it.
	/// </summary>
	// Token: 0x02000375 RID: 885
	public enum EVehicleBuildablePlacementRule
	{
		/// <summary>
		/// Vehicle does not override placement. This means, by default, that barricades can be placed on the vehicle
		/// unless the barricade sets Allow_Placement_On_Vehicle to false. (e.g., beds and sentry guns) Note that
		/// gameplay config Bypass_Buildable_Mobility, if true, takes priority.
		/// </summary>
		// Token: 0x04000C56 RID: 3158
		None,
		/// <summary>
		/// Vehicle allows any barricade to be placed on it, regardless of the barricade's Allow_Placement_On_Vehicle
		/// setting. The legacy option for this was the Supports_Mobile_Buildables flag. Vanilla trains originally
		/// used this option, but it was exploited to move beds into tunnel walls.
		/// </summary>
		// Token: 0x04000C57 RID: 3159
		AlwaysAllow,
		/// <summary>
		/// Vehicle prevents any barricade from being placed on it. Note that gameplay config Bypass_Buildable_Mobility,
		/// if true, takes priority.
		/// </summary>
		// Token: 0x04000C58 RID: 3160
		Block
	}
}
