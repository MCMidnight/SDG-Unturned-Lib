using System;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000625 RID: 1573
	public class PlayerInteract : PlayerCaller
	{
		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x060032C6 RID: 12998 RVA: 0x000E69EF File Offset: 0x000E4BEF
		public static Interactable interactable
		{
			get
			{
				return PlayerInteract._interactable;
			}
		}

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x060032C7 RID: 12999 RVA: 0x000E69F6 File Offset: 0x000E4BF6
		public static Interactable2 interactable2
		{
			get
			{
				return PlayerInteract._interactable2;
			}
		}

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x060032C8 RID: 13000 RVA: 0x000E6A00 File Offset: 0x000E4C00
		private float salvageTime
		{
			get
			{
				if (this.shouldOverrideSalvageTime)
				{
					return this.overrideSalvageTimeValue;
				}
				if (base.player.equipment.useable is UseableHousingPlanner)
				{
					return 0.5f;
				}
				if (Provider.isServer || base.channel.owner.isAdmin)
				{
					LevelAsset asset = Level.getAsset();
					if (asset == null || asset.enableAdminFasterSalvageDuration)
					{
						return 1f;
					}
				}
				return 8f;
			}
		}

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x060032C9 RID: 13001 RVA: 0x000E6A70 File Offset: 0x000E4C70
		private float interactableSalvageTime
		{
			get
			{
				float num = this.salvageTime;
				if (PlayerInteract._interactable2 != null)
				{
					num *= PlayerInteract._interactable2.salvageDurationMultiplier;
				}
				return num;
			}
		}

		// Token: 0x060032CA RID: 13002 RVA: 0x000E6A9F File Offset: 0x000E4C9F
		[Obsolete]
		public void tellSalvageTimeOverride(CSteamID senderId, float overrideValue)
		{
			this.ReceiveSalvageTimeOverride(overrideValue);
		}

		/// <summary>
		/// Called from the server to override salvage duration.
		/// Only used by plugins.
		/// </summary>
		// Token: 0x060032CB RID: 13003 RVA: 0x000E6AA8 File Offset: 0x000E4CA8
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellSalvageTimeOverride")]
		public void ReceiveSalvageTimeOverride(float overrideValue)
		{
			this.overrideSalvageTimeValue = overrideValue;
			this.shouldOverrideSalvageTime = (this.overrideSalvageTimeValue > -0.5f);
		}

		/// <summary>
		/// Override salvage duration without admin.
		/// Only used by plugins.
		/// </summary>
		// Token: 0x060032CC RID: 13004 RVA: 0x000E6AC4 File Offset: 0x000E4CC4
		public void sendSalvageTimeOverride(float overrideValue)
		{
			PlayerInteract.SendSalvageTimeOverride.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), overrideValue);
		}

		// Token: 0x060032CD RID: 13005 RVA: 0x000E6AE3 File Offset: 0x000E4CE3
		private void hotkey(byte button)
		{
			VehicleManager.swapVehicle(button);
		}

		// Token: 0x060032CE RID: 13006 RVA: 0x000E6AEB File Offset: 0x000E4CEB
		[Obsolete]
		public void askInspect(CSteamID steamID)
		{
			this.ReceiveInspectRequest();
		}

		// Token: 0x060032CF RID: 13007 RVA: 0x000E6AF3 File Offset: 0x000E4CF3
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 2, legacyName = "askInspect")]
		public void ReceiveInspectRequest()
		{
			if (base.player.equipment.canInspect)
			{
				PlayerInteract.SendPlayInspect.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner());
				base.player.equipment.InvokeOnInspectingUseable();
			}
		}

		// Token: 0x060032D0 RID: 13008 RVA: 0x000E6B33 File Offset: 0x000E4D33
		[Obsolete]
		public void tellInspect(CSteamID steamID)
		{
			this.ReceivePlayInspect();
		}

		// Token: 0x060032D1 RID: 13009 RVA: 0x000E6B3B File Offset: 0x000E4D3B
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellInspect")]
		public void ReceivePlayInspect()
		{
			base.player.equipment.inspect();
		}

		// Token: 0x060032D2 RID: 13010 RVA: 0x000E6B4D File Offset: 0x000E4D4D
		private void localInspect()
		{
			if (base.player.equipment.canInspect)
			{
				base.player.equipment.inspect();
				if (!Provider.isServer)
				{
					PlayerInteract.SendInspectRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable);
				}
			}
		}

		// Token: 0x060032D3 RID: 13011 RVA: 0x000E6B89 File Offset: 0x000E4D89
		private void onPurchaseUpdated(HordePurchaseVolume node)
		{
			if (node == null)
			{
				PlayerInteract.purchaseAsset = null;
				return;
			}
			PlayerInteract.purchaseAsset = (Assets.find(EAssetType.ITEM, node.id) as ItemAsset);
		}

		// Token: 0x060032D4 RID: 13012 RVA: 0x000E6BB1 File Offset: 0x000E4DB1
		private void OnLifeUpdated(bool isDead)
		{
			PlayerInteract.salvageHeldTime = 0f;
		}

		// Token: 0x060032D5 RID: 13013 RVA: 0x000E6BC0 File Offset: 0x000E4DC0
		private void Update()
		{
			if (base.channel.IsLocalPlayer)
			{
				if (base.player.stance.stance != EPlayerStance.DRIVING && base.player.stance.stance != EPlayerStance.SITTING && base.player.life.IsAlive && !base.player.workzone.isBuilding)
				{
					if (Time.realtimeSinceStartup - PlayerInteract.lastInteract > 0.1f)
					{
						PlayerInteract.lastInteract = Time.realtimeSinceStartup;
						int num = RayMasks.PLAYER_INTERACT;
						if (base.player.stance.stance == EPlayerStance.CLIMB)
						{
							num &= -33554433;
						}
						if (base.player.look.IsLocallyUsingFreecam)
						{
							Physics.Raycast(new Ray(base.player.look.aim.position, base.player.look.aim.forward), out PlayerInteract.hit, 4f, num);
						}
						else
						{
							Physics.Raycast(new Ray(MainCamera.instance.transform.position, MainCamera.instance.transform.forward), out PlayerInteract.hit, (float)((base.player.look.perspective == EPlayerPerspective.THIRD) ? 6 : 4), num);
						}
					}
					Transform x = (PlayerInteract.hit.collider != null) ? PlayerInteract.hit.collider.transform : null;
					bool flag = x != null;
					if (x != PlayerInteract.focus || flag != this.didHaveFocus)
					{
						this.clearHighlight();
						PlayerInteract.focus = null;
						this.didHaveFocus = false;
						PlayerInteract.target = null;
						PlayerInteract._interactable = null;
						PlayerInteract._interactable2 = null;
						if (x != null)
						{
							PlayerInteract.focus = x;
							this.didHaveFocus = true;
							PlayerInteract._interactable = PlayerInteract.focus.GetComponentInParent<Interactable>();
							PlayerInteract._interactable2 = PlayerInteract.focus.GetComponentInParent<Interactable2>();
							if (PlayerInteract._interactable == null && PlayerInteract.focus.CompareTag("Ladder"))
							{
								PlayerInteract._interactable = PlayerInteract.focus.gameObject.AddComponent<InteractableLadder>();
							}
							if (PlayerInteract.interactable != null)
							{
								PlayerInteract.target = PlayerInteract.interactable.transform.FindChildRecursive("Target");
								if (PlayerInteract.interactable.checkInteractable())
								{
									if (PlayerUI.window.isEnabled)
									{
										if (PlayerInteract.interactable.checkUseable())
										{
											Color color;
											if (!PlayerInteract.interactable.checkHighlight(out color))
											{
												color = Color.green;
											}
											InteractableDoorHinge componentInParent = PlayerInteract.focus.GetComponentInParent<InteractableDoorHinge>();
											if (componentInParent != null)
											{
												this.setHighlight(componentInParent.door.transform, color);
											}
											else
											{
												this.setHighlight(PlayerInteract.interactable.transform, color);
											}
										}
										else
										{
											Color color = Color.red;
											InteractableDoorHinge componentInParent2 = PlayerInteract.focus.GetComponentInParent<InteractableDoorHinge>();
											if (componentInParent2 != null)
											{
												this.setHighlight(componentInParent2.door.transform, color);
											}
											else
											{
												this.setHighlight(PlayerInteract.interactable.transform, color);
											}
										}
									}
								}
								else
								{
									PlayerInteract.target = null;
									PlayerInteract._interactable = null;
								}
							}
						}
					}
				}
				else
				{
					this.clearHighlight();
					PlayerInteract.focus = null;
					this.didHaveFocus = false;
					PlayerInteract.target = null;
					PlayerInteract._interactable = null;
					PlayerInteract._interactable2 = null;
				}
				if (base.player.life.IsAlive)
				{
					if (PlayerInteract.interactable != null)
					{
						EPlayerMessage eplayerMessage;
						string text;
						Color color2;
						if (PlayerInteract.interactable.checkHint(out eplayerMessage, out text, out color2) && !PlayerUI.window.showCursor)
						{
							if (eplayerMessage == EPlayerMessage.ITEM)
							{
								PlayerUI.hint((PlayerInteract.target != null) ? PlayerInteract.target : PlayerInteract.focus, eplayerMessage, text, color2, new object[]
								{
									((InteractableItem)PlayerInteract.interactable).item,
									((InteractableItem)PlayerInteract.interactable).asset
								});
							}
							else
							{
								PlayerUI.hint((PlayerInteract.target != null) ? PlayerInteract.target : PlayerInteract.focus, eplayerMessage, text, color2, Array.Empty<object>());
							}
						}
					}
					else if (PlayerInteract.purchaseAsset != null && base.player.movement.purchaseNode != null && !PlayerUI.window.showCursor)
					{
						PlayerUI.hint(null, EPlayerMessage.PURCHASE, "", Color.white, new object[]
						{
							PlayerInteract.purchaseAsset.itemName,
							base.player.movement.purchaseNode.cost
						});
					}
					else if (PlayerInteract.focus != null && PlayerInteract.focus.CompareTag("Enemy"))
					{
						bool flag2 = (base.player.pluginWidgetFlags & EPluginWidgetFlags.ShowInteractWithEnemy) == EPluginWidgetFlags.ShowInteractWithEnemy;
						bool flag3 = !PlayerUI.window.showCursor;
						if (flag2 && flag3)
						{
							Player player = DamageTool.getPlayer(PlayerInteract.focus);
							if (player != null && player != base.player)
							{
								PlayerUI.hint(null, EPlayerMessage.ENEMY, string.Empty, Color.white, new object[]
								{
									player.channel.owner
								});
							}
						}
					}
					if (PlayerInteract.interactable2 != null)
					{
						EPlayerMessage message;
						float data;
						if (PlayerInteract.interactable2.checkHint(out message, out data) && !PlayerUI.window.showCursor)
						{
							PlayerUI.hint2(message, PlayerInteract.isHoldingKey ? (PlayerInteract.salvageHeldTime / this.interactableSalvageTime) : 0f, data);
						}
					}
					else
					{
						PlayerInteract.salvageHeldTime = 0f;
					}
					if ((base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING) && !InputEx.GetKey(KeyCode.LeftShift))
					{
						if (InputEx.GetKeyDown(KeyCode.F1))
						{
							this.hotkey(0);
						}
						if (InputEx.GetKeyDown(KeyCode.F2))
						{
							this.hotkey(1);
						}
						if (InputEx.GetKeyDown(KeyCode.F3))
						{
							this.hotkey(2);
						}
						if (InputEx.GetKeyDown(KeyCode.F4))
						{
							this.hotkey(3);
						}
						if (InputEx.GetKeyDown(KeyCode.F5))
						{
							this.hotkey(4);
						}
						if (InputEx.GetKeyDown(KeyCode.F6))
						{
							this.hotkey(5);
						}
						if (InputEx.GetKeyDown(KeyCode.F7))
						{
							this.hotkey(6);
						}
						if (InputEx.GetKeyDown(KeyCode.F8))
						{
							this.hotkey(7);
						}
						if (InputEx.GetKeyDown(KeyCode.F9))
						{
							this.hotkey(8);
						}
						if (InputEx.GetKeyDown(KeyCode.F10))
						{
							this.hotkey(9);
						}
					}
					if (InputEx.GetKeyDown(ControlsSettings.interact))
					{
						PlayerInteract.salvageHeldTime = 0f;
						PlayerInteract.isHoldingKey = true;
					}
					if (InputEx.GetKeyDown(ControlsSettings.inspect) && ControlsSettings.inspect != ControlsSettings.interact)
					{
						this.localInspect();
					}
					if (PlayerInteract.isHoldingKey)
					{
						PlayerInteract.salvageHeldTime += Time.deltaTime;
						if (InputEx.GetKeyUp(ControlsSettings.interact))
						{
							PlayerInteract.isHoldingKey = false;
							if (PlayerUI.window.showCursor)
							{
								if (base.player.inventory.isStoring && base.player.inventory.shouldInteractCloseStorage)
								{
									PlayerDashboardUI.close();
									PlayerLifeUI.open();
									return;
								}
								if (PlayerBarricadeSignUI.active)
								{
									PlayerBarricadeSignUI.close();
									PlayerLifeUI.open();
									return;
								}
								if (PlayerUI.instance.boomboxUI.active)
								{
									PlayerUI.instance.boomboxUI.close();
									PlayerLifeUI.open();
									return;
								}
								if (PlayerBarricadeLibraryUI.active)
								{
									PlayerBarricadeLibraryUI.close();
									PlayerLifeUI.open();
									return;
								}
								if (PlayerUI.instance.mannequinUI.active)
								{
									PlayerUI.instance.mannequinUI.close();
									PlayerLifeUI.open();
									return;
								}
								if (PlayerNPCDialogueUI.active)
								{
									if (PlayerNPCDialogueUI.IsDialogueAnimating)
									{
										PlayerNPCDialogueUI.SkipAnimation();
										return;
									}
									if (PlayerNPCDialogueUI.CanAdvanceToNextPage)
									{
										PlayerNPCDialogueUI.AdvancePage();
										return;
									}
									PlayerNPCDialogueUI.close();
									PlayerLifeUI.open();
									return;
								}
								else
								{
									if (PlayerNPCQuestUI.active)
									{
										PlayerNPCQuestUI.closeNicely();
										return;
									}
									if (PlayerNPCVendorUI.active)
									{
										PlayerNPCVendorUI.closeNicely();
										return;
									}
								}
							}
							else
							{
								if (base.player.stance.stance == EPlayerStance.DRIVING || base.player.stance.stance == EPlayerStance.SITTING)
								{
									VehicleManager.exitVehicle();
									return;
								}
								if (PlayerInteract.focus != null && PlayerInteract.interactable != null)
								{
									if (PlayerInteract.interactable.checkUseable())
									{
										PlayerInteract.interactable.use();
										return;
									}
								}
								else if (PlayerInteract.purchaseAsset != null)
								{
									if (base.player.skills.experience >= base.player.movement.purchaseNode.cost)
									{
										base.player.skills.sendPurchase(base.player.movement.purchaseNode);
										return;
									}
								}
								else if (ControlsSettings.inspect == ControlsSettings.interact)
								{
									this.localInspect();
									return;
								}
							}
						}
						else if (PlayerInteract.salvageHeldTime > this.interactableSalvageTime)
						{
							PlayerInteract.isHoldingKey = false;
							if (!PlayerUI.window.showCursor && PlayerInteract.interactable2 != null)
							{
								PlayerInteract.interactable2.use();
							}
						}
					}
				}
			}
		}

		// Token: 0x060032D6 RID: 13014 RVA: 0x000E747C File Offset: 0x000E567C
		internal void InitializePlayer()
		{
			if (base.channel.IsLocalPlayer)
			{
				PlayerMovement movement = base.player.movement;
				movement.onPurchaseUpdated = (PurchaseUpdated)Delegate.Combine(movement.onPurchaseUpdated, new PurchaseUpdated(this.onPurchaseUpdated));
				PlayerLife life = base.player.life;
				life.onLifeUpdated = (LifeUpdated)Delegate.Combine(life.onLifeUpdated, new LifeUpdated(this.OnLifeUpdated));
			}
		}

		// Token: 0x060032D7 RID: 13015 RVA: 0x000E74EE File Offset: 0x000E56EE
		private void clearHighlight()
		{
			if (this.highlightedTransform != null)
			{
				HighlighterTool.unhighlight(this.highlightedTransform);
			}
		}

		// Token: 0x060032D8 RID: 13016 RVA: 0x000E7509 File Offset: 0x000E5709
		private void setHighlight(Transform newHighlightedTransform, Color color)
		{
			this.highlightedTransform = newHighlightedTransform;
			HighlighterTool.highlight(newHighlightedTransform, color);
		}

		// Token: 0x04001D51 RID: 7505
		private static Transform focus;

		// Token: 0x04001D52 RID: 7506
		private static Transform target;

		// Token: 0x04001D53 RID: 7507
		private static ItemAsset purchaseAsset;

		// Token: 0x04001D54 RID: 7508
		private static Interactable _interactable;

		// Token: 0x04001D55 RID: 7509
		private static Interactable2 _interactable2;

		// Token: 0x04001D56 RID: 7510
		internal static RaycastHit hit;

		// Token: 0x04001D57 RID: 7511
		private static float lastInteract;

		// Token: 0x04001D58 RID: 7512
		private static float salvageHeldTime;

		// Token: 0x04001D59 RID: 7513
		private static bool isHoldingKey;

		// Token: 0x04001D5A RID: 7514
		private bool shouldOverrideSalvageTime;

		// Token: 0x04001D5B RID: 7515
		private float overrideSalvageTimeValue;

		// Token: 0x04001D5C RID: 7516
		private static readonly ClientInstanceMethod<float> SendSalvageTimeOverride = ClientInstanceMethod<float>.Get(typeof(PlayerInteract), "ReceiveSalvageTimeOverride");

		// Token: 0x04001D5D RID: 7517
		private static readonly ServerInstanceMethod SendInspectRequest = ServerInstanceMethod.Get(typeof(PlayerInteract), "ReceiveInspectRequest");

		// Token: 0x04001D5E RID: 7518
		private static readonly ClientInstanceMethod SendPlayInspect = ClientInstanceMethod.Get(typeof(PlayerInteract), "ReceivePlayInspect");

		/// <summary>
		/// Outlined object is not necessarily the focused object, so we track it to disable later if focus is destroyed.
		/// </summary>
		// Token: 0x04001D5F RID: 7519
		private Transform highlightedTransform;

		/// <summary>
		/// Was focus non-null during last update? Used to detect when focus was destroyed.
		/// </summary>
		// Token: 0x04001D60 RID: 7520
		private bool didHaveFocus;
	}
}
