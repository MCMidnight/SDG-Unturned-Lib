using System;
using System.Collections;
using System.Collections.Generic;
using SDG.Framework.Debug;
using SDG.Framework.Devkit;
using SDG.Framework.Devkit.Transactions;
using SDG.Framework.Foliage;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Landscapes
{
	// Token: 0x0200009D RID: 157
	public class Landscape : DevkitHierarchyItemBase
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x0000F27F File Offset: 0x0000D47F
		// (set) Token: 0x060003D5 RID: 981 RVA: 0x0000F286 File Offset: 0x0000D486
		public static Landscape instance { get; protected set; }

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x060003D6 RID: 982 RVA: 0x0000F290 File Offset: 0x0000D490
		// (remove) Token: 0x060003D7 RID: 983 RVA: 0x0000F2C4 File Offset: 0x0000D4C4
		public static event LandscapeLoadedHandler loaded;

		/// <summary>
		/// Hacky workaround for height and material brushes in editor. As far as I can tell in Unity 2019 LTS there is no method to ignore
		/// holes when raycasting against terrain (e.g. when painting holes), so we use a duplicate TerrainData without holes in the editor.
		/// </summary>
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x0000F2F7 File Offset: 0x0000D4F7
		// (set) Token: 0x060003D9 RID: 985 RVA: 0x0000F300 File Offset: 0x0000D500
		public static bool DisableHoleColliders
		{
			get
			{
				return Landscape._disableHoleColliders;
			}
			set
			{
				if (Landscape._disableHoleColliders == value)
				{
					return;
				}
				Landscape._disableHoleColliders = value;
				foreach (KeyValuePair<LandscapeCoord, LandscapeTile> keyValuePair in Landscape.tiles)
				{
					LandscapeTile value2 = keyValuePair.Value;
					if (value2.collider != null)
					{
						value2.collider.terrainData = (Landscape._disableHoleColliders ? value2.dataWithoutHoles : value2.data);
					}
				}
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060003DA RID: 986 RVA: 0x0000F390 File Offset: 0x0000D590
		// (set) Token: 0x060003DB RID: 987 RVA: 0x0000F397 File Offset: 0x0000D597
		public static bool HighlightHoles
		{
			get
			{
				return Landscape._highlightHoles;
			}
			set
			{
				if (Landscape._highlightHoles == value)
				{
					return;
				}
				Landscape._highlightHoles = value;
				Shader.SetGlobalFloat("_TerrainHighlightHoles", Landscape._highlightHoles ? 1f : 0f);
			}
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000F3C8 File Offset: 0x0000D5C8
		public static void GetUniqueMaterials(List<LandscapeMaterialAsset> materials)
		{
			foreach (KeyValuePair<LandscapeCoord, LandscapeTile> keyValuePair in Landscape.tiles)
			{
				foreach (AssetReference<LandscapeMaterialAsset> assetReference in keyValuePair.Value.materials)
				{
					LandscapeMaterialAsset landscapeMaterialAsset = assetReference.Find();
					if (landscapeMaterialAsset != null && !materials.Contains(landscapeMaterialAsset))
					{
						materials.Add(landscapeMaterialAsset);
					}
				}
			}
		}

		/// <summary>
		/// Is point (on XZ plane) inside a masked-out pixel?
		/// </summary>
		// Token: 0x060003DD RID: 989 RVA: 0x0000F474 File Offset: 0x0000D674
		public static bool IsPointInsideHole(Vector3 worldPosition)
		{
			LandscapeCoord landscapeCoord = new LandscapeCoord(worldPosition);
			LandscapeTile tile = Landscape.getTile(landscapeCoord);
			if (tile != null)
			{
				SplatmapCoord splatmapCoord = new SplatmapCoord(landscapeCoord, worldPosition);
				return !tile.holes[splatmapCoord.x, splatmapCoord.y];
			}
			return false;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000F4B8 File Offset: 0x0000D6B8
		public static bool getWorldHeight(Vector3 position, out float height)
		{
			LandscapeTile tile = Landscape.getTile(new LandscapeCoord(position));
			if (tile != null)
			{
				height = tile.terrain.SampleHeight(position) - Landscape.TILE_HEIGHT / 2f;
				return true;
			}
			height = 0f;
			return false;
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000F4F8 File Offset: 0x0000D6F8
		public static bool getWorldHeight(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, out float height)
		{
			LandscapeTile tile = Landscape.getTile(tileCoord);
			if (tile != null)
			{
				height = tile.heightmap[heightmapCoord.x, heightmapCoord.y] * Landscape.TILE_HEIGHT - Landscape.TILE_HEIGHT / 2f;
				return true;
			}
			height = 0f;
			return false;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000F544 File Offset: 0x0000D744
		public static bool getHeight01(Vector3 position, out float height)
		{
			LandscapeTile tile = Landscape.getTile(new LandscapeCoord(position));
			if (tile != null)
			{
				height = tile.terrain.SampleHeight(position) / Landscape.TILE_HEIGHT;
				return true;
			}
			height = 0f;
			return false;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000F580 File Offset: 0x0000D780
		public static bool getHeight01(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, out float height)
		{
			LandscapeTile tile = Landscape.getTile(tileCoord);
			if (tile != null)
			{
				height = tile.heightmap[heightmapCoord.x, heightmapCoord.y];
				return true;
			}
			height = 0f;
			return false;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000F5BC File Offset: 0x0000D7BC
		public static bool getNormal(Vector3 position, out Vector3 normal)
		{
			LandscapeCoord landscapeCoord = new LandscapeCoord(position);
			LandscapeTile tile = Landscape.getTile(landscapeCoord);
			if (tile != null)
			{
				normal = tile.data.GetInterpolatedNormal((position.x - (float)landscapeCoord.x * Landscape.TILE_SIZE) / Landscape.TILE_SIZE, (position.z - (float)landscapeCoord.y * Landscape.TILE_SIZE) / Landscape.TILE_SIZE);
				return true;
			}
			normal = Vector3.up;
			return false;
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000F630 File Offset: 0x0000D830
		public static bool getSplatmapMaterial(Vector3 position, out AssetReference<LandscapeMaterialAsset> materialAsset)
		{
			LandscapeCoord tileCoord = new LandscapeCoord(position);
			SplatmapCoord splatmapCoord = new SplatmapCoord(tileCoord, position);
			return Landscape.getSplatmapMaterial(tileCoord, splatmapCoord, out materialAsset);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000F658 File Offset: 0x0000D858
		public static bool getSplatmapMaterial(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord, out AssetReference<LandscapeMaterialAsset> materialAsset)
		{
			int num;
			if (Landscape.getSplatmapLayer(tileCoord, splatmapCoord, out num))
			{
				materialAsset = Landscape.getTile(tileCoord).materials[num];
				return true;
			}
			materialAsset = AssetReference<LandscapeMaterialAsset>.invalid;
			return false;
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000F698 File Offset: 0x0000D898
		public static bool getSplatmapLayer(Vector3 position, out int layer)
		{
			LandscapeCoord tileCoord = new LandscapeCoord(position);
			SplatmapCoord splatmapCoord = new SplatmapCoord(tileCoord, position);
			return Landscape.getSplatmapLayer(tileCoord, splatmapCoord, out layer);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000F6C0 File Offset: 0x0000D8C0
		public static bool getSplatmapLayer(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord, out int layer)
		{
			LandscapeTile tile = Landscape.getTile(tileCoord);
			if (tile != null)
			{
				layer = Landscape.getSplatmapHighestWeightLayerIndex(splatmapCoord, tile.splatmap, -1);
				return true;
			}
			layer = -1;
			return false;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000F6EC File Offset: 0x0000D8EC
		public static int getSplatmapHighestWeightLayerIndex(SplatmapCoord splatmapCoord, float[,,] currentWeights, int ignoreLayer = -1)
		{
			float num = -1f;
			int result = -1;
			for (int i = 0; i < Landscape.SPLATMAP_LAYERS; i++)
			{
				if (i != ignoreLayer && currentWeights[splatmapCoord.x, splatmapCoord.y, i] > num)
				{
					num = currentWeights[splatmapCoord.x, splatmapCoord.y, i];
					result = i;
				}
			}
			return result;
		}

		/// <param name="ignoreLayer">If the highest weight layer is ignoreLayer then the next highest will be returned.</param>
		// Token: 0x060003E8 RID: 1000 RVA: 0x0000F744 File Offset: 0x0000D944
		public static int getSplatmapHighestWeightLayerIndex(float[] currentWeights, int ignoreLayer = -1)
		{
			float num = -1f;
			int result = -1;
			for (int i = 0; i < Landscape.SPLATMAP_LAYERS; i++)
			{
				if (i != ignoreLayer && currentWeights[i] > num)
				{
					num = currentWeights[i];
					result = i;
				}
			}
			return result;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000F77A File Offset: 0x0000D97A
		public static void clearHeightmapTransactions()
		{
			Landscape.heightmapTransactions.Clear();
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000F786 File Offset: 0x0000D986
		public static void clearSplatmapTransactions()
		{
			Landscape.splatmapTransactions.Clear();
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000F792 File Offset: 0x0000D992
		public static void clearHoleTransactions()
		{
			Landscape.holeTransactions.Clear();
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000F79E File Offset: 0x0000D99E
		public static bool isPointerInTile(Vector3 worldPosition)
		{
			return Landscape.getTile(worldPosition) != null;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000F7AC File Offset: 0x0000D9AC
		public static Vector3 getWorldPosition(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, float height)
		{
			float x = (float)Mathf.RoundToInt((float)tileCoord.x * Landscape.TILE_SIZE + (float)heightmapCoord.y / (float)Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE * Landscape.TILE_SIZE);
			float y = -Landscape.TILE_HEIGHT / 2f + height * Landscape.TILE_HEIGHT;
			float num = (float)tileCoord.y * Landscape.TILE_SIZE + (float)heightmapCoord.x / (float)Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE * Landscape.TILE_SIZE;
			num = (float)Mathf.RoundToInt(num);
			return new Vector3(x, y, num);
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000F828 File Offset: 0x0000DA28
		public static Vector3 getWorldPosition(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord)
		{
			float num = (float)tileCoord.x * Landscape.TILE_SIZE + (float)splatmapCoord.y / (float)Landscape.SPLATMAP_RESOLUTION * Landscape.TILE_SIZE;
			num = (float)Mathf.RoundToInt(num) + Landscape.HALF_SPLATMAP_WORLD_UNIT;
			float num2 = (float)tileCoord.y * Landscape.TILE_SIZE + (float)splatmapCoord.x / (float)Landscape.SPLATMAP_RESOLUTION * Landscape.TILE_SIZE;
			num2 = (float)Mathf.RoundToInt(num2) + Landscape.HALF_SPLATMAP_WORLD_UNIT;
			Vector3 vector = new Vector3(num, 0f, num2);
			float y;
			Landscape.getWorldHeight(vector, out y);
			vector.y = y;
			return vector;
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000F8B8 File Offset: 0x0000DAB8
		public static void readHeightmap(Bounds worldBounds, Landscape.LandscapeReadHeightmapHandler callback)
		{
			if (callback == null)
			{
				return;
			}
			LandscapeBounds landscapeBounds = new LandscapeBounds(worldBounds);
			for (int i = landscapeBounds.min.x; i <= landscapeBounds.max.x; i++)
			{
				for (int j = landscapeBounds.min.y; j <= landscapeBounds.max.y; j++)
				{
					LandscapeCoord landscapeCoord = new LandscapeCoord(i, j);
					LandscapeTile tile = Landscape.getTile(landscapeCoord);
					if (tile != null)
					{
						HeightmapBounds heightmapBounds = new HeightmapBounds(landscapeCoord, worldBounds);
						for (int k = heightmapBounds.min.x; k < heightmapBounds.max.x; k++)
						{
							for (int l = heightmapBounds.min.y; l < heightmapBounds.max.y; l++)
							{
								HeightmapCoord heightmapCoord = new HeightmapCoord(k, l);
								float num = tile.heightmap[k, l];
								Vector3 worldPosition = Landscape.getWorldPosition(landscapeCoord, heightmapCoord, num);
								callback(landscapeCoord, heightmapCoord, worldPosition, num);
							}
						}
					}
				}
			}
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000F9C0 File Offset: 0x0000DBC0
		public static void readSplatmap(Bounds worldBounds, Landscape.LandscapeReadSplatmapHandler callback)
		{
			if (callback == null)
			{
				return;
			}
			LandscapeBounds landscapeBounds = new LandscapeBounds(worldBounds);
			for (int i = landscapeBounds.min.x; i <= landscapeBounds.max.x; i++)
			{
				for (int j = landscapeBounds.min.y; j <= landscapeBounds.max.y; j++)
				{
					LandscapeCoord landscapeCoord = new LandscapeCoord(i, j);
					LandscapeTile tile = Landscape.getTile(landscapeCoord);
					if (tile != null)
					{
						SplatmapBounds splatmapBounds = new SplatmapBounds(landscapeCoord, worldBounds);
						for (int k = splatmapBounds.min.x; k < splatmapBounds.max.x; k++)
						{
							for (int l = splatmapBounds.min.y; l < splatmapBounds.max.y; l++)
							{
								SplatmapCoord splatmapCoord = new SplatmapCoord(k, l);
								for (int m = 0; m < Landscape.SPLATMAP_LAYERS; m++)
								{
									Landscape.SPLATMAP_LAYER_BUFFER[m] = tile.splatmap[k, l, m];
								}
								Vector3 worldPosition = Landscape.getWorldPosition(landscapeCoord, splatmapCoord);
								callback(landscapeCoord, splatmapCoord, worldPosition, Landscape.SPLATMAP_LAYER_BUFFER);
							}
						}
					}
				}
			}
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000FAE8 File Offset: 0x0000DCE8
		public static void writeHeightmap(Bounds worldBounds, Landscape.LandscapeWriteHeightmapHandler callback)
		{
			if (callback == null)
			{
				return;
			}
			LandscapeBounds landscapeBounds = new LandscapeBounds(worldBounds);
			for (int i = landscapeBounds.min.x; i <= landscapeBounds.max.x; i++)
			{
				for (int j = landscapeBounds.min.y; j <= landscapeBounds.max.y; j++)
				{
					LandscapeCoord landscapeCoord = new LandscapeCoord(i, j);
					LandscapeTile tile = Landscape.getTile(landscapeCoord);
					if (tile != null)
					{
						if (!Landscape.heightmapTransactions.ContainsKey(landscapeCoord))
						{
							LandscapeHeightmapTransaction landscapeHeightmapTransaction = new LandscapeHeightmapTransaction(tile);
							DevkitTransactionManager.recordTransaction(landscapeHeightmapTransaction);
							Landscape.heightmapTransactions.Add(landscapeCoord, landscapeHeightmapTransaction);
						}
						HeightmapBounds heightmapBounds = new HeightmapBounds(landscapeCoord, worldBounds);
						for (int k = heightmapBounds.min.x; k <= heightmapBounds.max.x; k++)
						{
							for (int l = heightmapBounds.min.y; l <= heightmapBounds.max.y; l++)
							{
								HeightmapCoord heightmapCoord = new HeightmapCoord(k, l);
								float num = tile.heightmap[k, l];
								Vector3 worldPosition = Landscape.getWorldPosition(landscapeCoord, heightmapCoord, num);
								tile.heightmap[k, l] = Mathf.Clamp01(callback(landscapeCoord, heightmapCoord, worldPosition, num));
							}
						}
					}
				}
			}
			for (int m = landscapeBounds.min.x; m <= landscapeBounds.max.x; m++)
			{
				for (int n = landscapeBounds.min.y; n <= landscapeBounds.max.y; n++)
				{
					LandscapeTile tile2 = Landscape.getTile(new LandscapeCoord(m, n));
					if (tile2 != null)
					{
						if (m < landscapeBounds.max.x)
						{
							LandscapeTile tile3 = Landscape.getTile(new LandscapeCoord(m + 1, n));
							if (tile3 != null)
							{
								for (int num2 = 0; num2 <= Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE; num2++)
								{
									tile2.heightmap[num2, Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE] = tile3.heightmap[num2, 0];
								}
							}
						}
						if (n < landscapeBounds.max.y)
						{
							LandscapeTile tile4 = Landscape.getTile(new LandscapeCoord(m, n + 1));
							if (tile4 != null)
							{
								for (int num3 = 0; num3 <= Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE; num3++)
								{
									tile2.heightmap[Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE, num3] = tile4.heightmap[0, num3];
								}
							}
						}
						if (m < landscapeBounds.max.x && n < landscapeBounds.max.y)
						{
							LandscapeTile tile5 = Landscape.getTile(new LandscapeCoord(m + 1, n + 1));
							if (tile5 != null)
							{
								tile2.heightmap[Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE, Landscape.HEIGHTMAP_RESOLUTION_MINUS_ONE] = tile5.heightmap[0, 0];
							}
						}
					}
				}
			}
			for (int num4 = landscapeBounds.min.x; num4 <= landscapeBounds.max.x; num4++)
			{
				for (int num5 = landscapeBounds.min.y; num5 <= landscapeBounds.max.y; num5++)
				{
					LandscapeTile tile6 = Landscape.getTile(new LandscapeCoord(num4, num5));
					if (tile6 != null)
					{
						tile6.SetHeightsDelayLOD();
					}
				}
			}
			LevelHierarchy.MarkDirty();
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000FE18 File Offset: 0x0000E018
		public static void writeSplatmap(Bounds worldBounds, Landscape.LandscapeWriteSplatmapHandler callback)
		{
			if (callback == null)
			{
				return;
			}
			LandscapeBounds landscapeBounds = new LandscapeBounds(worldBounds);
			for (int i = landscapeBounds.min.x; i <= landscapeBounds.max.x; i++)
			{
				for (int j = landscapeBounds.min.y; j <= landscapeBounds.max.y; j++)
				{
					LandscapeCoord landscapeCoord = new LandscapeCoord(i, j);
					LandscapeTile tile = Landscape.getTile(landscapeCoord);
					if (tile != null)
					{
						if (!Landscape.splatmapTransactions.ContainsKey(landscapeCoord))
						{
							LandscapeSplatmapTransaction landscapeSplatmapTransaction = new LandscapeSplatmapTransaction(tile);
							DevkitTransactionManager.recordTransaction(landscapeSplatmapTransaction);
							Landscape.splatmapTransactions.Add(landscapeCoord, landscapeSplatmapTransaction);
						}
						SplatmapBounds splatmapBounds = new SplatmapBounds(landscapeCoord, worldBounds);
						for (int k = splatmapBounds.min.x; k <= splatmapBounds.max.x; k++)
						{
							for (int l = splatmapBounds.min.y; l <= splatmapBounds.max.y; l++)
							{
								SplatmapCoord splatmapCoord = new SplatmapCoord(k, l);
								for (int m = 0; m < Landscape.SPLATMAP_LAYERS; m++)
								{
									Landscape.SPLATMAP_LAYER_BUFFER[m] = tile.splatmap[k, l, m];
								}
								Vector3 worldPosition = Landscape.getWorldPosition(landscapeCoord, splatmapCoord);
								callback(landscapeCoord, splatmapCoord, worldPosition, Landscape.SPLATMAP_LAYER_BUFFER);
								for (int n = 0; n < Landscape.SPLATMAP_LAYERS; n++)
								{
									tile.splatmap[k, l, n] = Mathf.Clamp01(Landscape.SPLATMAP_LAYER_BUFFER[n]);
								}
							}
						}
						tile.data.SetAlphamaps(0, 0, tile.splatmap);
					}
				}
			}
			LevelHierarchy.MarkDirty();
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000FFC0 File Offset: 0x0000E1C0
		public static void writeHoles(Bounds worldBounds, Landscape.LandscapeWriteHolesHandler callback)
		{
			if (callback == null)
			{
				return;
			}
			LandscapeBounds landscapeBounds = new LandscapeBounds(worldBounds);
			for (int i = landscapeBounds.min.x; i <= landscapeBounds.max.x; i++)
			{
				for (int j = landscapeBounds.min.y; j <= landscapeBounds.max.y; j++)
				{
					LandscapeCoord landscapeCoord = new LandscapeCoord(i, j);
					LandscapeTile tile = Landscape.getTile(landscapeCoord);
					if (tile != null)
					{
						if (!Landscape.holeTransactions.ContainsKey(landscapeCoord))
						{
							LandscapeHoleTransaction landscapeHoleTransaction = new LandscapeHoleTransaction(tile);
							DevkitTransactionManager.recordTransaction(landscapeHoleTransaction);
							Landscape.holeTransactions.Add(landscapeCoord, landscapeHoleTransaction);
						}
						SplatmapBounds splatmapBounds = new SplatmapBounds(landscapeCoord, worldBounds);
						for (int k = splatmapBounds.min.x; k <= splatmapBounds.max.x; k++)
						{
							for (int l = splatmapBounds.min.y; l <= splatmapBounds.max.y; l++)
							{
								SplatmapCoord splatmapCoord = new SplatmapCoord(k, l);
								Vector3 worldPosition = Landscape.getWorldPosition(landscapeCoord, splatmapCoord);
								bool flag = tile.holes[k, l];
								bool flag2 = callback(worldPosition, flag);
								tile.holes[k, l] = flag2;
								tile.hasAnyHolesData |= (flag2 != flag);
							}
						}
						tile.data.SetHoles(0, 0, tile.holes);
					}
				}
			}
			LevelHierarchy.MarkDirty();
		}

		/// <summary>
		/// Appends heightmap vertices to points list.
		/// </summary>
		// Token: 0x060003F4 RID: 1012 RVA: 0x00010138 File Offset: 0x0000E338
		public static void getHeightmapVertices(Bounds worldBounds, Landscape.LandscapeGetHeightmapVerticesHandler callback)
		{
			if (callback == null)
			{
				return;
			}
			LandscapeBounds landscapeBounds = new LandscapeBounds(worldBounds);
			for (int i = landscapeBounds.min.x; i <= landscapeBounds.max.x; i++)
			{
				for (int j = landscapeBounds.min.y; j <= landscapeBounds.max.y; j++)
				{
					LandscapeCoord landscapeCoord = new LandscapeCoord(i, j);
					LandscapeTile tile = Landscape.getTile(landscapeCoord);
					if (tile != null)
					{
						HeightmapBounds heightmapBounds = new HeightmapBounds(landscapeCoord, worldBounds);
						for (int k = heightmapBounds.min.x; k <= heightmapBounds.max.x; k++)
						{
							for (int l = heightmapBounds.min.y; l <= heightmapBounds.max.y; l++)
							{
								HeightmapCoord heightmapCoord = new HeightmapCoord(k, l);
								float height = tile.heightmap[k, l];
								Vector3 worldPosition = Landscape.getWorldPosition(landscapeCoord, heightmapCoord, height);
								callback(landscapeCoord, heightmapCoord, worldPosition);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Appends heightmap vertices to points list.
		/// </summary>
		// Token: 0x060003F5 RID: 1013 RVA: 0x00010240 File Offset: 0x0000E440
		public static void getSplatmapVertices(Bounds worldBounds, Landscape.LandscapeGetSplatmapVerticesHandler callback)
		{
			if (callback == null)
			{
				return;
			}
			LandscapeBounds landscapeBounds = new LandscapeBounds(worldBounds);
			for (int i = landscapeBounds.min.x; i <= landscapeBounds.max.x; i++)
			{
				for (int j = landscapeBounds.min.y; j <= landscapeBounds.max.y; j++)
				{
					LandscapeCoord landscapeCoord = new LandscapeCoord(i, j);
					if (Landscape.getTile(landscapeCoord) != null)
					{
						SplatmapBounds splatmapBounds = new SplatmapBounds(landscapeCoord, worldBounds);
						for (int k = splatmapBounds.min.x; k <= splatmapBounds.max.x; k++)
						{
							for (int l = splatmapBounds.min.y; l <= splatmapBounds.max.y; l++)
							{
								SplatmapCoord splatmapCoord = new SplatmapCoord(k, l);
								Vector3 worldPosition = Landscape.getWorldPosition(landscapeCoord, splatmapCoord);
								callback(landscapeCoord, splatmapCoord, worldPosition);
							}
						}
					}
				}
			}
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0001032C File Offset: 0x0000E52C
		public static void applyLOD()
		{
			foreach (KeyValuePair<LandscapeCoord, LandscapeTile> keyValuePair in Landscape.tiles)
			{
				keyValuePair.Value.SyncHeightmap();
			}
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00010384 File Offset: 0x0000E584
		public static void SyncHoles()
		{
			foreach (KeyValuePair<LandscapeCoord, LandscapeTile> keyValuePair in Landscape.tiles)
			{
				keyValuePair.Value.data.SyncTexture(TerrainData.HolesTextureName);
			}
		}

		/// <summary>
		/// Call this after you're done adding new tiles.
		/// </summary>
		// Token: 0x060003F8 RID: 1016 RVA: 0x000103E8 File Offset: 0x0000E5E8
		public static void linkNeighbors()
		{
		}

		/// <summary>
		/// Call this to sync a new tile up with nearby tiles.
		/// </summary>
		// Token: 0x060003F9 RID: 1017 RVA: 0x000103F8 File Offset: 0x0000E5F8
		public static void reconcileNeighbors(LandscapeTile tile)
		{
			LandscapeTile tile2 = Landscape.getTile(new LandscapeCoord(tile.coord.x - 1, tile.coord.y));
			if (tile2 != null)
			{
				for (int i = 0; i < Landscape.HEIGHTMAP_RESOLUTION; i++)
				{
					tile.heightmap[i, 0] = tile2.heightmap[i, Landscape.HEIGHTMAP_RESOLUTION - 1];
				}
			}
			LandscapeTile tile3 = Landscape.getTile(new LandscapeCoord(tile.coord.x, tile.coord.y - 1));
			if (tile3 != null)
			{
				for (int j = 0; j < Landscape.HEIGHTMAP_RESOLUTION; j++)
				{
					tile.heightmap[0, j] = tile3.heightmap[Landscape.HEIGHTMAP_RESOLUTION - 1, j];
				}
			}
			LandscapeTile tile4 = Landscape.getTile(new LandscapeCoord(tile.coord.x + 1, tile.coord.y));
			if (tile4 != null)
			{
				for (int k = 0; k < Landscape.HEIGHTMAP_RESOLUTION; k++)
				{
					tile.heightmap[k, Landscape.HEIGHTMAP_RESOLUTION - 1] = tile4.heightmap[k, 0];
				}
			}
			LandscapeTile tile5 = Landscape.getTile(new LandscapeCoord(tile.coord.x, tile.coord.y + 1));
			if (tile5 != null)
			{
				for (int l = 0; l < Landscape.HEIGHTMAP_RESOLUTION; l++)
				{
					tile.heightmap[Landscape.HEIGHTMAP_RESOLUTION - 1, l] = tile5.heightmap[0, l];
				}
			}
			tile.SetHeightsDelayLOD();
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0001057C File Offset: 0x0000E77C
		public static LandscapeTile addTile(LandscapeCoord coord)
		{
			if (Landscape.instance == null)
			{
				UnturnedLog.info("Adding default landscape to level");
				LevelHierarchy.AssignInstanceIdAndMarkDirty(new GameObject().AddComponent<Landscape>());
			}
			if (Landscape.tiles.ContainsKey(coord))
			{
				return null;
			}
			LandscapeTile landscapeTile = new LandscapeTile(coord);
			landscapeTile.enable();
			landscapeTile.applyGraphicsSettings();
			Landscape.tiles.Add(coord, landscapeTile);
			return landscapeTile;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x000105E0 File Offset: 0x0000E7E0
		protected static void clearTiles()
		{
			foreach (KeyValuePair<LandscapeCoord, LandscapeTile> keyValuePair in Landscape.tiles)
			{
				keyValuePair.Value.disable();
			}
			Landscape.tiles.Clear();
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x00010644 File Offset: 0x0000E844
		public static void CopyLayersToAllTiles(LandscapeTile source)
		{
			foreach (KeyValuePair<LandscapeCoord, LandscapeTile> keyValuePair in Landscape.tiles)
			{
				LandscapeTile value = keyValuePair.Value;
				if (value != source)
				{
					for (int i = 0; i < Landscape.SPLATMAP_LAYERS; i++)
					{
						value.materials[i] = source.materials[i];
					}
					value.updatePrototypes();
				}
			}
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x000106CC File Offset: 0x0000E8CC
		public static LandscapeTile getOrAddTile(Vector3 worldPosition)
		{
			return Landscape.getOrAddTile(new LandscapeCoord(worldPosition));
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x000106D9 File Offset: 0x0000E8D9
		public static LandscapeTile getTile(Vector3 worldPosition)
		{
			return Landscape.getTile(new LandscapeCoord(worldPosition));
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x000106E8 File Offset: 0x0000E8E8
		public static LandscapeTile getOrAddTile(LandscapeCoord coord)
		{
			LandscapeTile result;
			if (!Landscape.tiles.TryGetValue(coord, ref result))
			{
				result = Landscape.addTile(coord);
			}
			return result;
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0001070C File Offset: 0x0000E90C
		public static LandscapeTile getTile(LandscapeCoord coord)
		{
			LandscapeTile result;
			Landscape.tiles.TryGetValue(coord, ref result);
			return result;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00010728 File Offset: 0x0000E928
		public static bool removeTile(LandscapeCoord coord)
		{
			LandscapeTile landscapeTile;
			if (!Landscape.tiles.TryGetValue(coord, ref landscapeTile))
			{
				return false;
			}
			landscapeTile.disable();
			Object.Destroy(landscapeTile.gameObject);
			Landscape.tiles.Remove(coord);
			return true;
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00010764 File Offset: 0x0000E964
		public override void read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			int num = reader.readArrayLength("Tiles");
			if (Landscape.instance != this)
			{
				UnturnedLog.warn("Level contains multiple Landscapes. Ignoring {0} tile(s) with instance ID: {1}", new object[]
				{
					num,
					this.instanceID
				});
				return;
			}
			UnturnedLog.info("Loading {0} landscape tiles", new object[]
			{
				num
			});
			for (int i = 0; i < num; i++)
			{
				reader.readArrayIndex(i);
				LandscapeTile landscapeTile = new LandscapeTile(LandscapeCoord.ZERO);
				landscapeTile.enable();
				landscapeTile.applyGraphicsSettings();
				landscapeTile.read(reader);
				if (Landscape.tiles.ContainsKey(landscapeTile.coord))
				{
					UnturnedLog.error("Duplicate landscape coord read: " + landscapeTile.coord.ToString());
				}
				else
				{
					Landscape.tiles.Add(landscapeTile.coord, landscapeTile);
				}
			}
			Landscape.linkNeighbors();
			Landscape.applyLOD();
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00010858 File Offset: 0x0000EA58
		public override void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			writer.beginArray("Tiles");
			foreach (KeyValuePair<LandscapeCoord, LandscapeTile> keyValuePair in Landscape.tiles)
			{
				LandscapeTile value = keyValuePair.Value;
				writer.writeValue<LandscapeTile>(value);
			}
			writer.endArray();
			writer.endObject();
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x000108D0 File Offset: 0x0000EAD0
		protected void triggerLandscapeLoaded()
		{
			LandscapeLoadedHandler landscapeLoadedHandler = Landscape.loaded;
			if (landscapeLoadedHandler == null)
			{
				return;
			}
			landscapeLoadedHandler();
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x000108E4 File Offset: 0x0000EAE4
		protected void handleGraphicsSettingsApplied()
		{
			foreach (KeyValuePair<LandscapeCoord, LandscapeTile> keyValuePair in Landscape.tiles)
			{
				keyValuePair.Value.applyGraphicsSettings();
			}
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0001093C File Offset: 0x0000EB3C
		protected void handlePlanarReflectionPreRender()
		{
			foreach (KeyValuePair<LandscapeCoord, LandscapeTile> keyValuePair in Landscape.tiles)
			{
				keyValuePair.Value.terrain.basemapDistance = 0f;
			}
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x000109A0 File Offset: 0x0000EBA0
		protected void handlePlanarReflectionPostRender()
		{
			float terrainBasemapDistance = GraphicsSettings.terrainBasemapDistance;
			foreach (KeyValuePair<LandscapeCoord, LandscapeTile> keyValuePair in Landscape.tiles)
			{
				keyValuePair.Value.terrain.basemapDistance = terrainBasemapDistance;
			}
		}

		/// <summary>
		/// Capturing ortho view of map, so we raise the terrain to max quality.
		/// </summary>
		// Token: 0x06000408 RID: 1032 RVA: 0x00010A04 File Offset: 0x0000EC04
		protected void onSatellitePreCapture()
		{
			foreach (KeyValuePair<LandscapeCoord, LandscapeTile> keyValuePair in Landscape.tiles)
			{
				LandscapeTile value = keyValuePair.Value;
				value.terrain.basemapDistance = 8192f;
				value.terrain.heightmapPixelError = 1f;
			}
		}

		/// <summary>
		/// Finished capturing ortho view of map, so we restore the terrain to preferred quality.
		/// </summary>
		// Token: 0x06000409 RID: 1033 RVA: 0x00010A78 File Offset: 0x0000EC78
		protected void onSatellitePostCapture()
		{
			float terrainBasemapDistance = GraphicsSettings.terrainBasemapDistance;
			float terrainHeightmapPixelError = GraphicsSettings.terrainHeightmapPixelError;
			foreach (KeyValuePair<LandscapeCoord, LandscapeTile> keyValuePair in Landscape.tiles)
			{
				LandscapeTile value = keyValuePair.Value;
				value.terrain.basemapDistance = terrainBasemapDistance;
				value.terrain.heightmapPixelError = terrainHeightmapPixelError;
			}
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00010AF0 File Offset: 0x0000ECF0
		protected void OnEnable()
		{
			LevelHierarchy.addItem(this);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00010AF8 File Offset: 0x0000ECF8
		protected void OnDisable()
		{
			LevelHierarchy.removeItem(this);
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00010B00 File Offset: 0x0000ED00
		protected void Awake()
		{
			base.name = "Landscape";
			base.gameObject.layer = 20;
			if (Landscape.instance == null)
			{
				Landscape.instance = this;
				Landscape.clearTiles();
				Landscape._disableHoleColliders = false;
				Landscape.HighlightHoles = false;
				if (Level.isEditor)
				{
					LandscapeHeightmapCopyPool.warmup(DevkitTransactionManager.historyLength);
					LandscapeSplatmapCopyPool.warmup(DevkitTransactionManager.historyLength);
					LandscapeHoleCopyPool.warmup(DevkitTransactionManager.historyLength);
				}
				GraphicsSettings.graphicsSettingsApplied += this.handleGraphicsSettingsApplied;
				PlanarReflection.preRender += this.handlePlanarReflectionPreRender;
				PlanarReflection.postRender += this.handlePlanarReflectionPostRender;
				Level.bindSatelliteCaptureInEditor(new Level.SatelliteCaptureDelegate(this.onSatellitePreCapture), new Level.SatelliteCaptureDelegate(this.onSatellitePostCapture));
			}
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00010BC1 File Offset: 0x0000EDC1
		protected void Start()
		{
			if (Landscape.instance == this && this.shouldTriggerLandscapeLoaded)
			{
				this.triggerLandscapeLoaded();
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00010BE0 File Offset: 0x0000EDE0
		protected void OnDestroy()
		{
			if (Landscape.instance == this)
			{
				GraphicsSettings.graphicsSettingsApplied -= this.handleGraphicsSettingsApplied;
				PlanarReflection.preRender -= this.handlePlanarReflectionPreRender;
				PlanarReflection.postRender -= this.handlePlanarReflectionPostRender;
				Level.unbindSatelliteCapture(new Level.SatelliteCaptureDelegate(this.onSatellitePreCapture), new Level.SatelliteCaptureDelegate(this.onSatellitePostCapture));
				Landscape.instance = null;
				Landscape.clearTiles();
				Landscape._disableHoleColliders = false;
				Landscape.HighlightHoles = false;
				LandscapeHeightmapCopyPool.empty();
				LandscapeSplatmapCopyPool.empty();
				LandscapeHoleCopyPool.empty();
			}
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00010C70 File Offset: 0x0000EE70
		internal IEnumerator AutoConvertLegacyTerrain()
		{
			this.shouldTriggerLandscapeLoaded = false;
			int num = (int)Level.size / Landscape.TILE_SIZE_INT;
			int halfTiles = num / 2 + 1;
			int num2;
			for (int tile_x = -halfTiles; tile_x < halfTiles; tile_x = num2 + 1)
			{
				for (int tile_y = -halfTiles; tile_y < halfTiles; tile_y = num2 + 1)
				{
					LandscapeCoord coord = new LandscapeCoord(tile_x, tile_y);
					LandscapeTile tile = Landscape.getOrAddTile(coord);
					UnturnedLog.info("Auto convert heightmap {0}", new object[]
					{
						coord
					});
					tile.convertLegacyHeightmap();
					yield return null;
					if (LevelGround.doesLegacyDataIncludeSplatmapWeights)
					{
						UnturnedLog.info("Auto convert splatmap {0}", new object[]
						{
							coord
						});
						tile.convertLegacySplatmap();
						yield return null;
						for (int i = 0; i < Landscape.SPLATMAP_LAYERS; i++)
						{
							tile.materials[i] = LevelGround.legacyMaterialGuids[i];
						}
					}
					tile.updatePrototypes();
					yield return null;
					tile = null;
					num2 = tile_y;
				}
				num2 = tile_x;
			}
			FoliageSystem.CreateInLevelIfMissing();
			this.triggerLandscapeLoaded();
			yield break;
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00010C7F File Offset: 0x0000EE7F
		private IEnumerator convertLegacyTerrainImpl(InspectableList<AssetReference<LandscapeMaterialAsset>> materials)
		{
			yield return null;
			int size = (int)Level.size;
			int tiles = size / Landscape.TILE_SIZE_INT;
			int num;
			for (int tile_x = -tiles; tile_x < tiles; tile_x = num + 1)
			{
				for (int tile_y = -tiles; tile_y < tiles; tile_y = num + 1)
				{
					LandscapeCoord coord = new LandscapeCoord(tile_x, tile_y);
					LandscapeTile tile = Landscape.getOrAddTile(coord);
					UnturnedLog.info("Convert heightmap {0}", new object[]
					{
						coord
					});
					tile.convertLegacyHeightmap();
					yield return null;
					UnturnedLog.info("Convert splatmap {0}", new object[]
					{
						coord
					});
					tile.convertLegacySplatmap();
					yield return null;
					for (int i = 0; i < Landscape.SPLATMAP_LAYERS; i++)
					{
						tile.materials[i] = materials[i];
					}
					UnturnedLog.info("Convert prototypes {0}", new object[]
					{
						coord
					});
					tile.updatePrototypes();
					yield return null;
					tile = null;
					num = tile_y;
				}
				num = tile_x;
			}
			new GameObject
			{
				transform = 
				{
					position = Vector3.zero,
					rotation = Quaternion.identity,
					localScale = new Vector3((float)size, Landscape.TILE_HEIGHT, (float)size)
				}
			}.AddComponent<FoliageVolume>().mode = FoliageVolume.EFoliageVolumeMode.ADDITIVE;
			yield break;
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00010C8E File Offset: 0x0000EE8E
		public void convertLegacyTerrain(InspectableList<AssetReference<LandscapeMaterialAsset>> materials)
		{
			if (this.hasConverted)
			{
				return;
			}
			this.hasConverted = true;
			base.StartCoroutine(this.convertLegacyTerrainImpl(materials));
		}

		// Token: 0x040001A0 RID: 416
		public static readonly float TILE_SIZE = 1024f;

		// Token: 0x040001A1 RID: 417
		public static readonly int TILE_SIZE_INT = 1024;

		// Token: 0x040001A2 RID: 418
		public static readonly float TILE_HEIGHT = 2048f;

		// Token: 0x040001A3 RID: 419
		public static readonly int TILE_HEIGHT_INT = 2048;

		// Token: 0x040001A4 RID: 420
		public static readonly int HEIGHTMAP_RESOLUTION = 257;

		// Token: 0x040001A5 RID: 421
		public static readonly int HEIGHTMAP_RESOLUTION_MINUS_ONE = 256;

		// Token: 0x040001A6 RID: 422
		public static readonly float HEIGHTMAP_WORLD_UNIT = 4f;

		// Token: 0x040001A7 RID: 423
		public static readonly float HALF_HEIGHTMAP_WORLD_UNIT = 2f;

		// Token: 0x040001A8 RID: 424
		public static readonly int SPLATMAP_RESOLUTION = 256;

		// Token: 0x040001A9 RID: 425
		public static readonly int SPLATMAP_RESOLUTION_MINUS_ONE = 255;

		// Token: 0x040001AA RID: 426
		public static readonly float SPLATMAP_WORLD_UNIT = 4f;

		// Token: 0x040001AB RID: 427
		public static readonly float HALF_SPLATMAP_WORLD_UNIT = 2f;

		// Token: 0x040001AC RID: 428
		public static readonly int BASEMAP_RESOLUTION = 128;

		// Token: 0x040001AD RID: 429
		public static readonly int SPLATMAP_COUNT = 2;

		// Token: 0x040001AE RID: 430
		public static readonly int SPLATMAP_CHANNELS = 4;

		// Token: 0x040001AF RID: 431
		public static readonly int SPLATMAP_LAYERS = Landscape.SPLATMAP_COUNT * Landscape.SPLATMAP_CHANNELS;

		// Token: 0x040001B0 RID: 432
		public const int HOLES_RESOLUTION = 256;

		// Token: 0x040001B1 RID: 433
		public const float HALF_DIAGONAL_SPLATMAP_WORLD_UNIT = 2.828427f;

		// Token: 0x040001B2 RID: 434
		protected static readonly float[] SPLATMAP_LAYER_BUFFER = new float[Landscape.SPLATMAP_LAYERS];

		// Token: 0x040001B5 RID: 437
		protected static Dictionary<LandscapeCoord, LandscapeTile> tiles = new Dictionary<LandscapeCoord, LandscapeTile>();

		// Token: 0x040001B6 RID: 438
		protected static Dictionary<LandscapeCoord, LandscapeHeightmapTransaction> heightmapTransactions = new Dictionary<LandscapeCoord, LandscapeHeightmapTransaction>();

		// Token: 0x040001B7 RID: 439
		protected static Dictionary<LandscapeCoord, LandscapeSplatmapTransaction> splatmapTransactions = new Dictionary<LandscapeCoord, LandscapeSplatmapTransaction>();

		// Token: 0x040001B8 RID: 440
		protected static Dictionary<LandscapeCoord, LandscapeHoleTransaction> holeTransactions = new Dictionary<LandscapeCoord, LandscapeHoleTransaction>();

		// Token: 0x040001B9 RID: 441
		private static bool _disableHoleColliders;

		// Token: 0x040001BA RID: 442
		private static bool _highlightHoles;

		// Token: 0x040001BB RID: 443
		private bool shouldTriggerLandscapeLoaded = true;

		// Token: 0x040001BC RID: 444
		private bool hasConverted;

		// Token: 0x02000859 RID: 2137
		// (Invoke) Token: 0x060047D9 RID: 18393
		public delegate void LandscapeReadHeightmapHandler(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, Vector3 worldPosition, float currentHeight);

		// Token: 0x0200085A RID: 2138
		// (Invoke) Token: 0x060047DD RID: 18397
		public delegate void LandscapeReadSplatmapHandler(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord, Vector3 worldPosition, float[] currentWeights);

		// Token: 0x0200085B RID: 2139
		// (Invoke) Token: 0x060047E1 RID: 18401
		public delegate float LandscapeWriteHeightmapHandler(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, Vector3 worldPosition, float currentHeight);

		// Token: 0x0200085C RID: 2140
		// (Invoke) Token: 0x060047E5 RID: 18405
		public delegate void LandscapeWriteSplatmapHandler(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord, Vector3 worldPosition, float[] currentWeights);

		// Token: 0x0200085D RID: 2141
		// (Invoke) Token: 0x060047E9 RID: 18409
		public delegate bool LandscapeWriteHolesHandler(Vector3 worldPosition, bool currentlyVisible);

		// Token: 0x0200085E RID: 2142
		// (Invoke) Token: 0x060047ED RID: 18413
		public delegate void LandscapeGetHeightmapVerticesHandler(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, Vector3 worldPosition);

		// Token: 0x0200085F RID: 2143
		// (Invoke) Token: 0x060047F1 RID: 18417
		public delegate void LandscapeGetSplatmapVerticesHandler(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord, Vector3 worldPosition);
	}
}
