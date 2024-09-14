using System;

namespace SDG.Unturned
{
	// Token: 0x020002D6 RID: 726
	public class ItemBeaconAsset : ItemBarricadeAsset
	{
		// Token: 0x17000346 RID: 838
		// (get) Token: 0x0600156A RID: 5482 RVA: 0x0004FA48 File Offset: 0x0004DC48
		public ushort wave
		{
			get
			{
				return this._wave;
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x0600156B RID: 5483 RVA: 0x0004FA50 File Offset: 0x0004DC50
		public byte rewards
		{
			get
			{
				return this._rewards;
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x0600156C RID: 5484 RVA: 0x0004FA58 File Offset: 0x0004DC58
		public ushort rewardID
		{
			get
			{
				return this._rewardID;
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x0600156D RID: 5485 RVA: 0x0004FA60 File Offset: 0x0004DC60
		// (set) Token: 0x0600156E RID: 5486 RVA: 0x0004FA68 File Offset: 0x0004DC68
		public bool ShouldScaleWithNumberOfParticipants { get; private set; }

		// Token: 0x0600156F RID: 5487 RVA: 0x0004FA74 File Offset: 0x0004DC74
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._wave = data.ParseUInt16("Wave", 0);
			this._rewards = data.ParseUInt8("Rewards", 0);
			this._rewardID = data.ParseUInt16("Reward_ID", 0);
			this.ShouldScaleWithNumberOfParticipants = data.ParseBool("Enable_Participant_Scaling", true);
		}

		// Token: 0x040008DF RID: 2271
		private ushort _wave;

		// Token: 0x040008E0 RID: 2272
		private byte _rewards;

		// Token: 0x040008E1 RID: 2273
		private ushort _rewardID;
	}
}
