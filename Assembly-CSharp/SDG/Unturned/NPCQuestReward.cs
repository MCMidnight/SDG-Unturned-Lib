using System;

namespace SDG.Unturned
{
	// Token: 0x0200033F RID: 831
	public class NPCQuestReward : INPCReward
	{
		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x060018F2 RID: 6386 RVA: 0x0005A1D6 File Offset: 0x000583D6
		// (set) Token: 0x060018F3 RID: 6387 RVA: 0x0005A1DE File Offset: 0x000583DE
		public Guid questGuid { get; private set; }

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x060018F4 RID: 6388 RVA: 0x0005A1E7 File Offset: 0x000583E7
		// (set) Token: 0x060018F5 RID: 6389 RVA: 0x0005A1EF File Offset: 0x000583EF
		[Obsolete]
		public ushort id { get; protected set; }

		// Token: 0x060018F6 RID: 6390 RVA: 0x0005A1F8 File Offset: 0x000583F8
		public QuestAsset GetQuestAsset()
		{
			return Assets.FindNpcAssetByGuidOrLegacyId<QuestAsset>(this.questGuid, this.id);
		}

		// Token: 0x060018F7 RID: 6391 RVA: 0x0005A20C File Offset: 0x0005840C
		public override void GrantReward(Player player)
		{
			QuestAsset questAsset = this.GetQuestAsset();
			if (questAsset == null)
			{
				return;
			}
			player.quests.ServerAddQuest(questAsset);
		}

		// Token: 0x060018F8 RID: 6392 RVA: 0x0005A230 File Offset: 0x00058430
		public NPCQuestReward(Guid newQuestGuid, ushort newID, string newText) : base(newText)
		{
			this.questGuid = newQuestGuid;
			this.id = newID;
		}
	}
}
