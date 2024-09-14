using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004F5 RID: 1269
	public class LevelVehicles
	{
		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x060027CA RID: 10186 RVA: 0x000A7A28 File Offset: 0x000A5C28
		[Obsolete("Was the parent of all vehicles in the past, but now empty for TransformHierarchy performance.")]
		public static Transform models
		{
			get
			{
				if (LevelVehicles._models == null)
				{
					LevelVehicles._models = new GameObject().transform;
					LevelVehicles._models.name = "Vehicles";
					LevelVehicles._models.parent = Level.spawns;
					LevelVehicles._models.tag = "Logic";
					LevelVehicles._models.gameObject.layer = 8;
					CommandWindow.LogWarningFormat("Plugin referencing LevelVehicles.models which has been deprecated.", Array.Empty<object>());
				}
				return LevelVehicles._models;
			}
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x060027CB RID: 10187 RVA: 0x000A7AA2 File Offset: 0x000A5CA2
		public static List<VehicleTable> tables
		{
			get
			{
				return LevelVehicles._tables;
			}
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x060027CC RID: 10188 RVA: 0x000A7AA9 File Offset: 0x000A5CA9
		public static List<VehicleSpawnpoint> spawns
		{
			get
			{
				return LevelVehicles._spawns;
			}
		}

		// Token: 0x060027CD RID: 10189 RVA: 0x000A7AB0 File Offset: 0x000A5CB0
		public static void setEnabled(bool isEnabled)
		{
			if (LevelVehicles.spawns == null)
			{
				return;
			}
			for (int i = 0; i < LevelVehicles.spawns.Count; i++)
			{
				LevelVehicles.spawns[i].setEnabled(isEnabled);
			}
		}

		// Token: 0x060027CE RID: 10190 RVA: 0x000A7AEB File Offset: 0x000A5CEB
		public static void addTable(string name)
		{
			if (LevelVehicles.tables.Count == 255)
			{
				return;
			}
			LevelVehicles.tables.Add(new VehicleTable(name));
		}

		// Token: 0x060027CF RID: 10191 RVA: 0x000A7B10 File Offset: 0x000A5D10
		public static void removeTable()
		{
			LevelVehicles.tables.RemoveAt((int)EditorSpawns.selectedVehicle);
			List<VehicleSpawnpoint> list = new List<VehicleSpawnpoint>();
			for (int i = 0; i < LevelVehicles.spawns.Count; i++)
			{
				VehicleSpawnpoint vehicleSpawnpoint = LevelVehicles.spawns[i];
				if (vehicleSpawnpoint.type == EditorSpawns.selectedVehicle)
				{
					Object.Destroy(vehicleSpawnpoint.node.gameObject);
				}
				else
				{
					if (vehicleSpawnpoint.type > EditorSpawns.selectedVehicle)
					{
						VehicleSpawnpoint vehicleSpawnpoint2 = vehicleSpawnpoint;
						vehicleSpawnpoint2.type -= 1;
					}
					list.Add(vehicleSpawnpoint);
				}
			}
			LevelVehicles._spawns = list;
			EditorSpawns.selectedVehicle = 0;
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count)
			{
				EditorSpawns.vehicleSpawn.GetComponent<Renderer>().material.color = LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].color;
			}
		}

		// Token: 0x060027D0 RID: 10192 RVA: 0x000A7BDA File Offset: 0x000A5DDA
		public static void addSpawn(Vector3 point, float angle)
		{
			if ((int)EditorSpawns.selectedVehicle >= LevelVehicles.tables.Count)
			{
				return;
			}
			LevelVehicles.spawns.Add(new VehicleSpawnpoint(EditorSpawns.selectedVehicle, point, angle));
		}

		// Token: 0x060027D1 RID: 10193 RVA: 0x000A7C04 File Offset: 0x000A5E04
		public static void removeSpawn(Vector3 point, float radius)
		{
			radius *= radius;
			List<VehicleSpawnpoint> list = new List<VehicleSpawnpoint>();
			for (int i = 0; i < LevelVehicles.spawns.Count; i++)
			{
				VehicleSpawnpoint vehicleSpawnpoint = LevelVehicles.spawns[i];
				if ((vehicleSpawnpoint.point - point).sqrMagnitude < radius)
				{
					Object.Destroy(vehicleSpawnpoint.node.gameObject);
				}
				else
				{
					list.Add(vehicleSpawnpoint);
				}
			}
			LevelVehicles._spawns = list;
		}

		/// <summary>
		/// Returned asset is not necessarily a vehicle asset yet: It can also be a VehicleRedirectorAsset which the
		/// vehicle spawner requires to properly set paint color.
		/// </summary>
		// Token: 0x060027D2 RID: 10194 RVA: 0x000A7C73 File Offset: 0x000A5E73
		public static Asset GetRandomAssetForSpawnpoint(VehicleSpawnpoint spawnpoint)
		{
			return LevelVehicles.tables[(int)spawnpoint.type].GetRandomAsset();
		}

		// Token: 0x060027D3 RID: 10195 RVA: 0x000A7C8C File Offset: 0x000A5E8C
		public static void load()
		{
			if (Level.isEditor || Provider.isServer)
			{
				LevelVehicles._tables = new List<VehicleTable>();
				LevelVehicles._spawns = new List<VehicleSpawnpoint>();
				if (ReadWrite.fileExists(Level.info.path + "/Spawns/Vehicles.dat", false, false))
				{
					River river = new River(Level.info.path + "/Spawns/Vehicles.dat", false);
					byte b = river.readByte();
					if (b > 1 && b < 3)
					{
						river.readSteamID();
					}
					byte b2 = river.readByte();
					for (byte b3 = 0; b3 < b2; b3 += 1)
					{
						Color newColor = river.readColor();
						string text = river.readString();
						ushort num;
						if (b > 3)
						{
							num = river.readUInt16();
						}
						else
						{
							num = 0;
						}
						List<VehicleTier> list = new List<VehicleTier>();
						byte b4 = river.readByte();
						for (byte b5 = 0; b5 < b4; b5 += 1)
						{
							string newName = river.readString();
							float newChance = river.readSingle();
							List<VehicleSpawn> list2 = new List<VehicleSpawn>();
							byte b6 = river.readByte();
							for (byte b7 = 0; b7 < b6; b7 += 1)
							{
								ushort newVehicle = river.readUInt16();
								list2.Add(new VehicleSpawn(newVehicle));
							}
							list.Add(new VehicleTier(list2, newName, newChance));
						}
						VehicleTable vehicleTable = new VehicleTable(list, newColor, text, num);
						LevelVehicles.tables.Add(vehicleTable);
						if (!Level.isEditor)
						{
							vehicleTable.buildTable();
						}
						if (vehicleTable.tableID != 0 && SpawnTableTool.ResolveLegacyId(num, EAssetType.VEHICLE, new Func<string>(vehicleTable.OnGetSpawnTableValidationErrorContext)) == 0 && Assets.shouldLoadAnyAssets)
						{
							Assets.reportError(string.Concat(new string[]
							{
								Level.info.name,
								" vehicle table \"",
								text,
								"\" references invalid spawn table ",
								num.ToString(),
								"!"
							}));
						}
					}
					ushort num2 = river.readUInt16();
					for (int i = 0; i < (int)num2; i++)
					{
						byte newType = river.readByte();
						Vector3 newPoint = river.readSingleVector3();
						float newAngle = (float)(river.readByte() * 2);
						LevelVehicles.spawns.Add(new VehicleSpawnpoint(newType, newPoint, newAngle));
					}
					river.closeRiver();
				}
			}
		}

		// Token: 0x060027D4 RID: 10196 RVA: 0x000A7EB0 File Offset: 0x000A60B0
		public static void save()
		{
			River river = new River(Level.info.path + "/Spawns/Vehicles.dat", false);
			river.writeByte(LevelVehicles.SAVEDATA_VERSION);
			river.writeByte((byte)LevelVehicles.tables.Count);
			byte b = 0;
			while ((int)b < LevelVehicles.tables.Count)
			{
				VehicleTable vehicleTable = LevelVehicles.tables[(int)b];
				river.writeColor(vehicleTable.color);
				river.writeString(vehicleTable.name);
				river.writeUInt16(vehicleTable.tableID);
				river.writeByte((byte)vehicleTable.tiers.Count);
				byte b2 = 0;
				while ((int)b2 < vehicleTable.tiers.Count)
				{
					VehicleTier vehicleTier = vehicleTable.tiers[(int)b2];
					river.writeString(vehicleTier.name);
					river.writeSingle(vehicleTier.chance);
					river.writeByte((byte)vehicleTier.table.Count);
					byte b3 = 0;
					while ((int)b3 < vehicleTier.table.Count)
					{
						VehicleSpawn vehicleSpawn = vehicleTier.table[(int)b3];
						river.writeUInt16(vehicleSpawn.vehicle);
						b3 += 1;
					}
					b2 += 1;
				}
				b += 1;
			}
			river.writeUInt16((ushort)LevelVehicles.spawns.Count);
			for (int i = 0; i < LevelVehicles.spawns.Count; i++)
			{
				VehicleSpawnpoint vehicleSpawnpoint = LevelVehicles.spawns[i];
				river.writeByte(vehicleSpawnpoint.type);
				river.writeSingleVector3(vehicleSpawnpoint.point);
				river.writeByte(MeasurementTool.angleToByte(vehicleSpawnpoint.angle));
			}
			river.closeRiver();
		}

		// Token: 0x060027D5 RID: 10197 RVA: 0x000A804A File Offset: 0x000A624A
		[Obsolete("GetRandomAssetForSpawnpoint should be used instead because it properly supports guids in spawn assets.")]
		public static ushort getVehicle(VehicleSpawnpoint spawn)
		{
			return LevelVehicles.getVehicle(spawn.type);
		}

		// Token: 0x060027D6 RID: 10198 RVA: 0x000A8057 File Offset: 0x000A6257
		[Obsolete("GetRandomAssetForSpawnpoint should be used instead because it properly supports guids in spawn assets.")]
		public static ushort getVehicle(byte type)
		{
			return LevelVehicles.tables[(int)type].getVehicle();
		}

		// Token: 0x0400150E RID: 5390
		public static readonly byte SAVEDATA_VERSION = 4;

		// Token: 0x0400150F RID: 5391
		private static Transform _models;

		// Token: 0x04001510 RID: 5392
		private static List<VehicleTable> _tables;

		// Token: 0x04001511 RID: 5393
		private static List<VehicleSpawnpoint> _spawns;
	}
}
