using System;

namespace SDG.Unturned
{
	// Token: 0x02000512 RID: 1298
	public class VehicleSpawn
	{
		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x0600289F RID: 10399 RVA: 0x000ACFA8 File Offset: 0x000AB1A8
		public ushort vehicle
		{
			get
			{
				return this._vehicle;
			}
		}

		// Token: 0x060028A0 RID: 10400 RVA: 0x000ACFB0 File Offset: 0x000AB1B0
		public VehicleSpawn(ushort newVehicle)
		{
			this._vehicle = newVehicle;
		}

		// Token: 0x0400159D RID: 5533
		private ushort _vehicle;
	}
}
