using System;

namespace SDG.Provider
{
	// Token: 0x02000039 RID: 57
	public class UnturnedEconInfo
	{
		// Token: 0x060001C1 RID: 449 RVA: 0x000092CC File Offset: 0x000074CC
		public UnturnedEconInfo()
		{
			this.name = "";
			this.type = "";
			this.description = "";
			this.name_color = "";
			this.itemdefid = 0;
			this.scraps = 0;
			this.item_guid = Guid.Empty;
			this.item_skin = 0;
			this.item_effect = 0;
			this.vehicle_guid = Guid.Empty;
			this.quality = UnturnedEconInfo.EQuality.None;
		}

		// Token: 0x040000E4 RID: 228
		public string name;

		// Token: 0x040000E5 RID: 229
		public string type;

		// Token: 0x040000E6 RID: 230
		public string description;

		// Token: 0x040000E7 RID: 231
		public string name_color;

		// Token: 0x040000E8 RID: 232
		public int itemdefid;

		// Token: 0x040000E9 RID: 233
		public bool marketable;

		// Token: 0x040000EA RID: 234
		public int scraps;

		// Token: 0x040000EB RID: 235
		public Guid item_guid;

		// Token: 0x040000EC RID: 236
		public int item_skin;

		// Token: 0x040000ED RID: 237
		public int item_effect;

		// Token: 0x040000EE RID: 238
		public Guid vehicle_guid;

		// Token: 0x040000EF RID: 239
		public UnturnedEconInfo.EQuality quality;

		// Token: 0x0200084E RID: 2126
		public enum EQuality
		{
			// Token: 0x04003134 RID: 12596
			None,
			// Token: 0x04003135 RID: 12597
			Common,
			// Token: 0x04003136 RID: 12598
			Uncommon,
			// Token: 0x04003137 RID: 12599
			Gold,
			// Token: 0x04003138 RID: 12600
			Rare,
			// Token: 0x04003139 RID: 12601
			Epic,
			// Token: 0x0400313A RID: 12602
			Legendary,
			// Token: 0x0400313B RID: 12603
			Mythical,
			// Token: 0x0400313C RID: 12604
			Premium,
			// Token: 0x0400313D RID: 12605
			Achievement
		}

		/// <summary>
		/// This enum exists for sorting items based on rarity, and is derived from quality.
		/// Quality order cannot be changed due to loading from older files, but this one is ordered
		/// from lowest rarity to highest rarity and should match entries in quality.
		/// </summary>
		// Token: 0x0200084F RID: 2127
		public enum ERarity
		{
			// Token: 0x0400313F RID: 12607
			Common,
			// Token: 0x04003140 RID: 12608
			Uncommon,
			// Token: 0x04003141 RID: 12609
			Achievement,
			// Token: 0x04003142 RID: 12610
			Unknown,
			// Token: 0x04003143 RID: 12611
			Gold,
			// Token: 0x04003144 RID: 12612
			Premium,
			// Token: 0x04003145 RID: 12613
			Rare,
			// Token: 0x04003146 RID: 12614
			Epic,
			// Token: 0x04003147 RID: 12615
			Legendary,
			// Token: 0x04003148 RID: 12616
			Mythical
		}
	}
}
