using System;

namespace SDG.Unturned
{
	// Token: 0x020002A9 RID: 681
	public class DialogueMessage : DialogueElement
	{
		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x0600148A RID: 5258 RVA: 0x0004CB5F File Offset: 0x0004AD5F
		// (set) Token: 0x0600148B RID: 5259 RVA: 0x0004CB67 File Offset: 0x0004AD67
		public DialoguePage[] pages { get; protected set; }

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x0600148C RID: 5260 RVA: 0x0004CB70 File Offset: 0x0004AD70
		// (set) Token: 0x0600148D RID: 5261 RVA: 0x0004CB78 File Offset: 0x0004AD78
		public byte[] responses { get; protected set; }

		/// <summary>
		/// Please refer to <see cref="M:SDG.Unturned.DialogueMessage.FindPrevDialogueAsset" />.
		/// </summary>
		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x0600148E RID: 5262 RVA: 0x0004CB81 File Offset: 0x0004AD81
		// (set) Token: 0x0600148F RID: 5263 RVA: 0x0004CB89 File Offset: 0x0004AD89
		public ushort prev { [Obsolete] get; protected set; }

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06001490 RID: 5264 RVA: 0x0004CB92 File Offset: 0x0004AD92
		// (set) Token: 0x06001491 RID: 5265 RVA: 0x0004CB9A File Offset: 0x0004AD9A
		internal byte? faceOverride { get; private set; }

		/// <summary>
		/// The dialogue to go to when a message has no available responses.
		/// If this is not specified the previous dialogue is used as a default.
		/// If neither is available then a default "goodbye" response is added.
		///
		/// For example, Chief_Police_Doughnuts_Accepted dialogue has a single message
		/// "Let's just keep this between the two of us." shown with "prev" dialogue
		/// set to the NPC's root dialogue asset.
		/// </summary>
		// Token: 0x06001492 RID: 5266 RVA: 0x0004CBA3 File Offset: 0x0004ADA3
		public DialogueAsset FindPrevDialogueAsset()
		{
			return Assets.FindNpcAssetByGuidOrLegacyId<DialogueAsset>(this.prevGuid, this.prev);
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x0004CBB6 File Offset: 0x0004ADB6
		public DialogueMessage(byte newID, DialoguePage[] newPages, byte[] newResponses, ushort newPrev, Guid newPrevGuid, byte? faceOverride, INPCCondition[] newConditions, NPCRewardsList newRewardsList) : base(newID, newConditions, newRewardsList)
		{
			this.pages = newPages;
			this.responses = newResponses;
			this.prev = newPrev;
			this.prevGuid = newPrevGuid;
			this.faceOverride = faceOverride;
		}

		/// <summary>
		/// Please refer to <see cref="M:SDG.Unturned.DialogueMessage.FindPrevDialogueAsset" />.
		/// </summary>
		// Token: 0x04000716 RID: 1814
		public Guid prevGuid;
	}
}
