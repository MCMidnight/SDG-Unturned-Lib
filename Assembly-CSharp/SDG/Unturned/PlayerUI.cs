using System;
using System.Collections.Generic;
using SDG.Framework.Water;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000660 RID: 1632
	public class PlayerUI : MonoBehaviour
	{
		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x0600363C RID: 13884 RVA: 0x000FA67D File Offset: 0x000F887D
		// (set) Token: 0x0600363D RID: 13885 RVA: 0x000FA684 File Offset: 0x000F8884
		public static bool isBlindfolded
		{
			get
			{
				return PlayerUI._isBlindfolded;
			}
			set
			{
				if (PlayerUI.isBlindfolded == value)
				{
					return;
				}
				PlayerUI._isBlindfolded = value;
				PlayerUI.isBlindfoldedChanged();
				PlayerUI.UpdateWindowEnabled();
			}
		}

		// Token: 0x140000CC RID: 204
		// (add) Token: 0x0600363E RID: 13886 RVA: 0x000FA6A4 File Offset: 0x000F88A4
		// (remove) Token: 0x0600363F RID: 13887 RVA: 0x000FA6D8 File Offset: 0x000F88D8
		public static event IsBlindfoldedChangedHandler isBlindfoldedChanged;

		// Token: 0x06003640 RID: 13888 RVA: 0x000FA70C File Offset: 0x000F890C
		public static void stun(Color color, float amount)
		{
			PlayerUI.stunColor = color;
			PlayerUI.stunAlpha = amount * 5f;
			MainCamera.instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Sounds/General/Stun"), amount);
			if (!PlayerUI.isWindowEnabledByColorOverlay)
			{
				PlayerUI.isWindowEnabledByColorOverlay = true;
				PlayerUI.UpdateWindowEnabled();
			}
		}

		// Token: 0x06003641 RID: 13889 RVA: 0x000FA75C File Offset: 0x000F895C
		public static void pain(float amount)
		{
			PlayerUI.painAlpha = amount * 0.75f;
			if (!PlayerUI.isWindowEnabledByColorOverlay)
			{
				PlayerUI.isWindowEnabledByColorOverlay = true;
				PlayerUI.UpdateWindowEnabled();
			}
		}

		// Token: 0x06003642 RID: 13890 RVA: 0x000FA77C File Offset: 0x000F897C
		public static void hitmark(Vector3 point, bool worldspace, EPlayerHit newHit)
		{
			if (!PlayerUI.wantsWindowEnabled)
			{
				return;
			}
			if (!Provider.modeConfigData.Gameplay.Hitmarkers)
			{
				return;
			}
			HitmarkerInfo hitmarkerInfo = new HitmarkerInfo
			{
				worldPosition = point,
				shouldFollowWorldPosition = (worldspace || OptionsSettings.ShouldHitmarkersFollowWorldPosition),
				sleekElement = PlayerLifeUI.ClaimHitmarker()
			};
			hitmarkerInfo.sleekElement.SetStyle(newHit);
			if (OptionsSettings.hitmarkerStyle == EHitmarkerStyle.Animated)
			{
				hitmarkerInfo.sleekElement.PlayAnimation();
			}
			else
			{
				hitmarkerInfo.sleekElement.ApplyClassicPositions();
			}
			PlayerLifeUI.activeHitmarkers.Add(hitmarkerInfo);
			if (newHit == EPlayerHit.CRITICAL)
			{
				MainCamera.instance.GetComponent<AudioSource>().PlayOneShot(PlayerUI.hitCriticalSound, 0.5f);
			}
		}

		// Token: 0x06003643 RID: 13891 RVA: 0x000FA829 File Offset: 0x000F8A29
		public static void enableDot()
		{
			PlayerLifeUI.crosshair.SetGameWantsCenterDotVisible(true);
		}

		// Token: 0x06003644 RID: 13892 RVA: 0x000FA836 File Offset: 0x000F8A36
		public static void disableDot()
		{
			PlayerLifeUI.crosshair.SetGameWantsCenterDotVisible(false);
		}

		// Token: 0x06003645 RID: 13893 RVA: 0x000FA843 File Offset: 0x000F8A43
		public static void updateScope(bool isScoped)
		{
			PlayerLifeUI.scopeOverlay.IsVisible = isScoped;
			PlayerUI.container.IsVisible = !isScoped;
			PlayerUI.UpdateWindowEnabled();
		}

		// Token: 0x06003646 RID: 13894 RVA: 0x000FA863 File Offset: 0x000F8A63
		public static void updateBinoculars(bool isBinoculars)
		{
			PlayerLifeUI.binocularsOverlay.IsVisible = isBinoculars;
			PlayerUI.container.IsVisible = !isBinoculars;
			PlayerUI.UpdateWindowEnabled();
		}

		// Token: 0x06003647 RID: 13895 RVA: 0x000FA883 File Offset: 0x000F8A83
		private static void UpdateWindowEnabled()
		{
			PlayerUI.window.isEnabled = (PlayerUI.wantsWindowEnabled || PlayerLifeUI.scopeOverlay.IsVisible || PlayerLifeUI.binocularsOverlay.IsVisible || PlayerUI.isBlindfolded || PlayerUI.isWindowEnabledByColorOverlay);
		}

		// Token: 0x06003648 RID: 13896 RVA: 0x000FA8BD File Offset: 0x000F8ABD
		public static void enableCrosshair()
		{
			if (Provider.modeConfigData.Gameplay.Crosshair)
			{
				PlayerLifeUI.crosshair.SetDirectionalArrowsVisible(true);
			}
		}

		// Token: 0x06003649 RID: 13897 RVA: 0x000FA8DB File Offset: 0x000F8ADB
		public static void disableCrosshair()
		{
			if (Provider.modeConfigData.Gameplay.Crosshair)
			{
				PlayerLifeUI.crosshair.SetDirectionalArrowsVisible(false);
			}
		}

		/// <summary>
		/// Hints/messages are the pop-up texts below the interaction prompt, e.g. "reload" or "full moon rises". 
		/// Got a complaint that the item placement obstructed hint was shown if placing multiple signs.
		/// </summary>
		// Token: 0x0600364A RID: 13898 RVA: 0x000FA8F9 File Offset: 0x000F8AF9
		private static bool ShouldIgnoreHintAndMessageRequests()
		{
			return PlayerBarricadeSignUI.active || (PlayerUI.instance.boomboxUI != null && PlayerUI.instance.boomboxUI.active);
		}

		// Token: 0x0600364B RID: 13899 RVA: 0x000FA921 File Offset: 0x000F8B21
		public static void hint(Transform transform, EPlayerMessage message)
		{
			PlayerUI.hint(transform, message, "", Color.white, Array.Empty<object>());
		}

		// Token: 0x0600364C RID: 13900 RVA: 0x000FA93C File Offset: 0x000F8B3C
		public static void hint(Transform transform, EPlayerMessage message, string text, Color color, params object[] objects)
		{
			if (PlayerUI.messageBox == null || PlayerLifeUI.localization == null)
			{
				return;
			}
			if (PlayerUI.ShouldIgnoreHintAndMessageRequests())
			{
				return;
			}
			PlayerUI.lastHinted = true;
			PlayerUI.isHinted = true;
			if (message == EPlayerMessage.ENEMY)
			{
				if (objects.Length == 1)
				{
					SteamPlayer steamPlayer = (SteamPlayer)objects[0];
					if (PlayerUI.messagePlayer != null && PlayerUI.messagePlayer.player != steamPlayer)
					{
						PlayerUI.container.RemoveChild(PlayerUI.messagePlayer);
						PlayerUI.messagePlayer = null;
					}
					if (PlayerUI.messagePlayer == null)
					{
						PlayerUI.messagePlayer = new SleekPlayer(steamPlayer, false, SleekPlayer.ESleekPlayerDisplayContext.NONE);
						PlayerUI.messagePlayer.PositionOffset_X = -150f;
						PlayerUI.messagePlayer.PositionOffset_Y = -130f;
						PlayerUI.messagePlayer.PositionScale_X = 0.5f;
						PlayerUI.messagePlayer.PositionScale_Y = 1f;
						PlayerUI.messagePlayer.SizeOffset_X = 300f;
						PlayerUI.messagePlayer.SizeOffset_Y = 50f;
						PlayerUI.container.AddChild(PlayerUI.messagePlayer);
					}
				}
				PlayerUI.messageBox.IsVisible = false;
				if (PlayerUI.messagePlayer != null)
				{
					PlayerUI.messagePlayer.IsVisible = true;
				}
				return;
			}
			PlayerUI.messageBox.IsVisible = true;
			if (PlayerUI.messagePlayer != null)
			{
				PlayerUI.messagePlayer.IsVisible = false;
			}
			PlayerUI.messageIcon_0.PositionOffset_Y = 45f;
			PlayerUI.messageProgress_0.PositionOffset_Y = 50f;
			PlayerUI.messageIcon_1.PositionOffset_Y = 75f;
			PlayerUI.messageProgress_1.PositionOffset_Y = 80f;
			PlayerUI.messageIcon_2.PositionOffset_Y = 105f;
			PlayerUI.messageProgress_2.PositionOffset_Y = 110f;
			if (message == EPlayerMessage.VEHICLE_ENTER)
			{
				InteractableVehicle interactableVehicle = (InteractableVehicle)PlayerInteract.interactable;
				int num = 45;
				bool flag = interactableVehicle.usesFuel || interactableVehicle.asset.isStaminaPowered;
				PlayerUI.messageIcon_0.IsVisible = flag;
				PlayerUI.messageProgress_0.IsVisible = flag;
				if (flag)
				{
					PlayerUI.messageIcon_0.PositionOffset_Y = (float)num;
					PlayerUI.messageProgress_0.PositionOffset_Y = (float)(num + 5);
					num += 30;
				}
				PlayerUI.messageIcon_1.IsVisible = interactableVehicle.usesHealth;
				PlayerUI.messageProgress_1.IsVisible = interactableVehicle.usesHealth;
				if (interactableVehicle.usesHealth)
				{
					PlayerUI.messageIcon_1.PositionOffset_Y = (float)num;
					PlayerUI.messageProgress_1.PositionOffset_Y = (float)(num + 5);
					num += 30;
				}
				PlayerUI.messageIcon_2.IsVisible = interactableVehicle.usesBattery;
				PlayerUI.messageProgress_2.IsVisible = interactableVehicle.usesBattery;
				if (interactableVehicle.usesBattery)
				{
					PlayerUI.messageIcon_2.PositionOffset_Y = (float)num;
					PlayerUI.messageProgress_2.PositionOffset_Y = (float)(num + 5);
					num += 30;
				}
				PlayerUI.messageBox.SizeOffset_Y = (float)(num - 5);
				if (flag)
				{
					ushort num2;
					ushort num3;
					interactableVehicle.getDisplayFuel(out num2, out num3);
					PlayerUI.messageProgress_0.state = (float)num2 / (float)num3;
					PlayerUI.messageProgress_0.color = Palette.COLOR_Y;
					PlayerUI.messageIcon_0.Texture = PlayerLifeUI.icons.load<Texture2D>("Fuel");
				}
				if (interactableVehicle.usesHealth)
				{
					PlayerUI.messageProgress_1.state = (float)interactableVehicle.health / (float)interactableVehicle.asset.health;
					PlayerUI.messageProgress_1.color = Palette.COLOR_R;
					PlayerUI.messageIcon_1.Texture = PlayerLifeUI.icons.load<Texture2D>("Health");
				}
				if (interactableVehicle.usesBattery)
				{
					PlayerUI.messageProgress_2.state = (float)interactableVehicle.batteryCharge / 10000f;
					PlayerUI.messageProgress_2.color = Palette.COLOR_Y;
					PlayerUI.messageIcon_2.Texture = PlayerLifeUI.icons.load<Texture2D>("Stamina");
				}
				PlayerUI.messageQualityImage.IsVisible = false;
				PlayerUI.messageAmountLabel.IsVisible = false;
			}
			else if (message == EPlayerMessage.GENERATOR_ON || message == EPlayerMessage.GENERATOR_OFF || message == EPlayerMessage.GROW || message == EPlayerMessage.VOLUME_WATER || message == EPlayerMessage.VOLUME_FUEL)
			{
				PlayerUI.messageBox.SizeOffset_Y = 70f;
				PlayerUI.messageProgress_0.IsVisible = true;
				PlayerUI.messageIcon_0.IsVisible = true;
				PlayerUI.messageProgress_1.IsVisible = false;
				PlayerUI.messageIcon_1.IsVisible = false;
				PlayerUI.messageProgress_2.IsVisible = false;
				PlayerUI.messageIcon_2.IsVisible = false;
				if (message == EPlayerMessage.GENERATOR_ON || message == EPlayerMessage.GENERATOR_OFF)
				{
					InteractableGenerator interactableGenerator = (InteractableGenerator)PlayerInteract.interactable;
					PlayerUI.messageProgress_0.state = (float)interactableGenerator.fuel / (float)interactableGenerator.capacity;
					PlayerUI.messageIcon_0.Texture = PlayerLifeUI.icons.load<Texture2D>("Fuel");
				}
				else if (message == EPlayerMessage.GROW)
				{
					InteractableFarm interactableFarm = (InteractableFarm)PlayerInteract.interactable;
					float num4 = 0f;
					if (interactableFarm.planted > 0U && Provider.time > interactableFarm.planted)
					{
						num4 = Provider.time - interactableFarm.planted;
					}
					PlayerUI.messageProgress_0.state = num4 / interactableFarm.growth;
					PlayerUI.messageIcon_0.Texture = PlayerLifeUI.icons.load<Texture2D>("Grow");
				}
				else if (message == EPlayerMessage.VOLUME_WATER)
				{
					if (PlayerInteract.interactable is InteractableObjectResource)
					{
						InteractableObjectResource interactableObjectResource = (InteractableObjectResource)PlayerInteract.interactable;
						PlayerUI.messageProgress_0.state = (float)interactableObjectResource.amount / (float)interactableObjectResource.capacity;
					}
					else if (PlayerInteract.interactable is InteractableTank)
					{
						InteractableTank interactableTank = (InteractableTank)PlayerInteract.interactable;
						PlayerUI.messageProgress_0.state = (float)interactableTank.amount / (float)interactableTank.capacity;
					}
					else if (PlayerInteract.interactable is InteractableRainBarrel)
					{
						InteractableRainBarrel interactableRainBarrel = (InteractableRainBarrel)PlayerInteract.interactable;
						PlayerUI.messageProgress_0.state = (interactableRainBarrel.isFull ? 1f : 0f);
						if (interactableRainBarrel.isFull)
						{
							text = PlayerLifeUI.localization.format("Full");
						}
						else
						{
							text = PlayerLifeUI.localization.format("Empty");
						}
					}
					PlayerUI.messageIcon_0.Texture = PlayerLifeUI.icons.load<Texture2D>("Water");
				}
				else if (message == EPlayerMessage.VOLUME_FUEL)
				{
					if (PlayerInteract.interactable is InteractableObjectResource)
					{
						InteractableObjectResource interactableObjectResource2 = (InteractableObjectResource)PlayerInteract.interactable;
						PlayerUI.messageProgress_0.state = (float)interactableObjectResource2.amount / (float)interactableObjectResource2.capacity;
					}
					else if (PlayerInteract.interactable is InteractableTank)
					{
						InteractableTank interactableTank2 = (InteractableTank)PlayerInteract.interactable;
						PlayerUI.messageProgress_0.state = (float)interactableTank2.amount / (float)interactableTank2.capacity;
					}
					else if (PlayerInteract.interactable is InteractableOil)
					{
						InteractableOil interactableOil = (InteractableOil)PlayerInteract.interactable;
						PlayerUI.messageProgress_0.state = (float)interactableOil.fuel / (float)interactableOil.capacity;
					}
					PlayerUI.messageIcon_0.Texture = PlayerLifeUI.icons.load<Texture2D>("Fuel");
				}
				if (message == EPlayerMessage.GROW)
				{
					PlayerUI.messageProgress_0.color = Palette.COLOR_G;
				}
				else if (message == EPlayerMessage.VOLUME_WATER)
				{
					PlayerUI.messageProgress_0.color = Palette.COLOR_B;
				}
				else
				{
					PlayerUI.messageProgress_0.color = Palette.COLOR_Y;
				}
				PlayerUI.messageQualityImage.IsVisible = false;
				PlayerUI.messageAmountLabel.IsVisible = false;
			}
			else if (message == EPlayerMessage.ITEM)
			{
				PlayerUI.messageBox.SizeOffset_Y = 70f;
				if (objects.Length == 2)
				{
					if (((ItemAsset)objects[1]).showQuality)
					{
						PlayerUI.messageQualityImage.TintColor = ItemTool.getQualityColor((float)((Item)objects[0]).quality / 100f);
						PlayerUI.messageAmountLabel.Text = ((Item)objects[0]).quality.ToString() + "%";
						PlayerUI.messageAmountLabel.TextColor = PlayerUI.messageQualityImage.TintColor;
						PlayerUI.messageQualityImage.IsVisible = true;
						PlayerUI.messageAmountLabel.IsVisible = true;
					}
					else if (((ItemAsset)objects[1]).amount > 1)
					{
						PlayerUI.messageAmountLabel.Text = "x" + ((Item)objects[0]).amount.ToString();
						PlayerUI.messageAmountLabel.TextColor = 3;
						PlayerUI.messageQualityImage.IsVisible = false;
						PlayerUI.messageAmountLabel.IsVisible = true;
					}
					else
					{
						PlayerUI.messageQualityImage.IsVisible = false;
						PlayerUI.messageAmountLabel.IsVisible = false;
					}
				}
				PlayerUI.messageProgress_0.IsVisible = false;
				PlayerUI.messageIcon_0.IsVisible = false;
				PlayerUI.messageProgress_1.IsVisible = false;
				PlayerUI.messageIcon_1.IsVisible = false;
				PlayerUI.messageProgress_2.IsVisible = false;
				PlayerUI.messageIcon_2.IsVisible = false;
			}
			else
			{
				PlayerUI.messageBox.SizeOffset_Y = 50f;
				PlayerUI.messageQualityImage.IsVisible = false;
				PlayerUI.messageAmountLabel.IsVisible = false;
				PlayerUI.messageProgress_0.IsVisible = false;
				PlayerUI.messageIcon_0.IsVisible = false;
				PlayerUI.messageProgress_1.IsVisible = false;
				PlayerUI.messageIcon_1.IsVisible = false;
				PlayerUI.messageProgress_2.IsVisible = false;
				PlayerUI.messageIcon_2.IsVisible = false;
			}
			bool flag2 = message == EPlayerMessage.ITEM || message == EPlayerMessage.VEHICLE_ENTER;
			if (flag2)
			{
				PlayerUI.messageBox.BackgroundColor = SleekColor.BackgroundIfLight(color);
			}
			else
			{
				PlayerUI.messageBox.BackgroundColor = 1;
			}
			PlayerUI.messageLabel.AllowRichText = (message == EPlayerMessage.CONDITION || message == EPlayerMessage.TALK || message == EPlayerMessage.INTERACT);
			if (PlayerUI.messageLabel.AllowRichText)
			{
				PlayerUI.messageLabel.TextColor = 4;
				PlayerUI.messageLabel.TextContrastContext = 1;
			}
			else if (flag2)
			{
				PlayerUI.messageLabel.TextColor = color;
				PlayerUI.messageLabel.TextContrastContext = 1;
			}
			else
			{
				PlayerUI.messageLabel.TextColor = 3;
				PlayerUI.messageLabel.TextContrastContext = 0;
			}
			PlayerUI.messageBox.SizeOffset_X = 200f;
			if (message == EPlayerMessage.ITEM)
			{
				PlayerUI.messageBox.SizeOffset_X = 300f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Item", text, MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact));
			}
			else if (message == EPlayerMessage.VEHICLE_ENTER)
			{
				PlayerUI.messageBox.SizeOffset_X = 300f;
				InteractableVehicle interactableVehicle2 = (InteractableVehicle)PlayerInteract.interactable;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format(interactableVehicle2.isLocked ? "Vehicle_Enter_Locked" : "Vehicle_Enter_Unlocked", text, MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact));
			}
			else if (message == EPlayerMessage.DOOR_OPEN)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Door_Open", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact));
			}
			else if (message == EPlayerMessage.DOOR_CLOSE)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Door_Close", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact));
			}
			else if (message == EPlayerMessage.LOCKED)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Locked");
			}
			else if (message == EPlayerMessage.BLOCKED)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Blocked");
			}
			else if (message == EPlayerMessage.PLACEMENT_OBSTRUCTED_BY)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("PlacementObstructedBy", text);
			}
			else if (message == EPlayerMessage.PLACEMENT_OBSTRUCTED_BY_GROUND)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("PlacementObstructedByGround");
			}
			else if (message == EPlayerMessage.FREEFORM_BUILDABLE_NOT_ALLOWED)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("FreeformBuildableNotAllowed");
			}
			else if (message == EPlayerMessage.PILLAR)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Pillar");
			}
			else if (message == EPlayerMessage.POST)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Post");
			}
			else if (message == EPlayerMessage.ROOF)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Roof");
			}
			else if (message == EPlayerMessage.WALL)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Wall");
			}
			else if (message == EPlayerMessage.CORNER)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Corner");
			}
			else if (message == EPlayerMessage.GROUND)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Ground");
			}
			else if (message == EPlayerMessage.DOORWAY)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Doorway");
			}
			else if (message == EPlayerMessage.WINDOW)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Window");
			}
			else if (message == EPlayerMessage.GARAGE)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Garage");
			}
			else if (message == EPlayerMessage.BED_ON)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Bed_On", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact), text);
			}
			else if (message == EPlayerMessage.BED_OFF)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Bed_Off", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact), text);
			}
			else if (message == EPlayerMessage.BED_CLAIMED)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Bed_Claimed");
			}
			else if (message == EPlayerMessage.BOUNDS)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Bounds");
			}
			else if (message == EPlayerMessage.STORAGE)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Storage", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact));
			}
			else if (message == EPlayerMessage.FARM)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Farm", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact));
			}
			else if (message == EPlayerMessage.GROW)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Grow");
			}
			else if (message == EPlayerMessage.SOIL)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Soil");
			}
			else if (message == EPlayerMessage.FIRE_ON)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Fire_On", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact));
			}
			else if (message == EPlayerMessage.FIRE_OFF)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Fire_Off", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact));
			}
			else if (message == EPlayerMessage.FORAGE)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Forage", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact));
			}
			else if (message == EPlayerMessage.GENERATOR_ON)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Generator_On", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact));
			}
			else if (message == EPlayerMessage.GENERATOR_OFF)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Generator_Off", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact));
			}
			else if (message == EPlayerMessage.SPOT_ON)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Spot_On", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact));
			}
			else if (message == EPlayerMessage.SPOT_OFF)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Spot_Off", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact));
			}
			else if (message == EPlayerMessage.PURCHASE)
			{
				if (objects.Length == 2)
				{
					PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Purchase", objects[0], objects[1], MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact));
				}
			}
			else if (message == EPlayerMessage.POWER)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Power");
			}
			else if (message == EPlayerMessage.USE)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Use", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact));
			}
			else if (message == EPlayerMessage.TUTORIAL_MOVE)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Move", new object[]
				{
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.left),
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.right),
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.up),
					MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.down)
				});
			}
			else if (message == EPlayerMessage.TUTORIAL_LOOK)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Look");
			}
			else if (message == EPlayerMessage.TUTORIAL_JUMP)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Jump", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.jump));
			}
			else if (message == EPlayerMessage.TUTORIAL_PERSPECTIVE)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Perspective", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.perspective));
			}
			else if (message == EPlayerMessage.TUTORIAL_RUN)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Run", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.sprint));
			}
			else if (message == EPlayerMessage.TUTORIAL_INVENTORY)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Inventory", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact));
			}
			else if (message == EPlayerMessage.TUTORIAL_SURVIVAL)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Survival", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.inventory), MenuConfigurationControlsUI.getKeyCodeText(KeyCode.Mouse1));
			}
			else if (message == EPlayerMessage.TUTORIAL_GUN)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Gun", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.secondary), MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.primary));
			}
			else if (message == EPlayerMessage.TUTORIAL_LADDER)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Ladder");
			}
			else if (message == EPlayerMessage.TUTORIAL_CRAFT)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Craft", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.attach), MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.crafting));
			}
			else if (message == EPlayerMessage.TUTORIAL_SKILLS)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Skills", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.skills));
			}
			else if (message == EPlayerMessage.TUTORIAL_SWIM)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Swim", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.jump));
			}
			else if (message == EPlayerMessage.TUTORIAL_MEDICAL)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Medical", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.primary));
			}
			else if (message == EPlayerMessage.TUTORIAL_VEHICLE)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Vehicle", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.secondary), MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.primary), MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact));
			}
			else if (message == EPlayerMessage.TUTORIAL_CROUCH)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Crouch", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.crouch));
			}
			else if (message == EPlayerMessage.TUTORIAL_PRONE)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Prone", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.prone));
			}
			else if (message == EPlayerMessage.TUTORIAL_EDUCATED)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Educated", MenuConfigurationControlsUI.getKeyCodeText(KeyCode.Escape));
			}
			else if (message == EPlayerMessage.TUTORIAL_HARVEST)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Harvest", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact));
			}
			else if (message == EPlayerMessage.TUTORIAL_FISH)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Fish", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.primary));
			}
			else if (message == EPlayerMessage.TUTORIAL_BUILD)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Build");
			}
			else if (message == EPlayerMessage.TUTORIAL_HORN)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Horn", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.primary));
			}
			else if (message == EPlayerMessage.TUTORIAL_LIGHTS)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Lights", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.secondary));
			}
			else if (message == EPlayerMessage.TUTORIAL_SIRENS)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Sirens", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.other));
			}
			else if (message == EPlayerMessage.TUTORIAL_FARM)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Farm");
			}
			else if (message == EPlayerMessage.TUTORIAL_POWER)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Power");
			}
			else if (message == EPlayerMessage.TUTORIAL_FIRE)
			{
				PlayerUI.messageBox.SizeOffset_X = 600f;
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Tutorial_Fire", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.crafting));
			}
			else if (message == EPlayerMessage.CLAIM)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Claim");
			}
			else if (message == EPlayerMessage.UNDERWATER)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Underwater");
			}
			else if (message == EPlayerMessage.NAV)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Nav");
			}
			else if (message == EPlayerMessage.SPAWN)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Spawn");
			}
			else if (message == EPlayerMessage.MOBILE)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Mobile");
			}
			else if (message == EPlayerMessage.BUILD_ON_OCCUPIED_VEHICLE)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Build_On_Occupied_Vehicle");
			}
			else if (message == EPlayerMessage.NOT_ALLOWED_HERE)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Not_Allowed_Here");
			}
			else if (message == EPlayerMessage.CANNOT_BUILD_ON_VEHICLE)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Cannot_Build_On_Vehicle");
			}
			else if (message == EPlayerMessage.TOO_FAR_FROM_HULL)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Too_Far_From_Hull");
			}
			else if (message == EPlayerMessage.CANNOT_BUILD_WHILE_SEATED)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Cannot_Build_While_Seated");
			}
			else if (message == EPlayerMessage.OIL)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Oil");
			}
			else if (message == EPlayerMessage.VOLUME_WATER)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Volume_Water", text);
			}
			else if (message == EPlayerMessage.VOLUME_FUEL)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Volume_Fuel");
			}
			else if (message == EPlayerMessage.TRAPDOOR)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Trapdoor");
			}
			else if (message == EPlayerMessage.TALK)
			{
				InteractableObjectNPC interactableObjectNPC = PlayerInteract.interactable as InteractableObjectNPC;
				string arg = (interactableObjectNPC != null && interactableObjectNPC.npcAsset != null) ? interactableObjectNPC.npcAsset.GetNameShownToPlayer(Player.player) : "null";
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Talk", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact), arg);
			}
			else if (message == EPlayerMessage.CONDITION)
			{
				PlayerUI.messageLabel.Text = text;
			}
			else if (message == EPlayerMessage.INTERACT)
			{
				PlayerUI.messageLabel.Text = string.Format(text, MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact));
			}
			else if (message == EPlayerMessage.SAFEZONE)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Safezone");
			}
			else if (message == EPlayerMessage.CLIMB)
			{
				PlayerUI.messageLabel.Text = PlayerLifeUI.localization.format("Climb", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact));
			}
			PlayerUI.messageBox.PositionOffset_X = -PlayerUI.messageBox.SizeOffset_X / 2f;
			if (transform != null && MainCamera.instance != null)
			{
				PlayerUI.messageBox.PositionOffset_Y = 10f;
				Vector3 v = MainCamera.instance.WorldToViewportPoint(transform.position);
				Vector2 vector = PlayerUI.container.ViewportToNormalizedPosition(v);
				PlayerUI.messageBox.PositionScale_X = vector.x;
				PlayerUI.messageBox.PositionScale_Y = vector.y;
				return;
			}
			if (PlayerUI.messageBox2.IsVisible)
			{
				PlayerUI.messageBox.PositionOffset_Y = -80f - PlayerUI.messageBox.SizeOffset_Y - 10f - PlayerUI.messageBox2.SizeOffset_Y;
			}
			else
			{
				PlayerUI.messageBox.PositionOffset_Y = -80f - PlayerUI.messageBox.SizeOffset_Y;
			}
			PlayerUI.messageBox.PositionScale_X = 0.5f;
			PlayerUI.messageBox.PositionScale_Y = 1f;
		}

		// Token: 0x0600364D RID: 13901 RVA: 0x000FC278 File Offset: 0x000FA478
		public static void hint2(EPlayerMessage message, float progress, float data)
		{
			if (PlayerUI.messageBox2 == null || PlayerLifeUI.localization == null)
			{
				return;
			}
			if (PlayerUI.ShouldIgnoreHintAndMessageRequests())
			{
				return;
			}
			if (!PlayerUI.isMessaged)
			{
				PlayerUI.messageBox2.IsVisible = true;
				PlayerUI.lastHinted2 = true;
				PlayerUI.isHinted2 = true;
				if (message == EPlayerMessage.SALVAGE)
				{
					PlayerUI.messageBox2.SizeOffset_Y = 100f;
					PlayerUI.messageBox2.PositionOffset_Y = -80f - PlayerUI.messageBox2.SizeOffset_Y;
					PlayerUI.messageIcon2.IsVisible = true;
					PlayerUI.messageProgress2_0.IsVisible = true;
					PlayerUI.messageProgress2_1.IsVisible = true;
					PlayerUI.messageIcon2.Texture = PlayerLifeUI.icons.load<Texture2D>("Health");
					PlayerUI.messageLabel2.AllowRichText = false;
					PlayerUI.messageLabel2.TextColor = 3;
					PlayerUI.messageLabel2.TextContrastContext = 0;
					PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Salvage", ControlsSettings.interact);
					PlayerUI.messageProgress2_0.state = progress;
					PlayerUI.messageProgress2_0.color = Palette.COLOR_P;
					PlayerUI.messageProgress2_1.state = data;
					PlayerUI.messageProgress2_1.color = Palette.COLOR_R;
				}
			}
		}

		// Token: 0x0600364E RID: 13902 RVA: 0x000FC3A8 File Offset: 0x000FA5A8
		public static void message(EPlayerMessage message, string text, float duration = 2f)
		{
			if (PlayerUI.messageBox2 == null || PlayerLifeUI.localization == null)
			{
				return;
			}
			if (!OptionsSettings.hints && message != EPlayerMessage.EXPERIENCE && message != EPlayerMessage.MOON_ON && message != EPlayerMessage.MOON_OFF && message != EPlayerMessage.SAFEZONE_ON && message != EPlayerMessage.SAFEZONE_OFF && message != EPlayerMessage.WAVE_ON && message != EPlayerMessage.MOON_OFF && message != EPlayerMessage.DEADZONE_ON && message != EPlayerMessage.DEADZONE_OFF && message != EPlayerMessage.REPUTATION && message != EPlayerMessage.NPC_CUSTOM && message != EPlayerMessage.NOT_PAINTABLE)
			{
				return;
			}
			if (message == EPlayerMessage.NONE)
			{
				PlayerUI.messageBox2.IsVisible = false;
				PlayerUI.messageDisappearTime = 0f;
				PlayerUI.isMessaged = false;
				return;
			}
			if (PlayerUI.ShouldIgnoreHintAndMessageRequests())
			{
				return;
			}
			if ((message == EPlayerMessage.EXPERIENCE || message == EPlayerMessage.REPUTATION) && (PlayerNPCDialogueUI.active || PlayerNPCQuestUI.active || PlayerNPCVendorUI.active))
			{
				return;
			}
			PlayerUI.messageBox2.PositionOffset_X = -200f;
			PlayerUI.messageBox2.SizeOffset_X = 400f;
			PlayerUI.messageBox2.SizeOffset_Y = 50f;
			PlayerUI.messageBox2.PositionOffset_Y = -80f - PlayerUI.messageBox2.SizeOffset_Y;
			PlayerUI.messageBox2.IsVisible = true;
			PlayerUI.messageIcon2.IsVisible = false;
			PlayerUI.messageProgress2_0.IsVisible = false;
			PlayerUI.messageProgress2_1.IsVisible = false;
			PlayerUI.messageDisappearTime = Time.realtimeSinceStartup + duration;
			PlayerUI.isMessaged = true;
			PlayerUI.messageLabel2.AllowRichText = (message == EPlayerMessage.NPC_CUSTOM);
			if (PlayerUI.messageLabel2.AllowRichText)
			{
				PlayerUI.messageLabel2.TextColor = 4;
				PlayerUI.messageLabel2.TextContrastContext = 1;
			}
			else
			{
				PlayerUI.messageLabel2.TextColor = 3;
				PlayerUI.messageLabel2.TextContrastContext = 0;
			}
			if (message == EPlayerMessage.SPACE)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Space");
			}
			if (message == EPlayerMessage.RELOAD)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Reload", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.reload));
				return;
			}
			if (message == EPlayerMessage.SAFETY)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Safety", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.firemode));
				return;
			}
			if (message == EPlayerMessage.VEHICLE_EXIT)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Vehicle_Exit", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.interact));
				return;
			}
			if (message == EPlayerMessage.VEHICLE_SWAP)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Vehicle_Swap", Player.player.movement.getVehicle().passengers.Length);
				return;
			}
			if (message == EPlayerMessage.LIGHT)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Light", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.tactical));
				return;
			}
			if (message == EPlayerMessage.LASER)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Laser", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.tactical));
				return;
			}
			if (message == EPlayerMessage.HOUSING_PLANNER_TUTORIAL)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("HousingPlannerTutorial", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.attach));
				return;
			}
			if (message == EPlayerMessage.RANGEFINDER)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Rangefinder", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.tactical));
				return;
			}
			if (message == EPlayerMessage.EXPERIENCE)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Experience", text);
				return;
			}
			if (message == EPlayerMessage.EMPTY)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Empty");
				return;
			}
			if (message == EPlayerMessage.FULL)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Full");
				return;
			}
			if (message == EPlayerMessage.MOON_ON)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Moon_On");
				return;
			}
			if (message == EPlayerMessage.MOON_OFF)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Moon_Off");
				return;
			}
			if (message == EPlayerMessage.SAFEZONE_ON)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Safezone_On");
				return;
			}
			if (message == EPlayerMessage.SAFEZONE_OFF)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Safezone_Off");
				return;
			}
			if (message == EPlayerMessage.WAVE_ON)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Wave_On");
				return;
			}
			if (message == EPlayerMessage.WAVE_OFF)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Wave_Off");
				return;
			}
			if (message == EPlayerMessage.DEADZONE_ON)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Deadzone_On");
				return;
			}
			if (message == EPlayerMessage.DEADZONE_OFF)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Deadzone_Off");
				return;
			}
			if (message == EPlayerMessage.BUSY)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Busy");
				return;
			}
			if (message == EPlayerMessage.FUEL)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Fuel", text);
				return;
			}
			if (message == EPlayerMessage.CLEAN)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Clean");
				return;
			}
			if (message == EPlayerMessage.SALTY)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Salty");
				return;
			}
			if (message == EPlayerMessage.DIRTY)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Dirty");
				return;
			}
			if (message == EPlayerMessage.REPUTATION)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Reputation", text);
				return;
			}
			if (message == EPlayerMessage.BAYONET)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Bayonet", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.tactical));
				return;
			}
			if (message == EPlayerMessage.VEHICLE_LOCKED)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Vehicle_Locked", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.locker));
				return;
			}
			if (message == EPlayerMessage.VEHICLE_UNLOCKED)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("Vehicle_Unlocked", MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.locker));
				return;
			}
			if (message == EPlayerMessage.NOT_PAINTABLE)
			{
				PlayerUI.messageLabel2.Text = PlayerLifeUI.localization.format("NotPaintable");
				return;
			}
			if (message == EPlayerMessage.NPC_CUSTOM)
			{
				PlayerUI.messageBox2.PositionOffset_X = -300f;
				PlayerUI.messageBox2.SizeOffset_X = 600f;
				RichTextUtil.replaceNewlineMarkup(ref text);
				PlayerUI.messageLabel2.Text = text;
			}
		}

		// Token: 0x0600364F RID: 13903 RVA: 0x000FC979 File Offset: 0x000FAB79
		private void tickIsHallucinating(float deltaTime)
		{
			PlayerUI.hallucinationTimer += deltaTime;
			UnturnedPostProcess.instance.tickIsHallucinating(deltaTime, PlayerUI.hallucinationTimer);
		}

		// Token: 0x06003650 RID: 13904 RVA: 0x000FC998 File Offset: 0x000FAB98
		private void setIsHallucinating(bool isHallucinating)
		{
			if (isHallucinating && (double)Random.value < 0.5)
			{
				float value = Random.value;
				if ((double)value < 0.25)
				{
					this.hallucinationReverbZone.reverbPreset = 24;
				}
				else if ((double)value < 0.5)
				{
					this.hallucinationReverbZone.reverbPreset = 26;
				}
				else if ((double)value < 0.75)
				{
					this.hallucinationReverbZone.reverbPreset = 10;
				}
				else
				{
					this.hallucinationReverbZone.reverbPreset = 22;
				}
				this.hallucinationReverbZone.enabled = true;
			}
			else
			{
				this.hallucinationReverbZone.enabled = false;
			}
			UnturnedPostProcess.instance.setIsHallucinating(isHallucinating);
			if (!isHallucinating)
			{
				PlayerUI.hallucinationTimer = 0f;
			}
		}

		// Token: 0x06003651 RID: 13905 RVA: 0x000FCA55 File Offset: 0x000FAC55
		private void onVisionUpdated(bool isHallucinating)
		{
			this.setIsHallucinating(isHallucinating);
		}

		// Token: 0x06003652 RID: 13906 RVA: 0x000FCA60 File Offset: 0x000FAC60
		private void onLifeUpdated(bool isDead)
		{
			PlayerUI.isLocked = false;
			PlayerUI.inputWantsCustomModal = false;
			PlayerUI.usingCustomModal = false;
			MenuConfigurationOptionsUI.close();
			MenuConfigurationDisplayUI.close();
			MenuConfigurationGraphicsUI.close();
			MenuConfigurationControlsUI.close();
			PlayerPauseUI.audioMenu.close();
			PlayerPauseUI.close();
			PlayerDashboardUI.close();
			PlayerBarricadeSignUI.close();
			this.boomboxUI.close();
			PlayerBarricadeLibraryUI.close();
			this.mannequinUI.close();
			this.browserRequestUI.close();
			PlayerNPCDialogueUI.close();
			PlayerNPCQuestUI.close();
			PlayerNPCVendorUI.close();
			PlayerWorkzoneUI.close();
			if (isDead)
			{
				PlayerLifeUI.close();
				PlayerDeathUI.open(true);
				return;
			}
			PlayerDeathUI.close();
			PlayerLifeUI.open();
		}

		// Token: 0x06003653 RID: 13907 RVA: 0x000FCAFF File Offset: 0x000FACFF
		private void onGlassesUpdated(ushort newGlasses, byte newGlassesQuality, byte[] newGlassesState)
		{
			PlayerUI.isBlindfolded = (Player.player.clothing.glassesAsset != null && Player.player.clothing.glassesAsset.isBlindfold);
		}

		// Token: 0x06003654 RID: 13908 RVA: 0x000FCB2E File Offset: 0x000FAD2E
		private void onMoonUpdated(bool isFullMoon)
		{
			if (isFullMoon)
			{
				PlayerUI.message(EPlayerMessage.MOON_ON, "", 2f);
				return;
			}
			PlayerUI.message(EPlayerMessage.MOON_OFF, "", 2f);
		}

		// Token: 0x06003655 RID: 13909 RVA: 0x000FCB56 File Offset: 0x000FAD56
		private void OnEnable()
		{
			PlayerUI.instance = this;
			base.useGUILayout = false;
		}

		// Token: 0x06003656 RID: 13910 RVA: 0x000FCB65 File Offset: 0x000FAD65
		internal void Player_OnGUI()
		{
			if (PlayerUI.window != null)
			{
				Glazier.Get().Root = PlayerUI.window;
			}
		}

		// Token: 0x06003657 RID: 13911 RVA: 0x000FCB80 File Offset: 0x000FAD80
		private void OnGUI()
		{
			if (PlayerUI.window == null)
			{
				return;
			}
			if (Event.current.isKey && Event.current.type == 5)
			{
				if (Event.current.keyCode == KeyCode.UpArrow)
				{
					if (PlayerLifeUI.chatting)
					{
						PlayerLifeUI.repeatChat(1);
					}
				}
				else if (Event.current.keyCode == KeyCode.DownArrow)
				{
					if (PlayerLifeUI.chatting)
					{
						PlayerLifeUI.repeatChat(-1);
					}
				}
				else if (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter)
				{
					if (PlayerLifeUI.chatting)
					{
						PlayerLifeUI.SendChatAndClose();
					}
					else if (PlayerLifeUI.active && this.canOpenMenus)
					{
						PlayerLifeUI.openChat();
					}
				}
				else if (Event.current.keyCode == ControlsSettings.global)
				{
					if (PlayerLifeUI.active && this.canOpenMenus)
					{
						PlayerUI.chat = EChatMode.GLOBAL;
						PlayerLifeUI.openChat();
					}
				}
				else if (Event.current.keyCode == ControlsSettings.local)
				{
					if (PlayerLifeUI.active && this.canOpenMenus)
					{
						PlayerUI.chat = EChatMode.LOCAL;
						PlayerLifeUI.openChat();
					}
				}
				else if (Event.current.keyCode == ControlsSettings.group && PlayerLifeUI.active && this.canOpenMenus)
				{
					PlayerUI.chat = EChatMode.GROUP;
					PlayerLifeUI.openChat();
				}
			}
			if (PlayerLifeUI.chatting)
			{
				PlayerLifeUI.chatField.FocusControl();
			}
			MenuConfigurationControlsUI.bindOnGUI();
		}

		// Token: 0x06003658 RID: 13912 RVA: 0x000FCCEC File Offset: 0x000FAEEC
		private void escapeMenu()
		{
			if (MenuConfigurationOptionsUI.active)
			{
				MenuConfigurationOptionsUI.close();
				PlayerPauseUI.open();
				return;
			}
			if (MenuConfigurationDisplayUI.active)
			{
				MenuConfigurationDisplayUI.close();
				PlayerPauseUI.open();
				return;
			}
			if (MenuConfigurationGraphicsUI.active)
			{
				MenuConfigurationGraphicsUI.close();
				PlayerPauseUI.open();
				return;
			}
			if (MenuConfigurationControlsUI.active)
			{
				MenuConfigurationControlsUI.close();
				PlayerPauseUI.open();
				return;
			}
			if (PlayerPauseUI.audioMenu.active)
			{
				PlayerPauseUI.audioMenu.close();
				PlayerPauseUI.open();
				return;
			}
			if (PlayerPauseUI.active)
			{
				PlayerPauseUI.closeAndGotoAppropriateHUD();
				return;
			}
			if (PlayerDashboardUI.active && PlayerDashboardInventoryUI.active)
			{
				if (PlayerDashboardInventoryUI.isDragging)
				{
					PlayerDashboardInventoryUI.stopDrag();
					return;
				}
				if (PlayerDashboardInventoryUI.selectedPage != 255)
				{
					PlayerDashboardInventoryUI.closeSelection();
					return;
				}
			}
			bool flag = true;
			if (PlayerDashboardUI.active)
			{
				PlayerDashboardUI.close();
			}
			else if (PlayerBarricadeSignUI.active)
			{
				PlayerBarricadeSignUI.close();
			}
			else if (this.boomboxUI.active)
			{
				this.boomboxUI.close();
			}
			else if (PlayerBarricadeLibraryUI.active)
			{
				PlayerBarricadeLibraryUI.close();
			}
			else if (this.mannequinUI.active)
			{
				this.mannequinUI.close();
			}
			else if (this.browserRequestUI.isActive)
			{
				this.browserRequestUI.close();
			}
			else if (PlayerNPCDialogueUI.active)
			{
				PlayerNPCDialogueUI.close();
			}
			else if (PlayerWorkzoneUI.active)
			{
				PlayerWorkzoneUI.close();
			}
			else
			{
				flag = false;
			}
			if (flag)
			{
				if (Player.player.life.isDead)
				{
					PlayerDeathUI.open(false);
					return;
				}
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
				if (Player.player.equipment.isUseableShowingMenu)
				{
					return;
				}
				PlayerDeathUI.close();
				PlayerLifeUI.close();
				PlayerPauseUI.open();
				return;
			}
		}

		/// <summary>
		/// Adjust screen positioning and visibility of player name widgets to match their world-space counterparts.
		/// </summary>
		// Token: 0x06003659 RID: 13913 RVA: 0x000FCE90 File Offset: 0x000FB090
		private void updateGroupLabels()
		{
			if (Player.player == null || MainCamera.instance == null)
			{
				return;
			}
			if (this.groupUI.groups == null || this.groupUI.groups.Count != Provider.clients.Count)
			{
				return;
			}
			Camera camera = MainCamera.instance;
			bool areSpecStatsVisible = Player.player.look.areSpecStatsVisible;
			for (int i = 0; i < this.groupUI.groups.Count; i++)
			{
				ISleekLabel sleekLabel = this.groupUI.groups[i];
				SteamPlayer steamPlayer = Provider.clients[i];
				if (sleekLabel != null && steamPlayer != null && !(steamPlayer.model == null))
				{
					bool flag;
					if (areSpecStatsVisible)
					{
						flag = true;
					}
					else if (Provider.modeConfigData.Gameplay.Group_HUD)
					{
						bool flag2 = steamPlayer.playerID.steamID != Provider.client;
						bool flag3 = steamPlayer.player.quests.isMemberOfSameGroupAs(Player.player);
						flag = (flag2 && flag3);
					}
					else
					{
						flag = false;
					}
					if (!flag)
					{
						sleekLabel.IsVisible = false;
					}
					else if ((steamPlayer.model.position - camera.transform.position).sqrMagnitude > 262144f)
					{
						sleekLabel.IsVisible = false;
					}
					else
					{
						Vector3 vector = camera.WorldToViewportPoint(steamPlayer.model.position + Vector3.up * 3f);
						if (vector.z <= 0f)
						{
							sleekLabel.IsVisible = false;
						}
						else
						{
							Vector2 vector2 = this.groupUI.ViewportToNormalizedPosition(vector);
							sleekLabel.PositionScale_X = vector2.x;
							sleekLabel.PositionScale_Y = vector2.y;
							float num;
							if (areSpecStatsVisible)
							{
								num = 1f;
							}
							else if (!OptionsSettings.shouldNametagFadeOut)
							{
								num = 0.75f;
							}
							else
							{
								float magnitude = new Vector2(vector2.x - 0.5f, vector2.y - 0.5f).magnitude;
								float t = Mathf.InverseLerp(0.05f, 0.1f, magnitude);
								num = Mathf.Lerp(0.1f, 0.75f, t);
							}
							sleekLabel.TextColor = new SleekColor(3, num);
							if (!sleekLabel.IsVisible)
							{
								if (steamPlayer.isMemberOfSameGroupAs(Player.player) && !string.IsNullOrEmpty(steamPlayer.playerID.nickName))
								{
									sleekLabel.Text = steamPlayer.playerID.nickName;
								}
								else
								{
									sleekLabel.Text = steamPlayer.playerID.characterName;
								}
							}
							sleekLabel.IsVisible = true;
						}
					}
				}
			}
		}

		/// <summary>
		/// Update hitmarker visibility, and their world-space positions if user enabled that.
		/// </summary>
		// Token: 0x0600365A RID: 13914 RVA: 0x000FD138 File Offset: 0x000FB338
		private void updateHitmarkers()
		{
			if (PlayerLifeUI.activeHitmarkers == null || MainCamera.instance == null)
			{
				return;
			}
			float deltaTime = Time.deltaTime;
			for (int i = PlayerLifeUI.activeHitmarkers.Count - 1; i >= 0; i--)
			{
				HitmarkerInfo hitmarkerInfo = PlayerLifeUI.activeHitmarkers[i];
				if (hitmarkerInfo.aliveTime > PlayerUI.HIT_TIME)
				{
					PlayerLifeUI.ReleaseHitmarker(hitmarkerInfo.sleekElement);
					PlayerLifeUI.activeHitmarkers.RemoveAtFast(i);
				}
				else
				{
					hitmarkerInfo.aliveTime += deltaTime;
					PlayerLifeUI.activeHitmarkers[i] = hitmarkerInfo;
					Vector2 vector2;
					bool isVisible;
					if (hitmarkerInfo.shouldFollowWorldPosition)
					{
						Vector3 vector = MainCamera.instance.WorldToViewportPoint(hitmarkerInfo.worldPosition);
						vector2 = PlayerUI.window.ViewportToNormalizedPosition(vector);
						isVisible = (vector.z > 0f);
					}
					else
					{
						vector2 = new Vector3(0.5f, 0.5f);
						isVisible = true;
					}
					hitmarkerInfo.sleekElement.PositionScale_X = vector2.x;
					hitmarkerInfo.sleekElement.PositionScale_Y = vector2.y;
					hitmarkerInfo.sleekElement.IsVisible = isVisible;
				}
			}
		}

		/// <summary>
		/// Disable hints and messages if no longer applicable.
		/// </summary>
		// Token: 0x0600365B RID: 13915 RVA: 0x000FD250 File Offset: 0x000FB450
		private void updateHintsAndMessages()
		{
			if (PlayerUI.isHinted)
			{
				if (!PlayerUI.lastHinted)
				{
					PlayerUI.isHinted = false;
					if (PlayerUI.messageBox != null)
					{
						PlayerUI.messageBox.IsVisible = false;
					}
					if (PlayerUI.messagePlayer != null)
					{
						PlayerUI.messagePlayer.IsVisible = false;
					}
				}
				PlayerUI.lastHinted = false;
			}
			if (PlayerUI.isMessaged)
			{
				if (Time.realtimeSinceStartup > PlayerUI.messageDisappearTime)
				{
					PlayerUI.isMessaged = false;
					if (!PlayerUI.isHinted2 && PlayerUI.messageBox2 != null)
					{
						PlayerUI.messageBox2.IsVisible = false;
						return;
					}
				}
			}
			else if (PlayerUI.isHinted2)
			{
				if (!PlayerUI.lastHinted2)
				{
					PlayerUI.isHinted2 = false;
					if (PlayerUI.messageBox2 != null)
					{
						PlayerUI.messageBox2.IsVisible = false;
					}
				}
				PlayerUI.lastHinted2 = false;
			}
		}

		/// <summary>
		/// Disable vote popup if enough time has passed.
		/// </summary>
		// Token: 0x0600365C RID: 13916 RVA: 0x000FD2FA File Offset: 0x000FB4FA
		private void updateVoteDisplay()
		{
			if (PlayerLifeUI.isVoteMessaged && Time.realtimeSinceStartup - PlayerLifeUI.lastVoteMessage > 2f)
			{
				PlayerLifeUI.isVoteMessaged = false;
				if (PlayerLifeUI.voteBox != null)
				{
					PlayerLifeUI.voteBox.IsVisible = false;
				}
			}
		}

		/// <summary>
		/// Pause the game if playing singleplayer and menu is open.
		/// </summary>
		// Token: 0x0600365D RID: 13917 RVA: 0x000FD330 File Offset: 0x000FB530
		private void updatePauseTimeScale()
		{
			if (Provider.isServer && (MenuConfigurationOptionsUI.active || MenuConfigurationDisplayUI.active || MenuConfigurationGraphicsUI.active || MenuConfigurationControlsUI.active || PlayerPauseUI.audioMenu.active || PlayerPauseUI.active))
			{
				Time.timeScale = 0f;
				AudioListener.pause = true;
				return;
			}
			Time.timeScale = 1f;
			AudioListener.pause = false;
		}

		// Token: 0x0600365E RID: 13918 RVA: 0x000FD394 File Offset: 0x000FB594
		private void tickDeathTimers()
		{
			if (!PlayerDeathUI.active)
			{
				return;
			}
			if (PlayerDeathUI.homeButton != null)
			{
				if (!Provider.isServer && Provider.isPvP)
				{
					if (Time.realtimeSinceStartup - Player.player.life.lastDeath < Provider.modeConfigData.Gameplay.Timer_Home)
					{
						PlayerDeathUI.homeButton.text = PlayerDeathUI.localization.format("Home_Button_Timer", Mathf.Ceil(Provider.modeConfigData.Gameplay.Timer_Home - (Time.realtimeSinceStartup - Player.player.life.lastDeath)));
					}
					else
					{
						PlayerDeathUI.homeButton.text = PlayerDeathUI.localization.format("Home_Button");
					}
				}
				else if (Time.realtimeSinceStartup - Player.player.life.lastRespawn < Provider.modeConfigData.Gameplay.Timer_Respawn)
				{
					PlayerDeathUI.homeButton.text = PlayerDeathUI.localization.format("Home_Button_Timer", Mathf.Ceil(Provider.modeConfigData.Gameplay.Timer_Respawn - (Time.realtimeSinceStartup - Player.player.life.lastRespawn)));
				}
				else
				{
					PlayerDeathUI.homeButton.text = PlayerDeathUI.localization.format("Home_Button");
				}
			}
			if (PlayerDeathUI.respawnButton != null)
			{
				if (Time.realtimeSinceStartup - Player.player.life.lastRespawn < Provider.modeConfigData.Gameplay.Timer_Respawn)
				{
					PlayerDeathUI.respawnButton.text = PlayerDeathUI.localization.format("Respawn_Button_Timer", Mathf.Ceil(Provider.modeConfigData.Gameplay.Timer_Respawn - (Time.realtimeSinceStartup - Player.player.life.lastRespawn)));
					return;
				}
				PlayerDeathUI.respawnButton.text = PlayerDeathUI.localization.format("Respawn_Button");
			}
		}

		// Token: 0x0600365F RID: 13919 RVA: 0x000FD580 File Offset: 0x000FB780
		private void tickExitTimer()
		{
			if (!PlayerPauseUI.active)
			{
				return;
			}
			if (PlayerPauseUI.exitButton != null)
			{
				if (PlayerPauseUI.shouldExitButtonRespectTimer && Time.realtimeSinceStartup - PlayerPauseUI.lastLeave < Provider.modeConfigData.Gameplay.Timer_Exit)
				{
					PlayerPauseUI.exitButton.text = PlayerPauseUI.localization.format("Exit_Button_Timer", Mathf.Ceil(Provider.modeConfigData.Gameplay.Timer_Exit - (Time.realtimeSinceStartup - PlayerPauseUI.lastLeave)));
				}
				else
				{
					PlayerPauseUI.exitButton.text = PlayerPauseUI.localization.format("Exit_Button_Text");
				}
			}
			if (PlayerPauseUI.quitButton != null)
			{
				if (PlayerPauseUI.shouldExitButtonRespectTimer && Time.realtimeSinceStartup - PlayerPauseUI.lastLeave < Provider.modeConfigData.Gameplay.Timer_Exit)
				{
					PlayerPauseUI.quitButton.text = PlayerPauseUI.localization.format("Quit_Button_Timer", Mathf.Ceil(Provider.modeConfigData.Gameplay.Timer_Exit - (Time.realtimeSinceStartup - PlayerPauseUI.lastLeave)));
					return;
				}
				PlayerPauseUI.quitButton.text = PlayerPauseUI.localization.format("Quit_Button");
			}
		}

		/// <summary>
		/// Many places checked that the cursor and chat were closed to see if a menu could be opened. Moved here to
		/// also consider that useable might have a menu open.
		/// </summary>
		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x06003660 RID: 13920 RVA: 0x000FD6A5 File Offset: 0x000FB8A5
		private bool canOpenMenus
		{
			get
			{
				return (!(Player.player != null) || !Player.player.equipment.isUseableShowingMenu) && !PlayerUI.window.showCursor && !PlayerLifeUI.chatting;
			}
		}

		// Token: 0x06003661 RID: 13921 RVA: 0x000FD6E0 File Offset: 0x000FB8E0
		private void tickInput()
		{
			PlayerUI.inputWantsCustomModal = false;
			if (MenuConfigurationControlsUI.binding != 255)
			{
				return;
			}
			if ((InputEx.GetKeyDown(ControlsSettings.left) || InputEx.GetKeyDown(ControlsSettings.up) || InputEx.GetKeyDown(ControlsSettings.right) || InputEx.GetKeyDown(ControlsSettings.down)) && PlayerDashboardUI.active)
			{
				PlayerDashboardUI.close();
				if (Player.player.life.IsAlive)
				{
					PlayerLifeUI.open();
				}
			}
			if (PlayerLifeUI.chatting && Input.GetKeyDown(KeyCode.Escape))
			{
				PlayerLifeUI.closeChat();
			}
			else if (InputEx.ConsumeKeyDown(KeyCode.Escape))
			{
				this.escapeMenu();
			}
			if (Player.player.life.IsAlive)
			{
				if (InputEx.ConsumeKeyDown(ControlsSettings.dashboard))
				{
					if (PlayerDashboardUI.active)
					{
						PlayerDashboardUI.close();
						PlayerLifeUI.open();
					}
					else if (PlayerBarricadeSignUI.active)
					{
						PlayerBarricadeSignUI.close();
						PlayerLifeUI.open();
					}
					else if (this.boomboxUI.active)
					{
						this.boomboxUI.close();
						PlayerLifeUI.open();
					}
					else if (PlayerBarricadeLibraryUI.active)
					{
						PlayerBarricadeLibraryUI.close();
						PlayerLifeUI.open();
					}
					else if (this.mannequinUI.active)
					{
						this.mannequinUI.close();
						PlayerLifeUI.open();
					}
					else if (PlayerNPCDialogueUI.active)
					{
						PlayerNPCDialogueUI.close();
						PlayerLifeUI.open();
					}
					else if (PlayerNPCQuestUI.active)
					{
						PlayerNPCQuestUI.closeNicely();
					}
					else if (PlayerNPCVendorUI.active)
					{
						PlayerNPCVendorUI.closeNicely();
					}
					else if (this.canOpenMenus)
					{
						PlayerLifeUI.close();
						PlayerPauseUI.close();
						PlayerDashboardUI.open();
					}
				}
				if (InputEx.ConsumeKeyDown(ControlsSettings.inventory))
				{
					if (PlayerDashboardUI.active && PlayerDashboardInventoryUI.active)
					{
						PlayerDashboardUI.close();
						PlayerLifeUI.open();
					}
					else if (PlayerDashboardUI.active)
					{
						PlayerDashboardCraftingUI.close();
						PlayerDashboardSkillsUI.close();
						PlayerDashboardInformationUI.close();
						PlayerDashboardInventoryUI.open();
					}
					else if (this.canOpenMenus)
					{
						PlayerLifeUI.close();
						PlayerPauseUI.close();
						PlayerDashboardInventoryUI.active = true;
						PlayerDashboardCraftingUI.active = false;
						PlayerDashboardSkillsUI.active = false;
						PlayerDashboardInformationUI.active = false;
						PlayerDashboardUI.open();
					}
				}
				if (InputEx.ConsumeKeyDown(ControlsSettings.crafting) && Level.info != null && Level.info.type != ELevelType.HORDE && Level.info.configData.Allow_Crafting)
				{
					if (PlayerDashboardUI.active && PlayerDashboardCraftingUI.active)
					{
						PlayerDashboardUI.close();
						PlayerLifeUI.open();
					}
					else if (PlayerDashboardUI.active)
					{
						PlayerDashboardInventoryUI.close();
						PlayerDashboardSkillsUI.close();
						PlayerDashboardInformationUI.close();
						PlayerDashboardCraftingUI.open();
					}
					else if (this.canOpenMenus)
					{
						PlayerLifeUI.close();
						PlayerPauseUI.close();
						PlayerDashboardInventoryUI.active = false;
						PlayerDashboardCraftingUI.active = true;
						PlayerDashboardSkillsUI.active = false;
						PlayerDashboardInformationUI.active = false;
						PlayerDashboardUI.open();
					}
				}
				if (InputEx.ConsumeKeyDown(ControlsSettings.skills) && Level.info != null && Level.info.type != ELevelType.HORDE && Level.info.configData.Allow_Skills)
				{
					if (PlayerDashboardUI.active && PlayerDashboardSkillsUI.active)
					{
						PlayerDashboardUI.close();
						PlayerLifeUI.open();
					}
					else if (PlayerDashboardUI.active)
					{
						PlayerDashboardInventoryUI.close();
						PlayerDashboardCraftingUI.close();
						PlayerDashboardInformationUI.close();
						PlayerDashboardSkillsUI.open();
					}
					else if (this.canOpenMenus)
					{
						PlayerLifeUI.close();
						PlayerPauseUI.close();
						PlayerDashboardInventoryUI.active = false;
						PlayerDashboardCraftingUI.active = false;
						PlayerDashboardSkillsUI.active = true;
						PlayerDashboardInformationUI.active = false;
						PlayerDashboardUI.open();
					}
				}
				if ((InputEx.ConsumeKeyDown(ControlsSettings.map) || InputEx.ConsumeKeyDown(ControlsSettings.quests) || InputEx.ConsumeKeyDown(ControlsSettings.players)) && Level.info != null && Level.info.configData.Allow_Information)
				{
					if (PlayerDashboardUI.active && PlayerDashboardInformationUI.active)
					{
						PlayerDashboardUI.close();
						PlayerLifeUI.open();
					}
					else
					{
						if (InputEx.GetKeyDown(ControlsSettings.quests))
						{
							PlayerDashboardInformationUI.openQuests();
						}
						else if (InputEx.GetKeyDown(ControlsSettings.players))
						{
							PlayerDashboardInformationUI.openPlayers();
						}
						if (PlayerDashboardUI.active)
						{
							PlayerDashboardInventoryUI.close();
							PlayerDashboardCraftingUI.close();
							PlayerDashboardSkillsUI.close();
							PlayerDashboardInformationUI.open();
						}
						else if (this.canOpenMenus)
						{
							PlayerLifeUI.close();
							PlayerPauseUI.close();
							PlayerDashboardInventoryUI.active = false;
							PlayerDashboardCraftingUI.active = false;
							PlayerDashboardSkillsUI.active = false;
							PlayerDashboardInformationUI.active = true;
							PlayerDashboardUI.open();
						}
					}
				}
				if (InputEx.ConsumeKeyDown(ControlsSettings.gesture))
				{
					if (PlayerLifeUI.active && this.canOpenMenus)
					{
						PlayerLifeUI.openGestures();
					}
				}
				else if (InputEx.GetKeyUp(ControlsSettings.gesture) && PlayerLifeUI.active)
				{
					PlayerLifeUI.closeGestures();
				}
			}
			if (PlayerUI.window != null)
			{
				if (InputEx.GetKeyDown(ControlsSettings.screenshot))
				{
					Provider.RequestScreenshot();
				}
				if (InputEx.GetKeyDown(ControlsSettings.hud))
				{
					PlayerUI.wantsWindowEnabled = !PlayerUI.wantsWindowEnabled;
					PlayerUI.window.drawCursorWhileDisabled = false;
					PlayerUI.UpdateWindowEnabled();
				}
				InputEx.GetKeyDown(ControlsSettings.terminal);
			}
			if (InputEx.GetKeyDown(ControlsSettings.refreshAssets) && Provider.isServer)
			{
				Assets.RequestReloadAllAssets();
			}
			if (InputEx.GetKeyDown(ControlsSettings.clipboardDebug))
			{
				string text = string.Empty;
				for (int i = 0; i < Player.player.quests.flagsList.Count; i++)
				{
					if (i > 0)
					{
						text += "\n";
					}
					text += string.Format("{0, 5} {1, 5}", Player.player.quests.flagsList[i].id, Player.player.quests.flagsList[i].value);
				}
				GUIUtility.systemCopyBuffer = text;
			}
			PlayerUI.inputWantsCustomModal = InputEx.GetKey(ControlsSettings.CustomModal);
		}

		// Token: 0x06003662 RID: 13922 RVA: 0x000FDC20 File Offset: 0x000FBE20
		private void tickMenuBlur()
		{
			if (this.menuBlurFX == null)
			{
				return;
			}
			EPluginWidgetFlags pluginWidgetFlags = Player.player.pluginWidgetFlags;
			bool flag = (pluginWidgetFlags & EPluginWidgetFlags.ForceBlur) == EPluginWidgetFlags.ForceBlur || ((pluginWidgetFlags & EPluginWidgetFlags.NoBlur) != EPluginWidgetFlags.NoBlur && ((PlayerUI.window.showCursor && !PlayerUI.usingCustomModal && !MenuConfigurationGraphicsUI.active && !PlayerNPCDialogueUI.active && !PlayerNPCQuestUI.active && !PlayerNPCVendorUI.active && !PlayerWorkzoneUI.active) || (WaterUtility.isPointUnderwater(MainCamera.instance.transform.position) && (Player.player.clothing.glassesAsset == null || !Player.player.clothing.glassesAsset.proofWater)) || (Player.player.look.isScopeActive && GraphicsSettings.scopeQuality != EGraphicQuality.OFF && Player.player.look.perspective == EPlayerPerspective.FIRST && Player.player.equipment.useable != null && ((UseableGun)Player.player.equipment.useable).isAiming)));
			if (this.menuBlurFX.enabled != flag)
			{
				this.menuBlurFX.enabled = flag;
			}
		}

		// Token: 0x06003663 RID: 13923 RVA: 0x000FDD54 File Offset: 0x000FBF54
		private void UpdateOverlayColor()
		{
			Color color;
			float num;
			if (PlayerUI.isBlindfolded)
			{
				color = Color.black;
				num = 1f;
			}
			else
			{
				color = PlayerUI.stunColor;
				num = PlayerUI.stunAlpha;
			}
			color = Color.Lerp(color, Palette.COLOR_R, PlayerUI.painAlpha + (1f - num));
			color.a = Mathf.Max(num, PlayerUI.painAlpha);
			PlayerUI.colorOverlayImage.TintColor = color;
			if (PlayerUI.isWindowEnabledByColorOverlay && PlayerUI.stunAlpha < 0.001f && PlayerUI.painAlpha < 0.001f)
			{
				PlayerUI.isWindowEnabledByColorOverlay = false;
				PlayerUI.UpdateWindowEnabled();
			}
		}

		// Token: 0x06003664 RID: 13924 RVA: 0x000FDDE8 File Offset: 0x000FBFE8
		private void Update()
		{
			if (PlayerUI.window == null)
			{
				return;
			}
			MenuConfigurationControlsUI.bindUpdate();
			PlayerDashboardInventoryUI.updateDraggedItem();
			PlayerDashboardInventoryUI.updateNearbyDrops();
			this.updateGroupLabels();
			PlayerLifeUI.updateCompass();
			PlayerLifeUI.updateHotbar();
			PlayerLifeUI.updateStatTracker();
			PlayerNPCVendorUI.MaybeRefresh();
			this.UpdateOverlayColor();
			PlayerUI.painAlpha = Mathf.Max(0f, PlayerUI.painAlpha - Time.deltaTime);
			PlayerUI.stunAlpha = Mathf.Max(0f, PlayerUI.stunAlpha - Time.deltaTime);
			this.updateHitmarkers();
			this.updateHintsAndMessages();
			this.updateVoteDisplay();
			this.updatePauseTimeScale();
			this.tickDeathTimers();
			this.tickExitTimer();
			if (PlayerNPCDialogueUI.active)
			{
				PlayerNPCDialogueUI.UpdateAnimation();
			}
			if (PlayerDashboardInformationUI.active)
			{
				PlayerDashboardInformationUI.updateDynamicMap();
			}
			this.tickInput();
			bool flag = Player.player.inPluginModal || PlayerPauseUI.active || MenuConfigurationOptionsUI.active || MenuConfigurationDisplayUI.active || MenuConfigurationGraphicsUI.active || MenuConfigurationControlsUI.active || PlayerPauseUI.audioMenu.active || PlayerDashboardUI.active || PlayerDeathUI.active || PlayerLifeUI.chatting || PlayerLifeUI.gesturing || PlayerBarricadeSignUI.active || this.boomboxUI.active || PlayerBarricadeLibraryUI.active || this.mannequinUI.active || this.browserRequestUI.isActive || PlayerNPCDialogueUI.active || PlayerNPCQuestUI.active || PlayerNPCVendorUI.active || (PlayerWorkzoneUI.active && !InputEx.GetKey(ControlsSettings.secondary)) || PlayerUI.isLocked;
			PlayerUI.usingCustomModal = (!flag & PlayerUI.inputWantsCustomModal);
			flag |= PlayerUI.inputWantsCustomModal;
			PlayerUI.window.showCursor = flag;
			this.tickMenuBlur();
			if (Player.player.life.vision > 0)
			{
				this.tickIsHallucinating(Time.deltaTime);
			}
		}

		// Token: 0x06003665 RID: 13925 RVA: 0x000FDFBC File Offset: 0x000FC1BC
		internal void InitializePlayer()
		{
			PlayerUI.isLocked = false;
			PlayerUI.inputWantsCustomModal = false;
			PlayerUI.usingCustomModal = false;
			PlayerUI.chat = EChatMode.GLOBAL;
			PlayerUI.window = new SleekWindow();
			if (Player.player.channel.owner.playerID.BypassIntegrityChecks)
			{
				ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
				sleekLabel.SizeOffset_X = 200f;
				sleekLabel.SizeOffset_Y = 30f;
				sleekLabel.PositionOffset_X = -100f;
				sleekLabel.PositionOffset_Y = -15f;
				sleekLabel.PositionScale_X = 0.5f;
				sleekLabel.PositionScale_Y = 0.2f;
				sleekLabel.TextColor = 6;
				sleekLabel.Text = "Bypassing integrity checks";
				sleekLabel.TextContrastContext = 2;
				PlayerUI.window.AddChild(sleekLabel);
			}
			PlayerUI.colorOverlayImage = Glazier.Get().CreateImage();
			PlayerUI.colorOverlayImage.SizeScale_X = 1f;
			PlayerUI.colorOverlayImage.SizeScale_Y = 1f;
			PlayerUI.colorOverlayImage.Texture = GlazierResources.PixelTexture;
			PlayerUI.colorOverlayImage.TintColor = new Color(0f, 0f, 0f, 0f);
			PlayerUI.window.AddChild(PlayerUI.colorOverlayImage);
			PlayerUI.container = Glazier.Get().CreateFrame();
			PlayerUI.container.SizeScale_X = 1f;
			PlayerUI.container.SizeScale_Y = 1f;
			PlayerUI.window.AddChild(PlayerUI.container);
			PlayerUI.wantsWindowEnabled = true;
			PlayerUI.isWindowEnabledByColorOverlay = false;
			OptionsSettings.apply();
			GraphicsSettings.apply("loaded player");
			this.groupUI = new PlayerGroupUI();
			this.groupUI.SizeScale_X = 1f;
			this.groupUI.SizeScale_Y = 1f;
			PlayerUI.container.AddChild(this.groupUI);
			this.dashboardUI = new PlayerDashboardUI();
			this.pauseUI = new PlayerPauseUI();
			this.lifeUI = new PlayerLifeUI();
			new PlayerDeathUI();
			new PlayerBarricadeSignUI();
			this.boomboxUI = new PlayerBarricadeStereoUI();
			PlayerUI.container.AddChild(this.boomboxUI);
			new PlayerBarricadeLibraryUI();
			this.mannequinUI = new PlayerBarricadeMannequinUI();
			PlayerUI.container.AddChild(this.mannequinUI);
			this.browserRequestUI = new PlayerBrowserRequestUI();
			PlayerUI.container.AddChild(this.browserRequestUI);
			new PlayerNPCDialogueUI();
			new PlayerNPCQuestUI();
			new PlayerNPCVendorUI();
			new PlayerWorkzoneUI();
			PlayerLifeUI.UpdateTrackedQuest();
			PlayerUI.messagePlayer = null;
			PlayerUI.messageBox = Glazier.Get().CreateBox();
			PlayerUI.messageBox.PositionOffset_X = -200f;
			PlayerUI.messageBox.PositionScale_X = 0.5f;
			PlayerUI.messageBox.PositionScale_Y = 1f;
			PlayerUI.messageBox.SizeOffset_X = 400f;
			PlayerUI.container.AddChild(PlayerUI.messageBox);
			PlayerUI.messageBox.IsVisible = false;
			PlayerUI.messageLabel = Glazier.Get().CreateLabel();
			PlayerUI.messageLabel.PositionOffset_X = 5f;
			PlayerUI.messageLabel.PositionOffset_Y = 5f;
			PlayerUI.messageLabel.SizeOffset_X = -10f;
			PlayerUI.messageLabel.SizeOffset_Y = 40f;
			PlayerUI.messageLabel.SizeScale_X = 1f;
			PlayerUI.messageLabel.FontSize = 3;
			PlayerUI.messageBox.AddChild(PlayerUI.messageLabel);
			PlayerUI.messageIcon_0 = Glazier.Get().CreateImage();
			PlayerUI.messageIcon_0.PositionOffset_X = 5f;
			PlayerUI.messageIcon_0.PositionOffset_Y = 45f;
			PlayerUI.messageIcon_0.SizeOffset_X = 20f;
			PlayerUI.messageIcon_0.SizeOffset_Y = 20f;
			PlayerUI.messageBox.AddChild(PlayerUI.messageIcon_0);
			PlayerUI.messageIcon_0.IsVisible = false;
			PlayerUI.messageIcon_1 = Glazier.Get().CreateImage();
			PlayerUI.messageIcon_1.PositionOffset_X = 5f;
			PlayerUI.messageIcon_1.PositionOffset_Y = 75f;
			PlayerUI.messageIcon_1.SizeOffset_X = 20f;
			PlayerUI.messageIcon_1.SizeOffset_Y = 20f;
			PlayerUI.messageBox.AddChild(PlayerUI.messageIcon_1);
			PlayerUI.messageIcon_1.IsVisible = false;
			PlayerUI.messageIcon_2 = Glazier.Get().CreateImage();
			PlayerUI.messageIcon_2.PositionOffset_X = 5f;
			PlayerUI.messageIcon_2.PositionOffset_Y = 105f;
			PlayerUI.messageIcon_2.SizeOffset_X = 20f;
			PlayerUI.messageIcon_2.SizeOffset_Y = 20f;
			PlayerUI.messageBox.AddChild(PlayerUI.messageIcon_2);
			PlayerUI.messageIcon_2.IsVisible = false;
			PlayerUI.messageProgress_0 = new SleekProgress("");
			PlayerUI.messageProgress_0.PositionOffset_X = 30f;
			PlayerUI.messageProgress_0.PositionOffset_Y = 50f;
			PlayerUI.messageProgress_0.SizeOffset_X = -40f;
			PlayerUI.messageProgress_0.SizeOffset_Y = 10f;
			PlayerUI.messageProgress_0.SizeScale_X = 1f;
			PlayerUI.messageBox.AddChild(PlayerUI.messageProgress_0);
			PlayerUI.messageProgress_0.IsVisible = false;
			PlayerUI.messageProgress_1 = new SleekProgress("");
			PlayerUI.messageProgress_1.PositionOffset_X = 30f;
			PlayerUI.messageProgress_1.PositionOffset_Y = 80f;
			PlayerUI.messageProgress_1.SizeOffset_X = -40f;
			PlayerUI.messageProgress_1.SizeOffset_Y = 10f;
			PlayerUI.messageProgress_1.SizeScale_X = 1f;
			PlayerUI.messageBox.AddChild(PlayerUI.messageProgress_1);
			PlayerUI.messageProgress_1.IsVisible = false;
			PlayerUI.messageProgress_2 = new SleekProgress("");
			PlayerUI.messageProgress_2.PositionOffset_X = 30f;
			PlayerUI.messageProgress_2.PositionOffset_Y = 110f;
			PlayerUI.messageProgress_2.SizeOffset_X = -40f;
			PlayerUI.messageProgress_2.SizeOffset_Y = 10f;
			PlayerUI.messageProgress_2.SizeScale_X = 1f;
			PlayerUI.messageBox.AddChild(PlayerUI.messageProgress_2);
			PlayerUI.messageProgress_2.IsVisible = false;
			PlayerUI.messageQualityImage = Glazier.Get().CreateImage(PlayerDashboardInventoryUI.icons.load<Texture2D>("Quality_0"));
			PlayerUI.messageQualityImage.PositionOffset_X = -30f;
			PlayerUI.messageQualityImage.PositionOffset_Y = -30f;
			PlayerUI.messageQualityImage.PositionScale_X = 1f;
			PlayerUI.messageQualityImage.PositionScale_Y = 1f;
			PlayerUI.messageQualityImage.SizeOffset_X = 20f;
			PlayerUI.messageQualityImage.SizeOffset_Y = 20f;
			PlayerUI.messageBox.AddChild(PlayerUI.messageQualityImage);
			PlayerUI.messageQualityImage.IsVisible = false;
			PlayerUI.messageAmountLabel = Glazier.Get().CreateLabel();
			PlayerUI.messageAmountLabel.PositionOffset_X = 10f;
			PlayerUI.messageAmountLabel.PositionOffset_Y = -40f;
			PlayerUI.messageAmountLabel.PositionScale_Y = 1f;
			PlayerUI.messageAmountLabel.SizeOffset_X = -20f;
			PlayerUI.messageAmountLabel.SizeOffset_Y = 30f;
			PlayerUI.messageAmountLabel.SizeScale_X = 1f;
			PlayerUI.messageAmountLabel.TextAlignment = 6;
			PlayerUI.messageAmountLabel.TextContrastContext = 1;
			PlayerUI.messageBox.AddChild(PlayerUI.messageAmountLabel);
			PlayerUI.messageAmountLabel.IsVisible = false;
			PlayerUI.messageBox2 = Glazier.Get().CreateBox();
			PlayerUI.messageBox2.PositionOffset_X = -200f;
			PlayerUI.messageBox2.PositionScale_X = 0.5f;
			PlayerUI.messageBox2.PositionScale_Y = 1f;
			PlayerUI.messageBox2.SizeOffset_X = 400f;
			PlayerUI.container.AddChild(PlayerUI.messageBox2);
			PlayerUI.messageBox2.IsVisible = false;
			PlayerUI.messageLabel2 = Glazier.Get().CreateLabel();
			PlayerUI.messageLabel2.PositionOffset_X = 5f;
			PlayerUI.messageLabel2.PositionOffset_Y = 5f;
			PlayerUI.messageLabel2.SizeOffset_X = -10f;
			PlayerUI.messageLabel2.SizeOffset_Y = 40f;
			PlayerUI.messageLabel2.SizeScale_X = 1f;
			PlayerUI.messageLabel2.FontSize = 3;
			PlayerUI.messageBox2.AddChild(PlayerUI.messageLabel2);
			PlayerUI.messageIcon2 = Glazier.Get().CreateImage();
			PlayerUI.messageIcon2.PositionOffset_X = 5f;
			PlayerUI.messageIcon2.PositionOffset_Y = 75f;
			PlayerUI.messageIcon2.SizeOffset_X = 20f;
			PlayerUI.messageIcon2.SizeOffset_Y = 20f;
			PlayerUI.messageBox2.AddChild(PlayerUI.messageIcon2);
			PlayerUI.messageIcon2.IsVisible = false;
			PlayerUI.messageProgress2_0 = new SleekProgress("");
			PlayerUI.messageProgress2_0.PositionOffset_X = 5f;
			PlayerUI.messageProgress2_0.PositionOffset_Y = 50f;
			PlayerUI.messageProgress2_0.SizeOffset_X = -10f;
			PlayerUI.messageProgress2_0.SizeOffset_Y = 10f;
			PlayerUI.messageProgress2_0.SizeScale_X = 1f;
			PlayerUI.messageBox2.AddChild(PlayerUI.messageProgress2_0);
			PlayerUI.messageProgress2_1 = new SleekProgress("");
			PlayerUI.messageProgress2_1.PositionOffset_X = 30f;
			PlayerUI.messageProgress2_1.PositionOffset_Y = 80f;
			PlayerUI.messageProgress2_1.SizeOffset_X = -40f;
			PlayerUI.messageProgress2_1.SizeOffset_Y = 10f;
			PlayerUI.messageProgress2_1.SizeScale_X = 1f;
			PlayerUI.messageBox2.AddChild(PlayerUI.messageProgress2_1);
			PlayerUI.painAlpha = 0f;
			PlayerUI.stunAlpha = 0f;
			PlayerUI.isBlindfolded = false;
			PlayerLife life = Player.player.life;
			life.onVisionUpdated = (VisionUpdated)Delegate.Combine(life.onVisionUpdated, new VisionUpdated(this.onVisionUpdated));
			PlayerLife life2 = Player.player.life;
			life2.onLifeUpdated = (LifeUpdated)Delegate.Combine(life2.onLifeUpdated, new LifeUpdated(this.onLifeUpdated));
			this.onLifeUpdated(Player.player.life.isDead);
			PlayerClothing clothing = Player.player.clothing;
			clothing.onGlassesUpdated = (GlassesUpdated)Delegate.Combine(clothing.onGlassesUpdated, new GlassesUpdated(this.onGlassesUpdated));
			LightingManager.onMoonUpdated = (MoonUpdated)Delegate.Combine(LightingManager.onMoonUpdated, new MoonUpdated(this.onMoonUpdated));
			this.menuBlurFX = base.GetComponent<BlurEffect>();
			this.hallucinationReverbZone = base.GetComponent<AudioReverbZone>();
		}

		// Token: 0x06003666 RID: 13926 RVA: 0x000FE9B4 File Offset: 0x000FCBB4
		private void OnDestroy()
		{
			if (PlayerUI.window == null)
			{
				return;
			}
			if (this.dashboardUI != null)
			{
				this.dashboardUI.OnDestroy();
			}
			if (this.pauseUI != null)
			{
				this.pauseUI.OnDestroy();
			}
			if (this.lifeUI != null)
			{
				this.lifeUI.OnDestroy();
			}
			if (!Provider.isApplicationQuitting)
			{
				PlayerUI.window.InternalDestroy();
			}
			PlayerUI.window = null;
			this.setIsHallucinating(false);
		}

		// Token: 0x06003667 RID: 13927 RVA: 0x000FEA20 File Offset: 0x000FCC20
		private void OnApplicationFocus(bool focus)
		{
			if (!OptionsSettings.pauseWhenUnfocused)
			{
				return;
			}
			if (PlayerUI.window == null)
			{
				return;
			}
			if (!focus)
			{
				this.escapeMenu();
				if (!PlayerPauseUI.active)
				{
					this.escapeMenu();
				}
			}
		}

		// Token: 0x04001F30 RID: 7984
		public static readonly float HIT_TIME = 0.33f;

		// Token: 0x04001F31 RID: 7985
		public static SleekWindow window;

		// Token: 0x04001F32 RID: 7986
		public static ISleekElement container;

		// Token: 0x04001F33 RID: 7987
		private static ISleekImage colorOverlayImage;

		// Token: 0x04001F34 RID: 7988
		private static SleekPlayer messagePlayer;

		// Token: 0x04001F35 RID: 7989
		public static ISleekBox messageBox;

		// Token: 0x04001F36 RID: 7990
		private static ISleekLabel messageLabel;

		// Token: 0x04001F37 RID: 7991
		private static SleekProgress messageProgress_0;

		// Token: 0x04001F38 RID: 7992
		private static SleekProgress messageProgress_1;

		// Token: 0x04001F39 RID: 7993
		private static SleekProgress messageProgress_2;

		// Token: 0x04001F3A RID: 7994
		private static ISleekImage messageIcon_0;

		// Token: 0x04001F3B RID: 7995
		private static ISleekImage messageIcon_1;

		// Token: 0x04001F3C RID: 7996
		private static ISleekImage messageIcon_2;

		// Token: 0x04001F3D RID: 7997
		private static ISleekImage messageQualityImage;

		// Token: 0x04001F3E RID: 7998
		private static ISleekLabel messageAmountLabel;

		// Token: 0x04001F3F RID: 7999
		public static ISleekBox messageBox2;

		// Token: 0x04001F40 RID: 8000
		private static ISleekLabel messageLabel2;

		// Token: 0x04001F41 RID: 8001
		private static SleekProgress messageProgress2_0;

		// Token: 0x04001F42 RID: 8002
		private static SleekProgress messageProgress2_1;

		// Token: 0x04001F43 RID: 8003
		private static ISleekImage messageIcon2;

		// Token: 0x04001F44 RID: 8004
		private static float painAlpha;

		// Token: 0x04001F45 RID: 8005
		private static Color stunColor;

		// Token: 0x04001F46 RID: 8006
		private static float stunAlpha;

		// Token: 0x04001F47 RID: 8007
		private static bool _isBlindfolded;

		// Token: 0x04001F49 RID: 8009
		private static bool inputWantsCustomModal;

		// Token: 0x04001F4A RID: 8010
		private static bool usingCustomModal;

		// Token: 0x04001F4B RID: 8011
		public static bool isLocked;

		// Token: 0x04001F4C RID: 8012
		private BlurEffect menuBlurFX;

		// Token: 0x04001F4D RID: 8013
		private AudioReverbZone hallucinationReverbZone;

		// Token: 0x04001F4E RID: 8014
		private static float hallucinationTimer;

		// Token: 0x04001F4F RID: 8015
		private static float messageDisappearTime;

		// Token: 0x04001F50 RID: 8016
		private static bool isMessaged;

		// Token: 0x04001F51 RID: 8017
		private static bool lastHinted;

		// Token: 0x04001F52 RID: 8018
		private static bool isHinted;

		// Token: 0x04001F53 RID: 8019
		private static bool lastHinted2;

		// Token: 0x04001F54 RID: 8020
		private static bool isHinted2;

		// Token: 0x04001F55 RID: 8021
		private static bool wantsWindowEnabled;

		// Token: 0x04001F56 RID: 8022
		private static bool isWindowEnabledByColorOverlay;

		// Token: 0x04001F57 RID: 8023
		public static EChatMode chat;

		// Token: 0x04001F58 RID: 8024
		private static StaticResourceRef<AudioClip> hitCriticalSound = new StaticResourceRef<AudioClip>("Sounds/General/Hit");

		// Token: 0x04001F59 RID: 8025
		internal static PlayerUI instance;

		// Token: 0x04001F5A RID: 8026
		internal PlayerGroupUI groupUI;

		// Token: 0x04001F5B RID: 8027
		private PlayerDashboardUI dashboardUI;

		// Token: 0x04001F5C RID: 8028
		private PlayerPauseUI pauseUI;

		// Token: 0x04001F5D RID: 8029
		private PlayerLifeUI lifeUI;

		// Token: 0x04001F5E RID: 8030
		internal PlayerBarricadeStereoUI boomboxUI;

		// Token: 0x04001F5F RID: 8031
		internal PlayerBarricadeMannequinUI mannequinUI;

		// Token: 0x04001F60 RID: 8032
		internal PlayerBrowserRequestUI browserRequestUI;
	}
}
