using System;
using System.Collections.Generic;
using System.Globalization;
using Pathfinding;
using SDG.Framework.Landscapes;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004EB RID: 1259
	public class LevelNavigation
	{
		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x0600271A RID: 10010 RVA: 0x000A1E98 File Offset: 0x000A0098
		[Obsolete("Was the parent of misc objects in the past, but now empty for TransformHierarchy performance.")]
		public static Transform models
		{
			get
			{
				if (LevelNavigation._models == null)
				{
					LevelNavigation._models = new GameObject().transform;
					LevelNavigation._models.name = "Navigation";
					LevelNavigation._models.parent = Level.level;
					LevelNavigation._models.tag = "Logic";
					LevelNavigation._models.gameObject.layer = 8;
					CommandWindow.LogWarningFormat("Plugin referencing LevelNavigation.models which has been deprecated.", Array.Empty<object>());
				}
				return LevelNavigation._models;
			}
		}

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x0600271B RID: 10011 RVA: 0x000A1F12 File Offset: 0x000A0112
		public static List<Bounds> bounds
		{
			get
			{
				return LevelNavigation._bounds;
			}
		}

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x0600271C RID: 10012 RVA: 0x000A1F19 File Offset: 0x000A0119
		// (set) Token: 0x0600271D RID: 10013 RVA: 0x000A1F20 File Offset: 0x000A0120
		public static List<FlagData> flagData { get; private set; }

		// Token: 0x0600271E RID: 10014 RVA: 0x000A1F28 File Offset: 0x000A0128
		public static bool tryGetBounds(Vector3 point, out byte bound)
		{
			bound = byte.MaxValue;
			if (LevelNavigation.bounds != null)
			{
				byte b = 0;
				while ((int)b < LevelNavigation.bounds.Count)
				{
					if (LevelNavigation.bounds[(int)b].ContainsXZ(point))
					{
						bound = b;
						return true;
					}
					b += 1;
				}
			}
			return false;
		}

		// Token: 0x0600271F RID: 10015 RVA: 0x000A1F74 File Offset: 0x000A0174
		public static bool tryGetNavigation(Vector3 point, out byte nav)
		{
			nav = byte.MaxValue;
			if (AstarPath.active != null)
			{
				byte b = 0;
				while ((int)b < Mathf.Min(LevelNavigation.bounds.Count, AstarPath.active.graphs.Length))
				{
					if (AstarPath.active.graphs[(int)b] != null && ((RecastGraph)AstarPath.active.graphs[(int)b]).forcedBounds.ContainsXZ(point))
					{
						nav = b;
						return true;
					}
					b += 1;
				}
			}
			return false;
		}

		// Token: 0x06002720 RID: 10016 RVA: 0x000A1FEE File Offset: 0x000A01EE
		public static bool checkSafe(byte bound)
		{
			return LevelNavigation.bounds != null && (int)bound < LevelNavigation.bounds.Count;
		}

		// Token: 0x06002721 RID: 10017 RVA: 0x000A2008 File Offset: 0x000A0208
		public static bool checkSafe(Vector3 point)
		{
			if (LevelNavigation.bounds == null)
			{
				return false;
			}
			byte b = 0;
			while ((int)b < LevelNavigation.bounds.Count)
			{
				if (LevelNavigation.bounds[(int)b].ContainsXZ(point))
				{
					return true;
				}
				b += 1;
			}
			return false;
		}

		// Token: 0x06002722 RID: 10018 RVA: 0x000A204C File Offset: 0x000A024C
		public static bool checkSafeFakeNav(Vector3 point)
		{
			if (LevelNavigation.bounds == null)
			{
				return false;
			}
			byte b = 0;
			while ((int)b < LevelNavigation.bounds.Count)
			{
				Bounds bounds = LevelNavigation.bounds[(int)b];
				bounds.size -= LevelNavigation.BOUNDS_SIZE;
				if (bounds.ContainsXZ(point))
				{
					return true;
				}
				b += 1;
			}
			return false;
		}

		// Token: 0x06002723 RID: 10019 RVA: 0x000A20A8 File Offset: 0x000A02A8
		public static bool checkNavigation(Vector3 point)
		{
			if (AstarPath.active == null)
			{
				return false;
			}
			byte b = 0;
			while ((int)b < AstarPath.active.graphs.Length)
			{
				if (AstarPath.active.graphs[(int)b] != null && ((RecastGraph)AstarPath.active.graphs[(int)b]).forcedBounds.ContainsXZ(point))
				{
					return true;
				}
				b += 1;
			}
			return false;
		}

		// Token: 0x06002724 RID: 10020 RVA: 0x000A210C File Offset: 0x000A030C
		public static void setEnabled(bool isEnabled)
		{
			if (LevelNavigation.flags == null)
			{
				return;
			}
			for (int i = 0; i < LevelNavigation.flags.Count; i++)
			{
				LevelNavigation.flags[i].setEnabled(isEnabled);
			}
		}

		// Token: 0x06002725 RID: 10021 RVA: 0x000A2148 File Offset: 0x000A0348
		public static RecastGraph addGraph()
		{
			RecastGraph recastGraph = (RecastGraph)AstarPath.active.astarData.AddGraph(typeof(RecastGraph));
			recastGraph.cellSize = 0.1f;
			recastGraph.cellHeight = 0.1f;
			recastGraph.useTiles = true;
			recastGraph.editorTileSize = 128;
			recastGraph.minRegionSize = 64f;
			recastGraph.walkableHeight = 2f;
			recastGraph.walkableClimb = 0.75f;
			recastGraph.characterRadius = 0.5f;
			recastGraph.maxSlope = 75f;
			recastGraph.maxEdgeLength = 16f;
			recastGraph.contourMaxError = 2f;
			recastGraph.terrainSampleSize = 1;
			recastGraph.rasterizeTrees = false;
			recastGraph.rasterizeMeshes = false;
			recastGraph.rasterizeColliders = true;
			recastGraph.colliderRasterizeDetail = 4f;
			recastGraph.mask = RayMasks.BLOCK_NAVMESH;
			return recastGraph;
		}

		// Token: 0x06002726 RID: 10022 RVA: 0x000A2220 File Offset: 0x000A0420
		public static void updateBounds()
		{
			LevelNavigation._bounds = new List<Bounds>();
			for (int i = 0; i < AstarPath.active.graphs.Length; i++)
			{
				RecastGraph recastGraph = (RecastGraph)AstarPath.active.graphs[i];
				if (recastGraph != null)
				{
					LevelNavigation.bounds.Add(new Bounds(recastGraph.forcedBoundsCenter, recastGraph.forcedBoundsSize + LevelNavigation.BOUNDS_SIZE));
				}
				else
				{
					LevelNavigation.bounds.Add(new Bounds(new Vector3(20000f, 20000f, 20000f), Vector3.zero));
				}
			}
		}

		// Token: 0x06002727 RID: 10023 RVA: 0x000A22B4 File Offset: 0x000A04B4
		public static Transform addFlag(Vector3 point)
		{
			RecastGraph graph = null;
			Func<bool, bool> func = delegate(bool force)
			{
				graph = LevelNavigation.addGraph();
				return true;
			};
			AstarPath.active.AddWorkItem(new AstarPath.AstarWorkItem(func));
			AstarPath.active.FlushWorkItems(true, false);
			FlagData flagData = new FlagData("", 64, true, false, -1);
			LevelNavigation.flags.Add(new Flag(point, graph, flagData));
			LevelNavigation.flagData.Add(flagData);
			return LevelNavigation.flags[LevelNavigation.flags.Count - 1].model;
		}

		// Token: 0x06002728 RID: 10024 RVA: 0x000A2344 File Offset: 0x000A0544
		public static void removeFlag(Transform select)
		{
			for (int i = 0; i < LevelNavigation.flags.Count; i++)
			{
				if (LevelNavigation.flags[i].model == select)
				{
					for (int j = i + 1; j < LevelNavigation.flags.Count; j++)
					{
						LevelNavigation.flags[j].needsNavigationSave = true;
					}
					try
					{
						LevelNavigation.flags[i].remove();
					}
					catch
					{
					}
					LevelNavigation.flags.RemoveAt(i);
					LevelNavigation.flagData.RemoveAt(i);
					break;
				}
			}
			LevelNavigation.updateBounds();
		}

		// Token: 0x06002729 RID: 10025 RVA: 0x000A23EC File Offset: 0x000A05EC
		public static Flag getFlag(Transform select)
		{
			for (int i = 0; i < LevelNavigation.flags.Count; i++)
			{
				if (LevelNavigation.flags[i].model == select)
				{
					return LevelNavigation.flags[i];
				}
			}
			return null;
		}

		// Token: 0x0600272A RID: 10026 RVA: 0x000A2434 File Offset: 0x000A0634
		public static void load()
		{
			LevelNavigation._bounds = new List<Bounds>();
			LevelNavigation.flagData = new List<FlagData>();
			RecastGraph.UnturnedIsPointInsideTerrainHole = new Func<Vector3, bool>(Landscape.IsPointInsideHole);
			if (ReadWrite.fileExists(Level.info.path + "/Environment/Bounds.dat", false, false))
			{
				River river = new River(Level.info.path + "/Environment/Bounds.dat", false);
				if (river.readByte() > 0)
				{
					byte b = river.readByte();
					for (byte b2 = 0; b2 < b; b2 += 1)
					{
						Vector3 center = river.readSingleVector3();
						Vector3 size = river.readSingleVector3();
						LevelNavigation.bounds.Add(new Bounds(center, size));
					}
				}
				river.closeRiver();
			}
			if (ReadWrite.fileExists(Level.info.path + "/Environment/Flags_Data.dat", false, false))
			{
				River river2 = new River(Level.info.path + "/Environment/Flags_Data.dat", false);
				byte b3 = river2.readByte();
				if (b3 > 0)
				{
					byte b4 = river2.readByte();
					for (byte b5 = 0; b5 < b4; b5 += 1)
					{
						string newDifficultyGUID = river2.readString();
						byte newMaxZombies = 64;
						if (b3 > 1)
						{
							newMaxZombies = river2.readByte();
						}
						bool newSpawnZombies = true;
						if (b3 > 2)
						{
							newSpawnZombies = river2.readBoolean();
						}
						bool newHyperAgro = false;
						if (b3 >= 4)
						{
							newHyperAgro = river2.readBoolean();
						}
						int maxBossZombies = -1;
						if (b3 >= 5)
						{
							maxBossZombies = river2.readInt32();
						}
						LevelNavigation.flagData.Add(new FlagData(newDifficultyGUID, newMaxZombies, newSpawnZombies, newHyperAgro, maxBossZombies));
					}
				}
				river2.closeRiver();
			}
			if (LevelNavigation.flagData.Count < LevelNavigation.bounds.Count)
			{
				for (int i = LevelNavigation.flagData.Count; i < LevelNavigation.bounds.Count; i++)
				{
					LevelNavigation.flagData.Add(new FlagData("", 64, true, false, -1));
				}
			}
			Func<bool, bool> func = delegate(bool force)
			{
				if (Level.isEditor)
				{
					LevelNavigation.flags = new List<Flag>();
					Object.Destroy(AstarPath.active.GetComponent<TileHandlerHelpers>());
					if (ReadWrite.fileExists(Level.info.path + "/Environment/Flags.dat", false, false))
					{
						River river3 = new River(Level.info.path + "/Environment/Flags.dat", false);
						byte b6 = river3.readByte();
						if (b6 > 2)
						{
							byte b7 = river3.readByte();
							if (LevelNavigation.flagData.Count < (int)b7)
							{
								UnturnedLog.error(string.Format("Navigation flag data count ({0}) does not match flags count ({1}) during editor load, fixing", LevelNavigation.flagData.Count, b7));
								for (int j = LevelNavigation.flagData.Count; j < (int)b7; j++)
								{
									LevelNavigation.flagData.Add(new FlagData("", 64, true, false, -1));
								}
							}
							for (byte b8 = 0; b8 < b7; b8 += 1)
							{
								Vector3 newPoint = river3.readSingleVector3();
								float num = river3.readSingle();
								float num2 = river3.readSingle();
								if (b6 < 4)
								{
									num *= 0.5f;
									num2 *= 0.5f;
								}
								RecastGraph recastGraph = null;
								if (ReadWrite.fileExists(Level.info.path + "/Environment/Navigation_" + b8.ToString(CultureInfo.InvariantCulture) + ".dat", false, false))
								{
									River river4 = new River(Level.info.path + "/Environment/Navigation_" + b8.ToString(CultureInfo.InvariantCulture) + ".dat", false);
									if (river4.readByte() > 0)
									{
										recastGraph = LevelNavigation.buildGraph(river4);
									}
									river4.closeRiver();
								}
								if (recastGraph == null)
								{
									recastGraph = LevelNavigation.addGraph();
								}
								LevelNavigation.flags.Add(new Flag(newPoint, num, num2, recastGraph, LevelNavigation.flagData[(int)b8]));
							}
						}
						river3.closeRiver();
					}
					if (LevelNavigation.bounds.Count != AstarPath.active.graphs.Length)
					{
						UnturnedLog.error("Navigation bounds count ({0}) does not match graph count ({1}) during editor load, fixing", new object[]
						{
							LevelNavigation.bounds.Count,
							AstarPath.active.graphs.Length
						});
						LevelNavigation.updateBounds();
					}
				}
				else if (Provider.isServer)
				{
					int k = 0;
					int num3 = 0;
					while (k < 5)
					{
						string text = Level.info.path + "/Environment/Navigation_" + num3.ToString(CultureInfo.InvariantCulture) + ".dat";
						if (ReadWrite.fileExists(text, false, false))
						{
							River river5 = new River(text, false);
							if (river5.readByte() > 0)
							{
								LevelNavigation.buildGraph(river5);
							}
							river5.closeRiver();
							k = 0;
						}
						else
						{
							k++;
						}
						num3++;
					}
					if (LevelNavigation.bounds.Count != AstarPath.active.graphs.Length)
					{
						UnturnedLog.error("Navigation bounds count ({0}) does not match graph count ({1}) during server load", new object[]
						{
							LevelNavigation.bounds.Count,
							AstarPath.active.graphs.Length
						});
					}
				}
				return true;
			};
			AstarPath.active.AddWorkItem(new AstarPath.AstarWorkItem(func));
			AstarPath.active.FlushWorkItems(true, false);
		}

		// Token: 0x0600272B RID: 10027 RVA: 0x000A2648 File Offset: 0x000A0848
		public static void save()
		{
			if (LevelNavigation.bounds.Count != AstarPath.active.graphs.Length)
			{
				UnturnedLog.error("Navigation bounds count ({0}) does not match graph count ({1}) during save", new object[]
				{
					LevelNavigation.bounds.Count,
					AstarPath.active.graphs.Length
				});
			}
			River river = new River(Level.info.path + "/Environment/Bounds.dat", false);
			river.writeByte(LevelNavigation.SAVEDATA_BOUNDS_VERSION);
			river.writeByte((byte)LevelNavigation.bounds.Count);
			byte b = 0;
			while ((int)b < LevelNavigation.bounds.Count)
			{
				river.writeSingleVector3(LevelNavigation.bounds[(int)b].center);
				river.writeSingleVector3(LevelNavigation.bounds[(int)b].size);
				b += 1;
			}
			river.closeRiver();
			River river2 = new River(Level.info.path + "/Environment/Flags_Data.dat", false);
			river2.writeByte(5);
			river2.writeByte((byte)LevelNavigation.flagData.Count);
			byte b2 = 0;
			while ((int)b2 < LevelNavigation.flagData.Count)
			{
				river2.writeString(LevelNavigation.flagData[(int)b2].difficultyGUID);
				river2.writeByte(LevelNavigation.flagData[(int)b2].maxZombies);
				river2.writeBoolean(LevelNavigation.flagData[(int)b2].spawnZombies);
				river2.writeBoolean(LevelNavigation.flagData[(int)b2].hyperAgro);
				river2.writeInt32(LevelNavigation.flagData[(int)b2].maxBossZombies);
				b2 += 1;
			}
			river2.closeRiver();
			River river3 = new River(Level.info.path + "/Environment/Flags.dat", false);
			river3.writeByte(LevelNavigation.SAVEDATA_FLAGS_VERSION);
			int num = LevelNavigation.flags.Count;
			while (ReadWrite.fileExists(Level.info.path + "/Environment/Navigation_" + num.ToString(CultureInfo.InvariantCulture) + ".dat", false, false))
			{
				ReadWrite.deleteFile(Level.info.path + "/Environment/Navigation_" + num.ToString(CultureInfo.InvariantCulture) + ".dat", false, false);
				num++;
			}
			river3.writeByte((byte)LevelNavigation.flags.Count);
			byte b3 = 0;
			while ((int)b3 < LevelNavigation.flags.Count)
			{
				Flag flag = LevelNavigation.flags[(int)b3];
				river3.writeSingleVector3(flag.point);
				river3.writeSingle(flag.width);
				river3.writeSingle(flag.height);
				if (flag.needsNavigationSave)
				{
					River river4 = new River(Level.info.path + "/Environment/Navigation_" + b3.ToString(CultureInfo.InvariantCulture) + ".dat", false);
					river4.writeByte(LevelNavigation.SAVEDATA_NAVIGATION_VERSION);
					RecastGraph graph = flag.graph;
					river4.writeSingleVector3(graph.forcedBoundsCenter);
					river4.writeSingleVector3(graph.forcedBoundsSize);
					river4.writeByte((byte)graph.tileXCount);
					river4.writeByte((byte)graph.tileZCount);
					RecastGraph.NavmeshTile[] tiles = graph.GetTiles();
					for (int i = 0; i < graph.tileZCount; i++)
					{
						for (int j = 0; j < graph.tileXCount; j++)
						{
							RecastGraph.NavmeshTile navmeshTile = tiles[j + i * graph.tileXCount];
							river4.writeUInt16((ushort)navmeshTile.tris.Length);
							for (int k = 0; k < navmeshTile.tris.Length; k++)
							{
								river4.writeUInt16((ushort)navmeshTile.tris[k]);
							}
							river4.writeUInt16((ushort)navmeshTile.verts.Length);
							for (int l = 0; l < navmeshTile.verts.Length; l++)
							{
								Int3 @int = navmeshTile.verts[l];
								river4.writeInt32(@int.x);
								river4.writeInt32(@int.y);
								river4.writeInt32(@int.z);
							}
						}
					}
					river4.closeRiver();
					flag.needsNavigationSave = false;
				}
				b3 += 1;
			}
			river3.closeRiver();
		}

		// Token: 0x0600272C RID: 10028 RVA: 0x000A2A80 File Offset: 0x000A0C80
		private static RecastGraph buildGraph(River river)
		{
			RecastGraph recastGraph = LevelNavigation.addGraph();
			int graphIndex = AstarPath.active.astarData.GetGraphIndex(recastGraph);
			TriangleMeshNode.SetNavmeshHolder(graphIndex, recastGraph);
			recastGraph.forcedBoundsCenter = river.readSingleVector3();
			recastGraph.forcedBoundsSize = river.readSingleVector3();
			recastGraph.tileXCount = (int)river.readByte();
			recastGraph.tileZCount = (int)river.readByte();
			RecastGraph.NavmeshTile[] array = new RecastGraph.NavmeshTile[recastGraph.tileXCount * recastGraph.tileZCount];
			recastGraph.SetTiles(array);
			for (int i = 0; i < recastGraph.tileZCount; i++)
			{
				for (int j = 0; j < recastGraph.tileXCount; j++)
				{
					RecastGraph.NavmeshTile navmeshTile = new RecastGraph.NavmeshTile();
					navmeshTile.x = j;
					navmeshTile.z = i;
					navmeshTile.w = 1;
					navmeshTile.d = 1;
					navmeshTile.bbTree = new BBTree(navmeshTile);
					int num = j + i * recastGraph.tileXCount;
					array[num] = navmeshTile;
					navmeshTile.tris = new int[(int)river.readUInt16()];
					for (int k = 0; k < navmeshTile.tris.Length; k++)
					{
						navmeshTile.tris[k] = (int)river.readUInt16();
					}
					navmeshTile.verts = new Int3[(int)river.readUInt16()];
					for (int l = 0; l < navmeshTile.verts.Length; l++)
					{
						navmeshTile.verts[l] = new Int3(river.readInt32(), river.readInt32(), river.readInt32());
					}
					navmeshTile.nodes = new TriangleMeshNode[navmeshTile.tris.Length / 3];
					num <<= 12;
					for (int m = 0; m < navmeshTile.nodes.Length; m++)
					{
						navmeshTile.nodes[m] = new TriangleMeshNode(AstarPath.active);
						TriangleMeshNode triangleMeshNode = navmeshTile.nodes[m];
						triangleMeshNode.GraphIndex = (uint)graphIndex;
						triangleMeshNode.Penalty = 0U;
						triangleMeshNode.Walkable = true;
						triangleMeshNode.v0 = (navmeshTile.tris[m * 3] | num);
						triangleMeshNode.v1 = (navmeshTile.tris[m * 3 + 1] | num);
						triangleMeshNode.v2 = (navmeshTile.tris[m * 3 + 2] | num);
						triangleMeshNode.UpdatePositionFromVertices();
						navmeshTile.bbTree.Insert(triangleMeshNode);
					}
					recastGraph.CreateNodeConnections(navmeshTile.nodes);
				}
			}
			for (int n = 0; n < recastGraph.tileZCount; n++)
			{
				for (int num2 = 0; num2 < recastGraph.tileXCount; num2++)
				{
					RecastGraph.NavmeshTile navmeshTile2 = array[num2 + n * recastGraph.tileXCount];
					recastGraph.ConnectTileWithNeighbours(navmeshTile2);
				}
			}
			return recastGraph;
		}

		// Token: 0x040014BA RID: 5306
		public static readonly Vector3 BOUNDS_SIZE = new Vector3(64f, 64f, 64f);

		// Token: 0x040014BB RID: 5307
		public static readonly byte SAVEDATA_BOUNDS_VERSION = 1;

		// Token: 0x040014BC RID: 5308
		public static readonly byte SAVEDATA_FLAGS_VERSION = 4;

		// Token: 0x040014BD RID: 5309
		internal const byte SAVEDATA_VERSION_FLAG_DATA_ADDED_HYPER_AGRO = 4;

		// Token: 0x040014BE RID: 5310
		internal const byte SAVEDATA_VERSION_FLAG_DATA_ADDED_MAX_BOSS_COUNT = 5;

		// Token: 0x040014BF RID: 5311
		internal const byte SAVEDATA_VERSION_FLAG_DATA_NEWEST = 5;

		// Token: 0x040014C0 RID: 5312
		public static readonly byte SAVEDATA_FLAG_DATA_VERSION = 5;

		// Token: 0x040014C1 RID: 5313
		public static readonly byte SAVEDATA_NAVIGATION_VERSION = 1;

		// Token: 0x040014C2 RID: 5314
		private static Transform _models;

		// Token: 0x040014C3 RID: 5315
		private static List<Flag> flags;

		// Token: 0x040014C4 RID: 5316
		private static List<Bounds> _bounds;
	}
}
