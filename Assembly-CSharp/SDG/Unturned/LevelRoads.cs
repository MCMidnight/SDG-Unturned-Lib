using System;
using System.Collections.Generic;
using SDG.Framework.Landscapes;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004F3 RID: 1267
	public class LevelRoads
	{
		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x060027B7 RID: 10167 RVA: 0x000A6D1C File Offset: 0x000A4F1C
		[Obsolete("Was the parent of all roads in the past, but now empty for TransformHierarchy performance.")]
		public static Transform models
		{
			get
			{
				if (LevelRoads._models == null)
				{
					LevelRoads._models = new GameObject().transform;
					LevelRoads._models.name = "Roads";
					LevelRoads._models.parent = Level.level;
					LevelRoads._models.tag = "Logic";
					LevelRoads._models.gameObject.layer = 8;
					CommandWindow.LogWarningFormat("Plugin referencing LevelRoads.models which has been deprecated.", Array.Empty<object>());
				}
				return LevelRoads._models;
			}
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x060027B8 RID: 10168 RVA: 0x000A6D96 File Offset: 0x000A4F96
		public static RoadMaterial[] materials
		{
			get
			{
				return LevelRoads._materials;
			}
		}

		// Token: 0x060027B9 RID: 10169 RVA: 0x000A6DA0 File Offset: 0x000A4FA0
		public static void setEnabled(bool isEnabled)
		{
			for (int i = 0; i < LevelRoads.roads.Count; i++)
			{
				LevelRoads.roads[i].setEnabled(isEnabled);
			}
		}

		// Token: 0x060027BA RID: 10170 RVA: 0x000A6DD3 File Offset: 0x000A4FD3
		public static Transform addRoad(Vector3 point)
		{
			LevelRoads.roads.Add(new Road(EditorRoads.selected, 0));
			return LevelRoads.roads[LevelRoads.roads.Count - 1].addVertex(0, point);
		}

		// Token: 0x060027BB RID: 10171 RVA: 0x000A6E08 File Offset: 0x000A5008
		[Obsolete]
		public static void removeRoad(Transform select)
		{
			for (int i = 0; i < LevelRoads.roads.Count; i++)
			{
				for (int j = 0; j < LevelRoads.roads[i].paths.Count; j++)
				{
					if (LevelRoads.roads[i].paths[j].vertex == select)
					{
						LevelRoads.roads[i].remove();
						LevelRoads.roads.RemoveAt(i);
						return;
					}
				}
			}
		}

		// Token: 0x060027BC RID: 10172 RVA: 0x000A6E8C File Offset: 0x000A508C
		public static void removeRoad(Road road)
		{
			for (int i = 0; i < LevelRoads.roads.Count; i++)
			{
				if (LevelRoads.roads[i] == road)
				{
					LevelRoads.roads[i].remove();
					LevelRoads.roads.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x060027BD RID: 10173 RVA: 0x000A6ED8 File Offset: 0x000A50D8
		public static RoadMaterial getRoadMaterial(Transform road)
		{
			if (road == null || road.parent == null)
			{
				return null;
			}
			for (int i = 0; i < LevelRoads.roads.Count; i++)
			{
				if (LevelRoads.roads[i].road == road || LevelRoads.roads[i].road == road.parent)
				{
					return LevelRoads.materials[(int)LevelRoads.roads[i].material];
				}
			}
			return null;
		}

		// Token: 0x060027BE RID: 10174 RVA: 0x000A6F60 File Offset: 0x000A5160
		public static Road getRoad(int index)
		{
			if (index < 0 || index >= LevelRoads.roads.Count)
			{
				return null;
			}
			return LevelRoads.roads[index];
		}

		// Token: 0x060027BF RID: 10175 RVA: 0x000A6F80 File Offset: 0x000A5180
		public static int getRoadIndex(Road road)
		{
			for (int i = 0; i < LevelRoads.roads.Count; i++)
			{
				if (LevelRoads.roads[i] == road)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060027C0 RID: 10176 RVA: 0x000A6FB4 File Offset: 0x000A51B4
		public static Road getRoad(Transform target, out int vertexIndex, out int tangentIndex)
		{
			vertexIndex = -1;
			tangentIndex = -1;
			for (int i = 0; i < LevelRoads.roads.Count; i++)
			{
				Road road = LevelRoads.roads[i];
				for (int j = 0; j < road.paths.Count; j++)
				{
					RoadPath roadPath = road.paths[j];
					if (roadPath.vertex == target)
					{
						vertexIndex = j;
						return road;
					}
					if (roadPath.tangents[0] == target)
					{
						vertexIndex = j;
						tangentIndex = 0;
						return road;
					}
					if (roadPath.tangents[1] == target)
					{
						vertexIndex = j;
						tangentIndex = 1;
						return road;
					}
				}
			}
			return null;
		}

		// Token: 0x060027C1 RID: 10177 RVA: 0x000A7054 File Offset: 0x000A5254
		public static void bakeRoads()
		{
			for (int i = 0; i < LevelRoads.roads.Count; i++)
			{
				LevelRoads.roads[i].updatePoints();
			}
			LevelRoads.buildMeshes();
		}

		// Token: 0x060027C2 RID: 10178 RVA: 0x000A708C File Offset: 0x000A528C
		public static void load()
		{
			if (ReadWrite.fileExists(Level.info.path + "/Environment/Roads.unity3d", false, false))
			{
				try
				{
					Bundle bundle = Bundles.getBundle(Level.info.path + "/Environment/Roads.unity3d", false);
					Texture2D[] array = bundle.loadAll<Texture2D>();
					bundle.unload();
					LevelRoads._materials = new RoadMaterial[array.Length];
					for (int i = 0; i < LevelRoads.materials.Length; i++)
					{
						LevelRoads.materials[i] = new RoadMaterial(array[i]);
					}
					goto IL_9A;
				}
				catch (Exception e)
				{
					UnturnedLog.error("Failed to load level Roads bundle! Most likely needs to be re-built from Unity.");
					UnturnedLog.exception(e);
					LevelRoads._materials = new RoadMaterial[0];
					goto IL_9A;
				}
			}
			LevelRoads._materials = new RoadMaterial[0];
			IL_9A:
			LevelRoads.roads = new List<Road>();
			if (ReadWrite.fileExists(Level.info.path + "/Environment/Roads.dat", false, false))
			{
				River river = new River(Level.info.path + "/Environment/Roads.dat", false);
				byte b = river.readByte();
				if (b > 0)
				{
					byte b2 = river.readByte();
					byte b3 = 0;
					while (b3 < b2 && (int)b3 < LevelRoads.materials.Length)
					{
						LevelRoads.materials[(int)b3].width = river.readSingle();
						LevelRoads.materials[(int)b3].height = river.readSingle();
						LevelRoads.materials[(int)b3].depth = river.readSingle();
						if (b > 1)
						{
							LevelRoads.materials[(int)b3].offset = river.readSingle();
						}
						LevelRoads.materials[(int)b3].isConcrete = river.readBoolean();
						b3 += 1;
					}
				}
				river.closeRiver();
			}
			if (ReadWrite.fileExists(Level.info.path + "/Environment/Paths.dat", false, false))
			{
				River river2 = new River(Level.info.path + "/Environment/Paths.dat", false);
				byte b4 = river2.readByte();
				if (b4 > 1)
				{
					ushort num = river2.readUInt16();
					for (ushort num2 = 0; num2 < num; num2 += 1)
					{
						ushort num3 = river2.readUInt16();
						byte newMaterial = river2.readByte();
						bool newLoop = b4 > 2 && river2.readBoolean();
						List<RoadJoint> list = new List<RoadJoint>();
						for (ushort num4 = 0; num4 < num3; num4 += 1)
						{
							Vector3 vertex = river2.readSingleVector3();
							Vector3[] array2 = new Vector3[2];
							if (b4 > 2)
							{
								array2[0] = river2.readSingleVector3();
								array2[1] = river2.readSingleVector3();
							}
							ERoadMode mode;
							if (b4 > 2)
							{
								mode = (ERoadMode)river2.readByte();
							}
							else
							{
								mode = ERoadMode.FREE;
							}
							float offset;
							if (b4 > 4)
							{
								offset = river2.readSingle();
							}
							else
							{
								offset = 0f;
							}
							bool ignoreTerrain = b4 > 3 && river2.readBoolean();
							RoadJoint roadJoint = new RoadJoint(vertex, array2, mode, offset, ignoreTerrain);
							list.Add(roadJoint);
						}
						if (b4 < 3)
						{
							for (ushort num5 = 0; num5 < num3; num5 += 1)
							{
								RoadJoint roadJoint2 = list[(int)num5];
								if (num5 == 0)
								{
									roadJoint2.setTangent(0, (roadJoint2.vertex - list[(int)(num5 + 1)].vertex).normalized * 2.5f);
									roadJoint2.setTangent(1, (list[(int)(num5 + 1)].vertex - roadJoint2.vertex).normalized * 2.5f);
								}
								else if (num5 == num3 - 1)
								{
									roadJoint2.setTangent(0, (list[(int)(num5 - 1)].vertex - roadJoint2.vertex).normalized * 2.5f);
									roadJoint2.setTangent(1, (roadJoint2.vertex - list[(int)(num5 - 1)].vertex).normalized * 2.5f);
								}
								else
								{
									roadJoint2.setTangent(0, (list[(int)(num5 - 1)].vertex - roadJoint2.vertex).normalized * 2.5f);
									roadJoint2.setTangent(1, (list[(int)(num5 + 1)].vertex - roadJoint2.vertex).normalized * 2.5f);
								}
							}
						}
						LevelRoads.roads.Add(new Road(newMaterial, num2, newLoop, list));
					}
				}
				else if (b4 > 0)
				{
					byte b5 = river2.readByte();
					for (byte b6 = 0; b6 < b5; b6 += 1)
					{
						byte b7 = river2.readByte();
						byte newMaterial2 = river2.readByte();
						List<RoadJoint> list2 = new List<RoadJoint>();
						for (byte b8 = 0; b8 < b7; b8 += 1)
						{
							Vector3 vertex2 = river2.readSingleVector3();
							Vector3[] tangents = new Vector3[2];
							ERoadMode mode2 = ERoadMode.FREE;
							RoadJoint roadJoint3 = new RoadJoint(vertex2, tangents, mode2, 0f, false);
							list2.Add(roadJoint3);
						}
						for (byte b9 = 0; b9 < b7; b9 += 1)
						{
							RoadJoint roadJoint4 = list2[(int)b9];
							if (b9 == 0)
							{
								roadJoint4.setTangent(0, (roadJoint4.vertex - list2[(int)(b9 + 1)].vertex).normalized * 2.5f);
								roadJoint4.setTangent(1, (list2[(int)(b9 + 1)].vertex - roadJoint4.vertex).normalized * 2.5f);
							}
							else if (b9 == b7 - 1)
							{
								roadJoint4.setTangent(0, (list2[(int)(b9 - 1)].vertex - roadJoint4.vertex).normalized * 2.5f);
								roadJoint4.setTangent(1, (roadJoint4.vertex - list2[(int)(b9 - 1)].vertex).normalized * 2.5f);
							}
							else
							{
								roadJoint4.setTangent(0, (list2[(int)(b9 - 1)].vertex - roadJoint4.vertex).normalized * 2.5f);
								roadJoint4.setTangent(1, (list2[(int)(b9 + 1)].vertex - roadJoint4.vertex).normalized * 2.5f);
							}
						}
						LevelRoads.roads.Add(new Road(newMaterial2, (ushort)b6, false, list2));
					}
				}
				river2.closeRiver();
			}
			if (!LevelRoads.isListeningForLandscape)
			{
				LevelRoads.isListeningForLandscape = true;
				Landscape.loaded += LevelRoads.handleLandscapeLoaded;
			}
		}

		// Token: 0x060027C3 RID: 10179 RVA: 0x000A773C File Offset: 0x000A593C
		public static void save()
		{
			River river = new River(Level.info.path + "/Environment/Roads.dat", false);
			river.writeByte(LevelRoads.SAVEDATA_ROADS_VERSION);
			river.writeByte((byte)LevelRoads.materials.Length);
			byte b = 0;
			while ((int)b < LevelRoads.materials.Length)
			{
				river.writeSingle(LevelRoads.materials[(int)b].width);
				river.writeSingle(LevelRoads.materials[(int)b].height);
				river.writeSingle(LevelRoads.materials[(int)b].depth);
				river.writeSingle(LevelRoads.materials[(int)b].offset);
				river.writeBoolean(LevelRoads.materials[(int)b].isConcrete);
				b += 1;
			}
			river.closeRiver();
			river = new River(Level.info.path + "/Environment/Paths.dat", false);
			river.writeByte(LevelRoads.SAVEDATA_PATHS_VERSION);
			ushort num = 0;
			ushort num2 = 0;
			while ((int)num2 < LevelRoads.roads.Count)
			{
				if (LevelRoads.roads[(int)num2].joints.Count > 1)
				{
					num += 1;
				}
				num2 += 1;
			}
			river.writeUInt16(num);
			ushort num3 = 0;
			while ((int)num3 < LevelRoads.roads.Count)
			{
				List<RoadJoint> joints = LevelRoads.roads[(int)num3].joints;
				if (joints.Count > 1)
				{
					river.writeUInt16((ushort)joints.Count);
					river.writeByte(LevelRoads.roads[(int)num3].material);
					river.writeBoolean(LevelRoads.roads[(int)num3].isLoop);
					ushort num4 = 0;
					while ((int)num4 < joints.Count)
					{
						RoadJoint roadJoint = joints[(int)num4];
						river.writeSingleVector3(roadJoint.vertex);
						river.writeSingleVector3(roadJoint.getTangent(0));
						river.writeSingleVector3(roadJoint.getTangent(1));
						river.writeByte((byte)roadJoint.mode);
						river.writeSingle(roadJoint.offset);
						river.writeBoolean(roadJoint.ignoreTerrain);
						num4 += 1;
					}
				}
				num3 += 1;
			}
			river.closeRiver();
		}

		// Token: 0x060027C4 RID: 10180 RVA: 0x000A7948 File Offset: 0x000A5B48
		private static void buildMeshes()
		{
			for (int i = 0; i < LevelRoads.roads.Count; i++)
			{
				LevelRoads.roads[i].buildMesh();
			}
		}

		// Token: 0x060027C5 RID: 10181 RVA: 0x000A797A File Offset: 0x000A5B7A
		private static void handleLandscapeLoaded()
		{
			if (Level.isEditor)
			{
				LevelRoads.bakeRoads();
				return;
			}
			LevelRoads.buildMeshes();
		}

		// Token: 0x04001507 RID: 5383
		public static readonly byte SAVEDATA_ROADS_VERSION = 2;

		// Token: 0x04001508 RID: 5384
		public static readonly byte SAVEDATA_PATHS_VERSION = 5;

		// Token: 0x04001509 RID: 5385
		private static Transform _models;

		// Token: 0x0400150A RID: 5386
		private static RoadMaterial[] _materials;

		// Token: 0x0400150B RID: 5387
		private static List<Road> roads;

		// Token: 0x0400150C RID: 5388
		private static bool isListeningForLandscape;
	}
}
