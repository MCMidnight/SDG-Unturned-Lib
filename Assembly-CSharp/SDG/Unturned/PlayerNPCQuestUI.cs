using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007CC RID: 1996
	public class PlayerNPCQuestUI
	{
		// Token: 0x060043CE RID: 17358 RVA: 0x00182D27 File Offset: 0x00180F27
		public static void open(QuestAsset newQuest, DialogueAsset newDialogueContext, DialogueResponse newPendingResponse, EQuestViewMode newMode)
		{
			if (PlayerNPCQuestUI.active)
			{
				return;
			}
			PlayerNPCQuestUI.active = true;
			PlayerNPCQuestUI.updateQuest(newQuest, newDialogueContext, newPendingResponse, newMode);
			PlayerNPCQuestUI.container.AnimateIntoView();
		}

		// Token: 0x060043CF RID: 17359 RVA: 0x00182D4A File Offset: 0x00180F4A
		public static void close()
		{
			if (!PlayerNPCQuestUI.active)
			{
				return;
			}
			PlayerNPCQuestUI.active = false;
			PlayerNPCQuestUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x060043D0 RID: 17360 RVA: 0x00182D70 File Offset: 0x00180F70
		public static void closeNicely()
		{
			PlayerNPCQuestUI.close();
			if (PlayerNPCQuestUI.mode == EQuestViewMode.BEGIN || PlayerNPCQuestUI.mode == EQuestViewMode.END)
			{
				PlayerNPCDialogueUI.OpenCurrentDialogue();
				return;
			}
			if (PlayerNPCQuestUI.mode == EQuestViewMode.DETAILS)
			{
				PlayerDashboardInventoryUI.active = false;
				PlayerDashboardCraftingUI.active = false;
				PlayerDashboardSkillsUI.active = false;
				PlayerDashboardInformationUI.active = true;
				PlayerDashboardUI.open();
			}
		}

		// Token: 0x060043D1 RID: 17361 RVA: 0x00182DBC File Offset: 0x00180FBC
		private static void updateQuest(QuestAsset newQuest, DialogueAsset newDialogueContext, DialogueResponse newPendingResponse, EQuestViewMode newMode)
		{
			PlayerNPCQuestUI.quest = newQuest;
			PlayerNPCQuestUI.pendingResponse = newPendingResponse;
			PlayerNPCQuestUI.dialogueContext = newDialogueContext;
			PlayerNPCQuestUI.mode = newMode;
			if (PlayerNPCQuestUI.quest == null)
			{
				return;
			}
			PlayerNPCQuestUI.beginContainer.IsVisible = (PlayerNPCQuestUI.mode == EQuestViewMode.BEGIN);
			PlayerNPCQuestUI.endContainer.IsVisible = (PlayerNPCQuestUI.mode == EQuestViewMode.END);
			PlayerNPCQuestUI.detailsContainer.IsVisible = (PlayerNPCQuestUI.mode == EQuestViewMode.DETAILS);
			PlayerNPCQuestUI.SetButtonsAreClickable(true);
			if (PlayerNPCQuestUI.mode == EQuestViewMode.DETAILS)
			{
				if (Player.player.quests.GetTrackedQuest() == PlayerNPCQuestUI.quest)
				{
					PlayerNPCQuestUI.trackButton.Text = PlayerNPCQuestUI.localization.format("Track_Off");
				}
				else
				{
					PlayerNPCQuestUI.trackButton.Text = PlayerNPCQuestUI.localization.format("Track_On");
				}
			}
			PlayerNPCQuestUI.nameLabel.Text = PlayerNPCQuestUI.quest.questName;
			PlayerNPCQuestUI.descriptionLabel.Text = PlayerNPCQuestUI.quest.questDescription;
			float num = 0f;
			if (PlayerNPCQuestUI.quest.conditions != null && PlayerNPCQuestUI.quest.conditions.Length != 0)
			{
				PlayerNPCQuestUI.conditionsLabel.IsVisible = true;
				PlayerNPCQuestUI.conditionsContainer.IsVisible = true;
				PlayerNPCQuestUI.conditionsContainer.RemoveAllChildren();
				float num2 = 0f;
				PlayerNPCQuestUI.areConditionsMet.Clear();
				foreach (INPCCondition inpccondition in PlayerNPCQuestUI.quest.conditions)
				{
					PlayerNPCQuestUI.areConditionsMet.Add(inpccondition.isConditionMet(Player.player));
				}
				for (int j = 0; j < PlayerNPCQuestUI.quest.conditions.Length; j++)
				{
					INPCCondition inpccondition2 = PlayerNPCQuestUI.quest.conditions[j];
					if (inpccondition2.AreUIRequirementsMet(PlayerNPCQuestUI.areConditionsMet))
					{
						bool flag = PlayerNPCQuestUI.areConditionsMet[j];
						Texture2D icon = null;
						if (PlayerNPCQuestUI.mode != EQuestViewMode.BEGIN)
						{
							if (flag)
							{
								icon = PlayerNPCQuestUI.icons.load<Texture2D>("Complete");
							}
							else
							{
								icon = PlayerNPCQuestUI.icons.load<Texture2D>("Incomplete");
							}
						}
						ISleekElement sleekElement = inpccondition2.createUI(Player.player, icon);
						if (sleekElement != null)
						{
							sleekElement.PositionOffset_Y = num2;
							PlayerNPCQuestUI.conditionsContainer.AddChild(sleekElement);
							num2 += sleekElement.SizeOffset_Y;
						}
					}
				}
				PlayerNPCQuestUI.conditionsContainer.SizeOffset_Y = num2;
				num += PlayerNPCQuestUI.conditionsLabel.SizeOffset_Y;
				num += PlayerNPCQuestUI.conditionsContainer.SizeOffset_Y;
			}
			else
			{
				PlayerNPCQuestUI.conditionsLabel.IsVisible = false;
				PlayerNPCQuestUI.conditionsContainer.IsVisible = false;
			}
			if (PlayerNPCQuestUI.quest.rewards != null && PlayerNPCQuestUI.quest.rewards.Length != 0)
			{
				PlayerNPCQuestUI.rewardsLabel.IsVisible = true;
				PlayerNPCQuestUI.rewardsContainer.IsVisible = true;
				PlayerNPCQuestUI.rewardsContainer.RemoveAllChildren();
				float num3 = 0f;
				for (int k = 0; k < PlayerNPCQuestUI.quest.rewards.Length; k++)
				{
					ISleekElement sleekElement2 = PlayerNPCQuestUI.quest.rewards[k].createUI(Player.player);
					if (sleekElement2 != null)
					{
						sleekElement2.PositionOffset_Y = num3;
						PlayerNPCQuestUI.rewardsContainer.AddChild(sleekElement2);
						num3 += sleekElement2.SizeOffset_Y;
					}
				}
				PlayerNPCQuestUI.rewardsLabel.PositionOffset_Y = num;
				num += PlayerNPCQuestUI.rewardsLabel.SizeOffset_Y;
				PlayerNPCQuestUI.rewardsContainer.PositionOffset_Y = num;
				PlayerNPCQuestUI.rewardsContainer.SizeOffset_Y = num3;
				num += PlayerNPCQuestUI.rewardsContainer.SizeOffset_Y;
			}
			else
			{
				PlayerNPCQuestUI.rewardsLabel.IsVisible = false;
				PlayerNPCQuestUI.rewardsContainer.IsVisible = false;
			}
			PlayerNPCQuestUI.conditionsAndRewardsScrollView.ContentSizeOffset = new Vector2(0f, num);
			float num4 = (float)Screen.height / GraphicsSettings.userInterfaceScale;
			num4 -= 10f;
			num4 -= 10f;
			num4 -= 50f;
			num4 -= 10f;
			float num5 = PlayerNPCQuestUI.conditionsAndRewardsScrollView.PositionOffset_Y + num + 5f;
			if (num5 >= num4)
			{
				PlayerNPCQuestUI.questBox.PositionOffset_Y = 0f;
				PlayerNPCQuestUI.questBox.PositionScale_Y = 0f;
				PlayerNPCQuestUI.questBox.SizeOffset_Y = -60f;
				PlayerNPCQuestUI.questBox.SizeScale_Y = 1f;
				return;
			}
			PlayerNPCQuestUI.questBox.PositionOffset_Y = num5 * -0.5f - 30f;
			PlayerNPCQuestUI.questBox.PositionScale_Y = 0.5f;
			PlayerNPCQuestUI.questBox.SizeOffset_Y = num5;
			PlayerNPCQuestUI.questBox.SizeScale_Y = 0f;
		}

		// Token: 0x060043D2 RID: 17362 RVA: 0x001831E6 File Offset: 0x001813E6
		private static void onClickedAcceptButton(ISleekElement button)
		{
			PlayerNPCQuestUI.SetButtonsAreClickable(false);
			Player.player.quests.ClientChooseDialogueResponse(PlayerNPCQuestUI.dialogueContext.GUID, PlayerNPCQuestUI.pendingResponse.index);
		}

		// Token: 0x060043D3 RID: 17363 RVA: 0x00183211 File Offset: 0x00181411
		private static void onClickedDeclineButton(ISleekElement button)
		{
			PlayerNPCQuestUI.SetButtonsAreClickable(false);
			PlayerNPCQuestUI.close();
			PlayerNPCDialogueUI.OpenCurrentDialogue();
		}

		// Token: 0x060043D4 RID: 17364 RVA: 0x00183223 File Offset: 0x00181423
		private static void onClickedContinueButton(ISleekElement button)
		{
			PlayerNPCQuestUI.SetButtonsAreClickable(false);
			Player.player.quests.ClientChooseDialogueResponse(PlayerNPCQuestUI.dialogueContext.GUID, PlayerNPCQuestUI.pendingResponse.index);
		}

		// Token: 0x060043D5 RID: 17365 RVA: 0x0018324E File Offset: 0x0018144E
		private static void onClickedTrackButton(ISleekElement button)
		{
			Player.player.quests.ClientTrackQuest(PlayerNPCQuestUI.quest);
			if (!Provider.isServer)
			{
				Player.player.quests.TrackQuest(PlayerNPCQuestUI.quest);
			}
			PlayerNPCQuestUI.closeNicely();
		}

		// Token: 0x060043D6 RID: 17366 RVA: 0x00183284 File Offset: 0x00181484
		private static void onClickedAbandonButton(ISleekElement button)
		{
			Player.player.quests.ClientAbandonQuest(PlayerNPCQuestUI.quest);
			PlayerNPCQuestUI.closeNicely();
		}

		// Token: 0x060043D7 RID: 17367 RVA: 0x0018329F File Offset: 0x0018149F
		private static void onClickedReturnButton(ISleekElement button)
		{
			PlayerNPCQuestUI.closeNicely();
		}

		// Token: 0x060043D8 RID: 17368 RVA: 0x001832A6 File Offset: 0x001814A6
		private static void SetButtonsAreClickable(bool isClickable)
		{
			PlayerNPCQuestUI.acceptButton.IsClickable = isClickable;
			PlayerNPCQuestUI.declineButton.IsClickable = isClickable;
			PlayerNPCQuestUI.continueButton.IsClickable = isClickable;
		}

		// Token: 0x060043D9 RID: 17369 RVA: 0x001832CC File Offset: 0x001814CC
		public PlayerNPCQuestUI()
		{
			if (PlayerNPCQuestUI.icons != null)
			{
				PlayerNPCQuestUI.icons.unload();
			}
			PlayerNPCQuestUI.localization = Localization.read("/Player/PlayerNPCQuest.dat");
			PlayerNPCQuestUI.icons = Bundles.getBundle("/Bundles/Textures/Player/Icons/PlayerNPCQuest/PlayerNPCQuest.unity3d");
			PlayerNPCQuestUI.container = new SleekFullscreenBox();
			PlayerNPCQuestUI.container.PositionScale_Y = 1f;
			PlayerNPCQuestUI.container.PositionOffset_X = 10f;
			PlayerNPCQuestUI.container.PositionOffset_Y = 10f;
			PlayerNPCQuestUI.container.SizeOffset_X = -20f;
			PlayerNPCQuestUI.container.SizeOffset_Y = -20f;
			PlayerNPCQuestUI.container.SizeScale_X = 1f;
			PlayerNPCQuestUI.container.SizeScale_Y = 1f;
			PlayerUI.container.AddChild(PlayerNPCQuestUI.container);
			PlayerNPCQuestUI.active = false;
			PlayerNPCQuestUI.questBox = Glazier.Get().CreateBox();
			PlayerNPCQuestUI.questBox.PositionOffset_X = -250f;
			PlayerNPCQuestUI.questBox.PositionScale_X = 0.5f;
			PlayerNPCQuestUI.questBox.SizeOffset_X = 500f;
			PlayerNPCQuestUI.container.AddChild(PlayerNPCQuestUI.questBox);
			PlayerNPCQuestUI.nameLabel = Glazier.Get().CreateLabel();
			PlayerNPCQuestUI.nameLabel.PositionOffset_X = 5f;
			PlayerNPCQuestUI.nameLabel.PositionOffset_Y = 5f;
			PlayerNPCQuestUI.nameLabel.SizeOffset_X = -10f;
			PlayerNPCQuestUI.nameLabel.SizeOffset_Y = 30f;
			PlayerNPCQuestUI.nameLabel.SizeScale_X = 1f;
			PlayerNPCQuestUI.nameLabel.TextAlignment = 0;
			PlayerNPCQuestUI.nameLabel.TextColor = 4;
			PlayerNPCQuestUI.nameLabel.TextContrastContext = 1;
			PlayerNPCQuestUI.nameLabel.AllowRichText = true;
			PlayerNPCQuestUI.nameLabel.FontSize = 3;
			PlayerNPCQuestUI.questBox.AddChild(PlayerNPCQuestUI.nameLabel);
			PlayerNPCQuestUI.descriptionLabel = Glazier.Get().CreateLabel();
			PlayerNPCQuestUI.descriptionLabel.PositionOffset_X = 5f;
			PlayerNPCQuestUI.descriptionLabel.PositionOffset_Y = PlayerNPCQuestUI.nameLabel.SizeOffset_Y;
			PlayerNPCQuestUI.descriptionLabel.SizeOffset_X = -10f;
			PlayerNPCQuestUI.descriptionLabel.SizeOffset_Y = 70f;
			PlayerNPCQuestUI.descriptionLabel.SizeScale_X = 1f;
			PlayerNPCQuestUI.descriptionLabel.TextAlignment = 0;
			PlayerNPCQuestUI.descriptionLabel.TextColor = 4;
			PlayerNPCQuestUI.descriptionLabel.TextContrastContext = 1;
			PlayerNPCQuestUI.descriptionLabel.AllowRichText = true;
			PlayerNPCQuestUI.questBox.AddChild(PlayerNPCQuestUI.descriptionLabel);
			PlayerNPCQuestUI.conditionsAndRewardsScrollView = Glazier.Get().CreateScrollView();
			PlayerNPCQuestUI.conditionsAndRewardsScrollView.PositionOffset_X = 5f;
			PlayerNPCQuestUI.conditionsAndRewardsScrollView.PositionOffset_Y = PlayerNPCQuestUI.descriptionLabel.PositionOffset_Y + PlayerNPCQuestUI.descriptionLabel.SizeOffset_Y + 5f;
			PlayerNPCQuestUI.conditionsAndRewardsScrollView.SizeOffset_X = -10f;
			PlayerNPCQuestUI.conditionsAndRewardsScrollView.SizeOffset_Y = -PlayerNPCQuestUI.conditionsAndRewardsScrollView.PositionOffset_Y - 5f;
			PlayerNPCQuestUI.conditionsAndRewardsScrollView.SizeScale_X = 1f;
			PlayerNPCQuestUI.conditionsAndRewardsScrollView.SizeScale_Y = 1f;
			PlayerNPCQuestUI.conditionsAndRewardsScrollView.ScaleContentToWidth = true;
			PlayerNPCQuestUI.questBox.AddChild(PlayerNPCQuestUI.conditionsAndRewardsScrollView);
			PlayerNPCQuestUI.conditionsLabel = Glazier.Get().CreateLabel();
			PlayerNPCQuestUI.conditionsLabel.SizeOffset_Y = 30f;
			PlayerNPCQuestUI.conditionsLabel.SizeScale_X = 1f;
			PlayerNPCQuestUI.conditionsLabel.TextAlignment = 3;
			PlayerNPCQuestUI.conditionsLabel.Text = PlayerNPCQuestUI.localization.format("Conditions");
			PlayerNPCQuestUI.conditionsLabel.FontSize = 3;
			PlayerNPCQuestUI.conditionsAndRewardsScrollView.AddChild(PlayerNPCQuestUI.conditionsLabel);
			PlayerNPCQuestUI.conditionsContainer = Glazier.Get().CreateFrame();
			PlayerNPCQuestUI.conditionsContainer.PositionOffset_Y = 30f;
			PlayerNPCQuestUI.conditionsContainer.SizeScale_X = 1f;
			PlayerNPCQuestUI.conditionsAndRewardsScrollView.AddChild(PlayerNPCQuestUI.conditionsContainer);
			PlayerNPCQuestUI.rewardsLabel = Glazier.Get().CreateLabel();
			PlayerNPCQuestUI.rewardsLabel.SizeOffset_Y = 30f;
			PlayerNPCQuestUI.rewardsLabel.SizeScale_X = 1f;
			PlayerNPCQuestUI.rewardsLabel.TextAlignment = 3;
			PlayerNPCQuestUI.rewardsLabel.Text = PlayerNPCQuestUI.localization.format("Rewards");
			PlayerNPCQuestUI.rewardsLabel.FontSize = 3;
			PlayerNPCQuestUI.conditionsAndRewardsScrollView.AddChild(PlayerNPCQuestUI.rewardsLabel);
			PlayerNPCQuestUI.rewardsContainer = Glazier.Get().CreateFrame();
			PlayerNPCQuestUI.rewardsContainer.SizeScale_X = 1f;
			PlayerNPCQuestUI.conditionsAndRewardsScrollView.AddChild(PlayerNPCQuestUI.rewardsContainer);
			PlayerNPCQuestUI.beginContainer = Glazier.Get().CreateFrame();
			PlayerNPCQuestUI.beginContainer.PositionOffset_Y = 10f;
			PlayerNPCQuestUI.beginContainer.PositionScale_Y = 1f;
			PlayerNPCQuestUI.beginContainer.SizeOffset_Y = 50f;
			PlayerNPCQuestUI.beginContainer.SizeScale_X = 1f;
			PlayerNPCQuestUI.questBox.AddChild(PlayerNPCQuestUI.beginContainer);
			PlayerNPCQuestUI.beginContainer.IsVisible = false;
			PlayerNPCQuestUI.endContainer = Glazier.Get().CreateFrame();
			PlayerNPCQuestUI.endContainer.PositionOffset_Y = 10f;
			PlayerNPCQuestUI.endContainer.PositionScale_Y = 1f;
			PlayerNPCQuestUI.endContainer.SizeOffset_Y = 50f;
			PlayerNPCQuestUI.endContainer.SizeScale_X = 1f;
			PlayerNPCQuestUI.questBox.AddChild(PlayerNPCQuestUI.endContainer);
			PlayerNPCQuestUI.endContainer.IsVisible = false;
			PlayerNPCQuestUI.detailsContainer = Glazier.Get().CreateFrame();
			PlayerNPCQuestUI.detailsContainer.PositionOffset_Y = 10f;
			PlayerNPCQuestUI.detailsContainer.PositionScale_Y = 1f;
			PlayerNPCQuestUI.detailsContainer.SizeOffset_Y = 50f;
			PlayerNPCQuestUI.detailsContainer.SizeScale_X = 1f;
			PlayerNPCQuestUI.questBox.AddChild(PlayerNPCQuestUI.detailsContainer);
			PlayerNPCQuestUI.detailsContainer.IsVisible = false;
			PlayerNPCQuestUI.acceptButton = Glazier.Get().CreateButton();
			PlayerNPCQuestUI.acceptButton.SizeOffset_X = -5f;
			PlayerNPCQuestUI.acceptButton.SizeScale_X = 0.5f;
			PlayerNPCQuestUI.acceptButton.SizeScale_Y = 1f;
			PlayerNPCQuestUI.acceptButton.Text = PlayerNPCQuestUI.localization.format("Accept");
			PlayerNPCQuestUI.acceptButton.TooltipText = PlayerNPCQuestUI.localization.format("Accept_Tooltip");
			PlayerNPCQuestUI.acceptButton.FontSize = 3;
			PlayerNPCQuestUI.acceptButton.OnClicked += new ClickedButton(PlayerNPCQuestUI.onClickedAcceptButton);
			PlayerNPCQuestUI.beginContainer.AddChild(PlayerNPCQuestUI.acceptButton);
			PlayerNPCQuestUI.declineButton = Glazier.Get().CreateButton();
			PlayerNPCQuestUI.declineButton.PositionOffset_X = 5f;
			PlayerNPCQuestUI.declineButton.PositionScale_X = 0.5f;
			PlayerNPCQuestUI.declineButton.SizeOffset_X = -5f;
			PlayerNPCQuestUI.declineButton.SizeScale_X = 0.5f;
			PlayerNPCQuestUI.declineButton.SizeScale_Y = 1f;
			PlayerNPCQuestUI.declineButton.Text = PlayerNPCQuestUI.localization.format("Decline");
			PlayerNPCQuestUI.declineButton.TooltipText = PlayerNPCQuestUI.localization.format("Decline_Tooltip");
			PlayerNPCQuestUI.declineButton.FontSize = 3;
			PlayerNPCQuestUI.declineButton.OnClicked += new ClickedButton(PlayerNPCQuestUI.onClickedDeclineButton);
			PlayerNPCQuestUI.beginContainer.AddChild(PlayerNPCQuestUI.declineButton);
			PlayerNPCQuestUI.continueButton = Glazier.Get().CreateButton();
			PlayerNPCQuestUI.continueButton.SizeScale_X = 1f;
			PlayerNPCQuestUI.continueButton.SizeScale_Y = 1f;
			PlayerNPCQuestUI.continueButton.Text = PlayerNPCQuestUI.localization.format("Continue");
			PlayerNPCQuestUI.continueButton.TooltipText = PlayerNPCQuestUI.localization.format("Continue_Tooltip");
			PlayerNPCQuestUI.continueButton.FontSize = 3;
			PlayerNPCQuestUI.continueButton.OnClicked += new ClickedButton(PlayerNPCQuestUI.onClickedContinueButton);
			PlayerNPCQuestUI.endContainer.AddChild(PlayerNPCQuestUI.continueButton);
			PlayerNPCQuestUI.trackButton = Glazier.Get().CreateButton();
			PlayerNPCQuestUI.trackButton.SizeOffset_X = -5f;
			PlayerNPCQuestUI.trackButton.SizeScale_X = 0.333f;
			PlayerNPCQuestUI.trackButton.SizeScale_Y = 1f;
			PlayerNPCQuestUI.trackButton.TooltipText = PlayerNPCQuestUI.localization.format("Track_Tooltip");
			PlayerNPCQuestUI.trackButton.FontSize = 3;
			PlayerNPCQuestUI.trackButton.OnClicked += new ClickedButton(PlayerNPCQuestUI.onClickedTrackButton);
			PlayerNPCQuestUI.detailsContainer.AddChild(PlayerNPCQuestUI.trackButton);
			PlayerNPCQuestUI.abandonButton = Glazier.Get().CreateButton();
			PlayerNPCQuestUI.abandonButton.PositionOffset_X = 5f;
			PlayerNPCQuestUI.abandonButton.PositionScale_X = 0.333f;
			PlayerNPCQuestUI.abandonButton.SizeOffset_X = -10f;
			PlayerNPCQuestUI.abandonButton.SizeScale_X = 0.333f;
			PlayerNPCQuestUI.abandonButton.SizeScale_Y = 1f;
			PlayerNPCQuestUI.abandonButton.Text = PlayerNPCQuestUI.localization.format("Abandon");
			PlayerNPCQuestUI.abandonButton.TooltipText = PlayerNPCQuestUI.localization.format("Abandon_Tooltip");
			PlayerNPCQuestUI.abandonButton.FontSize = 3;
			PlayerNPCQuestUI.abandonButton.OnClicked += new ClickedButton(PlayerNPCQuestUI.onClickedAbandonButton);
			PlayerNPCQuestUI.detailsContainer.AddChild(PlayerNPCQuestUI.abandonButton);
			PlayerNPCQuestUI.returnButton = Glazier.Get().CreateButton();
			PlayerNPCQuestUI.returnButton.PositionOffset_X = 5f;
			PlayerNPCQuestUI.returnButton.PositionScale_X = 0.667f;
			PlayerNPCQuestUI.returnButton.SizeOffset_X = -5f;
			PlayerNPCQuestUI.returnButton.SizeScale_X = 0.333f;
			PlayerNPCQuestUI.returnButton.SizeScale_Y = 1f;
			PlayerNPCQuestUI.returnButton.Text = PlayerNPCQuestUI.localization.format("Return");
			PlayerNPCQuestUI.returnButton.TooltipText = PlayerNPCQuestUI.localization.format("Return_Tooltip");
			PlayerNPCQuestUI.returnButton.FontSize = 3;
			PlayerNPCQuestUI.returnButton.OnClicked += new ClickedButton(PlayerNPCQuestUI.onClickedReturnButton);
			PlayerNPCQuestUI.detailsContainer.AddChild(PlayerNPCQuestUI.returnButton);
		}

		// Token: 0x04002D29 RID: 11561
		private static SleekFullscreenBox container;

		// Token: 0x04002D2A RID: 11562
		public static Local localization;

		// Token: 0x04002D2B RID: 11563
		public static Bundle icons;

		// Token: 0x04002D2C RID: 11564
		public static bool active;

		// Token: 0x04002D2D RID: 11565
		private static QuestAsset quest;

		/// <summary>
		/// Valid when opened in Begin or End mode.
		///
		/// If the quest is ready to complete the UI is opened in End mode to allow
		/// the player to see what rewards they will receive after clicking continue. 
		/// Otherwise, in Begin mode the UI is opened to allow the player to review
		/// the conditions before accepting or declining the request.
		///
		/// If the player cancels the pending response is NOT chosen.
		/// </summary>
		// Token: 0x04002D2E RID: 11566
		private static DialogueResponse pendingResponse;

		/// <summary>
		/// Valid when opened in Begin or End mode.
		/// The player clicked pendingResponse in this dialogue to open the quest UI.
		/// </summary>
		// Token: 0x04002D2F RID: 11567
		private static DialogueAsset dialogueContext;

		// Token: 0x04002D30 RID: 11568
		private static EQuestViewMode mode;

		// Token: 0x04002D31 RID: 11569
		private static ISleekBox questBox;

		// Token: 0x04002D32 RID: 11570
		private static ISleekLabel nameLabel;

		// Token: 0x04002D33 RID: 11571
		private static ISleekLabel descriptionLabel;

		// Token: 0x04002D34 RID: 11572
		private static ISleekScrollView conditionsAndRewardsScrollView;

		// Token: 0x04002D35 RID: 11573
		private static ISleekLabel conditionsLabel;

		// Token: 0x04002D36 RID: 11574
		private static ISleekElement conditionsContainer;

		// Token: 0x04002D37 RID: 11575
		private static ISleekLabel rewardsLabel;

		// Token: 0x04002D38 RID: 11576
		private static ISleekElement rewardsContainer;

		// Token: 0x04002D39 RID: 11577
		private static ISleekElement beginContainer;

		// Token: 0x04002D3A RID: 11578
		private static ISleekButton acceptButton;

		// Token: 0x04002D3B RID: 11579
		private static ISleekButton declineButton;

		// Token: 0x04002D3C RID: 11580
		private static ISleekElement endContainer;

		// Token: 0x04002D3D RID: 11581
		private static ISleekButton continueButton;

		// Token: 0x04002D3E RID: 11582
		private static ISleekElement detailsContainer;

		// Token: 0x04002D3F RID: 11583
		private static ISleekButton trackButton;

		// Token: 0x04002D40 RID: 11584
		private static ISleekButton abandonButton;

		// Token: 0x04002D41 RID: 11585
		private static ISleekButton returnButton;

		// Token: 0x04002D42 RID: 11586
		private const int LOWER_BUTTONS_HEIGHT = 50;

		// Token: 0x04002D43 RID: 11587
		private const int LOWER_BUTTONS_VERTICAL_OFFSET = 10;

		// Token: 0x04002D44 RID: 11588
		private const int QUEST_BOX_INNER_SPACING = 5;

		// Token: 0x04002D45 RID: 11589
		private static List<bool> areConditionsMet = new List<bool>(8);
	}
}
