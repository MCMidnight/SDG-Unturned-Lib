using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007CA RID: 1994
	public class PlayerNPCDialogueUI
	{
		/// <summary>
		/// If true, animation is finished and there is another page to show when Interact [F] is pressed.
		/// </summary>
		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x060043BA RID: 17338 RVA: 0x00181E3D File Offset: 0x0018003D
		// (set) Token: 0x060043BB RID: 17339 RVA: 0x00181E44 File Offset: 0x00180044
		public static bool CanAdvanceToNextPage { get; private set; }

		/// <summary>
		/// If true, text on current page is in the process of gradually appearing.
		/// </summary>
		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x060043BC RID: 17340 RVA: 0x00181E4C File Offset: 0x0018004C
		// (set) Token: 0x060043BD RID: 17341 RVA: 0x00181E53 File Offset: 0x00180053
		public static bool IsDialogueAnimating { get; private set; }

		/// <summary>
		/// Used by quest UI to return to current dialogue.
		/// </summary>
		// Token: 0x060043BE RID: 17342 RVA: 0x00181E5B File Offset: 0x0018005B
		public static void OpenCurrentDialogue()
		{
			PlayerNPCDialogueUI.open(PlayerNPCDialogueUI.dialogue, PlayerNPCDialogueUI.message, PlayerNPCDialogueUI.hasNextDialogue);
		}

		// Token: 0x060043BF RID: 17343 RVA: 0x00181E74 File Offset: 0x00180074
		public static void open(DialogueAsset newDialogue, DialogueMessage newMessage, bool newHasNextDialogue)
		{
			if (PlayerNPCDialogueUI.active)
			{
				PlayerNPCDialogueUI.updateDialogue(newDialogue, newMessage, newHasNextDialogue);
				return;
			}
			PlayerNPCDialogueUI.active = true;
			if (PlayerLifeUI.npc != null && PlayerLifeUI.npc.npcAsset != null)
			{
				PlayerNPCDialogueUI.characterLabel.Text = PlayerLifeUI.npc.npcAsset.GetNameShownToPlayer(Player.player);
			}
			else
			{
				PlayerNPCDialogueUI.characterLabel.Text = "null";
			}
			PlayerNPCDialogueUI.updateDialogue(newDialogue, newMessage, newHasNextDialogue);
			PlayerNPCDialogueUI.container.AnimateIntoView();
		}

		// Token: 0x060043C0 RID: 17344 RVA: 0x00181EF1 File Offset: 0x001800F1
		public static void close()
		{
			if (!PlayerNPCDialogueUI.active)
			{
				return;
			}
			PlayerNPCDialogueUI.active = false;
			PlayerNPCDialogueUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x060043C1 RID: 17345 RVA: 0x00181F18 File Offset: 0x00180118
		private static void AddDefaultGoodbyeResponse()
		{
			LevelInfo info = Level.info;
			Local local = (info != null) ? info.getLocalization() : null;
			string text;
			if (local != null && local.has("DefaultGoodbyeResponse"))
			{
				text = local.format("DefaultGoodbyeResponse");
			}
			else
			{
				text = PlayerNPCDialogueUI.localization.format("Goodbye");
			}
			if (!string.IsNullOrEmpty(text))
			{
				PlayerNPCDialogueUI.responses.Add(new DialogueResponse(0, null, 0, default(Guid), 0, default(Guid), 0, default(Guid), text, null, default(NPCRewardsList)));
			}
		}

		// Token: 0x060043C2 RID: 17346 RVA: 0x00181FA8 File Offset: 0x001801A8
		private static void updateDialogue(DialogueAsset newDialogue, DialogueMessage newMessage, bool newHasNextDialogue)
		{
			PlayerNPCDialogueUI.dialogue = newDialogue;
			PlayerNPCDialogueUI.message = newMessage;
			PlayerNPCDialogueUI.hasNextDialogue = newHasNextDialogue;
			if (PlayerNPCDialogueUI.dialogue == null)
			{
				return;
			}
			PlayerNPCDialogueUI.responseBox.IsVisible = false;
			PlayerNPCDialogueUI.responseBox.ContentSizeOffset = Vector2.zero;
			PlayerNPCDialogueUI.responses.Clear();
			PlayerNPCDialogueUI.dialogue.getAvailableResponses(Player.player, (int)newMessage.index, PlayerNPCDialogueUI.responses);
			if (PlayerLifeUI.npc != null)
			{
				PlayerLifeUI.npc.SetFaceOverride(PlayerNPCDialogueUI.message.faceOverride);
			}
			if (PlayerNPCDialogueUI.responses.Count == 0 && !PlayerNPCDialogueUI.hasNextDialogue)
			{
				PlayerNPCDialogueUI.AddDefaultGoodbyeResponse();
			}
			PlayerNPCDialogueUI.responseBox.RemoveAllChildren();
			PlayerNPCDialogueUI.responseButtons.Clear();
			for (int i = 0; i < PlayerNPCDialogueUI.responses.Count; i++)
			{
				DialogueResponse dialogueResponse = PlayerNPCDialogueUI.responses[i];
				string text = dialogueResponse.text;
				text = text.Replace("<name_npc>", (PlayerLifeUI.npc != null) ? PlayerLifeUI.npc.npcAsset.GetNameShownToPlayer(Player.player) : "null");
				text = text.Replace("<name_char>", Player.player.channel.owner.playerID.characterName);
				QuestAsset questAsset = dialogueResponse.FindQuestAsset();
				Texture2D newIcon = null;
				if (questAsset != null)
				{
					if (Player.player.quests.GetQuestStatus(questAsset) == ENPCQuestStatus.READY)
					{
						newIcon = PlayerNPCDialogueUI.icons.load<Texture2D>("Quest_End");
					}
					else
					{
						newIcon = PlayerNPCDialogueUI.icons.load<Texture2D>("Quest_Begin");
					}
				}
				else if (!dialogueResponse.IsVendorRefNull())
				{
					newIcon = PlayerNPCDialogueUI.icons.load<Texture2D>("Vendor");
				}
				SleekButtonIcon sleekButtonIcon = new SleekButtonIcon(newIcon);
				sleekButtonIcon.PositionOffset_Y = (float)(i * 30);
				sleekButtonIcon.SizeOffset_Y = 30f;
				sleekButtonIcon.SizeScale_X = 1f;
				sleekButtonIcon.textColor = 4;
				sleekButtonIcon.shadowStyle = 1;
				sleekButtonIcon.enableRichText = true;
				sleekButtonIcon.text = text;
				sleekButtonIcon.onClickedButton += new ClickedButton(PlayerNPCDialogueUI.onClickedResponseButton);
				PlayerNPCDialogueUI.responseBox.AddChild(sleekButtonIcon);
				sleekButtonIcon.IsVisible = false;
				PlayerNPCDialogueUI.responseButtons.Add(sleekButtonIcon);
			}
			PlayerNPCDialogueUI.dialoguePageIndex = 0;
			PlayerNPCDialogueUI.UpdatePage();
		}

		/// <summary>
		/// Update timers and UI for current page index.
		/// </summary>
		// Token: 0x060043C3 RID: 17347 RVA: 0x001821D0 File Offset: 0x001803D0
		private static void UpdatePage()
		{
			PlayerNPCDialogueUI.messageLabel.Text = string.Empty;
			PlayerNPCDialogueUI.pageLabel.IsVisible = false;
			PlayerNPCDialogueUI.pageAnimationTime = 0f;
			PlayerNPCDialogueUI.pauseTimer = 0f;
			PlayerNPCDialogueUI.animatedTextBuilder.Length = 0;
			PlayerNPCDialogueUI.animatedTextClosingRichTags = string.Empty;
			PlayerNPCDialogueUI.animatedCharsVisibleCount = 0;
			PlayerNPCDialogueUI.pageAnimationTimeVisibleCharsOffset = 0;
			PlayerNPCDialogueUI.responsesVisibleTime = 0f;
			PlayerNPCDialogueUI.visibleResponsesCount = 0;
			PlayerNPCDialogueUI.IsDialogueAnimating = true;
			PlayerNPCDialogueUI.CanAdvanceToNextPage = false;
			if (PlayerNPCDialogueUI.message != null && PlayerNPCDialogueUI.message.pages != null && PlayerNPCDialogueUI.dialoguePageIndex < PlayerNPCDialogueUI.message.pages.Length)
			{
				PlayerNPCDialogueUI.pageFormattedText = PlayerNPCDialogueUI.message.pages[PlayerNPCDialogueUI.dialoguePageIndex].text;
				PlayerNPCDialogueUI.pageFormattedText = PlayerNPCDialogueUI.pageFormattedText.Replace("<name_npc>", (PlayerLifeUI.npc != null) ? PlayerLifeUI.npc.npcAsset.GetNameShownToPlayer(Player.player) : "null");
				PlayerNPCDialogueUI.pageFormattedText = PlayerNPCDialogueUI.pageFormattedText.Replace("<name_char>", Player.player.channel.owner.playerID.characterName);
			}
			else
			{
				PlayerNPCDialogueUI.pageFormattedText = "?";
			}
			if (OptionsSettings.talk)
			{
				PlayerNPCDialogueUI.SkipAnimation();
			}
		}

		// Token: 0x060043C4 RID: 17348 RVA: 0x00182314 File Offset: 0x00180514
		private static bool DoNextCharsMatchKeyword(string text, int index, string keyword)
		{
			if (index + keyword.Length > text.Length)
			{
				return false;
			}
			for (int i = 0; i < keyword.Length; i++)
			{
				if (text.get_Chars(index + i) != keyword.get_Chars(i))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060043C5 RID: 17349 RVA: 0x0018235C File Offset: 0x0018055C
		private static bool FindNextRichTextMarkupSpan(string text, int index, out int begin, out int end)
		{
			begin = -1;
			end = -1;
			while (index < text.Length)
			{
				if (text.get_Chars(index) == '<')
				{
					if (begin == -1)
					{
						begin = index;
					}
				}
				else if (text.get_Chars(index) == '>' && (index == text.Length - 1 || text.get_Chars(index + 1) != '<'))
				{
					end = index;
					return begin >= 0;
				}
				index++;
			}
			return false;
		}

		/// <summary>
		/// Called when the player presses Interact [F] in dialogue screen.
		/// </summary>
		// Token: 0x060043C6 RID: 17350 RVA: 0x001823C4 File Offset: 0x001805C4
		public static void AdvancePage()
		{
			if (PlayerNPCDialogueUI.dialoguePageIndex == PlayerNPCDialogueUI.message.pages.Length - 1)
			{
				Player.player.quests.ClientChooseNextDialogue(PlayerNPCDialogueUI.dialogue.GUID, PlayerNPCDialogueUI.message.index);
				return;
			}
			PlayerNPCDialogueUI.dialoguePageIndex++;
			PlayerNPCDialogueUI.UpdatePage();
		}

		// Token: 0x060043C7 RID: 17351 RVA: 0x0018241C File Offset: 0x0018061C
		private static void OnPageAnimationFinished()
		{
			PlayerNPCDialogueUI.IsDialogueAnimating = false;
			if (PlayerNPCDialogueUI.message == null || PlayerNPCDialogueUI.message.pages == null)
			{
				PlayerNPCDialogueUI.responseBox.IsVisible = true;
				return;
			}
			if (PlayerNPCDialogueUI.dialoguePageIndex < PlayerNPCDialogueUI.message.pages.Length - 1)
			{
				PlayerNPCDialogueUI.CanAdvanceToNextPage = true;
				PlayerNPCDialogueUI.pageLabel.Text = PlayerNPCDialogueUI.localization.format("Page", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact));
				PlayerNPCDialogueUI.pageLabel.IsVisible = true;
				return;
			}
			if (PlayerNPCDialogueUI.dialoguePageIndex == PlayerNPCDialogueUI.message.pages.Length - 1 && PlayerNPCDialogueUI.hasNextDialogue)
			{
				PlayerNPCDialogueUI.CanAdvanceToNextPage = true;
				PlayerNPCDialogueUI.pageLabel.Text = PlayerNPCDialogueUI.localization.format("Page", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact));
				PlayerNPCDialogueUI.pageLabel.IsVisible = true;
				PlayerNPCDialogueUI.responseBox.IsVisible = true;
				return;
			}
			PlayerNPCDialogueUI.responseBox.IsVisible = true;
		}

		/// <summary>
		/// Show complete text for the current page and make responses visible.
		/// Called if dialogue animation is disabled, and when the player presses Interact [F] during animation.
		/// </summary>
		// Token: 0x060043C8 RID: 17352 RVA: 0x00182508 File Offset: 0x00180708
		public static void SkipAnimation()
		{
			PlayerNPCDialogueUI.messageLabel.Text = PlayerNPCDialogueUI.pageFormattedText.Replace("<pause>", "");
			PlayerNPCDialogueUI.visibleResponsesCount = PlayerNPCDialogueUI.responses.Count;
			for (int i = 0; i < PlayerNPCDialogueUI.responses.Count; i++)
			{
				PlayerNPCDialogueUI.responseButtons[i].IsVisible = true;
			}
			PlayerNPCDialogueUI.responseBox.ContentSizeOffset = new Vector2(0f, (float)(PlayerNPCDialogueUI.responses.Count * 30));
			PlayerNPCDialogueUI.OnPageAnimationFinished();
		}

		// Token: 0x060043C9 RID: 17353 RVA: 0x00182590 File Offset: 0x00180790
		public static void UpdateAnimation()
		{
			if (PlayerNPCDialogueUI.dialogue == null)
			{
				return;
			}
			if (PlayerNPCDialogueUI.IsDialogueAnimating)
			{
				if (PlayerNPCDialogueUI.pauseTimer > 0f)
				{
					PlayerNPCDialogueUI.pauseTimer -= Time.deltaTime;
				}
				else
				{
					PlayerNPCDialogueUI.pageAnimationTime += Time.deltaTime;
				}
				int num = Mathf.Min(PlayerNPCDialogueUI.pageFormattedText.Length, Mathf.CeilToInt(PlayerNPCDialogueUI.pageAnimationTime * 30f) + PlayerNPCDialogueUI.pageAnimationTimeVisibleCharsOffset);
				if (PlayerNPCDialogueUI.animatedCharsVisibleCount != num)
				{
					while (PlayerNPCDialogueUI.animatedCharsVisibleCount < PlayerNPCDialogueUI.pageFormattedText.Length && PlayerNPCDialogueUI.animatedCharsVisibleCount < num)
					{
						char c = PlayerNPCDialogueUI.pageFormattedText.get_Chars(PlayerNPCDialogueUI.animatedCharsVisibleCount);
						if (c == '<')
						{
							int num2;
							int num3;
							if (PlayerNPCDialogueUI.animatedTextClosingRichTags.Length > 0)
							{
								num += PlayerNPCDialogueUI.animatedTextClosingRichTags.Length;
								PlayerNPCDialogueUI.animatedCharsVisibleCount += PlayerNPCDialogueUI.animatedTextClosingRichTags.Length;
								PlayerNPCDialogueUI.pageAnimationTimeVisibleCharsOffset += PlayerNPCDialogueUI.animatedTextClosingRichTags.Length;
								PlayerNPCDialogueUI.animatedTextBuilder.Append(PlayerNPCDialogueUI.animatedTextClosingRichTags);
								PlayerNPCDialogueUI.animatedTextClosingRichTags = string.Empty;
							}
							else if (PlayerNPCDialogueUI.DoNextCharsMatchKeyword(PlayerNPCDialogueUI.pageFormattedText, PlayerNPCDialogueUI.animatedCharsVisibleCount, "<pause>"))
							{
								PlayerNPCDialogueUI.pauseTimer += 0.5f;
								num = PlayerNPCDialogueUI.animatedCharsVisibleCount + "<pause>".Length;
								PlayerNPCDialogueUI.animatedCharsVisibleCount = num;
								PlayerNPCDialogueUI.pageAnimationTimeVisibleCharsOffset += "<pause>".Length - 1;
							}
							else if (PlayerNPCDialogueUI.FindNextRichTextMarkupSpan(PlayerNPCDialogueUI.pageFormattedText, PlayerNPCDialogueUI.animatedCharsVisibleCount, out num2, out num3))
							{
								int num4 = num3 - num2 + 1;
								num += num4;
								PlayerNPCDialogueUI.animatedCharsVisibleCount += num4;
								PlayerNPCDialogueUI.pageAnimationTimeVisibleCharsOffset += num4;
								PlayerNPCDialogueUI.animatedTextBuilder.Append(PlayerNPCDialogueUI.pageFormattedText.Substring(num2, num4));
								if (PlayerNPCDialogueUI.FindNextRichTextMarkupSpan(PlayerNPCDialogueUI.pageFormattedText, num3 + 1, out num2, out num3))
								{
									num4 = num3 - num2 + 1;
									PlayerNPCDialogueUI.animatedTextClosingRichTags = PlayerNPCDialogueUI.pageFormattedText.Substring(num2, num4);
								}
							}
							else
							{
								PlayerNPCDialogueUI.animatedTextBuilder.Append(c);
								PlayerNPCDialogueUI.animatedCharsVisibleCount++;
							}
						}
						else
						{
							PlayerNPCDialogueUI.animatedTextBuilder.Append(c);
							PlayerNPCDialogueUI.animatedCharsVisibleCount++;
						}
					}
					PlayerNPCDialogueUI.messageLabel.Text = PlayerNPCDialogueUI.animatedTextBuilder.ToString() + PlayerNPCDialogueUI.animatedTextClosingRichTags;
					if (PlayerNPCDialogueUI.animatedCharsVisibleCount == PlayerNPCDialogueUI.pageFormattedText.Length)
					{
						PlayerNPCDialogueUI.OnPageAnimationFinished();
						return;
					}
				}
			}
			else
			{
				PlayerNPCDialogueUI.responsesVisibleTime += Time.deltaTime;
				int num5 = Mathf.Min(PlayerNPCDialogueUI.responses.Count, Mathf.FloorToInt(PlayerNPCDialogueUI.responsesVisibleTime * 10f));
				if (PlayerNPCDialogueUI.visibleResponsesCount != num5)
				{
					while (PlayerNPCDialogueUI.visibleResponsesCount < num5)
					{
						PlayerNPCDialogueUI.responseButtons[PlayerNPCDialogueUI.visibleResponsesCount].IsVisible = true;
						PlayerNPCDialogueUI.responseBox.ContentSizeOffset = new Vector2(0f, (float)(num5 * 30));
						PlayerNPCDialogueUI.visibleResponsesCount++;
					}
				}
			}
		}

		// Token: 0x060043CA RID: 17354 RVA: 0x00182870 File Offset: 0x00180A70
		private static void onClickedResponseButton(ISleekElement button)
		{
			PlayerNPCDialogueUI.SetResponseButtonsAreClickable(false);
			int num = PlayerNPCDialogueUI.responseBox.FindIndexOfChild(button);
			DialogueResponse dialogueResponse = PlayerNPCDialogueUI.responses[num];
			QuestAsset questAsset = dialogueResponse.FindQuestAsset();
			if (questAsset != null)
			{
				PlayerNPCDialogueUI.close();
				PlayerNPCQuestUI.open(questAsset, PlayerNPCDialogueUI.dialogue, dialogueResponse, (Player.player.quests.GetQuestStatus(questAsset) == ENPCQuestStatus.READY) ? EQuestViewMode.END : EQuestViewMode.BEGIN);
				return;
			}
			bool flag = dialogueResponse.FindDialogueAsset() != null;
			VendorAsset vendorAsset = dialogueResponse.FindVendorAsset();
			if (!flag && vendorAsset == null)
			{
				PlayerNPCDialogueUI.close();
				PlayerLifeUI.open();
			}
			Player.player.quests.ClientChooseDialogueResponse(PlayerNPCDialogueUI.dialogue.GUID, dialogueResponse.index);
		}

		// Token: 0x060043CB RID: 17355 RVA: 0x0018290C File Offset: 0x00180B0C
		private static void SetResponseButtonsAreClickable(bool clickable)
		{
			foreach (SleekButtonIcon sleekButtonIcon in PlayerNPCDialogueUI.responseButtons)
			{
				sleekButtonIcon.isClickable = clickable;
			}
		}

		// Token: 0x060043CC RID: 17356 RVA: 0x0018295C File Offset: 0x00180B5C
		public PlayerNPCDialogueUI()
		{
			if (PlayerNPCDialogueUI.icons != null)
			{
				PlayerNPCDialogueUI.icons.unload();
			}
			PlayerNPCDialogueUI.localization = Localization.read("/Player/PlayerNPCDialogue.dat");
			PlayerNPCDialogueUI.icons = Bundles.getBundle("/Bundles/Textures/Player/Icons/PlayerNPCDialogue/PlayerNPCDialogue.unity3d");
			PlayerNPCDialogueUI.container = new SleekFullscreenBox();
			PlayerNPCDialogueUI.container.PositionScale_Y = 1f;
			PlayerNPCDialogueUI.container.PositionOffset_X = 10f;
			PlayerNPCDialogueUI.container.PositionOffset_Y = 10f;
			PlayerNPCDialogueUI.container.SizeOffset_X = -20f;
			PlayerNPCDialogueUI.container.SizeOffset_Y = -20f;
			PlayerNPCDialogueUI.container.SizeScale_X = 1f;
			PlayerNPCDialogueUI.container.SizeScale_Y = 1f;
			PlayerUI.container.AddChild(PlayerNPCDialogueUI.container);
			PlayerNPCDialogueUI.active = false;
			PlayerNPCDialogueUI.dialogueBox = Glazier.Get().CreateBox();
			PlayerNPCDialogueUI.dialogueBox.PositionOffset_X = -250f;
			PlayerNPCDialogueUI.dialogueBox.PositionOffset_Y = -200f;
			PlayerNPCDialogueUI.dialogueBox.PositionScale_X = 0.5f;
			PlayerNPCDialogueUI.dialogueBox.PositionScale_Y = 0.85f;
			PlayerNPCDialogueUI.dialogueBox.SizeOffset_X = 500f;
			PlayerNPCDialogueUI.dialogueBox.SizeOffset_Y = 100f;
			PlayerNPCDialogueUI.container.AddChild(PlayerNPCDialogueUI.dialogueBox);
			PlayerNPCDialogueUI.characterLabel = Glazier.Get().CreateLabel();
			PlayerNPCDialogueUI.characterLabel.PositionOffset_X = 5f;
			PlayerNPCDialogueUI.characterLabel.PositionOffset_Y = 5f;
			PlayerNPCDialogueUI.characterLabel.SizeOffset_X = -10f;
			PlayerNPCDialogueUI.characterLabel.SizeOffset_Y = 30f;
			PlayerNPCDialogueUI.characterLabel.SizeScale_X = 1f;
			PlayerNPCDialogueUI.characterLabel.TextAlignment = 0;
			PlayerNPCDialogueUI.characterLabel.TextColor = 4;
			PlayerNPCDialogueUI.characterLabel.TextContrastContext = 1;
			PlayerNPCDialogueUI.characterLabel.AllowRichText = true;
			PlayerNPCDialogueUI.characterLabel.FontSize = 3;
			PlayerNPCDialogueUI.dialogueBox.AddChild(PlayerNPCDialogueUI.characterLabel);
			PlayerNPCDialogueUI.messageLabel = Glazier.Get().CreateLabel();
			PlayerNPCDialogueUI.messageLabel.PositionOffset_X = 5f;
			PlayerNPCDialogueUI.messageLabel.PositionOffset_Y = 30f;
			PlayerNPCDialogueUI.messageLabel.SizeOffset_X = -10f;
			PlayerNPCDialogueUI.messageLabel.SizeOffset_Y = -35f;
			PlayerNPCDialogueUI.messageLabel.SizeScale_X = 1f;
			PlayerNPCDialogueUI.messageLabel.SizeScale_Y = 1f;
			PlayerNPCDialogueUI.messageLabel.TextAlignment = 0;
			PlayerNPCDialogueUI.messageLabel.TextColor = 4;
			PlayerNPCDialogueUI.messageLabel.TextContrastContext = 1;
			PlayerNPCDialogueUI.messageLabel.AllowRichText = true;
			PlayerNPCDialogueUI.dialogueBox.AddChild(PlayerNPCDialogueUI.messageLabel);
			PlayerNPCDialogueUI.pageLabel = Glazier.Get().CreateLabel();
			PlayerNPCDialogueUI.pageLabel.PositionOffset_X = -30f;
			PlayerNPCDialogueUI.pageLabel.PositionOffset_Y = -30f;
			PlayerNPCDialogueUI.pageLabel.PositionScale_X = 1f;
			PlayerNPCDialogueUI.pageLabel.PositionScale_Y = 1f;
			PlayerNPCDialogueUI.pageLabel.SizeOffset_X = 30f;
			PlayerNPCDialogueUI.pageLabel.SizeOffset_Y = 30f;
			PlayerNPCDialogueUI.pageLabel.TextAlignment = 8;
			PlayerNPCDialogueUI.dialogueBox.AddChild(PlayerNPCDialogueUI.pageLabel);
			PlayerNPCDialogueUI.responseBox = Glazier.Get().CreateScrollView();
			PlayerNPCDialogueUI.responseBox.PositionOffset_X = -250f;
			PlayerNPCDialogueUI.responseBox.PositionOffset_Y = -100f;
			PlayerNPCDialogueUI.responseBox.PositionScale_X = 0.5f;
			PlayerNPCDialogueUI.responseBox.PositionScale_Y = 0.85f;
			PlayerNPCDialogueUI.responseBox.SizeOffset_X = 500f;
			PlayerNPCDialogueUI.responseBox.SizeScale_Y = 0.15f;
			PlayerNPCDialogueUI.responseBox.ScaleContentToWidth = true;
			PlayerNPCDialogueUI.container.AddChild(PlayerNPCDialogueUI.responseBox);
			PlayerNPCDialogueUI.responseBox.IsVisible = false;
			PlayerNPCDialogueUI.responseButtons.Clear();
		}

		// Token: 0x04002D0A RID: 11530
		private const string KEYWORD_PAUSE = "<pause>";

		// Token: 0x04002D0B RID: 11531
		private static SleekFullscreenBox container;

		// Token: 0x04002D0C RID: 11532
		private static Local localization;

		// Token: 0x04002D0D RID: 11533
		public static Bundle icons;

		// Token: 0x04002D0E RID: 11534
		public static bool active;

		// Token: 0x04002D0F RID: 11535
		private static DialogueAsset dialogue;

		// Token: 0x04002D10 RID: 11536
		private static DialogueMessage message;

		/// <summary>
		/// If true, the player can press Interact [F] when there are no responses
		/// and the "next" dialogue will be opened.
		/// </summary>
		// Token: 0x04002D11 RID: 11537
		private static bool hasNextDialogue;

		// Token: 0x04002D12 RID: 11538
		private static List<DialogueResponse> responses = new List<DialogueResponse>();

		// Token: 0x04002D13 RID: 11539
		private static ISleekBox dialogueBox;

		// Token: 0x04002D14 RID: 11540
		private static ISleekLabel characterLabel;

		// Token: 0x04002D15 RID: 11541
		private static ISleekLabel messageLabel;

		// Token: 0x04002D16 RID: 11542
		private static ISleekLabel pageLabel;

		// Token: 0x04002D17 RID: 11543
		private static ISleekScrollView responseBox;

		// Token: 0x04002D18 RID: 11544
		private static List<SleekButtonIcon> responseButtons = new List<SleekButtonIcon>();

		/// <summary>
		/// Each dialogue message is separated into multiple pages.
		/// </summary>
		// Token: 0x04002D19 RID: 11545
		private static int dialoguePageIndex;

		/// <summary>
		/// Current page localized text with name_npc and name_char formatted in.
		/// </summary>
		// Token: 0x04002D1A RID: 11546
		private static string pageFormattedText;

		/// <summary>
		/// Seconds elapsed while viewing current page not including pause timer.
		/// Used to gradually show the message text.
		/// </summary>
		// Token: 0x04002D1B RID: 11547
		private static float pageAnimationTime;

		/// <summary>
		/// Seconds to wait before resuming pageAnimationTime counting.
		/// </summary>
		// Token: 0x04002D1C RID: 11548
		private static float pauseTimer;

		/// <summary>
		/// Appends chars from pageFormattedText according to pageAnimationTime.
		/// </summary>
		// Token: 0x04002D1D RID: 11549
		private static StringBuilder animatedTextBuilder = new StringBuilder();

		/// <summary>
		/// Rich text formatting tags to close those opened by visible text in animatedTextBuilder.
		/// For example, if animatedTextBuilder includes an opening color=#, this includes the closing color markup.
		/// Required depending on Glazier used.
		/// </summary>
		// Token: 0x04002D1E RID: 11550
		private static string animatedTextClosingRichTags;

		/// <summary>
		/// Number of chars of pageFormattedText currently visible.
		/// </summary>
		// Token: 0x04002D1F RID: 11551
		private static int animatedCharsVisibleCount;

		/// <summary>
		/// Added to animation visible chars to skip time on markup.
		/// </summary>
		// Token: 0x04002D20 RID: 11552
		private static int pageAnimationTimeVisibleCharsOffset;

		/// <summary>
		/// Seconds elapsed since responses started becoming visible.
		/// Used to gradually enable responses rather than all at once.
		/// </summary>
		// Token: 0x04002D21 RID: 11553
		private static float responsesVisibleTime;

		/// <summary>
		/// Animated toward total number of responses to make them gradually visible.
		/// </summary>
		// Token: 0x04002D22 RID: 11554
		private static int visibleResponsesCount;
	}
}
