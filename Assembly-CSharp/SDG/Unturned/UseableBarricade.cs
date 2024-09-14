using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using SDG.Framework.Water;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007D9 RID: 2009
	public class UseableBarricade : Useable
	{
		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x06004432 RID: 17458 RVA: 0x00187E5A File Offset: 0x0018605A
		private bool isUseable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime;
			}
		}

		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x06004433 RID: 17459 RVA: 0x00187E70 File Offset: 0x00186070
		private bool isBuildable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime * 0.8f;
			}
		}

		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x06004434 RID: 17460 RVA: 0x00187E8C File Offset: 0x0018608C
		public ItemBarricadeAsset equippedBarricadeAsset
		{
			get
			{
				return base.player.equipment.asset as ItemBarricadeAsset;
			}
		}

		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x06004435 RID: 17461 RVA: 0x00187EA3 File Offset: 0x001860A3
		private bool allowRotationInputOnAllAxes
		{
			get
			{
				return this.equippedBarricadeAsset.build == EBuild.FREEFORM || this.equippedBarricadeAsset.build == EBuild.SENTRY_FREEFORM;
			}
		}

		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x06004436 RID: 17462 RVA: 0x00187EC5 File Offset: 0x001860C5
		private bool serverAllowAnyRotation
		{
			get
			{
				return this.allowRotationInputOnAllAxes || this.equippedBarricadeAsset.build == EBuild.CHARGE || this.equippedBarricadeAsset.build == EBuild.CLOCK || this.equippedBarricadeAsset.build == EBuild.NOTE;
			}
		}

		/// <summary>
		/// Does the item being placed count as a "trap" for the purposes of vehicle placement restrictions?
		/// </summary>
		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x06004437 RID: 17463 RVA: 0x00187EFE File Offset: 0x001860FE
		private bool useTrapRestrictions
		{
			get
			{
				return this.equippedBarricadeAsset.type == EItemType.TRAP;
			}
		}

		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x06004438 RID: 17464 RVA: 0x00187F0F File Offset: 0x0018610F
		private bool allowedToPlaceOnVehicle
		{
			get
			{
				if (!this.useTrapRestrictions)
				{
					return Provider.modeConfigData.Barricades.Allow_Item_Placement_On_Vehicle;
				}
				return Provider.modeConfigData.Barricades.Allow_Trap_Placement_On_Vehicle;
			}
		}

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x06004439 RID: 17465 RVA: 0x00187F38 File Offset: 0x00186138
		private float maxDistanceFromHull
		{
			get
			{
				if (!this.useTrapRestrictions)
				{
					return Provider.modeConfigData.Barricades.Max_Item_Distance_From_Hull;
				}
				return Provider.modeConfigData.Barricades.Max_Trap_Distance_From_Hull;
			}
		}

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x0600443A RID: 17466 RVA: 0x00187F61 File Offset: 0x00186161
		private bool useMaxDistanceFromHull
		{
			get
			{
				return this.maxDistanceFromHull > -0.5f;
			}
		}

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x0600443B RID: 17467 RVA: 0x00187F70 File Offset: 0x00186170
		private float sqrMaxDistanceFromHull
		{
			get
			{
				return MathfEx.Square(this.maxDistanceFromHull);
			}
		}

		// Token: 0x0600443C RID: 17468 RVA: 0x00187F7D File Offset: 0x0018617D
		[Obsolete]
		public void askBarricadeVehicle(CSteamID steamID, Vector3 newPoint, float newAngle_X, float newAngle_Y, float newAngle_Z, ushort plant)
		{
		}

		// Token: 0x0600443D RID: 17469 RVA: 0x00187F80 File Offset: 0x00186180
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 10)]
		public void ReceiveBarricadeVehicle(in ServerInvocationContext context, Vector3 newPoint, float newAngle_X, float newAngle_Y, float newAngle_Z, NetId regionNetId)
		{
			if (this.wasAsked)
			{
				return;
			}
			this.wasAsked = true;
			if (!this.allowedToPlaceOnVehicle)
			{
				return;
			}
			VehicleBarricadeRegion vehicleBarricadeRegion = NetIdRegistry.Get<VehicleBarricadeRegion>(regionNetId);
			if (vehicleBarricadeRegion == null)
			{
				return;
			}
			InteractableVehicle vehicle = vehicleBarricadeRegion.vehicle;
			if (vehicle == null)
			{
				return;
			}
			if (this.useMaxDistanceFromHull)
			{
				Vector3 position = vehicleBarricadeRegion.parent.TransformPoint(newPoint);
				if (vehicle.getSqrDistanceFromHull(position) > this.sqrMaxDistanceFromHull)
				{
					return;
				}
			}
			this.parent = vehicleBarricadeRegion.parent;
			this.parentVehicle = vehicle;
			this.point = newPoint;
			if (this.serverAllowAnyRotation)
			{
				this.angle_x = newAngle_X;
				this.angle_z = newAngle_Z;
			}
			else
			{
				this.angle_x = 0f;
				this.angle_z = 0f;
			}
			this.angle_y = newAngle_Y;
			this.rotate_x = 0f;
			this.rotate_y = 0f;
			this.rotate_z = 0f;
			this.isValid = this.checkClaims();
		}

		// Token: 0x0600443E RID: 17470 RVA: 0x00188066 File Offset: 0x00186266
		[Obsolete]
		public void askBarricadeNone(CSteamID steamID, Vector3 newPoint, float newAngle_X, float newAngle_Y, float newAngle_Z)
		{
			this.ReceiveBarricadeNone(newPoint, newAngle_X, newAngle_Y, newAngle_Z);
		}

		// Token: 0x0600443F RID: 17471 RVA: 0x00188074 File Offset: 0x00186274
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 10, legacyName = "askBarricadeNone")]
		public void ReceiveBarricadeNone(Vector3 newPoint, float newAngle_X, float newAngle_Y, float newAngle_Z)
		{
			if (this.wasAsked)
			{
				return;
			}
			this.wasAsked = true;
			if ((newPoint - base.player.look.aim.position).sqrMagnitude < 256f)
			{
				this.parent = null;
				this.parentVehicle = null;
				this.point = newPoint;
				if (this.serverAllowAnyRotation)
				{
					this.angle_x = newAngle_X;
					this.angle_z = newAngle_Z;
				}
				else
				{
					this.angle_x = 0f;
					this.angle_z = 0f;
				}
				this.angle_y = newAngle_Y;
				this.rotate_x = 0f;
				this.rotate_y = 0f;
				this.rotate_z = 0f;
				this.isValid = this.checkClaims();
				if (this.isValid)
				{
					this.pendingBuildHandle = BuildRequestManager.registerPendingBuild(this.point);
				}
			}
		}

		// Token: 0x06004440 RID: 17472 RVA: 0x00188150 File Offset: 0x00186350
		private bool check()
		{
			if (!this.checkSpace())
			{
				return false;
			}
			if (this.equippedBarricadeAsset.build == EBuild.VEHICLE)
			{
				this.parentVehicle = null;
				this.parent = null;
			}
			else
			{
				this.parentVehicle = DamageTool.getVehicle(this.hit.transform);
				this.parent = ((this.parentVehicle != null) ? this.hit.transform.root : null);
			}
			return this.checkClaims();
		}

		/// <summary>
		/// Should placement ghost material change be done recursively?
		/// e.g. Sentry has a deep hierarchy of meshes.
		/// </summary>
		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x06004441 RID: 17473 RVA: 0x001881CE File Offset: 0x001863CE
		private bool isHighlightRecursive
		{
			get
			{
				return this.equippedBarricadeAsset.build == EBuild.SENTRY || this.equippedBarricadeAsset.build == EBuild.SENTRY_FREEFORM;
			}
		}

		// Token: 0x06004442 RID: 17474 RVA: 0x001881F0 File Offset: 0x001863F0
		private Vector3 getPointInWorldSpace()
		{
			if (this.parent == null || (base.channel.IsLocalPlayer && !this.wasAsked))
			{
				return this.point;
			}
			return this.parent.TransformPoint(this.point);
		}

		// Token: 0x06004443 RID: 17475 RVA: 0x00188230 File Offset: 0x00186430
		private bool checkClaims()
		{
			if (base.player.movement.isSafe && base.player.movement.isSafeInfo.noBuildables)
			{
				if (base.channel.IsLocalPlayer)
				{
					PlayerUI.hint(null, EPlayerMessage.SAFEZONE);
				}
				return false;
			}
			Vector3 pointInWorldSpace = this.getPointInWorldSpace();
			if (base.channel.IsLocalPlayer && this.parentVehicle != null)
			{
				if (!this.allowedToPlaceOnVehicle)
				{
					PlayerUI.hint(null, EPlayerMessage.CANNOT_BUILD_ON_VEHICLE);
					return false;
				}
				if (this.useMaxDistanceFromHull && this.parentVehicle.getSqrDistanceFromHull(pointInWorldSpace) > this.sqrMaxDistanceFromHull)
				{
					PlayerUI.hint(null, EPlayerMessage.TOO_FAR_FROM_HULL);
					return false;
				}
			}
			if (this.equippedBarricadeAsset.build == EBuild.BEACON)
			{
				if (!LevelNavigation.checkSafeFakeNav(pointInWorldSpace) || this.parent != null)
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.NAV);
					}
					return false;
				}
				byte bound;
				if (LevelNavigation.tryGetBounds(pointInWorldSpace, out bound))
				{
					ZombieDifficultyAsset difficultyInBound = ZombieManager.getDifficultyInBound(bound);
					if (difficultyInBound != null && !difficultyInBound.Allow_Horde_Beacon)
					{
						if (base.channel.IsLocalPlayer)
						{
							PlayerUI.hint(null, EPlayerMessage.NOT_ALLOWED_HERE);
						}
						return false;
					}
				}
			}
			if (this.equippedBarricadeAsset.build != EBuild.CHARGE && !this.equippedBarricadeAsset.bypassClaim)
			{
				if (this.parent != null && !ClaimManager.canBuildOnVehicle(this.parent, base.channel.owner.playerID.steamID, base.player.quests.groupID))
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.CLAIM);
					}
					return false;
				}
				if (!ClaimManager.checkCanBuild(pointInWorldSpace, base.channel.owner.playerID.steamID, base.player.quests.groupID, this.equippedBarricadeAsset.build == EBuild.CLAIM))
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.CLAIM);
					}
					return false;
				}
			}
			if (!this.equippedBarricadeAsset.AllowPlacementInsideClipVolumes && VolumeManager<PlayerClipVolume, PlayerClipVolumeManager>.Get().IsPositionInsideAnyVolume(pointInWorldSpace))
			{
				if (base.channel.IsLocalPlayer)
				{
					PlayerUI.hint(null, EPlayerMessage.BOUNDS);
				}
				return false;
			}
			if (this.equippedBarricadeAsset.build == EBuild.BED)
			{
				if (Level.info == null || Level.info.type == ELevelType.ARENA)
				{
					return false;
				}
				if (VolumeManager<KillVolume, KillVolumeManager>.Get().IsPositionInsideAnyVolume(pointInWorldSpace))
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BOUNDS);
					}
					return false;
				}
			}
			if (!Provider.modeConfigData.Gameplay.Bypass_Buildable_Mobility && this.parent != null && this.parentVehicle != null && this.parentVehicle.asset != null)
			{
				bool flag;
				switch (this.parentVehicle.asset.BuildablePlacementRule)
				{
				default:
					flag = this.equippedBarricadeAsset.allowPlacementOnVehicle;
					break;
				case EVehicleBuildablePlacementRule.AlwaysAllow:
					flag = true;
					break;
				case EVehicleBuildablePlacementRule.Block:
					flag = false;
					break;
				}
				if (!flag)
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.MOBILE);
					}
					return false;
				}
			}
			if (this.equippedBarricadeAsset.build == EBuild.FREEFORM && !base.channel.owner.isAdmin && !((this.parent != null && this.parentVehicle != null) ? Provider.modeConfigData.Gameplay.Allow_Freeform_Buildables_On_Vehicles : Provider.modeConfigData.Gameplay.Allow_Freeform_Buildables))
			{
				if (base.channel.IsLocalPlayer)
				{
					PlayerUI.hint(null, EPlayerMessage.FREEFORM_BUILDABLE_NOT_ALLOWED);
				}
				return false;
			}
			if (this.parent != null && this.parentVehicle != null && this.parentVehicle.anySeatsOccupied)
			{
				if (base.channel.IsLocalPlayer)
				{
					PlayerUI.hint(null, EPlayerMessage.BUILD_ON_OCCUPIED_VEHICLE);
				}
				return false;
			}
			if (base.player.movement.getVehicle() != null)
			{
				if (base.channel.IsLocalPlayer)
				{
					PlayerUI.hint(null, EPlayerMessage.CANNOT_BUILD_WHILE_SEATED);
				}
				return false;
			}
			if ((Level.info == null || Level.info.type != ELevelType.ARENA) && !LevelPlayers.checkCanBuild(pointInWorldSpace))
			{
				if (base.channel.IsLocalPlayer)
				{
					PlayerUI.hint(null, EPlayerMessage.SPAWN);
				}
				return false;
			}
			if (!BuildRequestManager.canBuildAt(pointInWorldSpace, this.pendingBuildHandle))
			{
				return false;
			}
			if (WaterUtility.isPointUnderwater(pointInWorldSpace) && (this.equippedBarricadeAsset.build == EBuild.CAMPFIRE || this.equippedBarricadeAsset.build == EBuild.TORCH))
			{
				if (base.channel.IsLocalPlayer)
				{
					PlayerUI.hint(null, EPlayerMessage.UNDERWATER);
				}
				return false;
			}
			this.boundsRotation = BarricadeManager.getRotation((ItemBarricadeAsset)base.player.equipment.asset, this.angle_x + this.rotate_x, this.angle_y + this.rotate_y, this.angle_z + this.rotate_z);
			int mask;
			if (this.parent != null)
			{
				mask = RayMasks.BLOCK_CHAR_BUILDABLE_OVERLAP;
			}
			else
			{
				mask = RayMasks.BLOCK_CHAR_BUILDABLE_OVERLAP_NOT_ON_VEHICLE;
			}
			if (Physics.OverlapBoxNonAlloc(pointInWorldSpace + this.boundsRotation * this.boundsCenter, this.boundsOverlap, UseableBarricade.checkColliders, this.boundsRotation, mask, QueryTriggerInteraction.Collide) > 0)
			{
				if (base.channel.IsLocalPlayer)
				{
					PlayerUI.hint(null, EPlayerMessage.BLOCKED);
				}
				return false;
			}
			if (this.equippedBarricadeAsset.build == EBuild.BED)
			{
				Vector3 end = pointInWorldSpace + new Vector3(0f, 0.1f, 0f);
				RaycastHit raycastHit;
				if (Physics.Linecast(base.player.transform.position + Vector3.up * (base.player.look.heightLook * 0.5f), end, out raycastHit, RayMasks.BLOCK_BED_LOS, QueryTriggerInteraction.Ignore))
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
			}
			if (this.equippedBarricadeAsset.build == EBuild.DOOR || this.equippedBarricadeAsset.build == EBuild.GATE || this.equippedBarricadeAsset.build == EBuild.SHUTTER)
			{
				Vector3 halfExtents = this.boundsExtents;
				halfExtents.x -= 0.25f;
				halfExtents.y -= 0.5f;
				halfExtents.z += 0.6f;
				if (Physics.OverlapBoxNonAlloc(pointInWorldSpace + this.boundsRotation * this.boundsCenter, halfExtents, UseableBarricade.checkColliders, this.boundsRotation, RayMasks.BLOCK_DOOR_OPENING) > 0)
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				bool flag2 = false;
				bool flag3 = false;
				if (this.equippedBarricadeAsset.build == EBuild.DOOR)
				{
					flag2 = true;
					flag3 = this.boundsDoubleDoor;
				}
				else if (this.equippedBarricadeAsset.build == EBuild.GATE)
				{
					flag2 = this.boundsDoubleDoor;
					flag3 = this.boundsDoubleDoor;
				}
				else if (this.equippedBarricadeAsset.build == EBuild.SHUTTER)
				{
					flag2 = true;
					flag3 = true;
				}
				if (flag2 && Physics.OverlapSphereNonAlloc(pointInWorldSpace + this.boundsRotation * new Vector3(-this.boundsExtents.x, 0f, this.boundsExtents.x), 0.75f, UseableBarricade.checkColliders, RayMasks.BLOCK_DOOR_OPENING) > 0)
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				if (flag3 && Physics.OverlapSphereNonAlloc(pointInWorldSpace + this.boundsRotation * new Vector3(this.boundsExtents.x, 0f, this.boundsExtents.x), 0.75f, UseableBarricade.checkColliders, RayMasks.BLOCK_DOOR_OPENING) > 0)
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004444 RID: 17476 RVA: 0x00188988 File Offset: 0x00186B88
		private bool checkSpace()
		{
			this.angle_y = base.player.look.yaw;
			if (this.equippedBarricadeAsset.build == EBuild.FORTIFICATION || this.equippedBarricadeAsset.build == EBuild.SHUTTER || this.equippedBarricadeAsset.build == EBuild.GLASS)
			{
				Physics.Raycast(base.player.look.aim.position, base.player.look.aim.forward, out this.hit, this.equippedBarricadeAsset.range, RayMasks.SLOTS_INTERACT);
				if (!(this.hit.collider != null))
				{
					this.point = Vector3.zero;
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.WINDOW);
					}
					return false;
				}
				Transform transform = this.hit.collider.transform;
				if (!transform.CompareTag("Logic") || !(transform.name == "Slot"))
				{
					this.point = Vector3.zero;
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.WINDOW);
					}
					return false;
				}
				this.point = this.hit.point - this.hit.normal * this.equippedBarricadeAsset.offset;
				if (Mathf.Abs(Vector3.Dot(transform.right, Vector3.up)) > 0.5f)
				{
					this.angle_y = Quaternion.LookRotation(transform.forward).eulerAngles.y;
					if (Vector3.Dot(MainCamera.instance.transform.forward, transform.forward) < 0f)
					{
						this.angle_y += 180f;
					}
				}
				else
				{
					this.angle_y = Quaternion.LookRotation(transform.up).eulerAngles.y;
					if (Vector3.Dot(MainCamera.instance.transform.forward, transform.up) > 0f)
					{
						this.angle_y += 180f;
					}
				}
				if ((this.equippedBarricadeAsset.build == EBuild.SHUTTER || this.equippedBarricadeAsset.build == EBuild.GLASS) && (transform.parent.CompareTag("Barricade") || transform.parent.CompareTag("Structure")))
				{
					this.point = transform.position - this.hit.normal * this.equippedBarricadeAsset.offset;
				}
				if (!this.equippedBarricadeAsset.AllowPlacementInsideClipVolumes && !Level.checkSafeIncludingClipVolumes(this.point))
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BOUNDS);
					}
					return false;
				}
				if (Physics.OverlapSphereNonAlloc(this.point, this.equippedBarricadeAsset.radius, UseableBarricade.checkColliders, RayMasks.BLOCK_WINDOW) > 0)
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				return true;
			}
			else if (this.equippedBarricadeAsset.build == EBuild.BARRICADE || this.equippedBarricadeAsset.build == EBuild.TANK || this.equippedBarricadeAsset.build == EBuild.LIBRARY || this.equippedBarricadeAsset.build == EBuild.BARREL_RAIN || this.equippedBarricadeAsset.build == EBuild.VEHICLE || this.equippedBarricadeAsset.build == EBuild.BED || this.equippedBarricadeAsset.build == EBuild.STORAGE || this.equippedBarricadeAsset.build == EBuild.MANNEQUIN || this.equippedBarricadeAsset.build == EBuild.SENTRY || this.equippedBarricadeAsset.build == EBuild.GENERATOR || this.equippedBarricadeAsset.build == EBuild.SPOT || this.equippedBarricadeAsset.build == EBuild.CAMPFIRE || this.equippedBarricadeAsset.build == EBuild.OVEN || this.equippedBarricadeAsset.build == EBuild.CLAIM || this.equippedBarricadeAsset.build == EBuild.SPIKE || this.equippedBarricadeAsset.build == EBuild.SAFEZONE || this.equippedBarricadeAsset.build == EBuild.OXYGENATOR || this.equippedBarricadeAsset.build == EBuild.BEACON || this.equippedBarricadeAsset.build == EBuild.SIGN || this.equippedBarricadeAsset.build == EBuild.STEREO)
			{
				if (this.equippedBarricadeAsset.build == EBuild.VEHICLE)
				{
					Physics.Raycast(base.player.look.aim.position, base.player.look.aim.forward, out this.hit, this.equippedBarricadeAsset.range, RayMasks.BARRICADE_INTERACT);
				}
				else
				{
					Physics.SphereCast(base.player.look.aim.position, 0.1f, base.player.look.aim.forward, out this.hit, this.equippedBarricadeAsset.range, RayMasks.BARRICADE_INTERACT);
				}
				if (!(this.hit.transform != null))
				{
					this.point = Vector3.zero;
					return false;
				}
				if (this.hit.normal.y < 0.01f)
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				if ((double)this.hit.normal.y > 0.75)
				{
					this.point = this.hit.point + this.hit.normal * this.equippedBarricadeAsset.offset;
				}
				else
				{
					this.point = this.hit.point + Vector3.up * this.equippedBarricadeAsset.offset;
				}
				RaycastHit raycastHit;
				if (this.equippedBarricadeAsset.build == EBuild.VEHICLE && Physics.Linecast(this.hit.point, this.point, out raycastHit, RayMasks.BLOCK_BARRICADE))
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				if (!this.equippedBarricadeAsset.AllowPlacementInsideClipVolumes && !Level.checkSafeIncludingClipVolumes(this.point))
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BOUNDS);
					}
					return false;
				}
				if (this.equippedBarricadeAsset.build == EBuild.BED)
				{
					if (Physics.OverlapSphereNonAlloc(this.point + Vector3.up, 0.95f + this.equippedBarricadeAsset.offset, UseableBarricade.checkColliders, RayMasks.BLOCK_BARRICADE) > 0)
					{
						if (base.channel.IsLocalPlayer)
						{
							PlayerUI.hint(null, EPlayerMessage.BLOCKED);
						}
						return false;
					}
				}
				else if (Physics.OverlapSphereNonAlloc(this.point, this.equippedBarricadeAsset.radius, UseableBarricade.checkColliders, RayMasks.BLOCK_BARRICADE) > 0)
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				return true;
			}
			else if (this.equippedBarricadeAsset.build == EBuild.WIRE)
			{
				Physics.SphereCast(base.player.look.aim.position, 0.1f, base.player.look.aim.forward, out this.hit, this.equippedBarricadeAsset.range, RayMasks.BARRICADE_INTERACT);
				if (!(this.hit.transform != null))
				{
					this.point = Vector3.zero;
					return false;
				}
				this.point = this.hit.point + this.hit.normal * this.equippedBarricadeAsset.offset;
				if (!this.equippedBarricadeAsset.AllowPlacementInsideClipVolumes && !Level.checkSafeIncludingClipVolumes(this.point))
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BOUNDS);
					}
					return false;
				}
				if (Physics.OverlapSphereNonAlloc(this.point, this.equippedBarricadeAsset.radius, UseableBarricade.checkColliders, RayMasks.BLOCK_BARRICADE) > 0)
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				return true;
			}
			else if (this.equippedBarricadeAsset.build == EBuild.FARM || this.equippedBarricadeAsset.build == EBuild.OIL)
			{
				Physics.SphereCast(base.player.look.aim.position, 0.1f, base.player.look.aim.forward, out this.hit, this.equippedBarricadeAsset.range, RayMasks.BARRICADE_INTERACT);
				if (!(this.hit.transform != null))
				{
					this.point = Vector3.zero;
					return false;
				}
				if ((double)this.hit.normal.y > 0.75)
				{
					this.point = this.hit.point + this.hit.normal * this.equippedBarricadeAsset.offset;
				}
				else
				{
					this.point = this.hit.point + Vector3.up * this.equippedBarricadeAsset.offset;
				}
				string materialName = PhysicsTool.GetMaterialName(this.hit);
				if (this.hit.transform.CompareTag("Ground"))
				{
					if (this.equippedBarricadeAsset.build == EBuild.FARM)
					{
						if (!(this.equippedBarricadeAsset as ItemFarmAsset).ignoreSoilRestrictions && !PhysicMaterialCustomData.IsArable(materialName))
						{
							if (base.channel.IsLocalPlayer)
							{
								PlayerUI.hint(null, EPlayerMessage.SOIL);
							}
							return false;
						}
					}
					else if (!PhysicMaterialCustomData.HasOil(materialName))
					{
						if (base.channel.IsLocalPlayer)
						{
							PlayerUI.hint(null, EPlayerMessage.OIL);
						}
						return false;
					}
				}
				else
				{
					if (this.equippedBarricadeAsset.build != EBuild.FARM)
					{
						if (base.channel.IsLocalPlayer)
						{
							PlayerUI.hint(null, EPlayerMessage.OIL);
						}
						return false;
					}
					if (!(this.equippedBarricadeAsset as ItemFarmAsset).ignoreSoilRestrictions && !PhysicMaterialCustomData.IsArable(materialName))
					{
						if (base.channel.IsLocalPlayer)
						{
							PlayerUI.hint(null, EPlayerMessage.SOIL);
						}
						return false;
					}
				}
				if (!this.equippedBarricadeAsset.AllowPlacementInsideClipVolumes && !Level.checkSafeIncludingClipVolumes(this.point))
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BOUNDS);
					}
					return false;
				}
				if (Physics.OverlapSphereNonAlloc(this.point, this.equippedBarricadeAsset.radius, UseableBarricade.checkColliders, RayMasks.BLOCK_BARRICADE) > 0)
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				return true;
			}
			else if (this.equippedBarricadeAsset.build == EBuild.DOOR)
			{
				Physics.SphereCast(base.player.look.aim.position, 0.1f, base.player.look.aim.forward, out this.hit, this.equippedBarricadeAsset.range, RayMasks.SLOTS_INTERACT);
				if (!(this.hit.collider != null))
				{
					this.point = Vector3.zero;
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.DOORWAY);
					}
					return false;
				}
				Transform transform2 = this.hit.collider.transform;
				if (!(transform2.name == "Door"))
				{
					this.point = Vector3.zero;
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.DOORWAY);
					}
					return false;
				}
				this.point = transform2.position;
				this.angle_y = Quaternion.LookRotation(transform2.forward).eulerAngles.y;
				if (Vector3.Dot(MainCamera.instance.transform.forward, transform2.forward) < 0f)
				{
					this.angle_y += 180f;
				}
				if (!this.equippedBarricadeAsset.AllowPlacementInsideClipVolumes && !Level.checkSafeIncludingClipVolumes(this.point))
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BOUNDS);
					}
					return false;
				}
				if (Physics.OverlapSphereNonAlloc(this.point, this.equippedBarricadeAsset.radius, UseableBarricade.checkColliders, RayMasks.BLOCK_FRAME) > 0)
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				return true;
			}
			else if (this.equippedBarricadeAsset.build == EBuild.HATCH)
			{
				Physics.SphereCast(base.player.look.aim.position, 0.1f, base.player.look.aim.forward, out this.hit, this.equippedBarricadeAsset.range, RayMasks.SLOTS_INTERACT);
				if (!(this.hit.transform != null))
				{
					this.point = Vector3.zero;
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.TRAPDOOR);
					}
					return false;
				}
				if (!this.hit.transform.CompareTag("Logic") || !(this.hit.transform.name == "Hatch"))
				{
					this.point = Vector3.zero;
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.TRAPDOOR);
					}
					return false;
				}
				this.point = this.hit.transform.position;
				float y = Quaternion.LookRotation(this.hit.transform.forward).eulerAngles.y;
				this.angle_y = y;
				float num = Vector3.Dot(MainCamera.instance.transform.forward, this.hit.transform.forward);
				float num2 = Vector3.Dot(MainCamera.instance.transform.forward, this.hit.transform.right);
				float num3 = Vector3.Dot(MainCamera.instance.transform.forward, -this.hit.transform.forward);
				float num4 = Vector3.Dot(MainCamera.instance.transform.forward, -this.hit.transform.right);
				float num5 = num;
				if (num2 < num5)
				{
					num5 = num2;
					this.angle_y = y + 90f;
				}
				if (num3 < num5)
				{
					num5 = num3;
					this.angle_y = y + 180f;
				}
				if (num4 < num5)
				{
					this.angle_y = y + 270f;
				}
				this.angle_y += 180f;
				if (!this.equippedBarricadeAsset.AllowPlacementInsideClipVolumes && !Level.checkSafeIncludingClipVolumes(this.point))
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BOUNDS);
					}
					return false;
				}
				if (Physics.OverlapSphereNonAlloc(this.point, this.equippedBarricadeAsset.radius, UseableBarricade.checkColliders, RayMasks.BLOCK_FRAME) > 0)
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				return true;
			}
			else if (this.equippedBarricadeAsset.build == EBuild.GATE)
			{
				Physics.SphereCast(base.player.look.aim.position, 0.1f, base.player.look.aim.forward, out this.hit, this.equippedBarricadeAsset.range, RayMasks.SLOTS_INTERACT);
				if (!(this.hit.transform != null))
				{
					this.point = Vector3.zero;
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.GARAGE);
					}
					return false;
				}
				if (!this.hit.transform.CompareTag("Logic") || !(this.hit.transform.name == "Gate"))
				{
					this.point = Vector3.zero;
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.GARAGE);
					}
					return false;
				}
				this.point = this.hit.transform.position;
				if (Mathf.Abs(Vector3.Dot(this.hit.transform.up, Vector3.up)) > 0.5f)
				{
					this.angle_y = Quaternion.LookRotation(this.hit.transform.forward).eulerAngles.y;
					if (Vector3.Dot(MainCamera.instance.transform.forward, this.hit.transform.forward) < 0f)
					{
						this.angle_y += 180f;
					}
				}
				else
				{
					this.angle_y = Quaternion.LookRotation(this.hit.transform.up).eulerAngles.y;
					if (Vector3.Dot(MainCamera.instance.transform.forward, this.hit.transform.up) > 0f)
					{
						this.angle_y += 180f;
					}
				}
				if (!this.equippedBarricadeAsset.AllowPlacementInsideClipVolumes && !Level.checkSafeIncludingClipVolumes(this.point))
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BOUNDS);
					}
					return false;
				}
				if (Physics.OverlapSphereNonAlloc(this.point, this.equippedBarricadeAsset.radius, UseableBarricade.checkColliders, RayMasks.BLOCK_FRAME) > 0)
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				if (Physics.OverlapSphereNonAlloc(this.point + this.hit.transform.forward * -1.5f + this.hit.transform.up * -2f, 0.25f, UseableBarricade.checkColliders, RayMasks.BLOCK_FRAME) > 0)
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				return true;
			}
			else if (this.equippedBarricadeAsset.build == EBuild.LADDER)
			{
				Physics.SphereCast(base.player.look.aim.position, 0.1f, base.player.look.aim.forward, out this.hit, this.equippedBarricadeAsset.range, RayMasks.LADDERS_INTERACT);
				if (this.hit.transform != null)
				{
					if (this.hit.transform.CompareTag("Logic") && this.hit.transform.name == "Climb")
					{
						this.point = this.hit.transform.position;
						this.angle_y = Quaternion.LookRotation(this.hit.transform.forward).eulerAngles.y;
						if (Physics.OverlapSphereNonAlloc(this.point + this.hit.transform.up * 0.5f, 0.1f, UseableBarricade.checkColliders, RayMasks.BLOCK_BARRICADE) > 0)
						{
							if (base.channel.IsLocalPlayer)
							{
								PlayerUI.hint(null, EPlayerMessage.BLOCKED);
							}
							return false;
						}
						if (Physics.OverlapSphereNonAlloc(this.point + this.hit.transform.up * -0.5f, 0.1f, UseableBarricade.checkColliders, RayMasks.BLOCK_BARRICADE) > 0)
						{
							if (base.channel.IsLocalPlayer)
							{
								PlayerUI.hint(null, EPlayerMessage.BLOCKED);
							}
							return false;
						}
					}
					else
					{
						if (Mathf.Abs(this.hit.normal.y) < 0.1f)
						{
							this.point = this.hit.point + this.hit.normal * this.equippedBarricadeAsset.offset;
							this.angle_y = Quaternion.LookRotation(this.hit.normal).eulerAngles.y;
							if (Physics.OverlapSphereNonAlloc(this.point + Quaternion.Euler(0f, this.angle_y, 0f) * Vector3.right * 0.5f, 0.1f, UseableBarricade.checkColliders, RayMasks.BLOCK_BARRICADE) > 0)
							{
								if (base.channel.IsLocalPlayer)
								{
									PlayerUI.hint(null, EPlayerMessage.BLOCKED);
								}
								return false;
							}
							if (Physics.OverlapSphereNonAlloc(this.point + Quaternion.Euler(0f, this.angle_y, 0f) * Vector3.left * 0.5f, 0.1f, UseableBarricade.checkColliders, RayMasks.BLOCK_BARRICADE) > 0)
							{
								if (base.channel.IsLocalPlayer)
								{
									PlayerUI.hint(null, EPlayerMessage.BLOCKED);
								}
								return false;
							}
						}
						else
						{
							if (this.hit.normal.y > 0.75f)
							{
								this.point = this.hit.point + this.hit.normal * StructureManager.HEIGHT;
							}
							else
							{
								this.point = this.hit.point + Vector3.up * StructureManager.HEIGHT;
							}
							if (Physics.OverlapSphereNonAlloc(this.point, 0.5f, UseableBarricade.checkColliders, RayMasks.BLOCK_BARRICADE) > 0)
							{
								if (base.channel.IsLocalPlayer)
								{
									PlayerUI.hint(null, EPlayerMessage.BLOCKED);
								}
								return false;
							}
						}
						if (!this.equippedBarricadeAsset.AllowPlacementInsideClipVolumes && !Level.checkSafeIncludingClipVolumes(this.point))
						{
							if (base.channel.IsLocalPlayer)
							{
								PlayerUI.hint(null, EPlayerMessage.BOUNDS);
							}
							return false;
						}
					}
					return true;
				}
				this.point = Vector3.zero;
				return false;
			}
			else if (this.equippedBarricadeAsset.build == EBuild.TORCH || this.equippedBarricadeAsset.build == EBuild.STORAGE_WALL || this.equippedBarricadeAsset.build == EBuild.SIGN_WALL || this.equippedBarricadeAsset.build == EBuild.CAGE || this.equippedBarricadeAsset.build == EBuild.BARRICADE_WALL)
			{
				Physics.SphereCast(base.player.look.aim.position, 0.1f, base.player.look.aim.forward, out this.hit, this.equippedBarricadeAsset.range, RayMasks.BARRICADE_INTERACT);
				if (!(this.hit.transform != null) || Mathf.Abs(this.hit.normal.y) >= 0.1f)
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.WALL);
					}
					this.point = Vector3.zero;
					return false;
				}
				this.point = this.hit.point + this.hit.normal * this.equippedBarricadeAsset.offset;
				this.angle_y = Quaternion.LookRotation(this.hit.normal).eulerAngles.y;
				if (Physics.OverlapSphereNonAlloc(this.point, 0.1f, UseableBarricade.checkColliders, RayMasks.BLOCK_BARRICADE) > 0)
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				if (!this.equippedBarricadeAsset.AllowPlacementInsideClipVolumes && !Level.checkSafeIncludingClipVolumes(this.point))
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BOUNDS);
					}
					return false;
				}
				return true;
			}
			else if (this.equippedBarricadeAsset.build == EBuild.FREEFORM || this.equippedBarricadeAsset.build == EBuild.SENTRY_FREEFORM)
			{
				Physics.SphereCast(base.player.look.aim.position, 0.1f, base.player.look.aim.forward, out this.hit, this.equippedBarricadeAsset.range, RayMasks.BARRICADE_INTERACT);
				if (!(this.hit.transform != null))
				{
					this.point = Vector3.zero;
					return false;
				}
				Quaternion quaternion = Quaternion.Euler(0f, this.angle_y + this.rotate_y, 0f);
				quaternion *= Quaternion.Euler(-90f + this.angle_x + this.rotate_x, 0f, 0f);
				quaternion *= Quaternion.Euler(0f, this.angle_z + this.rotate_z, 0f);
				this.point = this.hit.point + this.hit.normal * -0.125f + quaternion * Vector3.forward * this.equippedBarricadeAsset.offset;
				if (!this.equippedBarricadeAsset.AllowPlacementInsideClipVolumes && !Level.checkSafeIncludingClipVolumes(this.point))
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BOUNDS);
					}
					return false;
				}
				if (Physics.OverlapSphereNonAlloc(this.point, this.equippedBarricadeAsset.radius, UseableBarricade.checkColliders, RayMasks.BLOCK_BARRICADE) > 0)
				{
					if (base.channel.IsLocalPlayer)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				return true;
			}
			else
			{
				if (this.equippedBarricadeAsset.build != EBuild.CHARGE && this.equippedBarricadeAsset.build != EBuild.CLOCK && this.equippedBarricadeAsset.build != EBuild.NOTE)
				{
					this.point = Vector3.zero;
					return false;
				}
				Physics.SphereCast(base.player.look.aim.position, 0.1f, base.player.look.aim.forward, out this.hit, this.equippedBarricadeAsset.range, RayMasks.BARRICADE_INTERACT);
				if (this.hit.transform != null)
				{
					Vector3 eulerAngles = Quaternion.LookRotation(this.hit.normal).eulerAngles;
					this.angle_x = eulerAngles.x;
					this.angle_y = eulerAngles.y;
					this.angle_z = eulerAngles.z;
					this.point = this.hit.point + this.hit.normal * this.equippedBarricadeAsset.offset;
					return true;
				}
				this.point = Vector3.zero;
				return false;
			}
		}

		// Token: 0x06004445 RID: 17477 RVA: 0x0018A2F1 File Offset: 0x001884F1
		private void build()
		{
			this.startedUse = Time.realtimeSinceStartup;
			this.isUsing = true;
			this.isBuilding = true;
			base.player.animator.play("Use", false);
		}

		// Token: 0x06004446 RID: 17478 RVA: 0x0018A322 File Offset: 0x00188522
		[Obsolete]
		public void askBuild(CSteamID steamID)
		{
			this.ReceivePlayBuild();
		}

		// Token: 0x06004447 RID: 17479 RVA: 0x0018A32A File Offset: 0x0018852A
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askBuild")]
		public void ReceivePlayBuild()
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.build();
			}
		}

		// Token: 0x06004448 RID: 17480 RVA: 0x0018A344 File Offset: 0x00188544
		public override bool startPrimary()
		{
			if (base.player.equipment.isBusy)
			{
				return false;
			}
			if (this.isValid)
			{
				if (base.channel.IsLocalPlayer)
				{
					if (this.parent != null)
					{
						VehicleBarricadeRegion vehicleBarricadeRegion = BarricadeManager.FindVehicleRegionByTransform(this.parent);
						if (vehicleBarricadeRegion != null)
						{
							UseableBarricade.SendBarricadeVehicle.Invoke(base.GetNetId(), ENetReliability.Reliable, this.parent.InverseTransformPoint(this.point), this.angle_x + this.rotate_x, this.angle_y + this.rotate_y - this.parent.localRotation.eulerAngles.y, this.angle_z + this.rotate_z, vehicleBarricadeRegion._netId);
						}
					}
					else
					{
						UseableBarricade.SendBarricadeNone.Invoke(base.GetNetId(), ENetReliability.Reliable, this.point, this.angle_x + this.rotate_x, this.angle_y + this.rotate_y, this.angle_z + this.rotate_z);
					}
				}
				base.player.equipment.isBusy = true;
				this.build();
				if (Provider.isServer)
				{
					UseableBarricade.SendPlayBuild.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner());
				}
				return true;
			}
			if (this.wasAsked)
			{
				base.player.equipment.dequip();
				return true;
			}
			return false;
		}

		// Token: 0x06004449 RID: 17481 RVA: 0x0018A4A0 File Offset: 0x001886A0
		public override bool startSecondary()
		{
			if (base.player.equipment.isBusy)
			{
				return false;
			}
			if (this.equippedBarricadeAsset.build == EBuild.GLASS || this.equippedBarricadeAsset.build == EBuild.CHARGE || this.equippedBarricadeAsset.build == EBuild.CLOCK || this.equippedBarricadeAsset.build == EBuild.NOTE || this.equippedBarricadeAsset.build == EBuild.FORTIFICATION || this.equippedBarricadeAsset.build == EBuild.DOOR || this.equippedBarricadeAsset.build == EBuild.GATE || this.equippedBarricadeAsset.build == EBuild.SHUTTER || this.equippedBarricadeAsset.build == EBuild.HATCH || this.equippedBarricadeAsset.build == EBuild.TORCH || this.equippedBarricadeAsset.build == EBuild.CAGE || this.equippedBarricadeAsset.build == EBuild.STORAGE_WALL || this.equippedBarricadeAsset.build == EBuild.SIGN_WALL || this.equippedBarricadeAsset.build == EBuild.BARRICADE_WALL)
			{
				return false;
			}
			base.player.look.isIgnoringInput = true;
			this.inputWantsToRotate = true;
			return true;
		}

		// Token: 0x0600444A RID: 17482 RVA: 0x0018A5B8 File Offset: 0x001887B8
		public override void stopSecondary()
		{
			base.player.look.isIgnoringInput = false;
			this.inputWantsToRotate = false;
		}

		// Token: 0x0600444B RID: 17483 RVA: 0x0018A5D4 File Offset: 0x001887D4
		public override void equip()
		{
			base.player.animator.play("Equip", true);
			this.useTime = base.player.animator.GetAnimationLength("Use", true);
			if (this.equippedBarricadeAsset.build == EBuild.MANNEQUIN)
			{
				this.boundsUse = true;
				this.boundsCenter = new Vector3(0f, 0f, -0.05f);
				this.boundsExtents = new Vector3(1.175f, 0.2f, 1.05f);
			}
			else if (this.equippedBarricadeAsset.barricade != null)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.equippedBarricadeAsset.barricade, Vector3.zero, Quaternion.identity);
				gameObject.name = "Helper";
				Collider collider;
				if (this.equippedBarricadeAsset.build == EBuild.DOOR || this.equippedBarricadeAsset.build == EBuild.GATE || this.equippedBarricadeAsset.build == EBuild.SHUTTER)
				{
					collider = gameObject.transform.Find("Placeholder").GetComponent<Collider>();
					this.boundsDoubleDoor = (gameObject.transform.Find("Skeleton").Find("Hinge") == null);
				}
				else
				{
					collider = gameObject.GetComponentInChildren<Collider>();
				}
				if (collider != null)
				{
					this.boundsUse = true;
					this.boundsCenter = gameObject.transform.InverseTransformPoint(collider.bounds.center);
					this.boundsExtents = collider.bounds.extents;
				}
				Object.Destroy(gameObject);
			}
			this.boundsOverlap = this.boundsExtents + new Vector3(0.5f, 0.5f, 0.5f);
			if (base.channel.IsLocalPlayer)
			{
				if (this.help == null)
				{
					this.help = BarricadeTool.getBarricade(null, 0, Vector3.zero, Quaternion.identity, base.player.equipment.itemID, base.player.equipment.state);
				}
				this.guide = this.help.Find("Root");
				if (this.guide == null)
				{
					this.guide = this.help;
				}
				HighlighterTool.help(this.guide, this.isValid, this.isHighlightRecursive);
				this.arrow = ((GameObject)Object.Instantiate(Resources.Load("Build/Arrow"))).transform;
				this.arrow.name = "Arrow";
				this.arrow.parent = this.help;
				this.arrow.localPosition = Vector3.zero;
				if (this.equippedBarricadeAsset.build == EBuild.DOOR || this.equippedBarricadeAsset.build == EBuild.GATE || this.equippedBarricadeAsset.build == EBuild.SHUTTER || this.equippedBarricadeAsset.build == EBuild.HATCH)
				{
					this.arrow.localRotation = Quaternion.identity;
				}
				else if (this.equippedBarricadeAsset.build == EBuild.MANNEQUIN)
				{
					this.rotate_y = 180f;
					this.arrow.localEulerAngles = new Vector3(-90f, 0f, 0f);
				}
				else
				{
					this.arrow.localRotation = Quaternion.Euler(90f, 0f, 0f);
				}
				Collider collider2;
				if (this.equippedBarricadeAsset.build == EBuild.DOOR || this.equippedBarricadeAsset.build == EBuild.GATE || this.equippedBarricadeAsset.build == EBuild.SHUTTER)
				{
					collider2 = this.help.Find("Placeholder").GetComponent<Collider>();
					this.boundsDoubleDoor = (this.help.Find("Skeleton").Find("Hinge") == null);
				}
				else
				{
					collider2 = this.help.GetComponentInChildren<Collider>();
				}
				if (this.equippedBarricadeAsset.build == EBuild.MANNEQUIN)
				{
					this.boundsUse = true;
					this.boundsCenter = new Vector3(0f, 0f, -0.05f);
					this.boundsExtents = new Vector3(1.175f, 0.2f, 1.05f);
					if (collider2 != null)
					{
						Object.Destroy(collider2);
					}
				}
				else if (collider2 != null)
				{
					this.boundsUse = true;
					this.boundsCenter = this.help.InverseTransformPoint(collider2.bounds.center);
					this.boundsExtents = collider2.bounds.extents;
					Object.Destroy(collider2);
				}
				this.boundsOverlap = this.boundsExtents + new Vector3(0.5f, 0.5f, 0.5f);
				if (this.equippedBarricadeAsset.build == EBuild.GLASS)
				{
					WaterHeightTransparentSort componentInChildren = this.help.GetComponentInChildren<WaterHeightTransparentSort>(true);
					if (componentInChildren != null)
					{
						Object.Destroy(componentInChildren);
					}
				}
				HighlighterTool.help(this.arrow, this.isValid);
				if (this.help.Find("Radius") != null)
				{
					this.isPower = true;
					this.powerPoint = Vector3.zero;
					this.claimsInRadius = new List<InteractableClaim>();
					this.generatorsInRadius = new List<InteractableGenerator>();
					this.safezonesInRadius = new List<InteractableSafezone>();
					this.oxygenatorsInRadius = new List<InteractableOxygenator>();
					if (this.equippedBarricadeAsset.build == EBuild.CLAIM || this.equippedBarricadeAsset.build == EBuild.GENERATOR || this.equippedBarricadeAsset.build == EBuild.SAFEZONE || this.equippedBarricadeAsset.build == EBuild.OXYGENATOR)
					{
						this.help.Find("Radius").gameObject.SetActive(true);
					}
				}
				Interactable component = this.help.GetComponent<Interactable>();
				if (component != null)
				{
					Object.Destroy(component);
				}
				if (this.equippedBarricadeAsset.build == EBuild.SPIKE || this.equippedBarricadeAsset.build == EBuild.WIRE)
				{
					Object.Destroy(this.help.Find("Trap").GetComponent<InteractableTrap>());
				}
				if (this.equippedBarricadeAsset.build == EBuild.BEACON)
				{
					Object.Destroy(this.help.GetComponent<InteractableBeacon>());
				}
				if (this.equippedBarricadeAsset.build == EBuild.DOOR || this.equippedBarricadeAsset.build == EBuild.GATE || this.equippedBarricadeAsset.build == EBuild.SHUTTER || this.equippedBarricadeAsset.build == EBuild.HATCH)
				{
					if (this.help.Find("Placeholder") != null)
					{
						Object.Destroy(this.help.Find("Placeholder").gameObject);
					}
					List<InteractableDoorHinge> list = new List<InteractableDoorHinge>();
					this.help.GetComponentsInChildren<InteractableDoorHinge>(list);
					for (int i = 0; i < list.Count; i++)
					{
						InteractableDoorHinge interactableDoorHinge = list[i];
						if (interactableDoorHinge.transform.Find("Clip") != null)
						{
							Object.Destroy(interactableDoorHinge.transform.Find("Clip").gameObject);
						}
						if (interactableDoorHinge.transform.Find("Nav") != null)
						{
							Object.Destroy(interactableDoorHinge.transform.Find("Nav").gameObject);
						}
						Object.Destroy(interactableDoorHinge.transform.GetComponent<Collider>());
						Object.Destroy(interactableDoorHinge);
					}
				}
				else
				{
					if (this.help.Find("Clip") != null)
					{
						Object.Destroy(this.help.Find("Clip").gameObject);
					}
					if (this.help.Find("Nav") != null)
					{
						Object.Destroy(this.help.Find("Nav").gameObject);
					}
					if (this.help.Find("Ladder") != null)
					{
						Object.Destroy(this.help.Find("Ladder").gameObject);
					}
					if (this.help.Find("Block") != null)
					{
						Object.Destroy(this.help.Find("Block").gameObject);
					}
				}
				int num = 0;
				while (num < 2 && this.help.Find("Climb") != null)
				{
					Object.Destroy(this.help.Find("Climb").gameObject);
					num++;
				}
				this.help.GetComponentsInChildren<Collider>(true, UseableBarricade.colliders);
				for (int j = 0; j < UseableBarricade.colliders.Count; j++)
				{
					Object.Destroy(UseableBarricade.colliders[j]);
				}
			}
		}

		// Token: 0x0600444C RID: 17484 RVA: 0x0018AE24 File Offset: 0x00189024
		public override void dequip()
		{
			base.player.look.isIgnoringInput = false;
			this.inputWantsToRotate = false;
			if (base.channel.IsLocalPlayer)
			{
				if (this.help != null)
				{
					Object.Destroy(this.help.gameObject);
				}
				if (this.isPower)
				{
					for (int i = 0; i < this.claimsInRadius.Count; i++)
					{
						if (!(this.claimsInRadius[i] == null))
						{
							Transform transform = this.claimsInRadius[i].transform.Find("Radius");
							if (transform != null)
							{
								transform.gameObject.SetActive(false);
							}
						}
					}
					this.claimsInRadius.Clear();
					for (int j = 0; j < this.generatorsInRadius.Count; j++)
					{
						if (!(this.generatorsInRadius[j] == null))
						{
							Transform transform2 = this.generatorsInRadius[j].transform.Find("Radius");
							if (transform2 != null)
							{
								transform2.gameObject.SetActive(false);
							}
						}
					}
					this.generatorsInRadius.Clear();
					for (int k = 0; k < this.safezonesInRadius.Count; k++)
					{
						if (!(this.safezonesInRadius[k] == null))
						{
							Transform transform3 = this.safezonesInRadius[k].transform.Find("Radius");
							if (transform3 != null)
							{
								transform3.gameObject.SetActive(false);
							}
						}
					}
					this.safezonesInRadius.Clear();
					for (int l = 0; l < this.oxygenatorsInRadius.Count; l++)
					{
						if (!(this.oxygenatorsInRadius[l] == null))
						{
							Transform transform4 = this.oxygenatorsInRadius[l].transform.Find("Radius");
							if (transform4 != null)
							{
								transform4.gameObject.SetActive(false);
							}
						}
					}
					this.oxygenatorsInRadius.Clear();
				}
			}
			BuildRequestManager.finishPendingBuild(ref this.pendingBuildHandle);
		}

		// Token: 0x0600444D RID: 17485 RVA: 0x0018B014 File Offset: 0x00189214
		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isUsing && this.isUseable)
			{
				base.player.equipment.isBusy = false;
				if (Provider.isServer)
				{
					int mask;
					if (this.parentVehicle != null)
					{
						mask = RayMasks.BLOCK_CHAR_BUILDABLE_OVERLAP;
					}
					else
					{
						mask = RayMasks.BLOCK_CHAR_BUILDABLE_OVERLAP_NOT_ON_VEHICLE;
					}
					if (this.boundsUse && Physics.OverlapBoxNonAlloc(this.getPointInWorldSpace() + this.boundsRotation * this.boundsCenter, this.boundsOverlap, UseableBarricade.checkColliders, this.boundsRotation, mask, QueryTriggerInteraction.Collide) > 0)
					{
						base.player.equipment.dequip();
						return;
					}
					if (this.parentVehicle != null && this.parentVehicle.isGoingToRespawn)
					{
						base.player.equipment.dequip();
						return;
					}
					if (this.parentVehicle != null && this.parentVehicle.isHooked)
					{
						base.player.equipment.dequip();
						return;
					}
					if (!this.checkClaims())
					{
						base.player.equipment.dequip();
						return;
					}
					ItemBarricadeAsset itemBarricadeAsset = (ItemBarricadeAsset)base.player.equipment.asset;
					bool flag = false;
					if (itemBarricadeAsset != null)
					{
						base.player.sendStat(EPlayerStat.FOUND_BUILDABLES);
						if (itemBarricadeAsset.build == EBuild.VEHICLE)
						{
							Asset asset = itemBarricadeAsset.FindVehicleAsset();
							if (asset != null)
							{
								flag = (VehicleManager.spawnLockedVehicleForPlayerV2(asset, this.point, Quaternion.Euler(this.angle_x + this.rotate_x, this.angle_y + this.rotate_y, this.angle_z + this.rotate_z), base.player) != null);
							}
						}
						else
						{
							Barricade barricade = new Barricade(itemBarricadeAsset);
							if (itemBarricadeAsset.build == EBuild.DOOR || itemBarricadeAsset.build == EBuild.GATE || itemBarricadeAsset.build == EBuild.SHUTTER || itemBarricadeAsset.build == EBuild.SIGN || itemBarricadeAsset.build == EBuild.SIGN_WALL || itemBarricadeAsset.build == EBuild.NOTE || itemBarricadeAsset.build == EBuild.HATCH)
							{
								BitConverter.GetBytes(base.channel.owner.playerID.steamID.m_SteamID).CopyTo(barricade.state, 0);
								BitConverter.GetBytes(base.player.quests.groupID.m_SteamID).CopyTo(barricade.state, 8);
							}
							else if (itemBarricadeAsset.build == EBuild.BED)
							{
								BitConverter.GetBytes(CSteamID.Nil.m_SteamID).CopyTo(barricade.state, 0);
							}
							else if (itemBarricadeAsset.build == EBuild.STORAGE || itemBarricadeAsset.build == EBuild.STORAGE_WALL || itemBarricadeAsset.build == EBuild.MANNEQUIN || itemBarricadeAsset.build == EBuild.SENTRY || itemBarricadeAsset.build == EBuild.SENTRY_FREEFORM || itemBarricadeAsset.build == EBuild.LIBRARY || itemBarricadeAsset.build == EBuild.MANNEQUIN)
							{
								BitConverter.GetBytes(base.channel.owner.playerID.steamID.m_SteamID).CopyTo(barricade.state, 0);
								BitConverter.GetBytes(base.player.quests.groupID.m_SteamID).CopyTo(barricade.state, 8);
							}
							else if (itemBarricadeAsset.build == EBuild.FARM)
							{
								BitConverter.GetBytes(Provider.time - (uint)(((ItemFarmAsset)base.player.equipment.asset).growth * (base.player.skills.mastery(2, 5) * 0.25f))).CopyTo(barricade.state, 0);
							}
							else if (itemBarricadeAsset.build == EBuild.TORCH || itemBarricadeAsset.build == EBuild.CAMPFIRE || itemBarricadeAsset.build == EBuild.OVEN || itemBarricadeAsset.build == EBuild.SPOT || itemBarricadeAsset.build == EBuild.SAFEZONE || itemBarricadeAsset.build == EBuild.OXYGENATOR || itemBarricadeAsset.build == EBuild.CAGE)
							{
								barricade.state[0] = 1;
							}
							else if (itemBarricadeAsset.build == EBuild.GENERATOR)
							{
								barricade.state[0] = 1;
							}
							else if (itemBarricadeAsset.build == EBuild.STEREO)
							{
								barricade.state[16] = 100;
							}
							flag = (BarricadeManager.dropBarricade(barricade, this.parent, this.point, this.angle_x + this.rotate_x, this.angle_y + this.rotate_y, this.angle_z + this.rotate_z, base.channel.owner.playerID.steamID.m_SteamID, base.player.quests.groupID.m_SteamID) != null);
						}
					}
					if (flag)
					{
						base.player.equipment.use();
						return;
					}
					base.player.equipment.dequip();
				}
			}
		}

		// Token: 0x0600444E RID: 17486 RVA: 0x0018B490 File Offset: 0x00189690
		private void processRotationInput()
		{
			if (this.allowRotationInputOnAllAxes)
			{
				if (ControlsSettings.invert)
				{
					this.input_x += ControlsSettings.mouseAimSensitivity * 2f * Input.GetAxis("mouse_y");
				}
				else
				{
					this.input_x -= ControlsSettings.mouseAimSensitivity * 2f * Input.GetAxis("mouse_y");
				}
			}
			this.input_y += ControlsSettings.mouseAimSensitivity * 2f * Input.GetAxis("mouse_x");
			if (this.allowRotationInputOnAllAxes)
			{
				this.input_z += ControlsSettings.mouseAimSensitivity * 30f * Input.GetAxis("mouse_z");
			}
			if (InputEx.GetKey(ControlsSettings.snap))
			{
				this.rotate_x = (float)((int)(this.input_x / 15f)) * 15f;
				this.rotate_y = (float)((int)(this.input_y / 15f)) * 15f;
				this.rotate_z = (float)((int)(this.input_z / 15f)) * 15f;
				return;
			}
			this.rotate_x = this.input_x;
			this.rotate_y = this.input_y;
			this.rotate_z = this.input_z;
		}

		// Token: 0x0600444F RID: 17487 RVA: 0x0018B5C4 File Offset: 0x001897C4
		public override void tick()
		{
			if (this.isBuilding && this.isBuildable)
			{
				this.isBuilding = false;
				if (Provider.isServer)
				{
					AlertTool.alert(base.transform.position, 8f);
				}
			}
			if (base.channel.IsLocalPlayer)
			{
				if (this.help == null)
				{
					return;
				}
				if (this.isUsing)
				{
					return;
				}
				if (this.inputWantsToRotate)
				{
					this.processRotationInput();
				}
				if (this.check())
				{
					if (!this.isValid)
					{
						this.isValid = true;
						HighlighterTool.help(this.guide, this.isValid, this.isHighlightRecursive);
						if (this.arrow != null)
						{
							HighlighterTool.help(this.arrow, this.isValid);
						}
					}
				}
				else if (this.isValid)
				{
					this.isValid = false;
					HighlighterTool.help(this.guide, this.isValid, this.isHighlightRecursive);
					if (this.arrow != null)
					{
						HighlighterTool.help(this.arrow, this.isValid);
					}
				}
				bool flag = this.help.parent != this.parent;
				if (flag)
				{
					this.help.parent = this.parent;
					this.help.gameObject.SetActive(false);
					this.help.gameObject.SetActive(true);
				}
				if (this.parent != null)
				{
					this.help.localPosition = this.parent.InverseTransformPoint(this.point);
					this.help.localRotation = Quaternion.Euler(0f, this.angle_y + this.rotate_y - this.parent.localRotation.eulerAngles.y, 0f);
					this.help.localRotation *= Quaternion.Euler((float)((this.equippedBarricadeAsset.build == EBuild.DOOR || this.equippedBarricadeAsset.build == EBuild.GATE || this.equippedBarricadeAsset.build == EBuild.SHUTTER || this.equippedBarricadeAsset.build == EBuild.HATCH) ? 0 : -90) + this.angle_x + this.rotate_x, 0f, 0f);
					this.help.localRotation *= Quaternion.Euler(0f, this.angle_z + this.rotate_z, 0f);
				}
				else
				{
					this.help.position = this.point;
					this.help.rotation = Quaternion.Euler(0f, this.angle_y + this.rotate_y, 0f);
					this.help.rotation *= Quaternion.Euler((float)((this.equippedBarricadeAsset.build == EBuild.DOOR || this.equippedBarricadeAsset.build == EBuild.GATE || this.equippedBarricadeAsset.build == EBuild.SHUTTER || this.equippedBarricadeAsset.build == EBuild.HATCH) ? 0 : -90) + this.angle_x + this.rotate_x, 0f, 0f);
					this.help.rotation *= Quaternion.Euler(0f, this.angle_z + this.rotate_z, 0f);
				}
				if (this.isPower)
				{
					bool flag2 = flag;
					if ((base.transform.position - this.powerPoint).sqrMagnitude > 1f)
					{
						this.powerPoint = base.transform.position;
						flag2 = true;
					}
					if (flag2)
					{
						for (int i = 0; i < this.claimsInRadius.Count; i++)
						{
							if (!(this.claimsInRadius[i] == null))
							{
								Transform transform = this.claimsInRadius[i].transform.Find("Radius");
								if (transform != null)
								{
									transform.gameObject.SetActive(false);
								}
							}
						}
						this.claimsInRadius.Clear();
						for (int j = 0; j < this.generatorsInRadius.Count; j++)
						{
							if (!(this.generatorsInRadius[j] == null))
							{
								Transform transform2 = this.generatorsInRadius[j].transform.Find("Radius");
								if (transform2 != null)
								{
									transform2.gameObject.SetActive(false);
								}
							}
						}
						this.generatorsInRadius.Clear();
						for (int k = 0; k < this.safezonesInRadius.Count; k++)
						{
							if (!(this.safezonesInRadius[k] == null))
							{
								Transform transform3 = this.safezonesInRadius[k].transform.Find("Radius");
								if (transform3 != null)
								{
									transform3.gameObject.SetActive(false);
								}
							}
						}
						this.safezonesInRadius.Clear();
						for (int l = 0; l < this.oxygenatorsInRadius.Count; l++)
						{
							if (!(this.oxygenatorsInRadius[l] == null))
							{
								Transform transform4 = this.oxygenatorsInRadius[l].transform.Find("Radius");
								if (transform4 != null)
								{
									transform4.gameObject.SetActive(false);
								}
							}
						}
						this.oxygenatorsInRadius.Clear();
						byte b;
						byte b2;
						ushort plant;
						BarricadeRegion barricadeRegion;
						BarricadeManager.tryGetPlant(this.parent, out b, out b2, out plant, out barricadeRegion);
						if (this.equippedBarricadeAsset.build == EBuild.CLAIM)
						{
							PowerTool.checkInteractables<InteractableClaim>(this.powerPoint, 64f, plant, this.claimsInRadius);
							for (int m = 0; m < this.claimsInRadius.Count; m++)
							{
								if (!(this.claimsInRadius[m] == null))
								{
									Transform transform5 = this.claimsInRadius[m].transform.Find("Radius");
									if (transform5 != null)
									{
										transform5.gameObject.SetActive(true);
									}
								}
							}
						}
						else
						{
							PowerTool.checkInteractables<InteractableGenerator>(this.powerPoint, 64f, plant, this.generatorsInRadius);
							for (int n = 0; n < this.generatorsInRadius.Count; n++)
							{
								if (!(this.generatorsInRadius[n] == null))
								{
									Transform transform6 = this.generatorsInRadius[n].transform.Find("Radius");
									if (transform6 != null)
									{
										transform6.gameObject.SetActive(true);
									}
								}
							}
						}
						if (this.equippedBarricadeAsset.build == EBuild.SAFEZONE)
						{
							PowerTool.checkInteractables<InteractableSafezone>(this.powerPoint, 64f, plant, this.safezonesInRadius);
							for (int num = 0; num < this.safezonesInRadius.Count; num++)
							{
								if (!(this.safezonesInRadius[num] == null))
								{
									Transform transform7 = this.safezonesInRadius[num].transform.Find("Radius");
									if (transform7 != null)
									{
										transform7.gameObject.SetActive(true);
									}
								}
							}
						}
						if (this.equippedBarricadeAsset.build == EBuild.OXYGENATOR)
						{
							PowerTool.checkInteractables<InteractableOxygenator>(this.powerPoint, 64f, plant, this.oxygenatorsInRadius);
							for (int num2 = 0; num2 < this.oxygenatorsInRadius.Count; num2++)
							{
								if (!(this.oxygenatorsInRadius[num2] == null))
								{
									Transform transform8 = this.oxygenatorsInRadius[num2].transform.Find("Radius");
									if (transform8 != null)
									{
										transform8.gameObject.SetActive(true);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06004450 RID: 17488 RVA: 0x0018BD0C File Offset: 0x00189F0C
		protected void OnDestroy()
		{
			BuildRequestManager.finishPendingBuild(ref this.pendingBuildHandle);
		}

		// Token: 0x04002DA3 RID: 11683
		private static List<Collider> colliders = new List<Collider>();

		// Token: 0x04002DA4 RID: 11684
		private static Collider[] checkColliders = new Collider[1];

		// Token: 0x04002DA5 RID: 11685
		private Transform parent;

		// Token: 0x04002DA6 RID: 11686
		private Transform help;

		// Token: 0x04002DA7 RID: 11687
		private Transform guide;

		// Token: 0x04002DA8 RID: 11688
		private Transform arrow;

		// Token: 0x04002DA9 RID: 11689
		private InteractableVehicle parentVehicle;

		// Token: 0x04002DAA RID: 11690
		private bool boundsUse;

		// Token: 0x04002DAB RID: 11691
		private bool boundsDoubleDoor;

		// Token: 0x04002DAC RID: 11692
		private Vector3 boundsCenter;

		// Token: 0x04002DAD RID: 11693
		private Vector3 boundsExtents;

		// Token: 0x04002DAE RID: 11694
		private Vector3 boundsOverlap;

		// Token: 0x04002DAF RID: 11695
		private Quaternion boundsRotation;

		// Token: 0x04002DB0 RID: 11696
		private float startedUse;

		// Token: 0x04002DB1 RID: 11697
		private float useTime;

		// Token: 0x04002DB2 RID: 11698
		private bool inputWantsToRotate;

		// Token: 0x04002DB3 RID: 11699
		private bool isBuilding;

		// Token: 0x04002DB4 RID: 11700
		private bool isUsing;

		// Token: 0x04002DB5 RID: 11701
		private bool isValid;

		// Token: 0x04002DB6 RID: 11702
		private bool wasAsked;

		// Token: 0x04002DB7 RID: 11703
		private int pendingBuildHandle = -1;

		// Token: 0x04002DB8 RID: 11704
		private RaycastHit hit;

		// Token: 0x04002DB9 RID: 11705
		private Vector3 point;

		// Token: 0x04002DBA RID: 11706
		private float angle_x;

		// Token: 0x04002DBB RID: 11707
		private float angle_y;

		// Token: 0x04002DBC RID: 11708
		private float angle_z;

		// Token: 0x04002DBD RID: 11709
		private float rotate_x;

		// Token: 0x04002DBE RID: 11710
		private float rotate_y;

		// Token: 0x04002DBF RID: 11711
		private float rotate_z;

		// Token: 0x04002DC0 RID: 11712
		private float input_x;

		// Token: 0x04002DC1 RID: 11713
		private float input_y;

		// Token: 0x04002DC2 RID: 11714
		private float input_z;

		// Token: 0x04002DC3 RID: 11715
		private bool isPower;

		// Token: 0x04002DC4 RID: 11716
		private Vector3 powerPoint;

		// Token: 0x04002DC5 RID: 11717
		private List<InteractableClaim> claimsInRadius;

		// Token: 0x04002DC6 RID: 11718
		private List<InteractableGenerator> generatorsInRadius;

		// Token: 0x04002DC7 RID: 11719
		private List<InteractableSafezone> safezonesInRadius;

		// Token: 0x04002DC8 RID: 11720
		private List<InteractableOxygenator> oxygenatorsInRadius;

		// Token: 0x04002DC9 RID: 11721
		private static readonly ServerInstanceMethod<Vector3, float, float, float, NetId> SendBarricadeVehicle = ServerInstanceMethod<Vector3, float, float, float, NetId>.Get(typeof(UseableBarricade), "ReceiveBarricadeVehicle");

		// Token: 0x04002DCA RID: 11722
		private static readonly ServerInstanceMethod<Vector3, float, float, float> SendBarricadeNone = ServerInstanceMethod<Vector3, float, float, float>.Get(typeof(UseableBarricade), "ReceiveBarricadeNone");

		// Token: 0x04002DCB RID: 11723
		private static readonly ClientInstanceMethod SendPlayBuild = ClientInstanceMethod.Get(typeof(UseableBarricade), "ReceivePlayBuild");
	}
}
