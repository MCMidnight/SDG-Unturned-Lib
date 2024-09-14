using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020005A8 RID: 1448
	public class VehicleManager : SteamCaller
	{
		// Token: 0x140000A2 RID: 162
		// (add) Token: 0x06002E4B RID: 11851 RVA: 0x000C94E4 File Offset: 0x000C76E4
		// (remove) Token: 0x06002E4C RID: 11852 RVA: 0x000C9518 File Offset: 0x000C7718
		public static event VehicleManager.EnterVehicleRequestHandler onEnterVehicleRequested;

		// Token: 0x140000A3 RID: 163
		// (add) Token: 0x06002E4D RID: 11853 RVA: 0x000C954C File Offset: 0x000C774C
		// (remove) Token: 0x06002E4E RID: 11854 RVA: 0x000C9580 File Offset: 0x000C7780
		public static event VehicleManager.ExitVehicleRequestHandler onExitVehicleRequested;

		// Token: 0x140000A4 RID: 164
		// (add) Token: 0x06002E4F RID: 11855 RVA: 0x000C95B4 File Offset: 0x000C77B4
		// (remove) Token: 0x06002E50 RID: 11856 RVA: 0x000C95E8 File Offset: 0x000C77E8
		public static event VehicleManager.SwapSeatRequestHandler onSwapSeatRequested;

		/// <summary>
		/// Invoked immediately before Destroy vehicle.
		/// </summary>
		// Token: 0x140000A5 RID: 165
		// (add) Token: 0x06002E51 RID: 11857 RVA: 0x000C961C File Offset: 0x000C781C
		// (remove) Token: 0x06002E52 RID: 11858 RVA: 0x000C9650 File Offset: 0x000C7850
		public static event Action<InteractableVehicle> OnPreDestroyVehicle;

		/// <summary>
		/// Exposed for Rocket transition to modules backwards compatibility.
		/// </summary>
		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06002E53 RID: 11859 RVA: 0x000C9683 File Offset: 0x000C7883
		public static VehicleManager instance
		{
			get
			{
				return VehicleManager.manager;
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x06002E54 RID: 11860 RVA: 0x000C968A File Offset: 0x000C788A
		public static List<InteractableVehicle> vehicles
		{
			get
			{
				return VehicleManager._vehicles;
			}
		}

		// Token: 0x06002E55 RID: 11861 RVA: 0x000C9691 File Offset: 0x000C7891
		private static uint allocateInstanceID()
		{
			return VehicleManager.highestInstanceID += 1U;
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06002E56 RID: 11862 RVA: 0x000C96A0 File Offset: 0x000C78A0
		public static uint maxInstances
		{
			get
			{
				switch (Level.info.size)
				{
				case ELevelSize.TINY:
					return Provider.modeConfigData.Vehicles.Max_Instances_Tiny;
				case ELevelSize.SMALL:
					return Provider.modeConfigData.Vehicles.Max_Instances_Small;
				case ELevelSize.MEDIUM:
					return Provider.modeConfigData.Vehicles.Max_Instances_Medium;
				case ELevelSize.LARGE:
					return Provider.modeConfigData.Vehicles.Max_Instances_Large;
				case ELevelSize.INSANE:
					return Provider.modeConfigData.Vehicles.Max_Instances_Insane;
				default:
					return 0U;
				}
			}
		}

		// Token: 0x06002E57 RID: 11863 RVA: 0x000C9728 File Offset: 0x000C7928
		public static byte getVehicleRandomTireAliveMask(VehicleAsset asset)
		{
			if (asset.canTiresBeDamaged)
			{
				int num = 0;
				for (byte b = 0; b < 8; b += 1)
				{
					if (Random.value < Provider.modeConfigData.Vehicles.Has_Tire_Chance)
					{
						int num2 = 1 << (int)b;
						num |= num2;
					}
				}
				return (byte)num;
			}
			return byte.MaxValue;
		}

		// Token: 0x06002E58 RID: 11864 RVA: 0x000C9778 File Offset: 0x000C7978
		public static void getVehiclesInRadius(Vector3 center, float sqrRadius, List<InteractableVehicle> result)
		{
			if (VehicleManager.vehicles == null)
			{
				return;
			}
			for (int i = 0; i < VehicleManager.vehicles.Count; i++)
			{
				InteractableVehicle interactableVehicle = VehicleManager.vehicles[i];
				if (!interactableVehicle.isDead && (interactableVehicle.transform.position - center).sqrMagnitude < sqrRadius)
				{
					result.Add(interactableVehicle);
				}
			}
		}

		/// <summary>
		/// Find vehicle with matching replicated instance ID.
		/// </summary>
		// Token: 0x06002E59 RID: 11865 RVA: 0x000C97DC File Offset: 0x000C79DC
		public static InteractableVehicle findVehicleByNetInstanceID(uint instanceID)
		{
			foreach (InteractableVehicle interactableVehicle in VehicleManager.vehicles)
			{
				if (interactableVehicle != null && interactableVehicle.instanceID == instanceID)
				{
					return interactableVehicle;
				}
			}
			return null;
		}

		// Token: 0x06002E5A RID: 11866 RVA: 0x000C9840 File Offset: 0x000C7A40
		public static InteractableVehicle getVehicle(uint instanceID)
		{
			return VehicleManager.findVehicleByNetInstanceID(instanceID);
		}

		// Token: 0x06002E5B RID: 11867 RVA: 0x000C9848 File Offset: 0x000C7A48
		public static void damage(InteractableVehicle vehicle, float damage, float times, bool canRepair, CSteamID instigatorSteamID = default(CSteamID), EDamageOrigin damageOrigin = EDamageOrigin.Unknown)
		{
			if (vehicle == null || vehicle.asset == null)
			{
				return;
			}
			if (!vehicle.isDead)
			{
				if (!vehicle.asset.isVulnerable && !vehicle.asset.isVulnerableToExplosions && !vehicle.asset.isVulnerableToEnvironment)
				{
					UnturnedLog.error(string.Concat(new string[]
					{
						"Somehow tried to damage completely invulnerable vehicle: ",
						(vehicle != null) ? vehicle.ToString() : null,
						" ",
						damage.ToString(),
						" ",
						times.ToString(),
						" ",
						canRepair.ToString()
					}));
					return;
				}
				times *= Provider.modeConfigData.Vehicles.Armor_Multiplier;
				ushort num = (ushort)(damage * times);
				bool flag = true;
				DamageVehicleRequestHandler damageVehicleRequestHandler = VehicleManager.onDamageVehicleRequested;
				if (damageVehicleRequestHandler != null)
				{
					damageVehicleRequestHandler(instigatorSteamID, vehicle, ref num, ref canRepair, ref flag, damageOrigin);
				}
				if (!flag || num < 1)
				{
					return;
				}
				vehicle.askDamage(num, canRepair);
			}
		}

		// Token: 0x06002E5C RID: 11868 RVA: 0x000C9940 File Offset: 0x000C7B40
		public static void damageTire(InteractableVehicle vehicle, int tireIndex, CSteamID instigatorSteamID = default(CSteamID), EDamageOrigin damageOrigin = EDamageOrigin.Unknown)
		{
			if (tireIndex < 0)
			{
				return;
			}
			bool flag = true;
			DamageTireRequestHandler damageTireRequestHandler = VehicleManager.onDamageTireRequested;
			if (damageTireRequestHandler != null)
			{
				damageTireRequestHandler(instigatorSteamID, vehicle, tireIndex, ref flag, damageOrigin);
			}
			if (!flag)
			{
				return;
			}
			vehicle.askDamageTire(tireIndex);
		}

		// Token: 0x06002E5D RID: 11869 RVA: 0x000C9975 File Offset: 0x000C7B75
		public static void repair(InteractableVehicle vehicle, float damage, float times)
		{
			VehicleManager.repair(vehicle, damage, times, CSteamID.Nil);
		}

		// Token: 0x06002E5E RID: 11870 RVA: 0x000C9984 File Offset: 0x000C7B84
		public static void repair(InteractableVehicle vehicle, float damage, float times, CSteamID instigatorSteamID = default(CSteamID))
		{
			if (vehicle == null)
			{
				return;
			}
			if (!vehicle.isExploded && !vehicle.isRepaired)
			{
				ushort num = (ushort)(damage * times);
				bool flag = true;
				RepairVehicleRequestHandler repairVehicleRequestHandler = VehicleManager.onRepairVehicleRequested;
				if (repairVehicleRequestHandler != null)
				{
					repairVehicleRequestHandler(instigatorSteamID, vehicle, ref num, ref flag);
				}
				if (!flag || num < 1)
				{
					return;
				}
				vehicle.askRepair(num);
			}
		}

		/// <summary>
		/// Supports redirects by VehicleRedirectorAsset. If redirector's SpawnPaintColor is set, that color is used.
		/// </summary>
		// Token: 0x06002E5F RID: 11871 RVA: 0x000C99D8 File Offset: 0x000C7BD8
		public static InteractableVehicle spawnVehicleV2(ushort id, Vector3 point, Quaternion angle)
		{
			return VehicleManager.spawnVehicleInternal(Assets.find(EAssetType.VEHICLE, id), point, angle, CSteamID.Nil, default(Color32?));
		}

		/// <summary>
		/// Supports redirects by VehicleRedirectorAsset. If paintColor is set that takes priority, otherwise if
		/// redirector's SpawnPaintColor is set, that color is used,
		/// </summary>
		// Token: 0x06002E60 RID: 11872 RVA: 0x000C9A01 File Offset: 0x000C7C01
		public static InteractableVehicle spawnVehicleV2(ushort id, Vector3 point, Quaternion angle, Color32? paintColor)
		{
			return VehicleManager.spawnVehicleInternal(Assets.find(EAssetType.VEHICLE, id), point, angle, CSteamID.Nil, paintColor);
		}

		/// <summary>
		/// Supports redirects by VehicleRedirectorAsset. If redirector's SpawnPaintColor is set, that color is used.
		/// </summary>
		// Token: 0x06002E61 RID: 11873 RVA: 0x000C9A18 File Offset: 0x000C7C18
		public static InteractableVehicle spawnLockedVehicleForPlayerV2(ushort id, Vector3 point, Quaternion angle, Player player)
		{
			if (player == null)
			{
				throw new ArgumentNullException("player");
			}
			return VehicleManager.spawnVehicleInternal(Assets.find(EAssetType.VEHICLE, id), point, angle, player.channel.owner.playerID.steamID, default(Color32?));
		}

		/// <summary>
		/// Supports redirects by VehicleRedirectorAsset. If paintColor is set that takes priority, otherwise if
		/// redirector's SpawnPaintColor is set, that color is used,
		/// </summary>
		// Token: 0x06002E62 RID: 11874 RVA: 0x000C9A65 File Offset: 0x000C7C65
		public static InteractableVehicle spawnLockedVehicleForPlayerV2(ushort id, Vector3 point, Quaternion angle, Player player, Color32? paintColor)
		{
			if (player == null)
			{
				throw new ArgumentNullException("player");
			}
			return VehicleManager.spawnVehicleInternal(Assets.find(EAssetType.VEHICLE, id), point, angle, player.channel.owner.playerID.steamID, paintColor);
		}

		/// <summary>
		/// Supports redirects by VehicleRedirectorAsset. If redirector's SpawnPaintColor is set, that color is used.
		/// </summary>
		// Token: 0x06002E63 RID: 11875 RVA: 0x000C9AA0 File Offset: 0x000C7CA0
		public static InteractableVehicle spawnVehicleV2(Asset asset, Vector3 point, Quaternion angle)
		{
			return VehicleManager.spawnVehicleInternal(asset, point, angle, CSteamID.Nil, default(Color32?));
		}

		/// <summary>
		/// Supports redirects by VehicleRedirectorAsset. If paintColor is set that takes priority, otherwise if
		/// redirector's SpawnPaintColor is set, that color is used,
		/// </summary>
		// Token: 0x06002E64 RID: 11876 RVA: 0x000C9AC3 File Offset: 0x000C7CC3
		public static InteractableVehicle spawnVehicleV2(Asset asset, Vector3 point, Quaternion angle, Color32? paintColor)
		{
			return VehicleManager.spawnVehicleInternal(asset, point, angle, CSteamID.Nil, paintColor);
		}

		/// <summary>
		/// Supports redirects by VehicleRedirectorAsset. If redirector's SpawnPaintColor is set, that color is used.
		/// </summary>
		// Token: 0x06002E65 RID: 11877 RVA: 0x000C9AD4 File Offset: 0x000C7CD4
		public static InteractableVehicle spawnLockedVehicleForPlayerV2(Asset asset, Vector3 point, Quaternion angle, Player player)
		{
			if (player == null)
			{
				throw new ArgumentNullException("player");
			}
			return VehicleManager.spawnVehicleInternal(asset, point, angle, player.channel.owner.playerID.steamID, default(Color32?));
		}

		/// <summary>
		/// Supports redirects by VehicleRedirectorAsset. If paintColor is set that takes priority, otherwise if
		/// redirector's SpawnPaintColor is set, that color is used,
		/// </summary>
		// Token: 0x06002E66 RID: 11878 RVA: 0x000C9B1B File Offset: 0x000C7D1B
		public static InteractableVehicle spawnLockedVehicleForPlayerV2(Asset asset, Vector3 point, Quaternion angle, Player player, Color32? paintColor)
		{
			if (player == null)
			{
				throw new ArgumentNullException("player");
			}
			return VehicleManager.spawnVehicleInternal(asset, point, angle, player.channel.owner.playerID.steamID, paintColor);
		}

		/// <summary>
		/// Added so that garage plugins do not need to invoke RPC manually.
		/// </summary>
		/// <param name="batteryCharge">zero spawns without a battery, ushort.MaxValue indicates the battery should be randomly spawned according to asset configuration, other values force a battery to spawn.</param>
		// Token: 0x06002E67 RID: 11879 RVA: 0x000C9B50 File Offset: 0x000C7D50
		public static InteractableVehicle SpawnVehicleV3(VehicleAsset asset, ushort skinID, ushort mythicID, float roadPosition, Vector3 point, Quaternion angle, bool sirens, bool blimp, bool headlights, bool taillights, ushort fuel, ushort health, ushort batteryCharge, CSteamID owner, CSteamID group, bool locked, byte[][] turrets, byte tireAliveMask, Color32 paintColor)
		{
			NetId netId = NetIdRegistry.ClaimBlock(21U);
			InteractableVehicle spawnedVehicle = VehicleManager.manager.addVehicle(asset.GUID, skinID, mythicID, roadPosition, point, angle, sirens, blimp, headlights, taillights, fuel, false, health, batteryCharge, owner, group, locked, null, turrets, VehicleManager.allocateInstanceID(), tireAliveMask, netId, paintColor);
			if (spawnedVehicle == null)
			{
				return null;
			}
			VehicleManager.SendSingleVehicle.Invoke(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), delegate(NetPakWriter writer)
			{
				VehicleManager.sendVehicle(spawnedVehicle, writer);
			});
			return spawnedVehicle;
		}

		/// <summary>
		/// For backwards compatibility. This older method spawns a vehicle with a random paint color. (set paintColor
		/// to zero for a random paint color)
		/// </summary>
		/// <param name="batteryCharge">zero spawns without a battery, ushort.MaxValue indicates the battery should be randomly spawned according to asset configuration, other values force a battery to spawn.</param>
		// Token: 0x06002E68 RID: 11880 RVA: 0x000C9BDC File Offset: 0x000C7DDC
		public static InteractableVehicle SpawnVehicleV3(VehicleAsset asset, ushort skinID, ushort mythicID, float roadPosition, Vector3 point, Quaternion angle, bool sirens, bool blimp, bool headlights, bool taillights, ushort fuel, ushort health, ushort batteryCharge, CSteamID owner, CSteamID group, bool locked, byte[][] turrets, byte tireAliveMask)
		{
			return VehicleManager.SpawnVehicleV3(asset, skinID, mythicID, roadPosition, point, angle, sirens, blimp, headlights, taillights, fuel, health, batteryCharge, owner, group, locked, turrets, tireAliveMask, new Color32(0, 0, 0, 0));
		}

		/// <summary>
		/// Used by external spawn vehicle methods.
		/// Supports redirects by VehicleRedirectorAsset. If redirector's SpawnPaintColor is set, that color is used,
		/// unless preferredColor.a is byte.MaxValue.
		/// </summary>
		/// <param name="owner">Owner to lock vehicle for by default. Used to lock vehicles to the player who purchased them.</param>
		// Token: 0x06002E69 RID: 11881 RVA: 0x000C9C18 File Offset: 0x000C7E18
		private static InteractableVehicle spawnVehicleInternal(Asset asset, Vector3 point, Quaternion angle, CSteamID owner, Color32? preferredColor)
		{
			if (asset == null)
			{
				return null;
			}
			Color32 value = new Color32(0, 0, 0, 0);
			if (preferredColor != null)
			{
				value = preferredColor.Value;
			}
			VehicleRedirectorAsset vehicleRedirectorAsset = asset as VehicleRedirectorAsset;
			VehicleAsset vehicleAsset;
			if (vehicleRedirectorAsset != null)
			{
				vehicleAsset = vehicleRedirectorAsset.TargetVehicle.Find();
				if (preferredColor == null && vehicleRedirectorAsset.SpawnPaintColor != null)
				{
					value = vehicleRedirectorAsset.SpawnPaintColor.Value;
				}
			}
			else
			{
				vehicleAsset = (asset as VehicleAsset);
			}
			if (vehicleAsset == null)
			{
				return null;
			}
			bool locked = owner != CSteamID.Nil;
			return VehicleManager.SpawnVehicleV3(vehicleAsset, 0, 0, 0f, point, angle, false, false, false, false, vehicleAsset.fuel, vehicleAsset.health, 10000, owner, CSteamID.Nil, locked, null, byte.MaxValue, value);
		}

		// Token: 0x06002E6A RID: 11882 RVA: 0x000C9CD8 File Offset: 0x000C7ED8
		public static void enterVehicle(InteractableVehicle vehicle)
		{
			VehiclePhysicsProfileAsset vehiclePhysicsProfileAsset = vehicle.asset.physicsProfileRef.Find();
			byte[] arg = (vehiclePhysicsProfileAsset != null) ? vehiclePhysicsProfileAsset.hash : new byte[0];
			VehicleManager.SendEnterVehicleRequest.Invoke(ENetReliability.Unreliable, vehicle.instanceID, vehicle.asset.hash, arg, (byte)vehicle.asset.engine);
		}

		// Token: 0x06002E6B RID: 11883 RVA: 0x000C9D34 File Offset: 0x000C7F34
		public static void exitVehicle()
		{
			if (Player.player.movement.getVehicle() != null)
			{
				VehicleManager.SendExitVehicleRequest.Invoke(ENetReliability.Unreliable, Player.player.movement.getVehicle().GetComponent<Rigidbody>().velocity);
			}
		}

		// Token: 0x06002E6C RID: 11884 RVA: 0x000C9D71 File Offset: 0x000C7F71
		public static void swapVehicle(byte toSeat)
		{
			if (Player.player.movement.getVehicle() != null)
			{
				VehicleManager.SendSwapVehicleRequest.Invoke(ENetReliability.Unreliable, toSeat);
			}
		}

		// Token: 0x06002E6D RID: 11885 RVA: 0x000C9D96 File Offset: 0x000C7F96
		public static void sendVehicleLock()
		{
			if (Player.player.movement.getVehicle() != null)
			{
				VehicleManager.SendVehicleLockRequest.Invoke(ENetReliability.Unreliable);
			}
		}

		// Token: 0x06002E6E RID: 11886 RVA: 0x000C9DBA File Offset: 0x000C7FBA
		public static void sendVehicleSkin()
		{
			if (Player.player.movement.getVehicle() != null)
			{
				VehicleManager.SendVehicleSkinRequest.Invoke(ENetReliability.Unreliable);
			}
		}

		/// <summary>
		/// Client-side request server to toggle headlights.
		/// </summary>
		// Token: 0x06002E6F RID: 11887 RVA: 0x000C9DE0 File Offset: 0x000C7FE0
		public static void sendVehicleHeadlights()
		{
			InteractableVehicle vehicle = Player.player.movement.getVehicle();
			if (vehicle == null || vehicle.asset == null)
			{
				return;
			}
			bool flag = !vehicle.headlightsOn;
			if (!vehicle.asset.hasHeadlights && flag)
			{
				return;
			}
			VehicleManager.SendToggleVehicleHeadlights.Invoke(ENetReliability.Unreliable, flag);
		}

		/// <summary>
		/// As client request server to use bonus feature like towing hook or police sirens.
		/// </summary>
		// Token: 0x06002E70 RID: 11888 RVA: 0x000C9E38 File Offset: 0x000C8038
		public static void sendVehicleBonus()
		{
			InteractableVehicle vehicle = Player.player.movement.getVehicle();
			if (vehicle == null)
			{
				return;
			}
			byte arg;
			if (vehicle.asset.hasSirens)
			{
				arg = 0;
			}
			else if (vehicle.asset.hasHook)
			{
				arg = 1;
			}
			else
			{
				if (vehicle.asset.engine != EEngine.BLIMP)
				{
					return;
				}
				arg = 2;
			}
			VehicleManager.SendUseVehicleBonus.Invoke(ENetReliability.Unreliable, arg);
		}

		// Token: 0x06002E71 RID: 11889 RVA: 0x000C9EA0 File Offset: 0x000C80A0
		public static void sendVehicleStealBattery()
		{
			if (Player.player.movement.getVehicle() != null)
			{
				VehicleManager.SendStealVehicleBattery.Invoke(ENetReliability.Unreliable);
			}
		}

		// Token: 0x06002E72 RID: 11890 RVA: 0x000C9EC4 File Offset: 0x000C80C4
		public static void sendVehicleHorn()
		{
			InteractableVehicle vehicle = Player.player.movement.getVehicle();
			if (vehicle != null && vehicle.asset.hasHorn)
			{
				VehicleManager.SendVehicleHornRequest.Invoke(ENetReliability.Unreliable);
			}
		}

		// Token: 0x06002E73 RID: 11891 RVA: 0x000C9F04 File Offset: 0x000C8104
		internal static void sendVehicle(InteractableVehicle vehicle, NetPakWriter writer)
		{
			Vector3 vector;
			if (vehicle.asset.engine == EEngine.TRAIN)
			{
				vector = InteractableVehicle.PackRoadPosition(vehicle.roadPosition);
			}
			else
			{
				vector = vehicle.transform.position;
			}
			SystemNetPakWriterEx.WriteGuid(writer, vehicle.asset.GUID);
			SystemNetPakWriterEx.WriteUInt16(writer, vehicle.skinID);
			SystemNetPakWriterEx.WriteUInt16(writer, vehicle.mythicID);
			UnityNetPakWriterEx.WriteClampedVector3(writer, vector, 13, 8);
			UnityNetPakWriterEx.WriteQuaternion(writer, vehicle.transform.rotation, 11);
			writer.WriteBit(vehicle.sirensOn);
			writer.WriteBit(vehicle.isBlimpFloating);
			writer.WriteBit(vehicle.headlightsOn);
			writer.WriteBit(vehicle.taillightsOn);
			SystemNetPakWriterEx.WriteUInt16(writer, vehicle.fuel);
			writer.WriteBit(vehicle.isExploded);
			SystemNetPakWriterEx.WriteUInt16(writer, vehicle.health);
			SystemNetPakWriterEx.WriteUInt16(writer, vehicle.batteryCharge);
			SteamworksNetPakWriterEx.WriteSteamID(writer, vehicle.lockedOwner);
			SteamworksNetPakWriterEx.WriteSteamID(writer, vehicle.lockedGroup);
			writer.WriteBit(vehicle.isLocked);
			SystemNetPakWriterEx.WriteUInt8(writer, (byte)vehicle.passengers.Length);
			byte b = 0;
			while ((int)b < vehicle.passengers.Length)
			{
				Passenger passenger = vehicle.passengers[(int)b];
				if (passenger.player != null)
				{
					SteamworksNetPakWriterEx.WriteSteamID(writer, passenger.player.playerID.steamID);
				}
				else
				{
					SteamworksNetPakWriterEx.WriteSteamID(writer, CSteamID.Nil);
				}
				b += 1;
			}
			SystemNetPakWriterEx.WriteUInt32(writer, vehicle.instanceID);
			SystemNetPakWriterEx.WriteUInt8(writer, vehicle.tireAliveMask);
			writer.WriteNetId(vehicle.GetNetId());
			UnityNetPakWriterEx.WriteColor32RGBA(writer, vehicle.PaintColor);
			if (vehicle.asset.replicatedWheelIndices != null)
			{
				SystemNetPakWriterEx.WriteUInt8(writer, (byte)vehicle.asset.replicatedWheelIndices.Length);
				foreach (int num in vehicle.asset.replicatedWheelIndices)
				{
					Wheel wheelAtIndex = vehicle.GetWheelAtIndex(num);
					if (wheelAtIndex == null)
					{
						UnturnedLog.error(string.Format("\"{0}\" missing wheel for replicated index: {1}", vehicle.asset.FriendlyName, num));
						SystemNetPakWriterEx.WriteUnsignedNormalizedFloat(writer, 0f, 4);
					}
					else
					{
						SystemNetPakWriterEx.WriteUnsignedNormalizedFloat(writer, wheelAtIndex.replicatedSuspensionState, 4);
					}
				}
				return;
			}
			SystemNetPakWriterEx.WriteUInt8(writer, 0);
		}

		// Token: 0x06002E74 RID: 11892 RVA: 0x000CA140 File Offset: 0x000C8340
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellVehicleLock")]
		public static void ReceiveVehicleLockState(uint instanceID, CSteamID owner, CSteamID group, bool locked)
		{
			for (int i = 0; i < VehicleManager.vehicles.Count; i++)
			{
				if (VehicleManager.vehicles[i].instanceID == instanceID)
				{
					VehicleManager.vehicles[i].tellLocked(owner, group, locked);
					return;
				}
			}
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x000CA18C File Offset: 0x000C838C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellVehicleSkin")]
		public static void ReceiveVehicleSkin(uint instanceID, ushort skinID, ushort mythicID)
		{
			for (int i = 0; i < VehicleManager.vehicles.Count; i++)
			{
				if (VehicleManager.vehicles[i].instanceID == instanceID)
				{
					VehicleManager.vehicles[i].tellSkin(skinID, mythicID);
					return;
				}
			}
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x000CA1D4 File Offset: 0x000C83D4
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellVehicleSirens")]
		public static void ReceiveVehicleSirens(uint instanceID, bool on)
		{
			for (int i = 0; i < VehicleManager.vehicles.Count; i++)
			{
				if (VehicleManager.vehicles[i].instanceID == instanceID)
				{
					VehicleManager.vehicles[i].tellSirens(on);
					return;
				}
			}
		}

		// Token: 0x06002E77 RID: 11895 RVA: 0x000CA21C File Offset: 0x000C841C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellVehicleBlimp")]
		public static void ReceiveVehicleBlimp(uint instanceID, bool on)
		{
			for (int i = 0; i < VehicleManager.vehicles.Count; i++)
			{
				if (VehicleManager.vehicles[i].instanceID == instanceID)
				{
					VehicleManager.vehicles[i].tellBlimp(on);
					return;
				}
			}
		}

		// Token: 0x06002E78 RID: 11896 RVA: 0x000CA264 File Offset: 0x000C8464
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellVehicleHeadlights")]
		public static void ReceiveVehicleHeadlights(uint instanceID, bool on)
		{
			for (int i = 0; i < VehicleManager.vehicles.Count; i++)
			{
				if (VehicleManager.vehicles[i].instanceID == instanceID)
				{
					VehicleManager.vehicles[i].tellHeadlights(on);
					return;
				}
			}
		}

		// Token: 0x06002E79 RID: 11897 RVA: 0x000CA2AC File Offset: 0x000C84AC
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellVehicleHorn")]
		public static void ReceiveVehicleHorn(uint instanceID)
		{
			for (int i = 0; i < VehicleManager.vehicles.Count; i++)
			{
				if (VehicleManager.vehicles[i].instanceID == instanceID)
				{
					VehicleManager.vehicles[i].tellHorn();
					return;
				}
			}
		}

		// Token: 0x06002E7A RID: 11898 RVA: 0x000CA2F4 File Offset: 0x000C84F4
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellVehicleFuel")]
		public static void ReceiveVehicleFuel(uint instanceID, ushort newFuel)
		{
			for (int i = 0; i < VehicleManager.vehicles.Count; i++)
			{
				if (VehicleManager.vehicles[i].instanceID == instanceID)
				{
					VehicleManager.vehicles[i].tellFuel(newFuel);
					return;
				}
			}
		}

		// Token: 0x06002E7B RID: 11899 RVA: 0x000CA33C File Offset: 0x000C853C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellVehicleBatteryCharge")]
		public static void ReceiveVehicleBatteryCharge(uint instanceID, ushort newBatteryCharge)
		{
			for (int i = 0; i < VehicleManager.vehicles.Count; i++)
			{
				if (VehicleManager.vehicles[i].instanceID == instanceID)
				{
					VehicleManager.vehicles[i].tellBatteryCharge(newBatteryCharge);
					return;
				}
			}
		}

		// Token: 0x06002E7C RID: 11900 RVA: 0x000CA384 File Offset: 0x000C8584
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellVehicleTireAliveMask")]
		public static void ReceiveVehicleTireAliveMask(uint instanceID, byte newTireAliveMask)
		{
			for (int i = 0; i < VehicleManager.vehicles.Count; i++)
			{
				if (VehicleManager.vehicles[i].instanceID == instanceID)
				{
					VehicleManager.vehicles[i].tireAliveMask = newTireAliveMask;
					return;
				}
			}
		}

		// Token: 0x06002E7D RID: 11901 RVA: 0x000CA3CC File Offset: 0x000C85CC
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellVehicleExploded")]
		public static void ReceiveVehicleExploded(uint instanceID)
		{
			InteractableVehicle interactableVehicle = VehicleManager.findVehicleByNetInstanceID(instanceID);
			if (interactableVehicle == null || interactableVehicle.isExploded)
			{
				return;
			}
			BarricadeManager.trimPlant(interactableVehicle.transform);
			if (interactableVehicle.trainCars != null)
			{
				for (int i = 1; i < interactableVehicle.trainCars.Length; i++)
				{
					BarricadeManager.uprootPlant(interactableVehicle.trainCars[i].root);
				}
			}
			interactableVehicle.tellExploded();
		}

		// Token: 0x06002E7E RID: 11902 RVA: 0x000CA430 File Offset: 0x000C8630
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellVehicleHealth")]
		public static void ReceiveVehicleHealth(uint instanceID, ushort newHealth)
		{
			for (int i = 0; i < VehicleManager.vehicles.Count; i++)
			{
				if (VehicleManager.vehicles[i].instanceID == instanceID)
				{
					VehicleManager.vehicles[i].tellHealth(newHealth);
					return;
				}
			}
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x000CA478 File Offset: 0x000C8678
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellVehicleRecov")]
		public static void ReceiveVehicleRecov(uint instanceID, Vector3 newPosition, int newRecov)
		{
			for (int i = 0; i < VehicleManager.vehicles.Count; i++)
			{
				if (VehicleManager.vehicles[i].instanceID == instanceID)
				{
					VehicleManager.vehicles[i].tellRecov(newPosition, newRecov);
					return;
				}
			}
		}

		// Token: 0x06002E80 RID: 11904 RVA: 0x000CA4C0 File Offset: 0x000C86C0
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveVehicleStates(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			uint num;
			SystemNetPakReaderEx.ReadUInt32(reader, ref num);
			if (num <= VehicleManager.seq)
			{
				return;
			}
			VehicleManager.seq = num;
			ushort num2;
			SystemNetPakReaderEx.ReadUInt16(reader, ref num2);
			if (num2 < 1)
			{
				return;
			}
			for (ushort num3 = 0; num3 < num2; num3 += 1)
			{
				uint instanceID;
				SystemNetPakReaderEx.ReadUInt32(reader, ref instanceID);
				Vector3 newPosition;
				UnityNetPakReaderEx.ReadClampedVector3(reader, ref newPosition, 13, 8);
				Quaternion newRotation;
				UnityNetPakReaderEx.ReadQuaternion(reader, ref newRotation, 11);
				float newSpeed;
				SystemNetPakReaderEx.ReadUnsignedClampedFloat(reader, 8, 2, ref newSpeed);
				float newForwardVelocity;
				SystemNetPakReaderEx.ReadClampedFloat(reader, 9, 2, ref newForwardVelocity);
				float newReplicatedSteeringInput;
				SystemNetPakReaderEx.ReadSignedNormalizedFloat(reader, 2, ref newReplicatedSteeringInput);
				float newReplicatedVelocityInput;
				SystemNetPakReaderEx.ReadClampedFloat(reader, 9, 2, ref newReplicatedVelocityInput);
				bool flag;
				reader.ReadBit(ref flag);
				InteractableVehicle interactableVehicle = VehicleManager.findVehicleByNetInstanceID(instanceID);
				if (!(interactableVehicle == null))
				{
					interactableVehicle.tellState(newPosition, newRotation, newSpeed, newForwardVelocity, newReplicatedSteeringInput, newReplicatedVelocityInput);
					if (interactableVehicle.asset.replicatedWheelIndices != null)
					{
						foreach (int num4 in interactableVehicle.asset.replicatedWheelIndices)
						{
							Wheel wheelAtIndex = interactableVehicle.GetWheelAtIndex(num4);
							if (wheelAtIndex == null)
							{
								UnturnedLog.error(string.Format("\"{0}\" missing wheel for replicated index: {1}", interactableVehicle.asset.FriendlyName, num4));
							}
							if (flag)
							{
								if (wheelAtIndex == null)
								{
									float num5;
									SystemNetPakReaderEx.ReadUnsignedNormalizedFloat(reader, 4, ref num5);
									PhysicsMaterialNetId physicsMaterialNetId;
									reader.ReadPhysicsMaterialNetId(out physicsMaterialNetId);
								}
								else
								{
									float replicatedSuspensionState;
									if (SystemNetPakReaderEx.ReadUnsignedNormalizedFloat(reader, 4, ref replicatedSuspensionState))
									{
										wheelAtIndex.replicatedSuspensionState = replicatedSuspensionState;
									}
									reader.ReadPhysicsMaterialNetId(out wheelAtIndex.replicatedGroundMaterial);
								}
							}
							else if (wheelAtIndex != null)
							{
								wheelAtIndex.replicatedSuspensionState = 1f;
								wheelAtIndex.replicatedGroundMaterial = PhysicsMaterialNetId.NULL;
							}
						}
					}
					if (interactableVehicle.asset.UsesEngineRpmAndGears)
					{
						if (flag)
						{
							uint num6;
							reader.ReadBits(3, ref num6);
							int num7 = (int)(num6 - 1U);
							num7 = Mathf.Clamp(num7, -1, interactableVehicle.asset.forwardGearRatios.Length);
							interactableVehicle.GearNumber = num7;
							float t;
							SystemNetPakReaderEx.ReadUnsignedNormalizedFloat(reader, 7, ref t);
							interactableVehicle.ReplicatedEngineRpm = Mathf.Lerp(interactableVehicle.asset.EngineIdleRpm, interactableVehicle.asset.EngineMaxRpm, t);
						}
						else
						{
							interactableVehicle.GearNumber = 1;
							interactableVehicle.ReplicatedEngineRpm = interactableVehicle.asset.EngineIdleRpm;
						}
					}
				}
			}
		}

		// Token: 0x06002E81 RID: 11905 RVA: 0x000CA6EC File Offset: 0x000C88EC
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellVehicleDestroy")]
		public static void ReceiveDestroySingleVehicle(uint instanceID)
		{
			InteractableVehicle interactableVehicle = null;
			for (int i = 0; i < VehicleManager.vehicles.Count; i++)
			{
				if (VehicleManager.vehicles[i].instanceID == instanceID)
				{
					interactableVehicle = VehicleManager.vehicles[i];
					VehicleManager.vehicles.RemoveAt(i);
					break;
				}
			}
			if (interactableVehicle == null)
			{
				return;
			}
			BarricadeManager.uprootPlant(interactableVehicle.transform);
			if (interactableVehicle.trainCars != null)
			{
				for (int j = 1; j < interactableVehicle.trainCars.Length; j++)
				{
					BarricadeManager.uprootPlant(interactableVehicle.trainCars[j].root);
				}
			}
			Action<InteractableVehicle> onPreDestroyVehicle = VehicleManager.OnPreDestroyVehicle;
			if (onPreDestroyVehicle != null)
			{
				onPreDestroyVehicle.TryInvoke("OnPreDestroyVehicle", interactableVehicle);
			}
			NetIdRegistry.ReleaseTransform(interactableVehicle.GetNetId() + 1U, interactableVehicle.transform);
			interactableVehicle.ReleaseNetId();
			EffectManager.ClearAttachments(interactableVehicle.transform);
			Object.Destroy(interactableVehicle.gameObject);
			VehicleManager.respawnVehicleIndex -= 1;
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x000CA7D4 File Offset: 0x000C89D4
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellVehicleDestroyAll")]
		public static void ReceiveDestroyAllVehicles()
		{
			for (int i = VehicleManager.vehicles.Count - 1; i >= 0; i--)
			{
				BarricadeManager.uprootPlant(VehicleManager.vehicles[i].transform);
				if (VehicleManager.vehicles[i].trainCars != null)
				{
					for (int j = 1; j < VehicleManager.vehicles[i].trainCars.Length; j++)
					{
						BarricadeManager.uprootPlant(VehicleManager.vehicles[i].trainCars[j].root);
					}
				}
				Action<InteractableVehicle> onPreDestroyVehicle = VehicleManager.OnPreDestroyVehicle;
				if (onPreDestroyVehicle != null)
				{
					onPreDestroyVehicle.TryInvoke("OnPreDestroyVehicle", VehicleManager.vehicles[i]);
				}
				NetIdRegistry.ReleaseTransform(VehicleManager.vehicles[i].GetNetId() + 1U, VehicleManager.vehicles[i].transform);
				VehicleManager.vehicles[i].ReleaseNetId();
				EffectManager.ClearAttachments(VehicleManager.vehicles[i].transform);
				Object.Destroy(VehicleManager.vehicles[i].gameObject);
				VehicleManager.vehicles.RemoveAt(i);
			}
			VehicleManager.respawnVehicleIndex = 0;
			VehicleManager.vehicles.Clear();
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x000CA8FE File Offset: 0x000C8AFE
		public static void askVehicleDestroy(InteractableVehicle vehicle)
		{
			if (Provider.isServer)
			{
				vehicle.forceRemoveAllPlayers();
				VehicleManager.SendDestroySingleVehicle.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), vehicle.instanceID);
			}
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x000CA924 File Offset: 0x000C8B24
		public static void askVehicleDestroyAll()
		{
			if (Provider.isServer)
			{
				for (int i = VehicleManager.vehicles.Count - 1; i >= 0; i--)
				{
					VehicleManager.vehicles[i].forceRemoveAllPlayers();
				}
				VehicleManager.SendDestroyAllVehicles.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections());
			}
		}

		// Token: 0x06002E85 RID: 11909 RVA: 0x000CA970 File Offset: 0x000C8B70
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveSingleVehicle(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			Guid assetGuid;
			SystemNetPakReaderEx.ReadGuid(reader, ref assetGuid);
			ushort skinID;
			SystemNetPakReaderEx.ReadUInt16(reader, ref skinID);
			ushort mythicID;
			SystemNetPakReaderEx.ReadUInt16(reader, ref mythicID);
			Vector3 vector;
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref vector, 13, 8);
			float roadPosition = InteractableVehicle.UnpackRoadPosition(vector);
			Quaternion angle;
			UnityNetPakReaderEx.ReadQuaternion(reader, ref angle, 11);
			bool sirens;
			reader.ReadBit(ref sirens);
			bool blimp;
			reader.ReadBit(ref blimp);
			bool headlights;
			reader.ReadBit(ref headlights);
			bool taillights;
			reader.ReadBit(ref taillights);
			ushort fuel;
			SystemNetPakReaderEx.ReadUInt16(reader, ref fuel);
			bool isExploded;
			reader.ReadBit(ref isExploded);
			ushort health;
			SystemNetPakReaderEx.ReadUInt16(reader, ref health);
			ushort batteryCharge;
			SystemNetPakReaderEx.ReadUInt16(reader, ref batteryCharge);
			CSteamID owner;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref owner);
			CSteamID group;
			SteamworksNetPakReaderEx.ReadSteamID(reader, ref group);
			bool locked;
			reader.ReadBit(ref locked);
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			CSteamID[] array = new CSteamID[(int)b];
			for (int i = 0; i < array.Length; i++)
			{
				SteamworksNetPakReaderEx.ReadSteamID(reader, ref array[i]);
			}
			uint instanceID;
			SystemNetPakReaderEx.ReadUInt32(reader, ref instanceID);
			byte tireAliveMask;
			SystemNetPakReaderEx.ReadUInt8(reader, ref tireAliveMask);
			NetId netId;
			reader.ReadNetId(out netId);
			Color32 paintColor;
			UnityNetPakReaderEx.ReadColor32RGBA(reader, ref paintColor);
			InteractableVehicle interactableVehicle = VehicleManager.manager.addVehicle(assetGuid, skinID, mythicID, roadPosition, vector, angle, sirens, blimp, headlights, taillights, fuel, isExploded, health, batteryCharge, owner, group, locked, array, null, instanceID, tireAliveMask, netId, paintColor);
			byte b2;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b2);
			for (int j = 0; j < (int)b2; j++)
			{
				float state;
				SystemNetPakReaderEx.ReadUnsignedNormalizedFloat(reader, 4, ref state);
				if (interactableVehicle != null && interactableVehicle.asset != null && interactableVehicle.asset.replicatedWheelIndices != null && j < interactableVehicle.asset.replicatedWheelIndices.Length)
				{
					int index = interactableVehicle.asset.replicatedWheelIndices[j];
					Wheel wheelAtIndex = interactableVehicle.GetWheelAtIndex(index);
					if (wheelAtIndex != null)
					{
						wheelAtIndex.TeleportSuspensionState(state);
					}
				}
			}
		}

		// Token: 0x06002E86 RID: 11910 RVA: 0x000CAB34 File Offset: 0x000C8D34
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveMultipleVehicles(in ClientInvocationContext context)
		{
			ushort num;
			SystemNetPakReaderEx.ReadUInt16(context.reader, ref num);
			for (int i = 0; i < (int)num; i++)
			{
				VehicleManager.ReceiveSingleVehicle(context);
			}
			Level.isLoadingVehicles = false;
		}

		/// <summary>
		/// Helper for servers with huge numbers of vehicles.
		/// Called with fixed span of indexes e.g. [0, 10), then [10, 20). This function then clamps the final span to the vehicle count.
		/// </summary>
		// Token: 0x06002E87 RID: 11911 RVA: 0x000CAB68 File Offset: 0x000C8D68
		private static void askVehiclesHelper(ITransportConnection transportConnection, int startIndex, int endIndex)
		{
			if (endIndex > VehicleManager.vehicles.Count)
			{
				endIndex = VehicleManager.vehicles.Count;
			}
			int count = endIndex - startIndex;
			if (count < 1)
			{
				throw new ArgumentException("startIndex or endIndex to askVehiclesHelper invalid");
			}
			VehicleManager.SendMultipleVehicles.Invoke(ENetReliability.Reliable, transportConnection, delegate(NetPakWriter writer)
			{
				SystemNetPakWriterEx.WriteUInt16(writer, (ushort)count);
				for (int i = startIndex; i < endIndex; i++)
				{
					VehicleManager.sendVehicle(VehicleManager.vehicles[i], writer);
				}
			});
		}

		// Token: 0x06002E88 RID: 11912 RVA: 0x000CABEC File Offset: 0x000C8DEC
		internal static void SendInitialGlobalState(SteamPlayer client)
		{
			int count = VehicleManager.vehicles.Count;
			if (count > 0)
			{
				int num = (count - 1) / 50 + 1;
				int num2 = 0;
				for (int i = 0; i < num; i++)
				{
					int num3 = num2 + 50;
					VehicleManager.askVehiclesHelper(client.transportConnection, num2, num3);
					num2 = num3;
				}
			}
			else
			{
				VehicleManager.SendMultipleVehicles.Invoke(ENetReliability.Reliable, client.transportConnection, delegate(NetPakWriter writer)
				{
					SystemNetPakWriterEx.WriteUInt16(writer, 0);
				});
			}
			BarricadeManager.SendVehicleRegions(client);
		}

		// Token: 0x06002E89 RID: 11913 RVA: 0x000CAC70 File Offset: 0x000C8E70
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellEnterVehicle")]
		public static void ReceiveEnterVehicle(uint instanceID, byte seat, CSteamID player)
		{
			for (int i = 0; i < VehicleManager.vehicles.Count; i++)
			{
				if (VehicleManager.vehicles[i].instanceID == instanceID)
				{
					VehicleManager.vehicles[i].addPlayer(seat, player);
					return;
				}
			}
		}

		// Token: 0x06002E8A RID: 11914 RVA: 0x000CACB8 File Offset: 0x000C8EB8
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellExitVehicle")]
		public static void ReceiveExitVehicle(uint instanceID, byte seat, Vector3 point, byte angle, bool forceUpdate)
		{
			InteractableVehicle interactableVehicle = VehicleManager.findVehicleByNetInstanceID(instanceID);
			if (interactableVehicle != null)
			{
				interactableVehicle.removePlayer(seat, point, angle, forceUpdate);
			}
		}

		// Token: 0x06002E8B RID: 11915 RVA: 0x000CACE0 File Offset: 0x000C8EE0
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellSwapVehicle")]
		public static void ReceiveSwapVehicleSeats(uint instanceID, byte fromSeat, byte toSeat)
		{
			InteractableVehicle interactableVehicle = VehicleManager.findVehicleByNetInstanceID(instanceID);
			if (interactableVehicle != null)
			{
				interactableVehicle.swapPlayer(fromSeat, toSeat);
			}
		}

		// Token: 0x06002E8C RID: 11916 RVA: 0x000CAD08 File Offset: 0x000C8F08
		public static void unlockVehicle(InteractableVehicle vehicle, Player instigatingPlayer)
		{
			if (vehicle == null)
			{
				return;
			}
			bool flag = true;
			VehicleLockpickedSignature vehicleLockpickedSignature = VehicleManager.onVehicleLockpicked;
			if (vehicleLockpickedSignature != null)
			{
				vehicleLockpickedSignature(vehicle, instigatingPlayer, ref flag);
			}
			if (!flag)
			{
				return;
			}
			VehicleManager.ServerSetVehicleLock(vehicle, CSteamID.Nil, CSteamID.Nil, false);
			EffectManager.TriggerFiremodeEffect(vehicle.transform.position);
		}

		// Token: 0x06002E8D RID: 11917 RVA: 0x000CAD5C File Offset: 0x000C8F5C
		public static void carjackVehicle(InteractableVehicle vehicle, Player instigatingPlayer, Vector3 force, Vector3 torque)
		{
			if (!vehicle.isEmpty)
			{
				return;
			}
			if (vehicle.asset != null)
			{
				VehiclePhysicsProfileAsset vehiclePhysicsProfileAsset = vehicle.asset.physicsProfileRef.Find();
				if (vehiclePhysicsProfileAsset != null && vehiclePhysicsProfileAsset.carjackForceMultiplier != null)
				{
					force *= vehiclePhysicsProfileAsset.carjackForceMultiplier.Value;
				}
				force *= vehicle.asset.carjackForceMultiplier;
			}
			bool flag = true;
			VehicleCarjackedSignature vehicleCarjackedSignature = VehicleManager.onVehicleCarjacked;
			if (vehicleCarjackedSignature != null)
			{
				vehicleCarjackedSignature(vehicle, instigatingPlayer, ref flag, ref force, ref torque);
			}
			if (!flag)
			{
				return;
			}
			Rigidbody component = vehicle.GetComponent<Rigidbody>();
			if (component)
			{
				component.AddForce(force);
				component.AddTorque(torque);
			}
		}

		// Token: 0x06002E8E RID: 11918 RVA: 0x000CAE08 File Offset: 0x000C9008
		public static ushort siphonFromVehicle(InteractableVehicle vehicle, Player instigatingPlayer, ushort desiredAmount)
		{
			bool flag = true;
			SiphonVehicleRequestHandler siphonVehicleRequestHandler = VehicleManager.onSiphonVehicleRequested;
			if (siphonVehicleRequestHandler != null)
			{
				siphonVehicleRequestHandler(vehicle, instigatingPlayer, ref flag, ref desiredAmount);
			}
			if (!flag)
			{
				return 0;
			}
			if (desiredAmount > vehicle.fuel)
			{
				desiredAmount = vehicle.fuel;
			}
			if (desiredAmount < 1)
			{
				return 0;
			}
			vehicle.askBurnFuel(desiredAmount);
			VehicleManager.sendVehicleFuel(vehicle, vehicle.fuel);
			return desiredAmount;
		}

		// Token: 0x140000A6 RID: 166
		// (add) Token: 0x06002E8F RID: 11919 RVA: 0x000CAE60 File Offset: 0x000C9060
		// (remove) Token: 0x06002E90 RID: 11920 RVA: 0x000CAE94 File Offset: 0x000C9094
		public static event VehicleManager.ToggleVehicleLockRequested OnToggleVehicleLockRequested;

		// Token: 0x140000A7 RID: 167
		// (add) Token: 0x06002E91 RID: 11921 RVA: 0x000CAEC8 File Offset: 0x000C90C8
		// (remove) Token: 0x06002E92 RID: 11922 RVA: 0x000CAEFC File Offset: 0x000C90FC
		public static event Action<InteractableVehicle> OnToggledVehicleLock;

		// Token: 0x06002E93 RID: 11923 RVA: 0x000CAF2F File Offset: 0x000C912F
		public static void ServerSetVehicleLock(InteractableVehicle vehicle, CSteamID ownerID, CSteamID groupID, bool isLocked)
		{
			VehicleManager.SendVehicleLockState.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), vehicle.instanceID, ownerID, groupID, isLocked);
		}

		// Token: 0x06002E94 RID: 11924 RVA: 0x000CAF4C File Offset: 0x000C914C
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 4, legacyName = "askVehicleLock")]
		public static void ReceiveVehicleLockRequest(in ServerInvocationContext context)
		{
			Player player = context.GetPlayer();
			if (player == null)
			{
				return;
			}
			InteractableVehicle vehicle = player.movement.getVehicle();
			if (vehicle == null || vehicle.asset == null)
			{
				return;
			}
			if (!vehicle.checkDriver(player.channel.owner.playerID.steamID))
			{
				return;
			}
			bool isLocked = vehicle.isLocked;
			bool flag = vehicle.asset.canBeLocked && !isLocked;
			if (isLocked == flag)
			{
				return;
			}
			bool flag2 = true;
			VehicleManager.ToggleVehicleLockRequested onToggleVehicleLockRequested = VehicleManager.OnToggleVehicleLockRequested;
			if (onToggleVehicleLockRequested != null)
			{
				onToggleVehicleLockRequested(vehicle, ref flag2);
			}
			if (!flag2)
			{
				return;
			}
			VehicleManager.ServerSetVehicleLock(vehicle, player.channel.owner.playerID.steamID, player.quests.groupID, flag);
			EffectManager.TriggerFiremodeEffect(vehicle.transform.position);
			VehicleManager.OnToggledVehicleLock.TryInvoke("OnToggledVehicleLock", vehicle);
		}

		// Token: 0x06002E95 RID: 11925 RVA: 0x000CB02C File Offset: 0x000C922C
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 2, legacyName = "askVehicleSkin")]
		public static void ReceiveVehicleSkinRequest(in ServerInvocationContext context)
		{
			Player player = context.GetPlayer();
			if (player == null)
			{
				return;
			}
			InteractableVehicle vehicle = player.movement.getVehicle();
			if (vehicle == null)
			{
				return;
			}
			if (!vehicle.checkDriver(player.channel.owner.playerID.steamID))
			{
				return;
			}
			int item = 0;
			ushort num = 0;
			ushort num2 = 0;
			if (player.channel.owner.skinItems != null && player.channel.owner.GetVehicleSkinItemDefId(vehicle, out item))
			{
				num = Provider.provider.economyService.getInventorySkinID(item);
				num2 = Provider.provider.economyService.getInventoryMythicID(item);
			}
			if (num != 0)
			{
				if (num == vehicle.skinID && num2 == vehicle.mythicID)
				{
					num = 0;
					num2 = 0;
				}
			}
			else if (!vehicle.isSkinned)
			{
				return;
			}
			VehicleManager.SendVehicleSkin.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), vehicle.instanceID, num, num2);
		}

		// Token: 0x06002E96 RID: 11926 RVA: 0x000CB110 File Offset: 0x000C9310
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 10, legacyName = "askVehicleHeadlights")]
		public static void ReceiveToggleVehicleHeadlights(in ServerInvocationContext context, bool wantsHeadlightsOn)
		{
			Player player = context.GetPlayer();
			if (player == null)
			{
				return;
			}
			InteractableVehicle vehicle = player.movement.getVehicle();
			if (vehicle == null)
			{
				return;
			}
			if (wantsHeadlightsOn == vehicle.headlightsOn)
			{
				return;
			}
			if (!vehicle.canTurnOnLights)
			{
				return;
			}
			if (!vehicle.checkDriver(player.channel.owner.playerID.steamID))
			{
				return;
			}
			if (!vehicle.asset.hasHeadlights)
			{
				return;
			}
			VehicleManager.SendVehicleHeadlights.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), vehicle.instanceID, wantsHeadlightsOn);
			EffectManager.TriggerFiremodeEffect(vehicle.transform.position);
		}

		// Token: 0x06002E97 RID: 11927 RVA: 0x000CB1AC File Offset: 0x000C93AC
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 5, legacyName = "askVehicleBonus")]
		public static void ReceiveUseVehicleBonus(in ServerInvocationContext context, byte bonusType)
		{
			Player player = context.GetPlayer();
			if (player == null)
			{
				return;
			}
			InteractableVehicle vehicle = player.movement.getVehicle();
			if (vehicle == null)
			{
				return;
			}
			if (!vehicle.checkDriver(player.channel.owner.playerID.steamID))
			{
				return;
			}
			if (bonusType == 0)
			{
				if (!vehicle.canTurnOnLights)
				{
					return;
				}
				VehicleManager.SendVehicleSirens.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), vehicle.instanceID, !vehicle.sirensOn);
				EffectManager.TriggerFiremodeEffect(vehicle.transform.position);
				return;
			}
			else
			{
				if (bonusType == 1)
				{
					vehicle.useHook();
					return;
				}
				if (bonusType == 2)
				{
					VehicleManager.SendVehicleBlimp.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), vehicle.instanceID, !vehicle.isBlimpFloating);
				}
				return;
			}
		}

		// Token: 0x06002E98 RID: 11928 RVA: 0x000CB268 File Offset: 0x000C9468
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 2, legacyName = "askVehicleStealBattery")]
		public static void ReceiveStealVehicleBattery(in ServerInvocationContext context)
		{
			Player player = context.GetPlayer();
			if (player == null)
			{
				return;
			}
			InteractableVehicle vehicle = player.movement.getVehicle();
			if (vehicle == null)
			{
				return;
			}
			if (!vehicle.checkDriver(player.channel.owner.playerID.steamID))
			{
				return;
			}
			if (!vehicle.usesBattery)
			{
				return;
			}
			if (!vehicle.ContainsBatteryItem)
			{
				return;
			}
			if (!vehicle.asset.canStealBattery)
			{
				return;
			}
			vehicle.stealBattery(player);
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x000CB2E4 File Offset: 0x000C94E4
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 10, legacyName = "askVehicleHorn")]
		public static void ReceiveVehicleHornRequest(in ServerInvocationContext context)
		{
			Player player = context.GetPlayer();
			if (player == null)
			{
				return;
			}
			InteractableVehicle vehicle = player.movement.getVehicle();
			if (vehicle == null)
			{
				return;
			}
			if (!vehicle.asset.hasHorn)
			{
				return;
			}
			if (!vehicle.canUseHorn)
			{
				return;
			}
			if (!vehicle.checkDriver(player.channel.owner.playerID.steamID))
			{
				return;
			}
			VehicleManager.SendVehicleHorn.InvokeAndLoopback(ENetReliability.Unreliable, Provider.GatherRemoteClientConnections(), vehicle.instanceID);
		}

		// Token: 0x06002E9A RID: 11930 RVA: 0x000CB364 File Offset: 0x000C9564
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 2, legacyName = "askEnterVehicle")]
		public static void ReceiveEnterVehicleRequest(in ServerInvocationContext context, uint instanceID, byte[] hash, byte[] physicsProfileHash, byte engine)
		{
			Player player = context.GetPlayer();
			if (player == null)
			{
				return;
			}
			if (player.life.isDead)
			{
				return;
			}
			if (player.equipment.isBusy)
			{
				return;
			}
			if (LevelManager.isArenaMode && !LevelManager.isPlayerInArena(player))
			{
				return;
			}
			if (player.equipment.HasValidUseable && !player.equipment.IsEquipAnimationFinished)
			{
				return;
			}
			if (player.movement.getVehicle() != null)
			{
				return;
			}
			InteractableVehicle interactableVehicle = null;
			for (int i = 0; i < VehicleManager.vehicles.Count; i++)
			{
				if (VehicleManager.vehicles[i].instanceID == instanceID)
				{
					interactableVehicle = VehicleManager.vehicles[i];
					break;
				}
			}
			if (interactableVehicle == null)
			{
				return;
			}
			if (interactableVehicle.asset.shouldVerifyHash && !Hash.verifyHash(hash, interactableVehicle.asset.hash))
			{
				return;
			}
			if (physicsProfileHash.Length == 0)
			{
				if (interactableVehicle.asset.physicsProfileRef.Find() != null)
				{
					return;
				}
			}
			else
			{
				if (physicsProfileHash.Length != 20)
				{
					context.Kick("invalid vehicle physics profile hash");
					return;
				}
				VehiclePhysicsProfileAsset vehiclePhysicsProfileAsset = interactableVehicle.asset.physicsProfileRef.Find();
				if (vehiclePhysicsProfileAsset == null)
				{
					return;
				}
				if (!Hash.verifyHash(physicsProfileHash, vehiclePhysicsProfileAsset.hash))
				{
					return;
				}
			}
			if ((EEngine)engine != interactableVehicle.asset.engine)
			{
				return;
			}
			if ((interactableVehicle.transform.position - player.transform.position).sqrMagnitude > 100f)
			{
				return;
			}
			if (!interactableVehicle.checkEnter(player))
			{
				return;
			}
			byte b;
			if (!interactableVehicle.tryAddPlayer(out b, player))
			{
				return;
			}
			Transform seat = interactableVehicle.passengers[(int)b].seat;
			Vector3 position = seat.position;
			Vector3 end = seat.position + seat.up * 2f;
			Vector3 start = player.transform.position + Vector3.up;
			RaycastHit raycastHit;
			bool flag = Physics.Linecast(start, position, out raycastHit, RayMasks.BLOCK_ENTRY, QueryTriggerInteraction.Ignore);
			if (!flag)
			{
				flag = Physics.Linecast(start, end, out raycastHit, RayMasks.BLOCK_ENTRY, QueryTriggerInteraction.Ignore);
			}
			if (flag && !raycastHit.transform.IsChildOf(interactableVehicle.transform))
			{
				return;
			}
			if (VehicleManager.onEnterVehicleRequested != null)
			{
				bool flag2 = true;
				VehicleManager.onEnterVehicleRequested(player, interactableVehicle, ref flag2);
				if (!flag2)
				{
					return;
				}
			}
			VehicleManager.SendEnterVehicle.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), instanceID, b, player.channel.owner.playerID.steamID);
		}

		/// <summary>
		/// Does as few tests as possible while maintaining base game expectations.
		/// </summary>
		// Token: 0x06002E9B RID: 11931 RVA: 0x000CB5C8 File Offset: 0x000C97C8
		public static bool ServerForcePassengerIntoVehicle(Player player, InteractableVehicle vehicle)
		{
			if (player == null)
			{
				throw new ArgumentNullException("player");
			}
			if (vehicle == null)
			{
				throw new ArgumentNullException("vehicle");
			}
			if (player.life.isDead)
			{
				return false;
			}
			if (player.equipment.isBusy)
			{
				return false;
			}
			if (player.equipment.HasValidUseable && !player.equipment.IsEquipAnimationFinished)
			{
				return false;
			}
			if (player.movement.getVehicle() != null)
			{
				return false;
			}
			byte arg;
			if (!vehicle.tryAddPlayer(out arg, player))
			{
				return false;
			}
			VehicleManager.SendEnterVehicle.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), vehicle.instanceID, arg, player.channel.owner.playerID.steamID);
			return true;
		}

		// Token: 0x06002E9C RID: 11932 RVA: 0x000CB688 File Offset: 0x000C9888
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 2, legacyName = "askExitVehicle")]
		public static void ReceiveExitVehicleRequest(in ServerInvocationContext context, Vector3 velocity)
		{
			Player player = context.GetPlayer();
			if (player == null)
			{
				return;
			}
			if (player.life.isDead)
			{
				return;
			}
			if (player.equipment.isBusy)
			{
				return;
			}
			InteractableVehicle vehicle = player.movement.getVehicle();
			if (vehicle == null)
			{
				return;
			}
			byte b;
			Vector3 point;
			byte angle;
			if (!vehicle.forceRemovePlayer(out b, player.channel.owner.playerID.steamID, out point, out angle))
			{
				return;
			}
			if (VehicleManager.onExitVehicleRequested != null)
			{
				bool flag = true;
				float angle2 = MeasurementTool.byteToAngle(angle);
				VehicleManager.onExitVehicleRequested(player, vehicle, ref flag, ref point, ref angle2);
				angle = MeasurementTool.angleToByte(angle2);
				if (!flag)
				{
					return;
				}
			}
			VehicleManager.sendExitVehicle(vehicle, b, point, angle, false);
			if (b == 0)
			{
				vehicle.GetComponent<Rigidbody>().velocity = velocity;
			}
		}

		// Token: 0x06002E9D RID: 11933 RVA: 0x000CB74C File Offset: 0x000C994C
		public static void forceRemovePlayer(InteractableVehicle vehicle, CSteamID player)
		{
			byte seat;
			Vector3 point;
			byte angle;
			if (vehicle.forceRemovePlayer(out seat, player, out point, out angle))
			{
				VehicleManager.sendExitVehicle(vehicle, seat, point, angle, true);
			}
		}

		/// <summary>
		/// Force remove player from vehicle they were in, if any.
		/// Called when player disconnects to tidy up and run callbacks.
		/// </summary>
		/// <returns>True if player was in a vehicle, false otherwise.</returns>
		// Token: 0x06002E9E RID: 11934 RVA: 0x000CB774 File Offset: 0x000C9974
		public static bool forceRemovePlayer(CSteamID player)
		{
			InteractableVehicle interactableVehicle = null;
			byte seat = 0;
			Vector3 zero = Vector3.zero;
			byte angle = 0;
			foreach (InteractableVehicle interactableVehicle2 in VehicleManager.vehicles)
			{
				if (!(interactableVehicle2 == null) && interactableVehicle2.forceRemovePlayer(out seat, player, out zero, out angle))
				{
					interactableVehicle = interactableVehicle2;
					break;
				}
			}
			if (interactableVehicle != null)
			{
				VehicleManager.sendExitVehicle(interactableVehicle, seat, zero, angle, true);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Remove player from vehicle and teleport them to an unchecked destination.
		/// </summary>
		// Token: 0x06002E9F RID: 11935 RVA: 0x000CB804 File Offset: 0x000C9A04
		public static bool removePlayerTeleportUnsafe(InteractableVehicle vehicle, Player player, Vector3 position, float yaw)
		{
			byte seat;
			if (vehicle.findPlayerSeat(player, out seat))
			{
				byte angle = MeasurementTool.angleToByte(yaw);
				VehicleManager.sendExitVehicle(vehicle, seat, position, angle, false);
				return true;
			}
			return false;
		}

		// Token: 0x06002EA0 RID: 11936 RVA: 0x000CB830 File Offset: 0x000C9A30
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 2, legacyName = "askSwapVehicle")]
		public static void ReceiveSwapVehicleRequest(in ServerInvocationContext context, byte toSeat)
		{
			Player player = context.GetPlayer();
			if (player == null)
			{
				return;
			}
			if (player.life.isDead)
			{
				return;
			}
			if (player.equipment.isBusy)
			{
				return;
			}
			if (player.equipment.HasValidUseable && !player.equipment.IsEquipAnimationFinished)
			{
				return;
			}
			InteractableVehicle vehicle = player.movement.getVehicle();
			if (vehicle == null)
			{
				return;
			}
			if (Time.realtimeSinceStartup - vehicle.lastSeat < 1f)
			{
				return;
			}
			vehicle.lastSeat = Time.realtimeSinceStartup;
			byte b;
			if (!vehicle.trySwapPlayer(player, toSeat, out b))
			{
				return;
			}
			if (VehicleManager.onSwapSeatRequested != null)
			{
				bool flag = true;
				VehicleManager.onSwapSeatRequested(player, vehicle, ref flag, b, ref toSeat);
				if (!flag)
				{
					return;
				}
				if (!vehicle.trySwapPlayer(player, toSeat, out b))
				{
					return;
				}
			}
			VehicleManager.SendSwapVehicleSeats.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), vehicle.instanceID, b, toSeat);
		}

		// Token: 0x06002EA1 RID: 11937 RVA: 0x000CB90A File Offset: 0x000C9B0A
		public static void sendExitVehicle(InteractableVehicle vehicle, byte seat, Vector3 point, byte angle, bool forceUpdate)
		{
			VehicleManager.SendExitVehicle.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), vehicle.instanceID, seat, point, angle, forceUpdate);
		}

		// Token: 0x06002EA2 RID: 11938 RVA: 0x000CB927 File Offset: 0x000C9B27
		public static void sendVehicleFuel(InteractableVehicle vehicle, ushort newFuel)
		{
			VehicleManager.SendVehicleFuel.Invoke(ENetReliability.Unreliable, Provider.GatherClientConnections(), vehicle.instanceID, newFuel);
		}

		// Token: 0x06002EA3 RID: 11939 RVA: 0x000CB940 File Offset: 0x000C9B40
		public static void sendVehicleBatteryCharge(InteractableVehicle vehicle, ushort newBatteryCharge)
		{
			VehicleManager.SendVehicleBatteryCharge.InvokeAndLoopback(ENetReliability.Unreliable, Provider.GatherRemoteClientConnections(), vehicle.instanceID, newBatteryCharge);
		}

		// Token: 0x06002EA4 RID: 11940 RVA: 0x000CB959 File Offset: 0x000C9B59
		public static void sendVehicleTireAliveMask(InteractableVehicle vehicle, byte newTireAliveMask)
		{
			VehicleManager.SendVehicleTireAliveMask.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), vehicle.instanceID, newTireAliveMask);
		}

		// Token: 0x06002EA5 RID: 11941 RVA: 0x000CB972 File Offset: 0x000C9B72
		public static void sendVehicleExploded(InteractableVehicle vehicle)
		{
			VehicleManager.SendVehicleExploded.InvokeAndLoopback(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), vehicle.instanceID);
			VehicleManager.OnVehicleExploded.TryInvoke("OnVehicleExploded", vehicle);
		}

		// Token: 0x06002EA6 RID: 11942 RVA: 0x000CB99A File Offset: 0x000C9B9A
		public static void sendVehicleHealth(InteractableVehicle vehicle, ushort newHealth)
		{
			VehicleManager.SendVehicleHealth.InvokeAndLoopback(ENetReliability.Unreliable, Provider.GatherRemoteClientConnections(), vehicle.instanceID, newHealth);
		}

		// Token: 0x06002EA7 RID: 11943 RVA: 0x000CB9B3 File Offset: 0x000C9BB3
		public static void sendVehicleRecov(InteractableVehicle vehicle, Vector3 newPosition, int newRecov)
		{
			if (vehicle.passengers[0].player != null)
			{
				VehicleManager.SendVehicleRecov.Invoke(ENetReliability.Reliable, vehicle.passengers[0].player.transportConnection, vehicle.instanceID, newPosition, newRecov);
			}
		}

		// Token: 0x06002EA8 RID: 11944 RVA: 0x000CB9EC File Offset: 0x000C9BEC
		private InteractableVehicle addVehicle(Guid assetGuid, ushort skinID, ushort mythicID, float roadPosition, Vector3 point, Quaternion angle, bool sirens, bool blimp, bool headlights, bool taillights, ushort fuel, bool isExploded, ushort health, ushort batteryCharge, CSteamID owner, CSteamID group, bool locked, CSteamID[] passengers, byte[][] turrets, uint instanceID, byte tireAliveMask, NetId netId, Color32 paintColor)
		{
			ThreadUtil.ConditionalAssertIsGameThread();
			VehicleAsset vehicleAsset = Assets.find(assetGuid) as VehicleAsset;
			if (!Provider.isServer)
			{
				ClientAssetIntegrity.QueueRequest(assetGuid, vehicleAsset, "Vehicle");
			}
			if (vehicleAsset == null)
			{
				return null;
			}
			GameObject orLoadModel = vehicleAsset.GetOrLoadModel();
			if (orLoadModel == null)
			{
				Assets.reportError(vehicleAsset, "unable to spawn any gameobject");
				return null;
			}
			if (!vehicleAsset.canBeLocked)
			{
				owner = CSteamID.Nil;
				group = CSteamID.Nil;
				locked = false;
			}
			InteractableVehicle interactableVehicle = null;
			try
			{
				Transform transform = Object.Instantiate<GameObject>(orLoadModel, point, angle).transform;
				transform.name = vehicleAsset.id.ToString();
				Rigidbody orAddComponent = transform.GetOrAddComponent<Rigidbody>();
				orAddComponent.useGravity = true;
				orAddComponent.isKinematic = false;
				interactableVehicle = transform.gameObject.AddComponent<InteractableVehicle>();
				interactableVehicle.roadPosition = roadPosition;
				interactableVehicle.instanceID = instanceID;
				interactableVehicle.AssignNetId(netId);
				interactableVehicle.id = vehicleAsset.id;
				interactableVehicle.skinID = skinID;
				interactableVehicle.mythicID = mythicID;
				interactableVehicle.fuel = fuel;
				interactableVehicle.isExploded = isExploded;
				interactableVehicle.health = health;
				interactableVehicle.batteryCharge = batteryCharge;
				interactableVehicle.PaintColor = paintColor;
				interactableVehicle.init(vehicleAsset);
				interactableVehicle.gatherVehicleColliders();
				interactableVehicle.tellSirens(sirens);
				interactableVehicle.tellBlimp(blimp);
				interactableVehicle.tellHeadlights(headlights);
				interactableVehicle.tellTaillights(taillights);
				interactableVehicle.tellLocked(owner, group, locked);
				interactableVehicle.tireAliveMask = tireAliveMask;
				if (Provider.isServer)
				{
					if (turrets != null && turrets.Length == interactableVehicle.turrets.Length)
					{
						byte b = 0;
						while ((int)b < interactableVehicle.turrets.Length)
						{
							interactableVehicle.turrets[(int)b].state = turrets[(int)b];
							b += 1;
						}
					}
					else
					{
						byte b2 = 0;
						while ((int)b2 < interactableVehicle.turrets.Length)
						{
							ItemAsset itemAsset = Assets.find(EAssetType.ITEM, vehicleAsset.turrets[(int)b2].itemID) as ItemAsset;
							if (itemAsset != null)
							{
								interactableVehicle.turrets[(int)b2].state = itemAsset.getState();
							}
							else
							{
								interactableVehicle.turrets[(int)b2].state = null;
							}
							b2 += 1;
						}
					}
				}
				if (passengers != null)
				{
					byte b3 = 0;
					while ((int)b3 < passengers.Length)
					{
						if (passengers[(int)b3] != CSteamID.Nil)
						{
							interactableVehicle.addPlayer(b3, passengers[(int)b3]);
						}
						b3 += 1;
					}
				}
				if (vehicleAsset.trunkStorage_Y > 0)
				{
					interactableVehicle.trunkItems = new Items(PlayerInventory.STORAGE);
					interactableVehicle.trunkItems.resize(vehicleAsset.trunkStorage_X, vehicleAsset.trunkStorage_Y);
				}
				VehicleManager.vehicles.Add(interactableVehicle);
				NetIdRegistry.AssignTransform(netId = ++netId, interactableVehicle.transform);
				BarricadeManager.registerVehicleRegion(interactableVehicle.transform, interactableVehicle, 0, netId = ++netId);
				if (interactableVehicle.trainCars != null)
				{
					for (int i = 1; i < interactableVehicle.trainCars.Length; i++)
					{
						BarricadeManager.registerVehicleRegion(interactableVehicle.trainCars[i].root, interactableVehicle, i, netId = ++netId);
					}
				}
			}
			catch (Exception e)
			{
				UnturnedLog.warn("Exception while spawning vehicle: {0}", new object[]
				{
					vehicleAsset.name
				});
				UnturnedLog.exception(e);
			}
			return interactableVehicle;
		}

		/// <summary>
		/// Is spawnpoint open for vehicle?
		/// </summary>
		// Token: 0x06002EA9 RID: 11945 RVA: 0x000CBCFC File Offset: 0x000C9EFC
		private bool canUseSpawnpoint(VehicleSpawnpoint spawn)
		{
			foreach (InteractableVehicle interactableVehicle in VehicleManager.vehicles)
			{
				if (!(interactableVehicle == null) && (interactableVehicle.transform.position - spawn.point).sqrMagnitude < 64f)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Try to find a random spawnpoint to spawn a vehicle while server is running.
		/// </summary>
		// Token: 0x06002EAA RID: 11946 RVA: 0x000CBD7C File Offset: 0x000C9F7C
		private VehicleSpawnpoint findRandomSpawn()
		{
			List<VehicleSpawnpoint> spawns = LevelVehicles.spawns;
			if (spawns.Count < 1)
			{
				return null;
			}
			int num = Random.Range(0, spawns.Count);
			VehicleSpawnpoint vehicleSpawnpoint = spawns[num];
			if (vehicleSpawnpoint != null && this.canUseSpawnpoint(vehicleSpawnpoint))
			{
				return vehicleSpawnpoint;
			}
			return null;
		}

		/// <summary>
		/// Add a new vehicle at given spawnpoint.
		/// Supports redirects by VehicleRedirectorAsset. If redirector's SpawnPaintColor is set, that color is used.
		/// </summary>
		// Token: 0x06002EAB RID: 11947 RVA: 0x000CBDC0 File Offset: 0x000C9FC0
		private InteractableVehicle addVehicleAtSpawn(VehicleSpawnpoint spawn)
		{
			if (spawn == null)
			{
				return null;
			}
			Asset randomAssetForSpawnpoint = LevelVehicles.GetRandomAssetForSpawnpoint(spawn);
			if (randomAssetForSpawnpoint == null)
			{
				return null;
			}
			Color32 value = new Color32(0, 0, 0, 0);
			VehicleRedirectorAsset vehicleRedirectorAsset = randomAssetForSpawnpoint as VehicleRedirectorAsset;
			VehicleAsset vehicleAsset;
			if (vehicleRedirectorAsset != null)
			{
				vehicleAsset = vehicleRedirectorAsset.TargetVehicle.Find();
				if (vehicleRedirectorAsset.SpawnPaintColor != null)
				{
					value = vehicleRedirectorAsset.SpawnPaintColor.Value;
				}
			}
			else
			{
				vehicleAsset = (randomAssetForSpawnpoint as VehicleAsset);
			}
			if (vehicleAsset == null)
			{
				return null;
			}
			Vector3 point = spawn.point;
			point.y += 0.5f;
			NetId netId = NetIdRegistry.ClaimBlock(21U);
			return this.addVehicle(vehicleAsset.GUID, 0, 0, 0f, point, Quaternion.Euler(0f, spawn.angle, 0f), false, false, false, false, ushort.MaxValue, false, ushort.MaxValue, ushort.MaxValue, CSteamID.Nil, CSteamID.Nil, false, null, null, VehicleManager.allocateInstanceID(), VehicleManager.getVehicleRandomTireAliveMask(vehicleAsset), netId, value);
		}

		/// <summary>
		/// Add a new vehicle at given spawnpoint and replicate to clients.
		/// Supports redirects by VehicleRedirectorAsset. If redirector's SpawnPaintColor is set, that color is used.
		/// </summary>
		// Token: 0x06002EAC RID: 11948 RVA: 0x000CBEAC File Offset: 0x000CA0AC
		private void addVehicleAtSpawnAndReplicate(VehicleSpawnpoint spawn)
		{
			InteractableVehicle character = this.addVehicleAtSpawn(spawn);
			if (character != null)
			{
				VehicleManager.SendSingleVehicle.Invoke(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), delegate(NetPakWriter writer)
				{
					VehicleManager.sendVehicle(character, writer);
				});
			}
		}

		// Token: 0x06002EAD RID: 11949 RVA: 0x000CBEF8 File Offset: 0x000CA0F8
		private bool respawnVehicles_Destroy()
		{
			if ((int)VehicleManager.respawnVehicleIndex >= VehicleManager.vehicles.Count)
			{
				VehicleManager.respawnVehicleIndex = (ushort)(VehicleManager.vehicles.Count - 1);
			}
			InteractableVehicle interactableVehicle = VehicleManager.vehicles[(int)VehicleManager.respawnVehicleIndex];
			VehicleManager.respawnVehicleIndex += 1;
			if ((int)VehicleManager.respawnVehicleIndex >= VehicleManager.vehicles.Count)
			{
				VehicleManager.respawnVehicleIndex = 0;
			}
			if (interactableVehicle == null || interactableVehicle.asset == null)
			{
				return false;
			}
			if (interactableVehicle.asset.engine == EEngine.TRAIN)
			{
				return false;
			}
			if (!interactableVehicle.isEmpty)
			{
				return false;
			}
			float respawn_Time = Provider.modeConfigData.Vehicles.Respawn_Time;
			if (false | (interactableVehicle.isExploded && Time.realtimeSinceStartup - interactableVehicle.lastExploded > respawn_Time) | (interactableVehicle.isDrowned && Time.realtimeSinceStartup - interactableVehicle.lastUnderwater > respawn_Time))
			{
				VehicleManager.askVehicleDestroy(interactableVehicle);
				return true;
			}
			return false;
		}

		// Token: 0x06002EAE RID: 11950 RVA: 0x000CBFDC File Offset: 0x000CA1DC
		private void despawnAndRespawnVehicles()
		{
			if (Level.info == null || Level.info.type == ELevelType.ARENA)
			{
				return;
			}
			if (VehicleManager.vehicles == null)
			{
				return;
			}
			if (VehicleManager.vehicles.Count > 0 && this.respawnVehicles_Destroy())
			{
				return;
			}
			if (LevelVehicles.spawns == null || LevelVehicles.spawns.Count == 0)
			{
				return;
			}
			if ((long)VehicleManager.vehicles.Count < (long)((ulong)VehicleManager.maxInstances))
			{
				VehicleSpawnpoint vehicleSpawnpoint = this.findRandomSpawn();
				if (vehicleSpawnpoint != null)
				{
					this.addVehicleAtSpawnAndReplicate(vehicleSpawnpoint);
				}
			}
		}

		// Token: 0x06002EAF RID: 11951 RVA: 0x000CC054 File Offset: 0x000CA254
		private void RespawnReloadedVehicles()
		{
			List<InteractableVehicle> list = null;
			foreach (InteractableVehicle interactableVehicle in VehicleManager._vehicles)
			{
				if (interactableVehicle.asset.engine != EEngine.TRAIN && interactableVehicle.asset.hasBeenReplaced)
				{
					if (list == null)
					{
						list = new List<InteractableVehicle>();
					}
					list.Add(interactableVehicle);
				}
			}
			if (list == null)
			{
				return;
			}
			foreach (InteractableVehicle interactableVehicle2 in list)
			{
				VehicleAsset vehicleAsset = Assets.find<VehicleAsset>(interactableVehicle2.asset.GUID);
				if (vehicleAsset == null)
				{
					UnturnedLog.error("Missing replacement asset for reloaded vehicle");
				}
				else
				{
					VehicleManager.askVehicleDestroy(interactableVehicle2);
					VehicleManager.SpawnVehicleV3(vehicleAsset, interactableVehicle2.skinID, interactableVehicle2.mythicID, interactableVehicle2.roadPosition, interactableVehicle2.transform.position, interactableVehicle2.transform.rotation, interactableVehicle2.sirensOn, interactableVehicle2.isBlimpFloating, interactableVehicle2.headlightsOn, interactableVehicle2.taillightsOn, interactableVehicle2.fuel, interactableVehicle2.health, interactableVehicle2.batteryCharge, interactableVehicle2.lockedOwner, interactableVehicle2.lockedGroup, interactableVehicle2.isLocked, null, interactableVehicle2.tireAliveMask, interactableVehicle2.PaintColor);
				}
			}
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x000CC1B0 File Offset: 0x000CA3B0
		private void onLevelLoaded(int level)
		{
			if (level > Level.BUILD_INDEX_SETUP)
			{
				VehicleManager.seq = 0U;
				VehicleManager._vehicles = new List<InteractableVehicle>();
				VehicleManager.shouldRespawnReloadedVehicles = false;
				VehicleManager.highestInstanceID = 0U;
				VehicleManager.respawnVehicleIndex = 0;
				BarricadeManager.clearPlants();
				if (Provider.isServer)
				{
					this.enableDecayUpdate = (Provider.modeConfigData.Vehicles.Decay_Time > 0f);
					if (!this.enableDecayUpdate)
					{
						UnturnedLog.info("Disabling vehicle decay because Decay_Time is negative");
					}
					if (Level.info != null && Level.info.type != ELevelType.ARENA)
					{
						VehicleManager.load();
						if (LevelVehicles.spawns.Count > 0)
						{
							List<VehicleSpawnpoint> list = new List<VehicleSpawnpoint>();
							for (int i = 0; i < LevelVehicles.spawns.Count; i++)
							{
								list.Add(LevelVehicles.spawns[i]);
							}
							while ((long)VehicleManager.vehicles.Count < (long)((ulong)VehicleManager.maxInstances) && list.Count > 0)
							{
								int num = Random.Range(0, list.Count);
								VehicleSpawnpoint spawn = list[num];
								list.RemoveAt(num);
								if (this.canUseSpawnpoint(spawn))
								{
									this.addVehicleAtSpawn(spawn);
								}
							}
						}
						using (List<LevelTrainAssociation>.Enumerator enumerator = Level.info.configData.Trains.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								LevelTrainAssociation levelTrainAssociation = enumerator.Current;
								bool flag = false;
								using (List<InteractableVehicle>.Enumerator enumerator2 = VehicleManager.vehicles.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										if (enumerator2.Current.id == levelTrainAssociation.VehicleID)
										{
											flag = true;
											break;
										}
									}
								}
								if (!flag)
								{
									Road road = LevelRoads.getRoad((int)levelTrainAssociation.RoadIndex);
									if (road == null)
									{
										UnturnedLog.error(string.Concat(new string[]
										{
											"Failed to find track ",
											levelTrainAssociation.RoadIndex.ToString(),
											" for train ",
											levelTrainAssociation.VehicleID.ToString(),
											"!"
										}));
									}
									else
									{
										float trackSampledLength = road.trackSampledLength;
										float num2 = Random.Range(levelTrainAssociation.Min_Spawn_Placement, levelTrainAssociation.Max_Spawn_Placement);
										float roadPosition = trackSampledLength * num2;
										VehicleAsset vehicleAsset = Assets.find(EAssetType.VEHICLE, levelTrainAssociation.VehicleID) as VehicleAsset;
										if (vehicleAsset != null)
										{
											NetId netId = NetIdRegistry.ClaimBlock(21U);
											this.addVehicle(vehicleAsset.GUID, 0, 0, roadPosition, Vector3.zero, Quaternion.identity, false, false, false, false, ushort.MaxValue, false, ushort.MaxValue, ushort.MaxValue, CSteamID.Nil, CSteamID.Nil, false, null, null, VehicleManager.allocateInstanceID(), VehicleManager.getVehicleRandomTireAliveMask(vehicleAsset), netId, new Color32(0, 0, 0, 0));
										}
										else if (Assets.shouldLoadAnyAssets)
										{
											UnturnedLog.error("Failed to find asset for train " + levelTrainAssociation.VehicleID.ToString() + "!");
										}
									}
								}
							}
							goto IL_2BF;
						}
					}
					Level.isLoadingVehicles = false;
					IL_2BF:
					if (VehicleManager.vehicles != null)
					{
						for (int j = 0; j < VehicleManager.vehicles.Count; j++)
						{
							if (VehicleManager.vehicles[j] != null)
							{
								Rigidbody component = VehicleManager.vehicles[j].GetComponent<Rigidbody>();
								if (component != null)
								{
									component.constraints = RigidbodyConstraints.FreezeAll;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06002EB1 RID: 11953 RVA: 0x000CC50C File Offset: 0x000CA70C
		private void onPostLevelLoaded(int level)
		{
			if (level > Level.BUILD_INDEX_SETUP && Provider.isServer)
			{
				for (int i = 0; i < VehicleManager.vehicles.Count; i++)
				{
					if (VehicleManager.vehicles[i] != null)
					{
						Rigidbody component = VehicleManager.vehicles[i].GetComponent<Rigidbody>();
						if (component != null)
						{
							component.constraints = RigidbodyConstraints.None;
						}
					}
				}
			}
		}

		// Token: 0x06002EB2 RID: 11954 RVA: 0x000CC571 File Offset: 0x000CA771
		private void onServerDisconnected(CSteamID player)
		{
			if (Provider.isServer)
			{
				VehicleManager.forceRemovePlayer(player);
			}
		}

		// Token: 0x06002EB3 RID: 11955 RVA: 0x000CC584 File Offset: 0x000CA784
		private void sendVehicleStates()
		{
			VehicleManager.seq += 1U;
			for (int i = 0; i < Provider.clients.Count; i++)
			{
				SteamPlayer client = Provider.clients[i];
				if (client != null && !(client.player == null))
				{
					this.vehiclesToSend.Clear();
					for (int j = 0; j < VehicleManager.vehicles.Count; j++)
					{
						InteractableVehicle interactableVehicle = VehicleManager.vehicles[j];
						if (!(interactableVehicle == null) && ((interactableVehicle.updates != null && interactableVehicle.updates.Count > 0) || interactableVehicle.needsReplicationUpdate) && !interactableVehicle.checkDriver(client.playerID.steamID))
						{
							this.vehiclesToSend.Add(interactableVehicle);
						}
					}
					if (!this.vehiclesToSend.IsEmpty<InteractableVehicle>())
					{
						VehicleManager.SendVehicleStates.Invoke(ENetReliability.Unreliable, client.transportConnection, delegate(NetPakWriter writer)
						{
							Vector3 position = client.player.transform.position;
							SystemNetPakWriterEx.WriteUInt32(writer, VehicleManager.seq);
							SystemNetPakWriterEx.WriteUInt16(writer, (ushort)this.vehiclesToSend.Count);
							foreach (InteractableVehicle interactableVehicle3 in this.vehiclesToSend)
							{
								Vector3 position2 = interactableVehicle3.transform.position;
								bool flag = (position2 - position).sqrMagnitude < 90000f;
								Vector3 vector;
								if (interactableVehicle3.asset.engine == EEngine.TRAIN)
								{
									vector = InteractableVehicle.PackRoadPosition(interactableVehicle3.roadPosition);
								}
								else
								{
									vector = position2;
								}
								SystemNetPakWriterEx.WriteUInt32(writer, interactableVehicle3.instanceID);
								UnityNetPakWriterEx.WriteClampedVector3(writer, vector, 13, 8);
								UnityNetPakWriterEx.WriteQuaternion(writer, interactableVehicle3.transform.rotation, 11);
								SystemNetPakWriterEx.WriteUnsignedClampedFloat(writer, interactableVehicle3.ReplicatedSpeed, 8, 2);
								SystemNetPakWriterEx.WriteClampedFloat(writer, interactableVehicle3.ReplicatedForwardVelocity, 9, 2);
								SystemNetPakWriterEx.WriteSignedNormalizedFloat(writer, interactableVehicle3.ReplicatedSteeringInput, 2);
								SystemNetPakWriterEx.WriteClampedFloat(writer, interactableVehicle3.ReplicatedVelocityInput, 9, 2);
								writer.WriteBit(flag);
								if (flag)
								{
									if (interactableVehicle3.asset.replicatedWheelIndices != null)
									{
										foreach (int num in interactableVehicle3.asset.replicatedWheelIndices)
										{
											Wheel wheelAtIndex = interactableVehicle3.GetWheelAtIndex(num);
											if (wheelAtIndex == null)
											{
												UnturnedLog.error(string.Format("\"{0}\" missing wheel for replicated index: {1}", interactableVehicle3.asset.FriendlyName, num));
												SystemNetPakWriterEx.WriteUnsignedNormalizedFloat(writer, 0f, 4);
											}
											else
											{
												SystemNetPakWriterEx.WriteUnsignedNormalizedFloat(writer, wheelAtIndex.replicatedSuspensionState, 4);
												writer.WritePhysicsMaterialNetId(wheelAtIndex.replicatedGroundMaterial);
											}
										}
									}
									if (interactableVehicle3.asset.UsesEngineRpmAndGears)
									{
										uint num2 = (uint)(interactableVehicle3.GearNumber + 1);
										writer.WriteBits(num2, 3);
										float num3 = Mathf.InverseLerp(interactableVehicle3.asset.EngineIdleRpm, interactableVehicle3.asset.EngineMaxRpm, interactableVehicle3.ReplicatedEngineRpm);
										SystemNetPakWriterEx.WriteUnsignedNormalizedFloat(writer, num3, 7);
									}
								}
							}
							if (writer.errors != null && Time.realtimeSinceStartup - VehicleManager.lastSendOverflowWarning > 1f)
							{
								VehicleManager.lastSendOverflowWarning = Time.realtimeSinceStartup;
								CommandWindow.LogWarningFormat("Error {0} writing vehicle states. The vehicle count ({1}) is probably too high. No this is not a bug introduced in the update, rather a warning of a previously silent bug.", new object[]
								{
									writer.errors,
									VehicleManager._vehicles.Count
								});
							}
						});
					}
				}
			}
			for (int k = 0; k < VehicleManager.vehicles.Count; k++)
			{
				InteractableVehicle interactableVehicle2 = VehicleManager.vehicles[k];
				if (!(interactableVehicle2 == null))
				{
					if (interactableVehicle2.updates != null)
					{
						interactableVehicle2.updates.Clear();
					}
					interactableVehicle2.needsReplicationUpdate = false;
				}
			}
		}

		// Token: 0x06002EB4 RID: 11956 RVA: 0x000CC6F0 File Offset: 0x000CA8F0
		private void Update()
		{
			if (!Provider.isServer || !Level.isLoaded)
			{
				return;
			}
			if (VehicleManager.vehicles == null)
			{
				return;
			}
			if (VehicleManager.vehicles.Count > 0 && Time.realtimeSinceStartup - VehicleManager.lastTick > Provider.UPDATE_TIME)
			{
				VehicleManager.lastTick += Provider.UPDATE_TIME;
				if (Time.realtimeSinceStartup - VehicleManager.lastTick > Provider.UPDATE_TIME)
				{
					VehicleManager.lastTick = Time.realtimeSinceStartup;
				}
				this.sendVehicleStates();
			}
			this.despawnAndRespawnVehicles();
			if (this.enableDecayUpdate && VehicleManager._vehicles.Count > 0)
			{
				this.UpdateDecay();
			}
			if (VehicleManager.shouldRespawnReloadedVehicles)
			{
				VehicleManager.shouldRespawnReloadedVehicles = false;
				this.RespawnReloadedVehicles();
			}
		}

		// Token: 0x06002EB5 RID: 11957 RVA: 0x000CC79C File Offset: 0x000CA99C
		private void Start()
		{
			VehicleManager.manager = this;
			CommandLogMemoryUsage.OnExecuted = (Action<List<string>>)Delegate.Combine(CommandLogMemoryUsage.OnExecuted, new Action<List<string>>(this.OnLogMemoryUsage));
			Level.onPrePreLevelLoaded = (PrePreLevelLoaded)Delegate.Combine(Level.onPrePreLevelLoaded, new PrePreLevelLoaded(this.onLevelLoaded));
			Level.onPostLevelLoaded = (PostLevelLoaded)Delegate.Combine(Level.onPostLevelLoaded, new PostLevelLoaded(this.onPostLevelLoaded));
			Provider.onServerDisconnected = (Provider.ServerDisconnected)Delegate.Combine(Provider.onServerDisconnected, new Provider.ServerDisconnected(this.onServerDisconnected));
		}

		// Token: 0x06002EB6 RID: 11958 RVA: 0x000CC82F File Offset: 0x000CAA2F
		private void OnLogMemoryUsage(List<string> results)
		{
			results.Add(string.Format("Vehicles: {0}", VehicleManager.vehicles.Count));
		}

		// Token: 0x06002EB7 RID: 11959 RVA: 0x000CC850 File Offset: 0x000CAA50
		public static void load()
		{
			uint num = 0U;
			if (LevelSavedata.fileExists("/Vehicles.dat") && Level.info.type == ELevelType.SURVIVAL)
			{
				River river = LevelSavedata.openRiver("/Vehicles.dat", true);
				byte b = river.readByte();
				if (b > 2)
				{
					ushort num2 = river.readUInt16();
					for (ushort num3 = 0; num3 < num2; num3 += 1)
					{
						Asset asset;
						if (b < 14)
						{
							ushort id = river.readUInt16();
							asset = Assets.find(EAssetType.VEHICLE, id);
						}
						else
						{
							asset = Assets.find(river.readGUID());
						}
						Color32 paintColor = new Color32(0, 0, 0, 0);
						bool flag = false;
						VehicleRedirectorAsset vehicleRedirectorAsset = asset as VehicleRedirectorAsset;
						VehicleAsset vehicleAsset;
						if (vehicleRedirectorAsset != null)
						{
							vehicleAsset = vehicleRedirectorAsset.TargetVehicle.Find();
							if (vehicleRedirectorAsset.LoadPaintColor != null)
							{
								paintColor = vehicleRedirectorAsset.LoadPaintColor.Value;
								flag = true;
							}
						}
						else
						{
							vehicleAsset = (asset as VehicleAsset);
						}
						uint num4;
						if (b < 12)
						{
							num4 = VehicleManager.allocateInstanceID();
						}
						else
						{
							num4 = river.readUInt32();
							if (num4 > num)
							{
								num = num4;
							}
						}
						ushort skinID;
						if (b < 8)
						{
							skinID = 0;
						}
						else
						{
							skinID = river.readUInt16();
						}
						ushort mythicID;
						if (b < 9)
						{
							mythicID = 0;
						}
						else
						{
							mythicID = river.readUInt16();
						}
						float roadPosition;
						if (b < 10)
						{
							roadPosition = 0f;
						}
						else
						{
							roadPosition = river.readSingle();
						}
						Vector3 point = river.readSingleVector3();
						Quaternion angle = river.readSingleQuaternion();
						ushort fuel = river.readUInt16();
						ushort health = river.readUInt16();
						ushort batteryCharge = 10000;
						if (b > 5)
						{
							batteryCharge = river.readUInt16();
						}
						Guid batteryItemGuid;
						if (b >= 15)
						{
							batteryItemGuid = river.readGUID();
						}
						else
						{
							batteryItemGuid = Guid.Empty;
						}
						byte tireAliveMask = byte.MaxValue;
						if (b > 6)
						{
							tireAliveMask = river.readByte();
						}
						CSteamID owner = CSteamID.Nil;
						CSteamID group = CSteamID.Nil;
						bool locked = false;
						if (b > 4)
						{
							owner = river.readSteamID();
							group = river.readSteamID();
							locked = river.readBoolean();
						}
						byte[][] array = null;
						if (b > 3)
						{
							array = new byte[(int)river.readByte()][];
							byte b2 = 0;
							while ((int)b2 < array.Length)
							{
								array[(int)b2] = river.readBytes();
								b2 += 1;
							}
						}
						point.y += 0.02f;
						bool flag2 = b >= 11 && river.readBoolean();
						ItemJar[] array2 = null;
						if (flag2)
						{
							array2 = new ItemJar[(int)river.readByte()];
							byte b3 = 0;
							while ((int)b3 < array2.Length)
							{
								byte new_x = river.readByte();
								byte new_y = river.readByte();
								byte newRot = river.readByte();
								ushort num5 = river.readUInt16();
								byte newAmount = river.readByte();
								byte newQuality = river.readByte();
								byte[] newState = river.readBytes();
								if (Assets.find(EAssetType.ITEM, num5) is ItemAsset)
								{
									Item newItem = new Item(num5, newAmount, newQuality, newState);
									array2[(int)b3] = new ItemJar(new_x, new_y, newRot, newItem);
								}
								b3 += 1;
							}
						}
						float decayTimer;
						if (b >= 13)
						{
							decayTimer = river.readSingle();
						}
						else
						{
							decayTimer = 0f;
						}
						if (b >= 16)
						{
							Color32 color = new Color32(0, 0, 0, 0);
							color.r = river.readByte();
							color.g = river.readByte();
							color.b = river.readByte();
							color.a = river.readByte();
							if (!flag)
							{
								paintColor = color;
							}
						}
						if (vehicleAsset != null)
						{
							if (!vehicleAsset.canTiresBeDamaged)
							{
								tireAliveMask = byte.MaxValue;
							}
							NetId netId = NetIdRegistry.ClaimBlock(21U);
							InteractableVehicle interactableVehicle = VehicleManager.manager.addVehicle(vehicleAsset.GUID, skinID, mythicID, roadPosition, point, angle, false, false, false, false, fuel, false, health, batteryCharge, owner, group, locked, null, array, num4, tireAliveMask, netId, paintColor);
							if (interactableVehicle != null)
							{
								interactableVehicle.batteryItemGuid = batteryItemGuid;
								if (flag2 && array2 != null && array2.Length != 0 && interactableVehicle.trunkItems != null && interactableVehicle.trunkItems.height > 0)
								{
									byte b4 = 0;
									while ((int)b4 < array2.Length)
									{
										ItemJar itemJar = array2[(int)b4];
										if (itemJar != null)
										{
											interactableVehicle.trunkItems.loadItem(itemJar.x, itemJar.y, itemJar.rot, itemJar.item);
										}
										b4 += 1;
									}
								}
								interactableVehicle.decayTimer = decayTimer;
							}
						}
					}
				}
				else
				{
					ushort num6 = river.readUInt16();
					for (ushort num7 = 0; num7 < num6; num7 += 1)
					{
						ushort id2 = river.readUInt16();
						river.readColor();
						Vector3 point2 = river.readSingleVector3();
						Quaternion angle2 = river.readSingleQuaternion();
						ushort fuel2 = river.readUInt16();
						ushort health2 = ushort.MaxValue;
						ushort maxValue = ushort.MaxValue;
						byte maxValue2 = byte.MaxValue;
						id2 = (ushort)Random.Range(1, 51);
						if (b > 1)
						{
							health2 = river.readUInt16();
						}
						point2.y += 0.02f;
						Asset asset2 = Assets.find(EAssetType.VEHICLE, id2);
						Color32 value = new Color32(0, 0, 0, 0);
						VehicleRedirectorAsset vehicleRedirectorAsset2 = asset2 as VehicleRedirectorAsset;
						VehicleAsset vehicleAsset2;
						if (vehicleRedirectorAsset2 != null)
						{
							vehicleAsset2 = vehicleRedirectorAsset2.TargetVehicle.Find();
							if (vehicleRedirectorAsset2.LoadPaintColor != null)
							{
								value = vehicleRedirectorAsset2.LoadPaintColor.Value;
							}
						}
						else
						{
							vehicleAsset2 = (asset2 as VehicleAsset);
						}
						if (vehicleAsset2 != null)
						{
							NetId netId2 = NetIdRegistry.ClaimBlock(21U);
							VehicleManager.manager.addVehicle(vehicleAsset2.GUID, 0, 0, 0f, point2, angle2, false, false, false, false, fuel2, false, health2, maxValue, CSteamID.Nil, CSteamID.Nil, false, null, null, VehicleManager.allocateInstanceID(), maxValue2, netId2, value);
						}
					}
				}
				river.closeRiver();
			}
			if (num > VehicleManager.highestInstanceID)
			{
				VehicleManager.highestInstanceID = num;
			}
			Level.isLoadingVehicles = false;
		}

		// Token: 0x06002EB8 RID: 11960 RVA: 0x000CCD98 File Offset: 0x000CAF98
		public static void save()
		{
			River river = LevelSavedata.openRiver("/Vehicles.dat", false);
			river.writeByte(16);
			ushort num = 0;
			ushort num2 = 0;
			while ((int)num2 < VehicleManager.vehicles.Count)
			{
				InteractableVehicle interactableVehicle = VehicleManager.vehicles[(int)num2];
				if (!(interactableVehicle == null) && !(interactableVehicle.transform == null) && !interactableVehicle.isAutoClearable)
				{
					num += 1;
				}
				num2 += 1;
			}
			river.writeUInt16(num);
			ushort num3 = 0;
			while ((int)num3 < VehicleManager.vehicles.Count)
			{
				InteractableVehicle interactableVehicle2 = VehicleManager.vehicles[(int)num3];
				if (!(interactableVehicle2 == null) && !(interactableVehicle2.transform == null) && !interactableVehicle2.isAutoClearable)
				{
					Vector3 position = interactableVehicle2.transform.position;
					if (!position.IsFinite())
					{
						position = new Vector3(0f, Level.HEIGHT - 50f, 0f);
					}
					else if (position.y > Level.HEIGHT)
					{
						position.y = Level.HEIGHT - 50f;
					}
					river.writeGUID(interactableVehicle2.asset.GUID);
					river.writeUInt32(interactableVehicle2.instanceID);
					river.writeUInt16(interactableVehicle2.skinID);
					river.writeUInt16(interactableVehicle2.mythicID);
					river.writeSingle(interactableVehicle2.roadPosition);
					river.writeSingleVector3(position);
					river.writeSingleQuaternion(interactableVehicle2.transform.rotation);
					river.writeUInt16(interactableVehicle2.fuel);
					river.writeUInt16(interactableVehicle2.health);
					river.writeUInt16(interactableVehicle2.batteryCharge);
					river.writeGUID(interactableVehicle2.batteryItemGuid);
					river.writeByte(interactableVehicle2.tireAliveMask);
					river.writeSteamID(interactableVehicle2.lockedOwner);
					river.writeSteamID(interactableVehicle2.lockedGroup);
					river.writeBoolean(interactableVehicle2.isLocked);
					if (interactableVehicle2.turrets != null)
					{
						byte b = (byte)interactableVehicle2.turrets.Length;
						river.writeByte(b);
						for (byte b2 = 0; b2 < b; b2 += 1)
						{
							Passenger passenger = interactableVehicle2.turrets[(int)b2];
							if (passenger != null && passenger.state != null)
							{
								river.writeBytes(passenger.state);
							}
							else
							{
								river.writeBytes(new byte[0]);
							}
						}
					}
					else
					{
						river.writeByte(0);
					}
					if (interactableVehicle2.trunkItems != null && interactableVehicle2.trunkItems.height > 0)
					{
						river.writeBoolean(true);
						byte itemCount = interactableVehicle2.trunkItems.getItemCount();
						river.writeByte(itemCount);
						for (byte b3 = 0; b3 < itemCount; b3 += 1)
						{
							ItemJar item = interactableVehicle2.trunkItems.getItem(b3);
							river.writeByte((item != null) ? item.x : 0);
							river.writeByte((item != null) ? item.y : 0);
							river.writeByte((item != null) ? item.rot : 0);
							river.writeUInt16((item != null) ? item.item.id : 0);
							river.writeByte((item != null) ? item.item.amount : 0);
							river.writeByte((item != null) ? item.item.quality : 0);
							river.writeBytes((item != null) ? item.item.state : new byte[0]);
						}
					}
					else
					{
						river.writeBoolean(false);
					}
					river.writeSingle(interactableVehicle2.decayTimer);
					river.writeByte(interactableVehicle2.PaintColor.r);
					river.writeByte(interactableVehicle2.PaintColor.g);
					river.writeByte(interactableVehicle2.PaintColor.b);
					river.writeByte(interactableVehicle2.PaintColor.a);
				}
				num3 += 1;
			}
			river.closeRiver();
		}

		/// <summary>
		/// Called on server each frame to slowly damage abandoned vehicle.
		/// </summary>
		// Token: 0x06002EB9 RID: 11961 RVA: 0x000CD158 File Offset: 0x000CB358
		private void UpdateDecay()
		{
			this.decayUpdateIndex = (this.decayUpdateIndex + 1) % VehicleManager._vehicles.Count;
			InteractableVehicle interactableVehicle = VehicleManager._vehicles[this.decayUpdateIndex];
			if (interactableVehicle == null || interactableVehicle.asset == null || !interactableVehicle.asset.CanDecay)
			{
				return;
			}
			float num = Time.time - interactableVehicle.decayLastUpdateTime;
			interactableVehicle.decayLastUpdateTime = Time.time;
			if (interactableVehicle.isDriven && (interactableVehicle.transform.position - interactableVehicle.decayLastUpdatePosition).sqrMagnitude > 1f)
			{
				interactableVehicle.ResetDecayTimer();
				return;
			}
			interactableVehicle.decayTimer += num;
			if (interactableVehicle.decayTimer > Provider.modeConfigData.Vehicles.Decay_Time)
			{
				interactableVehicle.decayPendingDamage += Provider.modeConfigData.Vehicles.Decay_Damage_Per_Second * num;
				int num2 = Mathf.FloorToInt(interactableVehicle.decayPendingDamage);
				if (num2 > 0)
				{
					interactableVehicle.decayPendingDamage -= (float)num2;
					VehicleManager.damage(interactableVehicle, (float)num2, 1f, true, CSteamID.Nil, EDamageOrigin.VehicleDecay);
				}
			}
		}

		// Token: 0x06002EBA RID: 11962 RVA: 0x000CD271 File Offset: 0x000CB471
		[Obsolete]
		public void tellVehicleLock(CSteamID steamID, uint instanceID, CSteamID owner, CSteamID group, bool locked)
		{
			VehicleManager.ReceiveVehicleLockState(instanceID, owner, group, locked);
		}

		// Token: 0x06002EBB RID: 11963 RVA: 0x000CD27E File Offset: 0x000CB47E
		[Obsolete]
		public void tellVehicleSkin(CSteamID steamID, uint instanceID, ushort skinID, ushort mythicID)
		{
			VehicleManager.ReceiveVehicleSkin(instanceID, skinID, mythicID);
		}

		// Token: 0x06002EBC RID: 11964 RVA: 0x000CD289 File Offset: 0x000CB489
		[Obsolete]
		public void tellVehicleSirens(CSteamID steamID, uint instanceID, bool on)
		{
			VehicleManager.ReceiveVehicleSirens(instanceID, on);
		}

		// Token: 0x06002EBD RID: 11965 RVA: 0x000CD292 File Offset: 0x000CB492
		[Obsolete]
		public void tellVehicleBlimp(CSteamID steamID, uint instanceID, bool on)
		{
			VehicleManager.ReceiveVehicleBlimp(instanceID, on);
		}

		// Token: 0x06002EBE RID: 11966 RVA: 0x000CD29B File Offset: 0x000CB49B
		[Obsolete]
		public void tellVehicleHeadlights(CSteamID steamID, uint instanceID, bool on)
		{
			VehicleManager.ReceiveVehicleHeadlights(instanceID, on);
		}

		// Token: 0x06002EBF RID: 11967 RVA: 0x000CD2A4 File Offset: 0x000CB4A4
		[Obsolete]
		public void tellVehicleHorn(CSteamID steamID, uint instanceID)
		{
			VehicleManager.ReceiveVehicleHorn(instanceID);
		}

		// Token: 0x06002EC0 RID: 11968 RVA: 0x000CD2AC File Offset: 0x000CB4AC
		[Obsolete]
		public void tellVehicleFuel(CSteamID steamID, uint instanceID, ushort newFuel)
		{
			VehicleManager.ReceiveVehicleFuel(instanceID, newFuel);
		}

		// Token: 0x06002EC1 RID: 11969 RVA: 0x000CD2B5 File Offset: 0x000CB4B5
		[Obsolete]
		public void tellVehicleBatteryCharge(CSteamID steamID, uint instanceID, ushort newBatteryCharge)
		{
			VehicleManager.ReceiveVehicleBatteryCharge(instanceID, newBatteryCharge);
		}

		// Token: 0x06002EC2 RID: 11970 RVA: 0x000CD2BE File Offset: 0x000CB4BE
		[Obsolete]
		public void tellVehicleTireAliveMask(CSteamID steamID, uint instanceID, byte newTireAliveMask)
		{
			VehicleManager.ReceiveVehicleTireAliveMask(instanceID, newTireAliveMask);
		}

		// Token: 0x06002EC3 RID: 11971 RVA: 0x000CD2C7 File Offset: 0x000CB4C7
		[Obsolete]
		public void tellVehicleExploded(CSteamID steamID, uint instanceID)
		{
			VehicleManager.ReceiveVehicleExploded(instanceID);
		}

		// Token: 0x06002EC4 RID: 11972 RVA: 0x000CD2CF File Offset: 0x000CB4CF
		[Obsolete]
		public void tellVehicleHealth(CSteamID steamID, uint instanceID, ushort newHealth)
		{
			VehicleManager.ReceiveVehicleHealth(instanceID, newHealth);
		}

		// Token: 0x06002EC5 RID: 11973 RVA: 0x000CD2D8 File Offset: 0x000CB4D8
		[Obsolete]
		public void tellVehicleRecov(CSteamID steamID, uint instanceID, Vector3 newPosition, int newRecov)
		{
			VehicleManager.ReceiveVehicleRecov(instanceID, newPosition, newRecov);
		}

		// Token: 0x06002EC6 RID: 11974 RVA: 0x000CD2E3 File Offset: 0x000CB4E3
		[Obsolete]
		public void tellVehicleStates(CSteamID steamID)
		{
		}

		// Token: 0x06002EC7 RID: 11975 RVA: 0x000CD2E5 File Offset: 0x000CB4E5
		[Obsolete]
		public void tellVehicleDestroy(CSteamID steamID, uint instanceID)
		{
			VehicleManager.ReceiveDestroySingleVehicle(instanceID);
		}

		// Token: 0x06002EC8 RID: 11976 RVA: 0x000CD2ED File Offset: 0x000CB4ED
		[Obsolete]
		public void tellVehicleDestroyAll(CSteamID steamID)
		{
			VehicleManager.ReceiveDestroyAllVehicles();
		}

		// Token: 0x06002EC9 RID: 11977 RVA: 0x000CD2F4 File Offset: 0x000CB4F4
		[Obsolete]
		public void tellVehicle(CSteamID steamID)
		{
		}

		// Token: 0x06002ECA RID: 11978 RVA: 0x000CD2F6 File Offset: 0x000CB4F6
		[Obsolete]
		public void tellVehicles(CSteamID steamID)
		{
		}

		// Token: 0x06002ECB RID: 11979 RVA: 0x000CD2F8 File Offset: 0x000CB4F8
		[Obsolete]
		public void askVehicles(CSteamID steamID)
		{
		}

		// Token: 0x06002ECC RID: 11980 RVA: 0x000CD2FA File Offset: 0x000CB4FA
		[Obsolete]
		public void tellEnterVehicle(CSteamID steamID, uint instanceID, byte seat, CSteamID player)
		{
			VehicleManager.ReceiveEnterVehicle(instanceID, seat, player);
		}

		// Token: 0x06002ECD RID: 11981 RVA: 0x000CD305 File Offset: 0x000CB505
		[Obsolete]
		public void tellExitVehicle(CSteamID steamID, uint instanceID, byte seat, Vector3 point, byte angle, bool forceUpdate)
		{
			VehicleManager.ReceiveExitVehicle(instanceID, seat, point, angle, forceUpdate);
		}

		// Token: 0x06002ECE RID: 11982 RVA: 0x000CD314 File Offset: 0x000CB514
		[Obsolete]
		public void tellSwapVehicle(CSteamID steamID, uint instanceID, byte fromSeat, byte toSeat)
		{
			VehicleManager.ReceiveSwapVehicleSeats(instanceID, fromSeat, toSeat);
		}

		// Token: 0x06002ECF RID: 11983 RVA: 0x000CD320 File Offset: 0x000CB520
		[Obsolete]
		public void askVehicleLock(CSteamID steamID)
		{
			ServerInvocationContext serverInvocationContext = ServerInvocationContext.FromSteamIDForBackwardsCompatibility(steamID);
			VehicleManager.ReceiveVehicleLockRequest(serverInvocationContext);
		}

		// Token: 0x06002ED0 RID: 11984 RVA: 0x000CD33C File Offset: 0x000CB53C
		[Obsolete]
		public void askVehicleSkin(CSteamID steamID)
		{
			ServerInvocationContext serverInvocationContext = ServerInvocationContext.FromSteamIDForBackwardsCompatibility(steamID);
			VehicleManager.ReceiveVehicleSkinRequest(serverInvocationContext);
		}

		// Token: 0x06002ED1 RID: 11985 RVA: 0x000CD358 File Offset: 0x000CB558
		[Obsolete]
		public void askVehicleHeadlights(CSteamID steamID, bool wantsHeadlightsOn)
		{
			ServerInvocationContext serverInvocationContext = ServerInvocationContext.FromSteamIDForBackwardsCompatibility(steamID);
			VehicleManager.ReceiveToggleVehicleHeadlights(serverInvocationContext, wantsHeadlightsOn);
		}

		// Token: 0x06002ED2 RID: 11986 RVA: 0x000CD374 File Offset: 0x000CB574
		[Obsolete]
		public void askVehicleBonus(CSteamID steamID, byte bonusType)
		{
			ServerInvocationContext serverInvocationContext = ServerInvocationContext.FromSteamIDForBackwardsCompatibility(steamID);
			VehicleManager.ReceiveUseVehicleBonus(serverInvocationContext, bonusType);
		}

		// Token: 0x06002ED3 RID: 11987 RVA: 0x000CD390 File Offset: 0x000CB590
		[Obsolete]
		public void askVehicleStealBattery(CSteamID steamID)
		{
			ServerInvocationContext serverInvocationContext = ServerInvocationContext.FromSteamIDForBackwardsCompatibility(steamID);
			VehicleManager.ReceiveStealVehicleBattery(serverInvocationContext);
		}

		// Token: 0x06002ED4 RID: 11988 RVA: 0x000CD3AC File Offset: 0x000CB5AC
		[Obsolete]
		public void askVehicleHorn(CSteamID steamID)
		{
			ServerInvocationContext serverInvocationContext = ServerInvocationContext.FromSteamIDForBackwardsCompatibility(steamID);
			VehicleManager.ReceiveVehicleHornRequest(serverInvocationContext);
		}

		// Token: 0x06002ED5 RID: 11989 RVA: 0x000CD3C8 File Offset: 0x000CB5C8
		[Obsolete]
		public void askEnterVehicle(CSteamID steamID, uint instanceID, byte[] hash, byte engine)
		{
			ServerInvocationContext serverInvocationContext = ServerInvocationContext.FromSteamIDForBackwardsCompatibility(steamID);
			VehicleManager.ReceiveEnterVehicleRequest(serverInvocationContext, instanceID, hash, new byte[0], engine);
		}

		// Token: 0x06002ED6 RID: 11990 RVA: 0x000CD3F0 File Offset: 0x000CB5F0
		[Obsolete]
		public void askExitVehicle(CSteamID steamID, Vector3 velocity)
		{
			ServerInvocationContext serverInvocationContext = ServerInvocationContext.FromSteamIDForBackwardsCompatibility(steamID);
			VehicleManager.ReceiveExitVehicleRequest(serverInvocationContext, velocity);
		}

		// Token: 0x06002ED7 RID: 11991 RVA: 0x000CD40C File Offset: 0x000CB60C
		[Obsolete]
		public void askSwapVehicle(CSteamID steamID, byte toSeat)
		{
			ServerInvocationContext serverInvocationContext = ServerInvocationContext.FromSteamIDForBackwardsCompatibility(steamID);
			VehicleManager.ReceiveSwapVehicleRequest(serverInvocationContext, toSeat);
		}

		// Token: 0x06002ED8 RID: 11992 RVA: 0x000CD428 File Offset: 0x000CB628
		[Obsolete]
		public void sendVehicle(InteractableVehicle vehicle)
		{
		}

		// Token: 0x06002ED9 RID: 11993 RVA: 0x000CD42A File Offset: 0x000CB62A
		[Obsolete("spawnVehicleV2 returns the spawned instance")]
		public static void spawnVehicle(ushort id, Vector3 point, Quaternion angle)
		{
			VehicleManager.spawnVehicleV2(id, point, angle);
		}

		// Token: 0x06002EDA RID: 11994 RVA: 0x000CD435 File Offset: 0x000CB635
		[Obsolete("spawnLockedVehicleForPlayerV2 returns the spawned instance")]
		public static void spawnLockedVehicleForPlayer(ushort id, Vector3 point, Quaternion angle, Player player)
		{
			VehicleManager.spawnLockedVehicleForPlayerV2(id, point, angle, player);
		}

		// Token: 0x040018EA RID: 6378
		public const byte SAVEDATA_VERSION_ADDED_DECAY = 13;

		// Token: 0x040018EB RID: 6379
		public const byte SAVEDATA_VERSION_REPLACED_ID_WITH_GUID = 14;

		// Token: 0x040018EC RID: 6380
		public const byte SAVEDATA_VERSION_BATTERY_GUID = 15;

		// Token: 0x040018ED RID: 6381
		public const byte SAVEDATA_VERSION_ADDED_PAINT_COLOR = 16;

		// Token: 0x040018EE RID: 6382
		private const byte SAVEDATA_VERSION_NEWEST = 16;

		// Token: 0x040018EF RID: 6383
		public static readonly byte SAVEDATA_VERSION = 16;

		// Token: 0x040018F0 RID: 6384
		public static VehicleLockpickedSignature onVehicleLockpicked;

		// Token: 0x040018F1 RID: 6385
		public static DamageVehicleRequestHandler onDamageVehicleRequested;

		// Token: 0x040018F2 RID: 6386
		public static RepairVehicleRequestHandler onRepairVehicleRequested;

		// Token: 0x040018F3 RID: 6387
		public static DamageTireRequestHandler onDamageTireRequested;

		// Token: 0x040018F4 RID: 6388
		public static VehicleCarjackedSignature onVehicleCarjacked;

		// Token: 0x040018F5 RID: 6389
		public static SiphonVehicleRequestHandler onSiphonVehicleRequested;

		// Token: 0x040018FA RID: 6394
		private static VehicleManager manager;

		// Token: 0x040018FB RID: 6395
		private static List<InteractableVehicle> _vehicles;

		/// <summary>
		/// If true, a vehicle asset has been replaced.
		/// </summary>
		// Token: 0x040018FC RID: 6396
		internal static bool shouldRespawnReloadedVehicles;

		// Token: 0x040018FD RID: 6397
		private static uint highestInstanceID;

		// Token: 0x040018FE RID: 6398
		private static ushort respawnVehicleIndex;

		// Token: 0x040018FF RID: 6399
		private static float lastTick;

		// Token: 0x04001900 RID: 6400
		private static readonly ClientStaticMethod<uint, CSteamID, CSteamID, bool> SendVehicleLockState = ClientStaticMethod<uint, CSteamID, CSteamID, bool>.Get(new ClientStaticMethod<uint, CSteamID, CSteamID, bool>.ReceiveDelegate(VehicleManager.ReceiveVehicleLockState));

		// Token: 0x04001901 RID: 6401
		private static readonly ClientStaticMethod<uint, ushort, ushort> SendVehicleSkin = ClientStaticMethod<uint, ushort, ushort>.Get(new ClientStaticMethod<uint, ushort, ushort>.ReceiveDelegate(VehicleManager.ReceiveVehicleSkin));

		// Token: 0x04001902 RID: 6402
		private static readonly ClientStaticMethod<uint, bool> SendVehicleSirens = ClientStaticMethod<uint, bool>.Get(new ClientStaticMethod<uint, bool>.ReceiveDelegate(VehicleManager.ReceiveVehicleSirens));

		// Token: 0x04001903 RID: 6403
		private static readonly ClientStaticMethod<uint, bool> SendVehicleBlimp = ClientStaticMethod<uint, bool>.Get(new ClientStaticMethod<uint, bool>.ReceiveDelegate(VehicleManager.ReceiveVehicleBlimp));

		// Token: 0x04001904 RID: 6404
		private static readonly ClientStaticMethod<uint, bool> SendVehicleHeadlights = ClientStaticMethod<uint, bool>.Get(new ClientStaticMethod<uint, bool>.ReceiveDelegate(VehicleManager.ReceiveVehicleHeadlights));

		// Token: 0x04001905 RID: 6405
		private static readonly ClientStaticMethod<uint> SendVehicleHorn = ClientStaticMethod<uint>.Get(new ClientStaticMethod<uint>.ReceiveDelegate(VehicleManager.ReceiveVehicleHorn));

		// Token: 0x04001906 RID: 6406
		private static readonly ClientStaticMethod<uint, ushort> SendVehicleFuel = ClientStaticMethod<uint, ushort>.Get(new ClientStaticMethod<uint, ushort>.ReceiveDelegate(VehicleManager.ReceiveVehicleFuel));

		// Token: 0x04001907 RID: 6407
		private static readonly ClientStaticMethod<uint, ushort> SendVehicleBatteryCharge = ClientStaticMethod<uint, ushort>.Get(new ClientStaticMethod<uint, ushort>.ReceiveDelegate(VehicleManager.ReceiveVehicleBatteryCharge));

		// Token: 0x04001908 RID: 6408
		private static readonly ClientStaticMethod<uint, byte> SendVehicleTireAliveMask = ClientStaticMethod<uint, byte>.Get(new ClientStaticMethod<uint, byte>.ReceiveDelegate(VehicleManager.ReceiveVehicleTireAliveMask));

		// Token: 0x04001909 RID: 6409
		private static readonly ClientStaticMethod<uint> SendVehicleExploded = ClientStaticMethod<uint>.Get(new ClientStaticMethod<uint>.ReceiveDelegate(VehicleManager.ReceiveVehicleExploded));

		// Token: 0x0400190A RID: 6410
		private static readonly ClientStaticMethod<uint, ushort> SendVehicleHealth = ClientStaticMethod<uint, ushort>.Get(new ClientStaticMethod<uint, ushort>.ReceiveDelegate(VehicleManager.ReceiveVehicleHealth));

		// Token: 0x0400190B RID: 6411
		private static readonly ClientStaticMethod<uint, Vector3, int> SendVehicleRecov = ClientStaticMethod<uint, Vector3, int>.Get(new ClientStaticMethod<uint, Vector3, int>.ReceiveDelegate(VehicleManager.ReceiveVehicleRecov));

		// Token: 0x0400190C RID: 6412
		private static uint seq;

		// Token: 0x0400190D RID: 6413
		private static readonly ClientStaticMethod SendVehicleStates = ClientStaticMethod.Get(new ClientStaticMethod.ReceiveDelegateWithContext(VehicleManager.ReceiveVehicleStates));

		// Token: 0x0400190E RID: 6414
		private static readonly ClientStaticMethod<uint> SendDestroySingleVehicle = ClientStaticMethod<uint>.Get(new ClientStaticMethod<uint>.ReceiveDelegate(VehicleManager.ReceiveDestroySingleVehicle));

		// Token: 0x0400190F RID: 6415
		private static readonly ClientStaticMethod SendDestroyAllVehicles = ClientStaticMethod.Get(new ClientStaticMethod.ReceiveDelegate(VehicleManager.ReceiveDestroyAllVehicles));

		// Token: 0x04001910 RID: 6416
		private static readonly ClientStaticMethod SendSingleVehicle = ClientStaticMethod.Get(new ClientStaticMethod.ReceiveDelegateWithContext(VehicleManager.ReceiveSingleVehicle));

		// Token: 0x04001911 RID: 6417
		private static readonly ClientStaticMethod SendMultipleVehicles = ClientStaticMethod.Get(new ClientStaticMethod.ReceiveDelegateWithContext(VehicleManager.ReceiveMultipleVehicles));

		// Token: 0x04001912 RID: 6418
		private static readonly ClientStaticMethod<uint, byte, CSteamID> SendEnterVehicle = ClientStaticMethod<uint, byte, CSteamID>.Get(new ClientStaticMethod<uint, byte, CSteamID>.ReceiveDelegate(VehicleManager.ReceiveEnterVehicle));

		// Token: 0x04001913 RID: 6419
		private static readonly ClientStaticMethod<uint, byte, Vector3, byte, bool> SendExitVehicle = ClientStaticMethod<uint, byte, Vector3, byte, bool>.Get(new ClientStaticMethod<uint, byte, Vector3, byte, bool>.ReceiveDelegate(VehicleManager.ReceiveExitVehicle));

		// Token: 0x04001914 RID: 6420
		private static readonly ClientStaticMethod<uint, byte, byte> SendSwapVehicleSeats = ClientStaticMethod<uint, byte, byte>.Get(new ClientStaticMethod<uint, byte, byte>.ReceiveDelegate(VehicleManager.ReceiveSwapVehicleSeats));

		// Token: 0x04001917 RID: 6423
		private static readonly ServerStaticMethod SendVehicleLockRequest = ServerStaticMethod.Get(new ServerStaticMethod.ReceiveDelegateWithContext(VehicleManager.ReceiveVehicleLockRequest));

		// Token: 0x04001918 RID: 6424
		private static readonly ServerStaticMethod SendVehicleSkinRequest = ServerStaticMethod.Get(new ServerStaticMethod.ReceiveDelegateWithContext(VehicleManager.ReceiveVehicleSkinRequest));

		// Token: 0x04001919 RID: 6425
		private static readonly ServerStaticMethod<bool> SendToggleVehicleHeadlights = ServerStaticMethod<bool>.Get(new ServerStaticMethod<bool>.ReceiveDelegateWithContext(VehicleManager.ReceiveToggleVehicleHeadlights));

		// Token: 0x0400191A RID: 6426
		private static readonly ServerStaticMethod<byte> SendUseVehicleBonus = ServerStaticMethod<byte>.Get(new ServerStaticMethod<byte>.ReceiveDelegateWithContext(VehicleManager.ReceiveUseVehicleBonus));

		// Token: 0x0400191B RID: 6427
		private static readonly ServerStaticMethod SendStealVehicleBattery = ServerStaticMethod.Get(new ServerStaticMethod.ReceiveDelegateWithContext(VehicleManager.ReceiveStealVehicleBattery));

		// Token: 0x0400191C RID: 6428
		private static readonly ServerStaticMethod SendVehicleHornRequest = ServerStaticMethod.Get(new ServerStaticMethod.ReceiveDelegateWithContext(VehicleManager.ReceiveVehicleHornRequest));

		// Token: 0x0400191D RID: 6429
		private static readonly ServerStaticMethod<uint, byte[], byte[], byte> SendEnterVehicleRequest = ServerStaticMethod<uint, byte[], byte[], byte>.Get(new ServerStaticMethod<uint, byte[], byte[], byte>.ReceiveDelegateWithContext(VehicleManager.ReceiveEnterVehicleRequest));

		// Token: 0x0400191E RID: 6430
		private static readonly ServerStaticMethod<Vector3> SendExitVehicleRequest = ServerStaticMethod<Vector3>.Get(new ServerStaticMethod<Vector3>.ReceiveDelegateWithContext(VehicleManager.ReceiveExitVehicleRequest));

		// Token: 0x0400191F RID: 6431
		private static readonly ServerStaticMethod<byte> SendSwapVehicleRequest = ServerStaticMethod<byte>.Get(new ServerStaticMethod<byte>.ReceiveDelegateWithContext(VehicleManager.ReceiveSwapVehicleRequest));

		// Token: 0x04001920 RID: 6432
		public static Action<InteractableVehicle> OnVehicleExploded;

		// Token: 0x04001921 RID: 6433
		private List<InteractableVehicle> vehiclesToSend = new List<InteractableVehicle>();

		// Token: 0x04001922 RID: 6434
		private static float lastSendOverflowWarning;

		// Token: 0x04001923 RID: 6435
		private bool enableDecayUpdate;

		// Token: 0x04001924 RID: 6436
		private int decayUpdateIndex;

		/// <summary>
		/// +0 = InteractableVehicle
		/// +1 = root transform
		/// +X = VehicleBarricadeRegion
		/// Asset does not know number of train cars, so we always reserve slack.
		/// </summary>
		// Token: 0x04001925 RID: 6437
		internal const int NETIDS_PER_VEHICLE = 21;

		// Token: 0x04001926 RID: 6438
		internal const int POSITION_FRAC_BIT_COUNT = 8;

		// Token: 0x04001927 RID: 6439
		internal const int ROTATION_BIT_COUNT = 11;

		/// <summary>
		/// Speed is unsigned, so 8 bits allows a range of [0, 256).
		/// </summary>
		// Token: 0x04001928 RID: 6440
		internal const int SPEED_INT_BIT_COUNT = 8;

		// Token: 0x04001929 RID: 6441
		internal const int SPEED_FRAC_BIT_COUNT = 2;

		/// <summary>
		/// Velocity is signed, so 9 bits allows a range of [-256, 256).
		/// </summary>
		// Token: 0x0400192A RID: 6442
		internal const int FORWARD_VELOCITY_INT_BIT_COUNT = 9;

		// Token: 0x0400192B RID: 6443
		internal const int FORWARD_VELOCITY_FRAC_BIT_COUNT = 2;

		// Token: 0x0400192C RID: 6444
		internal const int STEERING_BIT_COUNT = 2;

		// Token: 0x0400192D RID: 6445
		internal const int GEAR_BIT_COUNT = 3;

		// Token: 0x0400192E RID: 6446
		internal const int ENGINE_RPM_BIT_COUNT = 7;

		// Token: 0x02000986 RID: 2438
		// (Invoke) Token: 0x06004B8C RID: 19340
		public delegate void EnterVehicleRequestHandler(Player player, InteractableVehicle vehicle, ref bool shouldAllow);

		// Token: 0x02000987 RID: 2439
		// (Invoke) Token: 0x06004B90 RID: 19344
		public delegate void ExitVehicleRequestHandler(Player player, InteractableVehicle vehicle, ref bool shouldAllow, ref Vector3 pendingLocation, ref float pendingYaw);

		// Token: 0x02000988 RID: 2440
		// (Invoke) Token: 0x06004B94 RID: 19348
		public delegate void SwapSeatRequestHandler(Player player, InteractableVehicle vehicle, ref bool shouldAllow, byte fromSeatIndex, ref byte toSeatIndex);

		// Token: 0x02000989 RID: 2441
		// (Invoke) Token: 0x06004B98 RID: 19352
		public delegate void ToggleVehicleLockRequested(InteractableVehicle vehicle, ref bool shouldAllow);
	}
}
