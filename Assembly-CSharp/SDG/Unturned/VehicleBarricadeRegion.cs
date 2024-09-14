using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000541 RID: 1345
	public class VehicleBarricadeRegion : BarricadeRegion
	{
		// Token: 0x06002ABE RID: 10942 RVA: 0x000B7007 File Offset: 0x000B5207
		public VehicleBarricadeRegion(Transform parent, InteractableVehicle vehicle, int subvehicleIndex) : base(parent)
		{
			this.vehicle = vehicle;
			this.subvehicleIndex = subvehicleIndex;
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x06002ABF RID: 10943 RVA: 0x000B701E File Offset: 0x000B521E
		// (set) Token: 0x06002AC0 RID: 10944 RVA: 0x000B7026 File Offset: 0x000B5226
		public InteractableVehicle vehicle { get; private set; }

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x06002AC1 RID: 10945 RVA: 0x000B702F File Offset: 0x000B522F
		// (set) Token: 0x06002AC2 RID: 10946 RVA: 0x000B7037 File Offset: 0x000B5237
		public int subvehicleIndex { get; private set; }

		// Token: 0x040016C8 RID: 5832
		internal NetId _netId;
	}
}
