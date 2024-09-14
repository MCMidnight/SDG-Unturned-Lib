using System;
using System.Collections.Generic;
using SDG.Provider;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007C9 RID: 1993
	public class PlayerLifeUI
	{
		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x06004372 RID: 17266 RVA: 0x0017CDDC File Offset: 0x0017AFDC
		public static SleekFullscreenBox container
		{
			get
			{
				return PlayerLifeUI._container;
			}
		}

		// Token: 0x06004373 RID: 17267 RVA: 0x0017CDE3 File Offset: 0x0017AFE3
		private static ISleekLabel getCompassLabelByAngle(int angle)
		{
			return PlayerLifeUI.compassLabels[angle / 5];
		}

		// Token: 0x06004374 RID: 17268 RVA: 0x0017CDEE File Offset: 0x0017AFEE
		public static void open()
		{
			if (PlayerLifeUI.active)
			{
				return;
			}
			PlayerLifeUI.active = true;
			if (PlayerLifeUI.npc != null)
			{
				PlayerLifeUI.npc.OnStoppedTalkingWithLocalPlayer();
				PlayerLifeUI.npc = null;
			}
			PlayerLifeUI.container.AnimateIntoView();
		}

		// Token: 0x06004375 RID: 17269 RVA: 0x0017CE25 File Offset: 0x0017B025
		public static void close()
		{
			if (!PlayerLifeUI.active)
			{
				return;
			}
			PlayerLifeUI.active = false;
			PlayerLifeUI.closeChat();
			PlayerLifeUI.closeGestures();
			if (PlayerLifeUI.container != null)
			{
				PlayerLifeUI.container.AnimateOutOfView(0f, 1f);
			}
		}

		// Token: 0x06004376 RID: 17270 RVA: 0x0017CE5C File Offset: 0x0017B05C
		public static void openChat()
		{
			if (PlayerLifeUI.chatting)
			{
				return;
			}
			PlayerLifeUI.chatting = true;
			PlayerLifeUI.chatField.Text = string.Empty;
			PlayerLifeUI.chatField.AnimatePositionOffset(100f, PlayerLifeUI.chatField.PositionOffset_Y, 1, 20f);
			PlayerLifeUI.chatModeButton.state = (int)PlayerUI.chat;
			if (PlayerLifeUI.chatEntriesV2 != null)
			{
				PlayerLifeUI.chatScrollViewV2.VerticalScrollbarVisibility = 0;
				PlayerLifeUI.chatScrollViewV2.IsRaycastTarget = true;
				foreach (SleekChatEntryV2 sleekChatEntryV in PlayerLifeUI.chatEntriesV2)
				{
					sleekChatEntryV.forceVisibleWhileBrowsingChatHistory = true;
				}
				PlayerLifeUI.chatScrollViewV2.ScrollToBottom();
				return;
			}
			if (PlayerLifeUI.chatHistoryBoxV1 != null)
			{
				PlayerLifeUI.chatHistoryBoxV1.IsVisible = true;
				PlayerLifeUI.chatHistoryBoxV1.ScrollToBottom();
				for (int i = 0; i < PlayerLifeUI.chatPreviewLabelsV1.Length; i++)
				{
					PlayerLifeUI.chatPreviewLabelsV1[i].IsVisible = false;
				}
			}
		}

		// Token: 0x06004377 RID: 17271 RVA: 0x0017CF5C File Offset: 0x0017B15C
		public static void closeChat()
		{
			if (!PlayerLifeUI.chatting)
			{
				return;
			}
			PlayerLifeUI.chatting = false;
			PlayerLifeUI.repeatChatIndex = -1;
			if (PlayerLifeUI.chatField != null)
			{
				PlayerLifeUI.chatField.Text = string.Empty;
				PlayerLifeUI.chatField.AnimatePositionOffset(-PlayerLifeUI.chatField.SizeOffset_X - 50f, PlayerLifeUI.chatField.PositionOffset_Y, 1, 20f);
			}
			if (PlayerLifeUI.chatEntriesV2 != null)
			{
				PlayerLifeUI.chatScrollViewV2.VerticalScrollbarVisibility = 1;
				PlayerLifeUI.chatScrollViewV2.IsRaycastTarget = false;
				foreach (SleekChatEntryV2 sleekChatEntryV in PlayerLifeUI.chatEntriesV2)
				{
					sleekChatEntryV.forceVisibleWhileBrowsingChatHistory = false;
				}
				PlayerLifeUI.chatScrollViewV2.ScrollToBottom();
				return;
			}
			if (PlayerLifeUI.chatHistoryBoxV1 != null)
			{
				PlayerLifeUI.chatHistoryBoxV1.IsVisible = false;
				for (int i = 0; i < PlayerLifeUI.chatPreviewLabelsV1.Length; i++)
				{
					PlayerLifeUI.chatPreviewLabelsV1[i].IsVisible = true;
				}
			}
		}

		// Token: 0x06004378 RID: 17272 RVA: 0x0017D05C File Offset: 0x0017B25C
		public static void SendChatAndClose()
		{
			if (!string.IsNullOrEmpty(PlayerLifeUI.chatField.Text))
			{
				ChatManager.sendChat(PlayerUI.chat, PlayerLifeUI.chatField.Text);
			}
			PlayerLifeUI.closeChat();
		}

		/// <summary>
		/// Fill chat field with previous sent message.
		/// Useful for repeating commands with minor changes.
		/// </summary>
		// Token: 0x06004379 RID: 17273 RVA: 0x0017D088 File Offset: 0x0017B288
		public static void repeatChat(int delta)
		{
			if (PlayerLifeUI.chatField == null)
			{
				return;
			}
			int index = Mathf.Max(PlayerLifeUI.repeatChatIndex + delta, 0);
			string recentlySentMessage = ChatManager.getRecentlySentMessage(index);
			if (string.IsNullOrEmpty(recentlySentMessage))
			{
				return;
			}
			PlayerLifeUI.chatField.Text = recentlySentMessage;
			PlayerLifeUI.repeatChatIndex = index;
		}

		// Token: 0x0600437A RID: 17274 RVA: 0x0017D0CC File Offset: 0x0017B2CC
		private static void OnChatFieldEscaped(ISleekField field)
		{
			if (PlayerLifeUI.chatting)
			{
				PlayerLifeUI.closeChat();
			}
		}

		// Token: 0x0600437B RID: 17275 RVA: 0x0017D0DA File Offset: 0x0017B2DA
		private static void OnSwappedChatModeState(SleekButtonState button, int index)
		{
			PlayerUI.chat = (EChatMode)index;
		}

		// Token: 0x0600437C RID: 17276 RVA: 0x0017D0E2 File Offset: 0x0017B2E2
		private static void OnSendChatButtonClicked(ISleekElement button)
		{
			if (PlayerLifeUI.chatting)
			{
				PlayerLifeUI.SendChatAndClose();
			}
		}

		// Token: 0x0600437D RID: 17277 RVA: 0x0017D0F0 File Offset: 0x0017B2F0
		public static void openGestures()
		{
			if (PlayerLifeUI.gesturing)
			{
				return;
			}
			PlayerLifeUI.gesturing = true;
			for (int i = 0; i < PlayerLifeUI.faceButtons.Length; i++)
			{
				PlayerLifeUI.faceButtons[i].IsVisible = true;
			}
			bool isVisible = !Player.player.equipment.HasValidUseable && Player.player.stance.stance != EPlayerStance.PRONE && Player.player.stance.stance != EPlayerStance.DRIVING && Player.player.stance.stance != EPlayerStance.SITTING;
			PlayerLifeUI.surrenderButton.IsVisible = isVisible;
			PlayerLifeUI.pointButton.IsVisible = isVisible;
			PlayerLifeUI.waveButton.IsVisible = isVisible;
			PlayerLifeUI.saluteButton.IsVisible = isVisible;
			PlayerLifeUI.restButton.IsVisible = isVisible;
			PlayerLifeUI.facepalmButton.IsVisible = isVisible;
		}

		// Token: 0x0600437E RID: 17278 RVA: 0x0017D1BC File Offset: 0x0017B3BC
		public static void closeGestures()
		{
			if (!PlayerLifeUI.gesturing)
			{
				return;
			}
			PlayerLifeUI.gesturing = false;
			if (PlayerLifeUI.faceButtons == null)
			{
				return;
			}
			for (int i = 0; i < PlayerLifeUI.faceButtons.Length; i++)
			{
				PlayerLifeUI.faceButtons[i].IsVisible = false;
			}
			PlayerLifeUI.surrenderButton.IsVisible = false;
			PlayerLifeUI.pointButton.IsVisible = false;
			PlayerLifeUI.waveButton.IsVisible = false;
			PlayerLifeUI.saluteButton.IsVisible = false;
			PlayerLifeUI.restButton.IsVisible = false;
			PlayerLifeUI.facepalmButton.IsVisible = false;
		}

		// Token: 0x0600437F RID: 17279 RVA: 0x0017D240 File Offset: 0x0017B440
		private static void OnLocalPluginWidgetFlagsChanged(Player player, EPluginWidgetFlags oldFlags)
		{
			EPluginWidgetFlags pluginWidgetFlags = player.pluginWidgetFlags;
			if ((oldFlags & EPluginWidgetFlags.ShowStatusIcons) != (pluginWidgetFlags & EPluginWidgetFlags.ShowStatusIcons))
			{
				PlayerLifeUI.updateIcons();
			}
			if ((oldFlags & EPluginWidgetFlags.ShowLifeMeters) != (pluginWidgetFlags & EPluginWidgetFlags.ShowLifeMeters))
			{
				PlayerLifeUI.updateLifeBoxVisibility();
			}
			if ((oldFlags & EPluginWidgetFlags.ShowVehicleStatus) != (pluginWidgetFlags & EPluginWidgetFlags.ShowVehicleStatus))
			{
				PlayerLifeUI.UpdateVehicleBoxVisibility();
			}
			if (PlayerLifeUI.crosshair != null)
			{
				PlayerLifeUI.crosshair.SetPluginAllowsCenterDotVisible(pluginWidgetFlags.HasFlag(EPluginWidgetFlags.ShowCenterDot));
			}
		}

		// Token: 0x06004380 RID: 17280 RVA: 0x0017D2B9 File Offset: 0x0017B4B9
		private static void onDamaged(byte damage)
		{
			if (damage > 5)
			{
				PlayerUI.pain(Mathf.Clamp((float)damage / 40f, 0f, 1f));
			}
		}

		// Token: 0x06004381 RID: 17281 RVA: 0x0017D2DC File Offset: 0x0017B4DC
		private static void updateHotbarItem(ref float offset, ItemJar jar, byte index)
		{
			SleekItemIcon sleekItemIcon = PlayerLifeUI.hotbarImages[(int)index];
			ISleekLabel sleekLabel = PlayerLifeUI.hotbarLabels[(int)index];
			ushort num = 0;
			byte[] array = null;
			if (jar != null && jar.item != null)
			{
				num = jar.item.id;
				array = jar.item.state;
			}
			if (PlayerLifeUI.cachedHotbarValues[(int)index].id != num || PlayerLifeUI.cachedHotbarValues[(int)index].state != array)
			{
				PlayerLifeUI.cachedHotbarValues[(int)index].id = num;
				PlayerLifeUI.cachedHotbarValues[(int)index].state = array;
				ItemAsset itemAsset = Assets.find(EAssetType.ITEM, num) as ItemAsset;
				sleekItemIcon.IsVisible = (itemAsset != null);
				sleekLabel.IsVisible = (itemAsset != null);
				if (itemAsset != null)
				{
					sleekItemIcon.SizeOffset_X = (float)(itemAsset.size_x * 25);
					sleekItemIcon.SizeOffset_Y = (float)(itemAsset.size_y * 25);
					sleekItemIcon.Refresh(jar.item.id, jar.item.quality, jar.item.state, itemAsset);
				}
			}
			sleekItemIcon.PositionOffset_X = offset;
			sleekLabel.PositionOffset_X = offset + sleekItemIcon.SizeOffset_X - 55f;
			if (sleekItemIcon.IsVisible)
			{
				offset += sleekItemIcon.SizeOffset_X;
				PlayerLifeUI.hotbarContainer.SizeOffset_X = offset;
				offset += 5f;
				PlayerLifeUI.hotbarContainer.SizeOffset_Y = Mathf.Max(PlayerLifeUI.hotbarContainer.SizeOffset_Y, sleekItemIcon.SizeOffset_Y);
			}
		}

		/// <summary>
		/// Use the latest hotbar items in the UI.
		/// </summary>
		// Token: 0x06004382 RID: 17282 RVA: 0x0017D444 File Offset: 0x0017B644
		public static void updateHotbar()
		{
			if (PlayerLifeUI.hotbarContainer == null || Player.player == null)
			{
				return;
			}
			PlayerLifeUI.hotbarContainer.IsVisible = (!PlayerUI.messageBox.IsVisible && !PlayerUI.messageBox2.IsVisible && OptionsSettings.showHotbar);
			if (!Player.player.inventory.doesSearchNeedRefresh(ref PlayerLifeUI.cachedHotbarSearch))
			{
				return;
			}
			float num = 0f;
			PlayerLifeUI.updateHotbarItem(ref num, Player.player.inventory.getItem(0, 0), 0);
			PlayerLifeUI.updateHotbarItem(ref num, Player.player.inventory.getItem(1, 0), 1);
			byte b = 0;
			while ((int)b < Player.player.equipment.hotkeys.Length)
			{
				HotkeyInfo hotkeyInfo = Player.player.equipment.hotkeys[(int)b];
				ItemJar itemJar = null;
				if (hotkeyInfo.id != 0)
				{
					byte index = Player.player.inventory.getIndex(hotkeyInfo.page, hotkeyInfo.x, hotkeyInfo.y);
					itemJar = Player.player.inventory.getItem(hotkeyInfo.page, index);
					if (itemJar != null && itemJar.item.id != hotkeyInfo.id)
					{
						itemJar = null;
					}
				}
				PlayerLifeUI.updateHotbarItem(ref num, itemJar, b + 2);
				b += 1;
			}
			PlayerLifeUI.hotbarContainer.PositionOffset_X = PlayerLifeUI.hotbarContainer.SizeOffset_X / -2f;
			PlayerLifeUI.hotbarContainer.PositionOffset_Y = -80f - PlayerLifeUI.hotbarContainer.SizeOffset_Y;
		}

		// Token: 0x06004383 RID: 17283 RVA: 0x0017D5B4 File Offset: 0x0017B7B4
		public static void updateStatTracker()
		{
			EStatTrackerType estatTrackerType;
			int num;
			PlayerLifeUI.statTrackerLabel.IsVisible = Player.player.equipment.getUseableStatTrackerValue(out estatTrackerType, out num);
			if (PlayerLifeUI.statTrackerLabel.IsVisible)
			{
				PlayerLifeUI.statTrackerLabel.TextColor = Provider.provider.economyService.getStatTrackerColor(estatTrackerType);
				PlayerLifeUI.statTrackerLabel.Text = PlayerLifeUI.localization.format((estatTrackerType == EStatTrackerType.TOTAL) ? "Stat_Tracker_Total_Kills" : "Stat_Tracker_Player_Kills", num.ToString("D7"));
			}
		}

		// Token: 0x06004384 RID: 17284 RVA: 0x0017D639 File Offset: 0x0017B839
		private static void onHotkeysUpdated()
		{
			PlayerLifeUI.cachedHotbarSearch = -1;
		}

		// Token: 0x06004385 RID: 17285 RVA: 0x0017D644 File Offset: 0x0017B844
		public static void updateGrayscale()
		{
			GrayscaleEffect component = Player.player.animator.viewmodelCameraTransform.GetComponent<GrayscaleEffect>();
			GrayscaleEffect component2 = MainCamera.instance.GetComponent<GrayscaleEffect>();
			GrayscaleEffect component3 = Player.player.look.characterCamera.GetComponent<GrayscaleEffect>();
			if (Player.player.look.perspective == EPlayerPerspective.FIRST)
			{
				component.enabled = true;
				component2.enabled = false;
			}
			else
			{
				component.enabled = false;
				component2.enabled = true;
			}
			if (LevelLighting.vision == ELightingVision.CIVILIAN)
			{
				component.blend = 1f;
			}
			else if (Player.player.life.health < 50)
			{
				component.blend = (1f - (float)Player.player.life.health / 50f) * (1f - Player.player.skills.mastery(1, 3) * 0.75f);
			}
			else
			{
				component.blend = 0f;
			}
			component2.blend = component.blend;
			component3.blend = component.blend;
		}

		// Token: 0x06004386 RID: 17286 RVA: 0x0017D740 File Offset: 0x0017B940
		private static void onPerspectiveUpdated(EPlayerPerspective newPerspective)
		{
			PlayerLifeUI.updateGrayscale();
		}

		// Token: 0x06004387 RID: 17287 RVA: 0x0017D747 File Offset: 0x0017B947
		private static void onHealthUpdated(byte newHealth)
		{
			PlayerLifeUI.healthProgress.state = (float)newHealth / 100f;
			PlayerLifeUI.onPerspectiveUpdated(Player.player.look.perspective);
		}

		// Token: 0x06004388 RID: 17288 RVA: 0x0017D76F File Offset: 0x0017B96F
		private static void onFoodUpdated(byte newFood)
		{
			PlayerLifeUI.updateIcons();
			PlayerLifeUI.foodProgress.state = (float)newFood / 100f;
		}

		// Token: 0x06004389 RID: 17289 RVA: 0x0017D788 File Offset: 0x0017B988
		private static void onWaterUpdated(byte newWater)
		{
			PlayerLifeUI.updateIcons();
			PlayerLifeUI.waterProgress.state = (float)newWater / 100f;
		}

		// Token: 0x0600438A RID: 17290 RVA: 0x0017D7A1 File Offset: 0x0017B9A1
		private static void onVirusUpdated(byte newVirus)
		{
			PlayerLifeUI.updateIcons();
			PlayerLifeUI.virusProgress.state = (float)newVirus / 100f;
		}

		// Token: 0x0600438B RID: 17291 RVA: 0x0017D7BA File Offset: 0x0017B9BA
		private static void onStaminaUpdated(byte newStamina)
		{
			PlayerLifeUI.staminaProgress.state = (float)newStamina / 100f;
		}

		// Token: 0x0600438C RID: 17292 RVA: 0x0017D7CE File Offset: 0x0017B9CE
		private static void onOxygenUpdated(byte newOxygen)
		{
			PlayerLifeUI.updateIcons();
			PlayerLifeUI.oxygenProgress.state = (float)newOxygen / 100f;
		}

		// Token: 0x0600438D RID: 17293 RVA: 0x0017D7E7 File Offset: 0x0017B9E7
		private static void OnIsAsphyxiatingChanged()
		{
			PlayerLifeUI.updateIcons();
		}

		// Token: 0x0600438E RID: 17294 RVA: 0x0017D7F0 File Offset: 0x0017B9F0
		private static void updateCompassElement(ISleekElement element, float viewAngle, float elementAngle, out float alpha)
		{
			float num = Mathf.DeltaAngle(viewAngle, elementAngle) / 22.5f;
			element.PositionScale_X = num / 2f + 0.5f;
			element.IsVisible = (Mathf.Abs(num) < 1f);
			alpha = 1f - MathfEx.Square(Mathf.Abs(num));
		}

		// Token: 0x0600438F RID: 17295 RVA: 0x0017D844 File Offset: 0x0017BA44
		protected static bool hasCompassInInventory()
		{
			if (!Player.player.inventory.doesSearchNeedRefresh(ref PlayerLifeUI.cachedCompassSearch))
			{
				return PlayerLifeUI.cachedHasCompass;
			}
			PlayerLifeUI.cachedHasCompass = false;
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
							if (asset != null && asset.enablesCompass)
							{
								PlayerLifeUI.cachedHasCompass = true;
								return PlayerLifeUI.cachedHasCompass;
							}
						}
					}
				}
			}
			return PlayerLifeUI.cachedHasCompass;
		}

		// Token: 0x06004390 RID: 17296 RVA: 0x0017D90C File Offset: 0x0017BB0C
		public static void updateCompass()
		{
			if (Provider.modeConfigData.Gameplay.Compass || (Level.info != null && Level.info.type == ELevelType.ARENA))
			{
				PlayerLifeUI.compassBox.IsVisible = true;
			}
			else
			{
				PlayerLifeUI.compassBox.IsVisible = PlayerLifeUI.hasCompassInInventory();
			}
			if (!PlayerLifeUI.compassBox.IsVisible)
			{
				return;
			}
			Transform transform = MainCamera.instance.transform;
			Vector3 position = transform.position;
			float y = transform.rotation.eulerAngles.y;
			for (int i = 0; i < PlayerLifeUI.compassLabels.Length; i++)
			{
				float elementAngle = (float)(i * 5);
				ISleekLabel sleekLabel = PlayerLifeUI.compassLabels[i];
				Color color = sleekLabel.TextColor;
				PlayerLifeUI.updateCompassElement(sleekLabel, y, elementAngle, out color.a);
				sleekLabel.TextColor = color;
			}
			int num = 0;
			foreach (SteamPlayer steamPlayer in Provider.clients)
			{
				if (!(steamPlayer.model == null))
				{
					PlayerQuests quests = steamPlayer.player.quests;
					if ((!(steamPlayer.playerID.steamID != Provider.client) || quests.isMemberOfSameGroupAs(Player.player)) && quests.isMarkerPlaced)
					{
						ISleekImage sleekImage;
						if (num < PlayerLifeUI.compassMarkers.Count)
						{
							sleekImage = PlayerLifeUI.compassMarkers[num];
						}
						else
						{
							sleekImage = Glazier.Get().CreateImage(PlayerLifeUI.icons.load<Texture2D>("Marker"));
							sleekImage.PositionOffset_X = -10f;
							sleekImage.PositionOffset_Y = -5f;
							sleekImage.SizeOffset_X = 20f;
							sleekImage.SizeOffset_Y = 20f;
							PlayerLifeUI.compassMarkersContainer.AddChild(sleekImage);
							PlayerLifeUI.compassMarkers.Add(sleekImage);
						}
						num++;
						float num2 = Mathf.Atan2(quests.markerPosition.x - position.x, quests.markerPosition.z - position.z);
						num2 *= 57.29578f;
						Color markerColor = steamPlayer.markerColor;
						PlayerLifeUI.updateCompassElement(sleekImage, y, num2, out markerColor.a);
						sleekImage.TintColor = markerColor;
					}
				}
			}
			for (int j = PlayerLifeUI.compassMarkersVisibleCount - 1; j >= num; j--)
			{
				PlayerLifeUI.compassMarkers[j].IsVisible = false;
			}
			PlayerLifeUI.compassMarkersVisibleCount = num;
		}

		// Token: 0x06004391 RID: 17297 RVA: 0x0017DB94 File Offset: 0x0017BD94
		private static void updateIcons()
		{
			Player player = Player.player;
			bool flag = player.isPluginWidgetFlagActive(EPluginWidgetFlags.ShowStatusIcons);
			int num = 0;
			PlayerLifeUI.bleedingBox.IsVisible = (player.life.isBleeding && flag);
			if (PlayerLifeUI.bleedingBox.IsVisible)
			{
				num += 60;
			}
			PlayerLifeUI.brokenBox.PositionOffset_X = (float)num;
			PlayerLifeUI.brokenBox.IsVisible = (player.life.isBroken && flag);
			if (PlayerLifeUI.brokenBox.IsVisible)
			{
				num += 60;
			}
			PlayerLifeUI.temperatureBox.PositionOffset_X = (float)num;
			PlayerLifeUI.temperatureBox.IsVisible = (player.life.temperature != EPlayerTemperature.NONE && flag);
			if (PlayerLifeUI.temperatureBox.IsVisible)
			{
				num += 60;
			}
			PlayerLifeUI.starvedBox.PositionOffset_X = (float)num;
			PlayerLifeUI.starvedBox.IsVisible = (player.life.food == 0 && flag);
			if (PlayerLifeUI.starvedBox.IsVisible)
			{
				num += 60;
			}
			PlayerLifeUI.dehydratedBox.PositionOffset_X = (float)num;
			PlayerLifeUI.dehydratedBox.IsVisible = (player.life.water == 0 && flag);
			if (PlayerLifeUI.dehydratedBox.IsVisible)
			{
				num += 60;
			}
			PlayerLifeUI.infectedBox.PositionOffset_X = (float)num;
			PlayerLifeUI.infectedBox.IsVisible = (player.life.virus == 0 && flag);
			if (PlayerLifeUI.infectedBox.IsVisible)
			{
				num += 60;
			}
			PlayerLifeUI.drownedBox.PositionOffset_X = (float)num;
			PlayerLifeUI.drownedBox.IsVisible = (player.life.oxygen == 0 && flag);
			if (PlayerLifeUI.drownedBox.IsVisible)
			{
				num += 60;
			}
			PlayerLifeUI.asphyxiatingBox.PositionOffset_X = (float)num;
			PlayerLifeUI.asphyxiatingBox.IsVisible = (!PlayerLifeUI.drownedBox.IsVisible && player.life.isAsphyxiating && flag);
			if (PlayerLifeUI.asphyxiatingBox.IsVisible)
			{
				num += 60;
			}
			PlayerLifeUI.moonBox.PositionOffset_X = (float)num;
			PlayerLifeUI.moonBox.IsVisible = (LightingManager.isFullMoon && flag);
			if (PlayerLifeUI.moonBox.IsVisible)
			{
				num += 60;
			}
			PlayerLifeUI.radiationBox.PositionOffset_X = (float)num;
			PlayerLifeUI.radiationBox.IsVisible = (player.movement.isRadiated && flag);
			if (PlayerLifeUI.radiationBox.IsVisible)
			{
				num += 60;
			}
			PlayerLifeUI.safeBox.PositionOffset_X = (float)num;
			PlayerLifeUI.safeBox.IsVisible = (player.movement.isSafe && flag);
			if (PlayerLifeUI.safeBox.IsVisible)
			{
				num += 60;
			}
			PlayerLifeUI.arrestBox.PositionOffset_X = (float)num;
			PlayerLifeUI.arrestBox.IsVisible = (player.animator.gesture == EPlayerGesture.ARREST_START && flag);
			if (PlayerLifeUI.arrestBox.IsVisible)
			{
				num += 60;
			}
			PlayerLifeUI.statusIconsContainer.SizeOffset_X = (float)(num - 10);
			PlayerLifeUI.statusIconsContainer.IsVisible = (num > 0);
		}

		// Token: 0x06004392 RID: 17298 RVA: 0x0017DE58 File Offset: 0x0017C058
		private static void updateLifeBoxVisibility()
		{
			Player player = Player.player;
			bool flag = player.isPluginWidgetFlagActive(EPluginWidgetFlags.ShowHealth);
			bool flag2 = player.isPluginWidgetFlagActive(EPluginWidgetFlags.ShowFood);
			bool flag3 = player.isPluginWidgetFlagActive(EPluginWidgetFlags.ShowWater);
			bool flag4 = player.isPluginWidgetFlagActive(EPluginWidgetFlags.ShowVirus);
			bool flag5 = player.isPluginWidgetFlagActive(EPluginWidgetFlags.ShowStamina);
			bool flag6 = player.isPluginWidgetFlagActive(EPluginWidgetFlags.ShowOxygen);
			bool flag7 = false;
			if (Level.info != null)
			{
				if (Level.info.configData != null)
				{
					flag &= Level.info.configData.PlayerUI_HealthVisible;
					flag2 &= Level.info.configData.PlayerUI_FoodVisible;
					flag3 &= Level.info.configData.PlayerUI_WaterVisible;
					flag4 &= Level.info.configData.PlayerUI_VirusVisible;
					flag5 &= Level.info.configData.PlayerUI_StaminaVisible;
					flag6 &= Level.info.configData.PlayerUI_OxygenVisible;
				}
				if (Level.info.type == ELevelType.ARENA)
				{
					PlayerLifeUI.levelTextBox.IsVisible = true;
					PlayerLifeUI.levelNumberBox.IsVisible = true;
					PlayerLifeUI.compassBox.PositionOffset_Y = 60f;
				}
				if (Level.info.type != ELevelType.SURVIVAL)
				{
					flag2 = false;
					flag3 = false;
					flag4 = false;
					if (Level.info.type == ELevelType.HORDE)
					{
						flag6 = false;
						flag7 = true;
					}
				}
			}
			int num = 5;
			PlayerLifeUI.healthIcon.IsVisible = flag;
			PlayerLifeUI.healthProgress.IsVisible = flag;
			if (flag)
			{
				PlayerLifeUI.healthIcon.PositionOffset_Y = (float)num;
				PlayerLifeUI.healthProgress.PositionOffset_Y = (float)(num + 5);
				num += 30;
			}
			PlayerLifeUI.foodIcon.IsVisible = flag2;
			PlayerLifeUI.foodProgress.IsVisible = flag2;
			if (flag2)
			{
				PlayerLifeUI.foodIcon.PositionOffset_Y = (float)num;
				PlayerLifeUI.foodProgress.PositionOffset_Y = (float)(num + 5);
				num += 30;
			}
			PlayerLifeUI.waterIcon.IsVisible = flag3;
			PlayerLifeUI.waterProgress.IsVisible = flag3;
			if (flag3)
			{
				PlayerLifeUI.waterIcon.PositionOffset_Y = (float)num;
				PlayerLifeUI.waterProgress.PositionOffset_Y = (float)(num + 5);
				num += 30;
			}
			PlayerLifeUI.virusIcon.IsVisible = flag4;
			PlayerLifeUI.virusProgress.IsVisible = flag4;
			if (flag4)
			{
				PlayerLifeUI.virusIcon.PositionOffset_Y = (float)num;
				PlayerLifeUI.virusProgress.PositionOffset_Y = (float)(num + 5);
				num += 30;
			}
			PlayerLifeUI.staminaIcon.IsVisible = flag5;
			PlayerLifeUI.staminaProgress.IsVisible = flag5;
			if (flag5)
			{
				PlayerLifeUI.staminaIcon.PositionOffset_Y = (float)num;
				PlayerLifeUI.staminaProgress.PositionOffset_Y = (float)(num + 5);
				num += 30;
			}
			PlayerLifeUI.waveLabel.IsVisible = flag7;
			PlayerLifeUI.scoreLabel.IsVisible = flag7;
			if (flag7)
			{
				PlayerLifeUI.waveLabel.PositionOffset_Y = (float)num;
				PlayerLifeUI.scoreLabel.PositionOffset_Y = (float)num;
				num += 30;
			}
			PlayerLifeUI.oxygenIcon.IsVisible = flag6;
			PlayerLifeUI.oxygenProgress.IsVisible = flag6;
			if (flag6)
			{
				PlayerLifeUI.oxygenIcon.PositionOffset_Y = (float)num;
				PlayerLifeUI.oxygenProgress.PositionOffset_Y = (float)(num + 5);
				num += 30;
			}
			PlayerLifeUI.lifeBox.SizeOffset_Y = (float)(num - 5);
			PlayerLifeUI.lifeBox.PositionOffset_Y = -PlayerLifeUI.lifeBox.SizeOffset_Y;
			PlayerLifeUI.lifeBox.IsVisible = (PlayerLifeUI.lifeBox.SizeOffset_Y > 0f);
			PlayerLifeUI.statusIconsContainer.PositionOffset_Y = PlayerLifeUI.lifeBox.PositionOffset_Y - 60f;
		}

		// Token: 0x06004393 RID: 17299 RVA: 0x0017E194 File Offset: 0x0017C394
		private static void UpdateVehicleBoxVisibility()
		{
			bool flag = PlayerLifeUI.vehicleVisibleByDefault;
			flag &= Player.player.isPluginWidgetFlagActive(EPluginWidgetFlags.ShowVehicleStatus);
			PlayerLifeUI.vehicleBox.IsVisible = flag;
		}

		// Token: 0x06004394 RID: 17300 RVA: 0x0017E1C4 File Offset: 0x0017C3C4
		private static void onBleedingUpdated(bool newBleeding)
		{
			PlayerLifeUI.updateIcons();
		}

		// Token: 0x06004395 RID: 17301 RVA: 0x0017E1CB File Offset: 0x0017C3CB
		private static void onBrokenUpdated(bool newBroken)
		{
			PlayerLifeUI.updateIcons();
		}

		// Token: 0x06004396 RID: 17302 RVA: 0x0017E1D4 File Offset: 0x0017C3D4
		private static void onTemperatureUpdated(EPlayerTemperature newTemperature)
		{
			switch (newTemperature)
			{
			case EPlayerTemperature.FREEZING:
				PlayerLifeUI.temperatureBox.icon = PlayerLifeUI.icons.load<Texture2D>("Freezing");
				goto IL_D7;
			case EPlayerTemperature.COLD:
				PlayerLifeUI.temperatureBox.icon = PlayerLifeUI.icons.load<Texture2D>("Cold");
				goto IL_D7;
			case EPlayerTemperature.WARM:
				PlayerLifeUI.temperatureBox.icon = PlayerLifeUI.icons.load<Texture2D>("Warm");
				goto IL_D7;
			case EPlayerTemperature.BURNING:
				PlayerLifeUI.temperatureBox.icon = PlayerLifeUI.icons.load<Texture2D>("Burning");
				goto IL_D7;
			case EPlayerTemperature.COVERED:
				PlayerLifeUI.temperatureBox.icon = PlayerLifeUI.icons.load<Texture2D>("Covered");
				goto IL_D7;
			case EPlayerTemperature.ACID:
				PlayerLifeUI.temperatureBox.icon = PlayerLifeUI.icons.load<Texture2D>("Acid");
				goto IL_D7;
			}
			PlayerLifeUI.temperatureBox.icon = null;
			IL_D7:
			PlayerLifeUI.updateIcons();
		}

		// Token: 0x06004397 RID: 17303 RVA: 0x0017E2BD File Offset: 0x0017C4BD
		private static void onMoonUpdated(bool isFullMoon)
		{
			PlayerLifeUI.updateIcons();
		}

		// Token: 0x06004398 RID: 17304 RVA: 0x0017E2C4 File Offset: 0x0017C4C4
		private static void onExperienceUpdated(uint newExperience)
		{
			PlayerLifeUI.scoreLabel.Text = PlayerLifeUI.localization.format("Score", newExperience.ToString());
		}

		// Token: 0x06004399 RID: 17305 RVA: 0x0017E2E8 File Offset: 0x0017C4E8
		private static void onWaveUpdated(bool newWaveReady, int newWaveIndex)
		{
			PlayerLifeUI.waveLabel.Text = PlayerLifeUI.localization.format("Round", newWaveIndex);
			if (newWaveReady)
			{
				PlayerUI.message(EPlayerMessage.WAVE_ON, "", 2f);
				return;
			}
			PlayerUI.message(EPlayerMessage.WAVE_OFF, "", 2f);
		}

		// Token: 0x0600439A RID: 17306 RVA: 0x0017E33C File Offset: 0x0017C53C
		private static void onSeated(bool isDriver, bool inVehicle, bool wasVehicle, InteractableVehicle oldVehicle, InteractableVehicle newVehicle)
		{
			if (isDriver && inVehicle)
			{
				int num = 5;
				bool flag = newVehicle.usesFuel || newVehicle.asset.isStaminaPowered;
				PlayerLifeUI.fuelIcon.IsVisible = flag;
				PlayerLifeUI.fuelProgress.IsVisible = flag;
				if (flag)
				{
					PlayerLifeUI.fuelIcon.PositionOffset_Y = (float)num;
					PlayerLifeUI.fuelProgress.PositionOffset_Y = (float)(num + 5);
					num += 30;
				}
				PlayerLifeUI.speedIcon.PositionOffset_Y = (float)num;
				PlayerLifeUI.speedProgress.PositionOffset_Y = (float)(num + 5);
				num += 30;
				PlayerLifeUI.hpIcon.IsVisible = newVehicle.usesHealth;
				PlayerLifeUI.hpProgress.IsVisible = newVehicle.usesHealth;
				if (newVehicle.usesHealth)
				{
					PlayerLifeUI.hpIcon.PositionOffset_Y = (float)num;
					PlayerLifeUI.hpProgress.PositionOffset_Y = (float)(num + 5);
					num += 30;
				}
				PlayerLifeUI.batteryChargeIcon.IsVisible = newVehicle.usesBattery;
				PlayerLifeUI.batteryChargeProgress.IsVisible = newVehicle.usesBattery;
				if (newVehicle.usesBattery)
				{
					PlayerLifeUI.batteryChargeIcon.PositionOffset_Y = (float)num;
					PlayerLifeUI.batteryChargeProgress.PositionOffset_Y = (float)(num + 5);
					num += 30;
				}
				PlayerLifeUI.vehicleEngineLabel.IsVisible = (newVehicle.asset.UsesEngineRpmAndGears && newVehicle.asset.AllowsEngineRpmAndGearsInHud);
				if (PlayerLifeUI.vehicleEngineLabel.IsVisible)
				{
					PlayerLifeUI.vehicleEngineLabel.PositionOffset_Y = (float)(num - 5);
					num += 30;
				}
				PlayerLifeUI.vehicleBox.SizeOffset_Y = (float)(num - 5);
				PlayerLifeUI.vehicleBox.PositionOffset_Y = -PlayerLifeUI.vehicleBox.SizeOffset_Y;
				if (newVehicle.passengers[(int)Player.player.movement.getSeat()].turret != null)
				{
					PlayerLifeUI.vehicleBox.PositionOffset_Y -= 80f;
				}
				PlayerLifeUI.vehicleVisibleByDefault = true;
			}
			else
			{
				PlayerLifeUI.vehicleVisibleByDefault = false;
			}
			PlayerLifeUI.UpdateVehicleBoxVisibility();
		}

		// Token: 0x0600439B RID: 17307 RVA: 0x0017E508 File Offset: 0x0017C708
		private static void onVehicleUpdated(bool isDriveable, ushort newFuel, ushort maxFuel, float newSpeed, float minSpeed, float maxSpeed, ushort newHealth, ushort maxHealth, ushort newBatteryCharge)
		{
			if (isDriveable)
			{
				PlayerLifeUI.fuelProgress.state = (float)newFuel / (float)maxFuel;
				float num = Mathf.Clamp(newSpeed, minSpeed, maxSpeed);
				if (num > 0f)
				{
					num /= maxSpeed;
				}
				else
				{
					num /= minSpeed;
				}
				PlayerLifeUI.speedProgress.state = num;
				if (OptionsSettings.metric)
				{
					PlayerLifeUI.speedProgress.measure = (int)MeasurementTool.speedToKPH(Mathf.Abs(newSpeed));
				}
				else
				{
					PlayerLifeUI.speedProgress.measure = (int)MeasurementTool.KPHToMPH(MeasurementTool.speedToKPH(Mathf.Abs(newSpeed)));
				}
				PlayerLifeUI.batteryChargeProgress.state = (float)newBatteryCharge / 10000f;
				PlayerLifeUI.hpProgress.state = (float)newHealth / (float)maxHealth;
				InteractableVehicle vehicle = Player.player.movement.getVehicle();
				if (vehicle.asset != null && vehicle.asset.canBeLocked)
				{
					PlayerLifeUI.vehicleLockedLabel.Text = PlayerLifeUI.localization.format(vehicle.isLocked ? "Vehicle_Locked" : "Vehicle_Unlocked", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.locker));
					PlayerLifeUI.vehicleLockedLabel.IsVisible = true;
				}
				else
				{
					PlayerLifeUI.vehicleLockedLabel.IsVisible = false;
				}
				if (PlayerLifeUI.vehicleEngineLabel.IsVisible)
				{
					string arg;
					if (vehicle.GearNumber < 0)
					{
						arg = PlayerLifeUI.localization.format("VehicleGear_Reverse");
					}
					else if (vehicle.GearNumber == 0)
					{
						arg = PlayerLifeUI.localization.format("VehicleGear_Neutral");
					}
					else
					{
						arg = vehicle.GearNumber.ToString();
					}
					PlayerLifeUI.vehicleEngineLabel.Text = PlayerLifeUI.localization.format("VehicleEngineStatus", arg, Mathf.RoundToInt(vehicle.AnimatedEngineRpm));
				}
			}
			PlayerLifeUI.vehicleVisibleByDefault = isDriveable;
			PlayerLifeUI.UpdateVehicleBoxVisibility();
		}

		// Token: 0x0600439C RID: 17308 RVA: 0x0017E6A8 File Offset: 0x0017C8A8
		private static void updateGasmask()
		{
			if (!Player.player.movement.isRadiated)
			{
				PlayerLifeUI.gasmaskBox.IsVisible = false;
				return;
			}
			ItemMaskAsset maskAsset = Player.player.clothing.maskAsset;
			if (maskAsset != null && maskAsset.proofRadiation)
			{
				PlayerLifeUI.gasmaskIcon.Refresh(maskAsset.id, Player.player.clothing.maskQuality, Player.player.clothing.maskState, maskAsset);
				PlayerLifeUI.gasmaskProgress.state = (float)Player.player.clothing.maskQuality / 100f;
				PlayerLifeUI.gasmaskProgress.color = ItemTool.getQualityColor((float)Player.player.clothing.maskQuality / 100f);
				PlayerLifeUI.gasmaskBox.IsVisible = true;
				return;
			}
			PlayerLifeUI.gasmaskBox.IsVisible = false;
		}

		// Token: 0x0600439D RID: 17309 RVA: 0x0017E781 File Offset: 0x0017C981
		private static void onMaskUpdated(ushort newMask, byte newMaskQuality, byte[] newMaskState)
		{
			PlayerLifeUI.updateGasmask();
		}

		// Token: 0x0600439E RID: 17310 RVA: 0x0017E788 File Offset: 0x0017C988
		private static void onSafetyUpdated(bool isSafe)
		{
			PlayerLifeUI.updateIcons();
			if (isSafe)
			{
				PlayerUI.message(EPlayerMessage.SAFEZONE_ON, "", 2f);
				return;
			}
			PlayerUI.message(EPlayerMessage.SAFEZONE_OFF, "", 2f);
		}

		// Token: 0x0600439F RID: 17311 RVA: 0x0017E7B5 File Offset: 0x0017C9B5
		private static void onRadiationUpdated(bool isRadiated)
		{
			PlayerLifeUI.updateIcons();
			if (isRadiated)
			{
				PlayerUI.message(EPlayerMessage.DEADZONE_ON, "", 2f);
			}
			else
			{
				PlayerUI.message(EPlayerMessage.DEADZONE_OFF, "", 2f);
			}
			PlayerLifeUI.updateGasmask();
		}

		// Token: 0x060043A0 RID: 17312 RVA: 0x0017E7E8 File Offset: 0x0017C9E8
		private static void onGestureUpdated(EPlayerGesture gesture)
		{
			PlayerLifeUI.updateIcons();
		}

		// Token: 0x060043A1 RID: 17313 RVA: 0x0017E7EF File Offset: 0x0017C9EF
		private static void onTalked(bool isTalking)
		{
			PlayerLifeUI.voiceBox.IsVisible = isTalking;
		}

		// Token: 0x060043A2 RID: 17314 RVA: 0x0017E7FC File Offset: 0x0017C9FC
		internal static void UpdateTrackedQuest()
		{
			QuestAsset trackedQuest = Player.player.quests.GetTrackedQuest();
			if (trackedQuest == null)
			{
				PlayerLifeUI.trackedQuestTitle.IsVisible = false;
				PlayerLifeUI.trackedQuestBar.IsVisible = false;
				return;
			}
			PlayerLifeUI.trackedQuestTitle.Text = trackedQuest.questName;
			bool flag = true;
			if (trackedQuest.conditions != null)
			{
				PlayerLifeUI.trackedQuestBar.RemoveAllChildren();
				PlayerLifeUI.areConditionsMet.Clear();
				foreach (INPCCondition inpccondition in trackedQuest.conditions)
				{
					PlayerLifeUI.areConditionsMet.Add(inpccondition.isConditionMet(Player.player));
				}
				int num = 5;
				for (int j = 0; j < trackedQuest.conditions.Length; j++)
				{
					INPCCondition inpccondition2 = trackedQuest.conditions[j];
					if (!PlayerLifeUI.areConditionsMet[j] && inpccondition2.AreUIRequirementsMet(PlayerLifeUI.areConditionsMet))
					{
						string text = inpccondition2.formatCondition(Player.player);
						if (!string.IsNullOrEmpty(text))
						{
							ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
							sleekLabel.PositionOffset_X = -300f;
							sleekLabel.PositionOffset_Y = (float)num;
							sleekLabel.SizeOffset_X = 500f;
							sleekLabel.SizeOffset_Y = 30f;
							sleekLabel.AllowRichText = true;
							sleekLabel.TextColor = 4;
							sleekLabel.TextAlignment = 5;
							sleekLabel.Text = text;
							sleekLabel.TextContrastContext = 2;
							PlayerLifeUI.trackedQuestBar.AddChild(sleekLabel);
							num += 20;
							flag = false;
						}
					}
				}
			}
			PlayerLifeUI.trackedQuestTitle.IsVisible = !flag;
			PlayerLifeUI.trackedQuestBar.IsVisible = PlayerLifeUI.trackedQuestTitle.IsVisible;
		}

		// Token: 0x060043A3 RID: 17315 RVA: 0x0017E998 File Offset: 0x0017CB98
		private static void OnTrackedQuestUpdated(PlayerQuests quests)
		{
			PlayerLifeUI.UpdateTrackedQuest();
		}

		// Token: 0x060043A4 RID: 17316 RVA: 0x0017E9A0 File Offset: 0x0017CBA0
		private static void OnChatMessageReceived()
		{
			if (PlayerLifeUI.chatScrollViewV2 != null && ChatManager.receivedChatHistory.Count > 0)
			{
				if (PlayerLifeUI.chatEntriesV2.Count >= Provider.preferenceData.Chat.History_Length)
				{
					SleekChatEntryV2 sleekChatEntryV = PlayerLifeUI.chatEntriesV2.Dequeue();
					PlayerLifeUI.chatScrollViewV2.RemoveChild(sleekChatEntryV);
				}
				SleekChatEntryV2 sleekChatEntryV2 = new SleekChatEntryV2();
				sleekChatEntryV2.shouldFadeOutWithAge = Glazier.Get().SupportsRichTextAlpha;
				sleekChatEntryV2.forceVisibleWhileBrowsingChatHistory = PlayerLifeUI.chatting;
				sleekChatEntryV2.representingChatMessage = ChatManager.receivedChatHistory[0];
				PlayerLifeUI.chatScrollViewV2.AddChild(sleekChatEntryV2);
				PlayerLifeUI.chatEntriesV2.Enqueue(sleekChatEntryV2);
				if (!PlayerLifeUI.chatting)
				{
					PlayerLifeUI.chatScrollViewV2.ScrollToBottom();
					return;
				}
			}
			else if (PlayerLifeUI.chatHistoryBoxV1 != null)
			{
				int num = 0;
				int num2 = 0;
				while (num2 < ChatManager.receivedChatHistory.Count && num2 < PlayerLifeUI.chatHistoryLabelsV1.Length)
				{
					int num3 = ChatManager.receivedChatHistory.Count - 1 - num2;
					PlayerLifeUI.chatHistoryLabelsV1[num2].representingChatMessage = ChatManager.receivedChatHistory[num3];
					num++;
					num2++;
				}
				int num4 = num * 40;
				int num5 = PlayerLifeUI.chatPreviewLabelsV1.Length * 40;
				PlayerLifeUI.chatHistoryBoxV1.SizeOffset_Y = (float)Mathf.Min(num4, num5);
				PlayerLifeUI.chatHistoryBoxV1.PositionOffset_Y = Mathf.Max(0f, (float)num5 - PlayerLifeUI.chatHistoryBoxV1.SizeOffset_Y);
				PlayerLifeUI.chatHistoryBoxV1.ContentSizeOffset = new Vector2(0f, (float)num4);
				for (int i = 0; i < PlayerLifeUI.chatPreviewLabelsV1.Length; i++)
				{
					int num6 = PlayerLifeUI.chatPreviewLabelsV1.Length - 1 - i;
					if (num6 < ChatManager.receivedChatHistory.Count)
					{
						PlayerLifeUI.chatPreviewLabelsV1[i].representingChatMessage = ChatManager.receivedChatHistory[num6];
					}
				}
			}
		}

		// Token: 0x060043A5 RID: 17317 RVA: 0x0017EB5C File Offset: 0x0017CD5C
		private static void onVotingStart(SteamPlayer origin, SteamPlayer target, byte votesNeeded)
		{
			PlayerLifeUI.isVoteMessaged = false;
			PlayerLifeUI.voteBox.Text = "";
			PlayerLifeUI.voteBox.IsVisible = true;
			PlayerLifeUI.voteInfoLabel.IsVisible = true;
			PlayerLifeUI.votesNeededLabel.IsVisible = true;
			PlayerLifeUI.voteYesLabel.IsVisible = true;
			PlayerLifeUI.voteNoLabel.IsVisible = true;
			PlayerLifeUI.voteInfoLabel.Text = PlayerLifeUI.localization.format("Vote_Kick", new object[]
			{
				origin.playerID.characterName,
				origin.playerID.playerName,
				target.playerID.characterName,
				target.playerID.playerName
			});
			PlayerLifeUI.votesNeededLabel.Text = PlayerLifeUI.localization.format("Votes_Needed", votesNeeded);
			PlayerLifeUI.voteYesLabel.Text = PlayerLifeUI.localization.format("Vote_Yes", KeyCode.F1, 0);
			PlayerLifeUI.voteNoLabel.Text = PlayerLifeUI.localization.format("Vote_No", KeyCode.F2, 0);
		}

		// Token: 0x060043A6 RID: 17318 RVA: 0x0017EC80 File Offset: 0x0017CE80
		private static void onVotingUpdate(byte voteYes, byte voteNo)
		{
			PlayerLifeUI.voteYesLabel.Text = PlayerLifeUI.localization.format("Vote_Yes", KeyCode.F1, voteYes);
			PlayerLifeUI.voteNoLabel.Text = PlayerLifeUI.localization.format("Vote_No", KeyCode.F2, voteNo);
		}

		// Token: 0x060043A7 RID: 17319 RVA: 0x0017ECE0 File Offset: 0x0017CEE0
		private static void onVotingStop(EVotingMessage message)
		{
			PlayerLifeUI.voteInfoLabel.IsVisible = false;
			PlayerLifeUI.votesNeededLabel.IsVisible = false;
			PlayerLifeUI.voteYesLabel.IsVisible = false;
			PlayerLifeUI.voteNoLabel.IsVisible = false;
			if (message == EVotingMessage.PASS)
			{
				PlayerLifeUI.voteBox.Text = PlayerLifeUI.localization.format("Vote_Pass");
			}
			else if (message == EVotingMessage.FAIL)
			{
				PlayerLifeUI.voteBox.Text = PlayerLifeUI.localization.format("Vote_Fail");
			}
			PlayerLifeUI.isVoteMessaged = true;
			PlayerLifeUI.lastVoteMessage = Time.realtimeSinceStartup;
		}

		// Token: 0x060043A8 RID: 17320 RVA: 0x0017ED68 File Offset: 0x0017CF68
		private static void onVotingMessage(EVotingMessage message)
		{
			PlayerLifeUI.voteBox.IsVisible = true;
			PlayerLifeUI.voteInfoLabel.IsVisible = false;
			PlayerLifeUI.votesNeededLabel.IsVisible = false;
			PlayerLifeUI.voteYesLabel.IsVisible = false;
			PlayerLifeUI.voteNoLabel.IsVisible = false;
			if (message == EVotingMessage.OFF)
			{
				PlayerLifeUI.voteBox.Text = PlayerLifeUI.localization.format("Vote_Off");
			}
			else if (message == EVotingMessage.DELAY)
			{
				PlayerLifeUI.voteBox.Text = PlayerLifeUI.localization.format("Vote_Delay");
			}
			else if (message == EVotingMessage.PLAYERS)
			{
				PlayerLifeUI.voteBox.Text = PlayerLifeUI.localization.format("Vote_Players");
			}
			PlayerLifeUI.isVoteMessaged = true;
			PlayerLifeUI.lastVoteMessage = Time.realtimeSinceStartup;
		}

		// Token: 0x060043A9 RID: 17321 RVA: 0x0017EE18 File Offset: 0x0017D018
		private static void onArenaMessageUpdated(EArenaMessage newArenaMessage)
		{
			switch (newArenaMessage)
			{
			case EArenaMessage.LOBBY:
				PlayerLifeUI.levelTextBox.Text = PlayerLifeUI.localization.format("Arena_Lobby");
				return;
			case EArenaMessage.WARMUP:
				PlayerLifeUI.levelTextBox.Text = PlayerLifeUI.localization.format("Arena_Warm_Up");
				return;
			case EArenaMessage.PLAY:
				PlayerLifeUI.levelTextBox.Text = PlayerLifeUI.localization.format("Arena_Play");
				return;
			case EArenaMessage.DIED:
			case EArenaMessage.ABANDONED:
			case EArenaMessage.WIN:
				return;
			case EArenaMessage.LOSE:
				PlayerLifeUI.levelTextBox.Text = PlayerLifeUI.localization.format("Arena_Lose");
				return;
			case EArenaMessage.INTERMISSION:
				PlayerLifeUI.levelTextBox.Text = PlayerLifeUI.localization.format("Arena_Intermission");
				return;
			default:
				return;
			}
		}

		// Token: 0x060043AA RID: 17322 RVA: 0x0017EED0 File Offset: 0x0017D0D0
		private static void onArenaPlayerUpdated(ulong[] playerIDs, EArenaMessage newArenaMessage)
		{
			List<SteamPlayer> list = new List<SteamPlayer>();
			for (int i = 0; i < playerIDs.Length; i++)
			{
				SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(playerIDs[i]);
				if (steamPlayer != null)
				{
					list.Add(steamPlayer);
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			string text = "";
			for (int j = 0; j < list.Count; j++)
			{
				SteamPlayer steamPlayer2 = list[j];
				if (j == 0)
				{
					text += steamPlayer2.playerID.characterName;
				}
				else if (j == list.Count - 1)
				{
					text = text + PlayerLifeUI.localization.format("List_Joint_1") + steamPlayer2.playerID.characterName;
				}
				else
				{
					text = text + PlayerLifeUI.localization.format("List_Joint_0") + steamPlayer2.playerID.characterName;
				}
			}
			switch (newArenaMessage)
			{
			case EArenaMessage.DIED:
				PlayerLifeUI.levelTextBox.Text = PlayerLifeUI.localization.format("Arena_Died", text);
				return;
			case EArenaMessage.ABANDONED:
				PlayerLifeUI.levelTextBox.Text = PlayerLifeUI.localization.format("Arena_Abandoned", text);
				return;
			case EArenaMessage.WIN:
				PlayerLifeUI.levelTextBox.Text = PlayerLifeUI.localization.format("Arena_Win", text);
				return;
			default:
				return;
			}
		}

		// Token: 0x060043AB RID: 17323 RVA: 0x0017F005 File Offset: 0x0017D205
		private static void onLevelNumberUpdated(int newLevelNumber)
		{
			PlayerLifeUI.levelNumberBox.Text = newLevelNumber.ToString();
		}

		// Token: 0x060043AC RID: 17324 RVA: 0x0017F018 File Offset: 0x0017D218
		private static void onClickedFaceButton(ISleekElement button)
		{
			byte b = 0;
			while ((int)b < PlayerLifeUI.faceButtons.Length && PlayerLifeUI.faceButtons[(int)b] != button)
			{
				b += 1;
			}
			Player.player.clothing.sendSwapFace(b);
			PlayerLifeUI.closeGestures();
		}

		// Token: 0x060043AD RID: 17325 RVA: 0x0017F057 File Offset: 0x0017D257
		private static void onClickedSurrenderButton(ISleekElement button)
		{
			if (Player.player.animator.gesture == EPlayerGesture.SURRENDER_START)
			{
				Player.player.animator.sendGesture(EPlayerGesture.SURRENDER_STOP, true);
			}
			else
			{
				Player.player.animator.sendGesture(EPlayerGesture.SURRENDER_START, true);
			}
			PlayerLifeUI.closeGestures();
		}

		// Token: 0x060043AE RID: 17326 RVA: 0x0017F094 File Offset: 0x0017D294
		private static void onClickedPointButton(ISleekElement button)
		{
			Player.player.animator.sendGesture(EPlayerGesture.POINT, true);
			PlayerLifeUI.closeGestures();
		}

		// Token: 0x060043AF RID: 17327 RVA: 0x0017F0AC File Offset: 0x0017D2AC
		private static void onClickedWaveButton(ISleekElement button)
		{
			Player.player.animator.sendGesture(EPlayerGesture.WAVE, true);
			PlayerLifeUI.closeGestures();
		}

		// Token: 0x060043B0 RID: 17328 RVA: 0x0017F0C5 File Offset: 0x0017D2C5
		private static void onClickedSaluteButton(ISleekElement button)
		{
			Player.player.animator.sendGesture(EPlayerGesture.SALUTE, true);
			PlayerLifeUI.closeGestures();
		}

		// Token: 0x060043B1 RID: 17329 RVA: 0x0017F0DE File Offset: 0x0017D2DE
		private static void onClickedRestButton(ISleekElement button)
		{
			if (Player.player.animator.gesture == EPlayerGesture.REST_START)
			{
				Player.player.animator.sendGesture(EPlayerGesture.REST_STOP, true);
			}
			else
			{
				Player.player.animator.sendGesture(EPlayerGesture.REST_START, true);
			}
			PlayerLifeUI.closeGestures();
		}

		// Token: 0x060043B2 RID: 17330 RVA: 0x0017F11E File Offset: 0x0017D31E
		private static void onClickedFacepalmButton(ISleekElement button)
		{
			Player.player.animator.sendGesture(EPlayerGesture.FACEPALM, true);
			PlayerLifeUI.closeGestures();
		}

		// Token: 0x060043B3 RID: 17331 RVA: 0x0017F137 File Offset: 0x0017D337
		private void OnUnitSystemChanged()
		{
			PlayerLifeUI.speedProgress.suffix = (OptionsSettings.metric ? " kph" : " mph");
		}

		// Token: 0x060043B4 RID: 17332 RVA: 0x0017F158 File Offset: 0x0017D358
		public void OnDestroy()
		{
			ChatManager.onChatMessageReceived = (ChatMessageReceivedHandler)Delegate.Remove(ChatManager.onChatMessageReceived, new ChatMessageReceivedHandler(PlayerLifeUI.OnChatMessageReceived));
			ChatManager.onVotingStart = (VotingStart)Delegate.Remove(ChatManager.onVotingStart, new VotingStart(PlayerLifeUI.onVotingStart));
			ChatManager.onVotingUpdate = (VotingUpdate)Delegate.Remove(ChatManager.onVotingUpdate, new VotingUpdate(PlayerLifeUI.onVotingUpdate));
			ChatManager.onVotingStop = (VotingStop)Delegate.Remove(ChatManager.onVotingStop, new VotingStop(PlayerLifeUI.onVotingStop));
			ChatManager.onVotingMessage = (VotingMessage)Delegate.Remove(ChatManager.onVotingMessage, new VotingMessage(PlayerLifeUI.onVotingMessage));
			LevelManager.onArenaMessageUpdated = (ArenaMessageUpdated)Delegate.Remove(LevelManager.onArenaMessageUpdated, new ArenaMessageUpdated(PlayerLifeUI.onArenaMessageUpdated));
			LevelManager.onArenaPlayerUpdated = (ArenaPlayerUpdated)Delegate.Remove(LevelManager.onArenaPlayerUpdated, new ArenaPlayerUpdated(PlayerLifeUI.onArenaPlayerUpdated));
			LevelManager.onLevelNumberUpdated = (LevelNumberUpdated)Delegate.Remove(LevelManager.onLevelNumberUpdated, new LevelNumberUpdated(PlayerLifeUI.onLevelNumberUpdated));
			OptionsSettings.OnUnitSystemChanged -= new Action(this.OnUnitSystemChanged);
			Player.player.life.OnIsAsphyxiatingChanged -= new Action(PlayerLifeUI.OnIsAsphyxiatingChanged);
		}

		// Token: 0x060043B5 RID: 17333 RVA: 0x0017F294 File Offset: 0x0017D494
		public PlayerLifeUI()
		{
			if (PlayerLifeUI.icons != null)
			{
				PlayerLifeUI.icons.unload();
			}
			PlayerLifeUI.localization = Localization.read("/Player/PlayerLife.dat");
			PlayerLifeUI.icons = Bundles.getBundle("/Bundles/Textures/Player/Icons/PlayerLife/PlayerLife.unity3d");
			PlayerLifeUI._container = new SleekFullscreenBox();
			PlayerLifeUI.container.PositionOffset_X = 10f;
			PlayerLifeUI.container.PositionOffset_Y = 10f;
			PlayerLifeUI.container.SizeOffset_X = -20f;
			PlayerLifeUI.container.SizeOffset_Y = -20f;
			PlayerLifeUI.container.SizeScale_X = 1f;
			PlayerLifeUI.container.SizeScale_Y = 1f;
			PlayerUI.container.AddChild(PlayerLifeUI.container);
			PlayerLifeUI.active = true;
			PlayerLifeUI.chatting = false;
			if (Glazier.Get().SupportsAutomaticLayout)
			{
				PlayerLifeUI.chatScrollViewV2 = Glazier.Get().CreateScrollView();
				PlayerLifeUI.chatScrollViewV2.SizeOffset_X = 630f;
				PlayerLifeUI.chatScrollViewV2.SizeOffset_Y = (float)(Provider.preferenceData.Chat.Preview_Length * 40);
				PlayerLifeUI.chatScrollViewV2.ScaleContentToWidth = true;
				PlayerLifeUI.chatScrollViewV2.ContentUseManualLayout = false;
				PlayerLifeUI.chatScrollViewV2.AlignContentToBottom = true;
				PlayerLifeUI.chatScrollViewV2.VerticalScrollbarVisibility = 1;
				PlayerLifeUI.chatScrollViewV2.IsRaycastTarget = false;
				PlayerLifeUI.container.AddChild(PlayerLifeUI.chatScrollViewV2);
				PlayerLifeUI.chatEntriesV2 = new Queue<SleekChatEntryV2>(Provider.preferenceData.Chat.History_Length);
			}
			else
			{
				PlayerLifeUI.chatHistoryBoxV1 = Glazier.Get().CreateScrollView();
				PlayerLifeUI.chatHistoryBoxV1.SizeOffset_X = 630f;
				PlayerLifeUI.chatHistoryBoxV1.ScaleContentToWidth = true;
				PlayerLifeUI.container.AddChild(PlayerLifeUI.chatHistoryBoxV1);
				PlayerLifeUI.chatHistoryBoxV1.IsVisible = false;
				PlayerLifeUI.chatHistoryLabelsV1 = new SleekChatEntryV1[Provider.preferenceData.Chat.History_Length];
				for (int i = 0; i < PlayerLifeUI.chatHistoryLabelsV1.Length; i++)
				{
					SleekChatEntryV1 sleekChatEntryV = new SleekChatEntryV1();
					sleekChatEntryV.PositionOffset_Y = (float)(i * 40);
					sleekChatEntryV.SizeOffset_X = PlayerLifeUI.chatHistoryBoxV1.SizeOffset_X - 30f;
					sleekChatEntryV.SizeOffset_Y = 40f;
					sleekChatEntryV.shouldFadeOutWithAge = false;
					PlayerLifeUI.chatHistoryBoxV1.AddChild(sleekChatEntryV);
					PlayerLifeUI.chatHistoryLabelsV1[i] = sleekChatEntryV;
				}
				bool supportsRichTextAlpha = Glazier.Get().SupportsRichTextAlpha;
				PlayerLifeUI.chatPreviewLabelsV1 = new SleekChatEntryV1[Provider.preferenceData.Chat.Preview_Length];
				for (int j = 0; j < PlayerLifeUI.chatPreviewLabelsV1.Length; j++)
				{
					SleekChatEntryV1 sleekChatEntryV2 = new SleekChatEntryV1();
					sleekChatEntryV2.PositionOffset_Y = (float)(j * 40);
					sleekChatEntryV2.SizeOffset_X = PlayerLifeUI.chatHistoryBoxV1.SizeOffset_X - 30f;
					sleekChatEntryV2.SizeOffset_Y = 40f;
					sleekChatEntryV2.shouldFadeOutWithAge = supportsRichTextAlpha;
					PlayerLifeUI.container.AddChild(sleekChatEntryV2);
					PlayerLifeUI.chatPreviewLabelsV1[j] = sleekChatEntryV2;
				}
			}
			PlayerLifeUI.chatField = Glazier.Get().CreateStringField();
			PlayerLifeUI.chatField.PositionOffset_Y = (float)(Provider.preferenceData.Chat.Preview_Length * 40 + 10);
			PlayerLifeUI.chatField.SizeOffset_X = 500f;
			PlayerLifeUI.chatField.PositionOffset_X = -PlayerLifeUI.chatField.SizeOffset_X - 50f;
			PlayerLifeUI.chatField.SizeOffset_Y = 30f;
			PlayerLifeUI.chatField.TextAlignment = 3;
			PlayerLifeUI.chatField.MaxLength = ChatManager.MAX_MESSAGE_LENGTH;
			PlayerLifeUI.chatField.OnTextEscaped += new Escaped(PlayerLifeUI.OnChatFieldEscaped);
			PlayerLifeUI.container.AddChild(PlayerLifeUI.chatField);
			PlayerLifeUI.chatModeButton = new SleekButtonState(Array.Empty<GUIContent>());
			PlayerLifeUI.chatModeButton.UseContentTooltip = true;
			PlayerLifeUI.chatModeButton.setContent(new GUIContent[]
			{
				new GUIContent(PlayerLifeUI.localization.format("Mode_Global"), PlayerLifeUI.localization.format("Mode_Global_Tooltip", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.global))),
				new GUIContent(PlayerLifeUI.localization.format("Mode_Local"), PlayerLifeUI.localization.format("Mode_Local_Tooltip", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.local))),
				new GUIContent(PlayerLifeUI.localization.format("Mode_Group"), PlayerLifeUI.localization.format("Mode_Group_Tooltip", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.group)))
			});
			PlayerLifeUI.chatModeButton.PositionOffset_X = -100f;
			PlayerLifeUI.chatModeButton.SizeOffset_X = 100f;
			PlayerLifeUI.chatModeButton.SizeOffset_Y = 30f;
			PlayerLifeUI.chatModeButton.onSwappedState = new SwappedState(PlayerLifeUI.OnSwappedChatModeState);
			PlayerLifeUI.chatField.AddChild(PlayerLifeUI.chatModeButton);
			PlayerLifeUI.sendChatButton = new SleekButtonIcon(PlayerLifeUI.icons.load<Texture2D>("SendChat"));
			PlayerLifeUI.sendChatButton.PositionScale_X = 1f;
			PlayerLifeUI.sendChatButton.SizeOffset_X = 30f;
			PlayerLifeUI.sendChatButton.SizeOffset_Y = 30f;
			PlayerLifeUI.sendChatButton.tooltip = PlayerLifeUI.localization.format("SendChat_Tooltip", MenuConfigurationControlsUI.getKeyCodeText(KeyCode.Return));
			PlayerLifeUI.sendChatButton.iconColor = 2;
			PlayerLifeUI.sendChatButton.onClickedButton += new ClickedButton(PlayerLifeUI.OnSendChatButtonClicked);
			PlayerLifeUI.chatField.AddChild(PlayerLifeUI.sendChatButton);
			PlayerLifeUI.voteBox = Glazier.Get().CreateBox();
			PlayerLifeUI.voteBox.PositionOffset_X = -430f;
			PlayerLifeUI.voteBox.PositionScale_X = 1f;
			PlayerLifeUI.voteBox.SizeOffset_X = 430f;
			PlayerLifeUI.voteBox.SizeOffset_Y = 90f;
			PlayerLifeUI.container.AddChild(PlayerLifeUI.voteBox);
			PlayerLifeUI.voteBox.IsVisible = false;
			PlayerLifeUI.voteInfoLabel = Glazier.Get().CreateLabel();
			PlayerLifeUI.voteInfoLabel.SizeOffset_Y = 30f;
			PlayerLifeUI.voteInfoLabel.SizeScale_X = 1f;
			PlayerLifeUI.voteBox.AddChild(PlayerLifeUI.voteInfoLabel);
			PlayerLifeUI.votesNeededLabel = Glazier.Get().CreateLabel();
			PlayerLifeUI.votesNeededLabel.PositionOffset_Y = 30f;
			PlayerLifeUI.votesNeededLabel.SizeOffset_Y = 30f;
			PlayerLifeUI.votesNeededLabel.SizeScale_X = 1f;
			PlayerLifeUI.voteBox.AddChild(PlayerLifeUI.votesNeededLabel);
			PlayerLifeUI.voteYesLabel = Glazier.Get().CreateLabel();
			PlayerLifeUI.voteYesLabel.PositionOffset_Y = 60f;
			PlayerLifeUI.voteYesLabel.SizeOffset_Y = 30f;
			PlayerLifeUI.voteYesLabel.SizeScale_X = 0.5f;
			PlayerLifeUI.voteBox.AddChild(PlayerLifeUI.voteYesLabel);
			PlayerLifeUI.voteNoLabel = Glazier.Get().CreateLabel();
			PlayerLifeUI.voteNoLabel.PositionOffset_Y = 60f;
			PlayerLifeUI.voteNoLabel.PositionScale_X = 0.5f;
			PlayerLifeUI.voteNoLabel.SizeOffset_Y = 30f;
			PlayerLifeUI.voteNoLabel.SizeScale_X = 0.5f;
			PlayerLifeUI.voteBox.AddChild(PlayerLifeUI.voteNoLabel);
			PlayerLifeUI.voiceBox = new SleekBoxIcon(PlayerLifeUI.icons.load<Texture2D>("Voice"));
			PlayerLifeUI.voiceBox.PositionOffset_Y = 210f;
			PlayerLifeUI.voiceBox.SizeOffset_X = 50f;
			PlayerLifeUI.voiceBox.SizeOffset_Y = 50f;
			PlayerLifeUI.voiceBox.iconColor = 2;
			PlayerLifeUI.container.AddChild(PlayerLifeUI.voiceBox);
			PlayerLifeUI.voiceBox.IsVisible = false;
			PlayerLifeUI.trackedQuestTitle = Glazier.Get().CreateLabel();
			PlayerLifeUI.trackedQuestTitle.PositionOffset_X = -500f;
			PlayerLifeUI.trackedQuestTitle.PositionOffset_Y = 200f;
			PlayerLifeUI.trackedQuestTitle.PositionScale_X = 1f;
			PlayerLifeUI.trackedQuestTitle.SizeOffset_X = 500f;
			PlayerLifeUI.trackedQuestTitle.SizeOffset_Y = 35f;
			PlayerLifeUI.trackedQuestTitle.AllowRichText = true;
			PlayerLifeUI.trackedQuestTitle.TextColor = 4;
			PlayerLifeUI.trackedQuestTitle.FontSize = 3;
			PlayerLifeUI.trackedQuestTitle.TextAlignment = 8;
			PlayerLifeUI.trackedQuestTitle.TextContrastContext = 2;
			PlayerLifeUI.container.AddChild(PlayerLifeUI.trackedQuestTitle);
			PlayerLifeUI.trackedQuestBar = Glazier.Get().CreateImage();
			PlayerLifeUI.trackedQuestBar.PositionOffset_X = -200f;
			PlayerLifeUI.trackedQuestBar.PositionOffset_Y = 240f;
			PlayerLifeUI.trackedQuestBar.PositionScale_X = 1f;
			PlayerLifeUI.trackedQuestBar.SizeOffset_X = 200f;
			PlayerLifeUI.trackedQuestBar.SizeOffset_Y = 3f;
			PlayerLifeUI.trackedQuestBar.Texture = GlazierResources.PixelTexture;
			PlayerLifeUI.trackedQuestBar.TintColor = 2;
			PlayerLifeUI.container.AddChild(PlayerLifeUI.trackedQuestBar);
			PlayerLifeUI.levelTextBox = Glazier.Get().CreateBox();
			PlayerLifeUI.levelTextBox.PositionOffset_X = -180f;
			PlayerLifeUI.levelTextBox.PositionScale_X = 0.5f;
			PlayerLifeUI.levelTextBox.SizeOffset_X = 300f;
			PlayerLifeUI.levelTextBox.SizeOffset_Y = 50f;
			PlayerLifeUI.levelTextBox.FontSize = 3;
			PlayerLifeUI.container.AddChild(PlayerLifeUI.levelTextBox);
			PlayerLifeUI.levelTextBox.IsVisible = false;
			PlayerLifeUI.levelNumberBox = Glazier.Get().CreateBox();
			PlayerLifeUI.levelNumberBox.PositionOffset_X = 130f;
			PlayerLifeUI.levelNumberBox.PositionScale_X = 0.5f;
			PlayerLifeUI.levelNumberBox.SizeOffset_X = 50f;
			PlayerLifeUI.levelNumberBox.SizeOffset_Y = 50f;
			PlayerLifeUI.levelNumberBox.FontSize = 3;
			PlayerLifeUI.container.AddChild(PlayerLifeUI.levelNumberBox);
			PlayerLifeUI.levelNumberBox.IsVisible = false;
			PlayerLifeUI.cachedCompassSearch = -1;
			PlayerLifeUI.cachedHasCompass = false;
			PlayerLifeUI.compassBox = Glazier.Get().CreateBox();
			PlayerLifeUI.compassBox.PositionOffset_X = -180f;
			PlayerLifeUI.compassBox.PositionScale_X = 0.5f;
			PlayerLifeUI.compassBox.SizeOffset_X = 360f;
			PlayerLifeUI.compassBox.SizeOffset_Y = 50f;
			PlayerLifeUI.compassBox.FontSize = 3;
			PlayerLifeUI.container.AddChild(PlayerLifeUI.compassBox);
			PlayerLifeUI.compassBox.IsVisible = false;
			PlayerLifeUI.compassLabelsContainer = Glazier.Get().CreateFrame();
			PlayerLifeUI.compassLabelsContainer.PositionOffset_X = 10f;
			PlayerLifeUI.compassLabelsContainer.PositionOffset_Y = 10f;
			PlayerLifeUI.compassLabelsContainer.SizeOffset_X = -20f;
			PlayerLifeUI.compassLabelsContainer.SizeOffset_Y = -20f;
			PlayerLifeUI.compassLabelsContainer.SizeScale_X = 1f;
			PlayerLifeUI.compassLabelsContainer.SizeScale_Y = 1f;
			PlayerLifeUI.compassBox.AddChild(PlayerLifeUI.compassLabelsContainer);
			PlayerLifeUI.compassMarkersContainer = Glazier.Get().CreateFrame();
			PlayerLifeUI.compassMarkersContainer.PositionOffset_X = 10f;
			PlayerLifeUI.compassMarkersContainer.PositionOffset_Y = 10f;
			PlayerLifeUI.compassMarkersContainer.SizeOffset_X = -20f;
			PlayerLifeUI.compassMarkersContainer.SizeOffset_Y = -20f;
			PlayerLifeUI.compassMarkersContainer.SizeScale_X = 1f;
			PlayerLifeUI.compassMarkersContainer.SizeScale_Y = 1f;
			PlayerLifeUI.compassBox.AddChild(PlayerLifeUI.compassMarkersContainer);
			PlayerLifeUI.compassMarkers = new List<ISleekImage>();
			PlayerLifeUI.compassMarkersVisibleCount = 0;
			PlayerLifeUI.compassLabels = new ISleekLabel[72];
			for (int k = 0; k < PlayerLifeUI.compassLabels.Length; k++)
			{
				ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
				sleekLabel.PositionOffset_X = -25f;
				sleekLabel.SizeOffset_X = 50f;
				sleekLabel.SizeOffset_Y = 30f;
				sleekLabel.Text = (k * 5).ToString();
				sleekLabel.TextColor = new Color(0.75f, 0.75f, 0.75f);
				PlayerLifeUI.compassLabelsContainer.AddChild(sleekLabel);
				PlayerLifeUI.compassLabels[k] = sleekLabel;
			}
			ISleekLabel compassLabelByAngle = PlayerLifeUI.getCompassLabelByAngle(0);
			compassLabelByAngle.FontSize = 4;
			compassLabelByAngle.Text = "N";
			compassLabelByAngle.TextColor = Palette.COLOR_R;
			ISleekLabel compassLabelByAngle2 = PlayerLifeUI.getCompassLabelByAngle(45);
			compassLabelByAngle2.FontSize = 3;
			compassLabelByAngle2.Text = "NE";
			compassLabelByAngle2.TextColor = new Color(1f, 1f, 1f);
			ISleekLabel compassLabelByAngle3 = PlayerLifeUI.getCompassLabelByAngle(90);
			compassLabelByAngle3.FontSize = 4;
			compassLabelByAngle3.Text = "E";
			compassLabelByAngle3.TextColor = new Color(1f, 1f, 1f);
			ISleekLabel compassLabelByAngle4 = PlayerLifeUI.getCompassLabelByAngle(135);
			compassLabelByAngle4.FontSize = 3;
			compassLabelByAngle4.Text = "SE";
			compassLabelByAngle4.TextColor = new Color(1f, 1f, 1f);
			ISleekLabel compassLabelByAngle5 = PlayerLifeUI.getCompassLabelByAngle(180);
			compassLabelByAngle5.FontSize = 4;
			compassLabelByAngle5.Text = "S";
			compassLabelByAngle5.TextColor = new Color(1f, 1f, 1f);
			ISleekLabel compassLabelByAngle6 = PlayerLifeUI.getCompassLabelByAngle(225);
			compassLabelByAngle6.FontSize = 3;
			compassLabelByAngle6.Text = "SW";
			compassLabelByAngle6.TextColor = new Color(1f, 1f, 1f);
			ISleekLabel compassLabelByAngle7 = PlayerLifeUI.getCompassLabelByAngle(270);
			compassLabelByAngle7.FontSize = 4;
			compassLabelByAngle7.Text = "W";
			compassLabelByAngle7.TextColor = new Color(1f, 1f, 1f);
			ISleekLabel compassLabelByAngle8 = PlayerLifeUI.getCompassLabelByAngle(315);
			compassLabelByAngle8.FontSize = 3;
			compassLabelByAngle8.Text = "NW";
			compassLabelByAngle8.TextColor = new Color(1f, 1f, 1f);
			PlayerLifeUI.hotbarContainer = Glazier.Get().CreateFrame();
			PlayerLifeUI.hotbarContainer.PositionScale_X = 0.5f;
			PlayerLifeUI.hotbarContainer.PositionScale_Y = 1f;
			PlayerLifeUI.hotbarContainer.PositionOffset_Y = -200f;
			PlayerLifeUI.container.AddChild(PlayerLifeUI.hotbarContainer);
			PlayerLifeUI.hotbarContainer.IsVisible = false;
			PlayerLifeUI.cachedHotbarSearch = -1;
			PlayerLifeUI.cachedHotbarValues = new PlayerLifeUI.CachedHotbarItem[10];
			PlayerLifeUI.hotbarImages = new SleekItemIcon[PlayerLifeUI.cachedHotbarValues.Length];
			for (int l = 0; l < PlayerLifeUI.hotbarImages.Length; l++)
			{
				SleekItemIcon sleekItemIcon = new SleekItemIcon();
				sleekItemIcon.color = new Color(1f, 1f, 1f, 0.5f);
				PlayerLifeUI.hotbarContainer.AddChild(sleekItemIcon);
				sleekItemIcon.IsVisible = false;
				PlayerLifeUI.hotbarImages[l] = sleekItemIcon;
			}
			PlayerLifeUI.hotbarLabels = new ISleekLabel[PlayerLifeUI.cachedHotbarValues.Length];
			for (int m = 0; m < PlayerLifeUI.hotbarLabels.Length; m++)
			{
				ISleekLabel sleekLabel2 = Glazier.Get().CreateLabel();
				sleekLabel2.PositionOffset_Y = 5f;
				sleekLabel2.SizeOffset_X = 50f;
				sleekLabel2.SizeOffset_Y = 30f;
				sleekLabel2.Text = ControlsSettings.getEquipmentHotkeyText(m);
				sleekLabel2.TextAlignment = 2;
				sleekLabel2.TextColor = new SleekColor(3, 0.75f);
				sleekLabel2.TextContrastContext = 1;
				PlayerLifeUI.hotbarContainer.AddChild(sleekLabel2);
				sleekLabel2.IsVisible = false;
				PlayerLifeUI.hotbarLabels[m] = sleekLabel2;
			}
			PlayerLifeUI.statTrackerLabel = Glazier.Get().CreateLabel();
			PlayerLifeUI.statTrackerLabel.PositionOffset_X = -100f;
			PlayerLifeUI.statTrackerLabel.PositionOffset_Y = -30f;
			PlayerLifeUI.statTrackerLabel.PositionScale_X = 0.5f;
			PlayerLifeUI.statTrackerLabel.PositionScale_Y = 1f;
			PlayerLifeUI.statTrackerLabel.SizeOffset_X = 200f;
			PlayerLifeUI.statTrackerLabel.SizeOffset_Y = 30f;
			PlayerLifeUI.statTrackerLabel.TextAlignment = 7;
			PlayerLifeUI.statTrackerLabel.FontStyle = 2;
			PlayerLifeUI.statTrackerLabel.FontSize = 2;
			PlayerLifeUI.container.AddChild(PlayerLifeUI.statTrackerLabel);
			PlayerLifeUI.statTrackerLabel.IsVisible = false;
			PlayerLifeUI.scopeOverlay = new SleekScopeOverlay();
			PlayerLifeUI.scopeOverlay.SizeScale_X = 1f;
			PlayerLifeUI.scopeOverlay.SizeScale_Y = 1f;
			PlayerLifeUI.scopeOverlay.IsVisible = false;
			PlayerUI.window.AddChild(PlayerLifeUI.scopeOverlay);
			PlayerLifeUI.binocularsOverlay = Glazier.Get().CreateImage((Texture2D)Resources.Load("Overlay/Binoculars"));
			PlayerLifeUI.binocularsOverlay.SizeScale_X = 1f;
			PlayerLifeUI.binocularsOverlay.SizeScale_Y = 1f;
			PlayerUI.window.AddChild(PlayerLifeUI.binocularsOverlay);
			PlayerLifeUI.binocularsOverlay.IsVisible = false;
			PlayerLifeUI.faceButtons = new ISleekButton[(int)(Customization.FACES_FREE + Customization.FACES_PRO)];
			for (int n = 0; n < PlayerLifeUI.faceButtons.Length; n++)
			{
				float num = 12.566371f * ((float)n / (float)PlayerLifeUI.faceButtons.Length);
				float num2 = 210f;
				if (n >= PlayerLifeUI.faceButtons.Length / 2)
				{
					num += 3.1415927f / (float)(PlayerLifeUI.faceButtons.Length / 2);
					num2 += 30f;
				}
				ISleekButton sleekButton = Glazier.Get().CreateButton();
				sleekButton.PositionOffset_X = (float)((int)(Mathf.Cos(num) * num2) - 20);
				sleekButton.PositionOffset_Y = (float)((int)(Mathf.Sin(num) * num2) - 20);
				sleekButton.PositionScale_X = 0.5f;
				sleekButton.PositionScale_Y = 0.5f;
				sleekButton.SizeOffset_X = 40f;
				sleekButton.SizeOffset_Y = 40f;
				PlayerLifeUI.container.AddChild(sleekButton);
				sleekButton.IsVisible = false;
				ISleekImage sleekImage = Glazier.Get().CreateImage();
				sleekImage.PositionOffset_X = 10f;
				sleekImage.PositionOffset_Y = 10f;
				sleekImage.SizeOffset_X = 20f;
				sleekImage.SizeOffset_Y = 20f;
				sleekImage.Texture = GlazierResources.PixelTexture;
				sleekImage.TintColor = Characters.active.skin;
				sleekButton.AddChild(sleekImage);
				ISleekImage sleekImage2 = Glazier.Get().CreateImage();
				sleekImage2.PositionOffset_X = 2f;
				sleekImage2.PositionOffset_Y = 2f;
				sleekImage2.SizeOffset_X = 16f;
				sleekImage2.SizeOffset_Y = 16f;
				sleekImage2.Texture = (Texture2D)Resources.Load("Faces/" + n.ToString() + "/Texture");
				sleekImage.AddChild(sleekImage2);
				if (n >= (int)Customization.FACES_FREE)
				{
					if (Provider.isPro)
					{
						sleekButton.OnClicked += new ClickedButton(PlayerLifeUI.onClickedFaceButton);
					}
					else
					{
						sleekButton.BackgroundColor = SleekColor.BackgroundIfLight(Palette.PRO);
						Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro/Pro.unity3d");
						ISleekImage sleekImage3 = Glazier.Get().CreateImage();
						sleekImage3.PositionOffset_X = -10f;
						sleekImage3.PositionOffset_Y = -10f;
						sleekImage3.PositionScale_X = 0.5f;
						sleekImage3.PositionScale_Y = 0.5f;
						sleekImage3.SizeOffset_X = 20f;
						sleekImage3.SizeOffset_Y = 20f;
						sleekImage3.Texture = bundle.load<Texture2D>("Lock_Small");
						sleekButton.AddChild(sleekImage3);
						bundle.unload();
					}
				}
				else
				{
					sleekButton.OnClicked += new ClickedButton(PlayerLifeUI.onClickedFaceButton);
				}
				PlayerLifeUI.faceButtons[n] = sleekButton;
			}
			PlayerLifeUI.surrenderButton = Glazier.Get().CreateButton();
			PlayerLifeUI.surrenderButton.PositionOffset_X = -160f;
			PlayerLifeUI.surrenderButton.PositionOffset_Y = -15f;
			PlayerLifeUI.surrenderButton.PositionScale_X = 0.5f;
			PlayerLifeUI.surrenderButton.PositionScale_Y = 0.5f;
			PlayerLifeUI.surrenderButton.SizeOffset_X = 150f;
			PlayerLifeUI.surrenderButton.SizeOffset_Y = 30f;
			PlayerLifeUI.surrenderButton.Text = PlayerLifeUI.localization.format("Surrender");
			PlayerLifeUI.surrenderButton.OnClicked += new ClickedButton(PlayerLifeUI.onClickedSurrenderButton);
			PlayerLifeUI.container.AddChild(PlayerLifeUI.surrenderButton);
			PlayerLifeUI.surrenderButton.IsVisible = false;
			PlayerLifeUI.pointButton = Glazier.Get().CreateButton();
			PlayerLifeUI.pointButton.PositionOffset_X = 10f;
			PlayerLifeUI.pointButton.PositionOffset_Y = -15f;
			PlayerLifeUI.pointButton.PositionScale_X = 0.5f;
			PlayerLifeUI.pointButton.PositionScale_Y = 0.5f;
			PlayerLifeUI.pointButton.SizeOffset_X = 150f;
			PlayerLifeUI.pointButton.SizeOffset_Y = 30f;
			PlayerLifeUI.pointButton.Text = PlayerLifeUI.localization.format("Point");
			PlayerLifeUI.pointButton.OnClicked += new ClickedButton(PlayerLifeUI.onClickedPointButton);
			PlayerLifeUI.container.AddChild(PlayerLifeUI.pointButton);
			PlayerLifeUI.pointButton.IsVisible = false;
			PlayerLifeUI.waveButton = Glazier.Get().CreateButton();
			PlayerLifeUI.waveButton.PositionOffset_X = -75f;
			PlayerLifeUI.waveButton.PositionOffset_Y = -55f;
			PlayerLifeUI.waveButton.PositionScale_X = 0.5f;
			PlayerLifeUI.waveButton.PositionScale_Y = 0.5f;
			PlayerLifeUI.waveButton.SizeOffset_X = 150f;
			PlayerLifeUI.waveButton.SizeOffset_Y = 30f;
			PlayerLifeUI.waveButton.Text = PlayerLifeUI.localization.format("Wave");
			PlayerLifeUI.waveButton.OnClicked += new ClickedButton(PlayerLifeUI.onClickedWaveButton);
			PlayerLifeUI.container.AddChild(PlayerLifeUI.waveButton);
			PlayerLifeUI.waveButton.IsVisible = false;
			PlayerLifeUI.saluteButton = Glazier.Get().CreateButton();
			PlayerLifeUI.saluteButton.PositionOffset_X = -75f;
			PlayerLifeUI.saluteButton.PositionOffset_Y = 25f;
			PlayerLifeUI.saluteButton.PositionScale_X = 0.5f;
			PlayerLifeUI.saluteButton.PositionScale_Y = 0.5f;
			PlayerLifeUI.saluteButton.SizeOffset_X = 150f;
			PlayerLifeUI.saluteButton.SizeOffset_Y = 30f;
			PlayerLifeUI.saluteButton.Text = PlayerLifeUI.localization.format("Salute");
			PlayerLifeUI.saluteButton.OnClicked += new ClickedButton(PlayerLifeUI.onClickedSaluteButton);
			PlayerLifeUI.container.AddChild(PlayerLifeUI.saluteButton);
			PlayerLifeUI.saluteButton.IsVisible = false;
			PlayerLifeUI.restButton = Glazier.Get().CreateButton();
			PlayerLifeUI.restButton.PositionOffset_X = -160f;
			PlayerLifeUI.restButton.PositionOffset_Y = 65f;
			PlayerLifeUI.restButton.PositionScale_X = 0.5f;
			PlayerLifeUI.restButton.PositionScale_Y = 0.5f;
			PlayerLifeUI.restButton.SizeOffset_X = 150f;
			PlayerLifeUI.restButton.SizeOffset_Y = 30f;
			PlayerLifeUI.restButton.Text = PlayerLifeUI.localization.format("Rest");
			PlayerLifeUI.restButton.OnClicked += new ClickedButton(PlayerLifeUI.onClickedRestButton);
			PlayerLifeUI.container.AddChild(PlayerLifeUI.restButton);
			PlayerLifeUI.restButton.IsVisible = false;
			PlayerLifeUI.facepalmButton = Glazier.Get().CreateButton();
			PlayerLifeUI.facepalmButton.PositionOffset_X = 10f;
			PlayerLifeUI.facepalmButton.PositionOffset_Y = -95f;
			PlayerLifeUI.facepalmButton.PositionScale_X = 0.5f;
			PlayerLifeUI.facepalmButton.PositionScale_Y = 0.5f;
			PlayerLifeUI.facepalmButton.SizeOffset_X = 150f;
			PlayerLifeUI.facepalmButton.SizeOffset_Y = 30f;
			PlayerLifeUI.facepalmButton.Text = PlayerLifeUI.localization.format("Facepalm");
			PlayerLifeUI.facepalmButton.OnClicked += new ClickedButton(PlayerLifeUI.onClickedFacepalmButton);
			PlayerLifeUI.container.AddChild(PlayerLifeUI.facepalmButton);
			PlayerLifeUI.facepalmButton.IsVisible = false;
			PlayerLifeUI.activeHitmarkers = new List<HitmarkerInfo>(16);
			PlayerLifeUI.hitmarkersPool = new List<SleekHitmarker>(16);
			for (int num3 = 0; num3 < 16; num3++)
			{
				PlayerLifeUI.ReleaseHitmarker(PlayerLifeUI.NewHitmarker());
			}
			PlayerLifeUI.crosshair = new Crosshair(PlayerLifeUI.icons);
			PlayerLifeUI.crosshair.SizeScale_X = 1f;
			PlayerLifeUI.crosshair.SizeScale_Y = 1f;
			PlayerLifeUI.container.AddChild(PlayerLifeUI.crosshair);
			PlayerLifeUI.crosshair.SetPluginAllowsCenterDotVisible(Player.player.isPluginWidgetFlagActive(EPluginWidgetFlags.ShowCenterDot));
			PlayerLifeUI.lifeBox = Glazier.Get().CreateBox();
			PlayerLifeUI.lifeBox.PositionScale_Y = 1f;
			PlayerLifeUI.lifeBox.SizeScale_X = 0.2f;
			PlayerLifeUI.container.AddChild(PlayerLifeUI.lifeBox);
			PlayerLifeUI.statusIconsContainer = Glazier.Get().CreateFrame();
			PlayerLifeUI.statusIconsContainer.PositionOffset_Y = -60f;
			PlayerLifeUI.statusIconsContainer.PositionScale_Y = 1f;
			PlayerLifeUI.statusIconsContainer.SizeScale_X = 0.2f;
			PlayerLifeUI.statusIconsContainer.SizeOffset_Y = 50f;
			PlayerLifeUI.container.AddChild(PlayerLifeUI.statusIconsContainer);
			PlayerLifeUI.healthIcon = Glazier.Get().CreateImage();
			PlayerLifeUI.healthIcon.PositionOffset_X = 5f;
			PlayerLifeUI.healthIcon.SizeOffset_X = 20f;
			PlayerLifeUI.healthIcon.SizeOffset_Y = 20f;
			PlayerLifeUI.healthIcon.Texture = PlayerLifeUI.icons.load<Texture2D>("Health");
			PlayerLifeUI.lifeBox.AddChild(PlayerLifeUI.healthIcon);
			PlayerLifeUI.healthProgress = new SleekProgress("");
			PlayerLifeUI.healthProgress.PositionOffset_X = 30f;
			PlayerLifeUI.healthProgress.SizeOffset_X = -40f;
			PlayerLifeUI.healthProgress.SizeOffset_Y = 10f;
			PlayerLifeUI.healthProgress.SizeScale_X = 1f;
			PlayerLifeUI.healthProgress.color = Palette.COLOR_R;
			PlayerLifeUI.lifeBox.AddChild(PlayerLifeUI.healthProgress);
			PlayerLifeUI.foodIcon = Glazier.Get().CreateImage();
			PlayerLifeUI.foodIcon.PositionOffset_X = 5f;
			PlayerLifeUI.foodIcon.SizeOffset_X = 20f;
			PlayerLifeUI.foodIcon.SizeOffset_Y = 20f;
			PlayerLifeUI.foodIcon.Texture = PlayerLifeUI.icons.load<Texture2D>("Food");
			PlayerLifeUI.lifeBox.AddChild(PlayerLifeUI.foodIcon);
			PlayerLifeUI.foodProgress = new SleekProgress("");
			PlayerLifeUI.foodProgress.PositionOffset_X = 30f;
			PlayerLifeUI.foodProgress.SizeOffset_X = -40f;
			PlayerLifeUI.foodProgress.SizeOffset_Y = 10f;
			PlayerLifeUI.foodProgress.SizeScale_X = 1f;
			PlayerLifeUI.foodProgress.color = Palette.COLOR_O;
			PlayerLifeUI.lifeBox.AddChild(PlayerLifeUI.foodProgress);
			PlayerLifeUI.waterIcon = Glazier.Get().CreateImage();
			PlayerLifeUI.waterIcon.PositionOffset_X = 5f;
			PlayerLifeUI.waterIcon.SizeOffset_X = 20f;
			PlayerLifeUI.waterIcon.SizeOffset_Y = 20f;
			PlayerLifeUI.waterIcon.Texture = PlayerLifeUI.icons.load<Texture2D>("Water");
			PlayerLifeUI.lifeBox.AddChild(PlayerLifeUI.waterIcon);
			PlayerLifeUI.waterProgress = new SleekProgress("");
			PlayerLifeUI.waterProgress.PositionOffset_X = 30f;
			PlayerLifeUI.waterProgress.SizeOffset_X = -40f;
			PlayerLifeUI.waterProgress.SizeOffset_Y = 10f;
			PlayerLifeUI.waterProgress.SizeScale_X = 1f;
			PlayerLifeUI.waterProgress.color = Palette.COLOR_B;
			PlayerLifeUI.lifeBox.AddChild(PlayerLifeUI.waterProgress);
			PlayerLifeUI.virusIcon = Glazier.Get().CreateImage();
			PlayerLifeUI.virusIcon.PositionOffset_X = 5f;
			PlayerLifeUI.virusIcon.SizeOffset_X = 20f;
			PlayerLifeUI.virusIcon.SizeOffset_Y = 20f;
			PlayerLifeUI.virusIcon.Texture = PlayerLifeUI.icons.load<Texture2D>("Virus");
			PlayerLifeUI.lifeBox.AddChild(PlayerLifeUI.virusIcon);
			PlayerLifeUI.virusProgress = new SleekProgress("");
			PlayerLifeUI.virusProgress.PositionOffset_X = 30f;
			PlayerLifeUI.virusProgress.SizeOffset_X = -40f;
			PlayerLifeUI.virusProgress.SizeOffset_Y = 10f;
			PlayerLifeUI.virusProgress.SizeScale_X = 1f;
			PlayerLifeUI.virusProgress.color = Palette.COLOR_G;
			PlayerLifeUI.lifeBox.AddChild(PlayerLifeUI.virusProgress);
			PlayerLifeUI.staminaIcon = Glazier.Get().CreateImage();
			PlayerLifeUI.staminaIcon.PositionOffset_X = 5f;
			PlayerLifeUI.staminaIcon.SizeOffset_X = 20f;
			PlayerLifeUI.staminaIcon.SizeOffset_Y = 20f;
			PlayerLifeUI.staminaIcon.Texture = PlayerLifeUI.icons.load<Texture2D>("Stamina");
			PlayerLifeUI.lifeBox.AddChild(PlayerLifeUI.staminaIcon);
			PlayerLifeUI.staminaProgress = new SleekProgress("");
			PlayerLifeUI.staminaProgress.PositionOffset_X = 30f;
			PlayerLifeUI.staminaProgress.SizeOffset_X = -40f;
			PlayerLifeUI.staminaProgress.SizeOffset_Y = 10f;
			PlayerLifeUI.staminaProgress.SizeScale_X = 1f;
			PlayerLifeUI.staminaProgress.color = Palette.COLOR_Y;
			PlayerLifeUI.lifeBox.AddChild(PlayerLifeUI.staminaProgress);
			PlayerLifeUI.waveLabel = Glazier.Get().CreateLabel();
			PlayerLifeUI.waveLabel.SizeOffset_Y = 30f;
			PlayerLifeUI.waveLabel.SizeScale_X = 0.5f;
			PlayerLifeUI.lifeBox.AddChild(PlayerLifeUI.waveLabel);
			PlayerLifeUI.scoreLabel = Glazier.Get().CreateLabel();
			PlayerLifeUI.scoreLabel.PositionScale_X = 0.5f;
			PlayerLifeUI.scoreLabel.SizeOffset_Y = 30f;
			PlayerLifeUI.scoreLabel.SizeScale_X = 0.5f;
			PlayerLifeUI.lifeBox.AddChild(PlayerLifeUI.scoreLabel);
			PlayerLifeUI.oxygenIcon = Glazier.Get().CreateImage();
			PlayerLifeUI.oxygenIcon.PositionOffset_X = 5f;
			PlayerLifeUI.oxygenIcon.SizeOffset_X = 20f;
			PlayerLifeUI.oxygenIcon.SizeOffset_Y = 20f;
			PlayerLifeUI.oxygenIcon.Texture = PlayerLifeUI.icons.load<Texture2D>("Oxygen");
			PlayerLifeUI.lifeBox.AddChild(PlayerLifeUI.oxygenIcon);
			PlayerLifeUI.oxygenProgress = new SleekProgress("");
			PlayerLifeUI.oxygenProgress.PositionOffset_X = 30f;
			PlayerLifeUI.oxygenProgress.SizeOffset_X = -40f;
			PlayerLifeUI.oxygenProgress.SizeOffset_Y = 10f;
			PlayerLifeUI.oxygenProgress.SizeScale_X = 1f;
			PlayerLifeUI.oxygenProgress.color = Palette.COLOR_W;
			PlayerLifeUI.lifeBox.AddChild(PlayerLifeUI.oxygenProgress);
			PlayerLifeUI.vehicleBox = Glazier.Get().CreateBox();
			PlayerLifeUI.vehicleBox.PositionOffset_Y = -120f;
			PlayerLifeUI.vehicleBox.PositionScale_X = 0.8f;
			PlayerLifeUI.vehicleBox.PositionScale_Y = 1f;
			PlayerLifeUI.vehicleBox.SizeOffset_Y = 120f;
			PlayerLifeUI.vehicleBox.SizeScale_X = 0.2f;
			PlayerLifeUI.container.AddChild(PlayerLifeUI.vehicleBox);
			PlayerLifeUI.vehicleVisibleByDefault = false;
			PlayerLifeUI.fuelIcon = Glazier.Get().CreateImage();
			PlayerLifeUI.fuelIcon.PositionOffset_X = 5f;
			PlayerLifeUI.fuelIcon.PositionOffset_Y = 5f;
			PlayerLifeUI.fuelIcon.SizeOffset_X = 20f;
			PlayerLifeUI.fuelIcon.SizeOffset_Y = 20f;
			PlayerLifeUI.fuelIcon.Texture = PlayerLifeUI.icons.load<Texture2D>("Fuel");
			PlayerLifeUI.vehicleBox.AddChild(PlayerLifeUI.fuelIcon);
			PlayerLifeUI.fuelProgress = new SleekProgress("");
			PlayerLifeUI.fuelProgress.PositionOffset_X = 30f;
			PlayerLifeUI.fuelProgress.PositionOffset_Y = 10f;
			PlayerLifeUI.fuelProgress.SizeOffset_X = -40f;
			PlayerLifeUI.fuelProgress.SizeOffset_Y = 10f;
			PlayerLifeUI.fuelProgress.SizeScale_X = 1f;
			PlayerLifeUI.fuelProgress.color = Palette.COLOR_Y;
			PlayerLifeUI.vehicleBox.AddChild(PlayerLifeUI.fuelProgress);
			PlayerLifeUI.speedIcon = Glazier.Get().CreateImage();
			PlayerLifeUI.speedIcon.PositionOffset_X = 5f;
			PlayerLifeUI.speedIcon.PositionOffset_Y = 35f;
			PlayerLifeUI.speedIcon.SizeOffset_X = 20f;
			PlayerLifeUI.speedIcon.SizeOffset_Y = 20f;
			PlayerLifeUI.speedIcon.Texture = PlayerLifeUI.icons.load<Texture2D>("Speed");
			PlayerLifeUI.vehicleBox.AddChild(PlayerLifeUI.speedIcon);
			PlayerLifeUI.speedProgress = new SleekProgress(OptionsSettings.metric ? " kph" : " mph");
			PlayerLifeUI.speedProgress.PositionOffset_X = 30f;
			PlayerLifeUI.speedProgress.PositionOffset_Y = 40f;
			PlayerLifeUI.speedProgress.SizeOffset_X = -40f;
			PlayerLifeUI.speedProgress.SizeOffset_Y = 10f;
			PlayerLifeUI.speedProgress.SizeScale_X = 1f;
			PlayerLifeUI.speedProgress.color = Palette.COLOR_P;
			PlayerLifeUI.vehicleBox.AddChild(PlayerLifeUI.speedProgress);
			PlayerLifeUI.hpIcon = Glazier.Get().CreateImage();
			PlayerLifeUI.hpIcon.PositionOffset_X = 5f;
			PlayerLifeUI.hpIcon.PositionOffset_Y = 65f;
			PlayerLifeUI.hpIcon.SizeOffset_X = 20f;
			PlayerLifeUI.hpIcon.SizeOffset_Y = 20f;
			PlayerLifeUI.hpIcon.Texture = PlayerLifeUI.icons.load<Texture2D>("Health");
			PlayerLifeUI.vehicleBox.AddChild(PlayerLifeUI.hpIcon);
			PlayerLifeUI.hpProgress = new SleekProgress("");
			PlayerLifeUI.hpProgress.PositionOffset_X = 30f;
			PlayerLifeUI.hpProgress.PositionOffset_Y = 70f;
			PlayerLifeUI.hpProgress.SizeOffset_X = -40f;
			PlayerLifeUI.hpProgress.SizeOffset_Y = 10f;
			PlayerLifeUI.hpProgress.SizeScale_X = 1f;
			PlayerLifeUI.hpProgress.color = Palette.COLOR_R;
			PlayerLifeUI.vehicleBox.AddChild(PlayerLifeUI.hpProgress);
			PlayerLifeUI.batteryChargeIcon = Glazier.Get().CreateImage();
			PlayerLifeUI.batteryChargeIcon.PositionOffset_X = 5f;
			PlayerLifeUI.batteryChargeIcon.PositionOffset_Y = 95f;
			PlayerLifeUI.batteryChargeIcon.SizeOffset_X = 20f;
			PlayerLifeUI.batteryChargeIcon.SizeOffset_Y = 20f;
			PlayerLifeUI.batteryChargeIcon.Texture = PlayerLifeUI.icons.load<Texture2D>("Stamina");
			PlayerLifeUI.vehicleBox.AddChild(PlayerLifeUI.batteryChargeIcon);
			PlayerLifeUI.batteryChargeProgress = new SleekProgress("");
			PlayerLifeUI.batteryChargeProgress.PositionOffset_X = 30f;
			PlayerLifeUI.batteryChargeProgress.PositionOffset_Y = 100f;
			PlayerLifeUI.batteryChargeProgress.SizeOffset_X = -40f;
			PlayerLifeUI.batteryChargeProgress.SizeOffset_Y = 10f;
			PlayerLifeUI.batteryChargeProgress.SizeScale_X = 1f;
			PlayerLifeUI.batteryChargeProgress.color = Palette.COLOR_Y;
			PlayerLifeUI.vehicleBox.AddChild(PlayerLifeUI.batteryChargeProgress);
			PlayerLifeUI.vehicleLockedLabel = Glazier.Get().CreateLabel();
			PlayerLifeUI.vehicleLockedLabel.PositionOffset_Y = -25f;
			PlayerLifeUI.vehicleLockedLabel.SizeScale_X = 1f;
			PlayerLifeUI.vehicleLockedLabel.SizeOffset_Y = 30f;
			PlayerLifeUI.vehicleLockedLabel.TextContrastContext = 2;
			PlayerLifeUI.vehicleBox.AddChild(PlayerLifeUI.vehicleLockedLabel);
			PlayerLifeUI.vehicleEngineLabel = Glazier.Get().CreateLabel();
			PlayerLifeUI.vehicleEngineLabel.SizeScale_X = 1f;
			PlayerLifeUI.vehicleEngineLabel.SizeOffset_Y = 30f;
			PlayerLifeUI.vehicleBox.AddChild(PlayerLifeUI.vehicleEngineLabel);
			PlayerLifeUI.gasmaskBox = Glazier.Get().CreateBox();
			PlayerLifeUI.gasmaskBox.PositionOffset_X = -200f;
			PlayerLifeUI.gasmaskBox.PositionOffset_Y = -60f;
			PlayerLifeUI.gasmaskBox.PositionScale_X = 0.5f;
			PlayerLifeUI.gasmaskBox.PositionScale_Y = 1f;
			PlayerLifeUI.gasmaskBox.SizeOffset_X = 400f;
			PlayerLifeUI.gasmaskBox.SizeOffset_Y = 60f;
			PlayerLifeUI.container.AddChild(PlayerLifeUI.gasmaskBox);
			PlayerLifeUI.gasmaskBox.IsVisible = false;
			PlayerLifeUI.gasmaskIcon = new SleekItemIcon();
			PlayerLifeUI.gasmaskIcon.PositionOffset_X = 5f;
			PlayerLifeUI.gasmaskIcon.PositionOffset_Y = 5f;
			PlayerLifeUI.gasmaskIcon.SizeOffset_X = 50f;
			PlayerLifeUI.gasmaskIcon.SizeOffset_Y = 50f;
			PlayerLifeUI.gasmaskBox.AddChild(PlayerLifeUI.gasmaskIcon);
			PlayerLifeUI.gasmaskProgress = new SleekProgress("");
			PlayerLifeUI.gasmaskProgress.PositionOffset_X = 60f;
			PlayerLifeUI.gasmaskProgress.PositionOffset_Y = 10f;
			PlayerLifeUI.gasmaskProgress.SizeOffset_X = -70f;
			PlayerLifeUI.gasmaskProgress.SizeOffset_Y = 40f;
			PlayerLifeUI.gasmaskProgress.SizeScale_X = 1f;
			PlayerLifeUI.gasmaskBox.AddChild(PlayerLifeUI.gasmaskProgress);
			PlayerLifeUI.bleedingBox = new SleekBoxIcon(PlayerLifeUI.icons.load<Texture2D>("Bleeding"));
			PlayerLifeUI.bleedingBox.SizeOffset_X = 50f;
			PlayerLifeUI.bleedingBox.SizeOffset_Y = 50f;
			PlayerLifeUI.statusIconsContainer.AddChild(PlayerLifeUI.bleedingBox);
			PlayerLifeUI.bleedingBox.IsVisible = false;
			PlayerLifeUI.brokenBox = new SleekBoxIcon(PlayerLifeUI.icons.load<Texture2D>("Broken"));
			PlayerLifeUI.brokenBox.SizeOffset_X = 50f;
			PlayerLifeUI.brokenBox.SizeOffset_Y = 50f;
			PlayerLifeUI.statusIconsContainer.AddChild(PlayerLifeUI.brokenBox);
			PlayerLifeUI.brokenBox.IsVisible = false;
			PlayerLifeUI.temperatureBox = new SleekBoxIcon(null);
			PlayerLifeUI.temperatureBox.SizeOffset_X = 50f;
			PlayerLifeUI.temperatureBox.SizeOffset_Y = 50f;
			PlayerLifeUI.statusIconsContainer.AddChild(PlayerLifeUI.temperatureBox);
			PlayerLifeUI.temperatureBox.IsVisible = false;
			PlayerLifeUI.starvedBox = new SleekBoxIcon(PlayerLifeUI.icons.load<Texture2D>("Starved"));
			PlayerLifeUI.starvedBox.SizeOffset_X = 50f;
			PlayerLifeUI.starvedBox.SizeOffset_Y = 50f;
			PlayerLifeUI.statusIconsContainer.AddChild(PlayerLifeUI.starvedBox);
			PlayerLifeUI.starvedBox.IsVisible = false;
			PlayerLifeUI.dehydratedBox = new SleekBoxIcon(PlayerLifeUI.icons.load<Texture2D>("Dehydrated"));
			PlayerLifeUI.dehydratedBox.SizeOffset_X = 50f;
			PlayerLifeUI.dehydratedBox.SizeOffset_Y = 50f;
			PlayerLifeUI.statusIconsContainer.AddChild(PlayerLifeUI.dehydratedBox);
			PlayerLifeUI.dehydratedBox.IsVisible = false;
			PlayerLifeUI.infectedBox = new SleekBoxIcon(PlayerLifeUI.icons.load<Texture2D>("Infected"));
			PlayerLifeUI.infectedBox.SizeOffset_X = 50f;
			PlayerLifeUI.infectedBox.SizeOffset_Y = 50f;
			PlayerLifeUI.statusIconsContainer.AddChild(PlayerLifeUI.infectedBox);
			PlayerLifeUI.infectedBox.IsVisible = false;
			PlayerLifeUI.drownedBox = new SleekBoxIcon(PlayerLifeUI.icons.load<Texture2D>("Drowned"));
			PlayerLifeUI.drownedBox.SizeOffset_X = 50f;
			PlayerLifeUI.drownedBox.SizeOffset_Y = 50f;
			PlayerLifeUI.statusIconsContainer.AddChild(PlayerLifeUI.drownedBox);
			PlayerLifeUI.drownedBox.IsVisible = false;
			PlayerLifeUI.asphyxiatingBox = new SleekBoxIcon(PlayerLifeUI.icons.load<Texture2D>("AsphyxiatingStatus"));
			PlayerLifeUI.asphyxiatingBox.SizeOffset_X = 50f;
			PlayerLifeUI.asphyxiatingBox.SizeOffset_Y = 50f;
			PlayerLifeUI.statusIconsContainer.AddChild(PlayerLifeUI.asphyxiatingBox);
			PlayerLifeUI.asphyxiatingBox.IsVisible = false;
			PlayerLifeUI.moonBox = new SleekBoxIcon(PlayerLifeUI.icons.load<Texture2D>("Moon"));
			PlayerLifeUI.moonBox.SizeOffset_X = 50f;
			PlayerLifeUI.moonBox.SizeOffset_Y = 50f;
			PlayerLifeUI.statusIconsContainer.AddChild(PlayerLifeUI.moonBox);
			PlayerLifeUI.moonBox.IsVisible = false;
			PlayerLifeUI.radiationBox = new SleekBoxIcon(PlayerLifeUI.icons.load<Texture2D>("Deadzone"));
			PlayerLifeUI.radiationBox.SizeOffset_X = 50f;
			PlayerLifeUI.radiationBox.SizeOffset_Y = 50f;
			PlayerLifeUI.statusIconsContainer.AddChild(PlayerLifeUI.radiationBox);
			PlayerLifeUI.radiationBox.IsVisible = false;
			PlayerLifeUI.safeBox = new SleekBoxIcon(PlayerLifeUI.icons.load<Texture2D>("Safe"));
			PlayerLifeUI.safeBox.SizeOffset_X = 50f;
			PlayerLifeUI.safeBox.SizeOffset_Y = 50f;
			PlayerLifeUI.statusIconsContainer.AddChild(PlayerLifeUI.safeBox);
			PlayerLifeUI.safeBox.IsVisible = false;
			PlayerLifeUI.arrestBox = new SleekBoxIcon(PlayerLifeUI.icons.load<Texture2D>("Arrest"));
			PlayerLifeUI.arrestBox.SizeOffset_X = 50f;
			PlayerLifeUI.arrestBox.SizeOffset_Y = 50f;
			PlayerLifeUI.statusIconsContainer.AddChild(PlayerLifeUI.arrestBox);
			PlayerLifeUI.arrestBox.IsVisible = false;
			PlayerLifeUI.updateIcons();
			PlayerLifeUI.updateLifeBoxVisibility();
			PlayerLifeUI.UpdateVehicleBoxVisibility();
			OptionsSettings.OnUnitSystemChanged += new Action(this.OnUnitSystemChanged);
			Player.player.onLocalPluginWidgetFlagsChanged += PlayerLifeUI.OnLocalPluginWidgetFlagsChanged;
			PlayerLife life = Player.player.life;
			life.onDamaged = (Damaged)Delegate.Combine(life.onDamaged, new Damaged(PlayerLifeUI.onDamaged));
			Player.player.life.onHealthUpdated = new HealthUpdated(PlayerLifeUI.onHealthUpdated);
			Player.player.life.onFoodUpdated = new FoodUpdated(PlayerLifeUI.onFoodUpdated);
			Player.player.life.onWaterUpdated = new WaterUpdated(PlayerLifeUI.onWaterUpdated);
			Player.player.life.onVirusUpdated = new VirusUpdated(PlayerLifeUI.onVirusUpdated);
			Player.player.life.onStaminaUpdated = new StaminaUpdated(PlayerLifeUI.onStaminaUpdated);
			Player.player.life.onOxygenUpdated = new OxygenUpdated(PlayerLifeUI.onOxygenUpdated);
			Player.player.life.OnIsAsphyxiatingChanged += new Action(PlayerLifeUI.OnIsAsphyxiatingChanged);
			Player.player.life.onBleedingUpdated = new BleedingUpdated(PlayerLifeUI.onBleedingUpdated);
			Player.player.life.onBrokenUpdated = new BrokenUpdated(PlayerLifeUI.onBrokenUpdated);
			Player.player.life.onTemperatureUpdated = new TemperatureUpdated(PlayerLifeUI.onTemperatureUpdated);
			PlayerLook look = Player.player.look;
			look.onPerspectiveUpdated = (PerspectiveUpdated)Delegate.Combine(look.onPerspectiveUpdated, new PerspectiveUpdated(PlayerLifeUI.onPerspectiveUpdated));
			PlayerMovement movement = Player.player.movement;
			movement.onSeated = (Seated)Delegate.Combine(movement.onSeated, new Seated(PlayerLifeUI.onSeated));
			PlayerMovement movement2 = Player.player.movement;
			movement2.onVehicleUpdated = (VehicleUpdated)Delegate.Combine(movement2.onVehicleUpdated, new VehicleUpdated(PlayerLifeUI.onVehicleUpdated));
			PlayerMovement movement3 = Player.player.movement;
			movement3.onSafetyUpdated = (SafetyUpdated)Delegate.Combine(movement3.onSafetyUpdated, new SafetyUpdated(PlayerLifeUI.onSafetyUpdated));
			PlayerMovement movement4 = Player.player.movement;
			movement4.onRadiationUpdated = (RadiationUpdated)Delegate.Combine(movement4.onRadiationUpdated, new RadiationUpdated(PlayerLifeUI.onRadiationUpdated));
			PlayerAnimator animator = Player.player.animator;
			animator.onGestureUpdated = (GestureUpdated)Delegate.Combine(animator.onGestureUpdated, new GestureUpdated(PlayerLifeUI.onGestureUpdated));
			PlayerEquipment equipment = Player.player.equipment;
			equipment.onHotkeysUpdated = (HotkeysUpdated)Delegate.Combine(equipment.onHotkeysUpdated, new HotkeysUpdated(PlayerLifeUI.onHotkeysUpdated));
			Player.player.voice.onTalkingChanged += PlayerLifeUI.onTalked;
			Player.player.quests.TrackedQuestUpdated += PlayerLifeUI.OnTrackedQuestUpdated;
			PlayerSkills skills = Player.player.skills;
			skills.onExperienceUpdated = (ExperienceUpdated)Delegate.Combine(skills.onExperienceUpdated, new ExperienceUpdated(PlayerLifeUI.onExperienceUpdated));
			LightingManager.onMoonUpdated = (MoonUpdated)Delegate.Combine(LightingManager.onMoonUpdated, new MoonUpdated(PlayerLifeUI.onMoonUpdated));
			ZombieManager.onWaveUpdated = (WaveUpdated)Delegate.Combine(ZombieManager.onWaveUpdated, new WaveUpdated(PlayerLifeUI.onWaveUpdated));
			PlayerClothing clothing = Player.player.clothing;
			clothing.onMaskUpdated = (MaskUpdated)Delegate.Combine(clothing.onMaskUpdated, new MaskUpdated(PlayerLifeUI.onMaskUpdated));
			PlayerLifeUI.OnChatMessageReceived();
			ChatManager.onChatMessageReceived = (ChatMessageReceivedHandler)Delegate.Combine(ChatManager.onChatMessageReceived, new ChatMessageReceivedHandler(PlayerLifeUI.OnChatMessageReceived));
			ChatManager.onVotingStart = (VotingStart)Delegate.Combine(ChatManager.onVotingStart, new VotingStart(PlayerLifeUI.onVotingStart));
			ChatManager.onVotingUpdate = (VotingUpdate)Delegate.Combine(ChatManager.onVotingUpdate, new VotingUpdate(PlayerLifeUI.onVotingUpdate));
			ChatManager.onVotingStop = (VotingStop)Delegate.Combine(ChatManager.onVotingStop, new VotingStop(PlayerLifeUI.onVotingStop));
			ChatManager.onVotingMessage = (VotingMessage)Delegate.Combine(ChatManager.onVotingMessage, new VotingMessage(PlayerLifeUI.onVotingMessage));
			LevelManager.onArenaMessageUpdated = (ArenaMessageUpdated)Delegate.Combine(LevelManager.onArenaMessageUpdated, new ArenaMessageUpdated(PlayerLifeUI.onArenaMessageUpdated));
			LevelManager.onArenaPlayerUpdated = (ArenaPlayerUpdated)Delegate.Combine(LevelManager.onArenaPlayerUpdated, new ArenaPlayerUpdated(PlayerLifeUI.onArenaPlayerUpdated));
			LevelManager.onLevelNumberUpdated = (LevelNumberUpdated)Delegate.Combine(LevelManager.onLevelNumberUpdated, new LevelNumberUpdated(PlayerLifeUI.onLevelNumberUpdated));
		}

		// Token: 0x060043B6 RID: 17334 RVA: 0x00181DAC File Offset: 0x0017FFAC
		private static SleekHitmarker NewHitmarker()
		{
			SleekHitmarker sleekHitmarker = new SleekHitmarker();
			sleekHitmarker.PositionOffset_X = -64f;
			sleekHitmarker.PositionOffset_Y = -64f;
			sleekHitmarker.SizeOffset_X = 128f;
			sleekHitmarker.SizeOffset_Y = 128f;
			PlayerUI.window.AddChild(sleekHitmarker);
			return sleekHitmarker;
		}

		// Token: 0x060043B7 RID: 17335 RVA: 0x00181DF7 File Offset: 0x0017FFF7
		internal static SleekHitmarker ClaimHitmarker()
		{
			if (PlayerLifeUI.hitmarkersPool.Count > 0)
			{
				return PlayerLifeUI.hitmarkersPool.GetAndRemoveTail<SleekHitmarker>();
			}
			return PlayerLifeUI.NewHitmarker();
		}

		// Token: 0x060043B8 RID: 17336 RVA: 0x00181E16 File Offset: 0x00180016
		internal static void ReleaseHitmarker(SleekHitmarker hitmarker)
		{
			hitmarker.IsVisible = false;
			PlayerLifeUI.hitmarkersPool.Add(hitmarker);
		}

		// Token: 0x04002CA8 RID: 11432
		public static Local localization;

		// Token: 0x04002CA9 RID: 11433
		public static Bundle icons;

		// Token: 0x04002CAA RID: 11434
		private static SleekFullscreenBox _container;

		// Token: 0x04002CAB RID: 11435
		public static bool active;

		// Token: 0x04002CAC RID: 11436
		public static bool chatting;

		// Token: 0x04002CAD RID: 11437
		public static bool gesturing;

		// Token: 0x04002CAE RID: 11438
		public static InteractableObjectNPC npc;

		// Token: 0x04002CAF RID: 11439
		public static bool isVoteMessaged;

		// Token: 0x04002CB0 RID: 11440
		public static float lastVoteMessage;

		// Token: 0x04002CB1 RID: 11441
		private static ISleekScrollView chatHistoryBoxV1;

		// Token: 0x04002CB2 RID: 11442
		private static SleekChatEntryV1[] chatHistoryLabelsV1;

		// Token: 0x04002CB3 RID: 11443
		private static SleekChatEntryV1[] chatPreviewLabelsV1;

		// Token: 0x04002CB4 RID: 11444
		private static ISleekScrollView chatScrollViewV2;

		// Token: 0x04002CB5 RID: 11445
		private static Queue<SleekChatEntryV2> chatEntriesV2;

		// Token: 0x04002CB6 RID: 11446
		public static ISleekField chatField;

		// Token: 0x04002CB7 RID: 11447
		private static SleekButtonState chatModeButton;

		// Token: 0x04002CB8 RID: 11448
		private static SleekButtonIcon sendChatButton;

		// Token: 0x04002CB9 RID: 11449
		public static ISleekBox voteBox;

		// Token: 0x04002CBA RID: 11450
		private static ISleekLabel voteInfoLabel;

		// Token: 0x04002CBB RID: 11451
		private static ISleekLabel votesNeededLabel;

		// Token: 0x04002CBC RID: 11452
		private static ISleekLabel voteYesLabel;

		// Token: 0x04002CBD RID: 11453
		private static ISleekLabel voteNoLabel;

		// Token: 0x04002CBE RID: 11454
		private static SleekBoxIcon voiceBox;

		// Token: 0x04002CBF RID: 11455
		private static ISleekLabel trackedQuestTitle;

		// Token: 0x04002CC0 RID: 11456
		private static ISleekImage trackedQuestBar;

		// Token: 0x04002CC1 RID: 11457
		private static ISleekBox levelTextBox;

		// Token: 0x04002CC2 RID: 11458
		private static ISleekBox levelNumberBox;

		// Token: 0x04002CC3 RID: 11459
		public static ISleekBox compassBox;

		// Token: 0x04002CC4 RID: 11460
		private static ISleekElement compassLabelsContainer;

		// Token: 0x04002CC5 RID: 11461
		private static ISleekElement compassMarkersContainer;

		// Token: 0x04002CC6 RID: 11462
		private static List<ISleekImage> compassMarkers;

		// Token: 0x04002CC7 RID: 11463
		private static int compassMarkersVisibleCount;

		// Token: 0x04002CC8 RID: 11464
		private static ISleekLabel[] compassLabels;

		// Token: 0x04002CC9 RID: 11465
		private static ISleekElement hotbarContainer;

		// Token: 0x04002CCA RID: 11466
		private static SleekItemIcon[] hotbarImages;

		// Token: 0x04002CCB RID: 11467
		private static ISleekLabel[] hotbarLabels;

		// Token: 0x04002CCC RID: 11468
		private static PlayerLifeUI.CachedHotbarItem[] cachedHotbarValues;

		// Token: 0x04002CCD RID: 11469
		public static ISleekLabel statTrackerLabel;

		// Token: 0x04002CCE RID: 11470
		private static ISleekButton[] faceButtons;

		// Token: 0x04002CCF RID: 11471
		private static ISleekButton surrenderButton;

		// Token: 0x04002CD0 RID: 11472
		private static ISleekButton pointButton;

		// Token: 0x04002CD1 RID: 11473
		private static ISleekButton waveButton;

		// Token: 0x04002CD2 RID: 11474
		private static ISleekButton saluteButton;

		// Token: 0x04002CD3 RID: 11475
		private static ISleekButton restButton;

		// Token: 0x04002CD4 RID: 11476
		private static ISleekButton facepalmButton;

		// Token: 0x04002CD5 RID: 11477
		public static SleekScopeOverlay scopeOverlay;

		// Token: 0x04002CD6 RID: 11478
		public static ISleekImage binocularsOverlay;

		// Token: 0x04002CD7 RID: 11479
		public static Crosshair crosshair;

		// Token: 0x04002CD8 RID: 11480
		private static ISleekBox lifeBox;

		// Token: 0x04002CD9 RID: 11481
		private static ISleekImage healthIcon;

		// Token: 0x04002CDA RID: 11482
		private static SleekProgress healthProgress;

		// Token: 0x04002CDB RID: 11483
		private static ISleekImage foodIcon;

		// Token: 0x04002CDC RID: 11484
		private static SleekProgress foodProgress;

		// Token: 0x04002CDD RID: 11485
		private static ISleekImage waterIcon;

		// Token: 0x04002CDE RID: 11486
		private static SleekProgress waterProgress;

		// Token: 0x04002CDF RID: 11487
		private static ISleekImage virusIcon;

		// Token: 0x04002CE0 RID: 11488
		private static SleekProgress virusProgress;

		// Token: 0x04002CE1 RID: 11489
		private static ISleekImage staminaIcon;

		// Token: 0x04002CE2 RID: 11490
		private static SleekProgress staminaProgress;

		// Token: 0x04002CE3 RID: 11491
		private static ISleekLabel waveLabel;

		// Token: 0x04002CE4 RID: 11492
		private static ISleekLabel scoreLabel;

		// Token: 0x04002CE5 RID: 11493
		private static ISleekImage oxygenIcon;

		// Token: 0x04002CE6 RID: 11494
		private static SleekProgress oxygenProgress;

		// Token: 0x04002CE7 RID: 11495
		private static ISleekBox vehicleBox;

		// Token: 0x04002CE8 RID: 11496
		private static ISleekImage fuelIcon;

		// Token: 0x04002CE9 RID: 11497
		private static SleekProgress fuelProgress;

		// Token: 0x04002CEA RID: 11498
		private static ISleekLabel vehicleLockedLabel;

		// Token: 0x04002CEB RID: 11499
		private static ISleekLabel vehicleEngineLabel;

		// Token: 0x04002CEC RID: 11500
		private static bool vehicleVisibleByDefault;

		// Token: 0x04002CED RID: 11501
		private static ISleekBox gasmaskBox;

		// Token: 0x04002CEE RID: 11502
		private static SleekItemIcon gasmaskIcon;

		// Token: 0x04002CEF RID: 11503
		private static SleekProgress gasmaskProgress;

		// Token: 0x04002CF0 RID: 11504
		private static ISleekImage speedIcon;

		// Token: 0x04002CF1 RID: 11505
		private static SleekProgress speedProgress;

		// Token: 0x04002CF2 RID: 11506
		private static ISleekImage batteryChargeIcon;

		// Token: 0x04002CF3 RID: 11507
		private static SleekProgress batteryChargeProgress;

		// Token: 0x04002CF4 RID: 11508
		private static ISleekImage hpIcon;

		// Token: 0x04002CF5 RID: 11509
		private static SleekProgress hpProgress;

		// Token: 0x04002CF6 RID: 11510
		private static ISleekElement statusIconsContainer;

		// Token: 0x04002CF7 RID: 11511
		private static SleekBoxIcon bleedingBox;

		// Token: 0x04002CF8 RID: 11512
		private static SleekBoxIcon brokenBox;

		// Token: 0x04002CF9 RID: 11513
		private static SleekBoxIcon temperatureBox;

		// Token: 0x04002CFA RID: 11514
		private static SleekBoxIcon starvedBox;

		// Token: 0x04002CFB RID: 11515
		private static SleekBoxIcon dehydratedBox;

		// Token: 0x04002CFC RID: 11516
		private static SleekBoxIcon infectedBox;

		// Token: 0x04002CFD RID: 11517
		private static SleekBoxIcon drownedBox;

		// Token: 0x04002CFE RID: 11518
		private static SleekBoxIcon asphyxiatingBox;

		// Token: 0x04002CFF RID: 11519
		private static SleekBoxIcon moonBox;

		// Token: 0x04002D00 RID: 11520
		private static SleekBoxIcon radiationBox;

		// Token: 0x04002D01 RID: 11521
		private static SleekBoxIcon safeBox;

		// Token: 0x04002D02 RID: 11522
		private static SleekBoxIcon arrestBox;

		/// <summary>
		/// Reset to -1 when not chatting. If player presses up/down we get index 0 (most recent).
		/// </summary>
		// Token: 0x04002D03 RID: 11523
		private static int repeatChatIndex = -1;

		// Token: 0x04002D04 RID: 11524
		private static int cachedHotbarSearch;

		// Token: 0x04002D05 RID: 11525
		private static int cachedCompassSearch;

		// Token: 0x04002D06 RID: 11526
		private static bool cachedHasCompass;

		// Token: 0x04002D07 RID: 11527
		internal static List<HitmarkerInfo> activeHitmarkers;

		// Token: 0x04002D08 RID: 11528
		private static List<SleekHitmarker> hitmarkersPool;

		// Token: 0x04002D09 RID: 11529
		private static List<bool> areConditionsMet = new List<bool>(8);

		// Token: 0x02000A10 RID: 2576
		private struct CachedHotbarItem
		{
			// Token: 0x04003514 RID: 13588
			public ushort id;

			// Token: 0x04003515 RID: 13589
			public byte[] state;
		}
	}
}
