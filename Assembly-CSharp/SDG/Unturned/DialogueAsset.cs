using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x020002A7 RID: 679
	public class DialogueAsset : Asset
	{
		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06001473 RID: 5235 RVA: 0x0004C3C2 File Offset: 0x0004A5C2
		// (set) Token: 0x06001474 RID: 5236 RVA: 0x0004C3CA File Offset: 0x0004A5CA
		public DialogueMessage[] messages { get; protected set; }

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06001475 RID: 5237 RVA: 0x0004C3D3 File Offset: 0x0004A5D3
		// (set) Token: 0x06001476 RID: 5238 RVA: 0x0004C3DB File Offset: 0x0004A5DB
		public DialogueResponse[] responses { get; protected set; }

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06001477 RID: 5239 RVA: 0x0004C3E4 File Offset: 0x0004A5E4
		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.NPC;
			}
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x0004C3E8 File Offset: 0x0004A5E8
		public DialogueMessage GetAvailableMessage(Player player)
		{
			for (int i = 0; i < this.messages.Length; i++)
			{
				DialogueMessage dialogueMessage = this.messages[i];
				if (dialogueMessage.areConditionsMet(player))
				{
					return dialogueMessage;
				}
			}
			return null;
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x0004C420 File Offset: 0x0004A620
		internal void GetAllResponsesForMessage(int messageIndex, List<DialogueResponse> messageResponses)
		{
			DialogueMessage dialogueMessage = this.messages[messageIndex];
			if (dialogueMessage.responses != null && dialogueMessage.responses.Length != 0)
			{
				for (int i = 0; i < dialogueMessage.responses.Length; i++)
				{
					DialogueResponse dialogueResponse = this.responses[(int)dialogueMessage.responses[i]];
					messageResponses.Add(dialogueResponse);
				}
				return;
			}
			int j = 0;
			while (j < this.responses.Length)
			{
				DialogueResponse dialogueResponse2 = this.responses[j];
				if (dialogueResponse2.messages == null || dialogueResponse2.messages.Length == 0)
				{
					goto IL_97;
				}
				bool flag = false;
				for (int k = 0; k < dialogueResponse2.messages.Length; k++)
				{
					if ((int)dialogueResponse2.messages[k] == messageIndex)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					goto IL_97;
				}
				IL_9F:
				j++;
				continue;
				IL_97:
				messageResponses.Add(dialogueResponse2);
				goto IL_9F;
			}
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x0004C4DC File Offset: 0x0004A6DC
		public void getAvailableResponses(Player player, int messageIndex, List<DialogueResponse> availableResponses)
		{
			DialogueMessage dialogueMessage = this.messages[messageIndex];
			if (dialogueMessage.responses != null && dialogueMessage.responses.Length != 0)
			{
				for (int i = 0; i < dialogueMessage.responses.Length; i++)
				{
					DialogueResponse dialogueResponse = this.responses[(int)dialogueMessage.responses[i]];
					if (dialogueResponse.areConditionsMet(player))
					{
						availableResponses.Add(dialogueResponse);
					}
				}
				return;
			}
			int j = 0;
			while (j < this.responses.Length)
			{
				DialogueResponse dialogueResponse2 = this.responses[j];
				if (dialogueResponse2.messages == null || dialogueResponse2.messages.Length == 0)
				{
					goto IL_A0;
				}
				bool flag = false;
				for (int k = 0; k < dialogueResponse2.messages.Length; k++)
				{
					if ((int)dialogueResponse2.messages[k] == messageIndex)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					goto IL_A0;
				}
				IL_B2:
				j++;
				continue;
				IL_A0:
				if (dialogueResponse2.areConditionsMet(player))
				{
					availableResponses.Add(dialogueResponse2);
					goto IL_B2;
				}
				goto IL_B2;
			}
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x0004C5AC File Offset: 0x0004A7AC
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			if (this.id < 2000 && !base.OriginAllowsVanillaLegacyId && !data.ContainsKey("Bypass_ID_Limit"))
			{
				throw new NotSupportedException("ID < 2000");
			}
			int num = data.ParseInt32("Messages", 0);
			int num2 = (int)data.ParseUInt8("Responses", 0);
			this.messages = new DialogueMessage[num];
			byte b = 0;
			while ((int)b < this.messages.Length)
			{
				DialoguePage[] array = new DialoguePage[(int)data.ParseUInt8("Message_" + b.ToString() + "_Pages", 0)];
				byte b2 = 0;
				while ((int)b2 < array.Length)
				{
					string text = localization.format("Message_" + b.ToString() + "_Page_" + b2.ToString());
					text = ItemTool.filterRarityRichText(text);
					RichTextUtil.replaceNewlineMarkup(ref text);
					if (string.IsNullOrEmpty(text))
					{
						throw new NotSupportedException("missing message " + b.ToString() + " page " + b2.ToString());
					}
					array[(int)b2] = new DialoguePage(text);
					b2 += 1;
				}
				byte[] array2 = new byte[(int)data.ParseUInt8("Message_" + b.ToString() + "_Responses", 0)];
				byte b3 = 0;
				while ((int)b3 < array2.Length)
				{
					string text2 = "Message_" + b.ToString() + "_Response_" + b3.ToString();
					array2[(int)b3] = data.ParseUInt8(text2, 0);
					if ((int)array2[(int)b3] >= num2)
					{
						Assets.reportError(this, "{0} out of bounds ({1})", text2, num2);
					}
					b3 += 1;
				}
				Guid newPrevGuid;
				ushort newPrev = data.ParseGuidOrLegacyId("Message_" + b.ToString() + "_Prev", out newPrevGuid);
				byte? faceOverride;
				if (data.ContainsKey("Message_" + b.ToString() + "_FaceOverride"))
				{
					faceOverride = new byte?(data.ParseUInt8("Message_" + b.ToString() + "_FaceOverride", 0));
				}
				else
				{
					faceOverride = default(byte?);
				}
				INPCCondition[] array3 = new INPCCondition[(int)data.ParseUInt8("Message_" + b.ToString() + "_Conditions", 0)];
				NPCTool.readConditions(data, localization, "Message_" + b.ToString() + "_Condition_", array3, this);
				NPCRewardsList newRewardsList = default(NPCRewardsList);
				newRewardsList.Parse(data, localization, this, "Message_" + b.ToString() + "_Rewards", "Message_" + b.ToString() + "_Reward_");
				this.messages[(int)b] = new DialogueMessage(b, array, array2, newPrev, newPrevGuid, faceOverride, array3, newRewardsList);
				b += 1;
			}
			this.responses = new DialogueResponse[num2];
			byte b4 = 0;
			while ((int)b4 < this.responses.Length)
			{
				byte[] array4 = new byte[(int)data.ParseUInt8("Response_" + b4.ToString() + "_Messages", 0)];
				byte b5 = 0;
				while ((int)b5 < array4.Length)
				{
					string text3 = "Response_" + b4.ToString() + "_Message_" + b5.ToString();
					array4[(int)b5] = data.ParseUInt8(text3, 0);
					if ((int)array4[(int)b5] >= num)
					{
						Assets.reportError(this, "{0} out of bounds ({1})", text3, num);
					}
					b5 += 1;
				}
				Guid newDialogueGuid;
				ushort newDialogue = data.ParseGuidOrLegacyId("Response_" + b4.ToString() + "_Dialogue", out newDialogueGuid);
				Guid newQuestGuid;
				ushort newQuest = data.ParseGuidOrLegacyId("Response_" + b4.ToString() + "_Quest", out newQuestGuid);
				Guid newVendorGuid;
				ushort newVendor = data.ParseGuidOrLegacyId("Response_" + b4.ToString() + "_Vendor", out newVendorGuid);
				string text4 = localization.format("Response_" + b4.ToString());
				text4 = ItemTool.filterRarityRichText(text4);
				RichTextUtil.replaceNewlineMarkup(ref text4);
				if (string.IsNullOrEmpty(text4))
				{
					throw new NotSupportedException("missing response " + b4.ToString());
				}
				INPCCondition[] array5 = new INPCCondition[(int)data.ParseUInt8("Response_" + b4.ToString() + "_Conditions", 0)];
				NPCTool.readConditions(data, localization, "Response_" + b4.ToString() + "_Condition_", array5, this);
				NPCRewardsList newRewardsList2 = default(NPCRewardsList);
				newRewardsList2.Parse(data, localization, this, "Response_" + b4.ToString() + "_Rewards", "Response_" + b4.ToString() + "_Reward_");
				this.responses[(int)b4] = new DialogueResponse(b4, array4, newDialogue, newDialogueGuid, newQuest, newQuestGuid, newVendor, newVendorGuid, text4, array5, newRewardsList2);
				b4 += 1;
			}
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x0004CA54 File Offset: 0x0004AC54
		[Obsolete("Please use GetAvailableMessage which returns the DialogueMessage rather than index")]
		public int getAvailableMessage(Player player)
		{
			DialogueMessage availableMessage = this.GetAvailableMessage(player);
			if (availableMessage == null)
			{
				return -1;
			}
			return (int)availableMessage.index;
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x0004CA74 File Offset: 0x0004AC74
		[Obsolete("Server now tracks dialogue tree")]
		public bool doesPlayerHaveAccessToVendor(Player player, VendorAsset vendorAsset)
		{
			return true;
		}
	}
}
