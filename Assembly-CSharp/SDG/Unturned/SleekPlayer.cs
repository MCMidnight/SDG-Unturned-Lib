using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000729 RID: 1833
	public class SleekPlayer : SleekWrapper
	{
		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x06003C69 RID: 15465 RVA: 0x0011C890 File Offset: 0x0011AA90
		// (set) Token: 0x06003C6A RID: 15466 RVA: 0x0011C898 File Offset: 0x0011AA98
		public SteamPlayer player { get; private set; }

		// Token: 0x06003C6B RID: 15467 RVA: 0x0011C8A4 File Offset: 0x0011AAA4
		private void onClickedPlayerButton(ISleekElement button)
		{
			Provider.provider.browserService.open("http://steamcommunity.com/profiles/" + this.player.playerID.steamID.ToString());
		}

		// Token: 0x06003C6C RID: 15468 RVA: 0x0011C8E8 File Offset: 0x0011AAE8
		private void OnMuteVoiceChatClicked(ISleekElement button)
		{
			this.UpdateMuteVoiceChatLabel();
		}

		// Token: 0x06003C6D RID: 15469 RVA: 0x0011C8F0 File Offset: 0x0011AAF0
		private void OnMuteTextChatClicked(ISleekElement button)
		{
			this.UpdateMuteTextChatLabel();
		}

		// Token: 0x06003C6E RID: 15470 RVA: 0x0011C8F8 File Offset: 0x0011AAF8
		private void onClickedPromoteButton(ISleekElement button)
		{
			Player.player.quests.sendPromote(this.player.playerID.steamID);
		}

		// Token: 0x06003C6F RID: 15471 RVA: 0x0011C919 File Offset: 0x0011AB19
		private void onClickedDemoteButton(ISleekElement button)
		{
			Player.player.quests.sendDemote(this.player.playerID.steamID);
		}

		// Token: 0x06003C70 RID: 15472 RVA: 0x0011C93C File Offset: 0x0011AB3C
		private void onClickedKickButton(ISleekElement button)
		{
			if (this.context == SleekPlayer.ESleekPlayerDisplayContext.GROUP_ROSTER)
			{
				Player.player.quests.sendKickFromGroup(this.player.playerID.steamID);
				return;
			}
			if (this.context == SleekPlayer.ESleekPlayerDisplayContext.PLAYER_LIST)
			{
				ChatManager.sendCallVote(this.player.playerID.steamID);
				PlayerDashboardUI.close();
				PlayerLifeUI.open();
			}
		}

		// Token: 0x06003C71 RID: 15473 RVA: 0x0011C99A File Offset: 0x0011AB9A
		private void onClickedInviteButton(ISleekElement button)
		{
			Player.player.quests.sendAskAddGroupInvite(this.player.playerID.steamID);
		}

		// Token: 0x06003C72 RID: 15474 RVA: 0x0011C9BC File Offset: 0x0011ABBC
		private void onClickedSpyButton(ISleekElement button)
		{
			ChatManager.sendChat(EChatMode.GLOBAL, "/spy " + this.player.playerID.steamID.ToString());
		}

		// Token: 0x06003C73 RID: 15475 RVA: 0x0011C9F7 File Offset: 0x0011ABF7
		private void onTalked(bool isTalking)
		{
			this.voice.IsVisible = isTalking;
		}

		// Token: 0x06003C74 RID: 15476 RVA: 0x0011CA05 File Offset: 0x0011AC05
		public override void OnDestroy()
		{
			if (this.player != null)
			{
				this.player.player.voice.onTalkingChanged -= this.onTalked;
			}
		}

		// Token: 0x06003C75 RID: 15477 RVA: 0x0011CA30 File Offset: 0x0011AC30
		private void UpdateMuteVoiceChatLabel()
		{
			this.muteVoiceChatButton.Text = (this.player.isVoiceChatLocallyMuted ? PlayerDashboardInformationUI.localization.format("UnmuteVoiceChat_Label") : PlayerDashboardInformationUI.localization.format("MuteVoiceChat_Label"));
		}

		// Token: 0x06003C76 RID: 15478 RVA: 0x0011CA6A File Offset: 0x0011AC6A
		private void UpdateMuteTextChatLabel()
		{
			this.muteTextChatButton.Text = (this.player.isTextChatLocallyMuted ? PlayerDashboardInformationUI.localization.format("UnmuteTextChat_Label") : PlayerDashboardInformationUI.localization.format("MuteTextChat_Label"));
		}

		// Token: 0x06003C77 RID: 15479 RVA: 0x0011CAA4 File Offset: 0x0011ACA4
		public SleekPlayer(SteamPlayer newPlayer, bool isButton, SleekPlayer.ESleekPlayerDisplayContext context)
		{
			this.player = newPlayer;
			this.context = context;
			Texture2D texture;
			if (OptionsSettings.streamer)
			{
				texture = null;
			}
			else if (Provider.isServer)
			{
				texture = Provider.provider.communityService.getIcon(Provider.user, false);
			}
			else
			{
				texture = Provider.provider.communityService.getIcon(this.player.playerID.steamID, false);
			}
			SleekColor backgroundColor = 1;
			SleekColor textColor = 2;
			if (this.player.isAdmin && !Provider.isServer)
			{
				backgroundColor = SleekColor.BackgroundIfLight(Palette.ADMIN);
				textColor = Palette.ADMIN;
			}
			else if (this.player.isPro)
			{
				backgroundColor = SleekColor.BackgroundIfLight(Palette.PRO);
				textColor = Palette.PRO;
			}
			if (isButton)
			{
				ISleekButton sleekButton = Glazier.Get().CreateButton();
				sleekButton.SizeScale_X = 1f;
				sleekButton.SizeScale_Y = 1f;
				sleekButton.TooltipText = this.player.playerID.playerName;
				sleekButton.FontSize = 3;
				sleekButton.BackgroundColor = backgroundColor;
				sleekButton.TextColor = textColor;
				sleekButton.OnClicked += new ClickedButton(this.onClickedPlayerButton);
				base.AddChild(sleekButton);
				this.box = sleekButton;
			}
			else
			{
				ISleekBox sleekBox = Glazier.Get().CreateBox();
				sleekBox.SizeScale_X = 1f;
				sleekBox.SizeScale_Y = 1f;
				sleekBox.TooltipText = this.player.playerID.playerName;
				sleekBox.FontSize = 3;
				sleekBox.BackgroundColor = backgroundColor;
				sleekBox.TextColor = textColor;
				base.AddChild(sleekBox);
				this.box = sleekBox;
			}
			this.avatarImage = Glazier.Get().CreateImage();
			this.avatarImage.PositionOffset_X = 9f;
			this.avatarImage.PositionOffset_Y = 9f;
			this.avatarImage.SizeOffset_X = 32f;
			this.avatarImage.SizeOffset_Y = 32f;
			this.avatarImage.Texture = texture;
			this.avatarImage.ShouldDestroyTexture = true;
			this.box.AddChild(this.avatarImage);
			if (this.player.player != null && this.player.player.skills != null)
			{
				this.repImage = Glazier.Get().CreateImage();
				this.repImage.PositionOffset_X = 46f;
				this.repImage.PositionOffset_Y = 9f;
				this.repImage.SizeOffset_X = 32f;
				this.repImage.SizeOffset_Y = 32f;
				this.repImage.Texture = PlayerTool.getRepTexture(this.player.player.skills.reputation);
				this.repImage.TintColor = PlayerTool.getRepColor(this.player.player.skills.reputation);
				this.box.AddChild(this.repImage);
			}
			this.nameLabel = Glazier.Get().CreateLabel();
			this.nameLabel.PositionOffset_X = 83f;
			this.nameLabel.SizeOffset_X = -113f;
			this.nameLabel.SizeOffset_Y = 30f;
			this.nameLabel.SizeScale_X = 1f;
			this.nameLabel.Text = this.player.GetLocalDisplayName();
			this.nameLabel.FontSize = 3;
			this.box.AddChild(this.nameLabel);
			if (this.player.player != null && this.player.player.skills != null)
			{
				this.repLabel = Glazier.Get().CreateLabel();
				this.repLabel.PositionOffset_X = 83f;
				this.repLabel.PositionOffset_Y = 20f;
				this.repLabel.SizeOffset_X = -113f;
				this.repLabel.SizeOffset_Y = 30f;
				this.repLabel.SizeScale_X = 1f;
				this.repLabel.TextColor = this.repImage.TintColor;
				this.repLabel.Text = PlayerTool.getRepTitle(this.player.player.skills.reputation);
				this.repLabel.TextContrastContext = 1;
				this.box.AddChild(this.repLabel);
			}
			if (context == SleekPlayer.ESleekPlayerDisplayContext.GROUP_ROSTER)
			{
				this.nameLabel.PositionOffset_Y = -5f;
				this.repLabel.PositionOffset_Y = 10f;
				ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
				sleekLabel.PositionOffset_X = 83f;
				sleekLabel.PositionOffset_Y = 25f;
				sleekLabel.SizeOffset_X = -113f;
				sleekLabel.SizeOffset_Y = 30f;
				sleekLabel.SizeScale_X = 1f;
				sleekLabel.TextColor = this.repImage.TintColor;
				sleekLabel.TextContrastContext = 1;
				this.box.AddChild(sleekLabel);
				switch (this.player.player.quests.groupRank)
				{
				case EPlayerGroupRank.MEMBER:
					sleekLabel.Text = PlayerDashboardInformationUI.localization.format("Group_Rank_Member");
					break;
				case EPlayerGroupRank.ADMIN:
					sleekLabel.Text = PlayerDashboardInformationUI.localization.format("Group_Rank_Admin");
					break;
				case EPlayerGroupRank.OWNER:
					sleekLabel.Text = PlayerDashboardInformationUI.localization.format("Group_Rank_Owner");
					break;
				}
			}
			this.voice = Glazier.Get().CreateImage();
			this.voice.PositionOffset_X = 15f;
			this.voice.PositionOffset_Y = 15f;
			this.voice.SizeOffset_X = 20f;
			this.voice.SizeOffset_Y = 20f;
			this.voice.Texture = PlayerDashboardInformationUI.icons.load<Texture2D>("Voice");
			this.box.AddChild(this.voice);
			this.skillset = Glazier.Get().CreateImage();
			this.skillset.PositionOffset_X = -25f;
			this.skillset.PositionOffset_Y = 25f;
			this.skillset.PositionScale_X = 1f;
			this.skillset.SizeOffset_X = 20f;
			this.skillset.SizeOffset_Y = 20f;
			this.skillset.Texture = MenuSurvivorsCharacterUI.icons.load<Texture2D>("Skillset_" + ((int)this.player.skillset).ToString());
			this.skillset.TintColor = 2;
			this.box.AddChild(this.skillset);
			if (this.player.isAdmin && !Provider.isServer)
			{
				this.nameLabel.TextColor = Palette.ADMIN;
				this.nameLabel.TextContrastContext = 1;
				this.icon = Glazier.Get().CreateImage();
				this.icon.PositionOffset_X = -25f;
				this.icon.PositionOffset_Y = 5f;
				this.icon.PositionScale_X = 1f;
				this.icon.SizeOffset_X = 20f;
				this.icon.SizeOffset_Y = 20f;
				this.icon.Texture = PlayerDashboardInformationUI.icons.load<Texture2D>("Admin");
				this.box.AddChild(this.icon);
			}
			else if (this.player.isPro)
			{
				this.nameLabel.TextColor = Palette.PRO;
				this.nameLabel.TextContrastContext = 1;
				this.icon = Glazier.Get().CreateImage();
				this.icon.PositionOffset_X = -25f;
				this.icon.PositionOffset_Y = 5f;
				this.icon.PositionScale_X = 1f;
				this.icon.SizeOffset_X = 20f;
				this.icon.SizeOffset_Y = 20f;
				this.icon.Texture = PlayerDashboardInformationUI.icons.load<Texture2D>("Pro");
				this.box.AddChild(this.icon);
			}
			if (context == SleekPlayer.ESleekPlayerDisplayContext.GROUP_ROSTER)
			{
				int num = 0;
				if (!this.player.player.channel.IsLocalPlayer)
				{
					if (Player.player.quests.hasPermissionToChangeRank)
					{
						if (this.player.player.quests.groupRank < EPlayerGroupRank.OWNER)
						{
							ISleekButton sleekButton2 = Glazier.Get().CreateButton();
							sleekButton2.PositionOffset_X = (float)num;
							sleekButton2.PositionScale_X = 1f;
							sleekButton2.SizeOffset_X = 80f;
							sleekButton2.SizeScale_Y = 1f;
							sleekButton2.Text = PlayerDashboardInformationUI.localization.format("Group_Promote");
							sleekButton2.TooltipText = PlayerDashboardInformationUI.localization.format("Group_Promote_Tooltip");
							sleekButton2.OnClicked += new ClickedButton(this.onClickedPromoteButton);
							this.box.AddChild(sleekButton2);
							num += 80;
						}
						if (this.player.player.quests.groupRank == EPlayerGroupRank.ADMIN)
						{
							ISleekButton sleekButton3 = Glazier.Get().CreateButton();
							sleekButton3.PositionOffset_X = (float)num;
							sleekButton3.PositionScale_X = 1f;
							sleekButton3.SizeOffset_X = 80f;
							sleekButton3.SizeScale_Y = 1f;
							sleekButton3.Text = PlayerDashboardInformationUI.localization.format("Group_Demote");
							sleekButton3.TooltipText = PlayerDashboardInformationUI.localization.format("Group_Demote_Tooltip");
							sleekButton3.OnClicked += new ClickedButton(this.onClickedDemoteButton);
							this.box.AddChild(sleekButton3);
							num += 80;
						}
					}
					if (Player.player.quests.hasPermissionToKickMembers && this.player.player.quests.canBeKickedFromGroup)
					{
						ISleekButton sleekButton4 = Glazier.Get().CreateButton();
						sleekButton4.PositionOffset_X = (float)num;
						sleekButton4.PositionScale_X = 1f;
						sleekButton4.SizeOffset_X = 50f;
						sleekButton4.SizeScale_Y = 1f;
						sleekButton4.Text = PlayerDashboardInformationUI.localization.format("Group_Kick");
						sleekButton4.TooltipText = PlayerDashboardInformationUI.localization.format("Group_Kick_Tooltip");
						sleekButton4.OnClicked += new ClickedButton(this.onClickedKickButton);
						this.box.AddChild(sleekButton4);
						num += 50;
					}
				}
				this.box.SizeOffset_X = (float)(-(float)num);
			}
			else if (context == SleekPlayer.ESleekPlayerDisplayContext.PLAYER_LIST)
			{
				int num2 = 0;
				if (!this.player.player.channel.IsLocalPlayer)
				{
					this.muteVoiceChatButton = Glazier.Get().CreateButton();
					this.muteVoiceChatButton.PositionScale_X = 1f;
					this.muteVoiceChatButton.SizeOffset_X = 100f;
					this.muteVoiceChatButton.SizeScale_Y = 0.5f;
					this.UpdateMuteVoiceChatLabel();
					this.muteVoiceChatButton.TooltipText = PlayerDashboardInformationUI.localization.format("Mute_Tooltip");
					this.muteVoiceChatButton.OnClicked += new ClickedButton(this.OnMuteVoiceChatClicked);
					this.box.AddChild(this.muteVoiceChatButton);
					this.muteTextChatButton = Glazier.Get().CreateButton();
					this.muteTextChatButton.PositionScale_X = 1f;
					this.muteTextChatButton.PositionScale_Y = 0.5f;
					this.muteTextChatButton.SizeOffset_X = 100f;
					this.muteTextChatButton.SizeScale_Y = 0.5f;
					this.UpdateMuteTextChatLabel();
					this.muteTextChatButton.TooltipText = PlayerDashboardInformationUI.localization.format("Mute_Tooltip");
					this.muteTextChatButton.OnClicked += new ClickedButton(this.OnMuteTextChatClicked);
					this.box.AddChild(this.muteTextChatButton);
					num2 += 100;
				}
				if (!this.player.player.channel.IsLocalPlayer && !this.player.isAdmin)
				{
					ISleekButton sleekButton5 = Glazier.Get().CreateButton();
					sleekButton5.PositionOffset_X = (float)num2;
					sleekButton5.PositionScale_X = 1f;
					sleekButton5.SizeOffset_X = 50f;
					sleekButton5.SizeScale_Y = 1f;
					sleekButton5.Text = PlayerDashboardInformationUI.localization.format("Vote_Kick");
					sleekButton5.TooltipText = PlayerDashboardInformationUI.localization.format("Vote_Kick_Tooltip");
					sleekButton5.OnClicked += new ClickedButton(this.onClickedKickButton);
					this.box.AddChild(sleekButton5);
					num2 += 50;
				}
				if (Player.player != null)
				{
					if (!this.player.player.channel.IsLocalPlayer && Player.player.quests.isMemberOfAGroup && Player.player.quests.hasPermissionToInviteMembers && Player.player.quests.hasSpaceForMoreMembersInGroup && !this.player.player.quests.isMemberOfAGroup)
					{
						ISleekButton sleekButton6 = Glazier.Get().CreateButton();
						sleekButton6.PositionOffset_X = (float)num2;
						sleekButton6.PositionScale_X = 1f;
						sleekButton6.SizeOffset_X = 60f;
						sleekButton6.SizeScale_Y = 1f;
						sleekButton6.Text = PlayerDashboardInformationUI.localization.format("Group_Invite");
						sleekButton6.TooltipText = PlayerDashboardInformationUI.localization.format("Group_Invite_Tooltip");
						sleekButton6.OnClicked += new ClickedButton(this.onClickedInviteButton);
						this.box.AddChild(sleekButton6);
						num2 += 60;
					}
					if (Player.player.channel.owner.isAdmin)
					{
						ISleekButton sleekButton7 = Glazier.Get().CreateButton();
						sleekButton7.PositionOffset_X = (float)num2;
						sleekButton7.PositionScale_X = 1f;
						sleekButton7.SizeOffset_X = 50f;
						sleekButton7.SizeScale_Y = 1f;
						sleekButton7.Text = PlayerDashboardInformationUI.localization.format("Spy");
						sleekButton7.TooltipText = PlayerDashboardInformationUI.localization.format("Spy_Tooltip");
						sleekButton7.OnClicked += new ClickedButton(this.onClickedSpyButton);
						this.box.AddChild(sleekButton7);
						num2 += 50;
					}
				}
				this.box.SizeOffset_X = (float)(-(float)num2);
			}
			if (this.player != null)
			{
				this.player.player.voice.onTalkingChanged += this.onTalked;
				this.onTalked(this.player.player.voice.isTalking);
			}
		}

		// Token: 0x040025BF RID: 9663
		private ISleekElement box;

		// Token: 0x040025C0 RID: 9664
		private ISleekImage avatarImage;

		// Token: 0x040025C1 RID: 9665
		private ISleekImage repImage;

		// Token: 0x040025C2 RID: 9666
		private ISleekLabel nameLabel;

		// Token: 0x040025C3 RID: 9667
		private ISleekLabel repLabel;

		// Token: 0x040025C4 RID: 9668
		private ISleekImage icon;

		// Token: 0x040025C5 RID: 9669
		private ISleekImage voice;

		// Token: 0x040025C6 RID: 9670
		private ISleekImage skillset;

		// Token: 0x040025C7 RID: 9671
		private ISleekButton muteVoiceChatButton;

		// Token: 0x040025C8 RID: 9672
		private ISleekButton muteTextChatButton;

		// Token: 0x040025CA RID: 9674
		private SleekPlayer.ESleekPlayerDisplayContext context;

		// Token: 0x020009F1 RID: 2545
		public enum ESleekPlayerDisplayContext
		{
			// Token: 0x040034BB RID: 13499
			NONE,
			// Token: 0x040034BC RID: 13500
			GROUP_ROSTER,
			// Token: 0x040034BD RID: 13501
			PLAYER_LIST
		}
	}
}
