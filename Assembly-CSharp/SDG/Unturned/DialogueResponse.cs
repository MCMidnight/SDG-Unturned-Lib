using System;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x020002AB RID: 683
	public class DialogueResponse : DialogueElement
	{
		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06001497 RID: 5271 RVA: 0x0004CC09 File Offset: 0x0004AE09
		// (set) Token: 0x06001498 RID: 5272 RVA: 0x0004CC11 File Offset: 0x0004AE11
		public byte[] messages { get; protected set; }

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06001499 RID: 5273 RVA: 0x0004CC1A File Offset: 0x0004AE1A
		// (set) Token: 0x0600149A RID: 5274 RVA: 0x0004CC22 File Offset: 0x0004AE22
		public ushort dialogue { [Obsolete] get; protected set; }

		// Token: 0x0600149B RID: 5275 RVA: 0x0004CC2B File Offset: 0x0004AE2B
		public bool IsDialogueRefNull()
		{
			return this.dialogue == 0 && GuidExtension.IsEmpty(this.dialogueGuid);
		}

		// Token: 0x0600149C RID: 5276 RVA: 0x0004CC42 File Offset: 0x0004AE42
		public DialogueAsset FindDialogueAsset()
		{
			return Assets.FindNpcAssetByGuidOrLegacyId<DialogueAsset>(this.dialogueGuid, this.dialogue);
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x0600149D RID: 5277 RVA: 0x0004CC55 File Offset: 0x0004AE55
		// (set) Token: 0x0600149E RID: 5278 RVA: 0x0004CC5D File Offset: 0x0004AE5D
		public ushort quest { [Obsolete] get; protected set; }

		// Token: 0x0600149F RID: 5279 RVA: 0x0004CC66 File Offset: 0x0004AE66
		public bool IsQuestRefNull()
		{
			return this.quest == 0 && GuidExtension.IsEmpty(this.questGuid);
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x0004CC7D File Offset: 0x0004AE7D
		public QuestAsset FindQuestAsset()
		{
			return Assets.FindNpcAssetByGuidOrLegacyId<QuestAsset>(this.questGuid, this.quest);
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x060014A1 RID: 5281 RVA: 0x0004CC90 File Offset: 0x0004AE90
		// (set) Token: 0x060014A2 RID: 5282 RVA: 0x0004CC98 File Offset: 0x0004AE98
		public ushort vendor { [Obsolete] get; protected set; }

		// Token: 0x060014A3 RID: 5283 RVA: 0x0004CCA1 File Offset: 0x0004AEA1
		public bool IsVendorRefNull()
		{
			return this.vendor == 0 && GuidExtension.IsEmpty(this.vendorGuid);
		}

		// Token: 0x060014A4 RID: 5284 RVA: 0x0004CCB8 File Offset: 0x0004AEB8
		public VendorAsset FindVendorAsset()
		{
			return Assets.FindNpcAssetByGuidOrLegacyId<VendorAsset>(this.vendorGuid, this.vendor);
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x060014A5 RID: 5285 RVA: 0x0004CCCB File Offset: 0x0004AECB
		// (set) Token: 0x060014A6 RID: 5286 RVA: 0x0004CCD3 File Offset: 0x0004AED3
		public string text { get; protected set; }

		// Token: 0x060014A7 RID: 5287 RVA: 0x0004CCDC File Offset: 0x0004AEDC
		public DialogueResponse(byte newID, byte[] newMessages, ushort newDialogue, Guid newDialogueGuid, ushort newQuest, Guid newQuestGuid, ushort newVendor, Guid newVendorGuid, string newText, INPCCondition[] newConditions, NPCRewardsList newRewardsList) : base(newID, newConditions, newRewardsList)
		{
			this.messages = newMessages;
			this.dialogue = newDialogue;
			this.dialogueGuid = newDialogueGuid;
			this.quest = newQuest;
			this.questGuid = newQuestGuid;
			this.vendor = newVendor;
			this.vendorGuid = newVendorGuid;
			this.text = newText;
		}

		// Token: 0x0400071B RID: 1819
		public Guid dialogueGuid;

		// Token: 0x0400071D RID: 1821
		public Guid questGuid;

		// Token: 0x0400071F RID: 1823
		public Guid vendorGuid;
	}
}
