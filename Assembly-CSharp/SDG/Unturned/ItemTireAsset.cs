using System;

namespace SDG.Unturned
{
	// Token: 0x02000307 RID: 775
	public class ItemTireAsset : ItemVehicleRepairToolAsset
	{
		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x0600176D RID: 5997 RVA: 0x00055940 File Offset: 0x00053B40
		public EUseableTireMode mode
		{
			get
			{
				return this._mode;
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x0600176E RID: 5998 RVA: 0x00055948 File Offset: 0x00053B48
		public override bool shouldFriendlySentryTargetUser
		{
			get
			{
				return this.mode == EUseableTireMode.REMOVE;
			}
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x00055953 File Offset: 0x00053B53
		public override bool canBeUsedInSafezone(SafezoneNode safezone, bool byAdmin)
		{
			return this.mode == EUseableTireMode.ADD;
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x0005595E File Offset: 0x00053B5E
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._mode = (EUseableTireMode)Enum.Parse(typeof(EUseableTireMode), data.GetString("Mode", null), true);
		}

		// Token: 0x04000A67 RID: 2663
		private EUseableTireMode _mode;
	}
}
