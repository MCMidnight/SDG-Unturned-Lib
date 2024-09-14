using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Associates a train vehicle ID with the index of a road path to spawn it on.
	/// The level only spawns the train if this vehicle ID isn't present in the map yet, so every train on the map has to be different.
	/// </summary>
	// Token: 0x020004E1 RID: 1249
	public class LevelTrainAssociation
	{
		// Token: 0x040013F7 RID: 5111
		public ushort VehicleID;

		// Token: 0x040013F8 RID: 5112
		public ushort RoadIndex;

		// Token: 0x040013F9 RID: 5113
		public float Min_Spawn_Placement = 0.1f;

		// Token: 0x040013FA RID: 5114
		public float Max_Spawn_Placement = 0.9f;
	}
}
