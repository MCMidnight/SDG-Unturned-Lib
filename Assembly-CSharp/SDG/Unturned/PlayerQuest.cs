using System;

namespace SDG.Unturned
{
	// Token: 0x02000649 RID: 1609
	public class PlayerQuest
	{
		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x060034BB RID: 13499 RVA: 0x000F42D1 File Offset: 0x000F24D1
		// (set) Token: 0x060034BC RID: 13500 RVA: 0x000F42D9 File Offset: 0x000F24D9
		public ushort id { get; private set; }

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x060034BD RID: 13501 RVA: 0x000F42E2 File Offset: 0x000F24E2
		// (set) Token: 0x060034BE RID: 13502 RVA: 0x000F42EA File Offset: 0x000F24EA
		public QuestAsset asset { get; protected set; }

		// Token: 0x060034BF RID: 13503 RVA: 0x000F42F3 File Offset: 0x000F24F3
		public PlayerQuest(ushort newID)
		{
			this.id = newID;
			this.asset = (Assets.find(EAssetType.NPC, this.id) as QuestAsset);
		}

		// Token: 0x060034C0 RID: 13504 RVA: 0x000F431A File Offset: 0x000F251A
		internal PlayerQuest(QuestAsset asset)
		{
			this.asset = asset;
			this.id = ((asset != null) ? asset.id : 0);
		}
	}
}
