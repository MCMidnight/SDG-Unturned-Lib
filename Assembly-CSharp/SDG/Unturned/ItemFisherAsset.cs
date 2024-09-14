using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020002E1 RID: 737
	public class ItemFisherAsset : ItemAsset
	{
		// Token: 0x1700038C RID: 908
		// (get) Token: 0x060015F2 RID: 5618 RVA: 0x00051579 File Offset: 0x0004F779
		public AudioClip cast
		{
			get
			{
				return this._cast;
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x060015F3 RID: 5619 RVA: 0x00051581 File Offset: 0x0004F781
		public AudioClip reel
		{
			get
			{
				return this._reel;
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x060015F4 RID: 5620 RVA: 0x00051589 File Offset: 0x0004F789
		public AudioClip tug
		{
			get
			{
				return this._tug;
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x060015F5 RID: 5621 RVA: 0x00051591 File Offset: 0x0004F791
		public ushort rewardID
		{
			get
			{
				return this._rewardID;
			}
		}

		// Token: 0x060015F6 RID: 5622 RVA: 0x0005159C File Offset: 0x0004F79C
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._cast = bundle.load<AudioClip>("Cast");
			this._reel = bundle.load<AudioClip>("Reel");
			this._tug = bundle.load<AudioClip>("Tug");
			this._rewardID = data.ParseUInt16("Reward_ID", 0);
			this.rewardExperienceMin = data.ParseInt32("Reward_Experience_Min", 3);
			this.rewardExperienceMax = data.ParseInt32("Reward_Experience_Max", 3);
			this.rewardsList.Parse(data, localization, this, "Quest_Rewards", "Quest_Reward_");
		}

		// Token: 0x04000936 RID: 2358
		private AudioClip _cast;

		// Token: 0x04000937 RID: 2359
		private AudioClip _reel;

		// Token: 0x04000938 RID: 2360
		private AudioClip _tug;

		// Token: 0x04000939 RID: 2361
		private ushort _rewardID;

		// Token: 0x0400093A RID: 2362
		public int rewardExperienceMin;

		// Token: 0x0400093B RID: 2363
		public int rewardExperienceMax;

		// Token: 0x0400093C RID: 2364
		internal NPCRewardsList rewardsList;
	}
}
