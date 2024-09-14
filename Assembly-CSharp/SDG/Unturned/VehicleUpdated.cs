using System;

namespace SDG.Unturned
{
	// Token: 0x02000641 RID: 1601
	// (Invoke) Token: 0x0600345F RID: 13407
	public delegate void VehicleUpdated(bool isDriveable, ushort newFuel, ushort maxFuel, float newSpeed, float minSpeed, float maxSpeed, ushort newHeath, ushort maxHealth, ushort newBatteryCharge);
}
