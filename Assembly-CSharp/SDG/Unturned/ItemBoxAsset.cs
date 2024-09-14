using System;

namespace SDG.Unturned
{
	// Token: 0x020002F5 RID: 757
	public class ItemBoxAsset : ItemAsset
	{
		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x060016B0 RID: 5808 RVA: 0x00053C9F File Offset: 0x00051E9F
		public int generate
		{
			get
			{
				return this._generate;
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x060016B1 RID: 5809 RVA: 0x00053CA7 File Offset: 0x00051EA7
		public int destroy
		{
			get
			{
				return this._destroy;
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x060016B2 RID: 5810 RVA: 0x00053CAF File Offset: 0x00051EAF
		public int[] drops
		{
			get
			{
				return this._drops;
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x060016B3 RID: 5811 RVA: 0x00053CB7 File Offset: 0x00051EB7
		// (set) Token: 0x060016B4 RID: 5812 RVA: 0x00053CBF File Offset: 0x00051EBF
		public EBoxItemOrigin itemOrigin { get; protected set; }

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x060016B5 RID: 5813 RVA: 0x00053CC8 File Offset: 0x00051EC8
		// (set) Token: 0x060016B6 RID: 5814 RVA: 0x00053CD0 File Offset: 0x00051ED0
		public EBoxProbabilityModel probabilityModel { get; protected set; }

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x060016B7 RID: 5815 RVA: 0x00053CD9 File Offset: 0x00051ED9
		// (set) Token: 0x060016B8 RID: 5816 RVA: 0x00053CE1 File Offset: 0x00051EE1
		public bool containsBonusItems { get; protected set; }

		// Token: 0x060016B9 RID: 5817 RVA: 0x00053CEC File Offset: 0x00051EEC
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._generate = data.ParseInt32("Generate", 0);
			this._destroy = data.ParseInt32("Destroy", 0);
			this._drops = new int[data.ParseInt32("Drops", 0)];
			for (int i = 0; i < this.drops.Length; i++)
			{
				this.drops[i] = data.ParseInt32("Drop_" + i.ToString(), 0);
			}
			this.itemOrigin = data.ParseEnum<EBoxItemOrigin>("Item_Origin", EBoxItemOrigin.Unbox);
			this.probabilityModel = data.ParseEnum<EBoxProbabilityModel>("Probability_Model", EBoxProbabilityModel.Original);
			this.containsBonusItems = data.ParseBool("Contains_Bonus_Items", false);
		}

		// Token: 0x040009ED RID: 2541
		protected int _generate;

		// Token: 0x040009EE RID: 2542
		protected int _destroy;

		// Token: 0x040009EF RID: 2543
		protected int[] _drops;
	}
}
