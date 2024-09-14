using System;

namespace SDG.Unturned
{
	// Token: 0x020002F6 RID: 758
	public class ItemOilPumpAsset : ItemBarricadeAsset
	{
		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x060016BB RID: 5819 RVA: 0x00053DAE File Offset: 0x00051FAE
		// (set) Token: 0x060016BC RID: 5820 RVA: 0x00053DB6 File Offset: 0x00051FB6
		public ushort fuelCapacity { get; protected set; }

		// Token: 0x060016BD RID: 5821 RVA: 0x00053DBF File Offset: 0x00051FBF
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this.fuelCapacity = data.ParseUInt16("Fuel_Capacity", 0);
		}
	}
}
