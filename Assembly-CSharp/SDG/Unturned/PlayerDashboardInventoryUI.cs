using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007C3 RID: 1987
	public class PlayerDashboardInventoryUI
	{
		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x06004303 RID: 17155 RVA: 0x00175531 File Offset: 0x00173731
		// (set) Token: 0x06004304 RID: 17156 RVA: 0x00175538 File Offset: 0x00173738
		public static bool isDragging { get; private set; }

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x06004305 RID: 17157 RVA: 0x00175540 File Offset: 0x00173740
		public static byte selectedPage
		{
			get
			{
				return PlayerDashboardInventoryUI._selectedPage;
			}
		}

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x06004306 RID: 17158 RVA: 0x00175547 File Offset: 0x00173747
		public static byte selected_x
		{
			get
			{
				return PlayerDashboardInventoryUI._selected_x;
			}
		}

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x06004307 RID: 17159 RVA: 0x0017554E File Offset: 0x0017374E
		public static byte selected_y
		{
			get
			{
				return PlayerDashboardInventoryUI._selected_y;
			}
		}

		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x06004308 RID: 17160 RVA: 0x00175555 File Offset: 0x00173755
		public static ItemJar selectedJar
		{
			get
			{
				return PlayerDashboardInventoryUI._selectedJar;
			}
		}

		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x06004309 RID: 17161 RVA: 0x0017555C File Offset: 0x0017375C
		public static ItemAsset selectedAsset
		{
			get
			{
				return PlayerDashboardInventoryUI._selectedAsset;
			}
		}

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x0600430A RID: 17162 RVA: 0x00175563 File Offset: 0x00173763
		private static bool isSplitClothingArea
		{
			get
			{
				return Screen.width >= 1350;
			}
		}

		// Token: 0x0600430B RID: 17163 RVA: 0x00175574 File Offset: 0x00173774
		public static void open()
		{
			if (PlayerDashboardInventoryUI.active)
			{
				return;
			}
			PlayerDashboardInventoryUI.active = true;
			Player.player.animator.sendGesture(EPlayerGesture.INVENTORY_START, false);
			Player.player.character.Find("Camera").gameObject.SetActive(true);
			if (PlayerDashboardInventoryUI.isSplitClothingArea)
			{
				PlayerDashboardInventoryUI.clothingBox.SizeOffset_X = -5f;
				PlayerDashboardInventoryUI.clothingBox.SizeScale_X = 0.5f;
				PlayerDashboardInventoryUI.areaBox.IsVisible = true;
			}
			else
			{
				PlayerDashboardInventoryUI.clothingBox.SizeOffset_X = 0f;
				PlayerDashboardInventoryUI.clothingBox.SizeScale_X = 1f;
				PlayerDashboardInventoryUI.areaBox.IsVisible = false;
			}
			PlayerDashboardInventoryUI.updateVehicle();
			PlayerDashboardInventoryUI.resetNearbyDrops();
			PlayerDashboardInventoryUI.updateHotkeys();
			if (PlayerDashboardInventoryUI.characterPlayer != null)
			{
				PlayerDashboardInventoryUI.backdropBox.RemoveChild(PlayerDashboardInventoryUI.characterPlayer);
				PlayerDashboardInventoryUI.characterPlayer = null;
			}
			if (Player.player != null)
			{
				PlayerDashboardInventoryUI.characterPlayer = new SleekPlayer(Player.player.channel.owner, true, SleekPlayer.ESleekPlayerDisplayContext.NONE);
				PlayerDashboardInventoryUI.characterPlayer.PositionOffset_X = 10f;
				PlayerDashboardInventoryUI.characterPlayer.PositionOffset_Y = 10f;
				PlayerDashboardInventoryUI.characterPlayer.SizeOffset_X = 410f;
				PlayerDashboardInventoryUI.characterPlayer.SizeOffset_Y = 50f;
				PlayerDashboardInventoryUI.backdropBox.AddChild(PlayerDashboardInventoryUI.characterPlayer);
			}
			PlayerDashboardInventoryUI.container.AnimateIntoView();
		}

		// Token: 0x0600430C RID: 17164 RVA: 0x001756C4 File Offset: 0x001738C4
		public static void close()
		{
			if (!PlayerDashboardInventoryUI.active)
			{
				return;
			}
			PlayerDashboardInventoryUI.active = false;
			Player.player.animator.sendGesture(EPlayerGesture.INVENTORY_STOP, false);
			Player.player.character.Find("Camera").gameObject.SetActive(false);
			PlayerDashboardInventoryUI.stopDrag();
			PlayerDashboardInventoryUI.closeSelection();
			PlayerDashboardInventoryUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x0600430D RID: 17165 RVA: 0x00175730 File Offset: 0x00173930
		private static void startDrag()
		{
			if (PlayerDashboardInventoryUI.isDragging)
			{
				return;
			}
			PlayerDashboardInventoryUI.isDragging = true;
			PlayerDashboardInventoryUI.setItemsEnabled(false);
			PlayerDashboardInventoryUI.dragItem.IsVisible = true;
			if (PlayerDashboardInventoryUI.hasDragOutsideHandlers)
			{
				PlayerDashboardInventoryUI.dragOutsideHandler.IsVisible = true;
				PlayerDashboardInventoryUI.dragOutsideClothingHandler.IsVisible = true;
				PlayerDashboardInventoryUI.dragOutsideAreaHandler.IsVisible = true;
			}
			PlayerDashboardInventoryUI.PlayInventoryAudio(PlayerDashboardInventoryUI.dragJar.GetAsset());
		}

		// Token: 0x0600430E RID: 17166 RVA: 0x00175794 File Offset: 0x00173994
		public static void stopDrag()
		{
			if (!PlayerDashboardInventoryUI.isDragging)
			{
				return;
			}
			PlayerDashboardInventoryUI.isDragging = false;
			PlayerDashboardInventoryUI.PlayInventoryAudio(PlayerDashboardInventoryUI.dragJar.GetAsset());
			PlayerDashboardInventoryUI.dragJar.rot = PlayerDashboardInventoryUI.dragFromRot;
			PlayerDashboardInventoryUI.setItemsEnabled(true);
			PlayerDashboardInventoryUI.dragItem.IsVisible = false;
			if (PlayerDashboardInventoryUI.hasDragOutsideHandlers)
			{
				PlayerDashboardInventoryUI.dragOutsideHandler.IsVisible = false;
				PlayerDashboardInventoryUI.dragOutsideClothingHandler.IsVisible = false;
				PlayerDashboardInventoryUI.dragOutsideAreaHandler.IsVisible = false;
			}
		}

		// Token: 0x0600430F RID: 17167 RVA: 0x00175808 File Offset: 0x00173A08
		private static void setItemsEnabled(bool enabled)
		{
			SleekSlot[] array = PlayerDashboardInventoryUI.slots;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].isItemEnabled = enabled;
			}
			SleekItems[] array2 = PlayerDashboardInventoryUI.items;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].areItemsEnabled = enabled;
			}
		}

		/// <summary>
		/// Annoying frustrating workaround for IMGUI. Disable inventory headers, grids and slots while selection is open
		/// to prevent them from interfering with selection menu.
		/// </summary>
		// Token: 0x06004310 RID: 17168 RVA: 0x00175850 File Offset: 0x00173A50
		private static void setMiscButtonsEnabled(bool enabled)
		{
			ISleekButton[] array = PlayerDashboardInventoryUI.headers;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].IsRaycastTarget = enabled;
			}
			SleekSlot[] array2 = PlayerDashboardInventoryUI.slots;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].isImageRaycastTarget = enabled;
			}
			SleekItems[] array3 = PlayerDashboardInventoryUI.items;
			for (int i = 0; i < array3.Length; i++)
			{
				array3[i].isGridRaycastTarget = enabled;
			}
		}

		// Token: 0x06004311 RID: 17169 RVA: 0x001758B4 File Offset: 0x00173AB4
		private static void onDraggedCharacterSlider(ISleekSlider slider, float state)
		{
			PlayerLook.characterYaw = state * 360f;
		}

		// Token: 0x06004312 RID: 17170 RVA: 0x001758C2 File Offset: 0x00173AC2
		private static void onClickedSwapCosmeticsButton(ISleekElement button)
		{
			Player.player.clothing.sendVisualToggle(EVisualToggleType.COSMETIC);
		}

		// Token: 0x06004313 RID: 17171 RVA: 0x001758D4 File Offset: 0x00173AD4
		private static void onClickedSwapSkinsButton(ISleekElement button)
		{
			Player.player.clothing.sendVisualToggle(EVisualToggleType.SKIN);
		}

		// Token: 0x06004314 RID: 17172 RVA: 0x001758E6 File Offset: 0x00173AE6
		private static void onClickedSwapMythicsButton(ISleekElement button)
		{
			Player.player.clothing.sendVisualToggle(EVisualToggleType.MYTHIC);
		}

		// Token: 0x06004315 RID: 17173 RVA: 0x001758F8 File Offset: 0x00173AF8
		private static void onClickedVehicleLockButton(ISleekElement button)
		{
			VehicleManager.sendVehicleLock();
		}

		// Token: 0x06004316 RID: 17174 RVA: 0x001758FF File Offset: 0x00173AFF
		private static void onClickedVehicleHornButton(ISleekElement button)
		{
			VehicleManager.sendVehicleHorn();
		}

		// Token: 0x06004317 RID: 17175 RVA: 0x00175906 File Offset: 0x00173B06
		private static void onClickedVehicleHeadlightsButton(ISleekElement button)
		{
			VehicleManager.sendVehicleHeadlights();
		}

		// Token: 0x06004318 RID: 17176 RVA: 0x0017590D File Offset: 0x00173B0D
		private static void onClickedVehicleSirensButton(ISleekElement button)
		{
			VehicleManager.sendVehicleBonus();
		}

		// Token: 0x06004319 RID: 17177 RVA: 0x00175914 File Offset: 0x00173B14
		private static void onClickedVehicleBlimpButton(ISleekElement button)
		{
			VehicleManager.sendVehicleBonus();
		}

		// Token: 0x0600431A RID: 17178 RVA: 0x0017591B File Offset: 0x00173B1B
		private static void onClickedVehicleHookButton(ISleekElement button)
		{
			VehicleManager.sendVehicleBonus();
		}

		// Token: 0x0600431B RID: 17179 RVA: 0x00175922 File Offset: 0x00173B22
		private static void onClickedVehicleStealBatteryButton(ISleekElement button)
		{
			VehicleManager.sendVehicleStealBattery();
		}

		// Token: 0x0600431C RID: 17180 RVA: 0x00175929 File Offset: 0x00173B29
		private static void onClickedVehicleSkinButton(ISleekElement button)
		{
			VehicleManager.sendVehicleSkin();
		}

		// Token: 0x0600431D RID: 17181 RVA: 0x00175930 File Offset: 0x00173B30
		private static void onClickedVehiclePassengerButton(ISleekElement button)
		{
			int num = PlayerDashboardInventoryUI.vehiclePassengersBox.FindIndexOfChild(button);
			if (num < 0)
			{
				return;
			}
			VehicleManager.swapVehicle((byte)num);
		}

		/// <summary>
		/// Was ConsumeEvent called during this frame?
		/// This is a hack to prevent firing when clicking in the UI on the same frame it closes.
		/// Moved from SleekWindow and Event.current.Use() during UI refactor.
		/// </summary>
		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x0600431E RID: 17182 RVA: 0x00175955 File Offset: 0x00173B55
		public static bool WasEventConsumed
		{
			get
			{
				return PlayerDashboardInventoryUI.eventGuard.HasBeenConsumed;
			}
		}

		// Token: 0x0600431F RID: 17183 RVA: 0x00175961 File Offset: 0x00173B61
		private static void ConsumeEvent()
		{
			PlayerDashboardInventoryUI.eventGuard.Consume();
		}

		// Token: 0x06004320 RID: 17184 RVA: 0x00175970 File Offset: 0x00173B70
		private static void onClickedEquip(ISleekElement button)
		{
			if (PlayerDashboardInventoryUI.selectedPage != 255)
			{
				PlayerDashboardInventoryUI.checkEquip(PlayerDashboardInventoryUI.selectedPage, PlayerDashboardInventoryUI.selected_x, PlayerDashboardInventoryUI.selected_y, Player.player.inventory.getItem(PlayerDashboardInventoryUI.selectedPage, Player.player.inventory.getIndex(PlayerDashboardInventoryUI.selectedPage, PlayerDashboardInventoryUI.selected_x, PlayerDashboardInventoryUI.selected_y)), byte.MaxValue);
				PlayerDashboardInventoryUI.ConsumeEvent();
			}
		}

		// Token: 0x06004321 RID: 17185 RVA: 0x001759DC File Offset: 0x00173BDC
		private static void onClickedContext(ISleekElement button)
		{
			if (PlayerDashboardInventoryUI.selectedPage != 255)
			{
				if (PlayerDashboardInventoryUI.selectedAsset.type == EItemType.GUN)
				{
					Player.player.crafting.sendStripAttachments(PlayerDashboardInventoryUI.selectedPage, PlayerDashboardInventoryUI.selected_x, PlayerDashboardInventoryUI.selected_y);
				}
				PlayerDashboardInventoryUI.ConsumeEvent();
				PlayerDashboardInventoryUI.closeSelection();
			}
		}

		// Token: 0x06004322 RID: 17186 RVA: 0x00175A2C File Offset: 0x00173C2C
		private static void onClickedDrop(ISleekElement button)
		{
			if (PlayerDashboardInventoryUI.selectedPage != 255)
			{
				if (PlayerDashboardInventoryUI.selectedPage == PlayerInventory.AREA)
				{
					if (PlayerDashboardInventoryUI.selectedJar.interactableItem != null)
					{
						ItemManager.takeItem(PlayerDashboardInventoryUI.selectedJar.interactableItem.transform.parent, byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					}
					PlayerDashboardInventoryUI.closeSelection();
				}
				else
				{
					Player.player.inventory.sendDropItem(PlayerDashboardInventoryUI.selectedPage, PlayerDashboardInventoryUI.selected_x, PlayerDashboardInventoryUI.selected_y);
				}
				PlayerDashboardInventoryUI.ConsumeEvent();
			}
		}

		// Token: 0x06004323 RID: 17187 RVA: 0x00175AB8 File Offset: 0x00173CB8
		private static void onClickedStore(ISleekElement button)
		{
			if (PlayerDashboardInventoryUI.selectedPage != 255)
			{
				byte x_2;
				byte y_2;
				byte rot_2;
				if (PlayerDashboardInventoryUI.selectedPage == PlayerInventory.AREA)
				{
					if (PlayerDashboardInventoryUI.selectedJar.interactableItem != null)
					{
						ItemManager.takeItem(PlayerDashboardInventoryUI.selectedJar.interactableItem.transform.parent, byte.MaxValue, byte.MaxValue, 0, PlayerInventory.STORAGE);
					}
					PlayerDashboardInventoryUI.closeSelection();
				}
				else if (PlayerDashboardInventoryUI.selectedPage == PlayerInventory.STORAGE)
				{
					byte page_;
					byte x_;
					byte y_;
					byte rot_;
					if (Player.player.inventory.tryFindSpace(PlayerDashboardInventoryUI.selectedJar.size_x, PlayerDashboardInventoryUI.selectedJar.size_y, out page_, out x_, out y_, out rot_))
					{
						Player.player.inventory.sendDragItem(PlayerDashboardInventoryUI.selectedPage, PlayerDashboardInventoryUI.selected_x, PlayerDashboardInventoryUI.selected_y, page_, x_, y_, rot_);
					}
				}
				else if (Player.player.inventory.tryFindSpace(PlayerInventory.STORAGE, PlayerDashboardInventoryUI.selectedJar.size_x, PlayerDashboardInventoryUI.selectedJar.size_y, out x_2, out y_2, out rot_2))
				{
					Player.player.inventory.sendDragItem(PlayerDashboardInventoryUI.selectedPage, PlayerDashboardInventoryUI.selected_x, PlayerDashboardInventoryUI.selected_y, PlayerInventory.STORAGE, x_2, y_2, rot_2);
				}
				PlayerDashboardInventoryUI.ConsumeEvent();
			}
		}

		// Token: 0x06004324 RID: 17188 RVA: 0x00175BE0 File Offset: 0x00173DE0
		private static void onClickedAction(ISleekElement button)
		{
			int num = PlayerDashboardInventoryUI.selectionExtraActionsBox.FindIndexOfChild(button);
			Action action = PlayerDashboardInventoryUI.actions[num];
			ItemAsset itemAsset = Assets.find(EAssetType.ITEM, action.source) as ItemAsset;
			if (itemAsset == null)
			{
				return;
			}
			Blueprint[] array = new Blueprint[action.blueprints.Length];
			bool flag = false;
			byte b = 0;
			while ((int)b < array.Length)
			{
				array[(int)b] = itemAsset.blueprints[(int)action.blueprints[(int)b].id];
				if (action.blueprints[(int)b].isLink)
				{
					flag = true;
				}
				b += 1;
			}
			PlayerDashboardCraftingUI.filteredBlueprintsOverride = array;
			if (!flag)
			{
				PlayerDashboardCraftingUI.updateSelection();
				foreach (Blueprint blueprint in array)
				{
					if (!blueprint.hasSupplies)
					{
						flag = true;
						break;
					}
					if (!blueprint.hasTool)
					{
						flag = true;
						break;
					}
					if (!blueprint.hasItem)
					{
						flag = true;
						break;
					}
					if (!blueprint.hasSkills)
					{
						flag = true;
						break;
					}
					if (Player.player.equipment.isBusy)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				PlayerDashboardInventoryUI.close();
				PlayerDashboardCraftingUI.open();
				return;
			}
			foreach (Blueprint blueprint2 in array)
			{
				Player.player.crafting.sendCraft(blueprint2.sourceItem.id, blueprint2.id, InputEx.GetKey(ControlsSettings.other));
			}
			PlayerDashboardCraftingUI.filteredBlueprintsOverride = null;
			PlayerDashboardInventoryUI.closeSelection();
		}

		// Token: 0x06004325 RID: 17189 RVA: 0x00175D44 File Offset: 0x00173F44
		private static void onClickedRot_XButton(ISleekElement button)
		{
			InteractableStorage interactableStorage = PlayerInteract.interactable as InteractableStorage;
			if (interactableStorage == null || !interactableStorage.isDisplay)
			{
				return;
			}
			byte b = interactableStorage.rot_x;
			b += 1;
			if (b > 3)
			{
				b = 0;
			}
			byte rotation = interactableStorage.getRotation(b, interactableStorage.rot_y, interactableStorage.rot_z);
			interactableStorage.ClientSetDisplayRotation(rotation);
		}

		// Token: 0x06004326 RID: 17190 RVA: 0x00175D9C File Offset: 0x00173F9C
		private static void onClickedRot_YButton(ISleekElement button)
		{
			InteractableStorage interactableStorage = PlayerInteract.interactable as InteractableStorage;
			if (interactableStorage == null || !interactableStorage.isDisplay)
			{
				return;
			}
			byte b = interactableStorage.rot_y;
			b += 1;
			if (b > 3)
			{
				b = 0;
			}
			byte rotation = interactableStorage.getRotation(interactableStorage.rot_x, b, interactableStorage.rot_z);
			interactableStorage.ClientSetDisplayRotation(rotation);
		}

		// Token: 0x06004327 RID: 17191 RVA: 0x00175DF4 File Offset: 0x00173FF4
		private static void onClickedRot_ZButton(ISleekElement button)
		{
			InteractableStorage interactableStorage = PlayerInteract.interactable as InteractableStorage;
			if (interactableStorage == null || !interactableStorage.isDisplay)
			{
				return;
			}
			InteractableStorage interactableStorage2 = interactableStorage;
			byte rot_z = interactableStorage2.rot_z;
			interactableStorage2.rot_z = rot_z + 1;
			byte b = rot_z;
			b += 1;
			if (b > 3)
			{
				b = 0;
			}
			byte rotation = interactableStorage.getRotation(interactableStorage.rot_x, interactableStorage.rot_y, b);
			interactableStorage.ClientSetDisplayRotation(rotation);
		}

		// Token: 0x06004328 RID: 17192 RVA: 0x00175E58 File Offset: 0x00174058
		private static void openSelection(byte page, byte x, byte y)
		{
			PlayerDashboardInventoryUI._selectedPage = page;
			PlayerDashboardInventoryUI._selected_x = x;
			PlayerDashboardInventoryUI._selected_y = y;
			if (!Glazier.Get().SupportsDepth)
			{
				PlayerDashboardInventoryUI.setItemsEnabled(false);
				PlayerDashboardInventoryUI.setMiscButtonsEnabled(false);
			}
			PlayerDashboardInventoryUI.selectionFrame.IsVisible = true;
			PlayerDashboardInventoryUI._selectedJar = Player.player.inventory.getItem(page, Player.player.inventory.getIndex(page, x, y));
			if (PlayerDashboardInventoryUI.selectedJar == null)
			{
				return;
			}
			PlayerDashboardInventoryUI._selectedAsset = PlayerDashboardInventoryUI.selectedJar.GetAsset();
			PlayerDashboardInventoryUI.selectionIconImage.Clear();
			if (PlayerDashboardInventoryUI.selectedAsset != null)
			{
				int num;
				int num2;
				if (PlayerDashboardInventoryUI.selectedAsset.size_x <= PlayerDashboardInventoryUI.selectedAsset.size_y)
				{
					PlayerDashboardInventoryUI.selectionBackdropBox.SizeOffset_X = 490f;
					PlayerDashboardInventoryUI.selectionBackdropBox.SizeOffset_Y = 330f;
					PlayerDashboardInventoryUI.selectionIconBox.SizeOffset_X = 210f;
					PlayerDashboardInventoryUI.selectionIconBox.SizeOffset_Y = 310f;
					if (PlayerDashboardInventoryUI.selectionDescriptionScrollView != null)
					{
						PlayerDashboardInventoryUI.selectionDescriptionScrollView.PositionOffset_X = 230f;
						PlayerDashboardInventoryUI.selectionDescriptionScrollView.PositionOffset_Y = 10f;
						PlayerDashboardInventoryUI.selectionDescriptionScrollView.SizeOffset_X = 250f;
						PlayerDashboardInventoryUI.selectionDescriptionScrollView.SizeOffset_Y = 150f;
					}
					else
					{
						PlayerDashboardInventoryUI.selectionDescriptionBox.PositionOffset_X = 230f;
						PlayerDashboardInventoryUI.selectionDescriptionBox.PositionOffset_Y = 10f;
						PlayerDashboardInventoryUI.selectionDescriptionBox.SizeOffset_X = 250f;
						PlayerDashboardInventoryUI.selectionDescriptionBox.SizeOffset_Y = 150f;
					}
					PlayerDashboardInventoryUI.selectionActionsBox.PositionOffset_X = 230f;
					PlayerDashboardInventoryUI.selectionActionsBox.PositionOffset_Y = 170f;
					PlayerDashboardInventoryUI.selectionActionsBox.SizeOffset_Y = 150f;
					if (PlayerDashboardInventoryUI.selectedAsset.size_x == PlayerDashboardInventoryUI.selectedAsset.size_y)
					{
						num = 200;
						num2 = 200;
					}
					else
					{
						num = 200;
						num2 = 300;
					}
				}
				else
				{
					PlayerDashboardInventoryUI.selectionBackdropBox.SizeOffset_X = 530f;
					PlayerDashboardInventoryUI.selectionBackdropBox.SizeOffset_Y = 390f;
					PlayerDashboardInventoryUI.selectionIconBox.SizeOffset_X = 510f;
					PlayerDashboardInventoryUI.selectionIconBox.SizeOffset_Y = 210f;
					if (PlayerDashboardInventoryUI.selectionDescriptionScrollView != null)
					{
						PlayerDashboardInventoryUI.selectionDescriptionScrollView.PositionOffset_X = 10f;
						PlayerDashboardInventoryUI.selectionDescriptionScrollView.PositionOffset_Y = 230f;
						PlayerDashboardInventoryUI.selectionDescriptionScrollView.SizeOffset_X = 250f;
						PlayerDashboardInventoryUI.selectionDescriptionScrollView.SizeOffset_Y = 150f;
					}
					else
					{
						PlayerDashboardInventoryUI.selectionDescriptionBox.PositionOffset_X = 10f;
						PlayerDashboardInventoryUI.selectionDescriptionBox.PositionOffset_Y = 230f;
						PlayerDashboardInventoryUI.selectionDescriptionBox.SizeOffset_X = 250f;
						PlayerDashboardInventoryUI.selectionDescriptionBox.SizeOffset_Y = 150f;
					}
					PlayerDashboardInventoryUI.selectionActionsBox.PositionOffset_X = 270f;
					PlayerDashboardInventoryUI.selectionActionsBox.PositionOffset_Y = 230f;
					PlayerDashboardInventoryUI.selectionActionsBox.SizeOffset_Y = 150f;
					num = 500;
					num2 = 200;
				}
				PlayerDashboardInventoryUI.selectionIconImage.PositionOffset_X = (float)(-(float)num / 2);
				PlayerDashboardInventoryUI.selectionIconImage.PositionOffset_Y = (float)(-(float)num2 / 2);
				PlayerDashboardInventoryUI.selectionIconImage.SizeOffset_X = (float)num;
				PlayerDashboardInventoryUI.selectionIconImage.SizeOffset_Y = (float)num2;
				PlayerDashboardInventoryUI.selectionIconImage.Refresh(PlayerDashboardInventoryUI.selectedJar.item.id, PlayerDashboardInventoryUI.selectedJar.item.quality, PlayerDashboardInventoryUI.selectedJar.item.state, PlayerDashboardInventoryUI.selectedAsset, num, num2);
				Vector2 vector = Input.mousePosition;
				vector.y = (float)Screen.height - vector.y;
				vector /= GraphicsSettings.userInterfaceScale;
				PlayerDashboardInventoryUI.selectionBackdropBox.PositionOffset_X = (float)((int)Mathf.Clamp(vector.x - PlayerDashboardInventoryUI.selectionBackdropBox.SizeOffset_X / 2f, 0f, (float)Screen.width / GraphicsSettings.userInterfaceScale - PlayerDashboardInventoryUI.selectionBackdropBox.SizeOffset_X));
				PlayerDashboardInventoryUI.selectionBackdropBox.PositionOffset_Y = (float)((int)Mathf.Clamp(vector.y - PlayerDashboardInventoryUI.selectionBackdropBox.SizeOffset_Y / 2f, 0f, (float)Screen.height / GraphicsSettings.userInterfaceScale - PlayerDashboardInventoryUI.selectionBackdropBox.SizeOffset_Y));
				StringBuilder stringBuilder = new StringBuilder(512);
				ItemDescriptionBuilder itemDescriptionBuilder = default(ItemDescriptionBuilder);
				itemDescriptionBuilder.stringBuilder = stringBuilder;
				itemDescriptionBuilder.shouldRestrictToLegacyContent = (!Glazier.Get().SupportsAutomaticLayout || !PlayerDashboardInventoryUI.selectedAsset.isEligibleForAutoStatDescriptions);
				itemDescriptionBuilder.lines = new List<ItemDescriptionLine>();
				PlayerDashboardInventoryUI.selectedAsset.BuildDescription(itemDescriptionBuilder, PlayerDashboardInventoryUI.selectedJar.item);
				itemDescriptionBuilder.lines.Sort();
				int num3 = 0;
				stringBuilder.Clear();
				foreach (ItemDescriptionLine itemDescriptionLine in itemDescriptionBuilder.lines)
				{
					if (itemDescriptionLine.sortOrder - num3 > 100)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.AppendLine(itemDescriptionLine.text);
					num3 = itemDescriptionLine.sortOrder;
				}
				PlayerDashboardInventoryUI.selectionDescriptionLabel.Text = stringBuilder.ToString();
				if (PlayerDashboardInventoryUI.selectionDescriptionScrollView != null)
				{
					PlayerDashboardInventoryUI.selectionDescriptionScrollView.ScrollToTop();
				}
				PlayerDashboardInventoryUI.selectionNameLabel.Text = PlayerDashboardInventoryUI.selectedAsset.itemName;
				if (PlayerDashboardInventoryUI.selectedPage < PlayerInventory.SLOTS)
				{
					PlayerDashboardInventoryUI.selectionHotkeyLabel.Text = PlayerDashboardInventoryUI.localization.format("Hotkey_Set", ControlsSettings.getEquipmentHotkeyText((int)PlayerDashboardInventoryUI.selectedPage));
					PlayerDashboardInventoryUI.selectionHotkeyLabel.IsVisible = true;
				}
				else if (PlayerDashboardInventoryUI.selectedPage < PlayerInventory.STORAGE && ItemTool.checkUseable(PlayerDashboardInventoryUI.selectedPage, PlayerDashboardInventoryUI.selectedJar.item.id))
				{
					PlayerDashboardInventoryUI.selectionHotkeyLabel.Text = PlayerDashboardInventoryUI.localization.format("Hotkey_Unset");
					PlayerDashboardInventoryUI.selectionHotkeyLabel.IsVisible = true;
					byte b = 0;
					while ((int)b < Player.player.equipment.hotkeys.Length)
					{
						HotkeyInfo hotkeyInfo = Player.player.equipment.hotkeys[(int)b];
						if (hotkeyInfo.page == PlayerDashboardInventoryUI.selectedPage && hotkeyInfo.x == PlayerDashboardInventoryUI.selected_x && hotkeyInfo.y == PlayerDashboardInventoryUI.selected_y)
						{
							PlayerDashboardInventoryUI.selectionHotkeyLabel.Text = PlayerDashboardInventoryUI.localization.format("Hotkey_Set", ControlsSettings.getEquipmentHotkeyText((int)(b + 2)));
							break;
						}
						b += 1;
					}
				}
				else
				{
					PlayerDashboardInventoryUI.selectionHotkeyLabel.IsVisible = false;
				}
				if (Player.player.equipment.checkSelection(page, x, y))
				{
					PlayerDashboardInventoryUI.selectionEquipButton.Text = PlayerDashboardInventoryUI.localization.format("Dequip_Button");
					PlayerDashboardInventoryUI.selectionEquipButton.TooltipText = PlayerDashboardInventoryUI.localization.format("Dequip_Button_Tooltip");
				}
				else
				{
					PlayerDashboardInventoryUI.selectionEquipButton.Text = PlayerDashboardInventoryUI.localization.format("Equip_Button");
					PlayerDashboardInventoryUI.selectionEquipButton.TooltipText = PlayerDashboardInventoryUI.localization.format("Equip_Button_Tooltip");
				}
				if (PlayerDashboardInventoryUI.selectedAsset.type == EItemType.GUN)
				{
					PlayerDashboardInventoryUI.selectionContextButton.Text = PlayerDashboardInventoryUI.localization.format("Attachments_Button");
					PlayerDashboardInventoryUI.selectionContextButton.TooltipText = PlayerDashboardInventoryUI.localization.format("Attachments_Button_Tooltip");
					PlayerDashboardInventoryUI.selectionContextButton.IsVisible = (PlayerDashboardInventoryUI.selectedPage >= PlayerInventory.SLOTS && PlayerDashboardInventoryUI.selectedPage < PlayerInventory.AREA);
				}
				else
				{
					PlayerDashboardInventoryUI.selectionContextButton.IsVisible = false;
				}
				bool flag = page == PlayerInventory.AREA;
				if (flag)
				{
					PlayerDashboardInventoryUI.selectionDropButton.Text = PlayerDashboardInventoryUI.localization.format("Pickup_Button");
					PlayerDashboardInventoryUI.selectionDropButton.TooltipText = PlayerDashboardInventoryUI.localization.format("Pickup_Button_Tooltip");
				}
				else
				{
					PlayerDashboardInventoryUI.selectionDropButton.Text = PlayerDashboardInventoryUI.localization.format("Drop_Button");
					PlayerDashboardInventoryUI.selectionDropButton.TooltipText = PlayerDashboardInventoryUI.localization.format("Drop_Button_Tooltip");
				}
				if (page == PlayerInventory.STORAGE)
				{
					PlayerDashboardInventoryUI.selectionStorageButton.Text = PlayerDashboardInventoryUI.localization.format("Take_Button");
					PlayerDashboardInventoryUI.selectionStorageButton.TooltipText = PlayerDashboardInventoryUI.localization.format("Take_Button_Tooltip");
				}
				else
				{
					PlayerDashboardInventoryUI.selectionStorageButton.Text = PlayerDashboardInventoryUI.localization.format("Store_Button");
					PlayerDashboardInventoryUI.selectionStorageButton.TooltipText = PlayerDashboardInventoryUI.localization.format("Store_Button_Tooltip");
				}
				PlayerDashboardInventoryUI.selectionEquipButton.IsVisible = (PlayerDashboardInventoryUI.selectedAsset.canPlayerEquip && page < PlayerInventory.PAGES - 2);
				PlayerDashboardInventoryUI.selectionDropButton.IsVisible = (flag || PlayerDashboardInventoryUI.selectedAsset.allowManualDrop);
				PlayerDashboardInventoryUI.selectionStorageButton.IsVisible = Player.player.inventory.isStoring;
				int num4 = 0;
				if (PlayerDashboardInventoryUI.selectionEquipButton.IsVisible)
				{
					PlayerDashboardInventoryUI.selectionEquipButton.PositionOffset_Y = (float)num4;
					num4 += 40;
				}
				if (PlayerDashboardInventoryUI.selectionContextButton.IsVisible)
				{
					PlayerDashboardInventoryUI.selectionContextButton.PositionOffset_Y = (float)num4;
					num4 += 40;
				}
				if (PlayerDashboardInventoryUI.selectionDropButton.IsVisible)
				{
					PlayerDashboardInventoryUI.selectionDropButton.PositionOffset_Y = (float)num4;
					num4 += 40;
				}
				if (PlayerDashboardInventoryUI.selectionStorageButton.IsVisible)
				{
					PlayerDashboardInventoryUI.selectionStorageButton.PositionOffset_Y = (float)num4;
					num4 += 40;
				}
				PlayerDashboardInventoryUI.selectionExtraActionsBox.RemoveAllChildren();
				PlayerDashboardInventoryUI.selectionExtraActionsBox.PositionOffset_Y = (float)num4;
				int num5 = 0;
				if (page != PlayerInventory.AREA)
				{
					PlayerDashboardInventoryUI.actions.Clear();
					int i = 0;
					while (i < PlayerDashboardInventoryUI.selectedAsset.actions.Count)
					{
						Action action = PlayerDashboardInventoryUI.selectedAsset.actions[i];
						if (action.type != EActionType.BLUEPRINT)
						{
							goto IL_9B4;
						}
						if (page >= PlayerInventory.SLOTS && page < PlayerInventory.STORAGE)
						{
							Blueprint blueprint = (Assets.find(EAssetType.ITEM, action.source) as ItemAsset).blueprints[(int)action.blueprints[0].id];
							if ((blueprint.skill != EBlueprintSkill.REPAIR || (uint)blueprint.level <= Provider.modeConfigData.Gameplay.Repair_Level_Max) && (blueprint.type != EBlueprintType.REPAIR || PlayerDashboardInventoryUI.selectedJar.item.quality != 100) && blueprint.areConditionsMet(Player.player) && !Player.player.crafting.isBlueprintBlacklisted(blueprint))
							{
								goto IL_9B4;
							}
						}
						IL_A8B:
						i++;
						continue;
						IL_9B4:
						PlayerDashboardInventoryUI.actions.Add(action);
						ISleekButton sleekButton = Glazier.Get().CreateButton();
						sleekButton.PositionOffset_Y = (float)num5;
						sleekButton.SizeScale_X = 1f;
						sleekButton.SizeOffset_Y = 30f;
						if (!string.IsNullOrEmpty(action.key))
						{
							sleekButton.Text = PlayerDashboardInventoryUI.localization.format(action.key + "_Button");
							sleekButton.TooltipText = PlayerDashboardInventoryUI.localization.format(action.key + "_Button_Tooltip");
						}
						else
						{
							sleekButton.Text = action.text;
							sleekButton.TooltipText = action.tooltip;
						}
						sleekButton.OnClicked += new ClickedButton(PlayerDashboardInventoryUI.onClickedAction);
						PlayerDashboardInventoryUI.selectionExtraActionsBox.AddChild(sleekButton);
						num5 += 40;
						num4 += 40;
						goto IL_A8B;
					}
				}
				PlayerDashboardInventoryUI.selectionExtraActionsBox.SizeOffset_Y = (float)(num5 - 10);
				PlayerDashboardInventoryUI.selectionActionsBox.ContentSizeOffset = new Vector2(0f, (float)(num4 - 10));
				PlayerDashboardInventoryUI.selectionNameLabel.TextColor = ItemTool.getRarityColorUI(PlayerDashboardInventoryUI.selectedAsset.rarity);
			}
		}

		// Token: 0x06004329 RID: 17193 RVA: 0x00176964 File Offset: 0x00174B64
		public static void closeSelection()
		{
			if (PlayerDashboardInventoryUI.selectedPage == 255)
			{
				return;
			}
			PlayerDashboardInventoryUI._selectedPage = byte.MaxValue;
			PlayerDashboardInventoryUI._selected_x = byte.MaxValue;
			PlayerDashboardInventoryUI._selected_y = byte.MaxValue;
			if (!Glazier.Get().SupportsDepth)
			{
				PlayerDashboardInventoryUI.setItemsEnabled(true);
				PlayerDashboardInventoryUI.setMiscButtonsEnabled(true);
			}
			PlayerDashboardInventoryUI.selectionFrame.IsVisible = false;
		}

		// Token: 0x0600432A RID: 17194 RVA: 0x001769C0 File Offset: 0x00174BC0
		private static void onSelectedItem(byte page, byte x, byte y)
		{
			if (page == 255 || (page == PlayerDashboardInventoryUI.selectedPage && x == PlayerDashboardInventoryUI.selected_x && y == PlayerDashboardInventoryUI.selected_y))
			{
				PlayerDashboardInventoryUI.closeSelection();
				return;
			}
			if (InputEx.GetKey(ControlsSettings.other))
			{
				ItemJar item = Player.player.inventory.getItem(page, Player.player.inventory.getIndex(page, x, y));
				if (item == null)
				{
					return;
				}
				if (!Player.player.inventory.isStoring)
				{
					PlayerDashboardInventoryUI.checkAction(page, x, y, item);
					return;
				}
				byte x_2;
				byte y_2;
				byte rot_2;
				if (page == PlayerInventory.AREA)
				{
					if (item.interactableItem != null)
					{
						ItemManager.takeItem(item.interactableItem.transform.parent, byte.MaxValue, byte.MaxValue, 0, PlayerInventory.STORAGE);
						return;
					}
				}
				else if (page == PlayerInventory.STORAGE)
				{
					byte page_;
					byte x_;
					byte y_;
					byte rot_;
					if (Player.player.inventory.tryFindSpace(item.size_x, item.size_y, out page_, out x_, out y_, out rot_))
					{
						Player.player.inventory.sendDragItem(page, x, y, page_, x_, y_, rot_);
						return;
					}
				}
				else if (Player.player.inventory.tryFindSpace(PlayerInventory.STORAGE, item.size_x, item.size_y, out x_2, out y_2, out rot_2))
				{
					Player.player.inventory.sendDragItem(page, x, y, PlayerInventory.STORAGE, x_2, y_2, rot_2);
					return;
				}
			}
			else
			{
				PlayerDashboardInventoryUI.openSelection(page, x, y);
			}
		}

		// Token: 0x0600432B RID: 17195 RVA: 0x00176B1C File Offset: 0x00174D1C
		private static bool checkSlot(byte page, byte x, byte y, ItemJar jar, byte slot)
		{
			if (Player.player.inventory.checkSpaceEmpty(slot, 255, 255, 0, 0, 0))
			{
				Player.player.inventory.sendDragItem(page, x, y, slot, 0, 0, 0);
				Player.player.equipment.ClientEquipAfterItemDrag(slot, 0, 0);
				PlayerDashboardUI.close();
				PlayerLifeUI.open();
				return true;
			}
			ItemJar item = Player.player.inventory.getItem(slot, 0);
			byte b = item.rot;
			if (!Player.player.inventory.checkSpaceSwap(page, x, y, jar.size_x, jar.size_y, jar.rot, item.size_x, item.size_y, b))
			{
				b = (b + 1) % 4;
				if (!Player.player.inventory.checkSpaceSwap(page, x, y, jar.size_x, jar.size_y, jar.rot, item.size_x, item.size_y, b))
				{
					return false;
				}
			}
			Player.player.inventory.sendSwapItem(page, x, y, b, slot, 0, 0, jar.rot);
			Player.player.equipment.ClientEquipAfterItemDrag(slot, 0, 0);
			PlayerDashboardUI.close();
			PlayerLifeUI.open();
			return true;
		}

		// Token: 0x0600432C RID: 17196 RVA: 0x00176C44 File Offset: 0x00174E44
		private static void checkEquip(byte page, byte x, byte y, ItemJar jar, byte slot)
		{
			if (page == PlayerInventory.AREA)
			{
				if (page == PlayerDashboardInventoryUI.selectedPage && x == PlayerDashboardInventoryUI.selected_x && y == PlayerDashboardInventoryUI.selected_y)
				{
					PlayerDashboardInventoryUI.closeSelection();
				}
				if (jar.interactableItem != null)
				{
					ItemManager.takeItem(jar.interactableItem.transform.parent, byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
				}
				return;
			}
			if (!Player.player.equipment.checkSelection(page, x, y))
			{
				ItemAsset asset = jar.GetAsset();
				if (asset != null)
				{
					if (asset.canPlayerEquip && asset.slot.canEquipInPage(page))
					{
						Player.player.equipment.equip(page, x, y);
						PlayerDashboardUI.close();
						PlayerLifeUI.open();
						return;
					}
					if (asset.slot == ESlotType.PRIMARY)
					{
						PlayerDashboardInventoryUI.checkSlot(page, x, y, jar, 0);
						return;
					}
					if (asset.slot == ESlotType.SECONDARY)
					{
						if (slot != 255)
						{
							PlayerDashboardInventoryUI.checkSlot(page, x, y, jar, slot);
							return;
						}
						if (!PlayerDashboardInventoryUI.checkSlot(page, x, y, jar, 1))
						{
							PlayerDashboardInventoryUI.checkSlot(page, x, y, jar, 0);
							return;
						}
					}
				}
			}
			else if (Player.player.equipment.HasValidUseable && !Player.player.equipment.isBusy && Player.player.equipment.IsEquipAnimationFinished)
			{
				Player.player.equipment.dequip();
				if (page == PlayerDashboardInventoryUI.selectedPage && x == PlayerDashboardInventoryUI.selected_x && y == PlayerDashboardInventoryUI.selected_y)
				{
					PlayerDashboardInventoryUI.closeSelection();
				}
			}
		}

		// Token: 0x0600432D RID: 17197 RVA: 0x00176DB0 File Offset: 0x00174FB0
		private static void checkAction(byte page, byte x, byte y, ItemJar jar)
		{
			if (page == PlayerInventory.AREA)
			{
				if (jar.interactableItem != null)
				{
					ItemManager.takeItem(jar.interactableItem.transform.parent, byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
				}
				return;
			}
			ItemAsset asset = jar.GetAsset();
			if (asset == null)
			{
				return;
			}
			if (asset.type == EItemType.HAT)
			{
				Player.player.clothing.sendSwapHat(page, x, y);
				return;
			}
			if (asset.type == EItemType.SHIRT)
			{
				Player.player.clothing.sendSwapShirt(page, x, y);
				return;
			}
			if (asset.type == EItemType.PANTS)
			{
				Player.player.clothing.sendSwapPants(page, x, y);
				return;
			}
			if (asset.type == EItemType.BACKPACK)
			{
				Player.player.clothing.sendSwapBackpack(page, x, y);
				return;
			}
			if (asset.type == EItemType.VEST)
			{
				Player.player.clothing.sendSwapVest(page, x, y);
				return;
			}
			if (asset.type == EItemType.MASK)
			{
				Player.player.clothing.sendSwapMask(page, x, y);
				return;
			}
			if (asset.type == EItemType.GLASSES)
			{
				Player.player.clothing.sendSwapGlasses(page, x, y);
				return;
			}
			if (asset.canPlayerEquip)
			{
				PlayerDashboardInventoryUI.checkEquip(page, x, y, jar, byte.MaxValue);
			}
		}

		// Token: 0x0600432E RID: 17198 RVA: 0x00176EE0 File Offset: 0x001750E0
		private static void onGrabbedItem(byte page, byte x, byte y, SleekItem item)
		{
			if (InputEx.GetKey(ControlsSettings.other))
			{
				if (page != PlayerInventory.AREA)
				{
					Player.player.inventory.sendDropItem(page, x, y);
					return;
				}
				if (item.jar.interactableItem == null)
				{
					UnturnedLog.warn("onGrabbedItem nearby without interactable");
					return;
				}
				ItemManager.takeItem(item.jar.interactableItem.transform.parent, byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
				return;
			}
			else
			{
				PlayerDashboardInventoryUI.dragJar = Player.player.inventory.getItem(page, Player.player.inventory.getIndex(page, x, y));
				if (PlayerDashboardInventoryUI.dragJar == null)
				{
					return;
				}
				PlayerDashboardInventoryUI.dragSource = item;
				PlayerDashboardInventoryUI.dragFromPage = page;
				PlayerDashboardInventoryUI.dragFrom_x = x;
				PlayerDashboardInventoryUI.dragFrom_y = y;
				PlayerDashboardInventoryUI.dragFromRot = PlayerDashboardInventoryUI.dragJar.rot;
				PlayerDashboardInventoryUI.dragOffset = -item.GetNormalizedCursorPosition();
				PlayerDashboardInventoryUI.dragOffset.x = PlayerDashboardInventoryUI.dragOffset.x * item.SizeOffset_X;
				PlayerDashboardInventoryUI.dragOffset.y = PlayerDashboardInventoryUI.dragOffset.y * item.SizeOffset_Y;
				if (PlayerDashboardInventoryUI.dragJar.rot == 1)
				{
					float x2 = PlayerDashboardInventoryUI.dragOffset.x;
					PlayerDashboardInventoryUI.dragOffset.x = PlayerDashboardInventoryUI.dragOffset.y;
					PlayerDashboardInventoryUI.dragOffset.y = -((float)(PlayerDashboardInventoryUI.dragJar.size_y * 50) + x2);
				}
				else if (PlayerDashboardInventoryUI.dragJar.rot == 2)
				{
					PlayerDashboardInventoryUI.dragOffset.x = -((float)(PlayerDashboardInventoryUI.dragJar.size_x * 50) + PlayerDashboardInventoryUI.dragOffset.x);
					PlayerDashboardInventoryUI.dragOffset.y = -((float)(PlayerDashboardInventoryUI.dragJar.size_y * 50) + PlayerDashboardInventoryUI.dragOffset.y);
				}
				else if (PlayerDashboardInventoryUI.dragJar.rot == 3)
				{
					float x3 = PlayerDashboardInventoryUI.dragOffset.x;
					PlayerDashboardInventoryUI.dragOffset.x = -((float)(PlayerDashboardInventoryUI.dragJar.size_x * 50) + PlayerDashboardInventoryUI.dragOffset.y);
					PlayerDashboardInventoryUI.dragOffset.y = x3;
				}
				PlayerDashboardInventoryUI.updatePivot();
				PlayerDashboardInventoryUI.dragItem.updateItem(PlayerDashboardInventoryUI.dragJar);
				PlayerDashboardInventoryUI.refreshDraggedVisualPosition();
				PlayerDashboardInventoryUI.startDrag();
				return;
			}
		}

		// Token: 0x0600432F RID: 17199 RVA: 0x001770F4 File Offset: 0x001752F4
		private static void onPlacedItem(byte page, byte x, byte y)
		{
			PlayerDashboardInventoryUI.ConsumeEvent();
			if (PlayerDashboardInventoryUI.dragSource != null && PlayerDashboardInventoryUI.isDragging)
			{
				if (page >= PlayerInventory.SLOTS)
				{
					int num = (int)x + (int)(PlayerDashboardInventoryUI.dragPivot.x / 50f);
					int num2 = (int)y + (int)(PlayerDashboardInventoryUI.dragPivot.y / 50f);
					if (num < 0)
					{
						num = 0;
					}
					if (num2 < 0)
					{
						num2 = 0;
					}
					byte b = PlayerDashboardInventoryUI.dragJar.size_x;
					byte b2 = PlayerDashboardInventoryUI.dragJar.size_y;
					if (PlayerDashboardInventoryUI.dragJar.rot % 2 == 1)
					{
						b = PlayerDashboardInventoryUI.dragJar.size_y;
						b2 = PlayerDashboardInventoryUI.dragJar.size_x;
					}
					if (num >= (int)(Player.player.inventory.getWidth(page) - b))
					{
						num = (int)(Player.player.inventory.getWidth(page) - b);
					}
					if (num2 >= (int)(Player.player.inventory.getHeight(page) - b2))
					{
						num2 = (int)(Player.player.inventory.getHeight(page) - b2);
					}
					x = (byte)num;
					y = (byte)num2;
				}
				ItemAsset asset = PlayerDashboardInventoryUI.dragJar.GetAsset();
				if (asset == null)
				{
					return;
				}
				if (page < PlayerInventory.SLOTS && !asset.slot.canEquipInPage(page))
				{
					return;
				}
				if (PlayerDashboardInventoryUI.dragFromPage == page && PlayerDashboardInventoryUI.dragFrom_x == x && PlayerDashboardInventoryUI.dragFrom_y == y && PlayerDashboardInventoryUI.dragFromRot == PlayerDashboardInventoryUI.dragJar.rot)
				{
					PlayerDashboardInventoryUI.stopDrag();
					return;
				}
				if (page == PlayerInventory.AREA)
				{
					PlayerDashboardInventoryUI.stopDrag();
					if (page != PlayerDashboardInventoryUI.dragFromPage)
					{
						Player.player.inventory.sendDropItem(PlayerDashboardInventoryUI.dragFromPage, PlayerDashboardInventoryUI.dragFrom_x, PlayerDashboardInventoryUI.dragFrom_y);
					}
					return;
				}
				if (PlayerDashboardInventoryUI.dragFromPage == PlayerInventory.AREA)
				{
					byte rot = PlayerDashboardInventoryUI.dragJar.rot;
					PlayerDashboardInventoryUI.stopDrag();
					if (page != PlayerDashboardInventoryUI.dragFromPage && Player.player.inventory.checkSpaceEmpty(page, x, y, PlayerDashboardInventoryUI.dragJar.size_x, PlayerDashboardInventoryUI.dragJar.size_y, rot) && PlayerDashboardInventoryUI.dragItem.jar != null && PlayerDashboardInventoryUI.dragItem.jar.interactableItem != null)
					{
						ItemManager.takeItem(PlayerDashboardInventoryUI.dragItem.jar.interactableItem.transform.parent, x, y, rot, page);
					}
					return;
				}
				if (Player.player.inventory.checkSpaceDrag(page, PlayerDashboardInventoryUI.dragFrom_x, PlayerDashboardInventoryUI.dragFrom_y, PlayerDashboardInventoryUI.dragFromRot, x, y, PlayerDashboardInventoryUI.dragJar.rot, PlayerDashboardInventoryUI.dragJar.size_x, PlayerDashboardInventoryUI.dragJar.size_y, page == PlayerDashboardInventoryUI.dragFromPage))
				{
					byte rot2 = PlayerDashboardInventoryUI.dragJar.rot;
					PlayerDashboardInventoryUI.stopDrag();
					Player.player.inventory.sendDragItem(PlayerDashboardInventoryUI.dragFromPage, PlayerDashboardInventoryUI.dragFrom_x, PlayerDashboardInventoryUI.dragFrom_y, page, x, y, rot2);
					if (page < PlayerInventory.SLOTS)
					{
						Player.player.equipment.equip(page, 0, 0);
						PlayerDashboardUI.close();
						PlayerLifeUI.open();
						return;
					}
				}
				else
				{
					if (page < PlayerInventory.SLOTS)
					{
						PlayerDashboardInventoryUI.stopDrag();
						PlayerDashboardInventoryUI.checkEquip(PlayerDashboardInventoryUI.dragFromPage, PlayerDashboardInventoryUI.dragFrom_x, PlayerDashboardInventoryUI.dragFrom_y, PlayerDashboardInventoryUI.dragJar, page);
						return;
					}
					byte b4;
					byte b5;
					byte b3 = Player.player.inventory.findIndex(page, x, y, out b4, out b5);
					if (b3 == 255)
					{
						return;
					}
					if (PlayerDashboardInventoryUI.dragFromPage == page && PlayerDashboardInventoryUI.dragFrom_x == b4 && PlayerDashboardInventoryUI.dragFrom_y == b5)
					{
						PlayerDashboardInventoryUI.stopDrag();
						return;
					}
					ItemJar item = Player.player.inventory.getItem(page, b3);
					if (Player.player.inventory.checkSpaceSwap(page, b4, b5, item.size_x, item.size_y, item.rot, PlayerDashboardInventoryUI.dragJar.size_x, PlayerDashboardInventoryUI.dragJar.size_y, PlayerDashboardInventoryUI.dragJar.rot))
					{
						byte b6 = item.rot;
						if (!Player.player.inventory.checkSpaceSwap(PlayerDashboardInventoryUI.dragFromPage, PlayerDashboardInventoryUI.dragFrom_x, PlayerDashboardInventoryUI.dragFrom_y, PlayerDashboardInventoryUI.dragJar.size_x, PlayerDashboardInventoryUI.dragJar.size_y, PlayerDashboardInventoryUI.dragFromRot, item.size_x, item.size_y, b6))
						{
							b6 = (b6 + 1) % 4;
							if (!Player.player.inventory.checkSpaceSwap(PlayerDashboardInventoryUI.dragFromPage, PlayerDashboardInventoryUI.dragFrom_x, PlayerDashboardInventoryUI.dragFrom_y, PlayerDashboardInventoryUI.dragJar.size_x, PlayerDashboardInventoryUI.dragJar.size_y, PlayerDashboardInventoryUI.dragFromRot, item.size_x, item.size_y, b6))
							{
								return;
							}
						}
						ItemAsset asset2 = item.GetAsset();
						if (asset2 != null && (PlayerDashboardInventoryUI.dragFromPage >= PlayerInventory.SLOTS || asset2.slot.canEquipInPage(PlayerDashboardInventoryUI.dragFromPage)))
						{
							byte rot3 = PlayerDashboardInventoryUI.dragJar.rot;
							PlayerDashboardInventoryUI.stopDrag();
							Player.player.inventory.sendSwapItem(page, b4, b5, rot3, PlayerDashboardInventoryUI.dragFromPage, PlayerDashboardInventoryUI.dragFrom_x, PlayerDashboardInventoryUI.dragFrom_y, b6);
							if (PlayerDashboardInventoryUI.dragFromPage < PlayerInventory.SLOTS)
							{
								PlayerDashboardInventoryUI.checkEquip(PlayerDashboardInventoryUI.dragFromPage, PlayerDashboardInventoryUI.dragFrom_x, PlayerDashboardInventoryUI.dragFrom_y, PlayerDashboardInventoryUI.dragJar, page);
							}
						}
					}
				}
			}
		}

		// Token: 0x06004330 RID: 17200 RVA: 0x001775B4 File Offset: 0x001757B4
		private static void onClickedCharacter()
		{
			if (PlayerDashboardInventoryUI.dragSource != null && PlayerDashboardInventoryUI.isDragging)
			{
				byte page = PlayerDashboardInventoryUI.dragFromPage;
				byte x = PlayerDashboardInventoryUI.dragFrom_x;
				byte y = PlayerDashboardInventoryUI.dragFrom_y;
				ItemJar jar = PlayerDashboardInventoryUI.dragJar;
				PlayerDashboardInventoryUI.stopDrag();
				PlayerDashboardInventoryUI.checkAction(page, x, y, jar);
			}
			else
			{
				Vector2 normalizedCursorPosition = PlayerDashboardInventoryUI.characterImage.GetNormalizedCursorPosition();
				Vector3 pos = new Vector3(normalizedCursorPosition.x, 1f - normalizedCursorPosition.y, 0f);
				RaycastHit raycastHit;
				Physics.Raycast(Player.player.look.characterCamera.ViewportPointToRay(pos), out raycastHit, 8f, RayMasks.CLOTHING_INTERACT);
				if (raycastHit.collider != null)
				{
					Transform transform = raycastHit.collider.transform;
					if (transform.CompareTag("Player"))
					{
						ELimb limb = DamageTool.getLimb(transform);
						if (limb == ELimb.LEFT_FOOT || limb == ELimb.LEFT_LEG || limb == ELimb.RIGHT_FOOT || limb == ELimb.RIGHT_LEG)
						{
							Player.player.clothing.sendSwapPants(byte.MaxValue, byte.MaxValue, byte.MaxValue);
						}
						else if (limb == ELimb.LEFT_HAND || limb == ELimb.LEFT_ARM || limb == ELimb.RIGHT_HAND || limb == ELimb.RIGHT_ARM || limb == ELimb.SPINE)
						{
							Player.player.clothing.sendSwapShirt(byte.MaxValue, byte.MaxValue, byte.MaxValue);
						}
					}
					else if (transform.CompareTag("Enemy"))
					{
						if (transform.name == "Hat")
						{
							Player.player.clothing.sendSwapHat(byte.MaxValue, byte.MaxValue, byte.MaxValue);
						}
						else if (transform.name == "Glasses")
						{
							Player.player.clothing.sendSwapGlasses(byte.MaxValue, byte.MaxValue, byte.MaxValue);
						}
						else if (transform.name == "Mask")
						{
							Player.player.clothing.sendSwapMask(byte.MaxValue, byte.MaxValue, byte.MaxValue);
						}
						else if (transform.name == "Vest")
						{
							Player.player.clothing.sendSwapVest(byte.MaxValue, byte.MaxValue, byte.MaxValue);
						}
						else if (transform.name == "Backpack")
						{
							Player.player.clothing.sendSwapBackpack(byte.MaxValue, byte.MaxValue, byte.MaxValue);
						}
					}
					else if (transform.CompareTag("Item"))
					{
						Player.player.equipment.dequip();
					}
				}
			}
			PlayerDashboardInventoryUI.ConsumeEvent();
		}

		// Token: 0x06004331 RID: 17201 RVA: 0x00177831 File Offset: 0x00175A31
		private static void onClickedOutsideSelection()
		{
			PlayerDashboardInventoryUI.closeSelection();
		}

		// Token: 0x06004332 RID: 17202 RVA: 0x00177838 File Offset: 0x00175A38
		private static void onClickedDuringDrag()
		{
			if (PlayerDashboardInventoryUI.dragSource != null && PlayerDashboardInventoryUI.isDragging)
			{
				byte b = PlayerDashboardInventoryUI.dragFromPage;
				byte x = PlayerDashboardInventoryUI.dragFrom_x;
				byte y = PlayerDashboardInventoryUI.dragFrom_y;
				PlayerDashboardInventoryUI.stopDrag();
				if (b != PlayerInventory.AREA)
				{
					Player.player.inventory.sendDropItem(b, x, y);
				}
				PlayerDashboardInventoryUI.ConsumeEvent();
			}
		}

		// Token: 0x06004333 RID: 17203 RVA: 0x00177889 File Offset: 0x00175A89
		private static void onRightClickedDuringDrag()
		{
			if (PlayerDashboardInventoryUI.dragSource != null && PlayerDashboardInventoryUI.isDragging)
			{
				PlayerDashboardInventoryUI.stopDrag();
			}
		}

		// Token: 0x06004334 RID: 17204 RVA: 0x001778A0 File Offset: 0x00175AA0
		private static void onItemDropAdded(Transform model, InteractableItem interactableItem)
		{
			if (!PlayerDashboardInventoryUI.active || !PlayerDashboardUI.active)
			{
				return;
			}
			if (Player.player == null)
			{
				return;
			}
			if (PlayerDashboardInventoryUI.areaItems.getItemCount() >= 200)
			{
				return;
			}
			Vector3 eyesPositionWithoutLeaning = Player.player.look.GetEyesPositionWithoutLeaning();
			if ((model.position - eyesPositionWithoutLeaning).sqrMagnitude > 16f)
			{
				return;
			}
			PlayerDashboardInventoryUI.pendingItemsInRadius.Add(interactableItem);
		}

		// Token: 0x06004335 RID: 17205 RVA: 0x00177914 File Offset: 0x00175B14
		private static void onItemDropRemoved(Transform model, InteractableItem interactableItem)
		{
			if (!PlayerDashboardInventoryUI.active || !PlayerDashboardUI.active)
			{
				return;
			}
			ItemJar jar = interactableItem.jar;
			if (jar == null)
			{
				return;
			}
			int num = PlayerDashboardInventoryUI.areaItems.FindIndexOfJar(jar);
			if (num < 0)
			{
				PlayerDashboardInventoryUI.pendingItemsInRadius.RemoveFast(interactableItem);
				return;
			}
			PlayerDashboardInventoryUI.areaItems.removeItem((byte)num);
			PlayerDashboardInventoryUI.items[(int)(PlayerInventory.AREA - PlayerInventory.SLOTS)].removeItem(jar);
		}

		// Token: 0x06004336 RID: 17206 RVA: 0x0017797C File Offset: 0x00175B7C
		private static void onSeated(bool isDriver, bool inVehicle, bool wasVehicle, InteractableVehicle oldVehicle, InteractableVehicle newVehicle)
		{
			if (oldVehicle != null)
			{
				oldVehicle.onPassengersUpdated -= PlayerDashboardInventoryUI.updateVehicle;
				oldVehicle.onLockUpdated -= PlayerDashboardInventoryUI.onVehicleLockUpdated;
				oldVehicle.onHeadlightsUpdated -= PlayerDashboardInventoryUI.updateVehicle;
				oldVehicle.onSirensUpdated -= PlayerDashboardInventoryUI.updateVehicle;
				oldVehicle.onBlimpUpdated -= PlayerDashboardInventoryUI.updateVehicle;
				oldVehicle.batteryChanged -= PlayerDashboardInventoryUI.updateVehicle;
				oldVehicle.skinChanged -= PlayerDashboardInventoryUI.updateVehicle;
			}
			if (newVehicle != null)
			{
				newVehicle.onPassengersUpdated += PlayerDashboardInventoryUI.updateVehicle;
				newVehicle.onLockUpdated += PlayerDashboardInventoryUI.onVehicleLockUpdated;
				newVehicle.onHeadlightsUpdated += PlayerDashboardInventoryUI.updateVehicle;
				newVehicle.onSirensUpdated += PlayerDashboardInventoryUI.updateVehicle;
				newVehicle.onBlimpUpdated += PlayerDashboardInventoryUI.updateVehicle;
				newVehicle.batteryChanged += PlayerDashboardInventoryUI.updateVehicle;
				newVehicle.skinChanged += PlayerDashboardInventoryUI.updateVehicle;
			}
			PlayerDashboardInventoryUI.updateVehicle();
		}

		// Token: 0x06004337 RID: 17207 RVA: 0x00177AA8 File Offset: 0x00175CA8
		private static void onVehicleLockUpdated()
		{
			PlayerDashboardInventoryUI.updateVehicle();
			InteractableVehicle vehicle = Player.player.movement.getVehicle();
			if (vehicle == null)
			{
				return;
			}
			PlayerUI.message(vehicle.isLocked ? EPlayerMessage.VEHICLE_LOCKED : EPlayerMessage.VEHICLE_UNLOCKED, string.Empty, 2f);
		}

		// Token: 0x06004338 RID: 17208 RVA: 0x00177AF4 File Offset: 0x00175CF4
		private static void updateVehicle()
		{
			if (!PlayerDashboardInventoryUI.active)
			{
				return;
			}
			InteractableVehicle vehicle = Player.player.movement.getVehicle();
			if (vehicle != null && vehicle.asset != null)
			{
				VehicleAsset asset = vehicle.asset;
				PlayerDashboardInventoryUI.vehicleNameLabel.Text = asset.vehicleName;
				PlayerDashboardInventoryUI.vehicleNameLabel.TextColor = ItemTool.getRarityColorUI(asset.rarity);
				int num = 0;
				int num2 = 0;
				if (asset.canBeLocked)
				{
					PlayerDashboardInventoryUI.vehicleLockButton.Text = PlayerDashboardInventoryUI.localization.format(vehicle.isLocked ? "Vehicle_Lock_Off" : "Vehicle_Lock_On", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.locker));
					PlayerDashboardInventoryUI.vehicleLockButton.TooltipText = PlayerDashboardInventoryUI.localization.format("Vehicle_Lock_Tooltip");
					PlayerDashboardInventoryUI.vehicleLockButton.IsVisible = true;
					PlayerDashboardInventoryUI.vehicleLockButton.PositionOffset_Y = (float)num;
					num += 40;
				}
				else
				{
					PlayerDashboardInventoryUI.vehicleLockButton.IsVisible = false;
				}
				if (asset.hasHorn)
				{
					PlayerDashboardInventoryUI.vehicleHornButton.Text = PlayerDashboardInventoryUI.localization.format("Vehicle_Horn", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.primary));
					PlayerDashboardInventoryUI.vehicleHornButton.TooltipText = PlayerDashboardInventoryUI.localization.format("Vehicle_Horn_Tooltip");
					PlayerDashboardInventoryUI.vehicleHornButton.IsVisible = true;
					PlayerDashboardInventoryUI.vehicleHornButton.PositionOffset_Y = (float)num;
					num += 40;
				}
				else
				{
					PlayerDashboardInventoryUI.vehicleHornButton.IsVisible = false;
				}
				if (asset.hasHeadlights)
				{
					PlayerDashboardInventoryUI.vehicleHeadlightsButton.Text = PlayerDashboardInventoryUI.localization.format(vehicle.headlightsOn ? "Vehicle_Headlights_Off" : "Vehicle_Headlights_On", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.secondary));
					PlayerDashboardInventoryUI.vehicleHeadlightsButton.TooltipText = PlayerDashboardInventoryUI.localization.format("Vehicle_Headlights_Tooltip");
					PlayerDashboardInventoryUI.vehicleHeadlightsButton.IsVisible = true;
					PlayerDashboardInventoryUI.vehicleHeadlightsButton.PositionOffset_Y = (float)num;
					num += 40;
				}
				else
				{
					PlayerDashboardInventoryUI.vehicleHeadlightsButton.IsVisible = false;
				}
				if (asset.hasSirens)
				{
					PlayerDashboardInventoryUI.vehicleSirensButton.Text = PlayerDashboardInventoryUI.localization.format(vehicle.sirensOn ? "Vehicle_Sirens_Off" : "Vehicle_Sirens_On", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.other));
					PlayerDashboardInventoryUI.vehicleSirensButton.TooltipText = PlayerDashboardInventoryUI.localization.format("Vehicle_Sirens_Tooltip");
					PlayerDashboardInventoryUI.vehicleSirensButton.IsVisible = true;
					PlayerDashboardInventoryUI.vehicleSirensButton.PositionOffset_Y = (float)num;
					num += 40;
				}
				else
				{
					PlayerDashboardInventoryUI.vehicleSirensButton.IsVisible = false;
				}
				if (asset.engine == EEngine.BLIMP)
				{
					PlayerDashboardInventoryUI.vehicleBlimpButton.Text = PlayerDashboardInventoryUI.localization.format(vehicle.isBlimpFloating ? "Vehicle_Blimp_Off" : "Vehicle_Blimp_On", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.other));
					PlayerDashboardInventoryUI.vehicleBlimpButton.TooltipText = PlayerDashboardInventoryUI.localization.format("Vehicle_Blimp_Tooltip");
					PlayerDashboardInventoryUI.vehicleBlimpButton.IsVisible = true;
					PlayerDashboardInventoryUI.vehicleBlimpButton.PositionOffset_Y = (float)num;
					num += 40;
				}
				else
				{
					PlayerDashboardInventoryUI.vehicleBlimpButton.IsVisible = false;
				}
				if (asset.hasHook)
				{
					PlayerDashboardInventoryUI.vehicleHookButton.Text = PlayerDashboardInventoryUI.localization.format("Vehicle_Hook", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.other));
					PlayerDashboardInventoryUI.vehicleHookButton.TooltipText = PlayerDashboardInventoryUI.localization.format("Vehicle_Hook_Tooltip");
					PlayerDashboardInventoryUI.vehicleHookButton.IsVisible = true;
					PlayerDashboardInventoryUI.vehicleHookButton.PositionOffset_Y = (float)num;
					num += 40;
				}
				else
				{
					PlayerDashboardInventoryUI.vehicleHookButton.IsVisible = false;
				}
				if (vehicle.usesBattery && vehicle.ContainsBatteryItem && vehicle.asset.canStealBattery)
				{
					PlayerDashboardInventoryUI.vehicleStealBatteryButton.Text = PlayerDashboardInventoryUI.localization.format("Vehicle_Steal_Battery");
					PlayerDashboardInventoryUI.vehicleStealBatteryButton.TooltipText = PlayerDashboardInventoryUI.localization.format("Vehicle_Steal_Battery_Tooltip");
					PlayerDashboardInventoryUI.vehicleStealBatteryButton.IsVisible = true;
					PlayerDashboardInventoryUI.vehicleStealBatteryButton.PositionOffset_Y = (float)num;
					num += 40;
				}
				else
				{
					PlayerDashboardInventoryUI.vehicleStealBatteryButton.IsVisible = false;
				}
				int item = 0;
				ushort num3 = 0;
				ushort num4 = 0;
				if (Player.player.channel.owner.skinItems != null && Player.player.channel.owner.GetVehicleSkinItemDefId(vehicle, out item))
				{
					num3 = Provider.provider.economyService.getInventorySkinID(item);
					num4 = Provider.provider.economyService.getInventoryMythicID(item);
				}
				bool flag;
				bool flag2;
				if (num3 != 0)
				{
					if (num3 == vehicle.skinID && num4 == vehicle.mythicID)
					{
						flag = true;
						flag2 = true;
					}
					else
					{
						flag = false;
						flag2 = true;
					}
				}
				else if (vehicle.isSkinned)
				{
					flag = true;
					flag2 = true;
				}
				else
				{
					flag = false;
					flag2 = false;
				}
				if (flag2)
				{
					PlayerDashboardInventoryUI.vehicleSkinButton.Text = PlayerDashboardInventoryUI.localization.format(flag ? "Vehicle_Skin_Off" : "Vehicle_Skin_On");
					PlayerDashboardInventoryUI.vehicleSkinButton.TooltipText = PlayerDashboardInventoryUI.localization.format("Vehicle_Skin_Tooltip");
					PlayerDashboardInventoryUI.vehicleSkinButton.IsVisible = true;
					PlayerDashboardInventoryUI.vehicleSkinButton.PositionOffset_Y = (float)num;
					num += 40;
				}
				else
				{
					PlayerDashboardInventoryUI.vehicleSkinButton.IsVisible = false;
				}
				if (Player.player.stance.stance == EPlayerStance.DRIVING)
				{
					PlayerDashboardInventoryUI.vehiclePassengersBox.PositionOffset_X = 270f;
					PlayerDashboardInventoryUI.vehiclePassengersBox.SizeOffset_X = -280f;
					PlayerDashboardInventoryUI.vehicleActionsBox.IsVisible = true;
				}
				else
				{
					PlayerDashboardInventoryUI.vehiclePassengersBox.PositionOffset_X = 10f;
					PlayerDashboardInventoryUI.vehiclePassengersBox.SizeOffset_X = -20f;
					PlayerDashboardInventoryUI.vehicleActionsBox.IsVisible = false;
				}
				PlayerDashboardInventoryUI.vehiclePassengersBox.RemoveAllChildren();
				for (int i = 0; i < vehicle.passengers.Length; i++)
				{
					Passenger passenger = vehicle.passengers[i];
					ISleekButton sleekButton = Glazier.Get().CreateButton();
					sleekButton.PositionOffset_Y = (float)num2;
					sleekButton.SizeOffset_Y = 30f;
					sleekButton.SizeScale_X = 1f;
					sleekButton.OnClicked += new ClickedButton(PlayerDashboardInventoryUI.onClickedVehiclePassengerButton);
					PlayerDashboardInventoryUI.vehiclePassengersBox.AddChild(sleekButton);
					if (passenger.player != null)
					{
						string localDisplayName = passenger.player.GetLocalDisplayName();
						if (i < 12)
						{
							sleekButton.Text = PlayerDashboardInventoryUI.localization.format("Vehicle_Seat_Slot", localDisplayName, "F" + (i + 1).ToString());
						}
						else
						{
							sleekButton.Text = localDisplayName;
						}
					}
					else if (i < 12)
					{
						sleekButton.Text = PlayerDashboardInventoryUI.localization.format("Vehicle_Seat_Slot", PlayerDashboardInventoryUI.localization.format("Vehicle_Seat_Empty"), "F" + (i + 1).ToString());
					}
					else
					{
						sleekButton.Text = PlayerDashboardInventoryUI.localization.format("Vehicle_Seat_Empty");
					}
					num2 += 40;
				}
				PlayerDashboardInventoryUI.vehicleActionsBox.SizeOffset_Y = (float)(num - 10);
				PlayerDashboardInventoryUI.vehiclePassengersBox.SizeOffset_Y = (float)(num2 - 10);
				PlayerDashboardInventoryUI.vehicleBox.IsVisible = true;
				int num5 = Mathf.Max(num, num2);
				PlayerDashboardInventoryUI.vehicleBox.SizeOffset_Y = (float)(60 + num5);
				PlayerDashboardInventoryUI.headers[(int)(PlayerInventory.STORAGE - PlayerInventory.SLOTS)].TextColor = PlayerDashboardInventoryUI.vehicleNameLabel.TextColor;
				PlayerDashboardInventoryUI.headers[(int)(PlayerInventory.STORAGE - PlayerInventory.SLOTS)].Text = PlayerDashboardInventoryUI.localization.format("Storage_Trunk", PlayerDashboardInventoryUI.vehicleNameLabel.Text);
			}
			else
			{
				PlayerDashboardInventoryUI.vehicleBox.IsVisible = false;
				PlayerDashboardInventoryUI.headers[(int)(PlayerInventory.STORAGE - PlayerInventory.SLOTS)].TextColor = 3;
				PlayerDashboardInventoryUI.headers[(int)(PlayerInventory.STORAGE - PlayerInventory.SLOTS)].Text = PlayerDashboardInventoryUI.localization.format("Storage");
			}
			PlayerDashboardInventoryUI.updateBoxAreas();
		}

		// Token: 0x06004339 RID: 17209 RVA: 0x00178230 File Offset: 0x00176430
		private static void resetNearbyDrops()
		{
			Vector3 eyesPositionWithoutLeaning = Player.player.look.GetEyesPositionWithoutLeaning();
			PlayerDashboardInventoryUI.pendingItemsInRadius.Clear();
			ItemManager.findSimulatedItemsInRadius(eyesPositionWithoutLeaning, 16f, PlayerDashboardInventoryUI.pendingItemsInRadius);
			PlayerDashboardInventoryUI.areaItems.clear();
			PlayerDashboardInventoryUI.areaItems.resize(8, 3);
			Player.player.inventory.replaceItems(PlayerInventory.AREA, PlayerDashboardInventoryUI.areaItems);
			SleekItems sleekItems = PlayerDashboardInventoryUI.items[(int)(PlayerInventory.AREA - PlayerInventory.SLOTS)];
			sleekItems.clear();
			sleekItems.resize(PlayerDashboardInventoryUI.areaItems.width, PlayerDashboardInventoryUI.areaItems.height);
			PlayerDashboardInventoryUI.updateBoxAreas();
		}

		// Token: 0x0600433A RID: 17210 RVA: 0x001782CC File Offset: 0x001764CC
		private static void onInventoryResized(byte page, byte newWidth, byte newHeight)
		{
			if (page < PlayerInventory.SLOTS)
			{
				return;
			}
			page -= PlayerInventory.SLOTS;
			PlayerDashboardInventoryUI.items[(int)page].resize(newWidth, newHeight);
			if (page > 0)
			{
				PlayerDashboardInventoryUI.headers[(int)page].IsVisible = (newHeight > 0);
			}
			PlayerDashboardInventoryUI.items[(int)page].IsVisible = (newHeight > 0);
			if (page == PlayerInventory.STORAGE - PlayerInventory.SLOTS && newHeight == 0)
			{
				PlayerDashboardInventoryUI.items[(int)page].clear();
			}
			PlayerDashboardInventoryUI.updateBoxAreas();
		}

		// Token: 0x0600433B RID: 17211 RVA: 0x00178340 File Offset: 0x00176540
		private static void updateBoxAreas()
		{
			float num = 0f;
			float num2 = 0f;
			bool isSplitClothingArea = PlayerDashboardInventoryUI.isSplitClothingArea;
			if (PlayerDashboardInventoryUI.vehicleBox.IsVisible)
			{
				if (isSplitClothingArea)
				{
					if (PlayerDashboardInventoryUI.vehicleBox.Parent != PlayerDashboardInventoryUI.areaBox)
					{
						PlayerDashboardInventoryUI.areaBox.AddChild(PlayerDashboardInventoryUI.vehicleBox);
					}
					PlayerDashboardInventoryUI.vehicleBox.PositionOffset_Y = num2;
					num2 += PlayerDashboardInventoryUI.vehicleBox.SizeOffset_Y + 10f;
				}
				else
				{
					if (PlayerDashboardInventoryUI.vehicleBox.Parent != PlayerDashboardInventoryUI.clothingBox)
					{
						PlayerDashboardInventoryUI.clothingBox.AddChild(PlayerDashboardInventoryUI.vehicleBox);
					}
					PlayerDashboardInventoryUI.vehicleBox.PositionOffset_Y = num;
					num += PlayerDashboardInventoryUI.vehicleBox.SizeOffset_Y + 10f;
				}
			}
			byte b = 0;
			while ((int)b < PlayerDashboardInventoryUI.items.Length)
			{
				if (PlayerDashboardInventoryUI.headers[(int)b].IsVisible)
				{
					if (isSplitClothingArea && (b == PlayerInventory.STORAGE - PlayerInventory.SLOTS || b == PlayerInventory.AREA - PlayerInventory.SLOTS))
					{
						if (PlayerDashboardInventoryUI.headers[(int)b].Parent != PlayerDashboardInventoryUI.areaBox)
						{
							PlayerDashboardInventoryUI.areaBox.AddChild(PlayerDashboardInventoryUI.headers[(int)b]);
						}
						if (PlayerDashboardInventoryUI.items[(int)b].Parent != PlayerDashboardInventoryUI.areaBox)
						{
							PlayerDashboardInventoryUI.areaBox.AddChild(PlayerDashboardInventoryUI.items[(int)b]);
						}
						PlayerDashboardInventoryUI.headers[(int)b].PositionOffset_Y = num2;
						PlayerDashboardInventoryUI.items[(int)b].PositionOffset_Y = num2 + 70f;
						num2 += PlayerDashboardInventoryUI.items[(int)b].SizeOffset_Y + 80f;
					}
					else
					{
						if (PlayerDashboardInventoryUI.headers[(int)b].Parent != PlayerDashboardInventoryUI.clothingBox)
						{
							PlayerDashboardInventoryUI.clothingBox.AddChild(PlayerDashboardInventoryUI.headers[(int)b]);
						}
						if (PlayerDashboardInventoryUI.items[(int)b].Parent != PlayerDashboardInventoryUI.clothingBox)
						{
							PlayerDashboardInventoryUI.clothingBox.AddChild(PlayerDashboardInventoryUI.items[(int)b]);
						}
						PlayerDashboardInventoryUI.headers[(int)b].PositionOffset_Y = num;
						PlayerDashboardInventoryUI.items[(int)b].PositionOffset_Y = num + 70f;
						num += PlayerDashboardInventoryUI.items[(int)b].SizeOffset_Y + 80f;
					}
				}
				b += 1;
			}
			PlayerDashboardInventoryUI.headers[7].IsVisible = (Player.player.clothing.hatAsset != null);
			if (PlayerDashboardInventoryUI.headers[7].IsVisible)
			{
				PlayerDashboardInventoryUI.headers[7].PositionOffset_Y = num;
				num += 70f;
			}
			PlayerDashboardInventoryUI.headers[8].IsVisible = (Player.player.clothing.maskAsset != null);
			if (PlayerDashboardInventoryUI.headers[8].IsVisible)
			{
				PlayerDashboardInventoryUI.headers[8].PositionOffset_Y = num;
				num += 70f;
			}
			PlayerDashboardInventoryUI.headers[9].IsVisible = (Player.player.clothing.glassesAsset != null);
			if (PlayerDashboardInventoryUI.headers[9].IsVisible)
			{
				PlayerDashboardInventoryUI.headers[9].PositionOffset_Y = num;
				num += 70f;
			}
			PlayerDashboardInventoryUI.clothingBox.ContentSizeOffset = new Vector2(0f, num - 10f);
			PlayerDashboardInventoryUI.areaBox.ContentSizeOffset = new Vector2(0f, num2 - 10f);
			InteractableStorage interactableStorage = PlayerInteract.interactable as InteractableStorage;
			if (interactableStorage != null && interactableStorage.isDisplay)
			{
				PlayerDashboardInventoryUI.headers[(int)(PlayerInventory.STORAGE - PlayerInventory.SLOTS)].SizeOffset_X = -180f;
				PlayerDashboardInventoryUI.rot_xButton.IsVisible = true;
				PlayerDashboardInventoryUI.rot_yButton.IsVisible = true;
				PlayerDashboardInventoryUI.rot_zButton.IsVisible = true;
				return;
			}
			PlayerDashboardInventoryUI.headers[(int)(PlayerInventory.STORAGE - PlayerInventory.SLOTS)].SizeOffset_X = 0f;
			PlayerDashboardInventoryUI.rot_xButton.IsVisible = false;
			PlayerDashboardInventoryUI.rot_yButton.IsVisible = false;
			PlayerDashboardInventoryUI.rot_zButton.IsVisible = false;
		}

		// Token: 0x0600433C RID: 17212 RVA: 0x001786E4 File Offset: 0x001768E4
		private static void updateHotkeys()
		{
			for (byte b = 0; b < PlayerInventory.STORAGE - PlayerInventory.SLOTS; b += 1)
			{
				PlayerDashboardInventoryUI.items[(int)b].resetHotkeyDisplay();
			}
			byte b2 = 0;
			while ((int)b2 < Player.player.equipment.hotkeys.Length)
			{
				HotkeyInfo hotkeyInfo = Player.player.equipment.hotkeys[(int)b2];
				byte button = b2 + 2;
				byte b3 = hotkeyInfo.page - 2;
				if (hotkeyInfo.id != 0)
				{
					byte index = Player.player.inventory.getIndex(hotkeyInfo.page, hotkeyInfo.x, hotkeyInfo.y);
					ItemJar item = Player.player.inventory.getItem(hotkeyInfo.page, index);
					if (item == null || item.item.id != hotkeyInfo.id)
					{
						hotkeyInfo.id = 0;
						hotkeyInfo.page = byte.MaxValue;
						hotkeyInfo.x = byte.MaxValue;
						hotkeyInfo.y = byte.MaxValue;
					}
					else
					{
						PlayerDashboardInventoryUI.items[(int)b3].updateHotkey(item, button);
					}
				}
				b2 += 1;
			}
		}

		// Token: 0x0600433D RID: 17213 RVA: 0x001787F2 File Offset: 0x001769F2
		private static void onHotkeysUpdated()
		{
			PlayerDashboardInventoryUI.updateHotkeys();
		}

		// Token: 0x0600433E RID: 17214 RVA: 0x001787F9 File Offset: 0x001769F9
		private static void onInventoryUpdated(byte page, byte index, ItemJar jar)
		{
			if (page < PlayerInventory.SLOTS)
			{
				PlayerDashboardInventoryUI.slots[(int)page].updateItem(jar);
				return;
			}
			page -= PlayerInventory.SLOTS;
			PlayerDashboardInventoryUI.items[(int)page].updateItem(jar);
		}

		// Token: 0x0600433F RID: 17215 RVA: 0x00178828 File Offset: 0x00176A28
		private static void onInventoryAdded(byte page, byte index, ItemJar jar)
		{
			if (page < PlayerInventory.SLOTS)
			{
				PlayerDashboardInventoryUI.slots[(int)page].applyItem(jar);
				return;
			}
			page -= PlayerInventory.SLOTS;
			PlayerDashboardInventoryUI.items[(int)page].addItem(jar);
		}

		// Token: 0x06004340 RID: 17216 RVA: 0x00178858 File Offset: 0x00176A58
		private static void onInventoryRemoved(byte page, byte index, ItemJar jar)
		{
			if (page == PlayerDashboardInventoryUI.selectedPage && jar.x == PlayerDashboardInventoryUI.selected_x && jar.y == PlayerDashboardInventoryUI.selected_y)
			{
				PlayerDashboardInventoryUI.closeSelection();
			}
			if (page < PlayerInventory.SLOTS)
			{
				PlayerDashboardInventoryUI.slots[(int)page].applyItem(null);
				return;
			}
			page -= PlayerInventory.SLOTS;
			PlayerDashboardInventoryUI.items[(int)page].removeItem(jar);
		}

		// Token: 0x06004341 RID: 17217 RVA: 0x001788BC File Offset: 0x00176ABC
		private static void onInventoryStored()
		{
			if (Player.player.inventory.shouldStorageOpenDashboard)
			{
				PlayerLifeUI.close();
				PlayerPauseUI.close();
				if (PlayerDashboardUI.active)
				{
					PlayerDashboardCraftingUI.close();
					PlayerDashboardSkillsUI.close();
					PlayerDashboardInformationUI.close();
					PlayerDashboardInventoryUI.open();
				}
				else
				{
					PlayerDashboardInventoryUI.active = true;
					PlayerDashboardCraftingUI.active = false;
					PlayerDashboardSkillsUI.active = false;
					PlayerDashboardInformationUI.active = false;
					PlayerDashboardUI.open();
				}
			}
			if (!PlayerDashboardInventoryUI.isSplitClothingArea)
			{
				PlayerDashboardInventoryUI.clothingBox.ScrollToBottom();
			}
		}

		// Token: 0x06004342 RID: 17218 RVA: 0x00178930 File Offset: 0x00176B30
		private static void onShirtUpdated(ushort newShirtObsolete, byte newShirtQuality, byte[] newShirtState)
		{
			ItemAsset shirtAsset = Player.player.clothing.shirtAsset;
			if (shirtAsset != null)
			{
				PlayerDashboardInventoryUI.headers[3].Text = shirtAsset.itemName;
				PlayerDashboardInventoryUI.headers[3].GetChildAtIndex(0).SizeOffset_X = (float)(shirtAsset.size_x * 25);
				PlayerDashboardInventoryUI.headers[3].GetChildAtIndex(0).SizeOffset_Y = (float)(shirtAsset.size_y * 25);
				PlayerDashboardInventoryUI.headers[3].GetChildAtIndex(0).PositionOffset_Y = -PlayerDashboardInventoryUI.headers[3].GetChildAtIndex(0).SizeOffset_Y / 2f;
				PlayerDashboardInventoryUI.headerItemIcons[3].Refresh(shirtAsset.id, newShirtQuality, newShirtState);
				((ISleekLabel)PlayerDashboardInventoryUI.headers[3].GetChildAtIndex(2)).Text = newShirtQuality.ToString() + "%";
				Color rarityColorUI = ItemTool.getRarityColorUI(shirtAsset.rarity);
				PlayerDashboardInventoryUI.headers[3].BackgroundColor = SleekColor.BackgroundIfLight(rarityColorUI);
				PlayerDashboardInventoryUI.headers[3].TextColor = rarityColorUI;
				Color qualityColor = ItemTool.getQualityColor((float)newShirtQuality / 100f);
				((ISleekImage)PlayerDashboardInventoryUI.headers[3].GetChildAtIndex(1)).TintColor = qualityColor;
				((ISleekLabel)PlayerDashboardInventoryUI.headers[3].GetChildAtIndex(2)).TextColor = qualityColor;
			}
		}

		// Token: 0x06004343 RID: 17219 RVA: 0x00178A7C File Offset: 0x00176C7C
		private static void onPantsUpdated(ushort newPantsObsolete, byte newPantsQuality, byte[] newPantsState)
		{
			if (PlayerDashboardInventoryUI.headers != null)
			{
				ItemAsset pantsAsset = Player.player.clothing.pantsAsset;
				if (pantsAsset != null)
				{
					PlayerDashboardInventoryUI.headers[4].Text = pantsAsset.itemName;
					PlayerDashboardInventoryUI.headers[4].GetChildAtIndex(0).SizeOffset_X = (float)(pantsAsset.size_x * 25);
					PlayerDashboardInventoryUI.headers[4].GetChildAtIndex(0).SizeOffset_Y = (float)(pantsAsset.size_y * 25);
					PlayerDashboardInventoryUI.headers[4].GetChildAtIndex(0).PositionOffset_Y = -PlayerDashboardInventoryUI.headers[4].GetChildAtIndex(0).SizeOffset_Y / 2f;
					PlayerDashboardInventoryUI.headerItemIcons[4].Refresh(pantsAsset.id, newPantsQuality, newPantsState);
					((ISleekLabel)PlayerDashboardInventoryUI.headers[4].GetChildAtIndex(2)).Text = newPantsQuality.ToString() + "%";
					Color rarityColorUI = ItemTool.getRarityColorUI(pantsAsset.rarity);
					PlayerDashboardInventoryUI.headers[4].BackgroundColor = SleekColor.BackgroundIfLight(rarityColorUI);
					PlayerDashboardInventoryUI.headers[4].TextColor = rarityColorUI;
					Color qualityColor = ItemTool.getQualityColor((float)newPantsQuality / 100f);
					((ISleekImage)PlayerDashboardInventoryUI.headers[4].GetChildAtIndex(1)).TintColor = qualityColor;
					((ISleekLabel)PlayerDashboardInventoryUI.headers[4].GetChildAtIndex(2)).TextColor = qualityColor;
				}
			}
		}

		// Token: 0x06004344 RID: 17220 RVA: 0x00178BD4 File Offset: 0x00176DD4
		private static void onHatUpdated(ushort newHatObsolete, byte newHatQuality, byte[] newHatState)
		{
			if (PlayerDashboardInventoryUI.headers != null)
			{
				ItemAsset hatAsset = Player.player.clothing.hatAsset;
				if (hatAsset != null)
				{
					PlayerDashboardInventoryUI.headers[7].Text = hatAsset.itemName;
					PlayerDashboardInventoryUI.headers[7].GetChildAtIndex(0).SizeOffset_X = (float)(hatAsset.size_x * 25);
					PlayerDashboardInventoryUI.headers[7].GetChildAtIndex(0).SizeOffset_Y = (float)(hatAsset.size_y * 25);
					PlayerDashboardInventoryUI.headers[7].GetChildAtIndex(0).PositionOffset_Y = -PlayerDashboardInventoryUI.headers[7].GetChildAtIndex(0).SizeOffset_Y / 2f;
					PlayerDashboardInventoryUI.headerItemIcons[7].Refresh(newHatObsolete, newHatQuality, newHatState);
					((ISleekLabel)PlayerDashboardInventoryUI.headers[7].GetChildAtIndex(2)).Text = newHatQuality.ToString() + "%";
					Color rarityColorUI = ItemTool.getRarityColorUI(hatAsset.rarity);
					PlayerDashboardInventoryUI.headers[7].BackgroundColor = SleekColor.BackgroundIfLight(rarityColorUI);
					PlayerDashboardInventoryUI.headers[7].TextColor = rarityColorUI;
					Color qualityColor = ItemTool.getQualityColor((float)newHatQuality / 100f);
					((ISleekImage)PlayerDashboardInventoryUI.headers[7].GetChildAtIndex(1)).TintColor = qualityColor;
					((ISleekLabel)PlayerDashboardInventoryUI.headers[7].GetChildAtIndex(2)).TextColor = qualityColor;
				}
				PlayerDashboardInventoryUI.headers[7].IsVisible = (hatAsset != null);
				PlayerDashboardInventoryUI.updateBoxAreas();
			}
		}

		// Token: 0x06004345 RID: 17221 RVA: 0x00178D3C File Offset: 0x00176F3C
		private static void onBackpackUpdated(ushort newBackpackObsolete, byte newBackpackQuality, byte[] newBackpackState)
		{
			ItemAsset backpackAsset = Player.player.clothing.backpackAsset;
			if (backpackAsset != null)
			{
				PlayerDashboardInventoryUI.headers[1].Text = backpackAsset.itemName;
				PlayerDashboardInventoryUI.headers[1].GetChildAtIndex(0).SizeOffset_X = (float)(backpackAsset.size_x * 25);
				PlayerDashboardInventoryUI.headers[1].GetChildAtIndex(0).SizeOffset_Y = (float)(backpackAsset.size_y * 25);
				PlayerDashboardInventoryUI.headers[1].GetChildAtIndex(0).PositionOffset_Y = -PlayerDashboardInventoryUI.headers[1].GetChildAtIndex(0).SizeOffset_Y / 2f;
				PlayerDashboardInventoryUI.headerItemIcons[1].Refresh(backpackAsset.id, newBackpackQuality, newBackpackState);
				((ISleekLabel)PlayerDashboardInventoryUI.headers[1].GetChildAtIndex(2)).Text = newBackpackQuality.ToString() + "%";
				Color rarityColorUI = ItemTool.getRarityColorUI(backpackAsset.rarity);
				PlayerDashboardInventoryUI.headers[1].BackgroundColor = SleekColor.BackgroundIfLight(rarityColorUI);
				PlayerDashboardInventoryUI.headers[1].TextColor = rarityColorUI;
				Color qualityColor = ItemTool.getQualityColor((float)newBackpackQuality / 100f);
				((ISleekImage)PlayerDashboardInventoryUI.headers[1].GetChildAtIndex(1)).TintColor = qualityColor;
				((ISleekLabel)PlayerDashboardInventoryUI.headers[1].GetChildAtIndex(2)).TextColor = qualityColor;
			}
		}

		// Token: 0x06004346 RID: 17222 RVA: 0x00178E88 File Offset: 0x00177088
		private static void onVestUpdated(ushort newVestObsolete, byte newVestQuality, byte[] newVestState)
		{
			ItemAsset vestAsset = Player.player.clothing.vestAsset;
			if (vestAsset != null)
			{
				PlayerDashboardInventoryUI.headers[2].Text = vestAsset.itemName;
				PlayerDashboardInventoryUI.headers[2].GetChildAtIndex(0).SizeOffset_X = (float)(vestAsset.size_x * 25);
				PlayerDashboardInventoryUI.headers[2].GetChildAtIndex(0).SizeOffset_Y = (float)(vestAsset.size_y * 25);
				PlayerDashboardInventoryUI.headers[2].GetChildAtIndex(0).PositionOffset_Y = -PlayerDashboardInventoryUI.headers[2].GetChildAtIndex(0).SizeOffset_Y / 2f;
				PlayerDashboardInventoryUI.headerItemIcons[2].Refresh(vestAsset.id, newVestQuality, newVestState);
				((ISleekLabel)PlayerDashboardInventoryUI.headers[2].GetChildAtIndex(2)).Text = newVestQuality.ToString() + "%";
				Color rarityColorUI = ItemTool.getRarityColorUI(vestAsset.rarity);
				PlayerDashboardInventoryUI.headers[2].BackgroundColor = SleekColor.BackgroundIfLight(rarityColorUI);
				PlayerDashboardInventoryUI.headers[2].TextColor = rarityColorUI;
				Color qualityColor = ItemTool.getQualityColor((float)newVestQuality / 100f);
				((ISleekImage)PlayerDashboardInventoryUI.headers[2].GetChildAtIndex(1)).TintColor = qualityColor;
				((ISleekLabel)PlayerDashboardInventoryUI.headers[2].GetChildAtIndex(2)).TextColor = qualityColor;
			}
		}

		// Token: 0x06004347 RID: 17223 RVA: 0x00178FD4 File Offset: 0x001771D4
		private static void onMaskUpdated(ushort newMaskObsolete, byte newMaskQuality, byte[] newMaskState)
		{
			if (PlayerDashboardInventoryUI.headers != null)
			{
				ItemAsset maskAsset = Player.player.clothing.maskAsset;
				if (maskAsset != null)
				{
					PlayerDashboardInventoryUI.headers[8].Text = maskAsset.itemName;
					PlayerDashboardInventoryUI.headers[8].GetChildAtIndex(0).SizeOffset_X = (float)(maskAsset.size_x * 25);
					PlayerDashboardInventoryUI.headers[8].GetChildAtIndex(0).SizeOffset_Y = (float)(maskAsset.size_y * 25);
					PlayerDashboardInventoryUI.headers[8].GetChildAtIndex(0).PositionOffset_Y = -PlayerDashboardInventoryUI.headers[8].GetChildAtIndex(0).SizeOffset_Y / 2f;
					PlayerDashboardInventoryUI.headerItemIcons[8].Refresh(maskAsset.id, newMaskQuality, newMaskState);
					((ISleekLabel)PlayerDashboardInventoryUI.headers[8].GetChildAtIndex(2)).Text = newMaskQuality.ToString() + "%";
					Color rarityColorUI = ItemTool.getRarityColorUI(maskAsset.rarity);
					PlayerDashboardInventoryUI.headers[8].BackgroundColor = SleekColor.BackgroundIfLight(rarityColorUI);
					PlayerDashboardInventoryUI.headers[8].TextColor = rarityColorUI;
					Color qualityColor = ItemTool.getQualityColor((float)newMaskQuality / 100f);
					((ISleekImage)PlayerDashboardInventoryUI.headers[8].GetChildAtIndex(1)).TintColor = qualityColor;
					((ISleekLabel)PlayerDashboardInventoryUI.headers[8].GetChildAtIndex(2)).TextColor = qualityColor;
				}
				PlayerDashboardInventoryUI.headers[8].IsVisible = (maskAsset != null);
				PlayerDashboardInventoryUI.updateBoxAreas();
			}
		}

		// Token: 0x06004348 RID: 17224 RVA: 0x00179140 File Offset: 0x00177340
		private static void onGlassesUpdated(ushort newGlassesObsolete, byte newGlassesQuality, byte[] newGlassesState)
		{
			if (PlayerDashboardInventoryUI.headers != null)
			{
				ItemAsset glassesAsset = Player.player.clothing.glassesAsset;
				if (glassesAsset != null)
				{
					PlayerDashboardInventoryUI.headers[9].Text = glassesAsset.itemName;
					PlayerDashboardInventoryUI.headers[9].GetChildAtIndex(0).SizeOffset_X = (float)(glassesAsset.size_x * 25);
					PlayerDashboardInventoryUI.headers[9].GetChildAtIndex(0).SizeOffset_Y = (float)(glassesAsset.size_y * 25);
					PlayerDashboardInventoryUI.headers[9].GetChildAtIndex(0).PositionOffset_Y = -PlayerDashboardInventoryUI.headers[9].GetChildAtIndex(0).SizeOffset_Y / 2f;
					PlayerDashboardInventoryUI.headerItemIcons[9].Refresh(glassesAsset.id, newGlassesQuality, newGlassesState);
					((ISleekLabel)PlayerDashboardInventoryUI.headers[9].GetChildAtIndex(2)).Text = newGlassesQuality.ToString() + "%";
					Color rarityColorUI = ItemTool.getRarityColorUI(glassesAsset.rarity);
					PlayerDashboardInventoryUI.headers[9].BackgroundColor = SleekColor.BackgroundIfLight(rarityColorUI);
					PlayerDashboardInventoryUI.headers[9].TextColor = rarityColorUI;
					Color qualityColor = ItemTool.getQualityColor((float)newGlassesQuality / 100f);
					((ISleekImage)PlayerDashboardInventoryUI.headers[9].GetChildAtIndex(1)).TintColor = qualityColor;
					((ISleekLabel)PlayerDashboardInventoryUI.headers[9].GetChildAtIndex(2)).TextColor = qualityColor;
				}
				PlayerDashboardInventoryUI.headers[9].IsVisible = (glassesAsset != null);
				PlayerDashboardInventoryUI.updateBoxAreas();
			}
		}

		// Token: 0x06004349 RID: 17225 RVA: 0x001792B8 File Offset: 0x001774B8
		private static void onClickedHeader(ISleekElement button)
		{
			int num = 0;
			while (num < PlayerDashboardInventoryUI.headers.Length && PlayerDashboardInventoryUI.headers[num] != button)
			{
				num++;
			}
			switch (num)
			{
			case 0:
				if (Player.player.equipment.HasValidUseable && !Player.player.equipment.isBusy && Player.player.equipment.IsEquipAnimationFinished)
				{
					Player.player.equipment.dequip();
				}
				return;
			case 1:
				Player.player.clothing.sendSwapBackpack(byte.MaxValue, byte.MaxValue, byte.MaxValue);
				return;
			case 2:
				Player.player.clothing.sendSwapVest(byte.MaxValue, byte.MaxValue, byte.MaxValue);
				return;
			case 3:
				Player.player.clothing.sendSwapShirt(byte.MaxValue, byte.MaxValue, byte.MaxValue);
				return;
			case 4:
				Player.player.clothing.sendSwapPants(byte.MaxValue, byte.MaxValue, byte.MaxValue);
				return;
			case 5:
				PlayerDashboardUI.close();
				PlayerLifeUI.open();
				return;
			case 6:
				PlayerDashboardUI.close();
				PlayerLifeUI.open();
				return;
			case 7:
				Player.player.clothing.sendSwapHat(byte.MaxValue, byte.MaxValue, byte.MaxValue);
				return;
			case 8:
				Player.player.clothing.sendSwapMask(byte.MaxValue, byte.MaxValue, byte.MaxValue);
				return;
			case 9:
				Player.player.clothing.sendSwapGlasses(byte.MaxValue, byte.MaxValue, byte.MaxValue);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600434A RID: 17226 RVA: 0x00179444 File Offset: 0x00177644
		private static void updatePivot()
		{
			if (PlayerDashboardInventoryUI.dragJar.rot == 0)
			{
				PlayerDashboardInventoryUI.dragPivot.x = PlayerDashboardInventoryUI.dragOffset.x;
				PlayerDashboardInventoryUI.dragPivot.y = PlayerDashboardInventoryUI.dragOffset.y;
				return;
			}
			if (PlayerDashboardInventoryUI.dragJar.rot == 1)
			{
				PlayerDashboardInventoryUI.dragPivot.x = -((float)(PlayerDashboardInventoryUI.dragJar.size_y * 50) + PlayerDashboardInventoryUI.dragOffset.y);
				PlayerDashboardInventoryUI.dragPivot.y = PlayerDashboardInventoryUI.dragOffset.x;
				return;
			}
			if (PlayerDashboardInventoryUI.dragJar.rot == 2)
			{
				PlayerDashboardInventoryUI.dragPivot.x = -((float)(PlayerDashboardInventoryUI.dragJar.size_x * 50) + PlayerDashboardInventoryUI.dragOffset.x);
				PlayerDashboardInventoryUI.dragPivot.y = -((float)(PlayerDashboardInventoryUI.dragJar.size_y * 50) + PlayerDashboardInventoryUI.dragOffset.y);
				return;
			}
			if (PlayerDashboardInventoryUI.dragJar.rot == 3)
			{
				PlayerDashboardInventoryUI.dragPivot.x = PlayerDashboardInventoryUI.dragOffset.y;
				PlayerDashboardInventoryUI.dragPivot.y = -((float)(PlayerDashboardInventoryUI.dragJar.size_x * 50) + PlayerDashboardInventoryUI.dragOffset.x);
			}
		}

		/// <summary>
		/// Move item drag visual to the cursor's position.
		/// </summary>
		// Token: 0x0600434B RID: 17227 RVA: 0x00179568 File Offset: 0x00177768
		private static void refreshDraggedVisualPosition()
		{
			PlayerDashboardInventoryUI.dragItem.PositionOffset_X = (float)((int)PlayerDashboardInventoryUI.dragPivot.x);
			PlayerDashboardInventoryUI.dragItem.PositionOffset_Y = (float)((int)PlayerDashboardInventoryUI.dragPivot.y);
			Vector2 vector = PlayerUI.container.ViewportToNormalizedPosition(InputEx.NormalizedMousePosition);
			PlayerDashboardInventoryUI.dragItem.PositionScale_X = vector.x;
			PlayerDashboardInventoryUI.dragItem.PositionScale_Y = vector.y;
		}

		// Token: 0x0600434C RID: 17228 RVA: 0x001795D4 File Offset: 0x001777D4
		public static void updateDraggedItem()
		{
			if (!PlayerDashboardInventoryUI.active || !PlayerDashboardUI.active || PlayerDashboardInventoryUI.dragFromPage == 255 || PlayerDashboardInventoryUI.dragJar == null || !PlayerDashboardInventoryUI.isDragging)
			{
				return;
			}
			if (InputEx.GetKeyDown(ControlsSettings.rotate))
			{
				ItemJar itemJar = PlayerDashboardInventoryUI.dragJar;
				itemJar.rot += 1;
				ItemJar itemJar2 = PlayerDashboardInventoryUI.dragJar;
				itemJar2.rot %= 4;
				PlayerDashboardInventoryUI.updatePivot();
				PlayerDashboardInventoryUI.dragItem.updateItem(PlayerDashboardInventoryUI.dragJar);
				PlayerDashboardInventoryUI.PlayInventoryAudio(PlayerDashboardInventoryUI.dragJar.GetAsset());
			}
			PlayerDashboardInventoryUI.refreshDraggedVisualPosition();
		}

		// Token: 0x0600434D RID: 17229 RVA: 0x00179664 File Offset: 0x00177864
		private static void createElementForNearbyDrop(InteractableItem interactableItem)
		{
			while (!PlayerDashboardInventoryUI.areaItems.tryAddItem(interactableItem.item))
			{
				if (PlayerDashboardInventoryUI.areaItems.height >= 200)
				{
					return;
				}
				PlayerDashboardInventoryUI.areaItems.resize(PlayerDashboardInventoryUI.areaItems.width, PlayerDashboardInventoryUI.areaItems.height + 1);
			}
			ItemJar item = PlayerDashboardInventoryUI.areaItems.getItem(PlayerDashboardInventoryUI.areaItems.getItemCount() - 1);
			item.interactableItem = interactableItem;
			interactableItem.jar = item;
			byte b = PlayerDashboardInventoryUI.areaItems.height - (item.y + ((item.rot % 2 == 0) ? item.size_y : item.size_x));
			if (b < 3 && PlayerDashboardInventoryUI.areaItems.height + b <= 200)
			{
				PlayerDashboardInventoryUI.areaItems.resize(PlayerDashboardInventoryUI.areaItems.width, PlayerDashboardInventoryUI.areaItems.height + (3 - b));
			}
			SleekItems sleekItems = PlayerDashboardInventoryUI.items[(int)(PlayerInventory.AREA - PlayerInventory.SLOTS)];
			sleekItems.resize(PlayerDashboardInventoryUI.areaItems.width, PlayerDashboardInventoryUI.areaItems.height);
			sleekItems.addItem(item);
		}

		// Token: 0x0600434E RID: 17230 RVA: 0x00179778 File Offset: 0x00177978
		public static void updateNearbyDrops()
		{
			if (!PlayerDashboardInventoryUI.active || PlayerDashboardInventoryUI.pendingItemsInRadius.Count < 1)
			{
				return;
			}
			int height = (int)PlayerDashboardInventoryUI.areaItems.height;
			Vector3 eyesPositionWithoutLeaning = Player.player.look.GetEyesPositionWithoutLeaning();
			int num = Mathf.Max(0, PlayerDashboardInventoryUI.pendingItemsInRadius.Count - 20);
			for (int i = PlayerDashboardInventoryUI.pendingItemsInRadius.Count - 1; i >= num; i--)
			{
				InteractableItem interactableItem = PlayerDashboardInventoryUI.pendingItemsInRadius[i];
				PlayerDashboardInventoryUI.pendingItemsInRadius.RemoveAt(i);
				if (!(interactableItem == null) && interactableItem.item != null)
				{
					Renderer componentInChildren = interactableItem.transform.GetComponentInChildren<Renderer>();
					if (!(componentInChildren == null))
					{
						Vector3 center = componentInChildren.bounds.center;
						RaycastHit raycastHit;
						if (!Physics.Linecast(eyesPositionWithoutLeaning, center, out raycastHit, RayMasks.BLOCK_PICKUP, QueryTriggerInteraction.Ignore))
						{
							PlayerDashboardInventoryUI.createElementForNearbyDrop(interactableItem);
						}
					}
				}
			}
			if ((int)PlayerDashboardInventoryUI.areaItems.height > height)
			{
				PlayerDashboardInventoryUI.updateBoxAreas();
			}
		}

		// Token: 0x0600434F RID: 17231 RVA: 0x00179860 File Offset: 0x00177A60
		private static void PlayInventoryAudio(ItemAsset item)
		{
		}

		// Token: 0x06004350 RID: 17232 RVA: 0x00179864 File Offset: 0x00177A64
		public PlayerDashboardInventoryUI()
		{
			if (PlayerDashboardInventoryUI.icons != null)
			{
				PlayerDashboardInventoryUI.icons.unload();
			}
			PlayerDashboardInventoryUI.pendingItemsInRadius = new List<InteractableItem>();
			PlayerDashboardInventoryUI.localization = Localization.read("/Player/PlayerDashboardInventory.dat");
			PlayerDashboardInventoryUI.icons = Bundles.getBundle("/Bundles/Textures/Player/Icons/PlayerDashboardInventory/PlayerDashboardInventory.unity3d");
			PlayerDashboardInventoryUI._selectedPage = byte.MaxValue;
			PlayerDashboardInventoryUI._selected_x = byte.MaxValue;
			PlayerDashboardInventoryUI._selected_y = byte.MaxValue;
			PlayerDashboardInventoryUI.isDragging = false;
			PlayerDashboardInventoryUI.container = new SleekFullscreenBox();
			PlayerDashboardInventoryUI.container.PositionScale_Y = 1f;
			PlayerDashboardInventoryUI.container.PositionOffset_X = 10f;
			PlayerDashboardInventoryUI.container.PositionOffset_Y = 10f;
			PlayerDashboardInventoryUI.container.SizeOffset_X = -20f;
			PlayerDashboardInventoryUI.container.SizeOffset_Y = -20f;
			PlayerDashboardInventoryUI.container.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.container.SizeScale_Y = 1f;
			PlayerUI.container.AddChild(PlayerDashboardInventoryUI.container);
			PlayerDashboardInventoryUI.active = true;
			PlayerDashboardInventoryUI.backdropBox = Glazier.Get().CreateBox();
			PlayerDashboardInventoryUI.backdropBox.PositionOffset_Y = 60f;
			PlayerDashboardInventoryUI.backdropBox.SizeOffset_Y = -60f;
			PlayerDashboardInventoryUI.backdropBox.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.backdropBox.SizeScale_Y = 1f;
			PlayerDashboardInventoryUI.backdropBox.BackgroundColor = new SleekColor(1, 0.5f);
			PlayerDashboardInventoryUI.container.AddChild(PlayerDashboardInventoryUI.backdropBox);
			PlayerDashboardInventoryUI.characterPlayer = null;
			PlayerDashboardInventoryUI.hasDragOutsideHandlers = Glazier.Get().SupportsDepth;
			if (PlayerDashboardInventoryUI.hasDragOutsideHandlers)
			{
				PlayerDashboardInventoryUI.dragOutsideHandler = Glazier.Get().CreateImage();
				PlayerDashboardInventoryUI.dragOutsideHandler.SizeScale_X = 1f;
				PlayerDashboardInventoryUI.dragOutsideHandler.SizeScale_Y = 1f;
				PlayerDashboardInventoryUI.dragOutsideHandler.OnClicked += new Action(PlayerDashboardInventoryUI.onClickedDuringDrag);
				PlayerDashboardInventoryUI.dragOutsideHandler.OnRightClicked += new Action(PlayerDashboardInventoryUI.onRightClickedDuringDrag);
				PlayerDashboardInventoryUI.dragOutsideHandler.IsVisible = false;
				PlayerDashboardInventoryUI.backdropBox.AddChild(PlayerDashboardInventoryUI.dragOutsideHandler);
			}
			else
			{
				PlayerDashboardInventoryUI.dragOutsideHandler = null;
			}
			ISleekBox sleekBox = Glazier.Get().CreateBox();
			sleekBox.PositionOffset_X = 10f;
			sleekBox.PositionOffset_Y = 70f;
			sleekBox.SizeOffset_X = 410f;
			sleekBox.SizeOffset_Y = -280f;
			sleekBox.SizeScale_Y = 1f;
			PlayerDashboardInventoryUI.backdropBox.AddChild(sleekBox);
			ISleekConstraintFrame sleekConstraintFrame = Glazier.Get().CreateConstraintFrame();
			sleekConstraintFrame.SizeScale_Y = 1f;
			sleekConstraintFrame.Constraint = 1;
			if (Glazier.Get().SupportsDepth)
			{
				sleekConstraintFrame.PositionScale_X = -0.5f;
				sleekConstraintFrame.SizeScale_X = 2f;
			}
			else
			{
				sleekConstraintFrame.PositionScale_X = 0f;
				sleekConstraintFrame.SizeScale_X = 1f;
			}
			sleekBox.AddChild(sleekConstraintFrame);
			PlayerDashboardInventoryUI.characterImage = new SleekCameraImage();
			PlayerDashboardInventoryUI.characterImage.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.characterImage.SizeScale_Y = 1f;
			PlayerDashboardInventoryUI.characterImage.internalImage.OnClicked += new Action(PlayerDashboardInventoryUI.onClickedCharacter);
			PlayerDashboardInventoryUI.characterImage.SetCamera(Player.player.look.characterCamera);
			sleekConstraintFrame.AddChild(PlayerDashboardInventoryUI.characterImage);
			PlayerDashboardInventoryUI.slots = new SleekSlot[(int)PlayerInventory.SLOTS];
			byte b = 0;
			while ((int)b < PlayerDashboardInventoryUI.slots.Length)
			{
				PlayerDashboardInventoryUI.slots[(int)b] = new SleekSlot(b);
				PlayerDashboardInventoryUI.slots[(int)b].onSelectedItem = new SelectedItem(PlayerDashboardInventoryUI.onSelectedItem);
				PlayerDashboardInventoryUI.slots[(int)b].onGrabbedItem = new GrabbedItem(PlayerDashboardInventoryUI.onGrabbedItem);
				PlayerDashboardInventoryUI.slots[(int)b].onPlacedItem = new PlacedItem(PlayerDashboardInventoryUI.onPlacedItem);
				PlayerDashboardInventoryUI.backdropBox.AddChild(PlayerDashboardInventoryUI.slots[(int)b]);
				b += 1;
			}
			PlayerDashboardInventoryUI.slots[0].PositionOffset_X = 10f;
			PlayerDashboardInventoryUI.slots[0].PositionOffset_Y = -160f;
			PlayerDashboardInventoryUI.slots[0].PositionScale_Y = 1f;
			PlayerDashboardInventoryUI.slots[1].PositionOffset_X = 270f;
			PlayerDashboardInventoryUI.slots[1].PositionOffset_Y = -160f;
			PlayerDashboardInventoryUI.slots[1].PositionScale_Y = 1f;
			PlayerDashboardInventoryUI.slots[1].SizeOffset_X = 150f;
			PlayerDashboardInventoryUI.characterSlider = Glazier.Get().CreateSlider();
			PlayerDashboardInventoryUI.characterSlider.SizeOffset_Y = 20f;
			PlayerDashboardInventoryUI.characterSlider.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.characterSlider.SizeOffset_X = -120f;
			PlayerDashboardInventoryUI.characterSlider.PositionOffset_X = 120f;
			PlayerDashboardInventoryUI.characterSlider.PositionOffset_Y = 15f;
			PlayerDashboardInventoryUI.characterSlider.PositionScale_Y = 1f;
			PlayerDashboardInventoryUI.characterSlider.Orientation = 0;
			PlayerDashboardInventoryUI.characterSlider.OnValueChanged += new Dragged(PlayerDashboardInventoryUI.onDraggedCharacterSlider);
			sleekBox.AddChild(PlayerDashboardInventoryUI.characterSlider);
			PlayerDashboardInventoryUI.swapCosmeticsButton = new SleekButtonIcon(PlayerDashboardInventoryUI.icons.load<Texture2D>("Swap_Cosmetics"));
			PlayerDashboardInventoryUI.swapCosmeticsButton.PositionOffset_Y = 10f;
			PlayerDashboardInventoryUI.swapCosmeticsButton.PositionScale_Y = 1f;
			PlayerDashboardInventoryUI.swapCosmeticsButton.SizeOffset_X = 30f;
			PlayerDashboardInventoryUI.swapCosmeticsButton.SizeOffset_Y = 30f;
			PlayerDashboardInventoryUI.swapCosmeticsButton.tooltip = PlayerDashboardInventoryUI.localization.format("Swap_Cosmetics_Tooltip");
			PlayerDashboardInventoryUI.swapCosmeticsButton.iconColor = 2;
			PlayerDashboardInventoryUI.swapCosmeticsButton.onClickedButton += new ClickedButton(PlayerDashboardInventoryUI.onClickedSwapCosmeticsButton);
			sleekBox.AddChild(PlayerDashboardInventoryUI.swapCosmeticsButton);
			PlayerDashboardInventoryUI.swapSkinsButton = new SleekButtonIcon(PlayerDashboardInventoryUI.icons.load<Texture2D>("Swap_Skins"));
			PlayerDashboardInventoryUI.swapSkinsButton.PositionOffset_X = 40f;
			PlayerDashboardInventoryUI.swapSkinsButton.PositionOffset_Y = 10f;
			PlayerDashboardInventoryUI.swapSkinsButton.PositionScale_Y = 1f;
			PlayerDashboardInventoryUI.swapSkinsButton.SizeOffset_X = 30f;
			PlayerDashboardInventoryUI.swapSkinsButton.SizeOffset_Y = 30f;
			PlayerDashboardInventoryUI.swapSkinsButton.tooltip = PlayerDashboardInventoryUI.localization.format("Swap_Skins_Tooltip");
			PlayerDashboardInventoryUI.swapSkinsButton.iconColor = 2;
			PlayerDashboardInventoryUI.swapSkinsButton.onClickedButton += new ClickedButton(PlayerDashboardInventoryUI.onClickedSwapSkinsButton);
			sleekBox.AddChild(PlayerDashboardInventoryUI.swapSkinsButton);
			PlayerDashboardInventoryUI.swapMythicsButton = new SleekButtonIcon(PlayerDashboardInventoryUI.icons.load<Texture2D>("Swap_Mythics"));
			PlayerDashboardInventoryUI.swapMythicsButton.PositionOffset_X = 80f;
			PlayerDashboardInventoryUI.swapMythicsButton.PositionOffset_Y = 10f;
			PlayerDashboardInventoryUI.swapMythicsButton.PositionScale_Y = 1f;
			PlayerDashboardInventoryUI.swapMythicsButton.SizeOffset_X = 30f;
			PlayerDashboardInventoryUI.swapMythicsButton.SizeOffset_Y = 30f;
			PlayerDashboardInventoryUI.swapMythicsButton.tooltip = PlayerDashboardInventoryUI.localization.format("Swap_Mythics_Tooltip");
			PlayerDashboardInventoryUI.swapMythicsButton.iconColor = 2;
			PlayerDashboardInventoryUI.swapMythicsButton.onClickedButton += new ClickedButton(PlayerDashboardInventoryUI.onClickedSwapMythicsButton);
			sleekBox.AddChild(PlayerDashboardInventoryUI.swapMythicsButton);
			PlayerDashboardInventoryUI.box = Glazier.Get().CreateFrame();
			PlayerDashboardInventoryUI.box.PositionOffset_X = 430f;
			PlayerDashboardInventoryUI.box.PositionOffset_Y = 10f;
			PlayerDashboardInventoryUI.box.SizeOffset_X = -440f;
			PlayerDashboardInventoryUI.box.SizeOffset_Y = -20f;
			PlayerDashboardInventoryUI.box.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.box.SizeScale_Y = 1f;
			PlayerDashboardInventoryUI.backdropBox.AddChild(PlayerDashboardInventoryUI.box);
			PlayerDashboardInventoryUI.clothingBox = Glazier.Get().CreateScrollView();
			PlayerDashboardInventoryUI.clothingBox.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.clothingBox.SizeScale_Y = 1f;
			PlayerDashboardInventoryUI.clothingBox.ContentSizeOffset = new Vector2(0f, 1000f);
			PlayerDashboardInventoryUI.clothingBox.ScaleContentToWidth = true;
			PlayerDashboardInventoryUI.box.AddChild(PlayerDashboardInventoryUI.clothingBox);
			PlayerDashboardInventoryUI.areaBox = Glazier.Get().CreateScrollView();
			PlayerDashboardInventoryUI.areaBox.PositionOffset_X = 5f;
			PlayerDashboardInventoryUI.areaBox.PositionScale_X = 0.5f;
			PlayerDashboardInventoryUI.areaBox.SizeOffset_X = -5f;
			PlayerDashboardInventoryUI.areaBox.SizeScale_X = 0.5f;
			PlayerDashboardInventoryUI.areaBox.SizeScale_Y = 1f;
			PlayerDashboardInventoryUI.areaBox.ContentSizeOffset = new Vector2(0f, 1000f);
			PlayerDashboardInventoryUI.areaBox.ScaleContentToWidth = true;
			PlayerDashboardInventoryUI.box.AddChild(PlayerDashboardInventoryUI.areaBox);
			if (PlayerDashboardInventoryUI.hasDragOutsideHandlers)
			{
				PlayerDashboardInventoryUI.dragOutsideClothingHandler = Glazier.Get().CreateImage();
				PlayerDashboardInventoryUI.dragOutsideClothingHandler.SizeScale_X = 1f;
				PlayerDashboardInventoryUI.dragOutsideClothingHandler.SizeScale_Y = 1f;
				PlayerDashboardInventoryUI.dragOutsideClothingHandler.OnClicked += new Action(PlayerDashboardInventoryUI.onClickedDuringDrag);
				PlayerDashboardInventoryUI.dragOutsideClothingHandler.OnRightClicked += new Action(PlayerDashboardInventoryUI.onRightClickedDuringDrag);
				PlayerDashboardInventoryUI.dragOutsideClothingHandler.IsVisible = false;
				PlayerDashboardInventoryUI.clothingBox.AddChild(PlayerDashboardInventoryUI.dragOutsideClothingHandler);
				PlayerDashboardInventoryUI.dragOutsideAreaHandler = Glazier.Get().CreateImage();
				PlayerDashboardInventoryUI.dragOutsideAreaHandler.SizeScale_X = 1f;
				PlayerDashboardInventoryUI.dragOutsideAreaHandler.SizeScale_Y = 1f;
				PlayerDashboardInventoryUI.dragOutsideAreaHandler.OnClicked += new Action(PlayerDashboardInventoryUI.onClickedDuringDrag);
				PlayerDashboardInventoryUI.dragOutsideAreaHandler.OnRightClicked += new Action(PlayerDashboardInventoryUI.onRightClickedDuringDrag);
				PlayerDashboardInventoryUI.dragOutsideAreaHandler.IsVisible = false;
				PlayerDashboardInventoryUI.areaBox.AddChild(PlayerDashboardInventoryUI.dragOutsideAreaHandler);
			}
			else
			{
				PlayerDashboardInventoryUI.dragOutsideClothingHandler = null;
				PlayerDashboardInventoryUI.dragOutsideAreaHandler = null;
			}
			PlayerDashboardInventoryUI.headers = new ISleekButton[(int)(PlayerInventory.PAGES - PlayerInventory.SLOTS + 3)];
			byte b2 = 0;
			while ((int)b2 < PlayerDashboardInventoryUI.headers.Length)
			{
				PlayerDashboardInventoryUI.headers[(int)b2] = Glazier.Get().CreateButton();
				PlayerDashboardInventoryUI.headers[(int)b2].SizeOffset_Y = 60f;
				PlayerDashboardInventoryUI.headers[(int)b2].SizeScale_X = 1f;
				PlayerDashboardInventoryUI.headers[(int)b2].FontSize = 3;
				PlayerDashboardInventoryUI.headers[(int)b2].OnClicked += new ClickedButton(PlayerDashboardInventoryUI.onClickedHeader);
				PlayerDashboardInventoryUI.headers[(int)b2].TextContrastContext = 1;
				PlayerDashboardInventoryUI.clothingBox.AddChild(PlayerDashboardInventoryUI.headers[(int)b2]);
				PlayerDashboardInventoryUI.headers[(int)b2].IsVisible = false;
				b2 += 1;
			}
			PlayerDashboardInventoryUI.headers[0].IsVisible = true;
			PlayerDashboardInventoryUI.headers[(int)(PlayerInventory.AREA - PlayerInventory.SLOTS)].IsVisible = true;
			PlayerDashboardInventoryUI.headerItemIcons = new SleekItemIcon[PlayerDashboardInventoryUI.headers.Length];
			byte b3 = 1;
			while ((int)b3 < PlayerDashboardInventoryUI.headers.Length)
			{
				if (b3 != PlayerInventory.STORAGE - PlayerInventory.SLOTS && b3 != PlayerInventory.AREA - PlayerInventory.SLOTS)
				{
					SleekItemIcon sleekItemIcon = new SleekItemIcon();
					sleekItemIcon.PositionOffset_X = 5f;
					sleekItemIcon.PositionScale_Y = 0.5f;
					PlayerDashboardInventoryUI.headerItemIcons[(int)b3] = sleekItemIcon;
					PlayerDashboardInventoryUI.headers[(int)b3].AddChild(sleekItemIcon);
					ISleekImage sleekImage = Glazier.Get().CreateImage();
					sleekImage.PositionOffset_X = -25f;
					sleekImage.PositionOffset_Y = -25f;
					sleekImage.PositionScale_X = 1f;
					sleekImage.PositionScale_Y = 1f;
					sleekImage.SizeOffset_X = 20f;
					sleekImage.SizeOffset_Y = 20f;
					sleekImage.Texture = PlayerDashboardInventoryUI.icons.load<Texture2D>("Quality_0");
					PlayerDashboardInventoryUI.headers[(int)b3].AddChild(sleekImage);
					ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
					sleekLabel.PositionOffset_X = -105f;
					sleekLabel.PositionOffset_Y = 5f;
					sleekLabel.PositionScale_X = 1f;
					sleekLabel.SizeOffset_X = 100f;
					sleekLabel.SizeOffset_Y = -10f;
					sleekLabel.SizeScale_Y = 1f;
					sleekLabel.TextAlignment = 2;
					sleekLabel.TextContrastContext = 1;
					PlayerDashboardInventoryUI.headers[(int)b3].AddChild(sleekLabel);
				}
				b3 += 1;
			}
			PlayerDashboardInventoryUI.headers[0].Text = PlayerDashboardInventoryUI.localization.format("Hands");
			PlayerDashboardInventoryUI.headers[(int)(PlayerInventory.AREA - PlayerInventory.SLOTS)].Text = PlayerDashboardInventoryUI.localization.format("Area");
			PlayerDashboardInventoryUI.onShirtUpdated(0, Player.player.clothing.shirtQuality, Player.player.clothing.shirtState);
			PlayerDashboardInventoryUI.onPantsUpdated(0, Player.player.clothing.pantsQuality, Player.player.clothing.pantsState);
			PlayerDashboardInventoryUI.onBackpackUpdated(0, Player.player.clothing.backpackQuality, Player.player.clothing.backpackState);
			PlayerDashboardInventoryUI.onVestUpdated(0, Player.player.clothing.vestQuality, Player.player.clothing.vestState);
			PlayerDashboardInventoryUI.items = new SleekItems[(int)(PlayerInventory.PAGES - PlayerInventory.SLOTS)];
			byte b4 = 0;
			while ((int)b4 < PlayerDashboardInventoryUI.items.Length)
			{
				PlayerDashboardInventoryUI.items[(int)b4] = new SleekItems(PlayerInventory.SLOTS + b4);
				PlayerDashboardInventoryUI.items[(int)b4].onSelectedItem = new SelectedItem(PlayerDashboardInventoryUI.onSelectedItem);
				PlayerDashboardInventoryUI.items[(int)b4].onGrabbedItem = new GrabbedItem(PlayerDashboardInventoryUI.onGrabbedItem);
				PlayerDashboardInventoryUI.items[(int)b4].onPlacedItem = new PlacedItem(PlayerDashboardInventoryUI.onPlacedItem);
				PlayerDashboardInventoryUI.clothingBox.AddChild(PlayerDashboardInventoryUI.items[(int)b4]);
				b4 += 1;
			}
			PlayerDashboardInventoryUI.areaItems = new Items(PlayerInventory.AREA);
			PlayerDashboardInventoryUI.actions = new List<Action>();
			PlayerDashboardInventoryUI.selectionFrame = Glazier.Get().CreateFrame();
			PlayerDashboardInventoryUI.selectionFrame.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.selectionFrame.SizeScale_Y = 1f;
			PlayerDashboardInventoryUI.selectionFrame.IsVisible = false;
			PlayerUI.container.AddChild(PlayerDashboardInventoryUI.selectionFrame);
			if (PlayerDashboardInventoryUI.hasDragOutsideHandlers)
			{
				PlayerDashboardInventoryUI.outsideSelectionInvisibleButton = Glazier.Get().CreateImage();
				PlayerDashboardInventoryUI.outsideSelectionInvisibleButton.SizeScale_X = 1f;
				PlayerDashboardInventoryUI.outsideSelectionInvisibleButton.SizeScale_Y = 1f;
				PlayerDashboardInventoryUI.outsideSelectionInvisibleButton.OnClicked += new Action(PlayerDashboardInventoryUI.onClickedOutsideSelection);
				PlayerDashboardInventoryUI.outsideSelectionInvisibleButton.OnRightClicked += new Action(PlayerDashboardInventoryUI.onClickedOutsideSelection);
				PlayerDashboardInventoryUI.outsideSelectionInvisibleButton.Texture = GlazierResources.PixelTexture;
				PlayerDashboardInventoryUI.outsideSelectionInvisibleButton.TintColor = new Color(0f, 0f, 0f, 0.5f);
				PlayerDashboardInventoryUI.selectionFrame.AddChild(PlayerDashboardInventoryUI.outsideSelectionInvisibleButton);
			}
			else
			{
				PlayerDashboardInventoryUI.outsideSelectionInvisibleButton = null;
			}
			PlayerDashboardInventoryUI.selectionBackdropBox = Glazier.Get().CreateBox();
			PlayerDashboardInventoryUI.selectionBackdropBox.SizeOffset_X = 530f;
			PlayerDashboardInventoryUI.selectionBackdropBox.SizeOffset_Y = 440f;
			PlayerDashboardInventoryUI.selectionFrame.AddChild(PlayerDashboardInventoryUI.selectionBackdropBox);
			PlayerDashboardInventoryUI.selectionIconBox = Glazier.Get().CreateBox();
			PlayerDashboardInventoryUI.selectionIconBox.PositionOffset_X = 10f;
			PlayerDashboardInventoryUI.selectionIconBox.PositionOffset_Y = 10f;
			PlayerDashboardInventoryUI.selectionIconBox.SizeOffset_X = 510f;
			PlayerDashboardInventoryUI.selectionIconBox.SizeOffset_Y = 310f;
			PlayerDashboardInventoryUI.selectionBackdropBox.AddChild(PlayerDashboardInventoryUI.selectionIconBox);
			PlayerDashboardInventoryUI.selectionIconImage = new SleekItemIcon();
			PlayerDashboardInventoryUI.selectionIconImage.PositionScale_X = 0.5f;
			PlayerDashboardInventoryUI.selectionIconImage.PositionScale_Y = 0.5f;
			PlayerDashboardInventoryUI.selectionIconBox.AddChild(PlayerDashboardInventoryUI.selectionIconImage);
			if (Glazier.Get().SupportsAutomaticLayout)
			{
				PlayerDashboardInventoryUI.selectionDescriptionScrollView = Glazier.Get().CreateScrollView();
				PlayerDashboardInventoryUI.selectionDescriptionScrollView.PositionOffset_X = 10f;
				PlayerDashboardInventoryUI.selectionDescriptionScrollView.PositionOffset_Y = 330f;
				PlayerDashboardInventoryUI.selectionDescriptionScrollView.SizeOffset_X = 250f;
				PlayerDashboardInventoryUI.selectionDescriptionScrollView.SizeOffset_Y = 100f;
				PlayerDashboardInventoryUI.selectionDescriptionScrollView.ScaleContentToWidth = true;
				PlayerDashboardInventoryUI.selectionDescriptionScrollView.ContentUseManualLayout = false;
				PlayerDashboardInventoryUI.selectionBackdropBox.AddChild(PlayerDashboardInventoryUI.selectionDescriptionScrollView);
				PlayerDashboardInventoryUI.selectionDescriptionLabel = Glazier.Get().CreateLabel();
				PlayerDashboardInventoryUI.selectionDescriptionLabel.UseManualLayout = false;
				PlayerDashboardInventoryUI.selectionDescriptionLabel.AllowRichText = true;
				PlayerDashboardInventoryUI.selectionDescriptionLabel.TextAlignment = 0;
				PlayerDashboardInventoryUI.selectionDescriptionLabel.TextColor = 4;
				PlayerDashboardInventoryUI.selectionDescriptionLabel.TextContrastContext = 1;
				PlayerDashboardInventoryUI.selectionDescriptionScrollView.AddChild(PlayerDashboardInventoryUI.selectionDescriptionLabel);
			}
			else
			{
				PlayerDashboardInventoryUI.selectionDescriptionBox = Glazier.Get().CreateBox();
				PlayerDashboardInventoryUI.selectionDescriptionBox.PositionOffset_X = 10f;
				PlayerDashboardInventoryUI.selectionDescriptionBox.PositionOffset_Y = 330f;
				PlayerDashboardInventoryUI.selectionDescriptionBox.SizeOffset_X = 250f;
				PlayerDashboardInventoryUI.selectionDescriptionBox.SizeOffset_Y = 100f;
				PlayerDashboardInventoryUI.selectionBackdropBox.AddChild(PlayerDashboardInventoryUI.selectionDescriptionBox);
				PlayerDashboardInventoryUI.selectionDescriptionLabel = Glazier.Get().CreateLabel();
				PlayerDashboardInventoryUI.selectionDescriptionLabel.AllowRichText = true;
				PlayerDashboardInventoryUI.selectionDescriptionLabel.PositionOffset_X = 5f;
				PlayerDashboardInventoryUI.selectionDescriptionLabel.PositionOffset_Y = 5f;
				PlayerDashboardInventoryUI.selectionDescriptionLabel.SizeOffset_X = -10f;
				PlayerDashboardInventoryUI.selectionDescriptionLabel.SizeOffset_Y = -10f;
				PlayerDashboardInventoryUI.selectionDescriptionLabel.SizeScale_X = 1f;
				PlayerDashboardInventoryUI.selectionDescriptionLabel.SizeScale_Y = 1f;
				PlayerDashboardInventoryUI.selectionDescriptionLabel.TextAlignment = 0;
				PlayerDashboardInventoryUI.selectionDescriptionLabel.TextColor = 4;
				PlayerDashboardInventoryUI.selectionDescriptionLabel.TextContrastContext = 1;
				PlayerDashboardInventoryUI.selectionDescriptionBox.AddChild(PlayerDashboardInventoryUI.selectionDescriptionLabel);
			}
			PlayerDashboardInventoryUI.selectionNameLabel = Glazier.Get().CreateLabel();
			PlayerDashboardInventoryUI.selectionNameLabel.PositionOffset_Y = -70f;
			PlayerDashboardInventoryUI.selectionNameLabel.PositionScale_Y = 1f;
			PlayerDashboardInventoryUI.selectionNameLabel.SizeOffset_Y = 70f;
			PlayerDashboardInventoryUI.selectionNameLabel.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.selectionNameLabel.FontSize = 4;
			PlayerDashboardInventoryUI.selectionNameLabel.TextContrastContext = 1;
			PlayerDashboardInventoryUI.selectionIconBox.AddChild(PlayerDashboardInventoryUI.selectionNameLabel);
			PlayerDashboardInventoryUI.selectionHotkeyLabel = Glazier.Get().CreateLabel();
			PlayerDashboardInventoryUI.selectionHotkeyLabel.PositionOffset_X = 5f;
			PlayerDashboardInventoryUI.selectionHotkeyLabel.PositionOffset_Y = 5f;
			PlayerDashboardInventoryUI.selectionHotkeyLabel.SizeOffset_X = -10f;
			PlayerDashboardInventoryUI.selectionHotkeyLabel.SizeOffset_Y = 30f;
			PlayerDashboardInventoryUI.selectionHotkeyLabel.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.selectionHotkeyLabel.TextAlignment = 2;
			PlayerDashboardInventoryUI.selectionIconBox.AddChild(PlayerDashboardInventoryUI.selectionHotkeyLabel);
			PlayerDashboardInventoryUI.selectionActionsBox = Glazier.Get().CreateScrollView();
			PlayerDashboardInventoryUI.selectionActionsBox.PositionOffset_X = 270f;
			PlayerDashboardInventoryUI.selectionActionsBox.PositionOffset_Y = 330f;
			PlayerDashboardInventoryUI.selectionActionsBox.SizeOffset_X = -280f;
			PlayerDashboardInventoryUI.selectionActionsBox.SizeOffset_Y = 100f;
			PlayerDashboardInventoryUI.selectionActionsBox.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.selectionActionsBox.ScaleContentToWidth = true;
			PlayerDashboardInventoryUI.selectionBackdropBox.AddChild(PlayerDashboardInventoryUI.selectionActionsBox);
			PlayerDashboardInventoryUI.selectionEquipButton = Glazier.Get().CreateButton();
			PlayerDashboardInventoryUI.selectionEquipButton.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.selectionEquipButton.SizeOffset_Y = 30f;
			PlayerDashboardInventoryUI.selectionEquipButton.OnClicked += new ClickedButton(PlayerDashboardInventoryUI.onClickedEquip);
			PlayerDashboardInventoryUI.selectionActionsBox.AddChild(PlayerDashboardInventoryUI.selectionEquipButton);
			PlayerDashboardInventoryUI.selectionContextButton = Glazier.Get().CreateButton();
			PlayerDashboardInventoryUI.selectionContextButton.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.selectionContextButton.SizeOffset_Y = 30f;
			PlayerDashboardInventoryUI.selectionContextButton.OnClicked += new ClickedButton(PlayerDashboardInventoryUI.onClickedContext);
			PlayerDashboardInventoryUI.selectionActionsBox.AddChild(PlayerDashboardInventoryUI.selectionContextButton);
			PlayerDashboardInventoryUI.selectionDropButton = Glazier.Get().CreateButton();
			PlayerDashboardInventoryUI.selectionDropButton.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.selectionDropButton.SizeOffset_Y = 30f;
			PlayerDashboardInventoryUI.selectionDropButton.OnClicked += new ClickedButton(PlayerDashboardInventoryUI.onClickedDrop);
			PlayerDashboardInventoryUI.selectionActionsBox.AddChild(PlayerDashboardInventoryUI.selectionDropButton);
			PlayerDashboardInventoryUI.selectionStorageButton = Glazier.Get().CreateButton();
			PlayerDashboardInventoryUI.selectionStorageButton.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.selectionStorageButton.SizeOffset_Y = 30f;
			PlayerDashboardInventoryUI.selectionStorageButton.OnClicked += new ClickedButton(PlayerDashboardInventoryUI.onClickedStore);
			PlayerDashboardInventoryUI.selectionActionsBox.AddChild(PlayerDashboardInventoryUI.selectionStorageButton);
			PlayerDashboardInventoryUI.selectionExtraActionsBox = Glazier.Get().CreateFrame();
			PlayerDashboardInventoryUI.selectionExtraActionsBox.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.selectionActionsBox.AddChild(PlayerDashboardInventoryUI.selectionExtraActionsBox);
			PlayerDashboardInventoryUI.vehicleBox = Glazier.Get().CreateBox();
			PlayerDashboardInventoryUI.vehicleBox.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.clothingBox.AddChild(PlayerDashboardInventoryUI.vehicleBox);
			PlayerDashboardInventoryUI.vehicleNameLabel = Glazier.Get().CreateLabel();
			PlayerDashboardInventoryUI.vehicleNameLabel.SizeOffset_Y = 60f;
			PlayerDashboardInventoryUI.vehicleNameLabel.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.vehicleNameLabel.FontSize = 3;
			PlayerDashboardInventoryUI.vehicleNameLabel.TextContrastContext = 1;
			PlayerDashboardInventoryUI.vehicleBox.AddChild(PlayerDashboardInventoryUI.vehicleNameLabel);
			PlayerDashboardInventoryUI.vehicleActionsBox = Glazier.Get().CreateFrame();
			PlayerDashboardInventoryUI.vehicleActionsBox.PositionOffset_X = 10f;
			PlayerDashboardInventoryUI.vehicleActionsBox.PositionOffset_Y = 60f;
			PlayerDashboardInventoryUI.vehicleActionsBox.SizeOffset_X = 250f;
			PlayerDashboardInventoryUI.vehicleBox.AddChild(PlayerDashboardInventoryUI.vehicleActionsBox);
			PlayerDashboardInventoryUI.vehicleLockButton = Glazier.Get().CreateButton();
			PlayerDashboardInventoryUI.vehicleLockButton.SizeOffset_Y = 30f;
			PlayerDashboardInventoryUI.vehicleLockButton.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.vehicleLockButton.OnClicked += new ClickedButton(PlayerDashboardInventoryUI.onClickedVehicleLockButton);
			PlayerDashboardInventoryUI.vehicleActionsBox.AddChild(PlayerDashboardInventoryUI.vehicleLockButton);
			PlayerDashboardInventoryUI.vehicleLockButton.IsVisible = false;
			PlayerDashboardInventoryUI.vehicleHornButton = Glazier.Get().CreateButton();
			PlayerDashboardInventoryUI.vehicleHornButton.SizeOffset_Y = 30f;
			PlayerDashboardInventoryUI.vehicleHornButton.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.vehicleHornButton.OnClicked += new ClickedButton(PlayerDashboardInventoryUI.onClickedVehicleHornButton);
			PlayerDashboardInventoryUI.vehicleActionsBox.AddChild(PlayerDashboardInventoryUI.vehicleHornButton);
			PlayerDashboardInventoryUI.vehicleHornButton.IsVisible = false;
			PlayerDashboardInventoryUI.vehicleHeadlightsButton = Glazier.Get().CreateButton();
			PlayerDashboardInventoryUI.vehicleHeadlightsButton.SizeOffset_Y = 30f;
			PlayerDashboardInventoryUI.vehicleHeadlightsButton.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.vehicleHeadlightsButton.OnClicked += new ClickedButton(PlayerDashboardInventoryUI.onClickedVehicleHeadlightsButton);
			PlayerDashboardInventoryUI.vehicleActionsBox.AddChild(PlayerDashboardInventoryUI.vehicleHeadlightsButton);
			PlayerDashboardInventoryUI.vehicleHeadlightsButton.IsVisible = false;
			PlayerDashboardInventoryUI.vehicleSirensButton = Glazier.Get().CreateButton();
			PlayerDashboardInventoryUI.vehicleSirensButton.SizeOffset_Y = 30f;
			PlayerDashboardInventoryUI.vehicleSirensButton.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.vehicleSirensButton.OnClicked += new ClickedButton(PlayerDashboardInventoryUI.onClickedVehicleSirensButton);
			PlayerDashboardInventoryUI.vehicleActionsBox.AddChild(PlayerDashboardInventoryUI.vehicleSirensButton);
			PlayerDashboardInventoryUI.vehicleSirensButton.IsVisible = false;
			PlayerDashboardInventoryUI.vehicleBlimpButton = Glazier.Get().CreateButton();
			PlayerDashboardInventoryUI.vehicleBlimpButton.SizeOffset_Y = 30f;
			PlayerDashboardInventoryUI.vehicleBlimpButton.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.vehicleBlimpButton.OnClicked += new ClickedButton(PlayerDashboardInventoryUI.onClickedVehicleBlimpButton);
			PlayerDashboardInventoryUI.vehicleActionsBox.AddChild(PlayerDashboardInventoryUI.vehicleBlimpButton);
			PlayerDashboardInventoryUI.vehicleBlimpButton.IsVisible = false;
			PlayerDashboardInventoryUI.vehicleHookButton = Glazier.Get().CreateButton();
			PlayerDashboardInventoryUI.vehicleHookButton.SizeOffset_Y = 30f;
			PlayerDashboardInventoryUI.vehicleHookButton.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.vehicleHookButton.OnClicked += new ClickedButton(PlayerDashboardInventoryUI.onClickedVehicleHookButton);
			PlayerDashboardInventoryUI.vehicleActionsBox.AddChild(PlayerDashboardInventoryUI.vehicleHookButton);
			PlayerDashboardInventoryUI.vehicleHookButton.IsVisible = false;
			PlayerDashboardInventoryUI.vehicleStealBatteryButton = Glazier.Get().CreateButton();
			PlayerDashboardInventoryUI.vehicleStealBatteryButton.SizeOffset_Y = 30f;
			PlayerDashboardInventoryUI.vehicleStealBatteryButton.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.vehicleStealBatteryButton.OnClicked += new ClickedButton(PlayerDashboardInventoryUI.onClickedVehicleStealBatteryButton);
			PlayerDashboardInventoryUI.vehicleActionsBox.AddChild(PlayerDashboardInventoryUI.vehicleStealBatteryButton);
			PlayerDashboardInventoryUI.vehicleStealBatteryButton.IsVisible = false;
			PlayerDashboardInventoryUI.vehicleSkinButton = Glazier.Get().CreateButton();
			PlayerDashboardInventoryUI.vehicleSkinButton.SizeOffset_Y = 30f;
			PlayerDashboardInventoryUI.vehicleSkinButton.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.vehicleSkinButton.OnClicked += new ClickedButton(PlayerDashboardInventoryUI.onClickedVehicleSkinButton);
			PlayerDashboardInventoryUI.vehicleActionsBox.AddChild(PlayerDashboardInventoryUI.vehicleSkinButton);
			PlayerDashboardInventoryUI.vehicleSkinButton.IsVisible = false;
			PlayerDashboardInventoryUI.vehiclePassengersBox = Glazier.Get().CreateFrame();
			PlayerDashboardInventoryUI.vehiclePassengersBox.PositionOffset_Y = 60f;
			PlayerDashboardInventoryUI.vehiclePassengersBox.SizeScale_X = 1f;
			PlayerDashboardInventoryUI.vehicleBox.AddChild(PlayerDashboardInventoryUI.vehiclePassengersBox);
			PlayerDashboardInventoryUI.rot_xButton = Glazier.Get().CreateButton();
			PlayerDashboardInventoryUI.rot_xButton.PositionScale_X = 1f;
			PlayerDashboardInventoryUI.rot_xButton.SizeOffset_X = 60f;
			PlayerDashboardInventoryUI.rot_xButton.SizeOffset_Y = 60f;
			PlayerDashboardInventoryUI.rot_xButton.OnClicked += new ClickedButton(PlayerDashboardInventoryUI.onClickedRot_XButton);
			PlayerDashboardInventoryUI.rot_xButton.Text = PlayerDashboardInventoryUI.localization.format("Rot_X");
			PlayerDashboardInventoryUI.headers[(int)(PlayerInventory.STORAGE - PlayerInventory.SLOTS)].AddChild(PlayerDashboardInventoryUI.rot_xButton);
			PlayerDashboardInventoryUI.rot_xButton.IsVisible = false;
			PlayerDashboardInventoryUI.rot_yButton = Glazier.Get().CreateButton();
			PlayerDashboardInventoryUI.rot_yButton.PositionScale_X = 1f;
			PlayerDashboardInventoryUI.rot_yButton.PositionOffset_X = 60f;
			PlayerDashboardInventoryUI.rot_yButton.SizeOffset_X = 60f;
			PlayerDashboardInventoryUI.rot_yButton.SizeOffset_Y = 60f;
			PlayerDashboardInventoryUI.rot_yButton.OnClicked += new ClickedButton(PlayerDashboardInventoryUI.onClickedRot_YButton);
			PlayerDashboardInventoryUI.rot_yButton.Text = PlayerDashboardInventoryUI.localization.format("Rot_Y");
			PlayerDashboardInventoryUI.headers[(int)(PlayerInventory.STORAGE - PlayerInventory.SLOTS)].AddChild(PlayerDashboardInventoryUI.rot_yButton);
			PlayerDashboardInventoryUI.rot_yButton.IsVisible = false;
			PlayerDashboardInventoryUI.rot_zButton = Glazier.Get().CreateButton();
			PlayerDashboardInventoryUI.rot_zButton.PositionScale_X = 1f;
			PlayerDashboardInventoryUI.rot_zButton.PositionOffset_X = 120f;
			PlayerDashboardInventoryUI.rot_zButton.SizeOffset_X = 60f;
			PlayerDashboardInventoryUI.rot_zButton.SizeOffset_Y = 60f;
			PlayerDashboardInventoryUI.rot_zButton.OnClicked += new ClickedButton(PlayerDashboardInventoryUI.onClickedRot_ZButton);
			PlayerDashboardInventoryUI.rot_zButton.Text = PlayerDashboardInventoryUI.localization.format("Rot_Z");
			PlayerDashboardInventoryUI.headers[(int)(PlayerInventory.STORAGE - PlayerInventory.SLOTS)].AddChild(PlayerDashboardInventoryUI.rot_zButton);
			PlayerDashboardInventoryUI.rot_zButton.IsVisible = false;
			PlayerDashboardInventoryUI.dragItem = new SleekItem();
			PlayerUI.container.AddChild(PlayerDashboardInventoryUI.dragItem);
			PlayerDashboardInventoryUI.dragItem.IsVisible = false;
			PlayerDashboardInventoryUI.dragItem.SetIsDragItem();
			PlayerDashboardInventoryUI.dragOffset = Vector2.zero;
			PlayerDashboardInventoryUI.dragPivot = Vector2.zero;
			PlayerDashboardInventoryUI.dragFromPage = byte.MaxValue;
			PlayerDashboardInventoryUI.dragFrom_x = byte.MaxValue;
			PlayerDashboardInventoryUI.dragFrom_y = byte.MaxValue;
			PlayerDashboardInventoryUI.dragFromRot = 0;
			PlayerInventory inventory = Player.player.inventory;
			inventory.onInventoryResized = (InventoryResized)Delegate.Combine(inventory.onInventoryResized, new InventoryResized(PlayerDashboardInventoryUI.onInventoryResized));
			PlayerInventory inventory2 = Player.player.inventory;
			inventory2.onInventoryUpdated = (InventoryUpdated)Delegate.Combine(inventory2.onInventoryUpdated, new InventoryUpdated(PlayerDashboardInventoryUI.onInventoryUpdated));
			PlayerInventory inventory3 = Player.player.inventory;
			inventory3.onInventoryAdded = (InventoryAdded)Delegate.Combine(inventory3.onInventoryAdded, new InventoryAdded(PlayerDashboardInventoryUI.onInventoryAdded));
			PlayerInventory inventory4 = Player.player.inventory;
			inventory4.onInventoryRemoved = (InventoryRemoved)Delegate.Combine(inventory4.onInventoryRemoved, new InventoryRemoved(PlayerDashboardInventoryUI.onInventoryRemoved));
			PlayerInventory inventory5 = Player.player.inventory;
			inventory5.onInventoryStored = (InventoryStored)Delegate.Combine(inventory5.onInventoryStored, new InventoryStored(PlayerDashboardInventoryUI.onInventoryStored));
			PlayerEquipment equipment = Player.player.equipment;
			equipment.onHotkeysUpdated = (HotkeysUpdated)Delegate.Combine(equipment.onHotkeysUpdated, new HotkeysUpdated(PlayerDashboardInventoryUI.onHotkeysUpdated));
			ItemManager.onItemDropAdded = new ItemDropAdded(PlayerDashboardInventoryUI.onItemDropAdded);
			ItemManager.onItemDropRemoved = new ItemDropRemoved(PlayerDashboardInventoryUI.onItemDropRemoved);
			PlayerMovement movement = Player.player.movement;
			movement.onSeated = (Seated)Delegate.Combine(movement.onSeated, new Seated(PlayerDashboardInventoryUI.onSeated));
			PlayerClothing clothing = Player.player.clothing;
			clothing.onShirtUpdated = (ShirtUpdated)Delegate.Combine(clothing.onShirtUpdated, new ShirtUpdated(PlayerDashboardInventoryUI.onShirtUpdated));
			PlayerClothing clothing2 = Player.player.clothing;
			clothing2.onPantsUpdated = (PantsUpdated)Delegate.Combine(clothing2.onPantsUpdated, new PantsUpdated(PlayerDashboardInventoryUI.onPantsUpdated));
			PlayerClothing clothing3 = Player.player.clothing;
			clothing3.onHatUpdated = (HatUpdated)Delegate.Combine(clothing3.onHatUpdated, new HatUpdated(PlayerDashboardInventoryUI.onHatUpdated));
			PlayerClothing clothing4 = Player.player.clothing;
			clothing4.onBackpackUpdated = (BackpackUpdated)Delegate.Combine(clothing4.onBackpackUpdated, new BackpackUpdated(PlayerDashboardInventoryUI.onBackpackUpdated));
			PlayerClothing clothing5 = Player.player.clothing;
			clothing5.onVestUpdated = (VestUpdated)Delegate.Combine(clothing5.onVestUpdated, new VestUpdated(PlayerDashboardInventoryUI.onVestUpdated));
			PlayerClothing clothing6 = Player.player.clothing;
			clothing6.onMaskUpdated = (MaskUpdated)Delegate.Combine(clothing6.onMaskUpdated, new MaskUpdated(PlayerDashboardInventoryUI.onMaskUpdated));
			PlayerClothing clothing7 = Player.player.clothing;
			clothing7.onGlassesUpdated = (GlassesUpdated)Delegate.Combine(clothing7.onGlassesUpdated, new GlassesUpdated(PlayerDashboardInventoryUI.onGlassesUpdated));
		}

		// Token: 0x06004351 RID: 17233 RVA: 0x0017B458 File Offset: 0x00179658
		internal static string FormatStatColor(string text, bool isBeneficial)
		{
			Color32 color = isBeneficial ? OptionsSettings.fontColor : OptionsSettings.badColor;
			return string.Concat(new string[]
			{
				"<color=",
				Palette.hex(color),
				">",
				text,
				"</color>"
			});
		}

		// Token: 0x06004352 RID: 17234 RVA: 0x0017B4AC File Offset: 0x001796AC
		internal static string FormatStatModifier(float modifier, bool higherIsPositive, bool higherIsBeneficial)
		{
			char c = higherIsPositive ? ((modifier > 1f) ? '+' : '-') : ((modifier > 1f) ? '-' : '+');
			bool isBeneficial = higherIsBeneficial ? (modifier > 1f) : (modifier < 1f);
			float num = (modifier > 1f) ? (modifier - 1f) : (1f - modifier);
			return PlayerDashboardInventoryUI.FormatStatColor(string.Format("{0}{1:P}", c, num), isBeneficial);
		}

		// Token: 0x04002C43 RID: 11331
		private static List<InteractableItem> pendingItemsInRadius;

		// Token: 0x04002C44 RID: 11332
		private static SleekFullscreenBox container;

		// Token: 0x04002C45 RID: 11333
		public static Local localization;

		// Token: 0x04002C46 RID: 11334
		public static Bundle icons;

		// Token: 0x04002C47 RID: 11335
		public static bool active;

		// Token: 0x04002C48 RID: 11336
		private static ISleekBox backdropBox;

		/// <summary>
		/// Added during the UI refactor to catch unhandled mouse clicks during drag.
		/// </summary>
		// Token: 0x04002C49 RID: 11337
		private static ISleekImage dragOutsideHandler;

		// Token: 0x04002C4A RID: 11338
		private static ISleekImage dragOutsideClothingHandler;

		// Token: 0x04002C4B RID: 11339
		private static ISleekImage dragOutsideAreaHandler;

		// Token: 0x04002C4C RID: 11340
		private static bool hasDragOutsideHandlers;

		// Token: 0x04002C4E RID: 11342
		private static ItemJar dragJar;

		// Token: 0x04002C4F RID: 11343
		private static SleekItem dragSource;

		// Token: 0x04002C50 RID: 11344
		private static SleekItem dragItem;

		// Token: 0x04002C51 RID: 11345
		private static Vector2 dragOffset;

		// Token: 0x04002C52 RID: 11346
		private static Vector2 dragPivot;

		// Token: 0x04002C53 RID: 11347
		private static byte dragFromPage;

		// Token: 0x04002C54 RID: 11348
		private static byte dragFrom_x;

		// Token: 0x04002C55 RID: 11349
		private static byte dragFrom_y;

		// Token: 0x04002C56 RID: 11350
		private static byte dragFromRot;

		// Token: 0x04002C57 RID: 11351
		private static SleekCameraImage characterImage;

		// Token: 0x04002C58 RID: 11352
		private static ISleekSlider characterSlider;

		// Token: 0x04002C59 RID: 11353
		private static SleekButtonIcon swapCosmeticsButton;

		// Token: 0x04002C5A RID: 11354
		private static SleekButtonIcon swapSkinsButton;

		// Token: 0x04002C5B RID: 11355
		private static SleekButtonIcon swapMythicsButton;

		// Token: 0x04002C5C RID: 11356
		private static SleekPlayer characterPlayer;

		// Token: 0x04002C5D RID: 11357
		private static SleekSlot[] slots;

		// Token: 0x04002C5E RID: 11358
		private static ISleekElement box;

		// Token: 0x04002C5F RID: 11359
		private static ISleekScrollView clothingBox;

		// Token: 0x04002C60 RID: 11360
		private static ISleekScrollView areaBox;

		// Token: 0x04002C61 RID: 11361
		private static ISleekButton[] headers;

		// Token: 0x04002C62 RID: 11362
		private static SleekItemIcon[] headerItemIcons;

		// Token: 0x04002C63 RID: 11363
		private static SleekItems[] items;

		/// <summary>
		/// Contains inspect item box and invisible button.
		/// </summary>
		// Token: 0x04002C64 RID: 11364
		private static ISleekElement selectionFrame;

		/// <summary>
		/// Added during the UI refactor to catch mouse clicks outside the selection box.
		/// </summary>
		// Token: 0x04002C65 RID: 11365
		private static ISleekImage outsideSelectionInvisibleButton;

		// Token: 0x04002C66 RID: 11366
		private static ISleekBox selectionBackdropBox;

		// Token: 0x04002C67 RID: 11367
		private static ISleekBox selectionIconBox;

		// Token: 0x04002C68 RID: 11368
		private static SleekItemIcon selectionIconImage;

		// Token: 0x04002C69 RID: 11369
		private static ISleekScrollView selectionDescriptionScrollView;

		// Token: 0x04002C6A RID: 11370
		private static ISleekBox selectionDescriptionBox;

		// Token: 0x04002C6B RID: 11371
		private static ISleekLabel selectionDescriptionLabel;

		// Token: 0x04002C6C RID: 11372
		private static ISleekLabel selectionNameLabel;

		// Token: 0x04002C6D RID: 11373
		private static ISleekLabel selectionHotkeyLabel;

		// Token: 0x04002C6E RID: 11374
		private static ISleekBox vehicleBox;

		// Token: 0x04002C6F RID: 11375
		private static ISleekLabel vehicleNameLabel;

		// Token: 0x04002C70 RID: 11376
		private static ISleekElement vehicleActionsBox;

		// Token: 0x04002C71 RID: 11377
		private static ISleekElement vehiclePassengersBox;

		// Token: 0x04002C72 RID: 11378
		private static ISleekButton vehicleLockButton;

		// Token: 0x04002C73 RID: 11379
		private static ISleekButton vehicleHornButton;

		// Token: 0x04002C74 RID: 11380
		private static ISleekButton vehicleHeadlightsButton;

		// Token: 0x04002C75 RID: 11381
		private static ISleekButton vehicleSirensButton;

		// Token: 0x04002C76 RID: 11382
		private static ISleekButton vehicleBlimpButton;

		// Token: 0x04002C77 RID: 11383
		private static ISleekButton vehicleHookButton;

		// Token: 0x04002C78 RID: 11384
		private static ISleekButton vehicleStealBatteryButton;

		// Token: 0x04002C79 RID: 11385
		private static ISleekButton vehicleSkinButton;

		// Token: 0x04002C7A RID: 11386
		private static ISleekScrollView selectionActionsBox;

		// Token: 0x04002C7B RID: 11387
		private static ISleekButton selectionEquipButton;

		// Token: 0x04002C7C RID: 11388
		private static ISleekButton selectionContextButton;

		// Token: 0x04002C7D RID: 11389
		private static ISleekButton selectionDropButton;

		// Token: 0x04002C7E RID: 11390
		private static ISleekButton selectionStorageButton;

		// Token: 0x04002C7F RID: 11391
		private static ISleekElement selectionExtraActionsBox;

		// Token: 0x04002C80 RID: 11392
		private static ISleekButton rot_xButton;

		// Token: 0x04002C81 RID: 11393
		private static ISleekButton rot_yButton;

		// Token: 0x04002C82 RID: 11394
		private static ISleekButton rot_zButton;

		// Token: 0x04002C83 RID: 11395
		private static byte _selectedPage;

		// Token: 0x04002C84 RID: 11396
		private static byte _selected_x;

		// Token: 0x04002C85 RID: 11397
		private static byte _selected_y;

		// Token: 0x04002C86 RID: 11398
		private static ItemJar _selectedJar;

		// Token: 0x04002C87 RID: 11399
		private static ItemAsset _selectedAsset;

		// Token: 0x04002C88 RID: 11400
		private static Items areaItems;

		// Token: 0x04002C89 RID: 11401
		private static List<Action> actions;

		// Token: 0x04002C8A RID: 11402
		private static OncePerFrameGuard eventGuard;
	}
}
