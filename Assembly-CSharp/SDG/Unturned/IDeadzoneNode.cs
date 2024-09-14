using System;

namespace SDG.Unturned
{
	// Token: 0x020004B5 RID: 1205
	public interface IDeadzoneNode
	{
		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06002524 RID: 9508
		EDeadzoneType DeadzoneType { get; }

		/// <summary>
		/// Damage dealt to players while inside the volume if they *don't* have clothing matching the deadzone type.
		/// Could help prevent players from running in and out to grab a few items without dieing.
		/// </summary>
		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06002525 RID: 9509
		float UnprotectedDamagePerSecond { get; }

		/// <summary>
		/// Damage dealt to players while inside the volume if they *do* have clothing matching the deadzone type.
		/// For example, an area could be so dangerous that even with protection they take a constant 0.1 DPS.
		/// </summary>
		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06002526 RID: 9510
		float ProtectedDamagePerSecond { get; }

		/// <summary>
		/// Virus damage to players while inside the volume if they *don't* have clothing matching the deadzone type.
		/// Defaults to 6.25 to preserve behavior from before adding this property.
		/// </summary>
		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06002527 RID: 9511
		float UnprotectedRadiationPerSecond { get; }

		/// <summary>
		/// Rate of depletion from gasmask filter's quality/durability.
		/// Defaults to 0.4 to preserve behavior from before adding this property.
		/// </summary>
		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06002528 RID: 9512
		float MaskFilterDamagePerSecond { get; }
	}
}
