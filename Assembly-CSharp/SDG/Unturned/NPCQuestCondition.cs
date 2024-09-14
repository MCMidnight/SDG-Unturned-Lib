using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x0200033E RID: 830
	public class NPCQuestCondition : NPCLogicCondition
	{
		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x060018E4 RID: 6372 RVA: 0x0005A06F File Offset: 0x0005826F
		// (set) Token: 0x060018E5 RID: 6373 RVA: 0x0005A077 File Offset: 0x00058277
		public Guid questGuid { get; private set; }

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x060018E6 RID: 6374 RVA: 0x0005A080 File Offset: 0x00058280
		// (set) Token: 0x060018E7 RID: 6375 RVA: 0x0005A088 File Offset: 0x00058288
		[Obsolete]
		public ushort id { get; protected set; }

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x060018E8 RID: 6376 RVA: 0x0005A091 File Offset: 0x00058291
		// (set) Token: 0x060018E9 RID: 6377 RVA: 0x0005A099 File Offset: 0x00058299
		public ENPCQuestStatus status { get; protected set; }

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x060018EA RID: 6378 RVA: 0x0005A0A2 File Offset: 0x000582A2
		// (set) Token: 0x060018EB RID: 6379 RVA: 0x0005A0AA File Offset: 0x000582AA
		public bool ignoreNPC { get; protected set; }

		// Token: 0x060018EC RID: 6380 RVA: 0x0005A0B3 File Offset: 0x000582B3
		public QuestAsset GetQuestAsset()
		{
			return Assets.FindNpcAssetByGuidOrLegacyId<QuestAsset>(this.questGuid, this.id);
		}

		// Token: 0x060018ED RID: 6381 RVA: 0x0005A0C8 File Offset: 0x000582C8
		public override bool isConditionMet(Player player)
		{
			QuestAsset questAsset = this.GetQuestAsset();
			return base.doesLogicPass<ENPCQuestStatus>(player.quests.GetQuestStatus(questAsset), this.status);
		}

		// Token: 0x060018EE RID: 6382 RVA: 0x0005A0F4 File Offset: 0x000582F4
		public override void ApplyCondition(Player player)
		{
			if (!this.shouldReset)
			{
				return;
			}
			QuestAsset questAsset = this.GetQuestAsset();
			if (questAsset == null)
			{
				return;
			}
			switch (this.status)
			{
			case ENPCQuestStatus.ACTIVE:
				player.quests.ServerRemoveQuest(questAsset);
				return;
			case ENPCQuestStatus.READY:
				player.quests.CompleteQuest(questAsset, this.ignoreNPC);
				return;
			case ENPCQuestStatus.COMPLETED:
				player.quests.sendRemoveFlag(questAsset.id);
				return;
			default:
				return;
			}
		}

		// Token: 0x060018EF RID: 6383 RVA: 0x0005A162 File Offset: 0x00058362
		public override bool isAssociatedWithFlag(ushort flagID)
		{
			return flagID == this.id;
		}

		// Token: 0x060018F0 RID: 6384 RVA: 0x0005A170 File Offset: 0x00058370
		internal override void GatherAssociatedFlags(HashSet<ushort> associatedFlags)
		{
			if (this.id > 0)
			{
				associatedFlags.Add(this.id);
				return;
			}
			QuestAsset questAsset = this.GetQuestAsset();
			if (questAsset != null)
			{
				associatedFlags.Add(questAsset.id);
			}
		}

		// Token: 0x060018F1 RID: 6385 RVA: 0x0005A1AB File Offset: 0x000583AB
		public NPCQuestCondition(Guid newQuestGuid, ushort newID, ENPCQuestStatus newStatus, bool newIgnoreNPC, ENPCLogicType newLogicType, string newText, bool newShouldReset) : base(newLogicType, newText, newShouldReset)
		{
			this.questGuid = newQuestGuid;
			this.id = newID;
			this.status = newStatus;
			this.ignoreNPC = newIgnoreNPC;
		}
	}
}
