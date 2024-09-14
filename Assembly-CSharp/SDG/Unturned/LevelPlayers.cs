using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004F2 RID: 1266
	public class LevelPlayers
	{
		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x060027AA RID: 10154 RVA: 0x000A6920 File Offset: 0x000A4B20
		[Obsolete("Was the parent of all players in the past, but now empty for TransformHierarchy performance.")]
		public static Transform models
		{
			get
			{
				if (LevelPlayers._models == null)
				{
					LevelPlayers._models = new GameObject().transform;
					LevelPlayers._models.name = "Players";
					LevelPlayers._models.parent = Level.spawns;
					LevelPlayers._models.tag = "Logic";
					LevelPlayers._models.gameObject.layer = 8;
					CommandWindow.LogWarningFormat("Plugin referencing LevelPlayers.models which has been deprecated.", Array.Empty<object>());
				}
				return LevelPlayers._models;
			}
		}

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x060027AB RID: 10155 RVA: 0x000A699A File Offset: 0x000A4B9A
		public static List<PlayerSpawnpoint> spawns
		{
			get
			{
				return LevelPlayers._spawns;
			}
		}

		// Token: 0x060027AC RID: 10156 RVA: 0x000A69A4 File Offset: 0x000A4BA4
		public static void setEnabled(bool isEnabled)
		{
			for (int i = 0; i < LevelPlayers.spawns.Count; i++)
			{
				LevelPlayers.spawns[i].setEnabled(isEnabled);
			}
		}

		// Token: 0x060027AD RID: 10157 RVA: 0x000A69D8 File Offset: 0x000A4BD8
		public static bool checkCanBuild(Vector3 point)
		{
			float num = 256f;
			if (Level.info != null && Level.info.configData != null)
			{
				float prevent_Building_Near_Spawnpoint_Radius = Level.info.configData.Prevent_Building_Near_Spawnpoint_Radius;
				num = prevent_Building_Near_Spawnpoint_Radius * prevent_Building_Near_Spawnpoint_Radius;
			}
			for (int i = 0; i < LevelPlayers.spawns.Count; i++)
			{
				if ((LevelPlayers.spawns[i].point - point).sqrMagnitude < num)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060027AE RID: 10158 RVA: 0x000A6A49 File Offset: 0x000A4C49
		public static void addSpawn(Vector3 point, float angle, bool isAlt)
		{
			LevelPlayers.spawns.Add(new PlayerSpawnpoint(point, angle, isAlt));
		}

		// Token: 0x060027AF RID: 10159 RVA: 0x000A6A60 File Offset: 0x000A4C60
		public static void removeSpawn(Vector3 point, float radius)
		{
			radius *= radius;
			List<PlayerSpawnpoint> list = new List<PlayerSpawnpoint>();
			for (int i = 0; i < LevelPlayers.spawns.Count; i++)
			{
				PlayerSpawnpoint playerSpawnpoint = LevelPlayers.spawns[i];
				if ((playerSpawnpoint.point - point).sqrMagnitude < radius)
				{
					Object.Destroy(playerSpawnpoint.node.gameObject);
				}
				else
				{
					list.Add(playerSpawnpoint);
				}
			}
			LevelPlayers._spawns = list;
		}

		// Token: 0x060027B0 RID: 10160 RVA: 0x000A6AD0 File Offset: 0x000A4CD0
		public static List<PlayerSpawnpoint> getRegSpawns()
		{
			List<PlayerSpawnpoint> list = new List<PlayerSpawnpoint>();
			for (int i = 0; i < LevelPlayers.spawns.Count; i++)
			{
				PlayerSpawnpoint playerSpawnpoint = LevelPlayers.spawns[i];
				if (!playerSpawnpoint.isAlt)
				{
					list.Add(playerSpawnpoint);
				}
			}
			return list;
		}

		// Token: 0x060027B1 RID: 10161 RVA: 0x000A6B14 File Offset: 0x000A4D14
		public static List<PlayerSpawnpoint> getAltSpawns()
		{
			List<PlayerSpawnpoint> list = new List<PlayerSpawnpoint>();
			for (int i = 0; i < LevelPlayers.spawns.Count; i++)
			{
				PlayerSpawnpoint playerSpawnpoint = LevelPlayers.spawns[i];
				if (playerSpawnpoint.isAlt)
				{
					list.Add(playerSpawnpoint);
				}
			}
			return list;
		}

		// Token: 0x060027B2 RID: 10162 RVA: 0x000A6B58 File Offset: 0x000A4D58
		public static PlayerSpawnpoint getSpawn(bool isAlt)
		{
			List<PlayerSpawnpoint> list = isAlt ? LevelPlayers.getAltSpawns() : LevelPlayers.getRegSpawns();
			if (list.Count == 0)
			{
				return new PlayerSpawnpoint(new Vector3(0f, 256f, 0f), 0f, isAlt);
			}
			return list[Random.Range(0, list.Count)];
		}

		// Token: 0x060027B3 RID: 10163 RVA: 0x000A6BB0 File Offset: 0x000A4DB0
		public static void load()
		{
			LevelPlayers._spawns = new List<PlayerSpawnpoint>();
			if (ReadWrite.fileExists(Level.info.path + "/Spawns/Players.dat", false, false))
			{
				River river = new River(Level.info.path + "/Spawns/Players.dat", false);
				byte b = river.readByte();
				if (b > 1 && b < 3)
				{
					river.readSteamID();
				}
				int num = 0;
				int num2 = 0;
				byte b2 = river.readByte();
				for (int i = 0; i < (int)b2; i++)
				{
					Vector3 point = river.readSingleVector3();
					float angle = (float)(river.readByte() * 2);
					bool flag = false;
					if (b > 3)
					{
						flag = river.readBoolean();
					}
					if (flag)
					{
						num2++;
					}
					else
					{
						num++;
					}
					LevelPlayers.addSpawn(point, angle, flag);
				}
				river.closeRiver();
			}
		}

		// Token: 0x060027B4 RID: 10164 RVA: 0x000A6C78 File Offset: 0x000A4E78
		public static void save()
		{
			River river = new River(Level.info.path + "/Spawns/Players.dat", false);
			river.writeByte(LevelPlayers.SAVEDATA_VERSION);
			river.writeByte((byte)LevelPlayers.spawns.Count);
			for (int i = 0; i < LevelPlayers.spawns.Count; i++)
			{
				PlayerSpawnpoint playerSpawnpoint = LevelPlayers.spawns[i];
				river.writeSingleVector3(playerSpawnpoint.point);
				river.writeByte(MeasurementTool.angleToByte(playerSpawnpoint.angle));
				river.writeBoolean(playerSpawnpoint.isAlt);
			}
			river.closeRiver();
		}

		// Token: 0x04001504 RID: 5380
		public static readonly byte SAVEDATA_VERSION = 4;

		// Token: 0x04001505 RID: 5381
		private static Transform _models;

		// Token: 0x04001506 RID: 5382
		private static List<PlayerSpawnpoint> _spawns;
	}
}
