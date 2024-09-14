using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002E3 RID: 739
	public class ItemFuelAsset : ItemAsset
	{
		// Token: 0x17000390 RID: 912
		// (get) Token: 0x060015F9 RID: 5625 RVA: 0x00051643 File Offset: 0x0004F843
		public AudioClip use
		{
			get
			{
				return this._use;
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x060015FA RID: 5626 RVA: 0x0005164B File Offset: 0x0004F84B
		public ushort fuel
		{
			get
			{
				return this._fuel;
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x060015FB RID: 5627 RVA: 0x00051653 File Offset: 0x0004F853
		// (set) Token: 0x060015FC RID: 5628 RVA: 0x0005165B File Offset: 0x0004F85B
		public bool shouldDeleteAfterFillingTarget { get; protected set; }

		// Token: 0x060015FD RID: 5629 RVA: 0x00051664 File Offset: 0x0004F864
		public override byte[] getState(EItemOrigin origin)
		{
			byte[] array = new byte[2];
			if (origin == EItemOrigin.ADMIN || this.shouldAlwaysSpawnFull)
			{
				array[0] = this.fuelState[0];
				array[1] = this.fuelState[1];
			}
			return array;
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x0005169C File Offset: 0x0004F89C
		public override void BuildDescription(ItemDescriptionBuilder builder, Item itemInstance)
		{
			base.BuildDescription(builder, itemInstance);
			ushort num = BitConverter.ToUInt16(itemInstance.state, 0);
			float num2 = (float)num / (float)this.fuel;
			builder.Append(PlayerDashboardInventoryUI.localization.format("ItemDescription_FuelAmountWithCapacity", num, this.fuel, num2.ToString("P")), 2000);
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x00051704 File Offset: 0x0004F904
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._use = bundle.load<AudioClip>("Use");
			this._fuel = data.ParseUInt16("Fuel", 0);
			this.fuelState = BitConverter.GetBytes(this.fuel);
			this.shouldDeleteAfterFillingTarget = data.ParseBool("Delete_After_Filling_Target", false);
			this.shouldAlwaysSpawnFull = data.ParseBool("Always_Spawn_Full", false);
		}

		// Token: 0x0400093D RID: 2365
		protected AudioClip _use;

		// Token: 0x0400093E RID: 2366
		protected ushort _fuel;

		// Token: 0x04000940 RID: 2368
		private bool shouldAlwaysSpawnFull;

		// Token: 0x04000941 RID: 2369
		private byte[] fuelState;
	}
}
