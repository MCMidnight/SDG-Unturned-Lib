using System;
using SDG.Framework.Devkit;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007E7 RID: 2023
	internal static class UseableHousingUtils
	{
		// Token: 0x0600456F RID: 17775 RVA: 0x0019D114 File Offset: 0x0019B314
		public static Transform InstantiatePlacementPreview(ItemStructureAsset asset)
		{
			Transform transform = null;
			if (transform == null)
			{
				transform = StructureTool.getStructure(asset.id, 0);
			}
			transform.position = Vector3.zero;
			transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
			Collider[] componentsInChildren = transform.GetComponentsInChildren<Collider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Object.Destroy(componentsInChildren[i]);
			}
			HighlighterTool.help(transform, false);
			if (transform.Find("Clip") != null)
			{
				Object.Destroy(transform.Find("Clip").gameObject);
			}
			if (transform.Find("Nav") != null)
			{
				Object.Destroy(transform.Find("Nav").gameObject);
			}
			if (transform.Find("Cutter") != null)
			{
				Object.Destroy(transform.Find("Cutter").gameObject);
			}
			if (transform.Find("Block") != null)
			{
				Object.Destroy(transform.Find("Block").gameObject);
			}
			return transform;
		}

		// Token: 0x06004570 RID: 17776 RVA: 0x0019D224 File Offset: 0x0019B424
		public static EHousingPlacementResult ValidatePendingPlacement(ItemStructureAsset asset, ref Vector3 position, float yaw, ref string obstructionHint)
		{
			try
			{
				switch (asset.construct)
				{
				case EConstruct.FLOOR:
					return StructureManager.housingConnections.ValidateSquareFloorPlacement(asset.terrainTestHeight, ref position, yaw, ref obstructionHint);
				case EConstruct.WALL:
					return StructureManager.housingConnections.ValidateWallPlacement(ref position, 2.125f, asset.requiresPillars, true, ref obstructionHint);
				case EConstruct.RAMPART:
					return StructureManager.housingConnections.ValidateWallPlacement(ref position, 0.9f, asset.requiresPillars, false, ref obstructionHint);
				case EConstruct.ROOF:
					return StructureManager.housingConnections.ValidateSquareRoofPlacement(ref position, yaw, ref obstructionHint);
				case EConstruct.PILLAR:
					return StructureManager.housingConnections.ValidatePillarPlacement(ref position, 2.125f, ref obstructionHint);
				case EConstruct.POST:
					return StructureManager.housingConnections.ValidatePillarPlacement(ref position, 0.9f, ref obstructionHint);
				case EConstruct.FLOOR_POLY:
					return StructureManager.housingConnections.ValidateTriangleFloorPlacement(asset.terrainTestHeight, ref position, yaw, ref obstructionHint);
				case EConstruct.ROOF_POLY:
					return StructureManager.housingConnections.ValidateTriangleRoofPlacement(ref position, yaw, ref obstructionHint);
				default:
					UnturnedLog.error("Unhandled housing type: " + asset.construct.ToString());
					break;
				}
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Caught exception while validating housing placement:");
			}
			return EHousingPlacementResult.Success;
		}

		// Token: 0x06004571 RID: 17777 RVA: 0x0019D36C File Offset: 0x0019B56C
		public static bool FindPlacement(ItemStructureAsset asset, Player player, float rotationOffset, float foundationOffset, out Vector3 pendingPlacementPosition, out float pendingPlacementYaw)
		{
			SteamChannel channel = player.channel;
			pendingPlacementPosition = default(Vector3);
			pendingPlacementYaw = player.look.yaw;
			EHousingPlacementResult ehousingPlacementResult = EHousingPlacementResult.Success;
			string text = null;
			Ray ray = new Ray(player.look.aim.position, player.look.aim.forward);
			if (asset.construct == EConstruct.FLOOR || asset.construct == EConstruct.FLOOR_POLY)
			{
				if (!StructureManager.housingConnections.FindEmptyFloorSlot(ray, false, out pendingPlacementPosition, out pendingPlacementYaw))
				{
					RaycastHit hit;
					if (!Physics.SphereCast(ray, 0.1f, out hit, asset.range, RayMasks.STRUCTURE_INTERACT))
					{
						pendingPlacementPosition = Vector3.zero;
						return false;
					}
					pendingPlacementPosition = hit.point;
					pendingPlacementYaw = player.look.yaw;
					if (asset.construct == EConstruct.FLOOR_POLY)
					{
						pendingPlacementPosition += Quaternion.Euler(0f, pendingPlacementYaw, 0f) * new Vector3(0f, 0f, 1.2679492f);
					}
					pendingPlacementPosition += new Vector3(0f, foundationOffset, 0f);
					if (!StructureManager.housingConnections.DoesHitCountAsTerrain(hit))
					{
						ehousingPlacementResult = EHousingPlacementResult.MissingGround;
					}
				}
			}
			else if (asset.construct == EConstruct.WALL || asset.construct == EConstruct.RAMPART)
			{
				if (StructureManager.housingConnections.FindEmptyWallSlot(ray, out pendingPlacementPosition, out pendingPlacementYaw))
				{
					if (asset.construct == EConstruct.RAMPART)
					{
						pendingPlacementPosition += Vector3.down * 1.225f;
					}
				}
				else
				{
					ehousingPlacementResult = EHousingPlacementResult.MissingSlot;
				}
			}
			else if (asset.construct == EConstruct.ROOF || asset.construct == EConstruct.ROOF_POLY)
			{
				if (!StructureManager.housingConnections.FindEmptyFloorSlot(ray, true, out pendingPlacementPosition, out pendingPlacementYaw))
				{
					ehousingPlacementResult = EHousingPlacementResult.MissingSlot;
				}
			}
			else if (asset.construct == EConstruct.PILLAR || asset.construct == EConstruct.POST)
			{
				if (StructureManager.housingConnections.FindEmptyPillarSlot(ray, out pendingPlacementPosition, out pendingPlacementYaw))
				{
					if (asset.construct == EConstruct.POST)
					{
						pendingPlacementPosition += Vector3.down * 1.225f;
					}
				}
				else
				{
					ehousingPlacementResult = EHousingPlacementResult.MissingSlot;
				}
			}
			if (ehousingPlacementResult == EHousingPlacementResult.Success)
			{
				ehousingPlacementResult = UseableHousingUtils.ValidatePendingPlacement(asset, ref pendingPlacementPosition, pendingPlacementYaw + rotationOffset, ref text);
			}
			if (channel.IsLocalPlayer)
			{
				if (ehousingPlacementResult == EHousingPlacementResult.MissingSlot)
				{
					switch (asset.construct)
					{
					case EConstruct.WALL:
					case EConstruct.RAMPART:
						PlayerUI.hint(null, EPlayerMessage.WALL);
						break;
					case EConstruct.ROOF:
					case EConstruct.ROOF_POLY:
						PlayerUI.hint(null, EPlayerMessage.ROOF);
						break;
					case EConstruct.PILLAR:
					case EConstruct.POST:
						PlayerUI.hint(null, EPlayerMessage.CORNER);
						break;
					}
				}
				else if (ehousingPlacementResult == EHousingPlacementResult.Obstructed)
				{
					if (string.IsNullOrEmpty(text))
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					else
					{
						PlayerUI.hint(null, EPlayerMessage.PLACEMENT_OBSTRUCTED_BY, text, Color.white, Array.Empty<object>());
					}
				}
				else if (ehousingPlacementResult == EHousingPlacementResult.MissingPillar)
				{
					switch (asset.construct)
					{
					case EConstruct.WALL:
					case EConstruct.ROOF:
					case EConstruct.ROOF_POLY:
						PlayerUI.hint(null, EPlayerMessage.PILLAR);
						break;
					case EConstruct.RAMPART:
						PlayerUI.hint(null, EPlayerMessage.POST);
						break;
					}
				}
				else if (ehousingPlacementResult == EHousingPlacementResult.MissingGround)
				{
					PlayerUI.hint(null, EPlayerMessage.GROUND);
				}
				else if (ehousingPlacementResult == EHousingPlacementResult.ObstructedByGround)
				{
					PlayerUI.hint(null, EPlayerMessage.PLACEMENT_OBSTRUCTED_BY_GROUND);
				}
			}
			return ehousingPlacementResult == EHousingPlacementResult.Success;
		}

		// Token: 0x06004572 RID: 17778 RVA: 0x0019D690 File Offset: 0x0019B890
		public static bool IsPendingPositionValid(Player player, Vector3 pendingPlacementPosition)
		{
			SteamChannel channel = player.channel;
			if (player.movement.isSafe && player.movement.isSafeInfo.noBuildables)
			{
				if (channel.IsLocalPlayer)
				{
					PlayerUI.hint(null, EPlayerMessage.SAFEZONE);
				}
				return false;
			}
			if (!Level.isPointWithinValidHeight(pendingPlacementPosition.y))
			{
				PlayerUI.hint(null, EPlayerMessage.BOUNDS);
				return false;
			}
			if (!ClaimManager.checkCanBuild(pendingPlacementPosition, channel.owner.playerID.steamID, player.quests.groupID, false))
			{
				if (channel.IsLocalPlayer)
				{
					PlayerUI.hint(null, EPlayerMessage.CLAIM);
				}
				return false;
			}
			if (VolumeManager<PlayerClipVolume, PlayerClipVolumeManager>.Get().IsPositionInsideAnyVolume(pendingPlacementPosition))
			{
				if (channel.IsLocalPlayer)
				{
					PlayerUI.hint(null, EPlayerMessage.BOUNDS);
				}
				return false;
			}
			if (!LevelPlayers.checkCanBuild(pendingPlacementPosition))
			{
				if (channel.IsLocalPlayer)
				{
					PlayerUI.hint(null, EPlayerMessage.SPAWN);
				}
				return false;
			}
			return true;
		}

		// Token: 0x04002EDD RID: 11997
		public const float FOUNDATION_MOUSE_SCROLL_MULTIPLIER = 0.05f;

		// Token: 0x04002EDE RID: 11998
		public const float FOUNDATION_MIN_OFFSET = -1f;

		// Token: 0x04002EDF RID: 11999
		public const float FOUNDATION_MAX_OFFSET = 1f;
	}
}
