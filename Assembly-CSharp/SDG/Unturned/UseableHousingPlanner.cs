using System;
using System.Collections.Generic;
using SDG.NetTransport;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007E6 RID: 2022
	public class UseableHousingPlanner : Useable
	{
		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x0600455B RID: 17755 RVA: 0x0019B955 File Offset: 0x00199B55
		public override bool isUseableShowingMenu
		{
			get
			{
				return this.isItemSelectionMenuOpen;
			}
		}

		// Token: 0x0600455C RID: 17756 RVA: 0x0019B960 File Offset: 0x00199B60
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceivePlaceHousingItemResult(bool success)
		{
			if (success)
			{
				OneShotAudioDefinition oneShotAudioDefinition = UseableHousingPlanner.popupAudioRef.loadAsset(true);
				if (oneShotAudioDefinition == null)
				{
					UnturnedLog.warn("Missing built-in housing planner success audio");
					return;
				}
				base.player.playSound(oneShotAudioDefinition.GetRandomClip(), 0.5f * oneShotAudioDefinition.volumeMultiplier, Random.Range(oneShotAudioDefinition.minPitch, oneShotAudioDefinition.maxPitch), 0f);
				return;
			}
			else
			{
				AudioClip audioClip = UseableHousingPlanner.errorClipRef.loadAsset(true);
				if (audioClip == null)
				{
					UnturnedLog.warn("Missing built-in housing planner error audio");
					return;
				}
				base.player.playSound(audioClip, 0.5f, 1f, 0.025f);
				return;
			}
		}

		// Token: 0x0600455D RID: 17757 RVA: 0x0019BA04 File Offset: 0x00199C04
		private bool ReceivePlaceHousingItemInternal(in ServerInvocationContext context, Guid assetGuid, Vector3 position, float yaw)
		{
			if ((position - base.player.look.aim.position).sqrMagnitude > 256f)
			{
				return false;
			}
			if (!UseableHousingUtils.IsPendingPositionValid(base.player, position))
			{
				return false;
			}
			ItemStructureAsset itemStructureAsset = Assets.find(assetGuid) as ItemStructureAsset;
			if (itemStructureAsset == null)
			{
				return false;
			}
			InventorySearch inventorySearch = base.player.inventory.has(itemStructureAsset.id);
			if (inventorySearch == null)
			{
				return false;
			}
			string empty = string.Empty;
			if (UseableHousingUtils.ValidatePendingPlacement(itemStructureAsset, ref position, yaw, ref empty) != EHousingPlacementResult.Success)
			{
				return false;
			}
			bool flag = StructureManager.dropStructure(new Structure(itemStructureAsset, itemStructureAsset.health), position, 0f, yaw, 0f, base.channel.owner.playerID.steamID.m_SteamID, base.player.quests.groupID.m_SteamID);
			if (flag)
			{
				base.player.sendStat(EPlayerStat.FOUND_BUILDABLES);
				inventorySearch.deleteAmount(base.player, 1U);
			}
			return flag;
		}

		// Token: 0x0600455E RID: 17758 RVA: 0x0019BAFC File Offset: 0x00199CFC
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 5)]
		public void ReceivePlaceHousingItem(in ServerInvocationContext context, Guid assetGuid, Vector3 position, float yaw)
		{
			bool arg = this.ReceivePlaceHousingItemInternal(context, assetGuid, position, yaw);
			UseableHousingPlanner.SendPlaceHousingItemResult.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GetOwnerTransportConnection(), arg);
		}

		// Token: 0x0600455F RID: 17759 RVA: 0x0019BB34 File Offset: 0x00199D34
		public override bool startPrimary()
		{
			if (base.player.equipment.isBusy)
			{
				return false;
			}
			if (base.channel.IsLocalPlayer && this.selectedAsset != null && this.UpdatePendingPlacement())
			{
				UseableHousingPlanner.SendPlaceHousingItem.Invoke(base.GetNetId(), ENetReliability.Reliable, this.selectedAsset.GUID, this.pendingPlacementPosition, this.pendingPlacementYaw + this.customRotationOffset);
				return true;
			}
			return false;
		}

		// Token: 0x06004560 RID: 17760 RVA: 0x0019BBA4 File Offset: 0x00199DA4
		public override bool startSecondary()
		{
			if (!base.channel.IsLocalPlayer || this.selectedAsset == null)
			{
				return false;
			}
			if (this.selectedAsset.construct == EConstruct.FLOOR_POLY || this.selectedAsset.construct == EConstruct.ROOF_POLY)
			{
				return false;
			}
			float num;
			if (this.selectedAsset.construct == EConstruct.FLOOR || this.selectedAsset.construct == EConstruct.ROOF)
			{
				num = 90f;
			}
			else if (this.selectedAsset.construct == EConstruct.RAMPART || this.selectedAsset.construct == EConstruct.WALL)
			{
				num = 180f;
			}
			else
			{
				num = 30f;
			}
			if (InputEx.GetKey(KeyCode.LeftShift))
			{
				num *= -1f;
			}
			this.customRotationOffset += num;
			return true;
		}

		// Token: 0x06004561 RID: 17761 RVA: 0x0019BC5C File Offset: 0x00199E5C
		public override void equip()
		{
			base.player.animator.play("Equip", true);
			if (base.channel.IsLocalPlayer)
			{
				this.itemSearch = new List<InventorySearch>();
				this.floors = new List<InventorySearch>();
				this.roofs = new List<InventorySearch>();
				this.walls = new List<InventorySearch>();
				this.pillars = new List<InventorySearch>();
				this.itemAmounts = new Dictionary<ushort, int>();
				this.selectedItemBox = Glazier.Get().CreateBox();
				this.selectedItemBox.PositionOffset_Y = -50f;
				this.selectedItemBox.PositionScale_X = 0.7f;
				this.selectedItemBox.PositionScale_Y = 1f;
				this.selectedItemBox.SizeOffset_Y = 50f;
				this.selectedItemBox.SizeScale_X = 0.3f;
				this.selectedItemBox.IsVisible = false;
				PlayerLifeUI.container.AddChild(this.selectedItemBox);
				this.selectedItemNameLabel = Glazier.Get().CreateLabel();
				this.selectedItemNameLabel.PositionOffset_X = 10f;
				this.selectedItemNameLabel.SizeScale_X = 1f;
				this.selectedItemNameLabel.SizeScale_Y = 1f;
				this.selectedItemNameLabel.SizeOffset_X = -20f;
				this.selectedItemNameLabel.TextAlignment = 5;
				this.selectedItemNameLabel.FontSize = 4;
				this.selectedItemBox.AddChild(this.selectedItemNameLabel);
				this.selectedItemNameLabel.TextContrastContext = 1;
				this.selectedItemQuantityLabel = Glazier.Get().CreateLabel();
				this.selectedItemQuantityLabel.PositionOffset_X = 10f;
				this.selectedItemQuantityLabel.SizeScale_X = 1f;
				this.selectedItemQuantityLabel.SizeScale_Y = 1f;
				this.selectedItemQuantityLabel.SizeOffset_X = -20f;
				this.selectedItemQuantityLabel.TextAlignment = 3;
				this.selectedItemQuantityLabel.FontSize = 4;
				this.selectedItemBox.AddChild(this.selectedItemQuantityLabel);
				this.selectedItemQuantityLabel.TextContrastContext = 1;
				Local local = Localization.read("/Player/Useable/PlayerUseableHousingPlanner.dat");
				Bundle bundle = Bundles.getBundle("/Bundles/Textures/Player/Icons/Useable/PlayerUseableHousingPlanner/PlayerUseableHousingPlanner.unity3d");
				Texture texture = bundle.load<Texture>("RadialMenu");
				bundle.unload();
				this.itemSelectionContainer = Glazier.Get().CreateFrame();
				this.itemSelectionContainer.SizeScale_X = 1f;
				this.itemSelectionContainer.SizeScale_Y = 1f;
				this.itemSelectionContainer.IsVisible = false;
				PlayerUI.container.AddChild(this.itemSelectionContainer);
				ISleekImage sleekImage = Glazier.Get().CreateImage(texture);
				sleekImage.PositionScale_X = 0.5f;
				sleekImage.PositionScale_Y = 0.5f;
				sleekImage.PositionOffset_X = 50f;
				sleekImage.PositionOffset_Y = -306f;
				sleekImage.SizeOffset_X = 256f;
				sleekImage.SizeOffset_Y = 256f;
				sleekImage.TintColor = SleekColor.BackgroundIfLight(new Color(0f, 0f, 0f, 0.2f));
				this.itemSelectionContainer.AddChild(sleekImage);
				this.floorsLabel = Glazier.Get().CreateLabel();
				this.floorsLabel.PositionScale_X = 0.5f;
				this.floorsLabel.PositionScale_Y = 0.5f;
				this.floorsLabel.PositionOffset_X = 50f;
				this.floorsLabel.PositionOffset_Y = -306f;
				this.floorsLabel.SizeOffset_X = 256f;
				this.floorsLabel.SizeOffset_Y = 256f;
				this.floorsLabel.FontSize = 4;
				this.floorsLabel.Text = local.format("Floors");
				this.itemSelectionContainer.AddChild(this.floorsLabel);
				this.floorsLabel.TextContrastContext = 2;
				this.noFloorItemsLabel = Glazier.Get().CreateLabel();
				this.noFloorItemsLabel.PositionScale_X = 0.5f;
				this.noFloorItemsLabel.PositionScale_Y = 0.5f;
				this.noFloorItemsLabel.PositionOffset_X = 50f;
				this.noFloorItemsLabel.PositionOffset_Y = -286f;
				this.noFloorItemsLabel.SizeOffset_X = 256f;
				this.noFloorItemsLabel.SizeOffset_Y = 256f;
				this.noFloorItemsLabel.FontSize = 3;
				this.noFloorItemsLabel.TextColor = 6;
				this.noFloorItemsLabel.Text = local.format("NoItems");
				this.noFloorItemsLabel.IsVisible = false;
				this.itemSelectionContainer.AddChild(this.noFloorItemsLabel);
				this.noFloorItemsLabel.TextContrastContext = 2;
				ISleekImage sleekImage2 = Glazier.Get().CreateImage(texture);
				sleekImage2.PositionScale_X = 0.5f;
				sleekImage2.PositionScale_Y = 0.5f;
				sleekImage2.PositionOffset_X = 50f;
				sleekImage2.PositionOffset_Y = 50f;
				sleekImage2.SizeOffset_X = 256f;
				sleekImage2.SizeOffset_Y = 256f;
				sleekImage2.TintColor = SleekColor.BackgroundIfLight(new Color(0f, 0f, 0f, 0.2f));
				this.itemSelectionContainer.AddChild(sleekImage2);
				this.roofsLabel = Glazier.Get().CreateLabel();
				this.roofsLabel.PositionScale_X = 0.5f;
				this.roofsLabel.PositionScale_Y = 0.5f;
				this.roofsLabel.PositionOffset_X = 50f;
				this.roofsLabel.PositionOffset_Y = 50f;
				this.roofsLabel.SizeOffset_X = 256f;
				this.roofsLabel.SizeOffset_Y = 256f;
				this.roofsLabel.FontSize = 4;
				this.roofsLabel.Text = local.format("Roofs");
				this.itemSelectionContainer.AddChild(this.roofsLabel);
				this.roofsLabel.TextContrastContext = 2;
				this.noRoofItemsLabel = Glazier.Get().CreateLabel();
				this.noRoofItemsLabel.PositionScale_X = 0.5f;
				this.noRoofItemsLabel.PositionScale_Y = 0.5f;
				this.noRoofItemsLabel.PositionOffset_X = 50f;
				this.noRoofItemsLabel.PositionOffset_Y = 70f;
				this.noRoofItemsLabel.SizeOffset_X = 256f;
				this.noRoofItemsLabel.SizeOffset_Y = 256f;
				this.noRoofItemsLabel.FontSize = 3;
				this.noRoofItemsLabel.TextColor = 6;
				this.noRoofItemsLabel.Text = local.format("NoItems");
				this.noRoofItemsLabel.IsVisible = false;
				this.itemSelectionContainer.AddChild(this.noRoofItemsLabel);
				this.noRoofItemsLabel.TextContrastContext = 2;
				ISleekImage sleekImage3 = Glazier.Get().CreateImage(texture);
				sleekImage3.PositionScale_X = 0.5f;
				sleekImage3.PositionScale_Y = 0.5f;
				sleekImage3.PositionOffset_X = -306f;
				sleekImage3.PositionOffset_Y = -306f;
				sleekImage3.SizeOffset_X = 256f;
				sleekImage3.SizeOffset_Y = 256f;
				sleekImage3.TintColor = SleekColor.BackgroundIfLight(new Color(0f, 0f, 0f, 0.2f));
				this.itemSelectionContainer.AddChild(sleekImage3);
				this.wallsLabel = Glazier.Get().CreateLabel();
				this.wallsLabel.PositionScale_X = 0.5f;
				this.wallsLabel.PositionScale_Y = 0.5f;
				this.wallsLabel.PositionOffset_X = -306f;
				this.wallsLabel.PositionOffset_Y = -306f;
				this.wallsLabel.SizeOffset_X = 256f;
				this.wallsLabel.SizeOffset_Y = 256f;
				this.wallsLabel.FontSize = 4;
				this.wallsLabel.Text = local.format("Walls");
				this.itemSelectionContainer.AddChild(this.wallsLabel);
				this.wallsLabel.TextContrastContext = 2;
				this.noWallItemsLabel = Glazier.Get().CreateLabel();
				this.noWallItemsLabel.PositionScale_X = 0.5f;
				this.noWallItemsLabel.PositionScale_Y = 0.5f;
				this.noWallItemsLabel.PositionOffset_X = -306f;
				this.noWallItemsLabel.PositionOffset_Y = -286f;
				this.noWallItemsLabel.SizeOffset_X = 256f;
				this.noWallItemsLabel.SizeOffset_Y = 256f;
				this.noWallItemsLabel.FontSize = 3;
				this.noWallItemsLabel.TextColor = 6;
				this.noWallItemsLabel.Text = local.format("NoItems");
				this.noWallItemsLabel.IsVisible = false;
				this.itemSelectionContainer.AddChild(this.noWallItemsLabel);
				this.noWallItemsLabel.TextContrastContext = 2;
				ISleekImage sleekImage4 = Glazier.Get().CreateImage(texture);
				sleekImage4.PositionScale_X = 0.5f;
				sleekImage4.PositionScale_Y = 0.5f;
				sleekImage4.PositionOffset_X = -306f;
				sleekImage4.PositionOffset_Y = 50f;
				sleekImage4.SizeOffset_X = 256f;
				sleekImage4.SizeOffset_Y = 256f;
				sleekImage4.TintColor = SleekColor.BackgroundIfLight(new Color(0f, 0f, 0f, 0.2f));
				this.itemSelectionContainer.AddChild(sleekImage4);
				this.pillarsLabel = Glazier.Get().CreateLabel();
				this.pillarsLabel.PositionScale_X = 0.5f;
				this.pillarsLabel.PositionScale_Y = 0.5f;
				this.pillarsLabel.PositionOffset_X = -306f;
				this.pillarsLabel.PositionOffset_Y = 50f;
				this.pillarsLabel.SizeOffset_X = 256f;
				this.pillarsLabel.SizeOffset_Y = 256f;
				this.pillarsLabel.FontSize = 4;
				this.pillarsLabel.Text = local.format("Pillars");
				this.itemSelectionContainer.AddChild(this.pillarsLabel);
				this.pillarsLabel.TextContrastContext = 2;
				this.noPillarItemsLabel = Glazier.Get().CreateLabel();
				this.noPillarItemsLabel.PositionScale_X = 0.5f;
				this.noPillarItemsLabel.PositionScale_Y = 0.5f;
				this.noPillarItemsLabel.PositionOffset_X = -306f;
				this.noPillarItemsLabel.PositionOffset_Y = 70f;
				this.noPillarItemsLabel.SizeOffset_X = 256f;
				this.noPillarItemsLabel.SizeOffset_Y = 256f;
				this.noPillarItemsLabel.FontSize = 3;
				this.noPillarItemsLabel.TextColor = 6;
				this.noPillarItemsLabel.Text = local.format("NoItems");
				this.noPillarItemsLabel.IsVisible = false;
				this.itemSelectionContainer.AddChild(this.noPillarItemsLabel);
				this.noPillarItemsLabel.TextContrastContext = 2;
				PlayerUI.message(EPlayerMessage.HOUSING_PLANNER_TUTORIAL, "", 2f);
			}
		}

		// Token: 0x06004562 RID: 17762 RVA: 0x0019C6D0 File Offset: 0x0019A8D0
		public override void dequip()
		{
			if (base.channel.IsLocalPlayer)
			{
				this.SetItemSelectionMenuOpen(false);
				this.SetSelectedAsset(null);
				PlayerUI.container.RemoveChild(this.selectedItemBox);
				PlayerUI.container.RemoveChild(this.itemSelectionContainer);
			}
		}

		// Token: 0x06004563 RID: 17763 RVA: 0x0019C710 File Offset: 0x0019A910
		public override void tick()
		{
			if (base.channel.IsLocalPlayer)
			{
				if (base.player.inventory.doesSearchNeedRefresh(ref this.cachedSearchIndex))
				{
					this.RefreshAvailableItems();
				}
				if (InputEx.GetKeyUp(ControlsSettings.attach))
				{
					this.SetItemSelectionMenuOpen(false);
				}
				else if (!PlayerUI.window.showCursor && InputEx.ConsumeKeyDown(ControlsSettings.attach))
				{
					this.SetItemSelectionMenuOpen(true);
				}
				if (this.placementPreviewTransform != null)
				{
					bool flag = this.UpdatePendingPlacement();
					if (this.isPlacementPreviewValid != flag)
					{
						this.isPlacementPreviewValid = flag;
						HighlighterTool.help(this.placementPreviewTransform, this.isPlacementPreviewValid);
					}
					float num = Glazier.Get().ShouldGameProcessInput ? Input.GetAxis("mouse_z") : 0f;
					this.foundationPositionOffset = Mathf.Clamp(this.foundationPositionOffset + num * 0.05f, -1f, 1f);
					this.animatedRotationOffset = Mathf.Lerp(this.animatedRotationOffset, this.customRotationOffset, 8f * Time.deltaTime);
					this.placementPreviewTransform.position = this.pendingPlacementPosition;
					this.placementPreviewTransform.rotation = Quaternion.Euler(-90f, this.pendingPlacementYaw + this.animatedRotationOffset, 0f);
				}
			}
		}

		// Token: 0x06004564 RID: 17764 RVA: 0x0019C854 File Offset: 0x0019AA54
		private void SetItemSelectionMenuOpen(bool isOpen)
		{
			if (this.isItemSelectionMenuOpen == isOpen)
			{
				return;
			}
			this.isItemSelectionMenuOpen = isOpen;
			PlayerUI.isLocked = isOpen;
			if (this.floorsMenu != null)
			{
				this.itemSelectionContainer.RemoveChild(this.floorsMenu);
				this.floorsMenu = null;
			}
			if (this.roofsMenu != null)
			{
				this.itemSelectionContainer.RemoveChild(this.roofsMenu);
				this.roofsMenu = null;
			}
			if (this.wallsMenu != null)
			{
				this.itemSelectionContainer.RemoveChild(this.wallsMenu);
				this.wallsMenu = null;
			}
			if (this.pillarsMenu != null)
			{
				this.itemSelectionContainer.RemoveChild(this.pillarsMenu);
				this.pillarsMenu = null;
			}
			if (isOpen)
			{
				PlayerLifeUI.close();
			}
			else
			{
				PlayerLifeUI.open();
			}
			this.itemSelectionContainer.IsVisible = isOpen;
			if (isOpen)
			{
				this.floors.Clear();
				this.roofs.Clear();
				this.walls.Clear();
				this.pillars.Clear();
				foreach (InventorySearch inventorySearch in this.itemSearch)
				{
					switch (inventorySearch.GetAsset<ItemStructureAsset>().construct)
					{
					case EConstruct.FLOOR:
					case EConstruct.FLOOR_POLY:
						this.floors.Add(inventorySearch);
						break;
					case EConstruct.WALL:
					case EConstruct.RAMPART:
						this.walls.Add(inventorySearch);
						break;
					case EConstruct.ROOF:
					case EConstruct.ROOF_POLY:
						this.roofs.Add(inventorySearch);
						break;
					case EConstruct.PILLAR:
					case EConstruct.POST:
						this.pillars.Add(inventorySearch);
						break;
					}
				}
				this.floors.Sort(new Comparison<InventorySearch>(this.CompareItemNames));
				this.roofs.Sort(new Comparison<InventorySearch>(this.CompareItemNames));
				this.walls.Sort(new Comparison<InventorySearch>(this.CompareItemNames));
				this.pillars.Sort(new Comparison<InventorySearch>(this.CompareItemNames));
				this.noFloorItemsLabel.IsVisible = (this.floors.Count < 1);
				this.noRoofItemsLabel.IsVisible = (this.roofs.Count < 1);
				this.noWallItemsLabel.IsVisible = (this.walls.Count < 1);
				this.noPillarItemsLabel.IsVisible = (this.pillars.Count < 1);
				this.floorsMenu = new SleekJars(128f, this.floors, 2.3561945f);
				this.floorsMenu.PositionScale_X = 0.5f;
				this.floorsMenu.PositionScale_Y = 0.5f;
				this.floorsMenu.PositionOffset_X = 50f;
				this.floorsMenu.PositionOffset_Y = -306f;
				this.floorsMenu.SizeOffset_X = 256f;
				this.floorsMenu.SizeOffset_Y = 256f;
				SleekJars sleekJars = this.floorsMenu;
				sleekJars.onClickedJar = (ClickedJar)Delegate.Combine(sleekJars.onClickedJar, new ClickedJar(this.OnSelectedFloorItem));
				this.itemSelectionContainer.AddChild(this.floorsMenu);
				this.roofsMenu = new SleekJars(128f, this.roofs, 3.926991f);
				this.roofsMenu.PositionScale_X = 0.5f;
				this.roofsMenu.PositionScale_Y = 0.5f;
				this.roofsMenu.PositionOffset_X = 50f;
				this.roofsMenu.PositionOffset_Y = 50f;
				this.roofsMenu.SizeOffset_X = 256f;
				this.roofsMenu.SizeOffset_Y = 256f;
				SleekJars sleekJars2 = this.roofsMenu;
				sleekJars2.onClickedJar = (ClickedJar)Delegate.Combine(sleekJars2.onClickedJar, new ClickedJar(this.OnSelectedRoofItem));
				this.itemSelectionContainer.AddChild(this.roofsMenu);
				this.wallsMenu = new SleekJars(128f, this.walls, 0.7853982f);
				this.wallsMenu.PositionScale_X = 0.5f;
				this.wallsMenu.PositionScale_Y = 0.5f;
				this.wallsMenu.PositionOffset_X = -306f;
				this.wallsMenu.PositionOffset_Y = -306f;
				this.wallsMenu.SizeOffset_X = 256f;
				this.wallsMenu.SizeOffset_Y = 256f;
				SleekJars sleekJars3 = this.wallsMenu;
				sleekJars3.onClickedJar = (ClickedJar)Delegate.Combine(sleekJars3.onClickedJar, new ClickedJar(this.OnSelectedWallItem));
				this.itemSelectionContainer.AddChild(this.wallsMenu);
				this.pillarsMenu = new SleekJars(128f, this.pillars, 5.4977875f);
				this.pillarsMenu.PositionScale_X = 0.5f;
				this.pillarsMenu.PositionScale_Y = 0.5f;
				this.pillarsMenu.PositionOffset_X = -306f;
				this.pillarsMenu.PositionOffset_Y = 50f;
				this.pillarsMenu.SizeOffset_X = 256f;
				this.pillarsMenu.SizeOffset_Y = 256f;
				SleekJars sleekJars4 = this.pillarsMenu;
				sleekJars4.onClickedJar = (ClickedJar)Delegate.Combine(sleekJars4.onClickedJar, new ClickedJar(this.OnSelectedPillarItem));
				this.itemSelectionContainer.AddChild(this.pillarsMenu);
			}
		}

		// Token: 0x06004565 RID: 17765 RVA: 0x0019CD7C File Offset: 0x0019AF7C
		private void OnSelectedFloorItem(SleekJars jars, int index)
		{
			ItemStructureAsset asset = this.floors[index].GetAsset<ItemStructureAsset>();
			this.SetSelectedAsset(asset);
		}

		// Token: 0x06004566 RID: 17766 RVA: 0x0019CDA4 File Offset: 0x0019AFA4
		private void OnSelectedRoofItem(SleekJars jars, int index)
		{
			ItemStructureAsset asset = this.roofs[index].GetAsset<ItemStructureAsset>();
			this.SetSelectedAsset(asset);
		}

		// Token: 0x06004567 RID: 17767 RVA: 0x0019CDCC File Offset: 0x0019AFCC
		private void OnSelectedWallItem(SleekJars jars, int index)
		{
			ItemStructureAsset asset = this.walls[index].GetAsset<ItemStructureAsset>();
			this.SetSelectedAsset(asset);
		}

		// Token: 0x06004568 RID: 17768 RVA: 0x0019CDF4 File Offset: 0x0019AFF4
		private void OnSelectedPillarItem(SleekJars jars, int index)
		{
			ItemStructureAsset asset = this.pillars[index].GetAsset<ItemStructureAsset>();
			this.SetSelectedAsset(asset);
		}

		// Token: 0x06004569 RID: 17769 RVA: 0x0019CE1C File Offset: 0x0019B01C
		private void SetSelectedAsset(ItemStructureAsset selectedAsset)
		{
			this.selectedAsset = selectedAsset;
			if (this.placementPreviewTransform != null)
			{
				Object.Destroy(this.placementPreviewTransform.gameObject);
				this.placementPreviewTransform = null;
			}
			this.isPlacementPreviewValid = false;
			this.foundationPositionOffset = 0f;
			this.customRotationOffset = 0f;
			this.animatedRotationOffset = 0f;
			if (selectedAsset != null)
			{
				this.placementPreviewTransform = UseableHousingUtils.InstantiatePlacementPreview(selectedAsset);
				this.selectedItemNameLabel.Text = selectedAsset.itemName;
				this.selectedItemNameLabel.TextColor = ItemTool.getRarityColorUI(selectedAsset.rarity);
				int num;
				if (this.itemAmounts.TryGetValue(selectedAsset.id, ref num))
				{
					this.selectedItemQuantityLabel.Text = "x" + num.ToString();
					this.selectedItemQuantityLabel.IsVisible = true;
				}
				else
				{
					this.selectedItemQuantityLabel.IsVisible = false;
				}
			}
			this.selectedItemBox.IsVisible = (selectedAsset != null);
		}

		// Token: 0x0600456A RID: 17770 RVA: 0x0019CF18 File Offset: 0x0019B118
		private bool UpdatePendingPlacement()
		{
			return UseableHousingUtils.FindPlacement(this.selectedAsset, base.player, this.customRotationOffset, this.foundationPositionOffset, out this.pendingPlacementPosition, out this.pendingPlacementYaw) && UseableHousingUtils.IsPendingPositionValid(base.player, this.pendingPlacementPosition);
		}

		/// <summary>
		/// Search inventory for housing items, count the quantity of each, and remove
		/// duplicate entries from the list because it is used for the UI.
		/// </summary>
		// Token: 0x0600456B RID: 17771 RVA: 0x0019CF68 File Offset: 0x0019B168
		private void RefreshAvailableItems()
		{
			this.itemSearch.Clear();
			this.itemAmounts.Clear();
			base.player.inventory.search(this.itemSearch, EItemType.STRUCTURE);
			for (int i = this.itemSearch.Count - 1; i >= 0; i--)
			{
				InventorySearch inventorySearch = this.itemSearch[i];
				int num;
				if (this.itemAmounts.TryGetValue(inventorySearch.jar.item.id, ref num))
				{
					this.itemSearch.RemoveAtFast(i);
				}
				this.itemAmounts[inventorySearch.jar.item.id] = num + (int)inventorySearch.jar.item.amount;
			}
			if (this.selectedAsset != null)
			{
				int num2;
				if (this.itemAmounts.TryGetValue(this.selectedAsset.id, ref num2))
				{
					this.selectedItemQuantityLabel.Text = "x" + num2.ToString();
					return;
				}
				this.SetSelectedAsset(null);
			}
		}

		// Token: 0x0600456C RID: 17772 RVA: 0x0019D068 File Offset: 0x0019B268
		private int CompareItemNames(InventorySearch lhs, InventorySearch rhs)
		{
			ItemAsset asset = lhs.GetAsset();
			ItemAsset asset2 = rhs.GetAsset();
			if (asset != null && asset2 != null)
			{
				return asset.itemName.CompareTo(asset2.itemName);
			}
			return 0;
		}

		// Token: 0x04002EB5 RID: 11957
		private static MasterBundleReference<OneShotAudioDefinition> popupAudioRef = new MasterBundleReference<OneShotAudioDefinition>("core.masterbundle", "Sounds/Popup/Popup.asset");

		// Token: 0x04002EB6 RID: 11958
		private static MasterBundleReference<AudioClip> errorClipRef = new MasterBundleReference<AudioClip>("core.masterbundle", "Sounds/Error.wav");

		// Token: 0x04002EB7 RID: 11959
		private static readonly ClientInstanceMethod<bool> SendPlaceHousingItemResult = ClientInstanceMethod<bool>.Get(typeof(UseableHousingPlanner), "ReceivePlaceHousingItemResult");

		// Token: 0x04002EB8 RID: 11960
		private static readonly ServerInstanceMethod<Guid, Vector3, float> SendPlaceHousingItem = ServerInstanceMethod<Guid, Vector3, float>.Get(typeof(UseableHousingPlanner), "ReceivePlaceHousingItem");

		/// <summary>
		/// Stripped-down version of structure prefab for previewing where the structure will be spawned.
		/// </summary>
		// Token: 0x04002EB9 RID: 11961
		private Transform placementPreviewTransform;

		/// <summary>
		/// Whether preview object is currently highlighted positively.
		/// </summary>
		// Token: 0x04002EBA RID: 11962
		private bool isPlacementPreviewValid;

		/// <summary>
		/// Position the item should be spawned at.
		/// </summary>
		// Token: 0x04002EBB RID: 11963
		private Vector3 pendingPlacementPosition;

		/// <summary>
		/// Rotation the item should be spawned at.
		/// </summary>
		// Token: 0x04002EBC RID: 11964
		private float pendingPlacementYaw;

		/// <summary>
		/// Interpolated toward customRotationOffset.
		/// </summary>
		// Token: 0x04002EBD RID: 11965
		private float animatedRotationOffset;

		/// <summary>
		/// Allows players to flip walls.
		/// </summary>
		// Token: 0x04002EBE RID: 11966
		private float customRotationOffset;

		/// <summary>
		/// Vertical offset using scroll wheel.
		/// </summary>
		// Token: 0x04002EBF RID: 11967
		private float foundationPositionOffset;

		// Token: 0x04002EC0 RID: 11968
		private ItemStructureAsset selectedAsset;

		// Token: 0x04002EC1 RID: 11969
		private bool isItemSelectionMenuOpen;

		// Token: 0x04002EC2 RID: 11970
		private ISleekElement itemSelectionContainer;

		// Token: 0x04002EC3 RID: 11971
		private SleekJars floorsMenu;

		// Token: 0x04002EC4 RID: 11972
		private SleekJars roofsMenu;

		// Token: 0x04002EC5 RID: 11973
		private SleekJars wallsMenu;

		// Token: 0x04002EC6 RID: 11974
		private SleekJars pillarsMenu;

		// Token: 0x04002EC7 RID: 11975
		private ISleekLabel floorsLabel;

		// Token: 0x04002EC8 RID: 11976
		private ISleekLabel noFloorItemsLabel;

		// Token: 0x04002EC9 RID: 11977
		private ISleekLabel roofsLabel;

		// Token: 0x04002ECA RID: 11978
		private ISleekLabel noRoofItemsLabel;

		// Token: 0x04002ECB RID: 11979
		private ISleekLabel wallsLabel;

		// Token: 0x04002ECC RID: 11980
		private ISleekLabel noWallItemsLabel;

		// Token: 0x04002ECD RID: 11981
		private ISleekLabel pillarsLabel;

		// Token: 0x04002ECE RID: 11982
		private ISleekLabel noPillarItemsLabel;

		/// <summary>
		/// Box in the HUD with selected item name and quantity.
		/// </summary>
		// Token: 0x04002ECF RID: 11983
		private ISleekBox selectedItemBox;

		// Token: 0x04002ED0 RID: 11984
		private ISleekLabel selectedItemNameLabel;

		// Token: 0x04002ED1 RID: 11985
		private ISleekLabel selectedItemQuantityLabel;

		// Token: 0x04002ED2 RID: 11986
		private List<InventorySearch> itemSearch;

		// Token: 0x04002ED3 RID: 11987
		private List<InventorySearch> floors;

		// Token: 0x04002ED4 RID: 11988
		private List<InventorySearch> roofs;

		// Token: 0x04002ED5 RID: 11989
		private List<InventorySearch> walls;

		// Token: 0x04002ED6 RID: 11990
		private List<InventorySearch> pillars;

		// Token: 0x04002ED7 RID: 11991
		private Dictionary<ushort, int> itemAmounts;

		// Token: 0x04002ED8 RID: 11992
		private int cachedSearchIndex = -1;

		// Token: 0x04002ED9 RID: 11993
		private const float MENU_RADIUS = 128f;

		// Token: 0x04002EDA RID: 11994
		private const int MENU_SIZE = 256;

		// Token: 0x04002EDB RID: 11995
		private const int MENU_PADDING = 50;

		// Token: 0x04002EDC RID: 11996
		private const float RADIAL_BACKDROP_ALPHA = 0.2f;
	}
}
