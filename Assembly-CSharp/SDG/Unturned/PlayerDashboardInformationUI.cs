using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007C2 RID: 1986
	public class PlayerDashboardInformationUI
	{
		// Token: 0x060042D2 RID: 17106 RVA: 0x00172348 File Offset: 0x00170548
		private static void synchronizeMapVisibility(int view)
		{
			if (view == 0)
			{
				if (PlayerDashboardInformationUI.chartTexture != null && !PlayerUI.isBlindfolded && PlayerDashboardInformationUI.hasChart)
				{
					PlayerDashboardInformationUI.mapImage.Texture = PlayerDashboardInformationUI.chartTexture;
					PlayerDashboardInformationUI.noLabel.IsVisible = false;
				}
				else
				{
					PlayerDashboardInformationUI.mapImage.Texture = PlayerDashboardInformationUI.staticTexture;
					PlayerDashboardInformationUI.noLabel.Text = PlayerDashboardInformationUI.localization.format("No_Chart");
					PlayerDashboardInformationUI.noLabel.IsVisible = true;
				}
			}
			else if (PlayerDashboardInformationUI.mapTexture != null && !PlayerUI.isBlindfolded && PlayerDashboardInformationUI.hasGPS)
			{
				PlayerDashboardInformationUI.mapImage.Texture = PlayerDashboardInformationUI.mapTexture;
				PlayerDashboardInformationUI.noLabel.IsVisible = false;
			}
			else
			{
				PlayerDashboardInformationUI.mapImage.Texture = PlayerDashboardInformationUI.staticTexture;
				PlayerDashboardInformationUI.noLabel.Text = PlayerDashboardInformationUI.localization.format("No_GPS");
				PlayerDashboardInformationUI.noLabel.IsVisible = true;
			}
			bool flag = !PlayerDashboardInformationUI.noLabel.IsVisible;
			PlayerDashboardInformationUI.mapLocationsContainer.IsVisible = flag;
			bool flag2 = flag && Provider.modeConfigData.Gameplay.Group_Map;
			PlayerDashboardInformationUI.mapMarkersContainer.IsVisible = (flag2 && PlayerDashboardInformationUI.showMarkersToggle.Value);
			PlayerDashboardInformationUI.mapArenaContainer.IsVisible = (flag2 && LevelManager.levelType == ELevelType.ARENA);
			PlayerDashboardInformationUI.mapRemotePlayersContainer.IsVisible = (flag2 && (PlayerDashboardInformationUI.showPlayerNamesToggle.Value || PlayerDashboardInformationUI.showPlayerAvatarsToggle.Value));
			PlayerDashboardInformationUI.localPlayerImage.IsVisible = flag2;
		}

		// Token: 0x060042D3 RID: 17107 RVA: 0x001724C8 File Offset: 0x001706C8
		private static void updateMarkers()
		{
			int num = 0;
			foreach (SteamPlayer steamPlayer in Provider.clients)
			{
				if (!(steamPlayer.model == null))
				{
					PlayerQuests quests = steamPlayer.player.quests;
					if ((!(steamPlayer.playerID.steamID != Provider.client) || quests.isMemberOfSameGroupAs(Player.player)) && quests.isMarkerPlaced)
					{
						ISleekImage sleekImage;
						if (num < PlayerDashboardInformationUI.markerImages.Count)
						{
							sleekImage = PlayerDashboardInformationUI.markerImages[num];
							sleekImage.IsVisible = true;
						}
						else
						{
							sleekImage = Glazier.Get().CreateImage(PlayerDashboardInformationUI.icons.load<Texture2D>("Marker"));
							sleekImage.PositionOffset_X = -10f;
							sleekImage.PositionOffset_Y = -10f;
							sleekImage.SizeOffset_X = 20f;
							sleekImage.SizeOffset_Y = 20f;
							sleekImage.AddLabel(string.Empty, 1);
							PlayerDashboardInformationUI.mapMarkersContainer.AddChild(sleekImage);
							PlayerDashboardInformationUI.markerImages.Add(sleekImage);
						}
						num++;
						Vector2 vector = PlayerDashboardInformationUI.ProjectWorldPositionToMap(quests.markerPosition);
						sleekImage.PositionScale_X = vector.x;
						sleekImage.PositionScale_Y = vector.y;
						sleekImage.TintColor = steamPlayer.markerColor;
						string text = quests.markerTextOverride;
						if (string.IsNullOrEmpty(text))
						{
							if (string.IsNullOrEmpty(steamPlayer.playerID.nickName))
							{
								text = steamPlayer.playerID.characterName;
							}
							else
							{
								text = steamPlayer.playerID.nickName;
							}
						}
						sleekImage.UpdateLabel(text);
					}
				}
			}
			for (int i = PlayerDashboardInformationUI.markerImages.Count - 1; i >= num; i--)
			{
				PlayerDashboardInformationUI.markerImages[i].IsVisible = false;
			}
		}

		// Token: 0x060042D4 RID: 17108 RVA: 0x001726C0 File Offset: 0x001708C0
		private static void updateArenaCircle()
		{
			int num = 0;
			if ((double)Mathf.Abs(LevelManager.arenaTargetRadius - 0.5f) > 0.01)
			{
				num = Mathf.RoundToInt(Mathf.Lerp(10f, 64f, LevelManager.arenaTargetRadius / 2000f));
				num *= PlayerDashboardInformationUI.zoomMultiplier;
				if (num > 1)
				{
					float num2 = Time.time / 100f;
					num2 -= Mathf.Floor(num2);
					for (int i = 0; i < num; i++)
					{
						float f = ((float)i / (float)num + num2) * 3.1415927f * 2f;
						float num3 = Mathf.Cos(f);
						float num4 = Mathf.Sin(f);
						Vector2 vector = PlayerDashboardInformationUI.ProjectWorldPositionToMap(LevelManager.arenaTargetCenter + new Vector3(num3 * LevelManager.arenaTargetRadius, 0f, num4 * LevelManager.arenaTargetRadius));
						ISleekImage sleekImage;
						if (i < PlayerDashboardInformationUI.arenaTargetPoints.Count)
						{
							sleekImage = PlayerDashboardInformationUI.arenaTargetPoints[i];
							sleekImage.IsVisible = true;
						}
						else
						{
							sleekImage = Glazier.Get().CreateImage(GlazierResources.PixelTexture);
							sleekImage.SizeOffset_X = 2f;
							sleekImage.SizeOffset_Y = 2f;
							sleekImage.TintColor = new Color(1f, 1f, 0f, 1f);
							PlayerDashboardInformationUI.arenaTargetPoints.Add(sleekImage);
							PlayerDashboardInformationUI.mapArenaContainer.AddChild(sleekImage);
						}
						sleekImage.PositionScale_X = vector.x;
						sleekImage.PositionScale_Y = vector.y;
					}
				}
			}
			for (int j = PlayerDashboardInformationUI.arenaTargetPoints.Count - 1; j >= num; j--)
			{
				PlayerDashboardInformationUI.arenaTargetPoints[j].IsVisible = false;
			}
			Vector2 vector2 = PlayerDashboardInformationUI.ProjectWorldPositionToMap(LevelManager.arenaCurrentCenter);
			float num5 = (float)Level.size - (float)Level.border * 2f;
			float num6 = LevelManager.arenaCurrentRadius / num5;
			float num7 = num6 * 2f;
			PlayerDashboardInformationUI.arenaAreaCurrentOverlay.PositionScale_X = vector2.x - num6;
			PlayerDashboardInformationUI.arenaAreaCurrentOverlay.PositionScale_Y = vector2.y - num6;
			PlayerDashboardInformationUI.arenaAreaCurrentOverlay.SizeScale_X = num7;
			PlayerDashboardInformationUI.arenaAreaCurrentOverlay.SizeScale_Y = num7;
			PlayerDashboardInformationUI.arenaAreaCurrentLeftOverlay.PositionScale_Y = PlayerDashboardInformationUI.arenaAreaCurrentOverlay.PositionScale_Y;
			PlayerDashboardInformationUI.arenaAreaCurrentLeftOverlay.SizeScale_X = PlayerDashboardInformationUI.arenaAreaCurrentOverlay.PositionScale_X;
			PlayerDashboardInformationUI.arenaAreaCurrentLeftOverlay.SizeScale_Y = PlayerDashboardInformationUI.arenaAreaCurrentOverlay.SizeScale_Y;
			PlayerDashboardInformationUI.arenaAreaCurrentRightOverlay.PositionScale_X = PlayerDashboardInformationUI.arenaAreaCurrentOverlay.PositionScale_X + PlayerDashboardInformationUI.arenaAreaCurrentOverlay.SizeScale_X;
			PlayerDashboardInformationUI.arenaAreaCurrentRightOverlay.PositionScale_Y = PlayerDashboardInformationUI.arenaAreaCurrentOverlay.PositionScale_Y;
			PlayerDashboardInformationUI.arenaAreaCurrentRightOverlay.SizeScale_X = 1f - PlayerDashboardInformationUI.arenaAreaCurrentOverlay.PositionScale_X - PlayerDashboardInformationUI.arenaAreaCurrentOverlay.SizeScale_X;
			PlayerDashboardInformationUI.arenaAreaCurrentRightOverlay.SizeScale_Y = PlayerDashboardInformationUI.arenaAreaCurrentOverlay.SizeScale_Y;
			PlayerDashboardInformationUI.arenaAreaCurrentUpOverlay.SizeScale_Y = PlayerDashboardInformationUI.arenaAreaCurrentOverlay.PositionScale_Y;
			PlayerDashboardInformationUI.arenaAreaCurrentDownOverlay.PositionScale_Y = PlayerDashboardInformationUI.arenaAreaCurrentOverlay.PositionScale_Y + PlayerDashboardInformationUI.arenaAreaCurrentOverlay.SizeScale_Y;
			PlayerDashboardInformationUI.arenaAreaCurrentDownOverlay.SizeScale_Y = 1f - PlayerDashboardInformationUI.arenaAreaCurrentOverlay.PositionScale_Y - PlayerDashboardInformationUI.arenaAreaCurrentOverlay.SizeScale_Y;
		}

		// Token: 0x060042D5 RID: 17109 RVA: 0x001729E4 File Offset: 0x00170BE4
		private static void updateRemotePlayerAvatars()
		{
			int num = 0;
			bool areSpecStatsVisible = Player.player.look.areSpecStatsVisible;
			foreach (SteamPlayer steamPlayer in Provider.clients)
			{
				if (!(steamPlayer.model == null) && !(steamPlayer.playerID.steamID == Provider.client))
				{
					bool flag = steamPlayer.player.quests.isMemberOfSameGroupAs(Player.player);
					if (areSpecStatsVisible || flag)
					{
						ISleekImage sleekImage;
						if (num < PlayerDashboardInformationUI.remotePlayerImages.Count)
						{
							sleekImage = PlayerDashboardInformationUI.remotePlayerImages[num];
							sleekImage.IsVisible = true;
						}
						else
						{
							sleekImage = Glazier.Get().CreateImage();
							sleekImage.PositionOffset_X = -10f;
							sleekImage.PositionOffset_Y = -10f;
							sleekImage.SizeOffset_X = 20f;
							sleekImage.SizeOffset_Y = 20f;
							sleekImage.AddLabel(string.Empty, 1);
							PlayerDashboardInformationUI.mapRemotePlayersContainer.AddChild(sleekImage);
							PlayerDashboardInformationUI.remotePlayerImages.Add(sleekImage);
						}
						num++;
						Vector2 vector = PlayerDashboardInformationUI.ProjectWorldPositionToMap(steamPlayer.player.transform.position);
						sleekImage.PositionScale_X = vector.x;
						sleekImage.PositionScale_Y = vector.y;
						if (OptionsSettings.streamer || !PlayerDashboardInformationUI.showPlayerAvatarsToggle.Value)
						{
							sleekImage.Texture = PlayerDashboardInformationUI.icons.load<Texture2D>("RemotePlayer");
							sleekImage.TintColor = steamPlayer.markerColor;
							sleekImage.SizeOffset_X = 4f;
							sleekImage.SizeOffset_Y = 4f;
						}
						else
						{
							sleekImage.Texture = Provider.provider.communityService.getIcon(steamPlayer.playerID.steamID, true);
							sleekImage.TintColor = Color.white;
							sleekImage.SizeOffset_X = 20f;
							sleekImage.SizeOffset_Y = 20f;
						}
						sleekImage.PositionOffset_X = sleekImage.SizeOffset_X / -2f;
						sleekImage.PositionOffset_Y = sleekImage.SizeOffset_Y / -2f;
						if (PlayerDashboardInformationUI.showPlayerNamesToggle.Value)
						{
							if (flag && !string.IsNullOrEmpty(steamPlayer.playerID.nickName))
							{
								sleekImage.UpdateLabel(steamPlayer.playerID.nickName);
							}
							else
							{
								sleekImage.UpdateLabel(steamPlayer.playerID.characterName);
							}
						}
						else
						{
							sleekImage.UpdateLabel(string.Empty);
						}
					}
				}
			}
			for (int i = PlayerDashboardInformationUI.remotePlayerImages.Count - 1; i >= num; i--)
			{
				PlayerDashboardInformationUI.remotePlayerImages[i].IsVisible = false;
			}
		}

		// Token: 0x060042D6 RID: 17110 RVA: 0x00172CA8 File Offset: 0x00170EA8
		public static void updateDynamicMap()
		{
			if (PlayerDashboardInformationUI.mapMarkersContainer.IsVisible)
			{
				PlayerDashboardInformationUI.updateMarkers();
			}
			if (PlayerDashboardInformationUI.mapArenaContainer.IsVisible)
			{
				PlayerDashboardInformationUI.updateArenaCircle();
			}
			if (PlayerDashboardInformationUI.mapRemotePlayersContainer.IsVisible)
			{
				PlayerDashboardInformationUI.updateRemotePlayerAvatars();
			}
			if (PlayerDashboardInformationUI.localPlayerImage.IsVisible && Player.player != null)
			{
				Vector2 vector = PlayerDashboardInformationUI.ProjectWorldPositionToMap(Player.player.transform.position);
				PlayerDashboardInformationUI.localPlayerImage.PositionScale_X = vector.x;
				PlayerDashboardInformationUI.localPlayerImage.PositionScale_Y = vector.y;
				PlayerDashboardInformationUI.localPlayerImage.RotationAngle = PlayerDashboardInformationUI.ProjectWorldRotationToMap(Player.player.transform.rotation.eulerAngles.y);
			}
		}

		// Token: 0x060042D7 RID: 17111 RVA: 0x00172D64 File Offset: 0x00170F64
		protected static void searchForMapsInInventory(ref bool enableChart, ref bool enableMap)
		{
			if (enableChart & enableMap)
			{
				return;
			}
			for (byte b = 0; b < PlayerInventory.PAGES - 2; b += 1)
			{
				Items items = Player.player.inventory.items[(int)b];
				if (items != null)
				{
					foreach (ItemJar itemJar in items.items)
					{
						if (itemJar != null)
						{
							ItemMapAsset asset = itemJar.GetAsset<ItemMapAsset>();
							if (asset != null)
							{
								enableChart |= asset.enablesChart;
								enableMap |= asset.enablesMap;
							}
							if (enableChart & enableMap)
							{
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x060042D8 RID: 17112 RVA: 0x00172E14 File Offset: 0x00171014
		public static void open()
		{
			if (PlayerDashboardInformationUI.active)
			{
				return;
			}
			PlayerDashboardInformationUI.active = true;
			PlayerDashboardInformationUI.hasChart = (Provider.modeConfigData.Gameplay.Chart || Level.info.type > ELevelType.SURVIVAL);
			PlayerDashboardInformationUI.hasGPS = (Provider.modeConfigData.Gameplay.Satellite || Level.info.type > ELevelType.SURVIVAL);
			PlayerDashboardInformationUI.searchForMapsInInventory(ref PlayerDashboardInformationUI.hasChart, ref PlayerDashboardInformationUI.hasGPS);
			if (PlayerDashboardInformationUI.hasChart && !PlayerDashboardInformationUI.hasGPS)
			{
				PlayerDashboardInformationUI.mapButtonState.state = 0;
			}
			if (PlayerDashboardInformationUI.hasGPS && !PlayerDashboardInformationUI.hasChart)
			{
				PlayerDashboardInformationUI.mapButtonState.state = 1;
			}
			PlayerDashboardInformationUI.synchronizeMapVisibility(PlayerDashboardInformationUI.mapButtonState.state);
			PlayerDashboardInformationUI.updateDynamicMap();
			PlayerDashboardInformationUI.questsButton.text = PlayerDashboardInformationUI.localization.format("Quests", Player.player.quests.countValidQuests());
			if (OptionsSettings.streamer)
			{
				PlayerDashboardInformationUI.playersButton.text = PlayerDashboardInformationUI.localization.format("Streamer");
			}
			else
			{
				PlayerDashboardInformationUI.playersButton.text = PlayerDashboardInformationUI.localization.format("Players", Provider.clients.Count, Provider.maxPlayers);
			}
			switch (PlayerDashboardInformationUI.tab)
			{
			case PlayerDashboardInformationUI.EInfoTab.QUESTS:
				PlayerDashboardInformationUI.openQuests();
				break;
			case PlayerDashboardInformationUI.EInfoTab.GROUPS:
				PlayerDashboardInformationUI.openGroups();
				break;
			case PlayerDashboardInformationUI.EInfoTab.PLAYERS:
				PlayerDashboardInformationUI.openPlayers();
				break;
			}
			PlayerDashboardInformationUI.container.AnimateIntoView();
		}

		// Token: 0x060042D9 RID: 17113 RVA: 0x00172F88 File Offset: 0x00171188
		public static void close()
		{
			if (!PlayerDashboardInformationUI.active)
			{
				return;
			}
			PlayerDashboardInformationUI.active = false;
			PlayerDashboardInformationUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x060042DA RID: 17114 RVA: 0x00172FAC File Offset: 0x001711AC
		public static void openQuests()
		{
			PlayerDashboardInformationUI.tab = PlayerDashboardInformationUI.EInfoTab.QUESTS;
			PlayerDashboardInformationUI.questsBox.RemoveAllChildren();
			PlayerDashboardInformationUI.displayedQuests.Clear();
			float num = 0f;
			foreach (PlayerQuest playerQuest in Player.player.quests.questsList)
			{
				if (playerQuest != null && playerQuest.asset != null)
				{
					PlayerDashboardInformationUI.displayedQuests.Add(playerQuest);
					bool flag = playerQuest.asset.areConditionsMet(Player.player);
					ISleekButton sleekButton = Glazier.Get().CreateButton();
					sleekButton.PositionOffset_Y = num;
					sleekButton.SizeOffset_Y = 50f;
					sleekButton.SizeScale_X = 1f;
					sleekButton.OnClicked += new ClickedButton(PlayerDashboardInformationUI.onClickedQuestButton);
					PlayerDashboardInformationUI.questsBox.AddChild(sleekButton);
					ISleekImage sleekImage = Glazier.Get().CreateImage(PlayerDashboardInformationUI.icons.load<Texture2D>(flag ? "Complete" : "Incomplete"));
					sleekImage.PositionOffset_X = 5f;
					sleekImage.PositionOffset_Y = 5f;
					sleekImage.SizeOffset_X = 40f;
					sleekImage.SizeOffset_Y = 40f;
					sleekButton.AddChild(sleekImage);
					ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
					sleekLabel.PositionOffset_X = 50f;
					sleekLabel.SizeOffset_X = -55f;
					sleekLabel.SizeScale_X = 1f;
					sleekLabel.SizeScale_Y = 1f;
					sleekLabel.TextAlignment = 3;
					sleekLabel.TextColor = 4;
					sleekLabel.TextContrastContext = 1;
					sleekLabel.AllowRichText = true;
					sleekLabel.FontSize = 3;
					sleekLabel.Text = playerQuest.asset.questName;
					sleekButton.AddChild(sleekLabel);
					num += sleekButton.SizeOffset_Y + 10f;
				}
			}
			PlayerDashboardInformationUI.questsBox.ContentSizeOffset = new Vector2(0f, num - 10f);
			PlayerDashboardInformationUI.updateTabs();
		}

		// Token: 0x060042DB RID: 17115 RVA: 0x001731C0 File Offset: 0x001713C0
		private static void onClickedTuneButton(ISleekElement button)
		{
			uint num = (uint)(PlayerDashboardInformationUI.radioFrequencyField.Value * 1000.0);
			if (num < 300000U)
			{
				num = 300000U;
			}
			else if (num > 900000U)
			{
				num = 900000U;
			}
			PlayerDashboardInformationUI.radioFrequencyField.Value = num / 1000.0;
			Player.player.quests.sendSetRadioFrequency(num);
		}

		// Token: 0x060042DC RID: 17116 RVA: 0x00173228 File Offset: 0x00171428
		private static void onClickedResetButton(ISleekElement button)
		{
			PlayerDashboardInformationUI.radioFrequencyField.Value = PlayerQuests.DEFAULT_RADIO_FREQUENCY / 1000.0;
			PlayerDashboardInformationUI.onClickedTuneButton(button);
		}

		// Token: 0x060042DD RID: 17117 RVA: 0x0017324B File Offset: 0x0017144B
		private static void onClickedRenameButton(ISleekElement button)
		{
			Player.player.quests.sendRenameGroup(PlayerDashboardInformationUI.groupNameField.Text);
		}

		// Token: 0x060042DE RID: 17118 RVA: 0x00173266 File Offset: 0x00171466
		private static void onClickedMainGroupButton(ISleekElement button)
		{
			Player.player.quests.SendAcceptGroupInvitation(Characters.active.group);
		}

		// Token: 0x060042DF RID: 17119 RVA: 0x00173281 File Offset: 0x00171481
		private static void onClickedLeaveGroupButton(ISleekElement button)
		{
			Player.player.quests.sendLeaveGroup();
		}

		// Token: 0x060042E0 RID: 17120 RVA: 0x00173292 File Offset: 0x00171492
		private static void onClickedDeleteGroupButton(SleekButtonIconConfirm button)
		{
			Player.player.quests.sendDeleteGroup();
		}

		// Token: 0x060042E1 RID: 17121 RVA: 0x001732A3 File Offset: 0x001714A3
		private static void onClickedCreateGroupButton(ISleekElement button)
		{
			Player.player.quests.sendCreateGroup();
		}

		// Token: 0x060042E2 RID: 17122 RVA: 0x001732B4 File Offset: 0x001714B4
		private static void refreshGroups()
		{
			if (!PlayerDashboardInformationUI.active)
			{
				return;
			}
			PlayerDashboardInformationUI.groupsBox.RemoveAllChildren();
			int num = 0;
			ISleekBox sleekBox = Glazier.Get().CreateBox();
			sleekBox.PositionOffset_Y = (float)num;
			sleekBox.SizeOffset_X = 125f;
			sleekBox.SizeOffset_Y = 30f;
			sleekBox.Text = PlayerDashboardInformationUI.localization.format("Radio_Frequency_Label");
			PlayerDashboardInformationUI.groupsBox.AddChild(sleekBox);
			PlayerDashboardInformationUI.radioFrequencyField = Glazier.Get().CreateFloat64Field();
			PlayerDashboardInformationUI.radioFrequencyField.PositionOffset_X = 125f;
			PlayerDashboardInformationUI.radioFrequencyField.SizeOffset_X = -225f;
			PlayerDashboardInformationUI.radioFrequencyField.PositionOffset_Y = (float)num;
			PlayerDashboardInformationUI.radioFrequencyField.SizeOffset_Y = 30f;
			PlayerDashboardInformationUI.radioFrequencyField.SizeScale_X = 1f;
			PlayerDashboardInformationUI.radioFrequencyField.Value = Player.player.quests.radioFrequency / 1000.0;
			PlayerDashboardInformationUI.groupsBox.AddChild(PlayerDashboardInformationUI.radioFrequencyField);
			ISleekButton sleekButton = Glazier.Get().CreateButton();
			sleekButton.PositionOffset_X = -100f;
			sleekButton.PositionScale_X = 1f;
			sleekButton.SizeOffset_X = 50f;
			sleekButton.SizeOffset_Y = 30f;
			sleekButton.Text = PlayerDashboardInformationUI.localization.format("Radio_Frequency_Tune");
			sleekButton.TooltipText = PlayerDashboardInformationUI.localization.format("Radio_Frequency_Tune_Tooltip");
			sleekButton.OnClicked += new ClickedButton(PlayerDashboardInformationUI.onClickedTuneButton);
			PlayerDashboardInformationUI.groupsBox.AddChild(sleekButton);
			ISleekButton sleekButton2 = Glazier.Get().CreateButton();
			sleekButton2.PositionOffset_X = -50f;
			sleekButton2.PositionScale_X = 1f;
			sleekButton2.SizeOffset_X = 50f;
			sleekButton2.SizeOffset_Y = 30f;
			sleekButton2.Text = PlayerDashboardInformationUI.localization.format("Radio_Frequency_Reset");
			sleekButton2.TooltipText = PlayerDashboardInformationUI.localization.format("Radio_Frequency_Reset_Tooltip");
			sleekButton2.OnClicked += new ClickedButton(PlayerDashboardInformationUI.onClickedResetButton);
			PlayerDashboardInformationUI.groupsBox.AddChild(sleekButton2);
			num += 30;
			PlayerQuests quests = Player.player.quests;
			if (quests.isMemberOfAGroup)
			{
				if (Characters.active.group == quests.groupID)
				{
					SteamGroup cachedGroup = Provider.provider.communityService.getCachedGroup(Characters.active.group);
					if (cachedGroup != null)
					{
						SleekBoxIcon sleekBoxIcon = new SleekBoxIcon(cachedGroup.icon, 40);
						sleekBoxIcon.PositionOffset_Y = (float)num;
						sleekBoxIcon.SizeOffset_Y = 50f;
						sleekBoxIcon.SizeScale_X = 1f;
						sleekBoxIcon.text = cachedGroup.name;
						PlayerDashboardInformationUI.groupsBox.AddChild(sleekBoxIcon);
						num += 50;
					}
				}
				else
				{
					GroupInfo groupInfo = GroupManager.getGroupInfo(quests.groupID);
					string text = (groupInfo != null) ? groupInfo.name : quests.groupID.ToString();
					if (quests.groupRank == EPlayerGroupRank.OWNER)
					{
						PlayerDashboardInformationUI.groupNameField = Glazier.Get().CreateStringField();
						PlayerDashboardInformationUI.groupNameField.PositionOffset_Y = (float)num;
						PlayerDashboardInformationUI.groupNameField.MaxLength = 32;
						PlayerDashboardInformationUI.groupNameField.Text = text;
						PlayerDashboardInformationUI.groupNameField.SizeOffset_X = -100f;
						PlayerDashboardInformationUI.groupNameField.SizeOffset_Y = 30f;
						PlayerDashboardInformationUI.groupNameField.SizeScale_X = 1f;
						PlayerDashboardInformationUI.groupsBox.AddChild(PlayerDashboardInformationUI.groupNameField);
						ISleekButton sleekButton3 = Glazier.Get().CreateButton();
						sleekButton3.PositionScale_X = 1f;
						sleekButton3.PositionOffset_X = -100f;
						sleekButton3.PositionOffset_Y = (float)num;
						sleekButton3.SizeOffset_X = 100f;
						sleekButton3.SizeOffset_Y = 30f;
						sleekButton3.Text = PlayerDashboardInformationUI.localization.format("Group_Rename");
						sleekButton3.TooltipText = PlayerDashboardInformationUI.localization.format("Group_Rename_Tooltip");
						sleekButton3.OnClicked += new ClickedButton(PlayerDashboardInformationUI.onClickedRenameButton);
						PlayerDashboardInformationUI.groupsBox.AddChild(sleekButton3);
					}
					else
					{
						ISleekBox sleekBox2 = Glazier.Get().CreateBox();
						sleekBox2.PositionOffset_Y = (float)num;
						sleekBox2.SizeOffset_Y = 30f;
						sleekBox2.SizeScale_X = 1f;
						sleekBox2.Text = text;
						PlayerDashboardInformationUI.groupsBox.AddChild(sleekBox2);
					}
					num += 30;
					if (quests.useMaxGroupMembersLimit)
					{
						ISleekBox sleekBox3 = Glazier.Get().CreateBox();
						sleekBox3.PositionOffset_Y = (float)num;
						sleekBox3.SizeOffset_Y = 30f;
						sleekBox3.SizeScale_X = 1f;
						sleekBox3.Text = PlayerDashboardInformationUI.localization.format("Group_Members", groupInfo.members, Provider.modeConfigData.Gameplay.Max_Group_Members);
						PlayerDashboardInformationUI.groupsBox.AddChild(sleekBox3);
						num += 30;
					}
				}
				if (quests.hasPermissionToLeaveGroup)
				{
					SleekButtonIcon sleekButtonIcon = new SleekButtonIcon(MenuWorkshopEditorUI.icons.load<Texture2D>("Remove"));
					sleekButtonIcon.PositionOffset_Y = (float)num;
					sleekButtonIcon.SizeOffset_Y = 30f;
					sleekButtonIcon.SizeScale_X = 1f;
					sleekButtonIcon.text = PlayerDashboardInformationUI.localization.format("Group_Leave");
					sleekButtonIcon.tooltip = PlayerDashboardInformationUI.localization.format("Group_Leave_Tooltip");
					sleekButtonIcon.onClickedButton += new ClickedButton(PlayerDashboardInformationUI.onClickedLeaveGroupButton);
					PlayerDashboardInformationUI.groupsBox.AddChild(sleekButtonIcon);
					num += 30;
				}
				if (quests.hasPermissionToDeleteGroup)
				{
					SleekButtonIconConfirm sleekButtonIconConfirm = new SleekButtonIconConfirm(MenuWorkshopEditorUI.icons.load<Texture2D>("Remove"), PlayerDashboardInformationUI.localization.format("Group_Delete_Confirm"), PlayerDashboardInformationUI.localization.format("Group_Delete_Confirm_Tooltip"), PlayerDashboardInformationUI.localization.format("Group_Delete_Deny"), PlayerDashboardInformationUI.localization.format("Group_Delete_Deny_Tooltip"));
					sleekButtonIconConfirm.PositionOffset_Y = (float)num;
					sleekButtonIconConfirm.SizeOffset_Y = 30f;
					sleekButtonIconConfirm.SizeScale_X = 1f;
					sleekButtonIconConfirm.text = PlayerDashboardInformationUI.localization.format("Group_Delete");
					sleekButtonIconConfirm.tooltip = PlayerDashboardInformationUI.localization.format("Group_Delete_Tooltip");
					SleekButtonIconConfirm sleekButtonIconConfirm2 = sleekButtonIconConfirm;
					sleekButtonIconConfirm2.onConfirmed = (Confirm)Delegate.Combine(sleekButtonIconConfirm2.onConfirmed, new Confirm(PlayerDashboardInformationUI.onClickedDeleteGroupButton));
					PlayerDashboardInformationUI.groupsBox.AddChild(sleekButtonIconConfirm);
					num += 30;
				}
				using (List<SteamPlayer>.Enumerator enumerator = Provider.clients.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						SteamPlayer steamPlayer = enumerator.Current;
						if (!(steamPlayer.player == null) && steamPlayer.player.quests.isMemberOfSameGroupAs(Player.player))
						{
							SleekPlayer sleekPlayer = new SleekPlayer(steamPlayer, true, SleekPlayer.ESleekPlayerDisplayContext.GROUP_ROSTER);
							sleekPlayer.PositionOffset_Y = (float)num;
							sleekPlayer.SizeOffset_Y = 50f;
							sleekPlayer.SizeScale_X = 1f;
							PlayerDashboardInformationUI.groupsBox.AddChild(sleekPlayer);
							num += 50;
						}
					}
					goto IL_851;
				}
			}
			if (Characters.active.group != CSteamID.Nil && Provider.modeConfigData.Gameplay.Allow_Static_Groups)
			{
				SteamGroup cachedGroup2 = Provider.provider.communityService.getCachedGroup(Characters.active.group);
				if (cachedGroup2 != null)
				{
					SleekButtonIcon sleekButtonIcon2 = new SleekButtonIcon(cachedGroup2.icon, 40);
					sleekButtonIcon2.PositionOffset_Y = (float)num;
					sleekButtonIcon2.SizeOffset_Y = 50f;
					sleekButtonIcon2.SizeScale_X = 1f;
					sleekButtonIcon2.text = cachedGroup2.name;
					sleekButtonIcon2.onClickedButton += new ClickedButton(PlayerDashboardInformationUI.onClickedMainGroupButton);
					PlayerDashboardInformationUI.groupsBox.AddChild(sleekButtonIcon2);
					num += 50;
				}
			}
			foreach (CSteamID newGroupID in quests.groupInvites)
			{
				PlayerDashboardInformationUI.SleekInviteButton sleekInviteButton = new PlayerDashboardInformationUI.SleekInviteButton(newGroupID);
				sleekInviteButton.PositionOffset_Y = (float)num;
				sleekInviteButton.SizeOffset_Y = 30f;
				sleekInviteButton.SizeScale_X = 1f;
				PlayerDashboardInformationUI.groupsBox.AddChild(sleekInviteButton);
				num += 30;
			}
			if (Player.player.quests.hasPermissionToCreateGroup)
			{
				SleekButtonIcon sleekButtonIcon3 = new SleekButtonIcon(MenuWorkshopEditorUI.icons.load<Texture2D>("Add"));
				sleekButtonIcon3.PositionOffset_Y = (float)num;
				sleekButtonIcon3.SizeOffset_Y = 30f;
				sleekButtonIcon3.SizeScale_X = 1f;
				sleekButtonIcon3.text = PlayerDashboardInformationUI.localization.format("Group_Create");
				sleekButtonIcon3.tooltip = PlayerDashboardInformationUI.localization.format("Group_Create_Tooltip");
				sleekButtonIcon3.onClickedButton += new ClickedButton(PlayerDashboardInformationUI.onClickedCreateGroupButton);
				PlayerDashboardInformationUI.groupsBox.AddChild(sleekButtonIcon3);
				num += 30;
			}
			IL_851:
			PlayerDashboardInformationUI.groupsBox.ContentSizeOffset = new Vector2(0f, (float)num);
		}

		// Token: 0x060042E3 RID: 17123 RVA: 0x00173B44 File Offset: 0x00171D44
		private static void handleGroupUpdated(PlayerQuests sender)
		{
			PlayerDashboardInformationUI.refreshGroups();
		}

		// Token: 0x060042E4 RID: 17124 RVA: 0x00173B4B File Offset: 0x00171D4B
		private static void handleGroupInfoReady(GroupInfo group)
		{
			PlayerDashboardInformationUI.refreshGroups();
		}

		// Token: 0x060042E5 RID: 17125 RVA: 0x00173B52 File Offset: 0x00171D52
		public static void openGroups()
		{
			PlayerDashboardInformationUI.tab = PlayerDashboardInformationUI.EInfoTab.GROUPS;
			PlayerDashboardInformationUI.refreshGroups();
			PlayerDashboardInformationUI.updateTabs();
		}

		// Token: 0x060042E6 RID: 17126 RVA: 0x00173B64 File Offset: 0x00171D64
		public static void openPlayers()
		{
			PlayerDashboardInformationUI.tab = PlayerDashboardInformationUI.EInfoTab.PLAYERS;
			PlayerDashboardInformationUI.SortAndRebuildPlayers();
			PlayerDashboardInformationUI.updateTabs();
		}

		// Token: 0x060042E7 RID: 17127 RVA: 0x00173B78 File Offset: 0x00171D78
		private static void SortAndRebuildPlayers()
		{
			PlayerDashboardInformationUI.sortedClients.Clear();
			PlayerDashboardInformationUI.sortedClients.AddRange(Provider.clients);
			int num;
			if (Provider.modeConfigData.Gameplay.Group_Player_List)
			{
				num = PlayerDashboardInformationUI.playerSortButton.state;
			}
			else
			{
				num = 0;
			}
			if (num == 0)
			{
				PlayerDashboardInformationUI.playersList.onCreateElement = new SleekList<SteamPlayer>.CreateElement(PlayerDashboardInformationUI.OnCreatePlayerEntry);
				PlayerDashboardInformationUI.sortedClients.Sort((SteamPlayer lhs, SteamPlayer rhs) => lhs.GetLocalDisplayName().CompareTo(rhs.GetLocalDisplayName()));
			}
			else
			{
				PlayerDashboardInformationUI.playersList.onCreateElement = new SleekList<SteamPlayer>.CreateElement(PlayerDashboardInformationUI.OnCreatePlayerEntryWithGrouping);
				PlayerDashboardInformationUI.sortedClients.Sort(delegate(SteamPlayer lhs, SteamPlayer rhs)
				{
					int num2 = lhs.player.quests.groupID.CompareTo(rhs.player.quests.groupID);
					if (num2 != 0)
					{
						return num2;
					}
					return lhs.GetLocalDisplayName().CompareTo(rhs.GetLocalDisplayName());
				});
			}
			PlayerDashboardInformationUI.playersList.ForceRebuildElements();
		}

		// Token: 0x060042E8 RID: 17128 RVA: 0x00173C4B File Offset: 0x00171E4B
		private static void updateTabs()
		{
			PlayerDashboardInformationUI.questsBox.IsVisible = (PlayerDashboardInformationUI.tab == PlayerDashboardInformationUI.EInfoTab.QUESTS);
			PlayerDashboardInformationUI.groupsBox.IsVisible = (PlayerDashboardInformationUI.tab == PlayerDashboardInformationUI.EInfoTab.GROUPS);
			PlayerDashboardInformationUI.playersBox.IsVisible = (PlayerDashboardInformationUI.tab == PlayerDashboardInformationUI.EInfoTab.PLAYERS);
		}

		// Token: 0x060042E9 RID: 17129 RVA: 0x00173C83 File Offset: 0x00171E83
		private static void updateZoom()
		{
			PlayerDashboardInformationUI.mapBox.ContentScaleFactor = (float)PlayerDashboardInformationUI.zoomMultiplier;
		}

		// Token: 0x060042EA RID: 17130 RVA: 0x00173C98 File Offset: 0x00171E98
		public static void focusPoint(Vector3 point)
		{
			Vector2 normalizedStateCenter = PlayerDashboardInformationUI.ProjectWorldPositionToMap(point);
			PlayerDashboardInformationUI.mapBox.NormalizedStateCenter = normalizedStateCenter;
		}

		// Token: 0x060042EB RID: 17131 RVA: 0x00173CB8 File Offset: 0x00171EB8
		private static float ProjectWorldRotationToMap(float yaw)
		{
			CartographyVolume mainVolume = VolumeManager<CartographyVolume, CartographyVolumeManager>.Get().GetMainVolume();
			if (mainVolume != null)
			{
				return yaw - mainVolume.transform.eulerAngles.y;
			}
			return yaw;
		}

		/// <summary>
		/// Convert level-space 3D position into normalized 2D position.
		/// </summary>
		// Token: 0x060042EC RID: 17132 RVA: 0x00173CF0 File Offset: 0x00171EF0
		private static Vector2 ProjectWorldPositionToMap(Vector3 worldPosition)
		{
			CartographyVolume mainVolume = VolumeManager<CartographyVolume, CartographyVolumeManager>.Get().GetMainVolume();
			if (mainVolume != null)
			{
				Vector3 vector = mainVolume.transform.InverseTransformPoint(worldPosition);
				return new Vector2(vector.x + 0.5f, 0.5f - vector.z);
			}
			float num = (float)Level.size - (float)Level.border * 2f;
			return new Vector2(worldPosition.x / num + 0.5f, 0.5f - worldPosition.z / num);
		}

		/// <summary>
		/// Convert normalized 2D position into level-space 3D position.
		/// </summary>
		// Token: 0x060042ED RID: 17133 RVA: 0x00173D74 File Offset: 0x00171F74
		private static Vector3 DeprojectMapToWorld(Vector2 mapPosition)
		{
			CartographyVolume mainVolume = VolumeManager<CartographyVolume, CartographyVolumeManager>.Get().GetMainVolume();
			if (mainVolume != null)
			{
				Vector3 position = new Vector3(mapPosition.x - 0.5f, 0f, 0.5f - mapPosition.y);
				return mainVolume.transform.TransformPoint(position).GetHorizontal();
			}
			float num = (float)Level.size - (float)Level.border * 2f;
			return new Vector3((mapPosition.x - 0.5f) * num, 0f, (0.5f - mapPosition.y) * num);
		}

		// Token: 0x060042EE RID: 17134 RVA: 0x00173E08 File Offset: 0x00172008
		private static void onRightClickedMap()
		{
			Vector2 normalizedCursorPosition = PlayerDashboardInformationUI.mapImage.GetNormalizedCursorPosition();
			Vector3 newMarkerPosition = PlayerDashboardInformationUI.DeprojectMapToWorld(normalizedCursorPosition);
			PlayerQuests quests = Player.player.quests;
			bool newIsMarkerPlaced = !quests.isMarkerPlaced || Vector2.Distance(PlayerDashboardInformationUI.ProjectWorldPositionToMap(quests.markerPosition), normalizedCursorPosition) * PlayerDashboardInformationUI.mapBox.ContentSizeOffset.x > 15f;
			quests.sendSetMarker(newIsMarkerPlaced, newMarkerPosition);
		}

		// Token: 0x060042EF RID: 17135 RVA: 0x00173E6F File Offset: 0x0017206F
		private static void OnShowMarkersToggled(ISleekToggle toggle, bool value)
		{
			PlayerDashboardInformationUI.synchronizeMapVisibility(PlayerDashboardInformationUI.mapButtonState.state);
			PlayerDashboardInformationUI.updateDynamicMap();
		}

		// Token: 0x060042F0 RID: 17136 RVA: 0x00173E88 File Offset: 0x00172088
		private static void onClickedZoomInButton(ISleekElement button)
		{
			if (PlayerDashboardInformationUI.zoomMultiplier < PlayerDashboardInformationUI.maxZoomMultiplier)
			{
				PlayerDashboardInformationUI.zoomMultiplier++;
				Vector2 normalizedStateCenter = PlayerDashboardInformationUI.mapBox.NormalizedStateCenter;
				PlayerDashboardInformationUI.updateZoom();
				PlayerDashboardInformationUI.mapBox.NormalizedStateCenter = normalizedStateCenter;
			}
		}

		// Token: 0x060042F1 RID: 17137 RVA: 0x00173EC8 File Offset: 0x001720C8
		private static void onClickedZoomOutButton(ISleekElement button)
		{
			if (PlayerDashboardInformationUI.zoomMultiplier > 1)
			{
				PlayerDashboardInformationUI.zoomMultiplier--;
				Vector2 normalizedStateCenter = PlayerDashboardInformationUI.mapBox.NormalizedStateCenter;
				PlayerDashboardInformationUI.updateZoom();
				PlayerDashboardInformationUI.mapBox.NormalizedStateCenter = normalizedStateCenter;
			}
		}

		// Token: 0x060042F2 RID: 17138 RVA: 0x00173F04 File Offset: 0x00172104
		private static void onClickedCenterButton(ISleekElement button)
		{
			PlayerDashboardInformationUI.focusPoint(Player.player.transform.position);
		}

		// Token: 0x060042F3 RID: 17139 RVA: 0x00173F1A File Offset: 0x0017211A
		private static void onSwappedMapState(SleekButtonState button, int index)
		{
			PlayerDashboardInformationUI.synchronizeMapVisibility(index);
			PlayerDashboardInformationUI.updateDynamicMap();
		}

		// Token: 0x060042F4 RID: 17140 RVA: 0x00173F28 File Offset: 0x00172128
		private static void onClickedQuestButton(ISleekElement button)
		{
			int num = PlayerDashboardInformationUI.questsBox.FindIndexOfChild(button);
			if (num < 0 || num >= PlayerDashboardInformationUI.displayedQuests.Count)
			{
				UnturnedLog.warn("Cannot find clicked quest");
				return;
			}
			PlayerQuest playerQuest = PlayerDashboardInformationUI.displayedQuests[num];
			PlayerDashboardUI.close();
			PlayerNPCQuestUI.open(playerQuest.asset, null, null, EQuestViewMode.DETAILS);
		}

		// Token: 0x060042F5 RID: 17141 RVA: 0x00173F7A File Offset: 0x0017217A
		private static void onClickedQuestsButton(ISleekElement button)
		{
			PlayerDashboardInformationUI.openQuests();
		}

		// Token: 0x060042F6 RID: 17142 RVA: 0x00173F81 File Offset: 0x00172181
		private static void onClickedGroupsButton(ISleekElement button)
		{
			PlayerDashboardInformationUI.openGroups();
		}

		// Token: 0x060042F7 RID: 17143 RVA: 0x00173F88 File Offset: 0x00172188
		private static void onClickedPlayersButton(ISleekElement button)
		{
			PlayerDashboardInformationUI.openPlayers();
		}

		// Token: 0x060042F8 RID: 17144 RVA: 0x00173F8F File Offset: 0x0017218F
		private static void handleIsBlindfoldedChanged()
		{
			if (PlayerDashboardInformationUI.active)
			{
				PlayerDashboardInformationUI.synchronizeMapVisibility(PlayerDashboardInformationUI.mapButtonState.state);
				PlayerDashboardInformationUI.updateDynamicMap();
			}
		}

		// Token: 0x060042F9 RID: 17145 RVA: 0x00173FAC File Offset: 0x001721AC
		private static void onPlayerTeleported(Player player, Vector3 point)
		{
			PlayerDashboardInformationUI.focusPoint(point);
		}

		// Token: 0x060042FA RID: 17146 RVA: 0x00173FB4 File Offset: 0x001721B4
		private void createLocationNameLabels()
		{
			LevelInfo info = Level.info;
			Local local = (info != null) ? info.getLocalization() : null;
			foreach (LocationDevkitNode locationDevkitNode in LocationDevkitNodeSystem.Get().GetAllNodes())
			{
				if (locationDevkitNode.isVisibleOnMap)
				{
					string text = locationDevkitNode.locationName;
					if (!string.IsNullOrWhiteSpace(text))
					{
						string key = text.Replace(' ', '_');
						if (local != null && local.has(key))
						{
							text = local.format(key);
						}
						Vector2 vector = PlayerDashboardInformationUI.ProjectWorldPositionToMap(locationDevkitNode.transform.position);
						ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
						sleekLabel.PositionOffset_X = -200f;
						sleekLabel.PositionOffset_Y = -30f;
						sleekLabel.PositionScale_X = vector.x;
						sleekLabel.PositionScale_Y = vector.y;
						sleekLabel.SizeOffset_X = 400f;
						sleekLabel.SizeOffset_Y = 60f;
						sleekLabel.Text = text;
						sleekLabel.TextColor = 3;
						sleekLabel.TextContrastContext = 2;
						PlayerDashboardInformationUI.mapLocationsContainer.AddChild(sleekLabel);
					}
				}
			}
		}

		// Token: 0x060042FB RID: 17147 RVA: 0x001740E8 File Offset: 0x001722E8
		private void OnSwappedPlayerSortState(SleekButtonState state, int index)
		{
			ConvenientSavedata.get().write("PlayerListSortMode", (long)index);
			PlayerDashboardInformationUI.SortAndRebuildPlayers();
		}

		// Token: 0x060042FC RID: 17148 RVA: 0x00174100 File Offset: 0x00172300
		private static ISleekElement OnCreatePlayerEntry(SteamPlayer player)
		{
			return new SleekPlayer(player, true, SleekPlayer.ESleekPlayerDisplayContext.PLAYER_LIST);
		}

		// Token: 0x060042FD RID: 17149 RVA: 0x0017410C File Offset: 0x0017230C
		private static ISleekElement OnCreatePlayerEntryWithGrouping(SteamPlayer player)
		{
			SleekPlayer sleekPlayer = new SleekPlayer(player, true, SleekPlayer.ESleekPlayerDisplayContext.PLAYER_LIST);
			int num = PlayerDashboardInformationUI.playersList.IndexOfCreateElementItem + 1;
			if (num < PlayerDashboardInformationUI.sortedClients.Count && player.player.quests.isMemberOfSameGroupAs(PlayerDashboardInformationUI.sortedClients[num].player))
			{
				ISleekImage sleekImage = Glazier.Get().CreateImage(PlayerDashboardInformationUI.icons.load<Texture2D>("Group"));
				sleekImage.PositionOffset_X = 21f;
				sleekImage.PositionOffset_Y = 47f;
				sleekImage.SizeOffset_X = 8f;
				sleekImage.SizeOffset_Y = 16f;
				sleekImage.TintColor = 2;
				sleekPlayer.AddChild(sleekImage);
			}
			return sleekPlayer;
		}

		// Token: 0x060042FE RID: 17150 RVA: 0x001741B8 File Offset: 0x001723B8
		private static void OnGameplayConfigReceived()
		{
			PlayerDashboardInformationUI.SyncPlayerSortButtonVisible();
		}

		// Token: 0x060042FF RID: 17151 RVA: 0x001741C0 File Offset: 0x001723C0
		private static void SyncPlayerSortButtonVisible()
		{
			bool group_Player_List = Provider.modeConfigData.Gameplay.Group_Player_List;
			PlayerDashboardInformationUI.playerSortButton.IsVisible = group_Player_List;
			PlayerDashboardInformationUI.playersList.PositionOffset_Y = (float)(group_Player_List ? 30 : 0);
			PlayerDashboardInformationUI.playersList.SizeOffset_Y = (float)(group_Player_List ? -30 : 0);
		}

		/// <summary>
		/// Temporary to unbind events because this class is static for now. (sigh)
		/// </summary>
		// Token: 0x06004300 RID: 17152 RVA: 0x00174210 File Offset: 0x00172410
		public void OnDestroy()
		{
			PlayerUI.isBlindfoldedChanged -= PlayerDashboardInformationUI.handleIsBlindfoldedChanged;
			if (Player.player != null)
			{
				Player player = Player.player;
				player.onPlayerTeleported = (PlayerTeleported)Delegate.Remove(player.onPlayerTeleported, new PlayerTeleported(PlayerDashboardInformationUI.onPlayerTeleported));
			}
			PlayerQuests.groupUpdated = (GroupUpdatedHandler)Delegate.Remove(PlayerQuests.groupUpdated, new GroupUpdatedHandler(PlayerDashboardInformationUI.handleGroupUpdated));
			GroupManager.groupInfoReady -= PlayerDashboardInformationUI.handleGroupInfoReady;
			ClientMessageHandler_Accepted.OnGameplayConfigReceived -= new Action(PlayerDashboardInformationUI.OnGameplayConfigReceived);
		}

		// Token: 0x06004301 RID: 17153 RVA: 0x001742A4 File Offset: 0x001724A4
		public PlayerDashboardInformationUI()
		{
			if (PlayerDashboardInformationUI.icons != null)
			{
				PlayerDashboardInformationUI.icons.unload();
			}
			PlayerDashboardInformationUI.localization = Localization.read("/Player/PlayerDashboardInformation.dat");
			PlayerDashboardInformationUI.icons = Bundles.getBundle("/Bundles/Textures/Player/Icons/PlayerDashboardInformation/PlayerDashboardInformation.unity3d");
			PlayerDashboardInformationUI.container = new SleekFullscreenBox();
			PlayerDashboardInformationUI.container.PositionScale_Y = 1f;
			PlayerDashboardInformationUI.container.PositionOffset_X = 10f;
			PlayerDashboardInformationUI.container.PositionOffset_Y = 10f;
			PlayerDashboardInformationUI.container.SizeOffset_X = -20f;
			PlayerDashboardInformationUI.container.SizeOffset_Y = -20f;
			PlayerDashboardInformationUI.container.SizeScale_X = 1f;
			PlayerDashboardInformationUI.container.SizeScale_Y = 1f;
			PlayerUI.container.AddChild(PlayerDashboardInformationUI.container);
			PlayerDashboardInformationUI.active = false;
			PlayerDashboardInformationUI.zoomMultiplier = 1;
			PlayerDashboardInformationUI.tab = PlayerDashboardInformationUI.EInfoTab.PLAYERS;
			PlayerDashboardInformationUI.backdropBox = Glazier.Get().CreateBox();
			PlayerDashboardInformationUI.backdropBox.PositionOffset_Y = 60f;
			PlayerDashboardInformationUI.backdropBox.SizeOffset_Y = -60f;
			PlayerDashboardInformationUI.backdropBox.SizeScale_X = 1f;
			PlayerDashboardInformationUI.backdropBox.SizeScale_Y = 1f;
			PlayerDashboardInformationUI.backdropBox.BackgroundColor = new SleekColor(1, 0.5f);
			PlayerDashboardInformationUI.container.AddChild(PlayerDashboardInformationUI.backdropBox);
			PlayerDashboardInformationUI.mapInspect = Glazier.Get().CreateFrame();
			PlayerDashboardInformationUI.mapInspect.PositionOffset_X = 10f;
			PlayerDashboardInformationUI.mapInspect.PositionOffset_Y = 10f;
			PlayerDashboardInformationUI.mapInspect.SizeOffset_X = -15f;
			PlayerDashboardInformationUI.mapInspect.SizeOffset_Y = -20f;
			PlayerDashboardInformationUI.mapInspect.SizeScale_X = 0.6f;
			PlayerDashboardInformationUI.mapInspect.SizeScale_Y = 1f;
			PlayerDashboardInformationUI.backdropBox.AddChild(PlayerDashboardInformationUI.mapInspect);
			ISleekConstraintFrame sleekConstraintFrame = Glazier.Get().CreateConstraintFrame();
			sleekConstraintFrame.SizeOffset_Y = -80f;
			sleekConstraintFrame.SizeScale_X = 1f;
			sleekConstraintFrame.SizeScale_Y = 1f;
			sleekConstraintFrame.Constraint = 1;
			PlayerDashboardInformationUI.mapInspect.AddChild(sleekConstraintFrame);
			CartographyVolume mainVolume = VolumeManager<CartographyVolume, CartographyVolumeManager>.Get().GetMainVolume();
			if (mainVolume != null)
			{
				Vector3 size = mainVolume.CalculateLocalBounds().size;
				sleekConstraintFrame.AspectRatio = size.x / size.z;
				PlayerDashboardInformationUI.maxZoomMultiplier = Mathf.CeilToInt(Mathf.Max(size.x, size.z) / 1024f) + 1;
			}
			else
			{
				PlayerDashboardInformationUI.maxZoomMultiplier = (int)(Level.size / 1024 + 1);
			}
			PlayerDashboardInformationUI.mapBox = Glazier.Get().CreateScrollView();
			PlayerDashboardInformationUI.mapBox.SizeScale_X = 1f;
			PlayerDashboardInformationUI.mapBox.SizeScale_Y = 1f;
			PlayerDashboardInformationUI.mapBox.HandleScrollWheel = false;
			PlayerDashboardInformationUI.mapBox.ScaleContentToWidth = true;
			PlayerDashboardInformationUI.mapBox.ScaleContentToHeight = true;
			sleekConstraintFrame.AddChild(PlayerDashboardInformationUI.mapBox);
			PlayerDashboardInformationUI.mapImage = Glazier.Get().CreateImage();
			PlayerDashboardInformationUI.mapImage.SizeScale_X = 1f;
			PlayerDashboardInformationUI.mapImage.SizeScale_Y = 1f;
			PlayerDashboardInformationUI.mapImage.OnRightClicked += new Action(PlayerDashboardInformationUI.onRightClickedMap);
			PlayerDashboardInformationUI.mapBox.AddChild(PlayerDashboardInformationUI.mapImage);
			PlayerDashboardInformationUI.mapLocationsContainer = Glazier.Get().CreateFrame();
			PlayerDashboardInformationUI.mapLocationsContainer.SizeScale_X = 1f;
			PlayerDashboardInformationUI.mapLocationsContainer.SizeScale_Y = 1f;
			PlayerDashboardInformationUI.mapImage.AddChild(PlayerDashboardInformationUI.mapLocationsContainer);
			this.createLocationNameLabels();
			PlayerDashboardInformationUI.mapArenaContainer = Glazier.Get().CreateFrame();
			PlayerDashboardInformationUI.mapArenaContainer.SizeScale_X = 1f;
			PlayerDashboardInformationUI.mapArenaContainer.SizeScale_Y = 1f;
			PlayerDashboardInformationUI.mapImage.AddChild(PlayerDashboardInformationUI.mapArenaContainer);
			PlayerDashboardInformationUI.mapMarkersContainer = Glazier.Get().CreateFrame();
			PlayerDashboardInformationUI.mapMarkersContainer.SizeScale_X = 1f;
			PlayerDashboardInformationUI.mapMarkersContainer.SizeScale_Y = 1f;
			PlayerDashboardInformationUI.mapImage.AddChild(PlayerDashboardInformationUI.mapMarkersContainer);
			PlayerDashboardInformationUI.mapRemotePlayersContainer = Glazier.Get().CreateFrame();
			PlayerDashboardInformationUI.mapRemotePlayersContainer.SizeScale_X = 1f;
			PlayerDashboardInformationUI.mapRemotePlayersContainer.SizeScale_Y = 1f;
			PlayerDashboardInformationUI.mapImage.AddChild(PlayerDashboardInformationUI.mapRemotePlayersContainer);
			PlayerDashboardInformationUI.arenaTargetPoints = new List<ISleekImage>();
			PlayerDashboardInformationUI.markerImages = new List<ISleekImage>();
			PlayerDashboardInformationUI.remotePlayerImages = new List<ISleekImage>();
			PlayerDashboardInformationUI.localPlayerImage = Glazier.Get().CreateImage();
			PlayerDashboardInformationUI.localPlayerImage.PositionOffset_X = -10f;
			PlayerDashboardInformationUI.localPlayerImage.PositionOffset_Y = -10f;
			PlayerDashboardInformationUI.localPlayerImage.SizeOffset_X = 20f;
			PlayerDashboardInformationUI.localPlayerImage.SizeOffset_Y = 20f;
			PlayerDashboardInformationUI.localPlayerImage.CanRotate = true;
			PlayerDashboardInformationUI.localPlayerImage.Texture = PlayerDashboardInformationUI.icons.load<Texture2D>("Player");
			PlayerDashboardInformationUI.localPlayerImage.TintColor = 2;
			if (string.IsNullOrEmpty(Characters.active.nick))
			{
				PlayerDashboardInformationUI.localPlayerImage.AddLabel(Characters.active.name, 1);
			}
			else
			{
				PlayerDashboardInformationUI.localPlayerImage.AddLabel(Characters.active.nick, 1);
			}
			PlayerDashboardInformationUI.mapImage.AddChild(PlayerDashboardInformationUI.localPlayerImage);
			PlayerDashboardInformationUI.arenaAreaCurrentOverlay = Glazier.Get().CreateImage(PlayerDashboardInformationUI.icons.load<Texture2D>("Arena_Area"));
			PlayerDashboardInformationUI.mapArenaContainer.AddChild(PlayerDashboardInformationUI.arenaAreaCurrentOverlay);
			PlayerDashboardInformationUI.arenaAreaCurrentLeftOverlay = Glazier.Get().CreateImage(GlazierResources.PixelTexture);
			PlayerDashboardInformationUI.arenaAreaCurrentLeftOverlay.SizeOffset_X = 1f;
			PlayerDashboardInformationUI.mapArenaContainer.AddChild(PlayerDashboardInformationUI.arenaAreaCurrentLeftOverlay);
			PlayerDashboardInformationUI.arenaAreaCurrentRightOverlay = Glazier.Get().CreateImage(GlazierResources.PixelTexture);
			PlayerDashboardInformationUI.arenaAreaCurrentRightOverlay.PositionOffset_X = -1f;
			PlayerDashboardInformationUI.arenaAreaCurrentRightOverlay.SizeOffset_X = 1f;
			PlayerDashboardInformationUI.mapArenaContainer.AddChild(PlayerDashboardInformationUI.arenaAreaCurrentRightOverlay);
			PlayerDashboardInformationUI.arenaAreaCurrentUpOverlay = Glazier.Get().CreateImage(GlazierResources.PixelTexture);
			PlayerDashboardInformationUI.arenaAreaCurrentUpOverlay.SizeOffset_Y = 1f;
			PlayerDashboardInformationUI.arenaAreaCurrentUpOverlay.SizeScale_X = 1f;
			PlayerDashboardInformationUI.mapArenaContainer.AddChild(PlayerDashboardInformationUI.arenaAreaCurrentUpOverlay);
			PlayerDashboardInformationUI.arenaAreaCurrentDownOverlay = Glazier.Get().CreateImage(GlazierResources.PixelTexture);
			PlayerDashboardInformationUI.arenaAreaCurrentDownOverlay.PositionOffset_Y = -1f;
			PlayerDashboardInformationUI.arenaAreaCurrentDownOverlay.SizeOffset_Y = 1f;
			PlayerDashboardInformationUI.arenaAreaCurrentDownOverlay.SizeScale_X = 1f;
			PlayerDashboardInformationUI.mapArenaContainer.AddChild(PlayerDashboardInformationUI.arenaAreaCurrentDownOverlay);
			PlayerDashboardInformationUI.noLabel = Glazier.Get().CreateLabel();
			PlayerDashboardInformationUI.noLabel.SizeOffset_Y = -80f;
			PlayerDashboardInformationUI.noLabel.SizeScale_X = 1f;
			PlayerDashboardInformationUI.noLabel.SizeScale_Y = 1f;
			PlayerDashboardInformationUI.noLabel.TextColor = Color.black;
			PlayerDashboardInformationUI.noLabel.FontSize = 4;
			PlayerDashboardInformationUI.noLabel.FontStyle = 1;
			PlayerDashboardInformationUI.mapInspect.AddChild(PlayerDashboardInformationUI.noLabel);
			PlayerDashboardInformationUI.noLabel.IsVisible = false;
			PlayerDashboardInformationUI.updateZoom();
			PlayerDashboardInformationUI.showMarkersToggle = Glazier.Get().CreateToggle();
			PlayerDashboardInformationUI.showMarkersToggle.PositionOffset_Y = -70f;
			PlayerDashboardInformationUI.showMarkersToggle.PositionScale_Y = 1f;
			PlayerDashboardInformationUI.showMarkersToggle.AddLabel(PlayerDashboardInformationUI.localization.format("ShowMarkersToggle_Label"), 1);
			PlayerDashboardInformationUI.showMarkersToggle.TooltipText = PlayerDashboardInformationUI.localization.format("ShowMarkersToggle_Tooltip");
			PlayerDashboardInformationUI.showMarkersToggle.Value = true;
			PlayerDashboardInformationUI.showMarkersToggle.OnValueChanged += new Toggled(PlayerDashboardInformationUI.OnShowMarkersToggled);
			PlayerDashboardInformationUI.mapInspect.AddChild(PlayerDashboardInformationUI.showMarkersToggle);
			PlayerDashboardInformationUI.showPlayerNamesToggle = Glazier.Get().CreateToggle();
			PlayerDashboardInformationUI.showPlayerNamesToggle.PositionOffset_Y = -70f;
			PlayerDashboardInformationUI.showPlayerNamesToggle.PositionScale_X = 0.25f;
			PlayerDashboardInformationUI.showPlayerNamesToggle.PositionScale_Y = 1f;
			PlayerDashboardInformationUI.showPlayerNamesToggle.AddLabel(PlayerDashboardInformationUI.localization.format("ShowPlayerNamesToggle_Label"), 1);
			PlayerDashboardInformationUI.showPlayerNamesToggle.TooltipText = PlayerDashboardInformationUI.localization.format("ShowPlayerNamesToggle_Tooltip");
			PlayerDashboardInformationUI.showPlayerNamesToggle.Value = true;
			PlayerDashboardInformationUI.showPlayerNamesToggle.OnValueChanged += new Toggled(PlayerDashboardInformationUI.OnShowMarkersToggled);
			PlayerDashboardInformationUI.mapInspect.AddChild(PlayerDashboardInformationUI.showPlayerNamesToggle);
			PlayerDashboardInformationUI.showPlayerAvatarsToggle = Glazier.Get().CreateToggle();
			PlayerDashboardInformationUI.showPlayerAvatarsToggle.PositionOffset_Y = -70f;
			PlayerDashboardInformationUI.showPlayerAvatarsToggle.PositionScale_X = 0.5f;
			PlayerDashboardInformationUI.showPlayerAvatarsToggle.PositionScale_Y = 1f;
			PlayerDashboardInformationUI.showPlayerAvatarsToggle.AddLabel(PlayerDashboardInformationUI.localization.format("ShowPlayerAvatarsToggle_Label"), 1);
			PlayerDashboardInformationUI.showPlayerAvatarsToggle.TooltipText = PlayerDashboardInformationUI.localization.format("ShowPlayerAvatarsToggle_Tooltip");
			PlayerDashboardInformationUI.showPlayerAvatarsToggle.Value = true;
			PlayerDashboardInformationUI.showPlayerAvatarsToggle.OnValueChanged += new Toggled(PlayerDashboardInformationUI.OnShowMarkersToggled);
			PlayerDashboardInformationUI.mapInspect.AddChild(PlayerDashboardInformationUI.showPlayerAvatarsToggle);
			PlayerDashboardInformationUI.zoomInButton = new SleekButtonIcon(PlayerDashboardInformationUI.icons.load<Texture2D>("Zoom_In"));
			PlayerDashboardInformationUI.zoomInButton.PositionOffset_Y = -30f;
			PlayerDashboardInformationUI.zoomInButton.PositionScale_Y = 1f;
			PlayerDashboardInformationUI.zoomInButton.SizeOffset_X = -5f;
			PlayerDashboardInformationUI.zoomInButton.SizeOffset_Y = 30f;
			PlayerDashboardInformationUI.zoomInButton.SizeScale_X = 0.25f;
			PlayerDashboardInformationUI.zoomInButton.text = PlayerDashboardInformationUI.localization.format("Zoom_In_Button");
			PlayerDashboardInformationUI.zoomInButton.tooltip = PlayerDashboardInformationUI.localization.format("Zoom_In_Button_Tooltip");
			PlayerDashboardInformationUI.zoomInButton.iconColor = 2;
			PlayerDashboardInformationUI.zoomInButton.onClickedButton += new ClickedButton(PlayerDashboardInformationUI.onClickedZoomInButton);
			PlayerDashboardInformationUI.mapInspect.AddChild(PlayerDashboardInformationUI.zoomInButton);
			PlayerDashboardInformationUI.zoomOutButton = new SleekButtonIcon(PlayerDashboardInformationUI.icons.load<Texture2D>("Zoom_Out"));
			PlayerDashboardInformationUI.zoomOutButton.PositionOffset_X = 5f;
			PlayerDashboardInformationUI.zoomOutButton.PositionOffset_Y = -30f;
			PlayerDashboardInformationUI.zoomOutButton.PositionScale_X = 0.25f;
			PlayerDashboardInformationUI.zoomOutButton.PositionScale_Y = 1f;
			PlayerDashboardInformationUI.zoomOutButton.SizeOffset_X = -10f;
			PlayerDashboardInformationUI.zoomOutButton.SizeOffset_Y = 30f;
			PlayerDashboardInformationUI.zoomOutButton.SizeScale_X = 0.25f;
			PlayerDashboardInformationUI.zoomOutButton.text = PlayerDashboardInformationUI.localization.format("Zoom_Out_Button");
			PlayerDashboardInformationUI.zoomOutButton.tooltip = PlayerDashboardInformationUI.localization.format("Zoom_Out_Button_Tooltip");
			PlayerDashboardInformationUI.zoomOutButton.iconColor = 2;
			PlayerDashboardInformationUI.zoomOutButton.onClickedButton += new ClickedButton(PlayerDashboardInformationUI.onClickedZoomOutButton);
			PlayerDashboardInformationUI.mapInspect.AddChild(PlayerDashboardInformationUI.zoomOutButton);
			PlayerDashboardInformationUI.centerButton = new SleekButtonIcon(PlayerDashboardInformationUI.icons.load<Texture2D>("Center"));
			PlayerDashboardInformationUI.centerButton.PositionOffset_X = 5f;
			PlayerDashboardInformationUI.centerButton.PositionOffset_Y = -30f;
			PlayerDashboardInformationUI.centerButton.PositionScale_X = 0.5f;
			PlayerDashboardInformationUI.centerButton.PositionScale_Y = 1f;
			PlayerDashboardInformationUI.centerButton.SizeOffset_X = -10f;
			PlayerDashboardInformationUI.centerButton.SizeOffset_Y = 30f;
			PlayerDashboardInformationUI.centerButton.SizeScale_X = 0.25f;
			PlayerDashboardInformationUI.centerButton.text = PlayerDashboardInformationUI.localization.format("Center_Button");
			PlayerDashboardInformationUI.centerButton.tooltip = PlayerDashboardInformationUI.localization.format("Center_Button_Tooltip");
			PlayerDashboardInformationUI.centerButton.iconColor = 2;
			PlayerDashboardInformationUI.centerButton.onClickedButton += new ClickedButton(PlayerDashboardInformationUI.onClickedCenterButton);
			PlayerDashboardInformationUI.mapInspect.AddChild(PlayerDashboardInformationUI.centerButton);
			PlayerDashboardInformationUI.mapButtonState = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(PlayerDashboardInformationUI.localization.format("Chart")),
				new GUIContent(PlayerDashboardInformationUI.localization.format("Satellite"))
			});
			PlayerDashboardInformationUI.mapButtonState.PositionOffset_X = 5f;
			PlayerDashboardInformationUI.mapButtonState.PositionOffset_Y = -30f;
			PlayerDashboardInformationUI.mapButtonState.PositionScale_X = 0.75f;
			PlayerDashboardInformationUI.mapButtonState.PositionScale_Y = 1f;
			PlayerDashboardInformationUI.mapButtonState.SizeOffset_X = -5f;
			PlayerDashboardInformationUI.mapButtonState.SizeOffset_Y = 30f;
			PlayerDashboardInformationUI.mapButtonState.SizeScale_X = 0.25f;
			PlayerDashboardInformationUI.mapButtonState.onSwappedState = new SwappedState(PlayerDashboardInformationUI.onSwappedMapState);
			PlayerDashboardInformationUI.mapInspect.AddChild(PlayerDashboardInformationUI.mapButtonState);
			PlayerDashboardInformationUI.headerButtonsContainer = Glazier.Get().CreateFrame();
			PlayerDashboardInformationUI.headerButtonsContainer.PositionOffset_X = 5f;
			PlayerDashboardInformationUI.headerButtonsContainer.PositionOffset_Y = 10f;
			PlayerDashboardInformationUI.headerButtonsContainer.PositionScale_X = 0.6f;
			PlayerDashboardInformationUI.headerButtonsContainer.SizeOffset_X = -15f;
			PlayerDashboardInformationUI.headerButtonsContainer.SizeOffset_Y = 50f;
			PlayerDashboardInformationUI.headerButtonsContainer.SizeScale_X = 0.4f;
			PlayerDashboardInformationUI.backdropBox.AddChild(PlayerDashboardInformationUI.headerButtonsContainer);
			PlayerDashboardInformationUI.questsButton = new SleekButtonIcon(PlayerDashboardInformationUI.icons.load<Texture2D>("Quests"));
			PlayerDashboardInformationUI.questsButton.SizeOffset_X = -5f;
			PlayerDashboardInformationUI.questsButton.SizeScale_X = 0.333f;
			PlayerDashboardInformationUI.questsButton.SizeScale_Y = 1f;
			PlayerDashboardInformationUI.questsButton.fontSize = 3;
			PlayerDashboardInformationUI.questsButton.tooltip = PlayerDashboardInformationUI.localization.format("Quests_Tooltip");
			PlayerDashboardInformationUI.questsButton.onClickedButton += new ClickedButton(PlayerDashboardInformationUI.onClickedQuestsButton);
			PlayerDashboardInformationUI.headerButtonsContainer.AddChild(PlayerDashboardInformationUI.questsButton);
			PlayerDashboardInformationUI.groupsButton = new SleekButtonIcon(PlayerDashboardInformationUI.icons.load<Texture2D>("Groups"));
			PlayerDashboardInformationUI.groupsButton.PositionOffset_X = 5f;
			PlayerDashboardInformationUI.groupsButton.PositionScale_X = 0.333f;
			PlayerDashboardInformationUI.groupsButton.SizeOffset_X = -10f;
			PlayerDashboardInformationUI.groupsButton.SizeScale_X = 0.334f;
			PlayerDashboardInformationUI.groupsButton.SizeScale_Y = 1f;
			PlayerDashboardInformationUI.groupsButton.fontSize = 3;
			PlayerDashboardInformationUI.groupsButton.text = PlayerDashboardInformationUI.localization.format("Groups");
			PlayerDashboardInformationUI.groupsButton.tooltip = PlayerDashboardInformationUI.localization.format("Groups_Tooltip");
			PlayerDashboardInformationUI.groupsButton.onClickedButton += new ClickedButton(PlayerDashboardInformationUI.onClickedGroupsButton);
			PlayerDashboardInformationUI.headerButtonsContainer.AddChild(PlayerDashboardInformationUI.groupsButton);
			PlayerDashboardInformationUI.playersButton = new SleekButtonIcon(PlayerDashboardInformationUI.icons.load<Texture2D>("Players"));
			PlayerDashboardInformationUI.playersButton.PositionOffset_X = 5f;
			PlayerDashboardInformationUI.playersButton.PositionScale_X = 0.667f;
			PlayerDashboardInformationUI.playersButton.SizeOffset_X = -5f;
			PlayerDashboardInformationUI.playersButton.SizeScale_X = 0.333f;
			PlayerDashboardInformationUI.playersButton.SizeScale_Y = 1f;
			PlayerDashboardInformationUI.playersButton.fontSize = 3;
			PlayerDashboardInformationUI.playersButton.tooltip = PlayerDashboardInformationUI.localization.format("Players_Tooltip");
			PlayerDashboardInformationUI.playersButton.onClickedButton += new ClickedButton(PlayerDashboardInformationUI.onClickedPlayersButton);
			PlayerDashboardInformationUI.headerButtonsContainer.AddChild(PlayerDashboardInformationUI.playersButton);
			PlayerDashboardInformationUI.questsBox = Glazier.Get().CreateScrollView();
			PlayerDashboardInformationUI.questsBox.PositionOffset_X = 5f;
			PlayerDashboardInformationUI.questsBox.PositionOffset_Y = 70f;
			PlayerDashboardInformationUI.questsBox.PositionScale_X = 0.6f;
			PlayerDashboardInformationUI.questsBox.SizeOffset_X = -15f;
			PlayerDashboardInformationUI.questsBox.SizeOffset_Y = -80f;
			PlayerDashboardInformationUI.questsBox.SizeScale_X = 0.4f;
			PlayerDashboardInformationUI.questsBox.SizeScale_Y = 1f;
			PlayerDashboardInformationUI.questsBox.ScaleContentToWidth = true;
			PlayerDashboardInformationUI.backdropBox.AddChild(PlayerDashboardInformationUI.questsBox);
			PlayerDashboardInformationUI.questsBox.IsVisible = false;
			PlayerDashboardInformationUI.groupsBox = Glazier.Get().CreateScrollView();
			PlayerDashboardInformationUI.groupsBox.PositionOffset_X = 5f;
			PlayerDashboardInformationUI.groupsBox.PositionOffset_Y = 70f;
			PlayerDashboardInformationUI.groupsBox.PositionScale_X = 0.6f;
			PlayerDashboardInformationUI.groupsBox.SizeOffset_X = -15f;
			PlayerDashboardInformationUI.groupsBox.SizeOffset_Y = -80f;
			PlayerDashboardInformationUI.groupsBox.SizeScale_X = 0.4f;
			PlayerDashboardInformationUI.groupsBox.SizeScale_Y = 1f;
			PlayerDashboardInformationUI.groupsBox.ScaleContentToWidth = true;
			PlayerDashboardInformationUI.backdropBox.AddChild(PlayerDashboardInformationUI.groupsBox);
			PlayerDashboardInformationUI.groupsBox.IsVisible = false;
			PlayerDashboardInformationUI.playersBox = Glazier.Get().CreateFrame();
			PlayerDashboardInformationUI.playersBox.PositionOffset_X = 5f;
			PlayerDashboardInformationUI.playersBox.PositionOffset_Y = 70f;
			PlayerDashboardInformationUI.playersBox.PositionScale_X = 0.6f;
			PlayerDashboardInformationUI.playersBox.SizeOffset_X = -15f;
			PlayerDashboardInformationUI.playersBox.SizeOffset_Y = -80f;
			PlayerDashboardInformationUI.playersBox.SizeScale_X = 0.4f;
			PlayerDashboardInformationUI.playersBox.SizeScale_Y = 1f;
			PlayerDashboardInformationUI.backdropBox.AddChild(PlayerDashboardInformationUI.playersBox);
			PlayerDashboardInformationUI.playersBox.IsVisible = true;
			PlayerDashboardInformationUI.playerSortButton = new SleekButtonState(new GUIContent[]
			{
				new GUIContent(PlayerDashboardInformationUI.localization.format("SortPlayers_Name")),
				new GUIContent(PlayerDashboardInformationUI.localization.format("SortPlayers_Group"))
			});
			PlayerDashboardInformationUI.playerSortButton.SizeScale_X = 1f;
			PlayerDashboardInformationUI.playerSortButton.SizeOffset_Y = 30f;
			PlayerDashboardInformationUI.playerSortButton.onSwappedState = new SwappedState(this.OnSwappedPlayerSortState);
			long value;
			if (ConvenientSavedata.get().read("PlayerListSortMode", out value))
			{
				PlayerDashboardInformationUI.playerSortButton.state = MathfEx.ClampLongToInt(value, 0, 1);
			}
			PlayerDashboardInformationUI.playersBox.AddChild(PlayerDashboardInformationUI.playerSortButton);
			PlayerDashboardInformationUI.playersList = new SleekList<SteamPlayer>();
			PlayerDashboardInformationUI.playersList.SizeScale_X = 1f;
			PlayerDashboardInformationUI.playersList.SizeScale_Y = 1f;
			PlayerDashboardInformationUI.playersList.itemHeight = 50;
			PlayerDashboardInformationUI.playersList.itemPadding = 10;
			PlayerDashboardInformationUI.playersBox.AddChild(PlayerDashboardInformationUI.playersList);
			PlayerDashboardInformationUI.sortedClients.Clear();
			PlayerDashboardInformationUI.playersList.SetData(PlayerDashboardInformationUI.sortedClients);
			PlayerUI.isBlindfoldedChanged += PlayerDashboardInformationUI.handleIsBlindfoldedChanged;
			Player player = Player.player;
			player.onPlayerTeleported = (PlayerTeleported)Delegate.Combine(player.onPlayerTeleported, new PlayerTeleported(PlayerDashboardInformationUI.onPlayerTeleported));
			PlayerQuests.groupUpdated = (GroupUpdatedHandler)Delegate.Combine(PlayerQuests.groupUpdated, new GroupUpdatedHandler(PlayerDashboardInformationUI.handleGroupUpdated));
			GroupManager.groupInfoReady += PlayerDashboardInformationUI.handleGroupInfoReady;
			ClientMessageHandler_Accepted.OnGameplayConfigReceived += new Action(PlayerDashboardInformationUI.OnGameplayConfigReceived);
			PlayerDashboardInformationUI.SyncPlayerSortButtonVisible();
			PlayerDashboardInformationUI.onPlayerTeleported(Player.player, Player.player.transform.position);
			string text = (Level.info != null) ? (Level.info.path + "/Chart.png") : null;
			if (text != null && ReadWrite.fileExists(text, false, false))
			{
				PlayerDashboardInformationUI.chartTexture = ReadWrite.readTextureFromFile(text, EReadTextureFromFileMode.UI);
			}
			else
			{
				PlayerDashboardInformationUI.chartTexture = null;
			}
			string text2 = (Level.info != null) ? (Level.info.path + "/Map.png") : null;
			if (text2 != null && ReadWrite.fileExists(text2, false, false))
			{
				PlayerDashboardInformationUI.mapTexture = ReadWrite.readTextureFromFile(text2, EReadTextureFromFileMode.UI);
			}
			else
			{
				PlayerDashboardInformationUI.mapTexture = null;
			}
			PlayerDashboardInformationUI.staticTexture = Resources.Load<Texture2D>("Level/Map");
		}

		// Token: 0x04002C10 RID: 11280
		private static readonly List<SteamPlayer> sortedClients = new List<SteamPlayer>();

		// Token: 0x04002C11 RID: 11281
		public static Local localization;

		// Token: 0x04002C12 RID: 11282
		public static Bundle icons;

		// Token: 0x04002C13 RID: 11283
		private static SleekFullscreenBox container;

		// Token: 0x04002C14 RID: 11284
		public static bool active;

		// Token: 0x04002C15 RID: 11285
		private static int zoomMultiplier;

		// Token: 0x04002C16 RID: 11286
		private static int maxZoomMultiplier;

		// Token: 0x04002C17 RID: 11287
		private static ISleekBox backdropBox;

		// Token: 0x04002C18 RID: 11288
		private static ISleekElement mapInspect;

		// Token: 0x04002C19 RID: 11289
		private static ISleekScrollView mapBox;

		// Token: 0x04002C1A RID: 11290
		private static ISleekImage mapImage;

		/// <summary>
		/// Labels for named locations.
		/// </summary>
		// Token: 0x04002C1B RID: 11291
		private static ISleekElement mapLocationsContainer;

		/// <summary>
		/// Contains arena outer circle and inner target points.
		/// </summary>
		// Token: 0x04002C1C RID: 11292
		private static ISleekElement mapArenaContainer;

		// Token: 0x04002C1D RID: 11293
		private static ISleekElement mapMarkersContainer;

		// Token: 0x04002C1E RID: 11294
		private static ISleekElement mapRemotePlayersContainer;

		// Token: 0x04002C1F RID: 11295
		private static List<ISleekImage> markerImages;

		// Token: 0x04002C20 RID: 11296
		private static List<ISleekImage> arenaTargetPoints;

		/// <summary>
		/// Player avatars.
		/// </summary>
		// Token: 0x04002C21 RID: 11297
		private static List<ISleekImage> remotePlayerImages;

		/// <summary>
		/// Arrow oriented with the local player.
		/// </summary>
		// Token: 0x04002C22 RID: 11298
		private static ISleekImage localPlayerImage;

		// Token: 0x04002C23 RID: 11299
		private static ISleekImage arenaAreaCurrentOverlay;

		// Token: 0x04002C24 RID: 11300
		private static ISleekImage arenaAreaCurrentLeftOverlay;

		// Token: 0x04002C25 RID: 11301
		private static ISleekImage arenaAreaCurrentRightOverlay;

		// Token: 0x04002C26 RID: 11302
		private static ISleekImage arenaAreaCurrentUpOverlay;

		// Token: 0x04002C27 RID: 11303
		private static ISleekImage arenaAreaCurrentDownOverlay;

		// Token: 0x04002C28 RID: 11304
		private static ISleekToggle showMarkersToggle;

		// Token: 0x04002C29 RID: 11305
		private static ISleekToggle showPlayerNamesToggle;

		// Token: 0x04002C2A RID: 11306
		private static ISleekToggle showPlayerAvatarsToggle;

		// Token: 0x04002C2B RID: 11307
		private static SleekButtonIcon zoomInButton;

		// Token: 0x04002C2C RID: 11308
		private static SleekButtonIcon zoomOutButton;

		// Token: 0x04002C2D RID: 11309
		private static SleekButtonIcon centerButton;

		// Token: 0x04002C2E RID: 11310
		private static SleekButtonState mapButtonState;

		// Token: 0x04002C2F RID: 11311
		public static ISleekLabel noLabel;

		// Token: 0x04002C30 RID: 11312
		private static ISleekElement headerButtonsContainer;

		// Token: 0x04002C31 RID: 11313
		private static SleekButtonIcon questsButton;

		// Token: 0x04002C32 RID: 11314
		private static SleekButtonIcon groupsButton;

		// Token: 0x04002C33 RID: 11315
		private static SleekButtonIcon playersButton;

		// Token: 0x04002C34 RID: 11316
		private static ISleekScrollView questsBox;

		// Token: 0x04002C35 RID: 11317
		private static ISleekScrollView groupsBox;

		// Token: 0x04002C36 RID: 11318
		private static ISleekElement playersBox;

		// Token: 0x04002C37 RID: 11319
		private static SleekButtonState playerSortButton;

		// Token: 0x04002C38 RID: 11320
		private static SleekList<SteamPlayer> playersList;

		// Token: 0x04002C39 RID: 11321
		private static ISleekFloat64Field radioFrequencyField;

		// Token: 0x04002C3A RID: 11322
		private static ISleekField groupNameField;

		// Token: 0x04002C3B RID: 11323
		private static bool hasChart;

		// Token: 0x04002C3C RID: 11324
		private static bool hasGPS;

		// Token: 0x04002C3D RID: 11325
		private static PlayerDashboardInformationUI.EInfoTab tab;

		// Token: 0x04002C3E RID: 11326
		private static Texture2D mapTexture;

		// Token: 0x04002C3F RID: 11327
		private static Texture2D chartTexture;

		// Token: 0x04002C40 RID: 11328
		private static Texture2D staticTexture;

		// Token: 0x04002C41 RID: 11329
		private static List<PlayerQuest> displayedQuests = new List<PlayerQuest>();

		// Token: 0x04002C42 RID: 11330
		private const string playerListSortKey = "PlayerListSortMode";

		// Token: 0x02000A0D RID: 2573
		private class SleekInviteButton : SleekWrapper
		{
			// Token: 0x17000C12 RID: 3090
			// (get) Token: 0x06004D52 RID: 19794 RVA: 0x001B9502 File Offset: 0x001B7702
			// (set) Token: 0x06004D53 RID: 19795 RVA: 0x001B950A File Offset: 0x001B770A
			public CSteamID groupID { get; protected set; }

			// Token: 0x06004D54 RID: 19796 RVA: 0x001B9513 File Offset: 0x001B7713
			private void handleJoinButtonClicked(ISleekElement button)
			{
				Player.player.quests.SendAcceptGroupInvitation(this.groupID);
			}

			// Token: 0x06004D55 RID: 19797 RVA: 0x001B952A File Offset: 0x001B772A
			private void handleIgnoreButtonClicked(ISleekElement button)
			{
				Player.player.quests.SendDeclineGroupInvitation(this.groupID);
			}

			// Token: 0x06004D56 RID: 19798 RVA: 0x001B9544 File Offset: 0x001B7744
			public SleekInviteButton(CSteamID newGroupID)
			{
				this.groupID = newGroupID;
				GroupInfo groupInfo = GroupManager.getGroupInfo(this.groupID);
				string text = (groupInfo != null) ? groupInfo.name : this.groupID.ToString();
				ISleekBox sleekBox = Glazier.Get().CreateBox();
				sleekBox.SizeOffset_X = -140f;
				sleekBox.SizeScale_X = 1f;
				sleekBox.SizeScale_Y = 1f;
				sleekBox.Text = text;
				base.AddChild(sleekBox);
				ISleekButton sleekButton = Glazier.Get().CreateButton();
				sleekButton.PositionScale_X = 1f;
				sleekButton.SizeOffset_X = 60f;
				sleekButton.SizeScale_Y = 1f;
				sleekButton.Text = PlayerDashboardInformationUI.localization.format("Group_Join");
				sleekButton.TooltipText = PlayerDashboardInformationUI.localization.format("Group_Join_Tooltip");
				sleekButton.OnClicked += new ClickedButton(this.handleJoinButtonClicked);
				sleekBox.AddChild(sleekButton);
				ISleekButton sleekButton2 = Glazier.Get().CreateButton();
				sleekButton2.PositionOffset_X = 60f;
				sleekButton2.PositionScale_X = 1f;
				sleekButton2.SizeOffset_X = 80f;
				sleekButton2.SizeScale_Y = 1f;
				sleekButton2.Text = PlayerDashboardInformationUI.localization.format("Group_Ignore");
				sleekButton2.TooltipText = PlayerDashboardInformationUI.localization.format("Group_Ignore_Tooltip");
				sleekButton2.OnClicked += new ClickedButton(this.handleIgnoreButtonClicked);
				sleekBox.AddChild(sleekButton2);
			}
		}

		// Token: 0x02000A0E RID: 2574
		private enum EInfoTab
		{
			// Token: 0x0400350E RID: 13582
			QUESTS,
			// Token: 0x0400350F RID: 13583
			GROUPS,
			// Token: 0x04003510 RID: 13584
			PLAYERS
		}
	}
}
