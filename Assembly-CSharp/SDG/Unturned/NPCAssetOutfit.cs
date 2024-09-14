using System;

namespace SDG.Unturned
{
	// Token: 0x02000352 RID: 850
	public class NPCAssetOutfit
	{
		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x0600199E RID: 6558 RVA: 0x0005C6AB File Offset: 0x0005A8AB
		// (set) Token: 0x0600199F RID: 6559 RVA: 0x0005C6B3 File Offset: 0x0005A8B3
		[Obsolete]
		public ushort shirt { get; protected set; }

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x060019A0 RID: 6560 RVA: 0x0005C6BC File Offset: 0x0005A8BC
		// (set) Token: 0x060019A1 RID: 6561 RVA: 0x0005C6C4 File Offset: 0x0005A8C4
		[Obsolete]
		public ushort pants { get; protected set; }

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x060019A2 RID: 6562 RVA: 0x0005C6CD File Offset: 0x0005A8CD
		// (set) Token: 0x060019A3 RID: 6563 RVA: 0x0005C6D5 File Offset: 0x0005A8D5
		[Obsolete]
		public ushort hat { get; protected set; }

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x060019A4 RID: 6564 RVA: 0x0005C6DE File Offset: 0x0005A8DE
		// (set) Token: 0x060019A5 RID: 6565 RVA: 0x0005C6E6 File Offset: 0x0005A8E6
		[Obsolete]
		public ushort backpack { get; protected set; }

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x060019A6 RID: 6566 RVA: 0x0005C6EF File Offset: 0x0005A8EF
		// (set) Token: 0x060019A7 RID: 6567 RVA: 0x0005C6F7 File Offset: 0x0005A8F7
		[Obsolete]
		public ushort vest { get; protected set; }

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x060019A8 RID: 6568 RVA: 0x0005C700 File Offset: 0x0005A900
		// (set) Token: 0x060019A9 RID: 6569 RVA: 0x0005C708 File Offset: 0x0005A908
		[Obsolete]
		public ushort mask { get; protected set; }

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x060019AA RID: 6570 RVA: 0x0005C711 File Offset: 0x0005A911
		// (set) Token: 0x060019AB RID: 6571 RVA: 0x0005C719 File Offset: 0x0005A919
		[Obsolete]
		public ushort glasses { get; protected set; }

		// Token: 0x060019AC RID: 6572 RVA: 0x0005C724 File Offset: 0x0005A924
		public NPCAssetOutfit(DatDictionary data, ENPCHoliday holiday)
		{
			string text;
			if (holiday != ENPCHoliday.HALLOWEEN)
			{
				if (holiday != ENPCHoliday.CHRISTMAS)
				{
					text = "";
				}
				else
				{
					text = "Christmas_";
				}
			}
			else
			{
				text = "Halloween_";
			}
			this.shirt = data.ParseGuidOrLegacyId(text + "Shirt", out this.shirtGuid);
			this.pants = data.ParseGuidOrLegacyId(text + "Pants", out this.pantsGuid);
			this.hat = data.ParseGuidOrLegacyId(text + "Hat", out this.hatGuid);
			this.backpack = data.ParseGuidOrLegacyId(text + "Backpack", out this.backpackGuid);
			this.vest = data.ParseGuidOrLegacyId(text + "Vest", out this.vestGuid);
			this.mask = data.ParseGuidOrLegacyId(text + "Mask", out this.maskGuid);
			this.glasses = data.ParseGuidOrLegacyId(text + "Glasses", out this.glassesGuid);
		}

		// Token: 0x04000BA2 RID: 2978
		public Guid shirtGuid;

		// Token: 0x04000BA3 RID: 2979
		public Guid pantsGuid;

		// Token: 0x04000BA4 RID: 2980
		public Guid hatGuid;

		// Token: 0x04000BA5 RID: 2981
		public Guid backpackGuid;

		// Token: 0x04000BA6 RID: 2982
		public Guid vestGuid;

		// Token: 0x04000BA7 RID: 2983
		public Guid maskGuid;

		// Token: 0x04000BA8 RID: 2984
		public Guid glassesGuid;
	}
}
