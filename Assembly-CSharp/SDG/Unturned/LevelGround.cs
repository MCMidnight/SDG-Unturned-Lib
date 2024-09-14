using System;
using System.Collections.Generic;
using System.IO;
using SDG.Framework.Foliage;
using SDG.Framework.Landscapes;
using UnityEngine;
using UnityEngine.Rendering;

namespace SDG.Unturned
{
	// Token: 0x020004DE RID: 1246
	public class LevelGround : MonoBehaviour
	{
		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06002641 RID: 9793 RVA: 0x0009AAAD File Offset: 0x00098CAD
		// (set) Token: 0x06002642 RID: 9794 RVA: 0x0009AAB4 File Offset: 0x00098CB4
		public static float triplanarPrimarySize
		{
			get
			{
				return LevelGround._triplanarPrimarySize;
			}
			set
			{
				LevelGround._triplanarPrimarySize = value;
				Shader.SetGlobalFloat(LevelGround._Triplanar_Primary_Size, LevelGround.triplanarPrimarySize);
				UnturnedLog.info("Set triplanar_primary_size to: " + LevelGround.triplanarPrimarySize.ToString());
			}
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06002643 RID: 9795 RVA: 0x0009AAF2 File Offset: 0x00098CF2
		// (set) Token: 0x06002644 RID: 9796 RVA: 0x0009AAFC File Offset: 0x00098CFC
		public static float triplanarPrimaryWeight
		{
			get
			{
				return LevelGround._triplanarPrimaryWeight;
			}
			set
			{
				LevelGround._triplanarPrimaryWeight = value;
				Shader.SetGlobalFloat(LevelGround._Triplanar_Primary_Weight, LevelGround.triplanarPrimaryWeight);
				UnturnedLog.info("Set triplanar_primary_weight to: " + LevelGround.triplanarPrimaryWeight.ToString());
			}
		}

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06002645 RID: 9797 RVA: 0x0009AB3A File Offset: 0x00098D3A
		// (set) Token: 0x06002646 RID: 9798 RVA: 0x0009AB44 File Offset: 0x00098D44
		public static float triplanarSecondarySize
		{
			get
			{
				return LevelGround._triplanarSecondarySize;
			}
			set
			{
				LevelGround._triplanarSecondarySize = value;
				Shader.SetGlobalFloat(LevelGround._Triplanar_Secondary_Size, LevelGround.triplanarSecondarySize);
				UnturnedLog.info("Set triplanar_secondary_size to: " + LevelGround.triplanarSecondarySize.ToString());
			}
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06002647 RID: 9799 RVA: 0x0009AB82 File Offset: 0x00098D82
		// (set) Token: 0x06002648 RID: 9800 RVA: 0x0009AB8C File Offset: 0x00098D8C
		public static float triplanarSecondaryWeight
		{
			get
			{
				return LevelGround._triplanarSecondaryWeight;
			}
			set
			{
				LevelGround._triplanarSecondaryWeight = value;
				Shader.SetGlobalFloat(LevelGround._Triplanar_Secondary_Weight, LevelGround.triplanarSecondaryWeight);
				UnturnedLog.info("Set triplanar_secondary_weight to: " + LevelGround.triplanarSecondaryWeight.ToString());
			}
		}

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06002649 RID: 9801 RVA: 0x0009ABCA File Offset: 0x00098DCA
		// (set) Token: 0x0600264A RID: 9802 RVA: 0x0009ABD4 File Offset: 0x00098DD4
		public static float triplanarTertiarySize
		{
			get
			{
				return LevelGround._triplanarTertiarySize;
			}
			set
			{
				LevelGround._triplanarTertiarySize = value;
				Shader.SetGlobalFloat(LevelGround._Triplanar_Tertiary_Size, LevelGround.triplanarTertiarySize);
				UnturnedLog.info("Set triplanar_tertiary_size to: " + LevelGround.triplanarTertiarySize.ToString());
			}
		}

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x0600264B RID: 9803 RVA: 0x0009AC12 File Offset: 0x00098E12
		// (set) Token: 0x0600264C RID: 9804 RVA: 0x0009AC1C File Offset: 0x00098E1C
		public static float triplanarTertiaryWeight
		{
			get
			{
				return LevelGround._triplanarTertiaryWeight;
			}
			set
			{
				LevelGround._triplanarTertiaryWeight = value;
				Shader.SetGlobalFloat(LevelGround._Triplanar_Tertiary_Weight, LevelGround.triplanarTertiaryWeight);
				UnturnedLog.info("Set triplanar_tertiary_weight to: " + LevelGround.triplanarTertiaryWeight.ToString());
			}
		}

		/// <summary>
		/// Hash of Trees.dat, or zeroed if any assets were missing locally.
		/// Should only be used if level is configured to, as many mod maps are typically missing assets.
		/// </summary>
		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x0600264D RID: 9805 RVA: 0x0009AC5A File Offset: 0x00098E5A
		// (set) Token: 0x0600264E RID: 9806 RVA: 0x0009AC61 File Offset: 0x00098E61
		public static byte[] treesHash { get; private set; }

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x0600264F RID: 9807 RVA: 0x0009AC69 File Offset: 0x00098E69
		[Obsolete("Legacy terrain game object only exists for auto-conversion")]
		public static Transform models
		{
			get
			{
				return LevelGround._models;
			}
		}

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06002650 RID: 9808 RVA: 0x0009AC70 File Offset: 0x00098E70
		[Obsolete("Legacy terrain game object only exists for auto-conversion")]
		public static Transform models2
		{
			get
			{
				return LevelGround._models2;
			}
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06002651 RID: 9809 RVA: 0x0009AC77 File Offset: 0x00098E77
		public static List<ResourceSpawnpoint>[,] trees
		{
			get
			{
				return LevelGround._trees;
			}
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06002652 RID: 9810 RVA: 0x0009AC7E File Offset: 0x00098E7E
		public static int total
		{
			get
			{
				return LevelGround._total;
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x06002653 RID: 9811 RVA: 0x0009AC85 File Offset: 0x00098E85
		public static bool[,] regions
		{
			get
			{
				return LevelGround._regions;
			}
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06002654 RID: 9812 RVA: 0x0009AC8C File Offset: 0x00098E8C
		// (set) Token: 0x06002655 RID: 9813 RVA: 0x0009AC93 File Offset: 0x00098E93
		public static bool shouldInstantlyLoad { get; private set; }

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x06002656 RID: 9814 RVA: 0x0009AC9B File Offset: 0x00098E9B
		[Obsolete("Legacy terrain only exists for auto-conversion")]
		public static Terrain terrain
		{
			get
			{
				return LevelGround._terrain;
			}
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x06002657 RID: 9815 RVA: 0x0009ACA2 File Offset: 0x00098EA2
		[Obsolete("Legacy terrain only exists for auto-conversion")]
		public static Terrain terrain2
		{
			get
			{
				return LevelGround._terrain2;
			}
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x06002658 RID: 9816 RVA: 0x0009ACA9 File Offset: 0x00098EA9
		public static TerrainData data
		{
			get
			{
				return LevelGround._data;
			}
		}

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x06002659 RID: 9817 RVA: 0x0009ACB0 File Offset: 0x00098EB0
		public static TerrainData data2
		{
			get
			{
				return LevelGround._data2;
			}
		}

		// Token: 0x0600265A RID: 9818 RVA: 0x0009ACB8 File Offset: 0x00098EB8
		internal static ResourceSpawnpoint FindResourceSpawnpointByTransform(Transform transform)
		{
			if (transform != null)
			{
				transform = transform.root;
			}
			byte b;
			byte b2;
			if (transform != null && Regions.tryGetCoordinate(transform.position, out b, out b2))
			{
				foreach (ResourceSpawnpoint resourceSpawnpoint in LevelGround._trees[(int)b, (int)b2])
				{
					if (resourceSpawnpoint.model == transform)
					{
						return resourceSpawnpoint;
					}
				}
			}
			return null;
		}

		// Token: 0x0600265B RID: 9819 RVA: 0x0009AD4C File Offset: 0x00098F4C
		[Obsolete]
		public static Vector3 checkSafe(Vector3 point)
		{
			UndergroundAllowlist.AdjustPosition(ref point, 0.5f, 1f);
			return point;
		}

		// Token: 0x0600265C RID: 9820 RVA: 0x0009AD61 File Offset: 0x00098F61
		[Obsolete]
		public static int getMaterialIndex(Vector3 point)
		{
			return 0;
		}

		// Token: 0x0600265D RID: 9821 RVA: 0x0009AD64 File Offset: 0x00098F64
		public static float getHeight(Vector3 point)
		{
			float result;
			Landscape.getWorldHeight(point, out result);
			return result;
		}

		// Token: 0x0600265E RID: 9822 RVA: 0x0009AD7C File Offset: 0x00098F7C
		public static float getConversionHeight(Vector3 point)
		{
			if (point.x < (float)(-Level.size / 2) || point.z < (float)(-Level.size / 2) || point.x > (float)(Level.size / 2) || point.z > (float)(Level.size / 2))
			{
				return LevelGround._terrain2.SampleHeight(point);
			}
			return Mathf.Max(LevelGround._terrain.SampleHeight(point), LevelGround._terrain2.SampleHeight(point));
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x0009ADF4 File Offset: 0x00098FF4
		public static float getConversionWeight(Vector3 point, int layer)
		{
			if (point.x < (float)(-Level.size / 2) || point.z < (float)(-Level.size / 2) || point.x > (float)(Level.size / 2) || point.z > (float)(Level.size / 2) || LevelGround._terrain2.SampleHeight(point) > LevelGround._terrain.SampleHeight(point))
			{
				int alphamap2_X = LevelGround.getAlphamap2_X(point);
				if (alphamap2_X < 0 || alphamap2_X >= LevelGround.data2.alphamapWidth)
				{
					return 0f;
				}
				int alphamap2_Y = LevelGround.getAlphamap2_Y(point);
				if (alphamap2_Y < 0 || alphamap2_Y >= LevelGround.data2.alphamapWidth)
				{
					return 0f;
				}
				return LevelGround.alphamap2HQ[alphamap2_Y, alphamap2_X, layer];
			}
			else
			{
				int alphamap_X = LevelGround.getAlphamap_X(point);
				if (alphamap_X < 0 || alphamap_X >= LevelGround.data.alphamapWidth)
				{
					return 0f;
				}
				int alphamap_Y = LevelGround.getAlphamap_Y(point);
				if (alphamap_Y < 0 || alphamap_Y >= LevelGround.data.alphamapWidth)
				{
					return 0f;
				}
				return LevelGround.alphamapHQ[alphamap_Y, alphamap_X, layer];
			}
		}

		// Token: 0x06002660 RID: 9824 RVA: 0x0009AEF0 File Offset: 0x000990F0
		public static Vector3 getNormal(Vector3 point)
		{
			Vector3 result;
			Landscape.getNormal(point, out result);
			return result;
		}

		// Token: 0x06002661 RID: 9825 RVA: 0x0009AF07 File Offset: 0x00099107
		public static int getAlphamap_X(Vector3 point)
		{
			return (int)((point.x - LevelGround._terrain.transform.position.x) / LevelGround.data.size.x * (float)LevelGround.data.alphamapWidth);
		}

		// Token: 0x06002662 RID: 9826 RVA: 0x0009AF41 File Offset: 0x00099141
		public static int getAlphamap_Y(Vector3 point)
		{
			return (int)((point.z - LevelGround._terrain.transform.position.z) / LevelGround.data.size.z * (float)LevelGround.data.alphamapHeight);
		}

		// Token: 0x06002663 RID: 9827 RVA: 0x0009AF7B File Offset: 0x0009917B
		public static int getAlphamap2_X(Vector3 point)
		{
			return (int)((point.x - LevelGround._terrain2.transform.position.x) / LevelGround.data2.size.x * (float)LevelGround.data2.alphamapWidth);
		}

		// Token: 0x06002664 RID: 9828 RVA: 0x0009AFB5 File Offset: 0x000991B5
		public static int getAlphamap2_Y(Vector3 point)
		{
			return (int)((point.z - LevelGround._terrain2.transform.position.z) / LevelGround.data2.size.z * (float)LevelGround.data2.alphamapHeight);
		}

		// Token: 0x06002665 RID: 9829 RVA: 0x0009AFEF File Offset: 0x000991EF
		public static int getHeightmap_X(Vector3 point)
		{
			return (int)((point.x - LevelGround._terrain.transform.position.x) / LevelGround.data.size.x * (float)LevelGround.data.heightmapResolution);
		}

		// Token: 0x06002666 RID: 9830 RVA: 0x0009B029 File Offset: 0x00099229
		public static int getHeightmap_Y(Vector3 point)
		{
			return (int)((point.z - LevelGround._terrain.transform.position.z) / LevelGround.data.size.z * (float)LevelGround.data.heightmapResolution);
		}

		// Token: 0x06002667 RID: 9831 RVA: 0x0009B063 File Offset: 0x00099263
		public static int getHeightmap2_X(Vector3 point)
		{
			return (int)((point.x - LevelGround._terrain2.transform.position.x) / LevelGround.data2.size.x * (float)LevelGround.data2.heightmapResolution);
		}

		// Token: 0x06002668 RID: 9832 RVA: 0x0009B09D File Offset: 0x0009929D
		public static int getHeightmap2_Y(Vector3 point)
		{
			return (int)((point.z - LevelGround._terrain2.transform.position.z) / LevelGround.data2.size.z * (float)LevelGround.data2.heightmapResolution);
		}

		// Token: 0x06002669 RID: 9833 RVA: 0x0009B0D7 File Offset: 0x000992D7
		[Obsolete]
		public static void cutFoliage(Vector3 point, float radius = 6f)
		{
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x0009B0DC File Offset: 0x000992DC
		protected static void handlePreBakeTile(FoliageBakeSettings bakeSettings, FoliageTile foliageTile)
		{
			if (!bakeSettings.bakeResources)
			{
				return;
			}
			byte b;
			byte b2;
			if (!Regions.tryGetCoordinate(foliageTile.worldBounds.center, out b, out b2))
			{
				return;
			}
			for (int i = LevelGround.trees[(int)b, (int)b2].Count - 1; i >= 0; i--)
			{
				ResourceSpawnpoint resourceSpawnpoint = LevelGround.trees[(int)b, (int)b2][i];
				if (resourceSpawnpoint.isGenerated)
				{
					Vector3 min = foliageTile.worldBounds.min;
					if (resourceSpawnpoint.point.x >= min.x && resourceSpawnpoint.point.z >= min.z)
					{
						Vector3 max = foliageTile.worldBounds.max;
						if (resourceSpawnpoint.point.x <= max.x && resourceSpawnpoint.point.z <= max.z)
						{
							resourceSpawnpoint.destroy();
							LevelGround.trees[(int)b, (int)b2].RemoveAt(i);
						}
					}
				}
			}
			LevelGround.regions[(int)b, (int)b2] = false;
		}

		// Token: 0x0600266B RID: 9835 RVA: 0x0009B1EA File Offset: 0x000993EA
		protected static void handlePostBake()
		{
			LevelGround.onRegionUpdated(byte.MaxValue, byte.MaxValue, EditorArea.instance.region_x, EditorArea.instance.region_y);
		}

		// Token: 0x0600266C RID: 9836 RVA: 0x0009B210 File Offset: 0x00099410
		public static void addSpawn(Vector3 point, Guid guid, bool isGenerated = false)
		{
			byte b;
			byte b2;
			if (!Regions.tryGetCoordinate(point, out b, out b2))
			{
				return;
			}
			ResourceSpawnpoint resourceSpawnpoint = new ResourceSpawnpoint(0, guid, point, isGenerated, NetId.INVALID);
			resourceSpawnpoint.enable();
			resourceSpawnpoint.disableSkybox();
			LevelGround.trees[(int)b, (int)b2].Add(resourceSpawnpoint);
			LevelGround._total++;
		}

		// Token: 0x0600266D RID: 9837 RVA: 0x0009B264 File Offset: 0x00099464
		[Obsolete("Replaced by overload which takes GUID rather than legacy ID")]
		public static void addSpawn(Vector3 point, ushort id, bool isGenerated = false)
		{
			ResourceAsset resourceAsset = Assets.find(EAssetType.RESOURCE, id) as ResourceAsset;
			if (resourceAsset != null)
			{
				LevelGround.addSpawn(point, resourceAsset.GUID, isGenerated);
			}
		}

		// Token: 0x0600266E RID: 9838 RVA: 0x0009B28E File Offset: 0x0009948E
		[Obsolete("Replaced by overload which takes ID rather than index.")]
		public static void addSpawn(Vector3 point, byte index, bool isGenerated = false)
		{
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x0009B290 File Offset: 0x00099490
		public static void removeSpawn(Vector3 point, float radius)
		{
			radius *= radius;
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					List<ResourceSpawnpoint> list = new List<ResourceSpawnpoint>();
					for (int i = 0; i < LevelGround.trees[(int)b, (int)b2].Count; i++)
					{
						ResourceSpawnpoint resourceSpawnpoint = LevelGround.trees[(int)b, (int)b2][i];
						if ((resourceSpawnpoint.point - point).sqrMagnitude < radius)
						{
							resourceSpawnpoint.destroy();
						}
						else
						{
							list.Add(resourceSpawnpoint);
						}
					}
					LevelGround._trees[(int)b, (int)b2] = list;
				}
			}
		}

		// Token: 0x06002670 RID: 9840 RVA: 0x0009B338 File Offset: 0x00099538
		protected static void loadSplatPrototypes()
		{
			string path = Level.info.path + "/Terrain/Materials.unity3d";
			if (ReadWrite.fileExists(path, false, false))
			{
				byte[] array = Hash.SHA1(ReadWrite.readBytes(path, false, false));
				UnturnedLog.info("Legacy terrain material hash: " + Hash.ToCodeString(array));
				byte[] hash_ = new byte[]
				{
					79,
					148,
					129,
					14,
					170,
					79,
					47,
					23,
					60,
					241,
					103,
					67,
					234,
					176,
					132,
					142,
					99,
					41,
					88,
					207
				};
				if (Hash.verifyHash(array, hash_))
				{
					UnturnedLog.info("Matched PEI legacy terrain materials hash");
					LevelGround.legacyMaterialGuids[0] = new AssetReference<LandscapeMaterialAsset>("92cb5a3afd534054a64eb320b50c48de");
					LevelGround.legacyMaterialGuids[1] = new AssetReference<LandscapeMaterialAsset>("22b77c4c51514b0fbb66765eedf1a7f4");
					LevelGround.legacyMaterialGuids[2] = new AssetReference<LandscapeMaterialAsset>("3d7717c2bc074401853b2fdacd9db1ba");
					LevelGround.legacyMaterialGuids[3] = new AssetReference<LandscapeMaterialAsset>("9a2de27c10aa41438154105292b2fd4a");
					LevelGround.legacyMaterialGuids[4] = new AssetReference<LandscapeMaterialAsset>("8729d40d361c4947be4188c70dd7100b");
					LevelGround.legacyMaterialGuids[5] = new AssetReference<LandscapeMaterialAsset>("a9f5c606fe0d433ab167fbe8e3273055");
					LevelGround.legacyMaterialGuids[6] = new AssetReference<LandscapeMaterialAsset>("e25f0351181f4ad1a9c0dc31d2fedade");
					LevelGround.legacyMaterialGuids[7] = new AssetReference<LandscapeMaterialAsset>("2e329671e8c9432eae580f7807acc021");
					return;
				}
				byte[] hash_2 = new byte[]
				{
					176,
					124,
					229,
					38,
					61,
					181,
					234,
					222,
					248,
					79,
					43,
					20,
					216,
					9,
					223,
					252,
					102,
					128,
					208,
					3
				};
				if (Hash.verifyHash(array, hash_2))
				{
					UnturnedLog.info("Matched Russia legacy terrain materials hash");
					LevelGround.legacyMaterialGuids[0] = new AssetReference<LandscapeMaterialAsset>("17b88227113041869ba4661b227a0590");
					LevelGround.legacyMaterialGuids[1] = new AssetReference<LandscapeMaterialAsset>("8a2b6f2215d6460f8b6fece2ccd9c208");
					LevelGround.legacyMaterialGuids[2] = new AssetReference<LandscapeMaterialAsset>("79787e2ca948457a9a322179cf580386");
					LevelGround.legacyMaterialGuids[3] = new AssetReference<LandscapeMaterialAsset>("db482f0f23d1414096114aee61195058");
					LevelGround.legacyMaterialGuids[4] = new AssetReference<LandscapeMaterialAsset>("ceb122707edc4f349be0a97d8f05fd09");
					LevelGround.legacyMaterialGuids[5] = new AssetReference<LandscapeMaterialAsset>("b4ffe0d7b8ed4ff2b4c302c489108b02");
					LevelGround.legacyMaterialGuids[6] = new AssetReference<LandscapeMaterialAsset>("8729d40d361c4947be4188c70dd7100b");
					LevelGround.legacyMaterialGuids[7] = new AssetReference<LandscapeMaterialAsset>("684f4b28200d4ceb9c5362d78d2c9619");
					return;
				}
				byte[] hash_3 = new byte[]
				{
					66,
					161,
					124,
					248,
					128,
					110,
					137,
					204,
					192,
					128,
					38,
					81,
					246,
					158,
					24,
					67,
					76,
					246,
					198,
					76
				};
				if (Hash.verifyHash(array, hash_3))
				{
					UnturnedLog.info("Matched Washington legacy terrain materials hash");
					LevelGround.legacyMaterialGuids[0] = new AssetReference<LandscapeMaterialAsset>("e52b20e26b7c47c89aa5a350938f8f42");
					LevelGround.legacyMaterialGuids[1] = new AssetReference<LandscapeMaterialAsset>("e981f9fae3fa43d68a9a0bfa6472a69f");
					LevelGround.legacyMaterialGuids[2] = new AssetReference<LandscapeMaterialAsset>("5020515a0b9a4b1eb610c006d81f806c");
					LevelGround.legacyMaterialGuids[3] = new AssetReference<LandscapeMaterialAsset>("a14df8dd9bb44f1d967a53f43bde54e6");
					LevelGround.legacyMaterialGuids[4] = new AssetReference<LandscapeMaterialAsset>("8729d40d361c4947be4188c70dd7100b");
					LevelGround.legacyMaterialGuids[5] = new AssetReference<LandscapeMaterialAsset>("684f4b28200d4ceb9c5362d78d2c9619");
					LevelGround.legacyMaterialGuids[6] = new AssetReference<LandscapeMaterialAsset>("d691f78202c84951a3a697f310abd115");
					LevelGround.legacyMaterialGuids[7] = new AssetReference<LandscapeMaterialAsset>("50acf0bddd844f93addd0097f7d95d95");
					return;
				}
				byte[] hash_4 = new byte[]
				{
					251,
					186,
					29,
					144,
					240,
					171,
					74,
					86,
					200,
					30,
					241,
					240,
					4,
					191,
					77,
					80,
					77,
					197,
					180,
					206
				};
				if (Hash.verifyHash(array, hash_4))
				{
					UnturnedLog.info("Matched Yukon legacy terrain materials hash");
					LevelGround.legacyMaterialGuids[0] = new AssetReference<LandscapeMaterialAsset>("e52b20e26b7c47c89aa5a350938f8f42");
					LevelGround.legacyMaterialGuids[1] = new AssetReference<LandscapeMaterialAsset>("e981f9fae3fa43d68a9a0bfa6472a69f");
					LevelGround.legacyMaterialGuids[2] = new AssetReference<LandscapeMaterialAsset>("3d7717c2bc074401853b2fdacd9db1ba");
					LevelGround.legacyMaterialGuids[3] = new AssetReference<LandscapeMaterialAsset>("a14df8dd9bb44f1d967a53f43bde54e6");
					LevelGround.legacyMaterialGuids[4] = new AssetReference<LandscapeMaterialAsset>("8729d40d361c4947be4188c70dd7100b");
					LevelGround.legacyMaterialGuids[5] = new AssetReference<LandscapeMaterialAsset>("684f4b28200d4ceb9c5362d78d2c9619");
					LevelGround.legacyMaterialGuids[6] = new AssetReference<LandscapeMaterialAsset>("e25f0351181f4ad1a9c0dc31d2fedade");
					LevelGround.legacyMaterialGuids[7] = new AssetReference<LandscapeMaterialAsset>("50acf0bddd844f93addd0097f7d95d95");
					return;
				}
				byte[] hash_5 = new byte[]
				{
					96,
					162,
					240,
					106,
					199,
					227,
					25,
					76,
					211,
					1,
					18,
					104,
					64,
					34,
					127,
					188,
					128,
					134,
					1,
					11
				};
				if (Hash.verifyHash(array, hash_5))
				{
					UnturnedLog.info("Matched Greece legacy terrain materials hash");
					LevelGround.legacyMaterialGuids[0] = new AssetReference<LandscapeMaterialAsset>("cf33a7a8fd52461bb523d84234b3a232");
					LevelGround.legacyMaterialGuids[1] = new AssetReference<LandscapeMaterialAsset>("1a917f0fbc0f48d2a4dfda5e15a623df");
					LevelGround.legacyMaterialGuids[2] = new AssetReference<LandscapeMaterialAsset>("76c32cee254f4aeda910d3d8d9788a46");
					LevelGround.legacyMaterialGuids[3] = new AssetReference<LandscapeMaterialAsset>("98c2ae7c2aad48148c9daeb6fab4aa2a");
					LevelGround.legacyMaterialGuids[4] = new AssetReference<LandscapeMaterialAsset>("c743d33c42f54753a529886997626040");
					LevelGround.legacyMaterialGuids[5] = new AssetReference<LandscapeMaterialAsset>("e476cd429bdb41fcafa1df84663e0a47");
					LevelGround.legacyMaterialGuids[6] = new AssetReference<LandscapeMaterialAsset>("30fe7a6c14ee4064865054b82cd71d13");
					LevelGround.legacyMaterialGuids[7] = new AssetReference<LandscapeMaterialAsset>("1b30a651c2ff4c90b8c62d6d9212c146");
					return;
				}
				UnturnedLog.info("Unable to match legacy terrain materials hash, using names instead");
				try
				{
					Bundle bundle = Bundles.getBundle(path, false);
					Texture2D[] array2 = bundle.loadAll<Texture2D>();
					int num = 0;
					Texture2D[] array3 = array2;
					for (int i = 0; i < array3.Length; i++)
					{
						string name = array3[i].name;
						if (name.IndexOf("_Mask") == -1)
						{
							if (name.IndexOf("Farm", 3) != -1)
							{
								LevelGround.legacyMaterialGuids[num] = new AssetReference<LandscapeMaterialAsset>("22b77c4c51514b0fbb66765eedf1a7f4");
							}
							else if (name.IndexOf("Road", 3) != -1)
							{
								LevelGround.legacyMaterialGuids[num] = new AssetReference<LandscapeMaterialAsset>("8729d40d361c4947be4188c70dd7100b");
							}
							else if (name.IndexOf("Grass", 3) != -1)
							{
								LevelGround.legacyMaterialGuids[num] = new AssetReference<LandscapeMaterialAsset>("3d7717c2bc074401853b2fdacd9db1ba");
							}
							else if (name.IndexOf("Gravel", 3) != -1)
							{
								LevelGround.legacyMaterialGuids[num] = new AssetReference<LandscapeMaterialAsset>("a14df8dd9bb44f1d967a53f43bde54e6");
							}
							else if (name.IndexOf("Sand", 3) != -1)
							{
								LevelGround.legacyMaterialGuids[num] = new AssetReference<LandscapeMaterialAsset>("684f4b28200d4ceb9c5362d78d2c9619");
							}
							else if (name.IndexOf("Snow", 3) != -1)
							{
								LevelGround.legacyMaterialGuids[num] = new AssetReference<LandscapeMaterialAsset>("e25f0351181f4ad1a9c0dc31d2fedade");
							}
							else if (name.IndexOf("Stone", 3) != -1)
							{
								LevelGround.legacyMaterialGuids[num] = new AssetReference<LandscapeMaterialAsset>("8a2b6f2215d6460f8b6fece2ccd9c208");
							}
							else if (name.IndexOf("Dirt", 3) != -1)
							{
								LevelGround.legacyMaterialGuids[num] = new AssetReference<LandscapeMaterialAsset>("e52b20e26b7c47c89aa5a350938f8f42");
							}
							else if (name.IndexOf("Leaves", 3) != -1)
							{
								LevelGround.legacyMaterialGuids[num] = new AssetReference<LandscapeMaterialAsset>("b4ffe0d7b8ed4ff2b4c302c489108b02");
							}
							else if (name.IndexOf("Dead", 3) != -1)
							{
								LevelGround.legacyMaterialGuids[num] = new AssetReference<LandscapeMaterialAsset>("17b88227113041869ba4661b227a0590");
							}
							if (LevelGround.legacyMaterialGuids[num].isNull)
							{
								UnturnedLog.warn(string.Format("Unable to match layer {0} name \"{1}\" with any known materials", num, name));
							}
							else
							{
								string text = "Matched layer {0} name \"{1}\" with \"{2}\"";
								object obj = num;
								object obj2 = name;
								LandscapeMaterialAsset landscapeMaterialAsset = LevelGround.legacyMaterialGuids[num].Find();
								UnturnedLog.info(string.Format(text, obj, obj2, (landscapeMaterialAsset != null) ? landscapeMaterialAsset.name : null));
							}
							num++;
							if (num >= 8)
							{
								break;
							}
						}
					}
					bundle.unload();
				}
				catch (Exception e)
				{
					UnturnedLog.exception(e, "Caught exception loading legacy terrain materials:");
				}
				AssetReference<LandscapeMaterialAsset>[] array4 = new AssetReference<LandscapeMaterialAsset>[]
				{
					new AssetReference<LandscapeMaterialAsset>("64357418ae184a959186d1f592a93761"),
					new AssetReference<LandscapeMaterialAsset>("8ea9b170d93e4f9a8a0e0c61cd4bee6a"),
					new AssetReference<LandscapeMaterialAsset>("e54a3da2c46e4927848fed4cdead560a"),
					new AssetReference<LandscapeMaterialAsset>("713fe6ff00e647408047d5dd39d815c0"),
					new AssetReference<LandscapeMaterialAsset>("00ddd72266914141b39e33227942a7df"),
					new AssetReference<LandscapeMaterialAsset>("498ca625072d443a876b2a4f11896018"),
					new AssetReference<LandscapeMaterialAsset>("9889a0b5aad04ddd8c4c463f3e1b79f6"),
					new AssetReference<LandscapeMaterialAsset>("5fd97d1f946c45a79e3d47b49d0348d8")
				};
				int num2 = 0;
				for (int j = 0; j < 8; j++)
				{
					if (LevelGround.legacyMaterialGuids[j].isNull)
					{
						string text2 = "Defaulting empty layer {0} to \"{1}\"";
						object obj3 = j;
						LandscapeMaterialAsset landscapeMaterialAsset2 = array4[num2].Find();
						UnturnedLog.warn(string.Format(text2, obj3, (landscapeMaterialAsset2 != null) ? landscapeMaterialAsset2.name : null));
						LevelGround.legacyMaterialGuids[j] = array4[num2];
						num2++;
					}
				}
			}
		}

		// Token: 0x06002671 RID: 9841 RVA: 0x0009BB40 File Offset: 0x00099D40
		protected static void loadTrees()
		{
			LevelGround._trees = new List<ResourceSpawnpoint>[(int)Regions.WORLD_SIZE, (int)Regions.WORLD_SIZE];
			LevelGround._total = 0;
			LevelGround._regions = new bool[(int)Regions.WORLD_SIZE, (int)Regions.WORLD_SIZE];
			LevelGround.loads = new int[(int)Regions.WORLD_SIZE, (int)Regions.WORLD_SIZE];
			LevelGround.shouldInstantlyLoad = true;
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					LevelGround.loads[(int)b, (int)b2] = -1;
				}
			}
			for (byte b3 = 0; b3 < Regions.WORLD_SIZE; b3 += 1)
			{
				for (byte b4 = 0; b4 < Regions.WORLD_SIZE; b4 += 1)
				{
					LevelGround.trees[(int)b3, (int)b4] = new List<ResourceSpawnpoint>();
				}
			}
			LevelGround.treesHash = new byte[20];
			if (ReadWrite.fileExists(Level.info.path + "/Terrain/Trees.dat", false, false))
			{
				River river = new River(Level.info.path + "/Terrain/Trees.dat", false);
				byte b5 = river.readByte();
				if (b5 > 3)
				{
					TreeRedirectorMap treeRedirectorMap = null;
					if (Level.shouldUseHolidayRedirects)
					{
						treeRedirectorMap = new TreeRedirectorMap();
					}
					LevelBatching levelBatching = null;
					for (byte b6 = 0; b6 < Regions.WORLD_SIZE; b6 += 1)
					{
						for (byte b7 = 0; b7 < Regions.WORLD_SIZE; b7 += 1)
						{
							ushort num = river.readUInt16();
							for (ushort num2 = 0; num2 < num; num2 += 1)
							{
								if (b5 > 4)
								{
									ushort num3 = river.readUInt16();
									Guid guid;
									if (b5 < 6)
									{
										guid = Guid.Empty;
									}
									else
									{
										guid = river.readGUID();
									}
									Vector3 newPoint = river.readSingleVector3();
									bool newGenerated = river.readBoolean();
									if (num3 != 0 || guid != Guid.Empty)
									{
										if (treeRedirectorMap != null)
										{
											ResourceAsset resourceAsset = treeRedirectorMap.redirect(guid);
											if (resourceAsset == null)
											{
												num3 = 0;
												guid = Guid.Empty;
											}
											else
											{
												num3 = resourceAsset.id;
												guid = resourceAsset.GUID;
											}
										}
										if (num3 != 0 || guid != Guid.Empty)
										{
											NetId treeNetId = LevelNetIdRegistry.GetTreeNetId(b6, b7, num2);
											ResourceSpawnpoint resourceSpawnpoint = new ResourceSpawnpoint(num3, guid, newPoint, newGenerated, treeNetId);
											if (resourceSpawnpoint.asset == null && Assets.shouldLoadAnyAssets)
											{
												UnturnedLog.error("Tree with no asset in region {0}, {1}: {2}", new object[]
												{
													b6,
													b7,
													num3
												});
											}
											LevelGround.trees[(int)b6, (int)b7].Add(resourceSpawnpoint);
											if (levelBatching != null)
											{
												levelBatching.AddResourceSpawnpoint(resourceSpawnpoint);
											}
											LevelGround._total++;
										}
									}
								}
								else
								{
									byte b8 = river.readByte();
									ushort num4 = 3;
									Vector3 newPoint2 = river.readSingleVector3();
									bool newGenerated2 = river.readBoolean();
									NetId treeNetId2 = LevelNetIdRegistry.GetTreeNetId(b6, b7, num2);
									ResourceSpawnpoint resourceSpawnpoint2 = new ResourceSpawnpoint(num4, newPoint2, newGenerated2, treeNetId2);
									if (resourceSpawnpoint2.asset == null && Assets.shouldLoadAnyAssets)
									{
										UnturnedLog.error("Tree with no asset in region {0}, {1}: {2} {3}", new object[]
										{
											b6,
											b7,
											num4,
											b8
										});
									}
									LevelGround.trees[(int)b6, (int)b7].Add(resourceSpawnpoint2);
									LevelGround._total++;
								}
							}
						}
					}
				}
				LevelGround.treesHash = river.getHash();
				river.closeRiver();
			}
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x0009BEA0 File Offset: 0x0009A0A0
		public static void load(ushort size)
		{
			LevelGround.hasLegacyDataForConversion = false;
			LevelGround.doesLegacyDataIncludeSplatmapWeights = false;
			if (!Level.info.configData.Use_Legacy_Ground)
			{
				LevelGround.loadTrees();
				return;
			}
			if (File.Exists(LevelGround.GetConversionMarkerFilePath()))
			{
				UnturnedLog.info("Skipping legacy terrain loading because it has already been converted");
				LevelGround.loadTrees();
				return;
			}
			LevelGround.hasLegacyDataForConversion = true;
			LevelGround.legacyMaterialGuids = new AssetReference<LandscapeMaterialAsset>[8];
			LevelGround._models = new GameObject().transform;
			LevelGround._models.name = "Ground";
			LevelGround._models.parent = Level.level;
			LevelGround._models.tag = "Ground";
			LevelGround._models.gameObject.layer = 20;
			LevelGround._terrain = LevelGround._models.gameObject.AddComponent<Terrain>();
			LevelGround._terrain.drawInstanced = SystemInfo.supportsInstancing;
			LevelGround._terrain.name = "Ground";
			LevelGround._terrain.heightmapPixelError = 200f;
			LevelGround._terrain.transform.position = new Vector3((float)(-size / 2), 0f, (float)(-size / 2));
			LevelGround._terrain.reflectionProbeUsage = ReflectionProbeUsage.Simple;
			LevelGround._terrain.shadowCastingMode = ShadowCastingMode.Off;
			LevelGround._terrain.drawHeightmap = false;
			LevelGround._terrain.drawTreesAndFoliage = false;
			LevelGround._terrain.treeDistance = 0f;
			LevelGround._terrain.treeBillboardDistance = 0f;
			LevelGround._terrain.treeCrossFadeLength = 0f;
			LevelGround._terrain.treeMaximumFullLODCount = 0;
			LevelGround._data = new TerrainData();
			LevelGround.data.name = "Ground";
			LevelGround.data.heightmapResolution = (int)(size / 8);
			LevelGround.data.alphamapResolution = (int)(size / 4);
			LevelGround.data.size = new Vector3((float)size, Level.TERRAIN, (float)size);
			LevelGround.data.wavingGrassTint = Color.white;
			byte b = 0;
			byte b2 = 0;
			if (ReadWrite.fileExists(Level.info.path + "/Terrain/Heights.dat", false, false))
			{
				Block block = ReadWrite.readBlock(Level.info.path + "/Terrain/Heights.dat", false, false, 0);
				b = block.readByte();
				b2 = block.readByte();
			}
			if (ReadWrite.fileExists(Level.info.path + "/Terrain/Heightmap.png", false, false))
			{
				byte[] array = ReadWrite.readBytes(Level.info.path + "/Terrain/Heightmap.png", false, false);
				Texture2D texture2D = new Texture2D(LevelGround.data.heightmapResolution, LevelGround.data.heightmapResolution, TextureFormat.ARGB32, false);
				texture2D.name = "Heightmap_Load";
				texture2D.hideFlags = HideFlags.HideAndDontSave;
				ImageConversion.LoadImage(texture2D, array);
				float[,] array2 = new float[texture2D.width, texture2D.height];
				for (int i = 0; i < texture2D.width; i++)
				{
					for (int j = 0; j < texture2D.height; j++)
					{
						if (b > 0)
						{
							byte[] array3 = new byte[]
							{
								(byte)(texture2D.GetPixel(i, j).r * 255f),
								(byte)(texture2D.GetPixel(i, j).g * 255f),
								(byte)(texture2D.GetPixel(i, j).b * 255f),
								(byte)(texture2D.GetPixel(i, j).a * 255f)
							};
							array2[i, j] = BitConverter.ToSingle(array3, 0);
						}
						else
						{
							array2[i, j] = texture2D.GetPixel(i, j).r;
						}
					}
				}
				LevelGround.data.SetHeights(0, 0, array2);
				Object.DestroyImmediate(texture2D);
			}
			else
			{
				float[,] array4 = new float[LevelGround.data.heightmapResolution, LevelGround.data.heightmapResolution];
				for (int k = 0; k < LevelGround.data.heightmapResolution; k++)
				{
					for (int l = 0; l < LevelGround.data.heightmapResolution; l++)
					{
						array4[k, l] = 0.03f;
					}
				}
				LevelGround.data.SetHeights(0, 0, array4);
			}
			LevelGround.loadSplatPrototypes();
			LevelGround.alphamapHQ = new float[LevelGround.data.alphamapWidth, LevelGround.data.alphamapHeight, (int)(LevelGround.ALPHAMAPS * 4)];
			for (int m = 0; m < (int)LevelGround.ALPHAMAPS; m++)
			{
				bool flag = false;
				if (ReadWrite.fileExists(Level.info.path + "/Terrain/AlphamapHQ_" + m.ToString() + ".png", false, false))
				{
					byte[] array5 = ReadWrite.readBytes(Level.info.path + "/Terrain/AlphamapHQ_" + m.ToString() + ".png", false, false);
					Texture2D texture2D2 = new Texture2D(LevelGround.data.heightmapResolution, LevelGround.data.heightmapResolution, TextureFormat.ARGB32, false);
					texture2D2.name = "AlphamapHQ_Load";
					texture2D2.hideFlags = HideFlags.HideAndDontSave;
					ImageConversion.LoadImage(texture2D2, array5);
					for (int n = 0; n < texture2D2.width; n++)
					{
						for (int num = 0; num < texture2D2.height; num++)
						{
							Color pixel = texture2D2.GetPixel(n, num);
							LevelGround.alphamapHQ[n, num, m * 4] = pixel.r;
							LevelGround.alphamapHQ[n, num, m * 4 + 1] = pixel.g;
							LevelGround.alphamapHQ[n, num, m * 4 + 2] = pixel.b;
							LevelGround.alphamapHQ[n, num, m * 4 + 3] = pixel.a;
						}
					}
					Object.DestroyImmediate(texture2D2);
					flag = true;
					LevelGround.doesLegacyDataIncludeSplatmapWeights = true;
				}
				if (!flag && ReadWrite.fileExists(Level.info.path + "/Terrain/Alphamap_" + m.ToString() + ".png", false, false))
				{
					byte[] array6 = ReadWrite.readBytes(Level.info.path + "/Terrain/Alphamap_" + m.ToString() + ".png", false, false);
					Texture2D texture2D3 = new Texture2D(LevelGround.data.heightmapResolution, LevelGround.data.heightmapResolution, TextureFormat.ARGB32, false);
					texture2D3.name = "Alphamap_Load";
					texture2D3.hideFlags = HideFlags.HideAndDontSave;
					ImageConversion.LoadImage(texture2D3, array6);
					for (int num2 = 0; num2 < texture2D3.width; num2++)
					{
						for (int num3 = 0; num3 < texture2D3.height; num3++)
						{
							Color pixel2 = texture2D3.GetPixel(num2, num3);
							LevelGround.alphamapHQ[num2, num3, m * 4] = pixel2.r;
							LevelGround.alphamapHQ[num2, num3, m * 4 + 1] = pixel2.g;
							LevelGround.alphamapHQ[num2, num3, m * 4 + 2] = pixel2.b;
							LevelGround.alphamapHQ[num2, num3, m * 4 + 3] = pixel2.a;
						}
					}
					LevelGround.doesLegacyDataIncludeSplatmapWeights = true;
					Object.DestroyImmediate(texture2D3);
				}
			}
			LevelGround.data.baseMapResolution = (int)(size / 8);
			LevelGround.data.baseMapResolution = (int)(size / 4);
			LevelGround._terrain.terrainData = LevelGround.data;
			LevelGround._terrain.terrainData.wavingGrassAmount = 0f;
			LevelGround._terrain.terrainData.wavingGrassSpeed = 1f;
			LevelGround._terrain.terrainData.wavingGrassStrength = 1f;
			LevelGround.loadTrees();
			LevelGround._models2 = new GameObject().transform;
			LevelGround._models2.name = "Ground2";
			LevelGround._models2.parent = Level.level;
			LevelGround._models2.tag = "Ground2";
			LevelGround._models2.gameObject.layer = 31;
			LevelGround._terrain2 = LevelGround._models2.gameObject.AddComponent<Terrain>();
			LevelGround._terrain2.drawInstanced = LevelGround._terrain.drawInstanced;
			LevelGround._terrain2.name = "Ground2";
			LevelGround._terrain2.heightmapPixelError = 200f;
			LevelGround._terrain2.transform.position = new Vector3((float)(-(float)size), 0f, (float)(-(float)size));
			LevelGround._terrain2.reflectionProbeUsage = ReflectionProbeUsage.Simple;
			LevelGround._terrain2.shadowCastingMode = ShadowCastingMode.Off;
			LevelGround._terrain2.drawHeightmap = LevelGround._terrain.drawHeightmap;
			LevelGround._terrain2.drawTreesAndFoliage = false;
			LevelGround._terrain2.treeDistance = 0f;
			LevelGround._terrain2.treeBillboardDistance = 0f;
			LevelGround._terrain2.treeCrossFadeLength = 0f;
			LevelGround._terrain2.treeMaximumFullLODCount = 0;
			LevelGround._data2 = new TerrainData();
			LevelGround.data2.name = "Ground2";
			LevelGround.data2.heightmapResolution = (int)(size / 16);
			LevelGround.data2.alphamapResolution = (int)(size / 8);
			LevelGround.data2.size = new Vector3((float)(size * 2), Level.TERRAIN, (float)(size * 2));
			if (ReadWrite.fileExists(Level.info.path + "/Terrain/Heightmap2.png", false, false))
			{
				byte[] array7 = ReadWrite.readBytes(Level.info.path + "/Terrain/Heightmap2.png", false, false);
				Texture2D texture2D4 = new Texture2D(LevelGround.data2.heightmapResolution, LevelGround.data2.heightmapResolution, TextureFormat.ARGB32, false);
				texture2D4.name = "Heightmap2_Load";
				texture2D4.hideFlags = HideFlags.HideAndDontSave;
				ImageConversion.LoadImage(texture2D4, array7);
				float[,] array8 = new float[texture2D4.width, texture2D4.height];
				for (int num4 = 0; num4 < texture2D4.width; num4++)
				{
					for (int num5 = 0; num5 < texture2D4.height; num5++)
					{
						if (b2 > 0)
						{
							byte[] array9 = new byte[]
							{
								(byte)(texture2D4.GetPixel(num4, num5).r * 255f),
								(byte)(texture2D4.GetPixel(num4, num5).g * 255f),
								(byte)(texture2D4.GetPixel(num4, num5).b * 255f),
								(byte)(texture2D4.GetPixel(num4, num5).a * 255f)
							};
							array8[num4, num5] = BitConverter.ToSingle(array9, 0);
						}
						else
						{
							array8[num4, num5] = texture2D4.GetPixel(num4, num5).r;
						}
					}
				}
				LevelGround.data2.SetHeights(0, 0, array8);
				Object.DestroyImmediate(texture2D4);
			}
			else
			{
				float[,] array10 = new float[LevelGround.data2.heightmapResolution, LevelGround.data2.heightmapResolution];
				for (int num6 = 0; num6 < LevelGround.data2.heightmapResolution; num6++)
				{
					for (int num7 = 0; num7 < LevelGround.data2.heightmapResolution; num7++)
					{
						array10[num6, num7] = 0f;
					}
				}
				LevelGround.data2.SetHeights(0, 0, array10);
			}
			LevelGround.alphamap2HQ = new float[LevelGround.data2.alphamapWidth, LevelGround.data2.alphamapHeight, (int)(LevelGround.ALPHAMAPS * 4)];
			for (int num8 = 0; num8 < (int)LevelGround.ALPHAMAPS; num8++)
			{
				bool flag2 = false;
				if (ReadWrite.fileExists(Level.info.path + "/Terrain/Alphamap2HQ_" + num8.ToString() + ".png", false, false))
				{
					byte[] array11 = ReadWrite.readBytes(Level.info.path + "/Terrain/Alphamap2HQ_" + num8.ToString() + ".png", false, false);
					Texture2D texture2D5 = new Texture2D(LevelGround.data2.heightmapResolution, LevelGround.data2.heightmapResolution, TextureFormat.ARGB32, false);
					texture2D5.name = "Alphamap2HQ_Load";
					texture2D5.hideFlags = HideFlags.HideAndDontSave;
					ImageConversion.LoadImage(texture2D5, array11);
					for (int num9 = 0; num9 < texture2D5.width; num9++)
					{
						for (int num10 = 0; num10 < texture2D5.height; num10++)
						{
							Color pixel3 = texture2D5.GetPixel(num9, num10);
							LevelGround.alphamap2HQ[num9, num10, num8 * 4] = pixel3.r;
							LevelGround.alphamap2HQ[num9, num10, num8 * 4 + 1] = pixel3.g;
							LevelGround.alphamap2HQ[num9, num10, num8 * 4 + 2] = pixel3.b;
							LevelGround.alphamap2HQ[num9, num10, num8 * 4 + 3] = pixel3.a;
						}
					}
					Object.DestroyImmediate(texture2D5);
					flag2 = true;
				}
				if (!flag2 && ReadWrite.fileExists(Level.info.path + "/Terrain/Alphamap2_" + num8.ToString() + ".png", false, false))
				{
					byte[] array12 = ReadWrite.readBytes(Level.info.path + "/Terrain/Alphamap2_" + num8.ToString() + ".png", false, false);
					Texture2D texture2D6 = new Texture2D(LevelGround.data2.heightmapResolution, LevelGround.data2.heightmapResolution, TextureFormat.ARGB32, false);
					texture2D6.name = "Alphamap2_Load";
					texture2D6.hideFlags = HideFlags.HideAndDontSave;
					ImageConversion.LoadImage(texture2D6, array12);
					for (int num11 = 0; num11 < texture2D6.width; num11++)
					{
						for (int num12 = 0; num12 < texture2D6.height; num12++)
						{
							Color pixel4 = texture2D6.GetPixel(num11, num12);
							LevelGround.alphamap2HQ[num11, num12, num8 * 4] = pixel4.r;
							LevelGround.alphamap2HQ[num11, num12, num8 * 4 + 1] = pixel4.g;
							LevelGround.alphamap2HQ[num11, num12, num8 * 4 + 2] = pixel4.b;
							LevelGround.alphamap2HQ[num11, num12, num8 * 4 + 3] = pixel4.a;
						}
					}
					Object.DestroyImmediate(texture2D6);
				}
			}
			LevelGround.data2.baseMapResolution = (int)(size / 8);
			LevelGround.data2.baseMapResolution = (int)(size / 4);
			LevelGround._terrain2.terrainData = LevelGround.data2;
			LevelGround.data2.wavingGrassTint = Color.white;
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x0009CC60 File Offset: 0x0009AE60
		protected static void saveTrees()
		{
			River river = new River(Level.info.path + "/Terrain/Trees.dat", false);
			river.writeByte(6);
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					List<ResourceSpawnpoint> list = LevelGround.trees[(int)b, (int)b2];
					river.writeUInt16((ushort)list.Count);
					ushort num = 0;
					while ((int)num < list.Count)
					{
						ResourceSpawnpoint resourceSpawnpoint = list[(int)num];
						ushort id = resourceSpawnpoint.id;
						if (resourceSpawnpoint != null && resourceSpawnpoint.model != null && (id != 0 || resourceSpawnpoint.guid != Guid.Empty))
						{
							river.writeUInt16(id);
							river.writeGUID(resourceSpawnpoint.guid);
							river.writeSingleVector3(resourceSpawnpoint.point);
							river.writeBoolean(resourceSpawnpoint.isGenerated);
						}
						else
						{
							river.writeUInt16(0);
							river.writeGUID(Guid.Empty);
							river.writeSingleVector3(Vector3.zero);
							river.writeBoolean(true);
						}
						num += 1;
					}
				}
			}
			river.closeRiver();
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x0009CD8B File Offset: 0x0009AF8B
		public static void save()
		{
			if (!Level.info.configData.Use_Legacy_Ground)
			{
				LevelGround.saveTrees();
				return;
			}
			if (LevelGround.hasLegacyDataForConversion)
			{
				File.WriteAllText(LevelGround.GetConversionMarkerFilePath(), "1");
			}
			LevelGround.saveTrees();
		}

		/// <summary>
		/// Game does not currently have a way to resave level's Config.json file, so instead we save a text file
		/// indicating that the terrain auto conversion was performed. If there was a bug with auto conversion then
		/// all of the old files are still present and can be re-converted.
		/// </summary>
		// Token: 0x06002675 RID: 9845 RVA: 0x0009CDBF File Offset: 0x0009AFBF
		private static string GetConversionMarkerFilePath()
		{
			return Path.Combine(Level.info.path, "Terrain", "TerrainWasAutoConverted.txt");
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x0009CDDC File Offset: 0x0009AFDC
		private static void onRegionUpdated(byte old_x, byte old_y, byte new_x, byte new_y)
		{
			bool flag = true;
			LevelGround.onRegionUpdated(null, old_x, old_y, new_x, new_y, 0, ref flag);
		}

		// Token: 0x06002677 RID: 9847 RVA: 0x0009CDF8 File Offset: 0x0009AFF8
		private static void onPlayerTeleported(Player player, Vector3 position)
		{
			LevelGround.shouldInstantlyLoad = true;
		}

		// Token: 0x06002678 RID: 9848 RVA: 0x0009CE00 File Offset: 0x0009B000
		private static void onRegionUpdated(Player player, byte old_x, byte old_y, byte new_x, byte new_y, byte step, ref bool canIncrementIndex)
		{
			if (step != 0)
			{
				return;
			}
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					if (LevelGround.regions[(int)b, (int)b2] && !Regions.checkArea(b, b2, new_x, new_y, LevelGround.RESOURCE_REGIONS))
					{
						LevelGround.regions[(int)b, (int)b2] = false;
						if (LevelGround.shouldInstantlyLoad)
						{
							List<ResourceSpawnpoint> list = LevelGround.trees[(int)b, (int)b2];
							for (int i = 0; i < list.Count; i++)
							{
								list[i].disable();
								if (GraphicsSettings.landmarkQuality >= EGraphicQuality.MEDIUM)
								{
									list[i].enableSkybox();
								}
							}
						}
						else
						{
							LevelGround.loads[(int)b, (int)b2] = 0;
							LevelGround.isRegionalVisibilityDirty = true;
						}
					}
				}
			}
			if (Regions.checkSafe((int)new_x, (int)new_y))
			{
				for (int j = (int)(new_x - LevelGround.RESOURCE_REGIONS); j <= (int)(new_x + LevelGround.RESOURCE_REGIONS); j++)
				{
					for (int k = (int)(new_y - LevelGround.RESOURCE_REGIONS); k <= (int)(new_y + LevelGround.RESOURCE_REGIONS); k++)
					{
						if (Regions.checkSafe((int)((byte)j), (int)((byte)k)) && !LevelGround.regions[j, k])
						{
							LevelGround.regions[j, k] = true;
							if (LevelGround.shouldInstantlyLoad)
							{
								List<ResourceSpawnpoint> list2 = LevelGround.trees[j, k];
								for (int l = 0; l < list2.Count; l++)
								{
									list2[l].enable();
									list2[l].disableSkybox();
								}
							}
							else
							{
								LevelGround.loads[j, k] = 0;
								LevelGround.isRegionalVisibilityDirty = true;
							}
						}
					}
				}
			}
			LevelGround.shouldInstantlyLoad = false;
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x0009CFAC File Offset: 0x0009B1AC
		private static void onPlayerCreated(Player player)
		{
			if (player.channel.IsLocalPlayer)
			{
				Player player2 = Player.player;
				player2.onPlayerTeleported = (PlayerTeleported)Delegate.Combine(player2.onPlayerTeleported, new PlayerTeleported(LevelGround.onPlayerTeleported));
				PlayerMovement movement = Player.player.movement;
				movement.onRegionUpdated = (PlayerRegionUpdated)Delegate.Combine(movement.onRegionUpdated, new PlayerRegionUpdated(LevelGround.onRegionUpdated));
			}
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x0009D017 File Offset: 0x0009B217
		private static void handleEditorAreaRegistered(EditorArea area)
		{
			area.onRegionUpdated = (EditorRegionUpdated)Delegate.Combine(area.onRegionUpdated, new EditorRegionUpdated(LevelGround.onRegionUpdated));
		}

		/// <summary>
		/// Stagger regional visibility across multiple frames.
		/// </summary>
		// Token: 0x0600267B RID: 9851 RVA: 0x0009D03C File Offset: 0x0009B23C
		private void tickRegionalVisibility()
		{
			bool flag = true;
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					if (LevelGround.loads[(int)b, (int)b2] != -1)
					{
						if (LevelGround.loads[(int)b, (int)b2] >= LevelGround.trees[(int)b, (int)b2].Count)
						{
							LevelGround.loads[(int)b, (int)b2] = -1;
						}
						else
						{
							if (LevelGround.regions[(int)b, (int)b2])
							{
								if (!LevelGround.trees[(int)b, (int)b2][LevelGround.loads[(int)b, (int)b2]].isEnabled)
								{
									LevelGround.trees[(int)b, (int)b2][LevelGround.loads[(int)b, (int)b2]].enable();
								}
								if (LevelGround.trees[(int)b, (int)b2][LevelGround.loads[(int)b, (int)b2]].isSkyboxEnabled)
								{
									LevelGround.trees[(int)b, (int)b2][LevelGround.loads[(int)b, (int)b2]].disableSkybox();
								}
							}
							else
							{
								if (LevelGround.trees[(int)b, (int)b2][LevelGround.loads[(int)b, (int)b2]].isEnabled)
								{
									LevelGround.trees[(int)b, (int)b2][LevelGround.loads[(int)b, (int)b2]].disable();
								}
								if (!LevelGround.trees[(int)b, (int)b2][LevelGround.loads[(int)b, (int)b2]].isSkyboxEnabled && GraphicsSettings.landmarkQuality >= EGraphicQuality.MEDIUM)
								{
									LevelGround.trees[(int)b, (int)b2][LevelGround.loads[(int)b, (int)b2]].enableSkybox();
								}
							}
							LevelGround.loads[(int)b, (int)b2]++;
							flag = false;
						}
					}
				}
			}
			if (flag)
			{
				LevelGround.isRegionalVisibilityDirty = false;
			}
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x0009D211 File Offset: 0x0009B411
		private void Update()
		{
			bool isLoaded = Level.isLoaded;
		}

		// Token: 0x0600267D RID: 9853 RVA: 0x0009D21C File Offset: 0x0009B41C
		private void Start()
		{
			Player.onPlayerCreated = (PlayerCreated)Delegate.Combine(Player.onPlayerCreated, new PlayerCreated(LevelGround.onPlayerCreated));
			EditorArea.registered += LevelGround.handleEditorAreaRegistered;
			if (LevelGround._Triplanar_Primary_Size == -1)
			{
				LevelGround._Triplanar_Primary_Size = Shader.PropertyToID("_Triplanar_Primary_Size");
			}
			Shader.SetGlobalFloat(LevelGround._Triplanar_Primary_Size, LevelGround.triplanarPrimarySize);
			if (LevelGround._Triplanar_Primary_Weight == -1)
			{
				LevelGround._Triplanar_Primary_Weight = Shader.PropertyToID("_Triplanar_Primary_Weight");
			}
			Shader.SetGlobalFloat(LevelGround._Triplanar_Primary_Weight, LevelGround.triplanarPrimaryWeight);
			if (LevelGround._Triplanar_Secondary_Size == -1)
			{
				LevelGround._Triplanar_Secondary_Size = Shader.PropertyToID("_Triplanar_Secondary_Size");
			}
			Shader.SetGlobalFloat(LevelGround._Triplanar_Secondary_Size, LevelGround.triplanarSecondarySize);
			if (LevelGround._Triplanar_Secondary_Weight == -1)
			{
				LevelGround._Triplanar_Secondary_Weight = Shader.PropertyToID("_Triplanar_Secondary_Weight");
			}
			Shader.SetGlobalFloat(LevelGround._Triplanar_Secondary_Weight, LevelGround.triplanarSecondaryWeight);
			if (LevelGround._Triplanar_Tertiary_Size == -1)
			{
				LevelGround._Triplanar_Tertiary_Size = Shader.PropertyToID("_Triplanar_Tertiary_Size");
			}
			Shader.SetGlobalFloat(LevelGround._Triplanar_Tertiary_Size, LevelGround.triplanarTertiarySize);
			if (LevelGround._Triplanar_Tertiary_Weight == -1)
			{
				LevelGround._Triplanar_Tertiary_Weight = Shader.PropertyToID("_Triplanar_Tertiary_Weight");
			}
			Shader.SetGlobalFloat(LevelGround._Triplanar_Tertiary_Weight, LevelGround.triplanarTertiaryWeight);
		}

		// Token: 0x0600267E RID: 9854 RVA: 0x0009D340 File Offset: 0x0009B540
		static LevelGround()
		{
			FoliageSystem.preBakeTile += LevelGround.handlePreBakeTile;
			FoliageSystem.postBake += LevelGround.handlePostBake;
		}

		// Token: 0x040013CE RID: 5070
		private static int _Triplanar_Primary_Size = -1;

		// Token: 0x040013CF RID: 5071
		private static float _triplanarPrimarySize = 16f;

		// Token: 0x040013D0 RID: 5072
		private static int _Triplanar_Primary_Weight = -1;

		// Token: 0x040013D1 RID: 5073
		private static float _triplanarPrimaryWeight = 0.4f;

		// Token: 0x040013D2 RID: 5074
		private static int _Triplanar_Secondary_Size = -1;

		// Token: 0x040013D3 RID: 5075
		private static float _triplanarSecondarySize = 64f;

		// Token: 0x040013D4 RID: 5076
		private static int _Triplanar_Secondary_Weight = -1;

		// Token: 0x040013D5 RID: 5077
		private static float _triplanarSecondaryWeight = 0.4f;

		// Token: 0x040013D6 RID: 5078
		private static int _Triplanar_Tertiary_Size = -1;

		// Token: 0x040013D7 RID: 5079
		private static float _triplanarTertiarySize = 4f;

		// Token: 0x040013D8 RID: 5080
		private static int _Triplanar_Tertiary_Weight = -1;

		// Token: 0x040013D9 RID: 5081
		private static float _triplanarTertiaryWeight = 0.2f;

		// Token: 0x040013DA RID: 5082
		private static Collider[] obstructionColliders = new Collider[16];

		// Token: 0x040013DB RID: 5083
		private const byte SAVEDATA_TREES_VERSION_ADDED_GUID = 6;

		// Token: 0x040013DC RID: 5084
		private const byte SAVEDATA_TREES_VERSION_NEWEST = 6;

		// Token: 0x040013DD RID: 5085
		public static readonly byte SAVEDATA_TREES_VERSION = 6;

		// Token: 0x040013DE RID: 5086
		public static readonly byte RESOURCE_REGIONS = 3;

		// Token: 0x040013DF RID: 5087
		public static readonly byte ALPHAMAPS = 2;

		// Token: 0x040013E0 RID: 5088
		private static float[,,] alphamapHQ;

		// Token: 0x040013E1 RID: 5089
		private static float[,,] alphamap2HQ;

		/// <summary>
		/// If true then level should convert old terrain.
		/// </summary>
		// Token: 0x040013E2 RID: 5090
		public static bool hasLegacyDataForConversion;

		/// <summary>
		/// If true, splatmap conversion should use weights as-is.
		/// </summary>
		// Token: 0x040013E3 RID: 5091
		public static bool doesLegacyDataIncludeSplatmapWeights;

		/// <summary>
		/// Material guids converted by legacy asset bundle hash or texture names.
		/// </summary>
		// Token: 0x040013E4 RID: 5092
		public static AssetReference<LandscapeMaterialAsset>[] legacyMaterialGuids;

		// Token: 0x040013E6 RID: 5094
		private static Transform _models;

		// Token: 0x040013E7 RID: 5095
		private static Transform _models2;

		// Token: 0x040013E8 RID: 5096
		private static List<ResourceSpawnpoint>[,] _trees;

		// Token: 0x040013E9 RID: 5097
		private static int _total;

		// Token: 0x040013EA RID: 5098
		private static bool[,] _regions;

		// Token: 0x040013EB RID: 5099
		private static int[,] loads;

		// Token: 0x040013ED RID: 5101
		private static bool isRegionalVisibilityDirty = true;

		// Token: 0x040013EE RID: 5102
		private static Terrain _terrain;

		// Token: 0x040013EF RID: 5103
		private static Terrain _terrain2;

		// Token: 0x040013F0 RID: 5104
		private static TerrainData _data;

		// Token: 0x040013F1 RID: 5105
		private static TerrainData _data2;
	}
}
